using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using KsbReportTool.Services;

namespace KsbReportTool
{
    public partial class MainForm : Form
    {
        private string _table1Path;
        private string _table2Path;
        private string _templatePath;
        private string _outputDir;

        public MainForm()
        {
            InitializeComponent();
            _outputDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory), "KSB_Reports_Output");
            txtOutputDir.Text = _outputDir;
            Log("欢迎使用 KSB 初始性能报告批量导出");
        }

        private void btnSelectTable1_Click(object sender, EventArgs e)
        {
            _table1Path = SelectXlsx();
            if (!string.IsNullOrEmpty(_table1Path)) txtTable1.Text = _table1Path;
        }

        private void btnSelectTable2_Click(object sender, EventArgs e)
        {
            _table2Path = SelectXlsx();
            if (!string.IsNullOrEmpty(_table2Path)) txtTable2.Text = _table2Path;
        }

        private void btnSelectTemplate_Click(object sender, EventArgs e)
        {
            _templatePath = SelectXlsx();
            if (!string.IsNullOrEmpty(_templatePath)) txtTemplate.Text = _templatePath;
        }

        private void btnSelectOutput_Click(object sender, EventArgs e)
        {
            using (var dlg = new FolderBrowserDialog())
            {
                dlg.SelectedPath = _outputDir;
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    _outputDir = dlg.SelectedPath;
                    txtOutputDir.Text = _outputDir;
                }
            }
        }

        private void btnOpenOutput_Click(object sender, EventArgs e)
        {
            try
            {
                if (!Directory.Exists(_outputDir)) Directory.CreateDirectory(_outputDir);
                Process.Start(_outputDir);
            }
            catch (Exception ex)
            {
                Log("打开输出目录失败: " + ex.Message);
            }
        }

        private void btnRun_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(_table1Path) || string.IsNullOrEmpty(_table2Path) || string.IsNullOrEmpty(_templatePath))
                {
                    Log("请先选择表1、表2、模板文件。");
                    return;
                }

                if (!Directory.Exists(_outputDir)) Directory.CreateDirectory(_outputDir);

                Log("开始生成...");
                var result = KsbProcessor.Process(_table1Path, _table2Path, _templatePath, _outputDir, chkZip.Checked);

                Log("生成数量: " + result.GeneratedCount);
                Log("跳过数量: " + result.SkippedCount);
                foreach (var s in result.SkippedItems)
                {
                    Log("跳过: " + s);
                }

                var logPath = Path.Combine(_outputDir, "log.txt");
                File.WriteAllLines(logPath, result.AllLogs);
                Log("日志已写入: " + logPath);
            }
            catch (Exception ex)
            {
                Log("发生错误: " + ex.Message);
            }
        }

        private string SelectXlsx()
        {
            using (var dlg = new OpenFileDialog())
            {
                dlg.Filter = "Excel Files (*.xlsx)|*.xlsx";
                dlg.Title = "选择 xlsx 文件";
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    return dlg.FileName;
                }
            }
            return null;
        }

        private void Log(string msg)
        {
            txtLog.AppendText("[" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "] " + msg + Environment.NewLine);
        }
    }
}
