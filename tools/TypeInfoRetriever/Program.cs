using System.Reflection;
using System.Text;

public class TypeInfo
{
	public string FullName { get; set; } = "";
	public string Name { get; set; } = "";
	public string Namespace { get; set; } = "";
	public string Assembly { get; set; } = "";
	public string[] BaseTypes { get; set; } = Array.Empty<string>();
	public string[] Interfaces { get; set; } = Array.Empty<string>();
	public PropertyInfo[] Properties { get; set; } = Array.Empty<PropertyInfo>();
	public MethodInfo[] Methods { get; set; } = Array.Empty<MethodInfo>();
	public FieldInfo[] Fields { get; set; } = Array.Empty<FieldInfo>();
	public AttributeInfo[] Attributes { get; set; } = Array.Empty<AttributeInfo>();
	public bool IsEnum { get; set; }
	public bool IsInterface { get; set; }
	public bool IsAbstract { get; set; }
	public bool IsSealed { get; set; }
	public bool IsStatic { get; set; }
	public EnumValueInfo[]? EnumValues { get; set; }
}

public class PropertyInfo
{
	public string Name { get; set; } = "";
	public string Type { get; set; } = "";
	public bool HasGetter { get; set; }
	public bool HasSetter { get; set; }
	public bool IsPublic { get; set; }
	public bool IsStatic { get; set; }
	public AttributeInfo[] Attributes { get; set; } = Array.Empty<AttributeInfo>();
}

public class MethodInfo
{
	public string Name { get; set; } = "";
	public string ReturnType { get; set; } = "";
	public ParameterInfo[] Parameters { get; set; } = Array.Empty<ParameterInfo>();
	public bool IsPublic { get; set; }
	public bool IsStatic { get; set; }
	public bool IsVirtual { get; set; }
	public bool IsAbstract { get; set; }
	public AttributeInfo[] Attributes { get; set; } = Array.Empty<AttributeInfo>();
}

public class ParameterInfo
{
	public string Name { get; set; } = "";
	public string Type { get; set; } = "";
	public bool HasDefaultValue { get; set; }
	public string? DefaultValue { get; set; }
}

public class FieldInfo
{
	public string Name { get; set; } = "";
	public string Type { get; set; } = "";
	public bool IsPublic { get; set; }
	public bool IsStatic { get; set; }
	public bool IsReadOnly { get; set; }
	public bool IsConstant { get; set; }
	public AttributeInfo[] Attributes { get; set; } = Array.Empty<AttributeInfo>();
}

public class AttributeInfo
{
	public string Name { get; set; } = "";
	public string[]? Arguments { get; set; }
}

public class EnumValueInfo
{
	public string Name { get; set; } = "";
	public object Value { get; set; } = null!;
}

class Program
{
	static void Main(string[] args)
	{
		if (args.Length == 0)
		{
			Console.WriteLine("Usage: dotnet run <TypeFullName> [output-format]");
			Console.WriteLine("Formats: json, csharp, simple (default: simple)");
			Console.WriteLine("Example: dotnet run System.String json");
			return;
		}

		string typeName = args[0];
		string format = args.Length > 1 ? args[1].ToLower() : "simple";

		try
		{
			var typeInfo = GetTypeInfo(typeName);
			if (typeInfo == null)
			{
				Console.WriteLine($"❌ Type '{typeName}' not found.");
				return;
			}

			switch (format)
			{
				case "json":
					OutputJson(typeInfo);
					break;
				case "csharp":
					OutputCSharp(typeInfo);
					break;
				default:
					OutputSimple(typeInfo);
					break;
			}
		}
		catch (Exception ex)
		{
			Console.WriteLine($"❌ Error: {ex.Message}");
		}
	}

	static TypeInfo? GetTypeInfo(string typeName)
	{
		// 1. 尝试从当前应用域加载的程序集中查找
		var type = Type.GetType(typeName);

		// 2. 如果没找到，遍历所有已加载的程序集
		if (type == null)
		{
			var assemblies = AppDomain.CurrentDomain.GetAssemblies();
			foreach (var assembly in assemblies)
			{
				try
				{
					type = assembly.GetType(typeName);
					if (type != null) break;
				}
				catch { continue; }
			}
		}

		// 3. 尝试加载常见的程序集
		if (type == null)
		{
			var commonAssemblies = new[]
			{
								"System.Private.CoreLib",
								"System.Runtime",
								"System.Collections",
								"Microsoft.AspNetCore.App",
								"Microsoft.NETCore.App"
						};

			foreach (var asmName in commonAssemblies)
			{
				try
				{
					var asm = Assembly.Load(asmName);
					type = asm.GetType(typeName);
					if (type != null) break;
				}
				catch { continue; }
			}
		}

		// 4. 尝试从当前目录和项目输出目录加载程序集
		if (type == null)
		{
			type = LoadFromProjectAssemblies(typeName);
		}

		if (type == null) return null;

		return BuildTypeInfo(type);
	}

	static Type? LoadFromProjectAssemblies(string typeName)
	{
		var searchPaths = new List<string>
				{
						Environment.CurrentDirectory,
						Path.Combine(Environment.CurrentDirectory, "bin"),
						Path.Combine(Environment.CurrentDirectory, "bin", "Debug"),
						Path.Combine(Environment.CurrentDirectory, "bin", "Release"),
						Path.Combine(Environment.CurrentDirectory, "src", "app"),
				};

		// 查找项目根目录
		var currentDir = new DirectoryInfo(Environment.CurrentDirectory);
		while (currentDir != null)
		{
			if (currentDir.GetFiles("*.sln").Any() || currentDir.GetFiles("*.csproj").Any())
			{
				searchPaths.Add(currentDir.FullName);
				searchPaths.Add(Path.Combine(currentDir.FullName, "src"));
				searchPaths.Add(Path.Combine(currentDir.FullName, "src", "app"));
				break;
			}
			currentDir = currentDir.Parent;
		}

		foreach (var path in searchPaths.Where(Directory.Exists))
		{
			var dllFiles = Directory.GetFiles(path, "*.dll", SearchOption.AllDirectories);
			foreach (var dllFile in dllFiles)
			{
				try
				{
					var assembly = Assembly.LoadFrom(dllFile);
					var type = assembly.GetType(typeName);
					if (type != null) return type;
				}
				catch { continue; }
			}
		}

		return null;
	}

	static TypeInfo BuildTypeInfo(Type type)
	{
		var info = new TypeInfo
		{
			FullName = type.FullName ?? type.Name,
			Name = type.Name,
			Namespace = type.Namespace ?? "",
			Assembly = type.Assembly.GetName().Name ?? "",
			IsEnum = type.IsEnum,
			IsInterface = type.IsInterface,
			IsAbstract = type.IsAbstract,
			IsSealed = type.IsSealed,
			IsStatic = type.IsAbstract && type.IsSealed && !type.IsEnum
		};

		// 基类
		var baseTypes = new List<string>();
		if (type.BaseType != null && type.BaseType != typeof(object))
		{
			baseTypes.Add(GetFriendlyTypeName(type.BaseType));
		}
		info.BaseTypes = baseTypes.ToArray();

		// 接口
		info.Interfaces = type.GetInterfaces()
				.Select(GetFriendlyTypeName)
				.ToArray();

		// 特性
		info.Attributes = type.GetCustomAttributes(false)
				.Select(attr => new AttributeInfo { Name = attr.GetType().Name })
				.ToArray();

		if (type.IsEnum)
		{
			// 枚举值
			info.EnumValues = Enum.GetValues(type)
					.Cast<object>()
					.Select(value => new EnumValueInfo
					{
						Name = value.ToString() ?? "",
						Value = Convert.ToInt64(value)
					})
					.ToArray();
		}
		else
		{
			// 属性 (包括继承的属性)
			info.Properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static)
					.Select(p => new PropertyInfo
					{
						Name = p.Name,
						Type = GetFriendlyTypeName(p.PropertyType),
						HasGetter = p.CanRead,
						HasSetter = p.CanWrite,
						IsPublic = true,
						IsStatic = (p.GetGetMethod() ?? p.GetSetMethod())?.IsStatic ?? false,
						Attributes = p.GetCustomAttributes(false)
									.Select(attr => new AttributeInfo { Name = attr.GetType().Name })
									.ToArray()
					})
					.ToArray();

			// 方法 (包括继承的方法，但排除系统方法)
			info.Methods = type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static)
					.Where(m => !m.IsSpecialName && m.DeclaringType != typeof(object) && m.DeclaringType != typeof(System.ValueType))
					.Select(m => new MethodInfo
					{
						Name = m.Name,
						ReturnType = GetFriendlyTypeName(m.ReturnType),
						IsPublic = m.IsPublic,
						IsStatic = m.IsStatic,
						IsVirtual = m.IsVirtual,
						IsAbstract = m.IsAbstract,
						Parameters = m.GetParameters()
									.Select(p => new ParameterInfo
									{
										Name = p.Name ?? "",
										Type = GetFriendlyTypeName(p.ParameterType),
										HasDefaultValue = p.HasDefaultValue,
										DefaultValue = p.HasDefaultValue ? p.DefaultValue?.ToString() : null
									})
									.ToArray(),
						Attributes = m.GetCustomAttributes(false)
									.Select(attr => new AttributeInfo { Name = attr.GetType().Name })
									.ToArray()
					})
					.ToArray();

			// 字段 (包括继承的字段，但排除系统字段)
			info.Fields = type.GetFields(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static)
					.Where(f => f.DeclaringType != typeof(object) && f.DeclaringType != typeof(System.ValueType))
					.Select(f => new FieldInfo
					{
						Name = f.Name,
						Type = GetFriendlyTypeName(f.FieldType),
						IsPublic = f.IsPublic,
						IsStatic = f.IsStatic,
						IsReadOnly = f.IsInitOnly,
						IsConstant = f.IsLiteral,
						Attributes = f.GetCustomAttributes(false)
									.Select(attr => new AttributeInfo { Name = attr.GetType().Name })
									.ToArray()
					})
					.ToArray();
		}

		return info;
	}

	static string GetFriendlyTypeName(Type type)
	{
		if (type.IsGenericType)
		{
			var genericType = type.GetGenericTypeDefinition();
			var genericArgs = type.GetGenericArguments();
			var name = genericType.Name.Split('`')[0];
			var args = string.Join(", ", genericArgs.Select(GetFriendlyTypeName));
			return $"{name}<{args}>";
		}

		return type.Name switch
		{
			"String" => "string",
			"Int32" => "int",
			"Int64" => "long",
			"Boolean" => "bool",
			"Double" => "double",
			"Decimal" => "decimal",
			"DateTime" => "DateTime",
			_ => type.Name
		};
	}

	static void OutputSimple(TypeInfo typeInfo)
	{
		Console.WriteLine($"📚 Type: {typeInfo.FullName}");
		Console.WriteLine($"📦 Assembly: {typeInfo.Assembly}");
		Console.WriteLine($"📁 Namespace: {typeInfo.Namespace}");
		Console.WriteLine();

		if (typeInfo.IsEnum && typeInfo.EnumValues != null)
		{
			Console.WriteLine("🔢 Enum Values:");
			foreach (var enumValue in typeInfo.EnumValues)
			{
				Console.WriteLine($"   {enumValue.Name} = {enumValue.Value}");
			}
		}
		else
		{
			if (typeInfo.Properties.Any())
			{
				Console.WriteLine("🏷️  Properties:");
				foreach (var prop in typeInfo.Properties)
				{
					var modifiers = new List<string>();
					if (prop.IsStatic) modifiers.Add("static");
					var modifierStr = modifiers.Any() ? string.Join(" ", modifiers) + " " : "";
					Console.WriteLine($"   {modifierStr}{prop.Type} {prop.Name} {{ {(prop.HasGetter ? "get;" : "")} {(prop.HasSetter ? "set;" : "")} }}");
				}
			}

			if (typeInfo.Methods.Any())
			{
				Console.WriteLine("\n⚙️  Methods:");
				foreach (var method in typeInfo.Methods.Take(10)) // 限制显示前10个方法
				{
					var modifiers = new List<string>();
					if (method.IsStatic) modifiers.Add("static");
					if (method.IsVirtual) modifiers.Add("virtual");
					if (method.IsAbstract) modifiers.Add("abstract");
					var modifierStr = modifiers.Any() ? string.Join(" ", modifiers) + " " : "";
					var parameters = string.Join(", ", method.Parameters.Select(p => $"{p.Type} {p.Name}"));
					Console.WriteLine($"   {modifierStr}{method.ReturnType} {method.Name}({parameters})");
				}
				if (typeInfo.Methods.Length > 10)
				{
					Console.WriteLine($"   ... and {typeInfo.Methods.Length - 10} more methods");
				}
			}
		}
	}

	static void OutputJson(TypeInfo typeInfo)
	{
		var json = Newtonsoft.Json.JsonConvert.SerializeObject(typeInfo, Newtonsoft.Json.Formatting.Indented);
		Console.WriteLine(json);
	}

	static void OutputCSharp(TypeInfo typeInfo)
	{
		var sb = new StringBuilder();

		// Namespace
		if (!string.IsNullOrEmpty(typeInfo.Namespace))
		{
			sb.AppendLine($"namespace {typeInfo.Namespace};");
			sb.AppendLine();
		}

		// Class declaration
		var modifiers = new List<string> { "public" };
		if (typeInfo.IsStatic) modifiers.Add("static");
		else if (typeInfo.IsAbstract) modifiers.Add("abstract");
		else if (typeInfo.IsSealed) modifiers.Add("sealed");

		var kind = typeInfo.IsInterface ? "interface" :
							typeInfo.IsEnum ? "enum" : "class";

		sb.Append($"{string.Join(" ", modifiers)} {kind} {typeInfo.Name}");

		if (typeInfo.BaseTypes.Any() || typeInfo.Interfaces.Any())
		{
			var inheritance = typeInfo.BaseTypes.Concat(typeInfo.Interfaces);
			sb.Append($" : {string.Join(", ", inheritance)}");
		}

		sb.AppendLine();
		sb.AppendLine("{");

		if (typeInfo.IsEnum && typeInfo.EnumValues != null)
		{
			// Enum values
			foreach (var enumValue in typeInfo.EnumValues)
			{
				sb.AppendLine($"    {enumValue.Name} = {enumValue.Value},");
			}
		}
		else
		{
			// Properties
			foreach (var prop in typeInfo.Properties)
			{
				var modifiers2 = new List<string> { "public" };
				if (prop.IsStatic) modifiers2.Add("static");
				sb.AppendLine($"    {string.Join(" ", modifiers2)} {prop.Type} {prop.Name} {{ get; set; }}");
			}

			if (typeInfo.Properties.Any() && typeInfo.Methods.Any())
			{
				sb.AppendLine();
			}

			// Methods (简化显示)
			foreach (var method in typeInfo.Methods.Take(5))
			{
				var modifiers3 = new List<string> { "public" };
				if (method.IsStatic) modifiers3.Add("static");
				if (method.IsVirtual) modifiers3.Add("virtual");
				if (method.IsAbstract) modifiers3.Add("abstract");

				var parameters = string.Join(", ", method.Parameters.Select(p => $"{p.Type} {p.Name}"));
				var semicolon = method.IsAbstract ? ";" : " { }";
				sb.AppendLine($"    {string.Join(" ", modifiers3)} {method.ReturnType} {method.Name}({parameters}){semicolon}");
			}

			if (typeInfo.Methods.Length > 5)
			{
				sb.AppendLine($"    // ... and {typeInfo.Methods.Length - 5} more methods");
			}
		}

		sb.AppendLine("}");
		Console.WriteLine(sb.ToString());
	}
}
