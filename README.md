# KSB 初始性能报告批量导出工具

WinForms 桌面应用，用于批量生成 KSB 初始性能报告。

## 📥 下载安装包

### 方式1：GitHub Actions（推荐）
1. 打开 [Actions](../../actions) 页面
2. 选择 `build-win7-exe` 工作流
3. 点击最新成功的运行记录
4. 下载 `KsbReportTool_Release` artifact
5. 解压到任意文件夹

### 方式2：Releases 页面
1. 打开 [Releases](../../releases) 页面
2. 下载最新版本
3. 解压到任意文件夹

## 🚀 使用方法

1. 运行 `KsbReportTool.exe`
2. 选择 **表1**：KSB原始报告汇总表.xlsx
3. 选择 **表2**：KSB编号对照汇总表.xlsx
4. （可选）修改输出目录
5. 点击"开始生成"
6. 生成的报告保存在输出目录中

## 💻 系统要求

- Windows 7 SP1 或更高版本
- .NET Framework 4.8

## 🏗️ 开发构建

项目使用 GitHub Actions 自动构建：
- 推送代码到 `main` 分支自动触发构建
- 构建产物可通过 Actions 页面下载

## 📝 更新日志

**v1.1** (2026-02-09)
- 模板内置，无需手动选择
- 简化用户界面

**v1.0** (2026-02-08)
- 初始版本
