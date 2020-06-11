namespace V2RayW
{
    partial class FormSubsSetting
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.buttonAddSub = new System.Windows.Forms.Button();
            this.textBoxSubUrl = new System.Windows.Forms.TextBox();
            this.listBoxSubs = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxRemark = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.buttonClose = new System.Windows.Forms.Button();
            this.buttonRemove = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // buttonAddSub
            // 
            this.buttonAddSub.Location = new System.Drawing.Point(345, 320);
            this.buttonAddSub.Name = "buttonAddSub";
            this.buttonAddSub.Size = new System.Drawing.Size(75, 23);
            this.buttonAddSub.TabIndex = 2;
            this.buttonAddSub.Text = "Add";
            this.buttonAddSub.UseVisualStyleBackColor = true;
            this.buttonAddSub.Click += new System.EventHandler(this.buttonAddSub_Click);
            // 
            // textBoxSubUrl
            // 
            this.textBoxSubUrl.Location = new System.Drawing.Point(90, 283);
            this.textBoxSubUrl.Name = "textBoxSubUrl";
            this.textBoxSubUrl.Size = new System.Drawing.Size(429, 21);
            this.textBoxSubUrl.TabIndex = 4;
            this.textBoxSubUrl.TextChanged += new System.EventHandler(this.textBoxSubUrl_TextChanged);
            // 
            // listBoxSubs
            // 
            this.listBoxSubs.FormattingEnabled = true;
            this.listBoxSubs.HorizontalScrollbar = true;
            this.listBoxSubs.ItemHeight = 12;
            this.listBoxSubs.Location = new System.Drawing.Point(25, 12);
            this.listBoxSubs.Name = "listBoxSubs";
            this.listBoxSubs.Size = new System.Drawing.Size(494, 256);
            this.listBoxSubs.TabIndex = 5;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(23, 286);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 12);
            this.label1.TabIndex = 6;
            this.label1.Text = "URL:";
            // 
            // textBoxRemark
            // 
            this.textBoxRemark.Location = new System.Drawing.Point(90, 321);
            this.textBoxRemark.Name = "textBoxRemark";
            this.textBoxRemark.Size = new System.Drawing.Size(218, 21);
            this.textBoxRemark.TabIndex = 7;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(23, 325);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(47, 12);
            this.label2.TabIndex = 8;
            this.label2.Text = "Remark:";
            // 
            // buttonClose
            // 
            this.buttonClose.Location = new System.Drawing.Point(444, 373);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(75, 23);
            this.buttonClose.TabIndex = 9;
            this.buttonClose.Text = "Close";
            this.buttonClose.UseVisualStyleBackColor = true;
            this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // buttonRemove
            // 
            this.buttonRemove.Location = new System.Drawing.Point(444, 320);
            this.buttonRemove.Name = "buttonRemove";
            this.buttonRemove.Size = new System.Drawing.Size(75, 23);
            this.buttonRemove.TabIndex = 10;
            this.buttonRemove.Text = "Remove";
            this.buttonRemove.UseVisualStyleBackColor = true;
            this.buttonRemove.Click += new System.EventHandler(this.buttonRemove_Click);
            // 
            // FormSubsSetting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(542, 397);
            this.Controls.Add(this.buttonRemove);
            this.Controls.Add(this.buttonClose);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBoxRemark);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.listBoxSubs);
            this.Controls.Add(this.textBoxSubUrl);
            this.Controls.Add(this.buttonAddSub);
            this.Name = "FormSubsSetting";
            this.Text = "Subscription setting";
            this.Load += new System.EventHandler(this.FormSubsSetting_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonAddSub;
        private System.Windows.Forms.TextBox textBoxSubUrl;
        private System.Windows.Forms.ListBox listBoxSubs;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxRemark;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button buttonClose;
        private System.Windows.Forms.Button buttonRemove;
    }
}