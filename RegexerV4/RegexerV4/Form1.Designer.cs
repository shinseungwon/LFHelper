
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
            this.tbTxt = new System.Windows.Forms.TabPage();
            this.tbXml = new System.Windows.Forms.TabPage();
            this.tbMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // tbMain
            // 
            this.tbMain.Controls.Add(this.tpRegex);
            this.tbMain.Controls.Add(this.tbTxt);
            this.tbMain.Controls.Add(this.tbXml);
            this.tbMain.Location = new System.Drawing.Point(12, 12);
            this.tbMain.Name = "tbMain";
            this.tbMain.SelectedIndex = 0;
            this.tbMain.Size = new System.Drawing.Size(1310, 626);
            this.tbMain.TabIndex = 0;
            // 
            // tpRegex
            // 
            this.tpRegex.Location = new System.Drawing.Point(4, 22);
            this.tpRegex.Name = "tpRegex";
            this.tpRegex.Padding = new System.Windows.Forms.Padding(3);
            this.tpRegex.Size = new System.Drawing.Size(1302, 600);
            this.tpRegex.TabIndex = 0;
            this.tpRegex.Text = "tpRegex";
            this.tpRegex.UseVisualStyleBackColor = true;
            // 
            // tbTxt
            // 
            this.tbTxt.Location = new System.Drawing.Point(4, 22);
            this.tbTxt.Name = "tbTxt";
            this.tbTxt.Padding = new System.Windows.Forms.Padding(3);
            this.tbTxt.Size = new System.Drawing.Size(1302, 600);
            this.tbTxt.TabIndex = 1;
            this.tbTxt.Text = "tbTxt";
            this.tbTxt.UseVisualStyleBackColor = true;
            // 
            // tbXml
            // 
            this.tbXml.Location = new System.Drawing.Point(4, 22);
            this.tbXml.Name = "tbXml";
            this.tbXml.Size = new System.Drawing.Size(1302, 600);
            this.tbXml.TabIndex = 2;
            this.tbXml.Text = "tbXml";
            this.tbXml.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1334, 650);
            this.Controls.Add(this.tbMain);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.Text = "Form1";
            this.tbMain.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tbMain;
        private System.Windows.Forms.TabPage tpRegex;
        private System.Windows.Forms.TabPage tbTxt;
        private System.Windows.Forms.TabPage tbXml;
    }
}

