#!/usr/bin/env bun

// @ts-ignore
import * as fs from "node:fs";
// @ts-ignore
import * as path from "node:path";

interface SkillInput {
	name: string;
	type: "Orchestrator" | "Task";
	actor: string;
	AIDriven: boolean;
	ins: string[];
	outs: string[];
}

interface GeneratedFields {
	description: string;
	overview: string;
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

function scaffoldSkill(input: SkillInput, generated: GeneratedFields): void {
	const skillDir = path.join(SKILLS_ROOT, input.name);
	fs.mkdirSync(skillDir, { recursive: true });

	const vars: Record<string, string> = {
		name: input.name,
		description: generated.description,
		overview: generated.overview,
		type: input.type,
		actor: input.actor,
		ins: JSON.stringify(input.ins, null, "\t"),
		outs: JSON.stringify(input.outs, null, "\t"),
	};

	// Select SKILL.md template based on skill type
	const skillTemplate =
		input.type === "Orchestrator" ? "SKILL.orchestrator.md.template" : "SKILL.md.template";
	fs.writeFileSync(path.join(skillDir, "SKILL.md"), renderTemplate(skillTemplate, vars));
	fs.writeFileSync(path.join(skillDir, "structure.json"), renderTemplate("structure.json.template", vars));

	if (input.type === "Orchestrator") {
		// Orchestrator skills coordinate sub-skills; provide an empty in.json placeholder and media.json
		fs.writeFileSync(path.join(skillDir, "in.json"), "{}\n");
		fs.writeFileSync(path.join(skillDir, "media.json"), "{}\n");
	} else {
		// Task skills implement concrete work
		fs.mkdirSync(path.join(skillDir, "templates"), { recursive: true });
		if (!input.AIDriven) {
			// Non-AI driven tasks need scripts
			fs.mkdirSync(path.join(skillDir, "scripts"), { recursive: true });
		}
	}

	console.log(`Skill '${input.name}' (${input.type}) scaffolded at: ${skillDir}`);
}

// Parse command line arguments
// @ts-ignore
const args = process.argv.slice(2) as string[];
if (args.length < 6) {
	console.error("Usage: bun create-skill.ts <name> <type> <actor> <AIDriven> <description> <overview> [ins...] [outs...]");
	// @ts-ignore
	process.exit(1);
}

const input: SkillInput = {
	name: args[0],
	type: args[1] as "Orchestrator" | "Task",
	actor: args[2],
	AIDriven: args[3] === "true",
	ins: [],
	outs: []
};

const generated: GeneratedFields = {
	description: args[4],
	overview: args[5]
};

scaffoldSkill(input, generated);
