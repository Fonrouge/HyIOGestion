namespace WinformsUI.Forms.Base
{
    partial class HostForm
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
            this.internalContainer = new System.Windows.Forms.Panel();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnExpand = new System.Windows.Forms.Button();
            this.btnMinimize = new System.Windows.Forms.Button();
            this.lblTitle = new System.Windows.Forms.Label();
            this.tlpTitleBar = new System.Windows.Forms.TableLayoutPanel();
            this.tlpTitleBar.SuspendLayout();
            this.SuspendLayout();
            // 
            // internalContainer
            // 
            this.internalContainer.BackColor = System.Drawing.Color.RosyBrown;
            this.internalContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.internalContainer.Location = new System.Drawing.Point(0, 31);
            this.internalContainer.Name = "internalContainer";
            this.internalContainer.Padding = new System.Windows.Forms.Padding(3);
            this.internalContainer.Size = new System.Drawing.Size(1057, 511);
            this.internalContainer.TabIndex = 7;
            this.internalContainer.Tag = "";
            // 
            // btnClose
            // 
            this.btnClose.BackColor = System.Drawing.Color.Transparent;
            this.btnClose.BackgroundImage = global::WinformsUI.Properties.Resources.btnCloseModern;
            this.btnClose.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnClose.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnClose.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnClose.FlatAppearance.BorderSize = 0;
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.Location = new System.Drawing.Point(1025, 2);
            this.btnClose.Margin = new System.Windows.Forms.Padding(2);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(30, 27);
            this.btnClose.TabIndex = 0;
            this.btnClose.Tag = "NonPaintable";
            this.btnClose.UseVisualStyleBackColor = false;
            // 
            // btnExpand
            // 
            this.btnExpand.BackColor = System.Drawing.Color.Transparent;
            this.btnExpand.BackgroundImage = global::WinformsUI.Properties.Resources.btnRestoreNew;
            this.btnExpand.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnExpand.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnExpand.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnExpand.FlatAppearance.BorderSize = 0;
            this.btnExpand.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExpand.Location = new System.Drawing.Point(991, 2);
            this.btnExpand.Margin = new System.Windows.Forms.Padding(2);
            this.btnExpand.Name = "btnExpand";
            this.btnExpand.Size = new System.Drawing.Size(30, 27);
            this.btnExpand.TabIndex = 0;
            this.btnExpand.Tag = "NonPaintable";
            this.btnExpand.UseVisualStyleBackColor = false;
            // 
            // btnMinimize
            // 
            this.btnMinimize.BackColor = System.Drawing.Color.Transparent;
            this.btnMinimize.BackgroundImage = global::WinformsUI.Properties.Resources.btnMinimizeNew;
            this.btnMinimize.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnMinimize.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnMinimize.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnMinimize.FlatAppearance.BorderSize = 0;
            this.btnMinimize.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMinimize.Location = new System.Drawing.Point(957, 2);
            this.btnMinimize.Margin = new System.Windows.Forms.Padding(2);
            this.btnMinimize.Name = "btnMinimize";
            this.btnMinimize.Size = new System.Drawing.Size(30, 27);
            this.btnMinimize.TabIndex = 0;
            this.btnMinimize.Tag = "NonPaintable";
            this.btnMinimize.UseVisualStyleBackColor = false;
            // 
            // lblTitle
            // 
            this.lblTitle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Cambria", 9.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.Location = new System.Drawing.Point(4, 0);
            this.lblTitle.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(46, 31);
            this.lblTitle.TabIndex = 6;
            this.lblTitle.Tag = "InternalTitleBar";
            this.lblTitle.Text = "Título ";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tlpTitleBar
            // 
            this.tlpTitleBar.BackColor = System.Drawing.Color.IndianRed;
            this.tlpTitleBar.ColumnCount = 4;
            this.tlpTitleBar.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpTitleBar.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 34F));
            this.tlpTitleBar.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 34F));
            this.tlpTitleBar.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 34F));
            this.tlpTitleBar.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpTitleBar.Controls.Add(this.lblTitle, 0, 0);
            this.tlpTitleBar.Controls.Add(this.btnMinimize, 1, 0);
            this.tlpTitleBar.Controls.Add(this.btnExpand, 2, 0);
            this.tlpTitleBar.Controls.Add(this.btnClose, 3, 0);
            this.tlpTitleBar.Dock = System.Windows.Forms.DockStyle.Top;
            this.tlpTitleBar.Location = new System.Drawing.Point(0, 0);
            this.tlpTitleBar.Margin = new System.Windows.Forms.Padding(0);
            this.tlpTitleBar.Name = "tlpTitleBar";
            this.tlpTitleBar.RowCount = 1;
            this.tlpTitleBar.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpTitleBar.Size = new System.Drawing.Size(1057, 31);
            this.tlpTitleBar.TabIndex = 12;
            this.tlpTitleBar.Tag = "InternalTitleBar";
            // 
            // HostForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1057, 542);
            this.Controls.Add(this.internalContainer);
            this.Controls.Add(this.tlpTitleBar);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "HostForm";
            this.Tag = "SubPanel";
            this.Text = "HostForm";
            this.tlpTitleBar.ResumeLayout(false);
            this.tlpTitleBar.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel internalContainer;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnExpand;
        private System.Windows.Forms.Button btnMinimize;
        private System.Windows.Forms.Label lblTitle;
        protected System.Windows.Forms.TableLayoutPanel tlpTitleBar;
    }
}