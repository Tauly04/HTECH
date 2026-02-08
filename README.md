# 我该怎么上传 GitHub 并下载成品

1. 把整个工程目录上传到你的 GitHub 仓库根目录（包含 `.github/workflows/build.yml`）。
2. 推送到 `main` 分支。
3. 打开 GitHub → Actions → 选择 `build-win7-exe` → 进入最新一次运行。
4. 在页面底部下载 `KsbReportTool_Release` artifact。
5. 解压后双击 `KsbReportTool.exe` 即可使用。
