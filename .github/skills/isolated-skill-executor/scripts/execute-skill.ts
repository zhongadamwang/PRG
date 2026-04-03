#!/usr/bin/env bun

// @ts-ignore
import { cpSync, existsSync, mkdirSync, readFileSync, readdirSync, rmSync, statSync, writeFileSync, } from "node:fs";
// @ts-ignore
import { spawnSync } from "node:child_process";
// @ts-ignore
import { tmpdir } from "node:os";
// @ts-ignore
import { basename, dirname, isAbsolute, join, resolve } from "node:path";

type JsonValue = null | boolean | number | string | JsonValue[] | { [key: string]: JsonValue };
type JsonObject = { [key: string]: JsonValue };

type SkillError<TDetails = JsonValue> = {
	code: string;
	message: string;
	retryable: boolean;
	details?: TDetails;
};

type SkillResult<TValue = JsonValue, TDetails = JsonValue> =
	| {
		schemaVersion: "skill-result/v1";
		invocationId: string;
		skillId: string;
		finishedAt: string;
		durationMs: number;
		ok: true;
		value: TValue | null;
		error: null;
	}
	| {
		schemaVersion: "skill-result/v1";
		invocationId: string;
		skillId: string;
		finishedAt: string;
		durationMs: number;
		ok: false;
		value: null;
		error: SkillError<TDetails>;
	};

type SkillManifest = {
	schemaVersion?: string;
	skillId?: string;
	author?: string;
	skillName?: string;
	name?: string;
	version?: string;
	description?: string;
	entry?: {
		kind?: string;
		script?: string;
		runtime?: string;
	};
};

type ContractSchema = JsonObject;

type SkillContract = {
	input?: ContractSchema;
	success?: ContractSchema;
	errorDetails?: ContractSchema;
};

type ExecutorInput = {
	targetSkillId?: string;
	targetSkillPath?: string;
	targetSkillVersion?: string;
	targetInvocationId?: string;
	targetInput?: JsonValue;
	inputSources?: string[];
	isolationMode?: "isolated-session";
};

type InvocationEnvelope = {
	schemaVersion?: string;
	invocationId?: string;
	skillId?: string;
	skillVersion?: string;
	timestamp?: string;
	caller?: JsonObject | null;
	run?: JsonObject | null;
	input?: JsonValue;
	context?: JsonObject | null;
};

type ResolvedTargetSkill = {
	manifestPath: string;
	skillDir: string;
	manifest: Required<Pick<SkillManifest, "skillId" | "version">> & SkillManifest;
};

const SELF_SKILL_ID = "sanjel-isolated-skill-executor-v1-0-0";
const SELF_SKILL_VERSION = "1.0.0";
// @ts-ignore
const SCRIPT_DIR = import.meta.dirname;
const REPO_ROOT = resolve(SCRIPT_DIR, "../../../../");
const SKILLS_ROOT = join(REPO_ROOT, ".github", "skills");
const RUNS_ROOT = join(REPO_ROOT, ".skill-runs");
const SESSION_ROOT = join(tmpdir(), "prg-skill-sessions");

function nowIso(): string {
	return new Date().toISOString();
}

function ensureDirectory(dirPath: string): void {
	if (!existsSync(dirPath)) {
		mkdirSync(dirPath, { recursive: true });
	}
}

function readJsonFile<T>(filePath: string): T {
	return JSON.parse(readFileSync(filePath, "utf-8")) as T;
}

function writeJsonFile(filePath: string, value: unknown): void {
	ensureDirectory(dirname(filePath));
	writeFileSync(filePath, `${JSON.stringify(value, null, "\t")}\n`, "utf-8");
}

function isObject(value: unknown): value is Record<string, unknown> {
	return typeof value === "object" && value !== null && !Array.isArray(value);
}

function asString(value: unknown): string | undefined {
	return typeof value === "string" && value.trim() !== "" ? value.trim() : undefined;
}

function sanitizeSegment(value: string): string {
	const normalized = value.toLowerCase().replace(/[^a-z0-9._-]+/g, "-").replace(/-+/g, "-").replace(/^-|-$/g, "");
	return normalized || "skill";
}

function padStep(order: number): string {
	return order.toString().padStart(2, "0");
}

function makeResult<TValue = JsonValue, TDetails = JsonValue>(
	invocationId: string,
	ok: boolean,
	startedAt: number,
	value: TValue | null,
	error: SkillError<TDetails> | null,
): SkillResult<TValue, TDetails> {
	return {
		schemaVersion: "skill-result/v1",
		invocationId,
		skillId: SELF_SKILL_ID,
		finishedAt: nowIso(),
		durationMs: Date.now() - startedAt,
		ok,
		value: ok ? value : null,
		error: ok ? null : (error as SkillError<TDetails>),
	} as SkillResult<TValue, TDetails>;
}

function collectSkillManifestPaths(dirPath: string, accumulator: string[]): void {
	for (const entry of readdirSync(dirPath, { withFileTypes: true })) {
		if (entry.name.startsWith(".")) {
			continue;
		}

		const entryPath = join(dirPath, entry.name);
		if (entry.isDirectory()) {
			collectSkillManifestPaths(entryPath, accumulator);
			continue;
		}

		if (entry.isFile() && entry.name === "skill.json") {
			accumulator.push(entryPath);
		}
	}
}

function findSkillById(skillId: string): ResolvedTargetSkill | undefined {
	const manifestPaths: string[] = [];
	collectSkillManifestPaths(SKILLS_ROOT, manifestPaths);

	for (const manifestPath of manifestPaths) {
		const manifest = readJsonFile<SkillManifest>(manifestPath);
		if (manifest.skillId === skillId) {
			if (!manifest.skillId || !manifest.version) {
				continue;
			}

			return {
				manifestPath,
				skillDir: dirname(manifestPath),
				manifest: manifest as ResolvedTargetSkill["manifest"],
			};
		}
	}

	return undefined;
}

function resolveSkillFromPath(targetSkillPath: string): ResolvedTargetSkill {
	const candidatePath = isAbsolute(targetSkillPath) ? targetSkillPath : resolve(REPO_ROOT, targetSkillPath);
	if (!existsSync(candidatePath)) {
		throw new Error(`Target skill path not found: ${candidatePath}`);
	}

	const manifestPath = statSync(candidatePath).isDirectory() ? join(candidatePath, "skill.json") : candidatePath;

	if (!existsSync(manifestPath)) {
		throw new Error(`Target skill manifest not found: ${manifestPath}`);
	}

	const manifest = readJsonFile<SkillManifest>(manifestPath);
	if (!manifest.skillId || !manifest.version) {
		throw new Error(`Target skill manifest is missing required fields: ${manifestPath}`);
	}

	return {
		manifestPath,
		skillDir: dirname(manifestPath),
		manifest: manifest as ResolvedTargetSkill["manifest"],
	};
}

function resolveTargetSkill(input: ExecutorInput): ResolvedTargetSkill {
	if (input.targetSkillPath) {
		return resolveSkillFromPath(input.targetSkillPath);
	}

	if (!input.targetSkillId) {
		throw new Error("Executor input must provide targetSkillId or targetSkillPath.");
	}

	const resolved = findSkillById(input.targetSkillId);
	if (!resolved) {
		throw new Error(`Target skill not found for skillId: ${input.targetSkillId}`);
	}

	return resolved;
}

function loadContract(skillDir: string): SkillContract {
	const signaturePath = join(skillDir, "signature.json");
	if (existsSync(signaturePath)) {
		const signature = readJsonFile<JsonObject>(signaturePath);
		return {
			input: isObject(signature.input) ? (signature.input as ContractSchema) : undefined,
			success: isObject(signature.success) ? (signature.success as ContractSchema) : undefined,
			errorDetails: isObject(signature.errorDetails) ? (signature.errorDetails as ContractSchema) : undefined,
		};
	}

	const inputSchemaPath = join(skillDir, "input.schema.json");
	const outputSchemaPath = join(skillDir, "output.schema.json");
	return {
		input: existsSync(inputSchemaPath) ? readJsonFile<ContractSchema>(inputSchemaPath) : undefined,
		success: existsSync(outputSchemaPath) ? readJsonFile<ContractSchema>(outputSchemaPath) : undefined,
	};
}

function validateAgainstSchema(value: unknown, schema: ContractSchema | undefined, pathLabel = "$"): string[] {
	if (!schema) {
		return [];
	}

	if (Array.isArray(schema.oneOf)) {
		const variants = schema.oneOf as ContractSchema[];
		const success = variants.some((variant) => validateAgainstSchema(value, variant, pathLabel).length === 0);
		if (!success) {
			return [`${pathLabel} does not match any allowed schema variant.`];
		}
	}

	const errors: string[] = [];
	const typeValue = schema.type;

	if (typeof schema.const !== "undefined" && value !== schema.const) {
		errors.push(`${pathLabel} must equal ${JSON.stringify(schema.const)}.`);
	}

	if (Array.isArray(schema.enum) && !schema.enum.includes(value as never)) {
		errors.push(`${pathLabel} must be one of ${schema.enum.map((entry) => JSON.stringify(entry)).join(", ")}.`);
	}

	if (typeof typeValue === "string") {
		switch (typeValue) {
			case "string":
				if (typeof value !== "string") {
					errors.push(`${pathLabel} must be a string.`);
				}
				break;
			case "number":
				if (typeof value !== "number" || Number.isNaN(value)) {
					errors.push(`${pathLabel} must be a number.`);
				}
				break;
			case "integer":
				if (typeof value !== "number" || !Number.isInteger(value)) {
					errors.push(`${pathLabel} must be an integer.`);
				}
				break;
			case "boolean":
				if (typeof value !== "boolean") {
					errors.push(`${pathLabel} must be a boolean.`);
				}
				break;
			case "null":
				if (value !== null) {
					errors.push(`${pathLabel} must be null.`);
				}
				break;
			case "array":
				if (!Array.isArray(value)) {
					errors.push(`${pathLabel} must be an array.`);
				} else {
					if (typeof schema.minItems === "number" && value.length < schema.minItems) {
						errors.push(`${pathLabel} must contain at least ${schema.minItems} item(s).`);
					}

					if (isObject(schema.items)) {
						value.forEach((item, index) => {
							errors.push(...validateAgainstSchema(item, schema.items as ContractSchema, `${pathLabel}[${index}]`));
						});
					}
				}
				break;
			case "object":
				if (!isObject(value)) {
					errors.push(`${pathLabel} must be an object.`);
				} else {
					const required = Array.isArray(schema.required) ? schema.required : [];
					for (const key of required) {
						if (!(key as string in value)) {
							errors.push(`${pathLabel}.${String(key)} is required.`);
						}
					}

					const properties = isObject(schema.properties) ? (schema.properties as Record<string, ContractSchema>) : {};
					for (const [key, propertySchema] of Object.entries(properties)) {
						if (key in value) {
							errors.push(...validateAgainstSchema(value[key], propertySchema, `${pathLabel}.${key}`));
						}
					}

					if (schema.additionalProperties === false) {
						for (const key of Object.keys(value)) {
							if (!(key in properties)) {
								errors.push(`${pathLabel}.${key} is not allowed.`);
							}
						}
					}
				}
				break;
			default:
				break;
		}
	}

	if (typeof schema.minimum === "number" && typeof value === "number" && value < schema.minimum) {
		errors.push(`${pathLabel} must be >= ${schema.minimum}.`);
	}

	return errors;
}

function validateResultEnvelope(result: unknown): string[] {
	if (!isObject(result)) {
		return ["result must be an object."];
	}

	const errors: string[] = [];
	if (result.schemaVersion !== "skill-result/v1") {
		errors.push("result.schemaVersion must equal \"skill-result/v1\".");
	}

	if (typeof result.invocationId !== "string" || result.invocationId.trim() === "") {
		errors.push("result.invocationId must be a non-empty string.");
	}

	if (typeof result.skillId !== "string" || result.skillId.trim() === "") {
		errors.push("result.skillId must be a non-empty string.");
	}

	if (typeof result.ok !== "boolean") {
		errors.push("result.ok must be a boolean.");
		return errors;
	}

	if (result.ok) {
		if (result.error !== null) {
			errors.push("result.error must be null when result.ok is true.");
		}
	} else {
		if (!isObject(result.error)) {
			errors.push("result.error must be an object when result.ok is false.");
		} else {
			if (typeof result.error.code !== "string" || result.error.code.trim() === "") {
				errors.push("result.error.code must be a non-empty string.");
			}
			if (typeof result.error.message !== "string" || result.error.message.trim() === "") {
				errors.push("result.error.message must be a non-empty string.");
			}
			if (typeof result.error.retryable !== "boolean") {
				errors.push("result.error.retryable must be a boolean.");
			}
		}

		if (result.value !== null) {
			errors.push("result.value must be null when result.ok is false.");
		}
	}

	return errors;
}

function nextStepOrder(runDir: string): number {
	if (!existsSync(runDir)) {
		return 0;
	}

	let maxOrder = -1;
	for (const entry of readdirSync(runDir, { withFileTypes: true })) {
		if (!entry.isDirectory()) {
			continue;
		}

		const match = entry.name.match(/^(\d+)-/);
		if (match) {
			maxOrder = Math.max(maxOrder, Number.parseInt(match[1], 10));
		}
	}

	return maxOrder + 1;
}

function makeExecutorFailure(
	invocationId: string,
	startedAt: number,
	code: string,
	message: string,
	details?: JsonValue,
	retryable = false,
): SkillResult<JsonObject, JsonValue> {
	return makeResult(invocationId, false, startedAt, null, {
		code,
		message,
		retryable,
		details,
	});
}

function normalizeExecutorInput(invocation: InvocationEnvelope): { input: ExecutorInput; errors: string[] } {
	const errors: string[] = [];
	if (typeof invocation.invocationId !== "string" || invocation.invocationId.trim() === "") {
		errors.push("invocationId must be a non-empty string.");
	}

	if (!isObject(invocation.input)) {
		errors.push("input must be an object.");
		return { input: {}, errors };
	}

	const input = invocation.input as Record<string, unknown>;
	const normalized: ExecutorInput = {
		targetSkillId: asString(input.targetSkillId),
		targetSkillPath: asString(input.targetSkillPath),
		targetSkillVersion: asString(input.targetSkillVersion),
		targetInvocationId: asString(input.targetInvocationId),
		targetInput: (input.targetInput as JsonValue | undefined) ?? null,
		inputSources: Array.isArray(input.inputSources)
			? input.inputSources.filter((item): item is string => typeof item === "string")
			: undefined,
		isolationMode: input.isolationMode === "isolated-session" ? "isolated-session" : "isolated-session",
	};

	if (!normalized.targetSkillId && !normalized.targetSkillPath) {
		errors.push("input must include targetSkillId or targetSkillPath.");
	}

	return { input: normalized, errors };
}

function buildRunSummary(
	runDir: string,
	executorInvocationId: string,
	targetStepFolder: string,
	targetSkillId: string,
	targetInvocationId: string,
	status: string,
	sessionId: string,
	startedAtIso: string,
): JsonObject {
	const stepMatch = targetStepFolder.match(/^(\d+)-/);
	const stepOrder = stepMatch ? Number.parseInt(stepMatch[1], 10) : 1;

	return {
		schemaVersion: "skill-run/v1",
		rootInvocationId: basename(runDir),
		rootSkillId: SELF_SKILL_ID,
		startedAt: startedAtIso,
		status,
		steps: [
			{
				order: 0,
				folder: "00-root",
				skillId: SELF_SKILL_ID,
				invocationId: executorInvocationId,
				status,
			},
			{
				order: stepOrder,
				folder: targetStepFolder,
				skillId: targetSkillId,
				invocationId: targetInvocationId,
				status,
				sessionId,
			},
		],
	};
}

function main(): number {
	// @ts-ignore
	const args = process.argv.slice(2);
	if (args.length !== 1 || args[0] === "--help" || args[0] === "-h") {
		console.log("Usage: bun run .github/skills/isolated-skill-executor/scripts/execute-skill.ts <executor-invocation.json>");
		return args.length === 1 ? 0 : 1;
	}

	const executorInvocationPath = resolve(args[0]);
	if (!existsSync(executorInvocationPath)) {
		console.error(`Executor invocation not found: ${executorInvocationPath}`);
		return 1;
	}

	const startedAt = Date.now();
	const startedAtIso = nowIso();
	const executorInvocation = readJsonFile<InvocationEnvelope>(executorInvocationPath);
	const invocationId = asString(executorInvocation.invocationId) ?? "invalid-executor-invocation";
	const rootRunDir = join(RUNS_ROOT, invocationId);
	const rootStepDir = join(rootRunDir, "00-root");
	ensureDirectory(rootStepDir);
	writeJsonFile(join(rootStepDir, "invocation.json"), executorInvocation);

	const { input: executorInput, errors: inputErrors } = normalizeExecutorInput(executorInvocation);
	if (inputErrors.length > 0) {
		const result = makeExecutorFailure(invocationId, startedAt, "INVALID_EXECUTOR_INPUT", "Executor invocation input is invalid.", inputErrors);
		writeJsonFile(join(rootStepDir, "result.json"), result);
		console.log(JSON.stringify(result, null, "\t"));
		return 0;
	}

	let targetSkill: ResolvedTargetSkill;
	try {
		targetSkill = resolveTargetSkill(executorInput);
	} catch (error) {
		const result = makeExecutorFailure(
			invocationId,
			startedAt,
			"TARGET_SKILL_NOT_FOUND",
			error instanceof Error ? error.message : String(error),
			{ targetSkillId: executorInput.targetSkillId ?? null, targetSkillPath: executorInput.targetSkillPath ?? null },
		);
		writeJsonFile(join(rootStepDir, "result.json"), result);
		console.log(JSON.stringify(result, null, "\t"));
		return 0;
	}

	if (targetSkill.manifest.skillId === SELF_SKILL_ID) {
		const result = makeExecutorFailure(
			invocationId,
			startedAt,
			"SELF_EXECUTION_FORBIDDEN",
			"The isolated skill executor cannot execute itself as a target skill.",
			{ targetSkillId: targetSkill.manifest.skillId },
		);
		writeJsonFile(join(rootStepDir, "result.json"), result);
		console.log(JSON.stringify(result, null, "\t"));
		return 0;
	}

	if (executorInput.targetSkillVersion && targetSkill.manifest.version !== executorInput.targetSkillVersion) {
		const result = makeExecutorFailure(
			invocationId,
			startedAt,
			"TARGET_VERSION_MISMATCH",
			"Target skill version does not match the requested targetSkillVersion.",
			{
				targetSkillId: targetSkill.manifest.skillId,
				targetSkillVersion: targetSkill.manifest.version,
				requestedVersion: executorInput.targetSkillVersion,
			},
		);
		writeJsonFile(join(rootStepDir, "result.json"), result);
		console.log(JSON.stringify(result, null, "\t"));
		return 0;
	}

	const entry = targetSkill.manifest.entry ?? {};
	if (entry.kind !== "bun-script" || !entry.script) {
		const result = makeExecutorFailure(
			invocationId,
			startedAt,
			"UNSUPPORTED_ENTRY_KIND",
			"Target skill is not script-executable through Bun.",
			{
				targetSkillId: targetSkill.manifest.skillId,
				entryKind: entry.kind ?? null,
			},
		);
		writeJsonFile(join(rootStepDir, "result.json"), result);
		console.log(JSON.stringify(result, null, "\t"));
		return 0;
	}

	const contract = loadContract(targetSkill.skillDir);
	const targetInputErrors = validateAgainstSchema(executorInput.targetInput, contract.input, "$.targetInput");
	if (targetInputErrors.length > 0) {
		const result = makeExecutorFailure(
			invocationId,
			startedAt,
			"TARGET_INPUT_INVALID",
			"Target input does not satisfy the target skill input schema.",
			{
				targetSkillId: targetSkill.manifest.skillId,
				validationErrors: targetInputErrors,
			},
		);
		writeJsonFile(join(rootStepDir, "result.json"), result);
		console.log(JSON.stringify(result, null, "\t"));
		return 0;
	}

	const childOrder = Math.max(1, nextStepOrder(rootRunDir));
	const stepFolder = `${padStep(childOrder)}-${sanitizeSegment(targetSkill.manifest.skillId)}`;
	const stepDir = join(rootRunDir, stepFolder);
	ensureDirectory(stepDir);

	const childInvocationId = executorInput.targetInvocationId ?? `${invocationId}__${padStep(childOrder)}`;
	const childInvocation: InvocationEnvelope = {
		schemaVersion: "skill-invocation/v1",
		invocationId: childInvocationId,
		skillId: targetSkill.manifest.skillId,
		skillVersion: targetSkill.manifest.version,
		timestamp: nowIso(),
		caller: {
			kind: "skill",
			skillId: SELF_SKILL_ID,
			invocationId,
		},
		run: {
			rootInvocationId: invocationId,
			stepOrder: childOrder,
			stepFolder,
			parentInvocationId: invocationId,
			recursionDepth: 0,
		},
		input: executorInput.targetInput ?? null,
		context: {
			traceId: `trace_${invocationId}`,
			inputSources: executorInput.inputSources ?? [],
			isolationMode: executorInput.isolationMode ?? "isolated-session",
		},
	};

	const childInvocationPath = join(stepDir, "invocation.json");
	const childResultPath = join(stepDir, "result.json");
	const childTracePath = join(stepDir, "trace.json");
	writeJsonFile(childInvocationPath, childInvocation);

	const sessionId = `session_${sanitizeSegment(childInvocationId)}`;
	const sessionDir = join(SESSION_ROOT, sessionId);
	const sessionSkillDir = join(sessionDir, "skill");
	ensureDirectory(sessionDir);
	cpSync(targetSkill.skillDir, sessionSkillDir, { recursive: true });

	const sessionScriptPath = join(sessionSkillDir, entry.script);
	const childProcess = spawnSync(
		"bun",
		["run", sessionScriptPath, childInvocationPath],
		{
			cwd: sessionDir,
			encoding: "utf-8",
			env: {
				// @ts-ignore
				PATH: process.env.PATH ?? "",
				// @ts-ignore
				HOME: process.env.HOME ?? "",
				// @ts-ignore
				TMPDIR: process.env.TMPDIR ?? tmpdir(),
				SKILL_ISOLATION_MODE: "isolated-session",
				SKILL_SESSION_ID: sessionId,
				SKILL_SESSION_DIR: sessionDir,
				SKILL_INVOCATION_PATH: childInvocationPath,
				SKILL_RESULT_PATH: childResultPath,
				SKILL_TRACE_PATH: childTracePath,
				SKILL_STEP_DIR: stepDir,
			},
		},
	);

	const tracePayload: JsonObject = {
		schemaVersion: "skill-trace/v1",
		traceId: `trace_${invocationId}`,
		rootInvocationId: invocationId,
		currentInvocationId: invocationId,
		childInvocations: [
			{
				skillId: targetSkill.manifest.skillId,
				invocationId: childInvocationId,
				stepOrder: childOrder,
				stepFolder,
				status: childProcess.status === 0 ? "completed" : "failed",
			},
		],
		execution: {
			sessionId,
			sessionDir,
			stdout: childProcess.stdout ?? "",
			stderr: childProcess.stderr ?? "",
			exitCode: childProcess.status ?? null,
		},
	};
	writeJsonFile(join(rootStepDir, "trace.json"), tracePayload);

	if (!existsSync(childTracePath)) {
		writeJsonFile(childTracePath, {
			schemaVersion: "skill-trace/v1",
			traceId: `trace_${childInvocationId}`,
			rootInvocationId: invocationId,
			currentInvocationId: childInvocationId,
			execution: {
				sessionId,
				stdout: childProcess.stdout ?? "",
				stderr: childProcess.stderr ?? "",
				exitCode: childProcess.status ?? null,
			},
		});
	}

	let finalResult: SkillResult<JsonObject, JsonValue>;

	if (childProcess.status !== 0) {
		finalResult = makeExecutorFailure(
			invocationId,
			startedAt,
			"TARGET_PROCESS_FAILED",
			"Target skill process exited with a non-zero status.",
			{
				targetSkillId: targetSkill.manifest.skillId,
				targetInvocationId: childInvocationId,
				stderr: childProcess.stderr ?? "",
				resultPath: childResultPath,
			},
		);
	} else if (!existsSync(childResultPath)) {
		finalResult = makeExecutorFailure(
			invocationId,
			startedAt,
			"TARGET_RESULT_MISSING",
			"Target skill completed without writing result.json.",
			{
				targetSkillId: targetSkill.manifest.skillId,
				targetInvocationId: childInvocationId,
				resultPath: childResultPath,
			},
		);
	} else {
		const childResult = readJsonFile<JsonObject>(childResultPath);
		const envelopeErrors = validateResultEnvelope(childResult);
		const payloadErrors =
			isObject(childResult) && childResult.ok === true
				? validateAgainstSchema(childResult.value, contract.success, "$.value")
				: isObject(childResult) && isObject(childResult.error)
					? validateAgainstSchema(childResult.error.details, contract.errorDetails, "$.error.details")
					: [];

		if (envelopeErrors.length > 0 || payloadErrors.length > 0) {
			finalResult = makeExecutorFailure(
				invocationId,
				startedAt,
				"TARGET_RESULT_INVALID",
				"Target skill wrote a result that does not satisfy the declared contract.",
				{
					targetSkillId: targetSkill.manifest.skillId,
					targetInvocationId: childInvocationId,
					resultPath: childResultPath,
					validationErrors: [...envelopeErrors, ...payloadErrors],
				},
			);
		} else if (childResult.ok === false) {
			finalResult = makeExecutorFailure(
				invocationId,
				startedAt,
				"TARGET_SKILL_FAILED",
				"Target skill returned a failure result.",
				{
					targetSkillId: targetSkill.manifest.skillId,
					targetInvocationId: childInvocationId,
					resultPath: childResultPath,
					childError: childResult.error ?? null,
				},
				isObject(childResult.error) && typeof childResult.error.retryable === "boolean"
					? childResult.error.retryable
					: false,
			);
		} else {
			finalResult = makeResult(invocationId, true, startedAt, {
				rootInvocationId: invocationId,
				targetInvocationId: childInvocationId,
				targetSkillId: targetSkill.manifest.skillId,
				stepFolder,
				resultPath: childResultPath,
				tracePath: childTracePath,
				sessionClosed: true,
			}, null);
		}
	}

	writeJsonFile(join(rootStepDir, "result.json"), finalResult);
	writeJsonFile(join(rootRunDir, "run.json"), buildRunSummary(
		rootRunDir,
		invocationId,
		stepFolder,
		targetSkill.manifest.skillId,
		childInvocationId,
		finalResult.ok ? "completed" : "failed",
		sessionId,
		startedAtIso,
	));

	rmSync(sessionDir, { recursive: true, force: true });

	console.log(JSON.stringify(finalResult, null, "\t"));
	return 0;
}

// @ts-ignore
process.exit(main());
