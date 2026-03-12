#!/usr/bin/env bun

// @ts-ignore
import * as fs from "node:fs";
// @ts-ignore
import * as path from "node:path";

interface SkillDefinition {
	name: string;
	description: string;
	type: "Orchestrator" | "Task";
	actor: string;
	ins: string[];
	outs: string[];
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

function scaffoldSkill(def: SkillDefinition): void {
	const skillDir = path.join(SKILLS_ROOT, def.name);
	fs.mkdirSync(skillDir, { recursive: true });

	const vars: Record<string, string> = {
		name: def.name,
		description: def.description,
		type: def.type,
		actor: def.actor,
		ins: JSON.stringify(def.ins, null, "\t"),
		outs: JSON.stringify(def.outs, null, "\t"),
	};

	// Select SKILL.md template based on skill type
	const skillTemplate =
		def.type === "Orchestrator" ? "SKILL.orchestrator.md.template" : "SKILL.md.template";
	fs.writeFileSync(path.join(skillDir, "SKILL.md"), renderTemplate(skillTemplate, vars));
	fs.writeFileSync(path.join(skillDir, "structure.json"), renderTemplate("structure.json.template", vars));

	if (def.type === "Orchestrator") {
		// Orchestrator skills coordinate sub-skills; provide an empty in.json placeholder
		fs.writeFileSync(path.join(skillDir, "in.json"), "{}\n");
	} else {
		// Task skills implement concrete work; scaffold scripts/ and templates/ directories
		fs.mkdirSync(path.join(skillDir, "scripts"), { recursive: true });
		fs.mkdirSync(path.join(skillDir, "templates"), { recursive: true });
	}

	console.log(`Skill '${def.name}' (${def.type}) scaffolded at: ${skillDir}`);
}

// @ts-ignore
const inPath = path.resolve(__dirname, "../in.json");
const def: SkillDefinition = JSON.parse(fs.readFileSync(inPath, "utf-8"));
scaffoldSkill(def);
