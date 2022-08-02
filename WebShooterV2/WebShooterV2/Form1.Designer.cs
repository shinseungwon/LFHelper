
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.tbUrl = new System.Windows.Forms.TextBox();
            this.tbHeader = new System.Windows.Forms.TextBox();
            this.tbBody = new System.Windows.Forms.TextBox();
            this.bGo = new System.Windows.Forms.Button();
            this.tbData = new System.Windows.Forms.TextBox();
            this.lvResult = new System.Windows.Forms.ListView();
            this.Request = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Response = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Status = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tbResponse = new System.Windows.Forms.TextBox();
            this.tbResultLike = new System.Windows.Forms.TextBox();
            this.bExport = new System.Windows.Forms.Button();
            this.bLoad = new System.Windows.Forms.Button();
            this.lvTemplate = new System.Windows.Forms.ListView();
            this.Template = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tbRequest = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // tbUrl
            // 
            this.tbUrl.Location = new System.Drawing.Point(305, 12);
            this.tbUrl.Name = "tbUrl";
            this.tbUrl.Size = new System.Drawing.Size(374, 21);
            this.tbUrl.TabIndex = 0;
            // 
            // tbHeader
            // 
            this.tbHeader.Location = new System.Drawing.Point(305, 39);
            this.tbHeader.Multiline = true;
            this.tbHeader.Name = "tbHeader";
            this.tbHeader.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.tbHeader.Size = new System.Drawing.Size(374, 96);
            this.tbHeader.TabIndex = 1;
            // 
            // tbBody
            // 
            this.tbBody.Location = new System.Drawing.Point(305, 141);
            this.tbBody.Multiline = true;
            this.tbBody.Name = "tbBody";
            this.tbBody.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.tbBody.Size = new System.Drawing.Size(374, 288);
            this.tbBody.TabIndex = 2;
            // 
            // bGo
            // 
            this.bGo.Location = new System.Drawing.Point(434, 653);
            this.bGo.Name = "bGo";
            this.bGo.Size = new System.Drawing.Size(112, 23);
            this.bGo.TabIndex = 3;
            this.bGo.Text = "Go";
            this.bGo.UseVisualStyleBackColor = true;
            this.bGo.Click += new System.EventHandler(this.bGo_Click);
            // 
            // tbData
            // 
            this.tbData.Location = new System.Drawing.Point(12, 12);
            this.tbData.Multiline = true;
            this.tbData.Name = "tbData";
            this.tbData.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.tbData.Size = new System.Drawing.Size(287, 665);
            this.tbData.TabIndex = 6;
            // 
            // lvResult
            // 
            this.lvResult.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.Request,
            this.Response,
            this.Status});
            this.lvResult.FullRowSelect = true;
            this.lvResult.HideSelection = false;
            this.lvResult.Location = new System.Drawing.Point(1034, 12);
            this.lvResult.MultiSelect = false;
            this.lvResult.Name = "lvResult";
            this.lvResult.Size = new System.Drawing.Size(304, 665);
            this.lvResult.TabIndex = 8;
            this.lvResult.UseCompatibleStateImageBehavior = false;
            this.lvResult.View = System.Windows.Forms.View.Details;
            this.lvResult.SelectedIndexChanged += new System.EventHandler(this.lvResult_SelectedIndexChanged);
            // 
            // Request
            // 
            this.Request.Text = "Request";
            this.Request.Width = 100;
            // 
            // Response
            // 
            this.Response.Text = "Response";
            this.Response.Width = 100;
            // 
            // Status
            // 
            this.Status.Text = "Status";
            this.Status.Width = 100;
            // 
            // tbResponse
            // 
            this.tbResponse.Location = new System.Drawing.Point(685, 337);
            this.tbResponse.Multiline = true;
            this.tbResponse.Name = "tbResponse";
            this.tbResponse.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.tbResponse.Size = new System.Drawing.Size(343, 339);
            this.tbResponse.TabIndex = 9;
            this.tbResponse.WordWrap = false;
            // 
            // tbResultLike
            // 
            this.tbResultLike.Location = new System.Drawing.Point(305, 627);
            this.tbResultLike.Name = "tbResultLike";
            this.tbResultLike.Size = new System.Drawing.Size(374, 21);
            this.tbResultLike.TabIndex = 10;
            this.tbResultLike.Text = "Success";
            // 
            // bExport
            // 
            this.bExport.Location = new System.Drawing.Point(552, 653);
            this.bExport.Name = "bExport";
            this.bExport.Size = new System.Drawing.Size(127, 23);
            this.bExport.TabIndex = 11;
            this.bExport.Text = "Export";
            this.bExport.UseVisualStyleBackColor = true;
            this.bExport.Click += new System.EventHandler(this.bExport_Click);
            // 
            // bLoad
            // 
            this.bLoad.Location = new System.Drawing.Point(305, 653);
            this.bLoad.Name = "bLoad";
            this.bLoad.Size = new System.Drawing.Size(123, 23);
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
            this.lvTemplate.Location = new System.Drawing.Point(305, 435);
            this.lvTemplate.Name = "lvTemplate";
            this.lvTemplate.Size = new System.Drawing.Size(374, 183);
            this.lvTemplate.TabIndex = 13;
            this.lvTemplate.UseCompatibleStateImageBehavior = false;
            this.lvTemplate.View = System.Windows.Forms.View.Details;
            this.lvTemplate.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lvTemplate_MouseDoubleClick);
            // 
            // Template
            // 
            this.Template.Text = "TemplateName";
            this.Template.Width = 500;
            // 
            // tbRequest
            // 
            this.tbRequest.Location = new System.Drawing.Point(685, 12);
            this.tbRequest.Multiline = true;
            this.tbRequest.Name = "tbRequest";
            this.tbRequest.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.tbRequest.Size = new System.Drawing.Size(343, 319);
            this.tbRequest.TabIndex = 14;
            this.tbRequest.WordWrap = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1350, 689);
            this.Controls.Add(this.tbRequest);
            this.Controls.Add(this.lvTemplate);
            this.Controls.Add(this.bLoad);
            this.Controls.Add(this.bExport);
            this.Controls.Add(this.tbResultLike);
            this.Controls.Add(this.tbResponse);
            this.Controls.Add(this.lvResult);
            this.Controls.Add(this.tbData);
            this.Controls.Add(this.bGo);
            this.Controls.Add(this.tbBody);
            this.Controls.Add(this.tbHeader);
            this.Controls.Add(this.tbUrl);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
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
        private System.Windows.Forms.Button bGo;
        private System.Windows.Forms.TextBox tbData;
        private System.Windows.Forms.ListView lvResult;
        private System.Windows.Forms.ColumnHeader Request;
        private System.Windows.Forms.ColumnHeader Response;
        private System.Windows.Forms.TextBox tbResponse;
        private System.Windows.Forms.TextBox tbResultLike;
        private System.Windows.Forms.Button bExport;
        private System.Windows.Forms.Button bLoad;
        private System.Windows.Forms.ListView lvTemplate;
        private System.Windows.Forms.ColumnHeader Template;
        private System.Windows.Forms.ColumnHeader Status;
        private System.Windows.Forms.TextBox tbRequest;
    }
}

