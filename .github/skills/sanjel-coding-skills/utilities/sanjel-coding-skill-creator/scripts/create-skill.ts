#!/usr/bin/env bun

// @ts-ignore
import * as fs from "node:fs";
// @ts-ignore
import * as path from "node:path";

type SkillType = "Orchestrator" | "Task";
type ContractSchema = Record<string, unknown>;

interface SkillInput {
	name: string;
	type: SkillType;
	actor: string;
	AIDriven: boolean;
	usesTemplates: boolean;
	responsibility: string;
	flowSummary: string;
	inputContract: ContractSchema;
	outputContract: ContractSchema;
}

interface GeneratedFields {
	description: string;
	overview: string;
}

interface SkillPayload extends SkillInput, GeneratedFields { }

interface ScaffoldResult {
	skillDirectory: string;
	createdPaths: string[];
	inputContractPath: string;
	outputContractPath: string;
	executionMode: "coordination-only" | "AI-driven" | "script-driven";
}

// @ts-ignore
const TEMPLATES_DIR = path.resolve(__dirname, "../templates");
// @ts-ignore
const SKILLS_ROOT = path.resolve(__dirname, "../../");

function renderTemplate(templateFile: string, vars: Record<string, string>): string {
	let content = fs.readFileSync(path.join(TEMPLATES_DIR, templateFile), "utf-8");
	for (const [key, value] of Object.entries(vars)) {
		content = content.replaceAll(`{{${key}}}`, value);
	}
	return content;
}

function writeJsonFile(filePath: string, value: unknown): void {
	fs.writeFileSync(filePath, `${JSON.stringify(value, null, "\t")}\n`);
}

function assertString(value: unknown, fieldName: string): string {
	if (typeof value !== "string" || value.trim() === "") {
		throw new Error(`${fieldName} must be a non-empty string.`);
	}

	return value.trim();
}

function toBoolean(value: unknown, defaultValue = false): boolean {
	if (typeof value === "boolean") {
		return value;
	}

	if (typeof value === "string") {
		return value === "true";
	}

	return defaultValue;
}

function parseJsonObject(raw: string, fieldName: string): ContractSchema {
	const parsed = JSON.parse(raw) as unknown;
	if (!parsed || Array.isArray(parsed) || typeof parsed !== "object") {
		throw new Error(`${fieldName} must be a JSON object.`);
	}

	return parsed as ContractSchema;
}

function coerceContract(
	objectValue: unknown,
	jsonValue: unknown,
	fieldName: string,
): ContractSchema {
	if (objectValue && !Array.isArray(objectValue) && typeof objectValue === "object") {
		return objectValue as ContractSchema;
	}

	if (typeof jsonValue === "string" && jsonValue.trim() !== "") {
		return parseJsonObject(jsonValue, fieldName);
	}

	return {};
}

function normalizePayload(raw: Record<string, unknown>): SkillPayload {
	const type = assertString(raw.type, "type") as SkillType;
	if (type !== "Orchestrator" && type !== "Task") {
		throw new Error("type must be either 'Orchestrator' or 'Task'.");
	}

	const payload: SkillPayload = {
		name: assertString(raw.name, "name"),
		type,
		actor: assertString(raw.actor, "actor"),
		AIDriven: type === "Task" ? toBoolean(raw.AIDriven, true) : true,
		usesTemplates: type === "Task" ? toBoolean(raw.usesTemplates, false) : false,
		responsibility: assertString(raw.responsibility, "responsibility"),
		flowSummary: typeof raw.flowSummary === "string" ? raw.flowSummary.trim() : "",
		inputContract: coerceContract(raw.inputContract, raw.inputContractJson, "inputContract"),
		outputContract: coerceContract(raw.outputContract, raw.outputContractJson, "outputContract"),
		description: assertString(raw.description, "description"),
		overview: assertString(raw.overview, "overview"),
	};

	return payload;
}

function parseSingleArgumentPayload(arg: string): SkillPayload {
	const possibleFilePath = path.resolve(arg);
	const rawPayload = fs.existsSync(possibleFilePath)
		? fs.readFileSync(possibleFilePath, "utf-8")
		: arg;

	return normalizePayload(JSON.parse(rawPayload) as Record<string, unknown>);
}

function parsePositionalPayload(args: string[]): SkillPayload {
	if (args.length >= 11) {
		const [
			name,
			type,
			actor,
			aiDriven,
			usesTemplates,
			description,
			overview,
			responsibility,
			flowSummary,
			inputContractJson,
			outputContractJson,
		] = args;

		return normalizePayload({
			name,
			type,
			actor,
			AIDriven: aiDriven,
			usesTemplates,
			description,
			overview,
			responsibility,
			flowSummary,
			inputContractJson,
			outputContractJson,
		});
	}

	if (args.length >= 6) {
		const [name, type, actor, aiDriven, description, overview] = args;
		return normalizePayload({
			name,
			type,
			actor,
			AIDriven: aiDriven,
			usesTemplates: false,
			description,
			overview,
			responsibility: overview,
			flowSummary: "",
			inputContract: {},
			outputContract: {},
		});
	}

	throw new Error(
		"Usage:\n  bun create-skill.ts '<json-payload>'\n  bun create-skill.ts <name> <type> <actor> <AIDriven> <usesTemplates> <description> <overview> <responsibility> <flowSummary> <inputContractJson> <outputContractJson>",
	);
}

function getExecutionMode(input: SkillInput): ScaffoldResult["executionMode"] {
	if (input.type === "Orchestrator") {
		return "coordination-only";
	}

	return input.AIDriven ? "AI-driven" : "script-driven";
}

function scaffoldSkill(input: SkillInput, generated: GeneratedFields): ScaffoldResult {
	const skillDir = path.join(SKILLS_ROOT, input.name);
	if (fs.existsSync(skillDir) && fs.readdirSync(skillDir).length > 0) {
		throw new Error(`Target skill directory already exists and is not empty: ${skillDir}`);
	}

	fs.mkdirSync(skillDir, { recursive: true });

	const executionMode = getExecutionMode(input);
	const usesScripts = input.type === "Task" && !input.AIDriven;

	const vars: Record<string, string> = {
		name: input.name,
		description: generated.description,
		overview: generated.overview,
		type: input.type,
		actor: input.actor,
		responsibility: input.responsibility,
		flowSummary: input.flowSummary,
		executionMode,
		usesScripts: String(usesScripts),
		usesTemplates: String(input.usesTemplates),
	};

	const skillTemplate =
		input.type === "Orchestrator" ? "SKILL.orchestrator.md.template" : "SKILL.md.template";
	const skillPath = path.join(skillDir, "SKILL.md");
	const inputContractPath = path.join(skillDir, "in.json");
	const outputContractPath = path.join(skillDir, "out.json");

	fs.writeFileSync(skillPath, renderTemplate(skillTemplate, vars));
	writeJsonFile(inputContractPath, input.inputContract);
	writeJsonFile(outputContractPath, input.outputContract);

	const createdPaths = ["SKILL.md", "in.json", "out.json"];

	if (usesScripts) {
		fs.mkdirSync(path.join(skillDir, "scripts"), { recursive: true });
		createdPaths.push("scripts/");
	}

	if (input.type === "Task" && input.usesTemplates) {
		fs.mkdirSync(path.join(skillDir, "templates"), { recursive: true });
		createdPaths.push("templates/");
	}

	return {
		skillDirectory: skillDir,
		createdPaths,
		inputContractPath,
		outputContractPath,
		executionMode,
	};
}

try {
	// @ts-ignore
	const args = process.argv.slice(2) as string[];
	const payload = args.length === 1 ? parseSingleArgumentPayload(args[0]) : parsePositionalPayload(args);
	const result = scaffoldSkill(payload, {
		description: payload.description,
		overview: payload.overview,
	});
	console.log(JSON.stringify(result, null, "\t"));
} catch (error) {
	const message = error instanceof Error ? error.message : String(error);
	console.error(message);
	// @ts-ignore
	process.exit(1);
}
