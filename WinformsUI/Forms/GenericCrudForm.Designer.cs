namespace WinformsUI.Forms
{
    partial class GenericCrudForm
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
            this.pnlForRibonColorization = new System.Windows.Forms.Panel();
            this.ribbonTLP = new System.Windows.Forms.TableLayoutPanel();
            this.label2 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.eyeRestRibbon = new WinformsUI.UserControls.Ribbon.EyeRestRibbon();
            this.toolStrip2 = new System.Windows.Forms.ToolStrip();
            this.DGVFunctionsControl = new WinformsUI.UserControls.CustomDGV.CustomDgvRibbon();
            this.communicationFunctions1 = new WinformsUI.UserControls.Ribbon.CommunicationRibbon();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.btnCreate = new System.Windows.Forms.ToolStripButton();
            this.btnDelete = new System.Windows.Forms.ToolStripButton();
            this.btnRefresh = new System.Windows.Forms.ToolStripButton();
            this.btnUpdate = new System.Windows.Forms.ToolStripButton();
            this.label5 = new System.Windows.Forms.Label();
            this.rjButton1 = new WinformsUI.UserControls.RJButton();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.pnlForRibonColorization.SuspendLayout();
            this.ribbonTLP.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlForRibonColorization
            // 
            this.pnlForRibonColorization.Controls.Add(this.ribbonTLP);
            this.pnlForRibonColorization.Location = new System.Drawing.Point(6, 40);
            this.pnlForRibonColorization.Name = "pnlForRibonColorization";
            this.pnlForRibonColorization.Size = new System.Drawing.Size(1348, 108);
            this.pnlForRibonColorization.TabIndex = 0;
            // 
            // ribbonTLP
            // 
            this.ribbonTLP.BackColor = System.Drawing.Color.Transparent;
            this.ribbonTLP.ColumnCount = 6;
            this.ribbonTLP.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.ribbonTLP.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.ribbonTLP.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.ribbonTLP.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.ribbonTLP.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.ribbonTLP.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.ribbonTLP.Controls.Add(this.label2, 2, 1);
            this.ribbonTLP.Controls.Add(this.label4, 1, 1);
            this.ribbonTLP.Controls.Add(this.label1, 0, 1);
            this.ribbonTLP.Controls.Add(this.button2, 5, 0);
            this.ribbonTLP.Controls.Add(this.eyeRestRibbon, 4, 0);
            this.ribbonTLP.Controls.Add(this.toolStrip2, 3, 0);
            this.ribbonTLP.Controls.Add(this.DGVFunctionsControl, 2, 0);
            this.ribbonTLP.Controls.Add(this.communicationFunctions1, 1, 0);
            this.ribbonTLP.Controls.Add(this.toolStrip1, 0, 0);
            this.ribbonTLP.Controls.Add(this.label5, 4, 1);
            this.ribbonTLP.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ribbonTLP.Location = new System.Drawing.Point(0, 0);
            this.ribbonTLP.Name = "ribbonTLP";
            this.ribbonTLP.RowCount = 2;
            this.ribbonTLP.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.ribbonTLP.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.ribbonTLP.Size = new System.Drawing.Size(1348, 108);
            this.ribbonTLP.TabIndex = 37;
            this.ribbonTLP.Tag = "NonPaintable";
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label2.Font = new System.Drawing.Font("Microsoft YaHei UI", 8F);
            this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(24)))), ((int)(((byte)(49)))), ((int)(((byte)(83)))));
            this.label2.Location = new System.Drawing.Point(533, 86);
            this.label2.Margin = new System.Windows.Forms.Padding(0);
            this.label2.Name = "label2";
            this.label2.Padding = new System.Windows.Forms.Padding(0, 4, 0, 0);
            this.label2.Size = new System.Drawing.Size(361, 20);
            this.label2.TabIndex = 49;
            this.label2.Tag = "NonPaintable";
            this.label2.Text = "Apariencia";
            this.label2.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label4.Font = new System.Drawing.Font("Microsoft YaHei UI", 8F);
            this.label4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(24)))), ((int)(((byte)(49)))), ((int)(((byte)(83)))));
            this.label4.Location = new System.Drawing.Point(346, 86);
            this.label4.Margin = new System.Windows.Forms.Padding(0);
            this.label4.Name = "label4";
            this.label4.Padding = new System.Windows.Forms.Padding(0, 4, 0, 0);
            this.label4.Size = new System.Drawing.Size(187, 20);
            this.label4.TabIndex = 48;
            this.label4.Tag = "NonPaintable";
            this.label4.Text = "Contactar";
            this.label4.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label1.Font = new System.Drawing.Font("Microsoft YaHei UI", 8F);
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(24)))), ((int)(((byte)(49)))), ((int)(((byte)(83)))));
            this.label1.Location = new System.Drawing.Point(5, 86);
            this.label1.Margin = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.label1.Name = "label1";
            this.label1.Padding = new System.Windows.Forms.Padding(0, 4, 0, 0);
            this.label1.Size = new System.Drawing.Size(341, 20);
            this.label1.TabIndex = 47;
            this.label1.Tag = "NonPaintable";
            this.label1.Text = "Gestionar";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // button2
            // 
            this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button2.BackColor = System.Drawing.Color.Transparent;
            this.button2.FlatAppearance.BorderSize = 0;
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button2.Location = new System.Drawing.Point(1302, 39);
            this.button2.Margin = new System.Windows.Forms.Padding(0);
            this.button2.MaximumSize = new System.Drawing.Size(46, 46);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(46, 46);
            this.button2.TabIndex = 46;
            this.button2.Tag = "NonPaintable";
            this.button2.Text = "▲";
            this.button2.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.button2.UseVisualStyleBackColor = false;
            // 
            // eyeRestRibbon
            // 
            this.eyeRestRibbon.AutoSize = true;
            this.eyeRestRibbon.Location = new System.Drawing.Point(999, 3);
            this.eyeRestRibbon.Margin = new System.Windows.Forms.Padding(0, 3, 0, 0);
            this.eyeRestRibbon.Name = "eyeRestRibbon";
            this.eyeRestRibbon.Size = new System.Drawing.Size(277, 73);
            this.eyeRestRibbon.TabIndex = 45;
            // 
            // toolStrip2
            // 
            this.toolStrip2.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.toolStrip2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStrip2.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip2.Location = new System.Drawing.Point(894, 3);
            this.toolStrip2.Margin = new System.Windows.Forms.Padding(0, 3, 3, 0);
            this.toolStrip2.Name = "toolStrip2";
            this.toolStrip2.Padding = new System.Windows.Forms.Padding(0, 10, 0, 0);
            this.toolStrip2.Size = new System.Drawing.Size(102, 82);
            this.toolStrip2.TabIndex = 44;
            this.toolStrip2.Text = "toolStrip2";
            // 
            // DGVFunctionsControl
            // 
            this.DGVFunctionsControl.AutoSize = true;
            this.DGVFunctionsControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DGVFunctionsControl.Location = new System.Drawing.Point(533, 3);
            this.DGVFunctionsControl.Margin = new System.Windows.Forms.Padding(0, 3, 3, 0);
            this.DGVFunctionsControl.Name = "DGVFunctionsControl";
            this.DGVFunctionsControl.Size = new System.Drawing.Size(358, 82);
            this.DGVFunctionsControl.TabIndex = 36;
            this.DGVFunctionsControl.Tag = "NonPaintable";
            this.DGVFunctionsControl.TargetDGV = null;
            // 
            // communicationFunctions1
            // 
            this.communicationFunctions1.AutoSize = true;
            this.communicationFunctions1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.communicationFunctions1.Location = new System.Drawing.Point(346, 3);
            this.communicationFunctions1.Margin = new System.Windows.Forms.Padding(0, 3, 3, 0);
            this.communicationFunctions1.Name = "communicationFunctions1";
            this.communicationFunctions1.Size = new System.Drawing.Size(184, 82);
            this.communicationFunctions1.TabIndex = 35;
            // 
            // toolStrip1
            // 
            this.toolStrip1.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnCreate,
            this.btnDelete,
            this.btnRefresh,
            this.btnUpdate});
            this.toolStrip1.Location = new System.Drawing.Point(3, 3);
            this.toolStrip1.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Padding = new System.Windows.Forms.Padding(0, 10, 0, 0);
            this.toolStrip1.Size = new System.Drawing.Size(340, 82);
            this.toolStrip1.TabIndex = 34;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // btnCreate
            // 
            this.btnCreate.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnCreate.Font = new System.Drawing.Font("Microsoft YaHei UI", 8F);
            this.btnCreate.Image = global::WinformsUI.Properties.Resources.BigIconAddUser;
            this.btnCreate.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnCreate.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.btnCreate.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnCreate.Margin = new System.Windows.Forms.Padding(0, -10, 0, 0);
            this.btnCreate.Name = "btnCreate";
            this.btnCreate.Padding = new System.Windows.Forms.Padding(15, 0, 15, 0);
            this.btnCreate.Size = new System.Drawing.Size(76, 82);
            this.btnCreate.Text = "Añadir";
            this.btnCreate.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnCreate.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            // 
            // btnDelete
            // 
            this.btnDelete.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnDelete.Font = new System.Drawing.Font("Microsoft YaHei UI", 8F);
            this.btnDelete.Image = global::WinformsUI.Properties.Resources.BigIconDeleteUser;
            this.btnDelete.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnDelete.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.btnDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnDelete.Margin = new System.Windows.Forms.Padding(0, -10, 0, 0);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Padding = new System.Windows.Forms.Padding(15, 0, 15, 0);
            this.btnDelete.Size = new System.Drawing.Size(83, 82);
            this.btnDelete.Text = "Eliminar";
            this.btnDelete.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnDelete.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            // 
            // btnRefresh
            // 
            this.btnRefresh.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnRefresh.Font = new System.Drawing.Font("Microsoft YaHei UI", 8F);
            this.btnRefresh.Image = global::WinformsUI.Properties.Resources.BigDBIcon;
            this.btnRefresh.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnRefresh.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.btnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnRefresh.Margin = new System.Windows.Forms.Padding(0, -10, 0, 0);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Padding = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.btnRefresh.Size = new System.Drawing.Size(87, 82);
            this.btnRefresh.Text = "Refrescar lista";
            this.btnRefresh.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnRefresh.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            // 
            // btnUpdate
            // 
            this.btnUpdate.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnUpdate.Font = new System.Drawing.Font("Microsoft YaHei UI", 8F);
            this.btnUpdate.Image = global::WinformsUI.Properties.Resources.BigaIconEditar;
            this.btnUpdate.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnUpdate.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.btnUpdate.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnUpdate.Margin = new System.Windows.Forms.Padding(0, -10, 0, 0);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Padding = new System.Windows.Forms.Padding(15, 0, 15, 0);
            this.btnUpdate.Size = new System.Drawing.Size(92, 82);
            this.btnUpdate.Text = "Modificar";
            this.btnUpdate.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnUpdate.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label5.Font = new System.Drawing.Font("Microsoft YaHei UI", 8F);
            this.label5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(24)))), ((int)(((byte)(49)))), ((int)(((byte)(83)))));
            this.label5.Location = new System.Drawing.Point(999, 86);
            this.label5.Margin = new System.Windows.Forms.Padding(0, 0, 5, 0);
            this.label5.Name = "label5";
            this.label5.Padding = new System.Windows.Forms.Padding(0, 4, 0, 0);
            this.label5.Size = new System.Drawing.Size(272, 20);
            this.label5.TabIndex = 50;
            this.label5.Tag = "NonPaintable";
            this.label5.Text = "Modo descanso";
            this.label5.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // rjButton1
            // 
            this.rjButton1.BackColor = System.Drawing.Color.MediumSlateBlue;
            this.rjButton1.BackgroundColor = System.Drawing.Color.MediumSlateBlue;
            this.rjButton1.BorderColor = System.Drawing.Color.PaleVioletRed;
            this.rjButton1.BorderRadius = 0;
            this.rjButton1.BorderSize = 0;
            this.rjButton1.FlatAppearance.BorderSize = 0;
            this.rjButton1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rjButton1.ForeColor = System.Drawing.Color.White;
            this.rjButton1.Location = new System.Drawing.Point(267, 405);
            this.rjButton1.Name = "rjButton1";
            this.rjButton1.Size = new System.Drawing.Size(150, 40);
            this.rjButton1.TabIndex = 1;
            this.rjButton1.Text = "rjButton1";
            this.rjButton1.TextColor = System.Drawing.Color.White;
            this.rjButton1.UseVisualStyleBackColor = false;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(43, 12);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.Padding = new System.Drawing.Point(0, 0);
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1422, 211);
            this.tabControl1.TabIndex = 2;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.pnlForRibonColorization);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(1414, 185);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "tabPage1";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(283, 185);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "tabPage2";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // GenericCrudForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1474, 659);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.rjButton1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "GenericCrudForm";
            this.Text = "GenericCrudForm";
            this.pnlForRibonColorization.ResumeLayout(false);
            this.ribbonTLP.ResumeLayout(false);
            this.ribbonTLP.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlForRibonColorization;
        private System.Windows.Forms.TableLayoutPanel ribbonTLP;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button2;
        private UserControls.Ribbon.EyeRestRibbon eyeRestRibbon;
        private System.Windows.Forms.ToolStrip toolStrip2;
        private UserControls.CustomDGV.CustomDgvRibbon DGVFunctionsControl;
        private UserControls.Ribbon.CommunicationRibbon communicationFunctions1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton btnCreate;
        private System.Windows.Forms.ToolStripButton btnDelete;
        private System.Windows.Forms.ToolStripButton btnRefresh;
        private System.Windows.Forms.ToolStripButton btnUpdate;
        private System.Windows.Forms.Label label5;
        private UserControls.RJButton rjButton1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
    }
}