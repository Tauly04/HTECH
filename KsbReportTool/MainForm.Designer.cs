namespace KsbReportTool
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.btnSelectTable1 = new System.Windows.Forms.Button();
            this.btnSelectTable2 = new System.Windows.Forms.Button();
            this.txtTable1 = new System.Windows.Forms.TextBox();
            this.txtTable2 = new System.Windows.Forms.TextBox();
            this.lblTemplate = new System.Windows.Forms.Label();
            this.btnSelectOutput = new System.Windows.Forms.Button();
            this.txtOutputDir = new System.Windows.Forms.TextBox();
            this.btnRun = new System.Windows.Forms.Button();
            this.btnOpenOutput = new System.Windows.Forms.Button();
            this.chkZip = new System.Windows.Forms.CheckBox();
            this.txtLog = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // btnSelectTable1
            // 
            this.btnSelectTable1.Location = new System.Drawing.Point(20, 20);
            this.btnSelectTable1.Name = "btnSelectTable1";
            this.btnSelectTable1.Size = new System.Drawing.Size(110, 28);
            this.btnSelectTable1.TabIndex = 0;
            this.btnSelectTable1.Text = "选择表1";
            this.btnSelectTable1.UseVisualStyleBackColor = true;
            this.btnSelectTable1.Click += new System.EventHandler(this.btnSelectTable1_Click);
            // 
            // btnSelectTable2
            // 
            this.btnSelectTable2.Location = new System.Drawing.Point(20, 60);
            this.btnSelectTable2.Name = "btnSelectTable2";
            this.btnSelectTable2.Size = new System.Drawing.Size(110, 28);
            this.btnSelectTable2.TabIndex = 1;
            this.btnSelectTable2.Text = "选择表2";
            this.btnSelectTable2.UseVisualStyleBackColor = true;
            this.btnSelectTable2.Click += new System.EventHandler(this.btnSelectTable2_Click);
            // 
            // txtTable1
            // 
            this.txtTable1.Location = new System.Drawing.Point(140, 23);
            this.txtTable1.Name = "txtTable1";
            this.txtTable1.ReadOnly = true;
            this.txtTable1.Size = new System.Drawing.Size(520, 21);
            this.txtTable1.TabIndex = 3;
            // 
            // txtTable2
            // 
            this.txtTable2.Location = new System.Drawing.Point(140, 63);
            this.txtTable2.Name = "txtTable2";
            this.txtTable2.ReadOnly = true;
            this.txtTable2.Size = new System.Drawing.Size(520, 21);
            this.txtTable2.TabIndex = 4;
            // 
            // lblTemplate
            // 
            this.lblTemplate.AutoSize = true;
            this.lblTemplate.Location = new System.Drawing.Point(140, 107);
            this.lblTemplate.Name = "lblTemplate";
            this.lblTemplate.Size = new System.Drawing.Size(107, 12);
            this.lblTemplate.TabIndex = 5;
            this.lblTemplate.Text = "模板: 内置（OH2）";
            // 
            // btnSelectOutput
            // 
            this.btnSelectOutput.Location = new System.Drawing.Point(20, 140);
            this.btnSelectOutput.Name = "btnSelectOutput";
            this.btnSelectOutput.Size = new System.Drawing.Size(110, 28);
            this.btnSelectOutput.TabIndex = 6;
            this.btnSelectOutput.Text = "选择输出目录";
            this.btnSelectOutput.UseVisualStyleBackColor = true;
            this.btnSelectOutput.Click += new System.EventHandler(this.btnSelectOutput_Click);
            // 
            // txtOutputDir
            // 
            this.txtOutputDir.Location = new System.Drawing.Point(140, 143);
            this.txtOutputDir.Name = "txtOutputDir";
            this.txtOutputDir.ReadOnly = true;
            this.txtOutputDir.Size = new System.Drawing.Size(520, 21);
            this.txtOutputDir.TabIndex = 7;
            // 
            // btnRun
            // 
            this.btnRun.Location = new System.Drawing.Point(20, 180);
            this.btnRun.Name = "btnRun";
            this.btnRun.Size = new System.Drawing.Size(110, 30);
            this.btnRun.TabIndex = 8;
            this.btnRun.Text = "开始生成";
            this.btnRun.UseVisualStyleBackColor = true;
            this.btnRun.Click += new System.EventHandler(this.btnRun_Click);
            // 
            // btnOpenOutput
            // 
            this.btnOpenOutput.Location = new System.Drawing.Point(140, 180);
            this.btnOpenOutput.Name = "btnOpenOutput";
            this.btnOpenOutput.Size = new System.Drawing.Size(120, 30);
            this.btnOpenOutput.TabIndex = 9;
            this.btnOpenOutput.Text = "打开输出目录";
            this.btnOpenOutput.UseVisualStyleBackColor = true;
            this.btnOpenOutput.Click += new System.EventHandler(this.btnOpenOutput_Click);
            // 
            // chkZip
            // 
            this.chkZip.AutoSize = true;
            this.chkZip.Location = new System.Drawing.Point(280, 187);
            this.chkZip.Name = "chkZip";
            this.chkZip.Size = new System.Drawing.Size(96, 16);
            this.chkZip.TabIndex = 10;
            this.chkZip.Text = "同时生成ZIP";
            this.chkZip.UseVisualStyleBackColor = true;
            // 
            // txtLog
            // 
            this.txtLog.Location = new System.Drawing.Point(20, 220);
            this.txtLog.Multiline = true;
            this.txtLog.Name = "txtLog";
            this.txtLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtLog.Size = new System.Drawing.Size(640, 240);
            this.txtLog.TabIndex = 11;
            // 
            // MainForm
            // 
            this.ClientSize = new System.Drawing.Size(684, 481);
            this.Controls.Add(this.txtLog);
            this.Controls.Add(this.chkZip);
            this.Controls.Add(this.btnOpenOutput);
            this.Controls.Add(this.btnRun);
            this.Controls.Add(this.txtOutputDir);
            this.Controls.Add(this.btnSelectOutput);
            this.Controls.Add(this.lblTemplate);
            this.Controls.Add(this.txtTable2);
            this.Controls.Add(this.txtTable1);
            this.Controls.Add(this.btnSelectTable2);
            this.Controls.Add(this.btnSelectTable1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "KSB 初始性能报告批量导出";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Button btnSelectTable1;
        private System.Windows.Forms.Button btnSelectTable2;
        private System.Windows.Forms.TextBox txtTable1;
        private System.Windows.Forms.TextBox txtTable2;
        private System.Windows.Forms.Label lblTemplate;
        private System.Windows.Forms.Button btnSelectOutput;
        private System.Windows.Forms.TextBox txtOutputDir;
        private System.Windows.Forms.Button btnRun;
        private System.Windows.Forms.Button btnOpenOutput;
        private System.Windows.Forms.CheckBox chkZip;
        private System.Windows.Forms.TextBox txtLog;
    }
}
