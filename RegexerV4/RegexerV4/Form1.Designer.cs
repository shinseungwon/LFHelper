
namespace RegexerV4
{
    partial class Form1
    {
        /// <summary>
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.tbMain = new System.Windows.Forms.TabControl();
            this.tpRegex = new System.Windows.Forms.TabPage();
            this.btRegexRedo = new System.Windows.Forms.Button();
            this.btRegexUndo = new System.Windows.Forms.Button();
            this.lbRegexDelimeter = new System.Windows.Forms.Label();
            this.tbRegexDelimeter = new System.Windows.Forms.TextBox();
            this.dgRegexReplace = new System.Windows.Forms.DataGridView();
            this.From = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.To = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btRegexReplace = new System.Windows.Forms.Button();
            this.btRegexGo = new System.Windows.Forms.Button();
            this.tbRegexOutput = new System.Windows.Forms.TextBox();
            this.tbRegexInput = new System.Windows.Forms.TextBox();
            this.cbRegexFormula = new System.Windows.Forms.ComboBox();
            this.tpTxt = new System.Windows.Forms.TabPage();
            this.tbTxtSeq = new System.Windows.Forms.TextBox();
            this.tbTxtStarts = new System.Windows.Forms.TextBox();
            this.btTxtCheck = new System.Windows.Forms.Button();
            this.tbTxtContent = new System.Windows.Forms.TextBox();
            this.dgTxt = new System.Windows.Forms.DataGridView();
            this.tpXml = new System.Windows.Forms.TabPage();
            this.tbXmlDirectory = new System.Windows.Forms.TextBox();
            this.btXmlCheck = new System.Windows.Forms.Button();
            this.tbXmlContent = new System.Windows.Forms.TextBox();
            this.dgXml = new System.Windows.Forms.DataGridView();
            this.tbMain.SuspendLayout();
            this.tpRegex.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgRegexReplace)).BeginInit();
            this.tpTxt.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgTxt)).BeginInit();
            this.tpXml.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgXml)).BeginInit();
            this.SuspendLayout();
            // 
            // tbMain
            // 
            this.tbMain.AllowDrop = true;
            this.tbMain.Controls.Add(this.tpRegex);
            this.tbMain.Controls.Add(this.tpTxt);
            this.tbMain.Controls.Add(this.tpXml);
            this.tbMain.Location = new System.Drawing.Point(12, 12);
            this.tbMain.Name = "tbMain";
            this.tbMain.SelectedIndex = 0;
            this.tbMain.Size = new System.Drawing.Size(1310, 626);
            this.tbMain.TabIndex = 0;
            // 
            // tpRegex
            // 
            this.tpRegex.Controls.Add(this.btRegexRedo);
            this.tpRegex.Controls.Add(this.btRegexUndo);
            this.tpRegex.Controls.Add(this.lbRegexDelimeter);
            this.tpRegex.Controls.Add(this.tbRegexDelimeter);
            this.tpRegex.Controls.Add(this.dgRegexReplace);
            this.tpRegex.Controls.Add(this.btRegexReplace);
            this.tpRegex.Controls.Add(this.btRegexGo);
            this.tpRegex.Controls.Add(this.tbRegexOutput);
            this.tpRegex.Controls.Add(this.tbRegexInput);
            this.tpRegex.Controls.Add(this.cbRegexFormula);
            this.tpRegex.Location = new System.Drawing.Point(4, 22);
            this.tpRegex.Name = "tpRegex";
            this.tpRegex.Padding = new System.Windows.Forms.Padding(3);
            this.tpRegex.Size = new System.Drawing.Size(1302, 600);
            this.tpRegex.TabIndex = 0;
            this.tpRegex.Text = "tpRegex";
            this.tpRegex.UseVisualStyleBackColor = true;
            // 
            // btRegexRedo
            // 
            this.btRegexRedo.Location = new System.Drawing.Point(1181, 30);
            this.btRegexRedo.Name = "btRegexRedo";
            this.btRegexRedo.Size = new System.Drawing.Size(115, 23);
            this.btRegexRedo.TabIndex = 9;
            this.btRegexRedo.Text = "btRegexRedo";
            this.btRegexRedo.UseVisualStyleBackColor = true;
            this.btRegexRedo.Click += new System.EventHandler(this.btRegexRedo_Click);
            // 
            // btRegexUndo
            // 
            this.btRegexUndo.Location = new System.Drawing.Point(1049, 30);
            this.btRegexUndo.Name = "btRegexUndo";
            this.btRegexUndo.Size = new System.Drawing.Size(126, 23);
            this.btRegexUndo.TabIndex = 8;
            this.btRegexUndo.Text = "btRegexUndo";
            this.btRegexUndo.UseVisualStyleBackColor = true;
            this.btRegexUndo.Click += new System.EventHandler(this.btRegexUndo_Click);
            // 
            // lbRegexDelimeter
            // 
            this.lbRegexDelimeter.AutoSize = true;
            this.lbRegexDelimeter.Location = new System.Drawing.Point(517, 9);
            this.lbRegexDelimeter.Name = "lbRegexDelimeter";
            this.lbRegexDelimeter.Size = new System.Drawing.Size(104, 12);
            this.lbRegexDelimeter.TabIndex = 7;
            this.lbRegexDelimeter.Text = "lbRegexDelimeter";
            // 
            // tbRegexDelimeter
            // 
            this.tbRegexDelimeter.AcceptsReturn = true;
            this.tbRegexDelimeter.AcceptsTab = true;
            this.tbRegexDelimeter.Location = new System.Drawing.Point(415, 6);
            this.tbRegexDelimeter.Multiline = true;
            this.tbRegexDelimeter.Name = "tbRegexDelimeter";
            this.tbRegexDelimeter.Size = new System.Drawing.Size(96, 21);
            this.tbRegexDelimeter.TabIndex = 6;
            this.tbRegexDelimeter.TextChanged += new System.EventHandler(this.tbRegexDelimeter_TextChanged);
            // 
            // dgRegexReplace
            // 
            this.dgRegexReplace.AllowUserToOrderColumns = true;
            this.dgRegexReplace.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgRegexReplace.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.From,
            this.To});
            this.dgRegexReplace.Location = new System.Drawing.Point(1049, 59);
            this.dgRegexReplace.Name = "dgRegexReplace";
            this.dgRegexReplace.RowTemplate.Height = 23;
            this.dgRegexReplace.Size = new System.Drawing.Size(247, 535);
            this.dgRegexReplace.TabIndex = 5;
            this.dgRegexReplace.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dgRegexReplace_KeyDown);
            // 
            // From
            // 
            this.From.HeaderText = "From";
            this.From.Name = "From";
            // 
            // To
            // 
            this.To.HeaderText = "To";
            this.To.Name = "To";
            // 
            // btRegexReplace
            // 
            this.btRegexReplace.Location = new System.Drawing.Point(1181, 3);
            this.btRegexReplace.Name = "btRegexReplace";
            this.btRegexReplace.Size = new System.Drawing.Size(115, 23);
            this.btRegexReplace.TabIndex = 4;
            this.btRegexReplace.Text = "btRegexReplace";
            this.btRegexReplace.UseVisualStyleBackColor = true;
            this.btRegexReplace.Click += new System.EventHandler(this.btRegexReplace_Click);
            // 
            // btRegexGo
            // 
            this.btRegexGo.Location = new System.Drawing.Point(1049, 4);
            this.btRegexGo.Name = "btRegexGo";
            this.btRegexGo.Size = new System.Drawing.Size(126, 23);
            this.btRegexGo.TabIndex = 3;
            this.btRegexGo.Text = "btRegexGo";
            this.btRegexGo.UseVisualStyleBackColor = true;
            this.btRegexGo.Click += new System.EventHandler(this.btRegexGo_Click);
            // 
            // tbRegexOutput
            // 
            this.tbRegexOutput.Location = new System.Drawing.Point(520, 59);
            this.tbRegexOutput.Multiline = true;
            this.tbRegexOutput.Name = "tbRegexOutput";
            this.tbRegexOutput.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.tbRegexOutput.Size = new System.Drawing.Size(523, 535);
            this.tbRegexOutput.TabIndex = 2;
            this.tbRegexOutput.WordWrap = false;
            // 
            // tbRegexInput
            // 
            this.tbRegexInput.Location = new System.Drawing.Point(3, 59);
            this.tbRegexInput.Multiline = true;
            this.tbRegexInput.Name = "tbRegexInput";
            this.tbRegexInput.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.tbRegexInput.Size = new System.Drawing.Size(508, 535);
            this.tbRegexInput.TabIndex = 1;
            this.tbRegexInput.WordWrap = false;
            // 
            // cbRegexFormula
            // 
            this.cbRegexFormula.FormattingEnabled = true;
            this.cbRegexFormula.Location = new System.Drawing.Point(6, 6);
            this.cbRegexFormula.Name = "cbRegexFormula";
            this.cbRegexFormula.Size = new System.Drawing.Size(406, 20);
            this.cbRegexFormula.TabIndex = 0;
            // 
            // tpTxt
            // 
            this.tpTxt.AllowDrop = true;
            this.tpTxt.Controls.Add(this.tbTxtSeq);
            this.tpTxt.Controls.Add(this.tbTxtStarts);
            this.tpTxt.Controls.Add(this.btTxtCheck);
            this.tpTxt.Controls.Add(this.tbTxtContent);
            this.tpTxt.Controls.Add(this.dgTxt);
            this.tpTxt.Location = new System.Drawing.Point(4, 22);
            this.tpTxt.Name = "tpTxt";
            this.tpTxt.Padding = new System.Windows.Forms.Padding(3);
            this.tpTxt.Size = new System.Drawing.Size(1302, 600);
            this.tpTxt.TabIndex = 1;
            this.tpTxt.Text = "tpTxt";
            this.tpTxt.UseVisualStyleBackColor = true;
            // 
            // tbTxtSeq
            // 
            this.tbTxtSeq.Location = new System.Drawing.Point(6, 371);
            this.tbTxtSeq.Name = "tbTxtSeq";
            this.tbTxtSeq.Size = new System.Drawing.Size(1290, 21);
            this.tbTxtSeq.TabIndex = 5;
            // 
            // tbTxtStarts
            // 
            this.tbTxtStarts.Location = new System.Drawing.Point(6, 398);
            this.tbTxtStarts.Name = "tbTxtStarts";
            this.tbTxtStarts.Size = new System.Drawing.Size(1290, 21);
            this.tbTxtStarts.TabIndex = 4;
            // 
            // btTxtCheck
            // 
            this.btTxtCheck.Location = new System.Drawing.Point(6, 571);
            this.btTxtCheck.Name = "btTxtCheck";
            this.btTxtCheck.Size = new System.Drawing.Size(1290, 23);
            this.btTxtCheck.TabIndex = 2;
            this.btTxtCheck.Text = "btTxtCheck";
            this.btTxtCheck.UseVisualStyleBackColor = true;
            this.btTxtCheck.Click += new System.EventHandler(this.btTxtCheck_Click);
            // 
            // tbTxtContent
            // 
            this.tbTxtContent.AllowDrop = true;
            this.tbTxtContent.Location = new System.Drawing.Point(6, 425);
            this.tbTxtContent.Multiline = true;
            this.tbTxtContent.Name = "tbTxtContent";
            this.tbTxtContent.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.tbTxtContent.Size = new System.Drawing.Size(1290, 140);
            this.tbTxtContent.TabIndex = 1;
            this.tbTxtContent.WordWrap = false;
            // 
            // dgTxt
            // 
            this.dgTxt.AllowUserToAddRows = false;
            this.dgTxt.AllowUserToDeleteRows = false;
            this.dgTxt.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgTxt.Location = new System.Drawing.Point(6, 6);
            this.dgTxt.Name = "dgTxt";
            this.dgTxt.ReadOnly = true;
            this.dgTxt.RowTemplate.Height = 23;
            this.dgTxt.Size = new System.Drawing.Size(1290, 359);
            this.dgTxt.TabIndex = 0;
            // 
            // tpXml
            // 
            this.tpXml.Controls.Add(this.tbXmlDirectory);
            this.tpXml.Controls.Add(this.btXmlCheck);
            this.tpXml.Controls.Add(this.tbXmlContent);
            this.tpXml.Controls.Add(this.dgXml);
            this.tpXml.Location = new System.Drawing.Point(4, 22);
            this.tpXml.Name = "tpXml";
            this.tpXml.Size = new System.Drawing.Size(1302, 600);
            this.tpXml.TabIndex = 2;
            this.tpXml.Text = "tpXml";
            this.tpXml.UseVisualStyleBackColor = true;
            // 
            // tbXmlDirectory
            // 
            this.tbXmlDirectory.Location = new System.Drawing.Point(6, 398);
            this.tbXmlDirectory.Name = "tbXmlDirectory";
            this.tbXmlDirectory.Size = new System.Drawing.Size(1290, 21);
            this.tbXmlDirectory.TabIndex = 3;
            // 
            // btXmlCheck
            // 
            this.btXmlCheck.Location = new System.Drawing.Point(6, 571);
            this.btXmlCheck.Name = "btXmlCheck";
            this.btXmlCheck.Size = new System.Drawing.Size(1290, 23);
            this.btXmlCheck.TabIndex = 2;
            this.btXmlCheck.Text = "btXmlCheck";
            this.btXmlCheck.UseVisualStyleBackColor = true;
            this.btXmlCheck.Click += new System.EventHandler(this.btXmlCheck_Click);
            // 
            // tbXmlContent
            // 
            this.tbXmlContent.Location = new System.Drawing.Point(6, 425);
            this.tbXmlContent.Multiline = true;
            this.tbXmlContent.Name = "tbXmlContent";
            this.tbXmlContent.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.tbXmlContent.Size = new System.Drawing.Size(1290, 140);
            this.tbXmlContent.TabIndex = 1;
            this.tbXmlContent.WordWrap = false;
            // 
            // dgXml
            // 
            this.dgXml.AllowUserToAddRows = false;
            this.dgXml.AllowUserToDeleteRows = false;
            this.dgXml.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgXml.Location = new System.Drawing.Point(6, 6);
            this.dgXml.Name = "dgXml";
            this.dgXml.ReadOnly = true;
            this.dgXml.RowTemplate.Height = 23;
            this.dgXml.Size = new System.Drawing.Size(1290, 386);
            this.dgXml.TabIndex = 0;
            // 
            // Form1
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1334, 650);
            this.Controls.Add(this.tbMain);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "RegexerV4";
            this.TopMost = true;
            this.tbMain.ResumeLayout(false);
            this.tpRegex.ResumeLayout(false);
            this.tpRegex.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgRegexReplace)).EndInit();
            this.tpTxt.ResumeLayout(false);
            this.tpTxt.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgTxt)).EndInit();
            this.tpXml.ResumeLayout(false);
            this.tpXml.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgXml)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tbMain;
        private System.Windows.Forms.TabPage tpRegex;
        private System.Windows.Forms.TabPage tpTxt;
        private System.Windows.Forms.TabPage tpXml;
        private System.Windows.Forms.ComboBox cbRegexFormula;
        private System.Windows.Forms.TextBox tbRegexOutput;
        private System.Windows.Forms.TextBox tbRegexInput;
        private System.Windows.Forms.Button btRegexGo;
        private System.Windows.Forms.DataGridView dgTxt;
        private System.Windows.Forms.TextBox tbTxtContent;
        private System.Windows.Forms.Button btTxtCheck;
        private System.Windows.Forms.Button btXmlCheck;
        private System.Windows.Forms.TextBox tbXmlContent;
        private System.Windows.Forms.DataGridView dgXml;
        private System.Windows.Forms.TextBox tbXmlDirectory;
        private System.Windows.Forms.TextBox tbTxtStarts;
        private System.Windows.Forms.TextBox tbTxtSeq;
        private System.Windows.Forms.Button btRegexReplace;
        private System.Windows.Forms.DataGridView dgRegexReplace;
        private System.Windows.Forms.DataGridViewTextBoxColumn From;
        private System.Windows.Forms.DataGridViewTextBoxColumn To;
        private System.Windows.Forms.TextBox tbRegexDelimeter;
        private System.Windows.Forms.Label lbRegexDelimeter;
        private System.Windows.Forms.Button btRegexRedo;
        private System.Windows.Forms.Button btRegexUndo;
    }
}

