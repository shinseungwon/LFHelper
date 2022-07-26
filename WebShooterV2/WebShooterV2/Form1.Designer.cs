
namespace WebShooterV2
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
            this.tbUrl = new System.Windows.Forms.TextBox();
            this.tbHeader = new System.Windows.Forms.TextBox();
            this.tbBody = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.lvData = new System.Windows.Forms.ListView();
            this.Line = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Status = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tbData = new System.Windows.Forms.TextBox();
            this.bApply = new System.Windows.Forms.Button();
            this.lvResult = new System.Windows.Forms.ListView();
            this.Title = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Result = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tbResultDetail = new System.Windows.Forms.TextBox();
            this.tbResultLike = new System.Windows.Forms.TextBox();
            this.bExport = new System.Windows.Forms.Button();
            this.bLoad = new System.Windows.Forms.Button();
            this.lvTemplate = new System.Windows.Forms.ListView();
            this.Template = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SuspendLayout();
            // 
            // tbUrl
            // 
            this.tbUrl.Location = new System.Drawing.Point(432, 12);
            this.tbUrl.Name = "tbUrl";
            this.tbUrl.Size = new System.Drawing.Size(442, 21);
            this.tbUrl.TabIndex = 0;
            // 
            // tbHeader
            // 
            this.tbHeader.Location = new System.Drawing.Point(432, 39);
            this.tbHeader.Multiline = true;
            this.tbHeader.Name = "tbHeader";
            this.tbHeader.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.tbHeader.Size = new System.Drawing.Size(442, 96);
            this.tbHeader.TabIndex = 1;
            // 
            // tbBody
            // 
            this.tbBody.Location = new System.Drawing.Point(432, 141);
            this.tbBody.Multiline = true;
            this.tbBody.Name = "tbBody";
            this.tbBody.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.tbBody.Size = new System.Drawing.Size(442, 288);
            this.tbBody.TabIndex = 2;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(432, 654);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(442, 23);
            this.button1.TabIndex = 3;
            this.button1.Text = "Go";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.bGo_Click);
            // 
            // lvData
            // 
            this.lvData.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.Line,
            this.Status});
            this.lvData.HideSelection = false;
            this.lvData.Location = new System.Drawing.Point(12, 12);
            this.lvData.Name = "lvData";
            this.lvData.Size = new System.Drawing.Size(414, 417);
            this.lvData.TabIndex = 4;
            this.lvData.UseCompatibleStateImageBehavior = false;
            this.lvData.View = System.Windows.Forms.View.Details;
            // 
            // Line
            // 
            this.Line.Text = "Line";
            this.Line.Width = 300;
            // 
            // Status
            // 
            this.Status.Text = "Status";
            this.Status.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.Status.Width = 100;
            // 
            // tbData
            // 
            this.tbData.Location = new System.Drawing.Point(12, 436);
            this.tbData.Multiline = true;
            this.tbData.Name = "tbData";
            this.tbData.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.tbData.Size = new System.Drawing.Size(414, 212);
            this.tbData.TabIndex = 6;
            // 
            // bApply
            // 
            this.bApply.Location = new System.Drawing.Point(12, 654);
            this.bApply.Name = "bApply";
            this.bApply.Size = new System.Drawing.Size(414, 23);
            this.bApply.TabIndex = 7;
            this.bApply.Text = "Apply";
            this.bApply.UseVisualStyleBackColor = true;
            this.bApply.Click += new System.EventHandler(this.bApply_Click);
            // 
            // lvResult
            // 
            this.lvResult.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.Title,
            this.Result});
            this.lvResult.HideSelection = false;
            this.lvResult.Location = new System.Drawing.Point(880, 12);
            this.lvResult.Name = "lvResult";
            this.lvResult.Size = new System.Drawing.Size(458, 417);
            this.lvResult.TabIndex = 8;
            this.lvResult.UseCompatibleStateImageBehavior = false;
            this.lvResult.View = System.Windows.Forms.View.Details;
            // 
            // Title
            // 
            this.Title.Text = "Title";
            this.Title.Width = 200;
            // 
            // Result
            // 
            this.Result.Text = "Result";
            this.Result.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.Result.Width = 200;
            // 
            // tbResultDetail
            // 
            this.tbResultDetail.Location = new System.Drawing.Point(880, 436);
            this.tbResultDetail.Multiline = true;
            this.tbResultDetail.Name = "tbResultDetail";
            this.tbResultDetail.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.tbResultDetail.Size = new System.Drawing.Size(458, 183);
            this.tbResultDetail.TabIndex = 9;
            // 
            // tbResultLike
            // 
            this.tbResultLike.Location = new System.Drawing.Point(880, 627);
            this.tbResultLike.Name = "tbResultLike";
            this.tbResultLike.Size = new System.Drawing.Size(458, 21);
            this.tbResultLike.TabIndex = 10;
            // 
            // bExport
            // 
            this.bExport.Location = new System.Drawing.Point(880, 654);
            this.bExport.Name = "bExport";
            this.bExport.Size = new System.Drawing.Size(458, 23);
            this.bExport.TabIndex = 11;
            this.bExport.Text = "Export";
            this.bExport.UseVisualStyleBackColor = true;
            this.bExport.Click += new System.EventHandler(this.bExport_Click);
            // 
            // bLoad
            // 
            this.bLoad.Location = new System.Drawing.Point(432, 625);
            this.bLoad.Name = "bLoad";
            this.bLoad.Size = new System.Drawing.Size(442, 23);
            this.bLoad.TabIndex = 12;
            this.bLoad.Text = "Load";
            this.bLoad.UseVisualStyleBackColor = true;
            this.bLoad.Click += new System.EventHandler(this.bLoad_Click);
            // 
            // lvTemplate
            // 
            this.lvTemplate.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.Template});
            this.lvTemplate.HideSelection = false;
            this.lvTemplate.Location = new System.Drawing.Point(432, 436);
            this.lvTemplate.Name = "lvTemplate";
            this.lvTemplate.Size = new System.Drawing.Size(442, 183);
            this.lvTemplate.TabIndex = 13;
            this.lvTemplate.UseCompatibleStateImageBehavior = false;
            this.lvTemplate.View = System.Windows.Forms.View.Details;
            this.lvTemplate.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lvTemplate_MouseDoubleClick);
            // 
            // Template
            // 
            this.Template.Text = "TemplateName";
            this.Template.Width = 400;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1350, 689);
            this.Controls.Add(this.lvTemplate);
            this.Controls.Add(this.bLoad);
            this.Controls.Add(this.bExport);
            this.Controls.Add(this.tbResultLike);
            this.Controls.Add(this.tbResultDetail);
            this.Controls.Add(this.lvResult);
            this.Controls.Add(this.bApply);
            this.Controls.Add(this.tbData);
            this.Controls.Add(this.lvData);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.tbBody);
            this.Controls.Add(this.tbHeader);
            this.Controls.Add(this.tbUrl);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "WebShooterV2";
            this.TopMost = true;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tbUrl;
        private System.Windows.Forms.TextBox tbHeader;
        private System.Windows.Forms.TextBox tbBody;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ListView lvData;
        private System.Windows.Forms.ColumnHeader Line;
        private System.Windows.Forms.ColumnHeader Status;
        private System.Windows.Forms.TextBox tbData;
        private System.Windows.Forms.Button bApply;
        private System.Windows.Forms.ListView lvResult;
        private System.Windows.Forms.ColumnHeader Title;
        private System.Windows.Forms.ColumnHeader Result;
        private System.Windows.Forms.TextBox tbResultDetail;
        private System.Windows.Forms.TextBox tbResultLike;
        private System.Windows.Forms.Button bExport;
        private System.Windows.Forms.Button bLoad;
        private System.Windows.Forms.ListView lvTemplate;
        private System.Windows.Forms.ColumnHeader Template;
    }
}

