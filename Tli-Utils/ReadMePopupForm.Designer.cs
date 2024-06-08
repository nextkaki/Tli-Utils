namespace Tli_Utils
{
    partial class ReadMePopupForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ReadMePopupForm));
            this.btnClosePopup = new MetroFramework.Controls.MetroTile();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.txtReadMe = new System.Windows.Forms.TextBox();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnClosePopup
            // 
            this.btnClosePopup.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnClosePopup.Location = new System.Drawing.Point(3, 559);
            this.btnClosePopup.Name = "btnClosePopup";
            this.btnClosePopup.Size = new System.Drawing.Size(724, 53);
            this.btnClosePopup.TabIndex = 0;
            this.btnClosePopup.Text = "닫 기";
            this.btnClosePopup.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnClosePopup.TileImageAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnClosePopup.TileTextFontSize = MetroFramework.MetroTileTextSize.Tall;
            this.btnClosePopup.TileTextFontWeight = MetroFramework.MetroTileTextWeight.Bold;
            this.btnClosePopup.Click += new System.EventHandler(this.btnClosePopup_Click);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.btnClosePopup, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.txtReadMe, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 90.4065F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 9.593496F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(730, 615);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // txtReadMe
            // 
            this.txtReadMe.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtReadMe.Font = new System.Drawing.Font("나눔고딕", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtReadMe.Location = new System.Drawing.Point(3, 3);
            this.txtReadMe.Multiline = true;
            this.txtReadMe.Name = "txtReadMe";
            this.txtReadMe.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtReadMe.Size = new System.Drawing.Size(724, 550);
            this.txtReadMe.TabIndex = 1;
            this.txtReadMe.Text = "가나다라마바사아자차카타파하\r\n\r\n  테스트\r\n   테스트";
            // 
            // ReadMePopupForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(730, 615);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ReadMePopupForm";
            this.Text = "Notice";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private MetroFramework.Controls.MetroTile btnClosePopup;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TextBox txtReadMe;
    }
}