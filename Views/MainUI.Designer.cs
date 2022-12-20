namespace NTPTimeUP
{
    partial class MainUI
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainUI));
            this.tbNTPUrl = new System.Windows.Forms.TextBox();
            this.btnSyncNTP = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // tbNTPUrl
            // 
            this.tbNTPUrl.BackColor = System.Drawing.Color.Black;
            this.tbNTPUrl.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbNTPUrl.ForeColor = System.Drawing.Color.White;
            this.tbNTPUrl.Location = new System.Drawing.Point(41, 46);
            this.tbNTPUrl.Name = "tbNTPUrl";
            this.tbNTPUrl.Size = new System.Drawing.Size(217, 23);
            this.tbNTPUrl.TabIndex = 0;
            this.tbNTPUrl.Text = "a.st1.ntp.br";
            this.tbNTPUrl.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // btnSyncNTP
            // 
            this.btnSyncNTP.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSyncNTP.Location = new System.Drawing.Point(41, 75);
            this.btnSyncNTP.Name = "btnSyncNTP";
            this.btnSyncNTP.Size = new System.Drawing.Size(217, 50);
            this.btnSyncNTP.TabIndex = 1;
            this.btnSyncNTP.Text = "Sincronizar com NTP";
            this.btnSyncNTP.UseVisualStyleBackColor = true;
            this.btnSyncNTP.Click += new System.EventHandler(this.BtnSyncNTP_Click);
            // 
            // MainUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(297, 177);
            this.Controls.Add(this.btnSyncNTP);
            this.Controls.Add(this.tbNTPUrl);
            this.ForeColor = System.Drawing.Color.White;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainUI";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "NTPTimeUP";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private TextBox tbNTPUrl;
        private Button btnSyncNTP;
    }
}