# eServiceCloud Foundation Files

这个目录包含创建 eServiceCloud 风格项目时必需的基础文件模板。

## 目录结构

```
foundation-files/
├── template-manifest.json      # 模板文件配置清单
├── .editorconfig               # 编辑器配置文件
├── .gitattributes             # Git 属性配置
├── .gitignore                 # Git 忽略文件模式  
├── run.sh                     # 开发服务器启动脚本
├── Core/                       # Core 层基础文件
│   ├── Common/
│   │   ├── Pager.cs           # 分页支持
│   │   ├── PagerResult.cs     # 分页结果容器
│   │   └── ServicesExtensions.cs # DI 性能日志扩展
│   └── Models/
│       ├── IModel.cs          # 基础模型接口
│       └── Model.cs           # 通用模型基类
├── Repositories/               # Repositories 层基础文件
│   └── Common/
│       ├── IRepository.cs              # 仓储接口
│       ├── CommonRepository.cs         # 仓储基础实现
│       └── CommonVersionRepository.cs  # 版本化实体仓储基类
├── BusinessProcess/            # BusinessProcess 层基础文件（暂无）
└── Blazor/                    # Blazor 层基础文件（暂无）
```

## 使用方式

1. **自动模板处理**（推荐）
   - 使用 `template-manifest.json` 配置
   - 脚本自动复制文件并替换命名空间

2. **手动复制**
   ```bash
   # 复制根目录配置文件
   cp foundation-files/.editorconfig .
   cp foundation-files/.gitattributes .
   cp foundation-files/.gitignore .
   cp foundation-files/run.sh .
   
   # 复制 Core 层文件
   cp foundation-files/Core/Common/*.cs target-project/Core/Common/
   cp foundation-files/Core/Models/*.cs target-project/Core/Models/
   
   # 复制 Repositories 层文件
   cp foundation-files/Repositories/Common/*.cs target-project/Repositories/Common/
   
   # 替换命名空间
   find target-project -name "*.cs" -exec sed -i 's/Sanjel\.eServiceCloud/YourPrefix.YourProject/g' {} +
   
   # 替换 run.sh 中的项目引用
   sed -i 's/Sanjel\.eServiceCloud\.Blazor/YourPrefix.YourProject.Blazor/g' run.sh
   ```

## 模板文件说明

### 项目根目录文件

- **.editorconfig**: 编辑器配置，确保代码格式一致性
- **.gitattributes**: Git 属性配置，处理行尾和文件类型
- **.gitignore**: Git 忽略模式，适用于 .NET 项目
- **run.sh**: 开发服务器启动脚本，包含项目清理和启动命令

### Core 层

- **Pager.cs**: 提供分页功能支持，包含 PageIndex, PageSize, PageTotal 等属性
- **PagerResult.cs**: 分页查询结果的容器类
- **IModel.cs**: 所有领域模型的基础接口，定义 Id 属性
- **Model.cs**: 通用模型基类，提供与 MDM 实体的映射功能

### Repositories 层

- **IRepository.cs**: 通用仓储接口，定义 CRUD 和分页查询操作
- **CommonRepository.cs**: 仓储基础实现，提供通用的数据访问逻辑
- **CommonVersionRepository.cs**: 版本化实体的仓储基类，支持审计字段

## 维护说明

1. **保持同步**: 模板文件应与 eServiceCloud 参考实现保持同步
2. **版本管理**: 当 eServiceCloud 基础文件更新时，需要同步更新模板文件
3. **测试验证**: 添加新模板文件后，需要测试项目创建流程确保正常工作

## 扩展机制

要添加新的模板文件：

1. 将文件复制到相应的层目录下
2. 在 `template-manifest.json` 中添加配置条目
3. 指定源路径、目标路径和替换规则
4. 测试验证新文件的模板处理逻辑