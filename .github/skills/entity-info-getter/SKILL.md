---
name: entity-info-getter
description: Get detailed information about business entities in Sesi.SanjelData.Entities namespace. Use when users need entity structure, fields, properties, and relationships for mapping, debugging, or documentation. Triggers on: get entity info, entity information, entity details.
---

# Entity Info Getter

## Skill Purpose

This skill retrieves comprehensive information about business entities in the `Sesi.SanjelData.Entities` namespace. It provides detailed insights into entity structure, properties, relationships, and metadata from the MDM (Master Data Management) system to support development, mapping, debugging, and documentation tasks.

## When to Use

**Primary Triggers:**
- When users ask for entity information, structure details, or entity properties
- During mapping implementation from MDM entities to domain models
- For debugging entity-related issues or understanding data relationships
- When creating documentation about business entities
- Before implementing repository patterns or entity mapping services

**Usage Scenarios:**
- "Get entity info for Employee"
- "Show me the structure of UnitOfMeasureSystemOfUnits entity"
- "What properties does BonusPosition entity have?"
- "Entity details for mapping purposes"

## How to Use

### Primary Tool: TypeInfoRetriever

For MDM entities under `Sesi.SanjelData.Entities` namespace:

```bash
cd tools/TypeInfoRetriever && dotnet run --project TypeInfoRetriever.csproj "<entityFullName>" [format]
```

**Example:**
```bash
cd tools/TypeInfoRetriever && dotnet run --project TypeInfoRetriever.csproj "Sesi.SanjelData.Entities.Common.Model.UnitOfMeasure.UnitOfMeasureSystemOfUnits"
```

**Formats Available:**
- `simple` (default): Clean, readable format
- `csharp`: C# code structure
- `json`: Structured JSON data

**Examples:**
```bash
# Basic entity info (default simple format)
cd tools/TypeInfoRetriever && dotnet run --project TypeInfoRetriever.csproj "Sesi.SanjelData.Entities.Common.BusinessEntities.HumanResources.Employee"

# C# structure format
cd tools/TypeInfoRetriever && dotnet run --project TypeInfoRetriever.csproj "Sesi.SanjelData.Entities.Common.BusinessEntities.HumanResources.BonusPosition" csharp

# JSON output
cd tools/TypeInfoRetriever && dotnet run --project TypeInfoRetriever.csproj "Sesi.SanjelData.Entities.Common.Model.UnitOfMeasure.UnitOfMeasureSystemOfUnits" json
```

## Output Information

**Entity Details Include:**

✅ **Properties & Fields**
- Property names, types, and accessibility
- Field information and modifiers
- Nullable reference types indication

✅ **Type Relationships**
- Base classes and inheritance hierarchy
- Implemented interfaces
- Generic type parameters and constraints

✅ **Structural Information**
- Class modifiers (abstract, sealed, static)
- Constructor signatures
- Method signatures and overloads

✅ **Metadata**
- Namespace and assembly information
- Attributes and annotations
- XML documentation (when available)

✅ **Dependencies**
- Referenced types and assemblies
- Navigation properties for entity relationships

## AI Integration Guidelines

**When to Trigger:**
- User asks for "entity info", "entity details", "entity structure"
- Questions about properties, fields, or relationships of Sesi.SanjelData.Entities types
- Mapping-related queries requiring MDM entity structure understanding
- Debugging scenarios needing entity information from MDM system

**Processing Steps:**
1. Extract the entity name from user query
2. Verify it's a Sesi.SanjelData.Entities namespace type
3. Use TypeInfoRetriever tool as the primary method
4. Navigate to the correct directory and execute with appropriate format
5. Return formatted results with entity structure explanation
6. Provide context about the entity structure for user understanding

**Example Implementation:**

```
User Request: "Get entity info for Employee"

AI Processing:
1. Identify entity: Sesi.SanjelData.Entities.Common.BusinessEntities.HumanResources.Employee
2. Execute command: cd tools/TypeInfoRetriever && dotnet run --project TypeInfoRetriever.csproj "Sesi.SanjelData.Entities.Common.BusinessEntities.HumanResources.Employee"
3. Return results with explanation of entity structure and properties
4. Provide context about how the entity relates to business domain
```

## Best Practices

### For Developers

**🎯 Use Before Mapping**
- Always check MDM entity structure before creating mapping configurations
- Verify entity property types and nullability for accurate mapping to models
- Understand entity relationships before implementing navigation properties

**🔍 Debug with Details**
- Use different output formats to get the right level of detail
- Understand MDM entity structures when debugging data mapping issues
- Check inheritance hierarchies for polymorphic entity scenarios

**📝 Document Findings**
- Save entity information for team reference
- Include property details in mapping documentation
- Update domain models based on entity analysis

### For AI Agents

**🤖 Smart Tool Selection**
- Use TypeInfoRetriever as the primary tool for all Sesi.SanjelData.Entities types
- Navigate to the correct directory before execution
- Choose appropriate output format based on user needs (simple, csharp, json)
- Ensure entity name includes complete Sesi.SanjelData.Entities namespace

**📊 Context-Aware Output**
- Format output appropriately for user's intended use
- Highlight relevant properties for mapping scenarios
- Provide relationship context when showing entity structures

**🔄 Follow-up Actions**
- Suggest related entities when showing relationships
- Recommend mapping patterns based on discovered structures
- Offer to generate boilerplate code using discovered properties

## Troubleshooting

### Common Issues

**❌ Type Not Found**
```
Solutions:
- Verify the full namespace path is correct
- Check if the type exists in the current solution
- Ensure proper case sensitivity for type names
- Try searching with partial names first
```

**❌ Tool Execution Errors**
```
Solutions:
- Ensure you're in the correct workspace directory
- Verify all required projects are built successfully
- Check that dependencies are properly restored
- Try building the solution first: dotnet build
```

**❌ Output Format Issues**
```
Solutions:
- Try different output formats (simple, csharp, json)
- Check for special characters in type names
- Ensure proper quoting of type names with spaces
```

### Advanced Usage

**🔧 Batch Processing**
- Create scripts to analyze multiple related types
- Use JSON output for programmatic processing
- Combine with grep/filter tools for specific property searches

**🔍 Deep Analysis**
- Use C# format to understand implementation details
- Combine with source code analysis for complete picture
- Cross-reference with database schema when applicable

---

**Supported Languages**: English
**Version Requirements**: .NET 6.0+
**Required Tools**: dotnet CLI, TypeInfoRetriever tool, eServiceCloud solution