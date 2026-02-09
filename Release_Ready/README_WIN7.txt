KSB 初始性能报告生成工具 (Win7 兼容版)
========================================

📥 获取安装包
--------------
由于安装包包含多个依赖文件，请通过以下方式获取：

方式1（推荐）：GitHub Actions
1. 打开 GitHub 仓库 → Actions → build-win7-exe
2. 选择最新成功的运行记录
3. 下载 Artifacts: KsbReportTool_Release
4. 解压后即可使用

方式2：Releases 页面
1. 打开 GitHub 仓库 → Releases
2. 下载最新版本的 KSB_ReportTool.zip
3. 解压后即可使用

🚀 使用方法
------------
1. 解压安装包
2. 确保 Resources\KSBTemplate.xlsx 内置模板文件存在（已内置）
3. 双击 KsbReportTool.exe 运行
4. 在界面中选择两个输入文件：
   - KSB原始报告汇总表.xlsx（表1）
   - KSB编号对照汇总表.xlsx（表2）
5. 点击"开始生成"按钮
6. 生成的报告将保存在输出目录中

💻 系统要求
------------
- Windows 7 SP1 或更高版本
- .NET Framework 4.8 (Win7 需单独安装)

📁 文件说明
------------
- KsbReportTool.exe - 主程序
- KsbReportTool.exe.config - 配置文件
- Resources\KSBTemplate.xlsx - 内置报告模板（OH2）
- *.dll - 运行依赖库 (NPOI, SharpZipLib 等)

📝 更新说明
------------
- 模板已内置，无需手动选择
- 只需选择表1、表2即可生成报告

生成时间：2026-02-09
