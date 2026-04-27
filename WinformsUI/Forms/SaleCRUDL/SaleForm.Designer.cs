namespace WinformsUI.Forms.SaleCRUDL
{
    partial class SaleForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.mainTLP = new System.Windows.Forms.TableLayoutPanel();
            this.pnlFeedbackBar = new System.Windows.Forms.Panel();
            this.pnlExpandedRibbons = new System.Windows.Forms.Panel();
            this.tlpExpandedRibbon = new System.Windows.Forms.TableLayoutPanel();
            this.label3 = new System.Windows.Forms.Label();
            this.toolStrip2 = new System.Windows.Forms.ToolStrip();
            this.ribbonEyeRest = new WinformsUI.UserControls.Ribbon.EyeRestRibbon();
            this.communicationFunctions1 = new WinformsUI.UserControls.Ribbon.CommunicationRibbon();
            this.ribbonDgvFunctions = new WinformsUI.UserControls.CustomDGV.CustomDgvRibbon();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.btnCreate = new System.Windows.Forms.ToolStripButton();
            this.btnDelete = new System.Windows.Forms.ToolStripButton();
            this.btnRefresh = new System.Windows.Forms.ToolStripButton();
            this.btnUpdate = new System.Windows.Forms.ToolStripButton();
            this.btnRibbonCollapser = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.ExportRibbon = new WinformsUI.UserControls.Ribbon.ExportRibbon();
            this.pnlCollapsedRibbons = new System.Windows.Forms.Panel();
            this.miniTLP = new System.Windows.Forms.TableLayoutPanel();
            this.collapsedRibbon = new System.Windows.Forms.ToolStrip();
            this.toolStripButton8 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton7 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton6 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
            this.toolStripSplitButton1 = new System.Windows.Forms.ToolStripSplitButton();
            this.fsdsfgdfgssdfgToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.asdasdasdToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripButton5 = new System.Windows.Forms.ToolStripButton();
            this.btnRibbonExpander = new System.Windows.Forms.Button();
            this.pnlDgv = new System.Windows.Forms.Panel();
            this.mainTLP.SuspendLayout();
            this.pnlExpandedRibbons.SuspendLayout();
            this.tlpExpandedRibbon.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.pnlCollapsedRibbons.SuspendLayout();
            this.miniTLP.SuspendLayout();
            this.collapsedRibbon.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainTLP
            // 
            this.mainTLP.ColumnCount = 1;
            this.mainTLP.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.mainTLP.Controls.Add(this.pnlFeedbackBar, 0, 2);
            this.mainTLP.Controls.Add(this.pnlExpandedRibbons, 0, 1);
            this.mainTLP.Controls.Add(this.pnlCollapsedRibbons, 0, 0);
            this.mainTLP.Controls.Add(this.pnlDgv, 0, 3);
            this.mainTLP.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainTLP.Location = new System.Drawing.Point(0, 0);
            this.mainTLP.Name = "mainTLP";
            this.mainTLP.RowCount = 4;
            this.mainTLP.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.mainTLP.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.mainTLP.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.mainTLP.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.mainTLP.Size = new System.Drawing.Size(1387, 638);
            this.mainTLP.TabIndex = 66;
            // 
            // pnlFeedbackBar
            // 
            this.pnlFeedbackBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlFeedbackBar.BackColor = System.Drawing.Color.LawnGreen;
            this.mainTLP.SetColumnSpan(this.pnlFeedbackBar, 6);
            this.pnlFeedbackBar.ForeColor = System.Drawing.Color.LawnGreen;
            this.pnlFeedbackBar.Location = new System.Drawing.Point(0, 159);
            this.pnlFeedbackBar.Margin = new System.Windows.Forms.Padding(0);
            this.pnlFeedbackBar.Name = "pnlFeedbackBar";
            this.pnlFeedbackBar.Size = new System.Drawing.Size(1387, 3);
            this.pnlFeedbackBar.TabIndex = 58;
            this.pnlFeedbackBar.Tag = "PanelAccentuable";
            // 
            // pnlExpandedRibbons
            // 
            this.pnlExpandedRibbons.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.pnlExpandedRibbons.BackColor = System.Drawing.Color.Transparent;
            this.pnlExpandedRibbons.Controls.Add(this.tlpExpandedRibbon);
            this.pnlExpandedRibbons.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlExpandedRibbons.Location = new System.Drawing.Point(0, 52);
            this.pnlExpandedRibbons.Margin = new System.Windows.Forms.Padding(0);
            this.pnlExpandedRibbons.Name = "pnlExpandedRibbons";
            this.pnlExpandedRibbons.Size = new System.Drawing.Size(1387, 107);
            this.pnlExpandedRibbons.TabIndex = 0;
            this.pnlExpandedRibbons.Tag = "";
            // 
            // tlpExpandedRibbon
            // 
            this.tlpExpandedRibbon.AutoSize = true;
            this.tlpExpandedRibbon.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tlpExpandedRibbon.BackColor = System.Drawing.Color.Transparent;
            this.tlpExpandedRibbon.ColumnCount = 7;
            this.tlpExpandedRibbon.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpExpandedRibbon.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpExpandedRibbon.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpExpandedRibbon.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpExpandedRibbon.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpExpandedRibbon.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpExpandedRibbon.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpExpandedRibbon.Controls.Add(this.label3, 3, 1);
            this.tlpExpandedRibbon.Controls.Add(this.toolStrip2, 4, 0);
            this.tlpExpandedRibbon.Controls.Add(this.ribbonEyeRest, 5, 0);
            this.tlpExpandedRibbon.Controls.Add(this.communicationFunctions1, 1, 0);
            this.tlpExpandedRibbon.Controls.Add(this.ribbonDgvFunctions, 2, 0);
            this.tlpExpandedRibbon.Controls.Add(this.toolStrip1, 0, 0);
            this.tlpExpandedRibbon.Controls.Add(this.btnRibbonCollapser, 6, 0);
            this.tlpExpandedRibbon.Controls.Add(this.label2, 2, 1);
            this.tlpExpandedRibbon.Controls.Add(this.label4, 1, 1);
            this.tlpExpandedRibbon.Controls.Add(this.label1, 0, 1);
            this.tlpExpandedRibbon.Controls.Add(this.label5, 5, 1);
            this.tlpExpandedRibbon.Controls.Add(this.ExportRibbon, 3, 0);
            this.tlpExpandedRibbon.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpExpandedRibbon.Location = new System.Drawing.Point(0, 0);
            this.tlpExpandedRibbon.Margin = new System.Windows.Forms.Padding(0);
            this.tlpExpandedRibbon.Name = "tlpExpandedRibbon";
            this.tlpExpandedRibbon.RowCount = 2;
            this.tlpExpandedRibbon.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 75F));
            this.tlpExpandedRibbon.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpExpandedRibbon.Size = new System.Drawing.Size(1387, 107);
            this.tlpExpandedRibbon.TabIndex = 66;
            this.tlpExpandedRibbon.Tag = "NonPaintable";
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label3.Font = new System.Drawing.Font("Microsoft YaHei UI", 7F);
            this.label3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(24)))), ((int)(((byte)(49)))), ((int)(((byte)(83)))));
            this.label3.Location = new System.Drawing.Point(878, 78);
            this.label3.Margin = new System.Windows.Forms.Padding(0, 0, 0, 5);
            this.label3.Name = "label3";
            this.label3.Padding = new System.Windows.Forms.Padding(0, 4, 0, 0);
            this.label3.Size = new System.Drawing.Size(338, 20);
            this.label3.TabIndex = 55;
            this.label3.Tag = "NonPaintable";
            this.label3.Text = "Exportar";
            this.label3.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // toolStrip2
            // 
            this.toolStrip2.AllowItemReorder = true;
            this.toolStrip2.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.toolStrip2.CanOverflow = false;
            this.toolStrip2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStrip2.Font = new System.Drawing.Font("Microsoft YaHei UI", 8F);
            this.toolStrip2.GripMargin = new System.Windows.Forms.Padding(0);
            this.toolStrip2.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip2.ImageScalingSize = new System.Drawing.Size(19, 19);
            this.toolStrip2.Location = new System.Drawing.Point(1216, 1);
            this.toolStrip2.Margin = new System.Windows.Forms.Padding(0, 1, 0, 0);
            this.toolStrip2.Name = "toolStrip2";
            this.toolStrip2.Padding = new System.Windows.Forms.Padding(0);
            this.toolStrip2.Size = new System.Drawing.Size(1, 74);
            this.toolStrip2.TabIndex = 53;
            this.toolStrip2.Text = "toolStrip2";
            // 
            // ribbonEyeRest
            // 
            this.ribbonEyeRest.AutoSize = true;
            this.ribbonEyeRest.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ribbonEyeRest.Font = new System.Drawing.Font("Microsoft YaHei UI", 8F);
            this.ribbonEyeRest.Location = new System.Drawing.Point(1061, 1);
            this.ribbonEyeRest.Margin = new System.Windows.Forms.Padding(1, 1, 3, 0);
            this.ribbonEyeRest.Name = "ribbonEyeRest";
            this.ribbonEyeRest.Size = new System.Drawing.Size(277, 73);
            this.ribbonEyeRest.TabIndex = 46;
            // 
            // communicationFunctions1
            // 
            this.communicationFunctions1.AutoSize = true;
            this.communicationFunctions1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.communicationFunctions1.Font = new System.Drawing.Font("Microsoft YaHei UI", 8F);
            this.communicationFunctions1.Location = new System.Drawing.Point(334, 1);
            this.communicationFunctions1.Margin = new System.Windows.Forms.Padding(0, 1, 0, 0);
            this.communicationFunctions1.Name = "communicationFunctions1";
            this.communicationFunctions1.Size = new System.Drawing.Size(184, 74);
            this.communicationFunctions1.TabIndex = 35;
            // 
            // ribbonDgvFunctions
            // 
            this.ribbonDgvFunctions.AutoSize = true;
            this.ribbonDgvFunctions.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ribbonDgvFunctions.Font = new System.Drawing.Font("Microsoft YaHei UI", 8F);
            this.ribbonDgvFunctions.Location = new System.Drawing.Point(519, 1);
            this.ribbonDgvFunctions.Margin = new System.Windows.Forms.Padding(1, 1, 1, 0);
            this.ribbonDgvFunctions.Name = "ribbonDgvFunctions";
            this.ribbonDgvFunctions.Size = new System.Drawing.Size(358, 74);
            this.ribbonDgvFunctions.TabIndex = 36;
            this.ribbonDgvFunctions.Tag = "";
            this.ribbonDgvFunctions.TargetDGV = null;
            // 
            // toolStrip1
            // 
            this.toolStrip1.AllowItemReorder = true;
            this.toolStrip1.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.toolStrip1.CanOverflow = false;
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStrip1.Font = new System.Drawing.Font("Microsoft YaHei UI", 8F);
            this.toolStrip1.GripMargin = new System.Windows.Forms.Padding(0);
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(19, 19);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnCreate,
            this.btnDelete,
            this.btnRefresh,
            this.btnUpdate});
            this.toolStrip1.Location = new System.Drawing.Point(3, 1);
            this.toolStrip1.Margin = new System.Windows.Forms.Padding(3, 1, 1, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Padding = new System.Windows.Forms.Padding(0);
            this.toolStrip1.Size = new System.Drawing.Size(330, 74);
            this.toolStrip1.TabIndex = 52;
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
            this.btnCreate.Margin = new System.Windows.Forms.Padding(0);
            this.btnCreate.Name = "btnCreate";
            this.btnCreate.Padding = new System.Windows.Forms.Padding(15, 0, 15, 0);
            this.btnCreate.Size = new System.Drawing.Size(76, 74);
            this.btnCreate.Text = "Añadir";
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
            this.btnDelete.Margin = new System.Windows.Forms.Padding(0);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Padding = new System.Windows.Forms.Padding(15, 0, 15, 0);
            this.btnDelete.Size = new System.Drawing.Size(83, 74);
            this.btnDelete.Text = "Eliminar";
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
            this.btnRefresh.Margin = new System.Windows.Forms.Padding(0);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Padding = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.btnRefresh.Size = new System.Drawing.Size(87, 74);
            this.btnRefresh.Text = "Refrescar lista";
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
            this.btnUpdate.Margin = new System.Windows.Forms.Padding(0);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Padding = new System.Windows.Forms.Padding(5, 0, 15, 0);
            this.btnUpdate.Size = new System.Drawing.Size(82, 74);
            this.btnUpdate.Text = "Modificar";
            this.btnUpdate.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            // 
            // btnRibbonCollapser
            // 
            this.btnRibbonCollapser.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRibbonCollapser.BackColor = System.Drawing.Color.Transparent;
            this.btnRibbonCollapser.FlatAppearance.BorderSize = 0;
            this.btnRibbonCollapser.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRibbonCollapser.Location = new System.Drawing.Point(1341, 32);
            this.btnRibbonCollapser.Margin = new System.Windows.Forms.Padding(0);
            this.btnRibbonCollapser.MaximumSize = new System.Drawing.Size(46, 46);
            this.btnRibbonCollapser.Name = "btnRibbonCollapser";
            this.btnRibbonCollapser.Size = new System.Drawing.Size(46, 43);
            this.btnRibbonCollapser.TabIndex = 47;
            this.btnRibbonCollapser.Tag = "NonPaintable";
            this.btnRibbonCollapser.Text = "▲";
            this.btnRibbonCollapser.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnRibbonCollapser.UseVisualStyleBackColor = false;
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label2.Font = new System.Drawing.Font("Microsoft YaHei UI", 7F);
            this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(24)))), ((int)(((byte)(49)))), ((int)(((byte)(83)))));
            this.label2.Location = new System.Drawing.Point(518, 78);
            this.label2.Margin = new System.Windows.Forms.Padding(0, 0, 0, 5);
            this.label2.Name = "label2";
            this.label2.Padding = new System.Windows.Forms.Padding(0, 4, 0, 0);
            this.label2.Size = new System.Drawing.Size(360, 20);
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
            this.label4.Font = new System.Drawing.Font("Microsoft YaHei UI", 7F);
            this.label4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(24)))), ((int)(((byte)(49)))), ((int)(((byte)(83)))));
            this.label4.Location = new System.Drawing.Point(334, 81);
            this.label4.Margin = new System.Windows.Forms.Padding(0);
            this.label4.Name = "label4";
            this.label4.Padding = new System.Windows.Forms.Padding(0, 4, 0, 0);
            this.label4.Size = new System.Drawing.Size(184, 20);
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
            this.label1.Font = new System.Drawing.Font("Microsoft YaHei UI", 7F);
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(24)))), ((int)(((byte)(49)))), ((int)(((byte)(83)))));
            this.label1.Location = new System.Drawing.Point(5, 81);
            this.label1.Margin = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.label1.Name = "label1";
            this.label1.Padding = new System.Windows.Forms.Padding(0, 4, 0, 0);
            this.label1.Size = new System.Drawing.Size(329, 20);
            this.label1.TabIndex = 47;
            this.label1.Tag = "NonPaintable";
            this.label1.Text = "Gestionar";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label5.Font = new System.Drawing.Font("Microsoft YaHei UI", 7F);
            this.label5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(24)))), ((int)(((byte)(49)))), ((int)(((byte)(83)))));
            this.label5.Location = new System.Drawing.Point(1060, 81);
            this.label5.Margin = new System.Windows.Forms.Padding(0, 0, 5, 0);
            this.label5.Name = "label5";
            this.label5.Padding = new System.Windows.Forms.Padding(0, 4, 0, 0);
            this.label5.Size = new System.Drawing.Size(276, 20);
            this.label5.TabIndex = 50;
            this.label5.Tag = "NonPaintable";
            this.label5.Text = "Modo descanso";
            this.label5.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // ExportRibbon
            // 
            this.ExportRibbon.AutoSize = true;
            this.ExportRibbon.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ExportRibbon.Font = new System.Drawing.Font("Microsoft YaHei UI", 8F);
            this.ExportRibbon.Location = new System.Drawing.Point(879, 1);
            this.ExportRibbon.Margin = new System.Windows.Forms.Padding(1, 1, 1, 0);
            this.ExportRibbon.Name = "ExportRibbon";
            this.ExportRibbon.Size = new System.Drawing.Size(336, 74);
            this.ExportRibbon.TabIndex = 54;
            // 
            // pnlCollapsedRibbons
            // 
            this.pnlCollapsedRibbons.AutoSize = true;
            this.pnlCollapsedRibbons.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.pnlCollapsedRibbons.BackColor = System.Drawing.Color.Transparent;
            this.pnlCollapsedRibbons.Controls.Add(this.miniTLP);
            this.pnlCollapsedRibbons.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlCollapsedRibbons.Location = new System.Drawing.Point(0, 0);
            this.pnlCollapsedRibbons.Margin = new System.Windows.Forms.Padding(0);
            this.pnlCollapsedRibbons.Name = "pnlCollapsedRibbons";
            this.pnlCollapsedRibbons.Size = new System.Drawing.Size(1387, 52);
            this.pnlCollapsedRibbons.TabIndex = 0;
            this.pnlCollapsedRibbons.Tag = "";
            this.pnlCollapsedRibbons.Visible = false;
            // 
            // miniTLP
            // 
            this.miniTLP.AutoSize = true;
            this.miniTLP.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.miniTLP.BackColor = System.Drawing.Color.Transparent;
            this.miniTLP.ColumnCount = 3;
            this.miniTLP.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.miniTLP.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.miniTLP.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.miniTLP.Controls.Add(this.collapsedRibbon, 0, 0);
            this.miniTLP.Controls.Add(this.btnRibbonExpander, 2, 0);
            this.miniTLP.Dock = System.Windows.Forms.DockStyle.Fill;
            this.miniTLP.Location = new System.Drawing.Point(0, 0);
            this.miniTLP.Name = "miniTLP";
            this.miniTLP.RowCount = 1;
            this.miniTLP.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.miniTLP.Size = new System.Drawing.Size(1387, 52);
            this.miniTLP.TabIndex = 65;
            this.miniTLP.Tag = "NonPaintable";
            // 
            // collapsedRibbon
            // 
            this.collapsedRibbon.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.collapsedRibbon.Dock = System.Windows.Forms.DockStyle.None;
            this.collapsedRibbon.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.collapsedRibbon.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton8,
            this.toolStripButton7,
            this.toolStripButton6,
            this.toolStripButton1,
            this.toolStripButton2,
            this.toolStripSplitButton1,
            this.toolStripButton5});
            this.collapsedRibbon.Location = new System.Drawing.Point(3, 3);
            this.collapsedRibbon.Margin = new System.Windows.Forms.Padding(3, 3, 0, 3);
            this.collapsedRibbon.Name = "collapsedRibbon";
            this.collapsedRibbon.Padding = new System.Windows.Forms.Padding(0);
            this.collapsedRibbon.Size = new System.Drawing.Size(638, 46);
            this.collapsedRibbon.TabIndex = 65;
            this.collapsedRibbon.Text = "toolStrip4";
            // 
            // toolStripButton8
            // 
            this.toolStripButton8.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.toolStripButton8.Font = new System.Drawing.Font("Microsoft YaHei UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolStripButton8.Image = global::WinformsUI.Properties.Resources.SmallCreateIcon;
            this.toolStripButton8.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.toolStripButton8.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripButton8.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton8.Margin = new System.Windows.Forms.Padding(0);
            this.toolStripButton8.Name = "toolStripButton8";
            this.toolStripButton8.Padding = new System.Windows.Forms.Padding(0, 0, 10, 0);
            this.toolStripButton8.Size = new System.Drawing.Size(80, 46);
            this.toolStripButton8.Text = "Añadir";
            this.toolStripButton8.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // toolStripButton7
            // 
            this.toolStripButton7.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.toolStripButton7.Font = new System.Drawing.Font("Microsoft YaHei UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolStripButton7.Image = global::WinformsUI.Properties.Resources.SmallRemoveIcon;
            this.toolStripButton7.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.toolStripButton7.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripButton7.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton7.Margin = new System.Windows.Forms.Padding(0);
            this.toolStripButton7.Name = "toolStripButton7";
            this.toolStripButton7.Padding = new System.Windows.Forms.Padding(0, 0, 10, 0);
            this.toolStripButton7.Size = new System.Drawing.Size(89, 46);
            this.toolStripButton7.Text = "Eliminar";
            this.toolStripButton7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // toolStripButton6
            // 
            this.toolStripButton6.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.toolStripButton6.Font = new System.Drawing.Font("Microsoft YaHei UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolStripButton6.Image = global::WinformsUI.Properties.Resources.SmallRefreshIcon;
            this.toolStripButton6.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.toolStripButton6.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripButton6.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton6.Margin = new System.Windows.Forms.Padding(0);
            this.toolStripButton6.Name = "toolStripButton6";
            this.toolStripButton6.Padding = new System.Windows.Forms.Padding(0, 0, 10, 0);
            this.toolStripButton6.Size = new System.Drawing.Size(96, 46);
            this.toolStripButton6.Text = "Refrescar";
            this.toolStripButton6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.toolStripButton1.Font = new System.Drawing.Font("Microsoft YaHei UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolStripButton1.Image = global::WinformsUI.Properties.Resources.SmallEditIcon;
            this.toolStripButton1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.toolStripButton1.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Margin = new System.Windows.Forms.Padding(0);
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Padding = new System.Windows.Forms.Padding(0, 10, 10, 10);
            this.toolStripButton1.Size = new System.Drawing.Size(97, 46);
            this.toolStripButton1.Text = "Modificar";
            this.toolStripButton1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // toolStripButton2
            // 
            this.toolStripButton2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.toolStripButton2.Font = new System.Drawing.Font("Microsoft YaHei UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolStripButton2.Image = global::WinformsUI.Properties.Resources.SmallEditIcon;
            this.toolStripButton2.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.toolStripButton2.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton2.Margin = new System.Windows.Forms.Padding(0);
            this.toolStripButton2.Name = "toolStripButton2";
            this.toolStripButton2.Padding = new System.Windows.Forms.Padding(0, 10, 10, 10);
            this.toolStripButton2.Size = new System.Drawing.Size(102, 46);
            this.toolStripButton2.Text = "WhatsApp";
            this.toolStripButton2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // toolStripSplitButton1
            // 
            this.toolStripSplitButton1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fsdsfgdfgssdfgToolStripMenuItem,
            this.asdasdasdToolStripMenuItem});
            this.toolStripSplitButton1.Font = new System.Drawing.Font("Microsoft YaHei UI", 8.25F, System.Drawing.FontStyle.Bold);
            this.toolStripSplitButton1.Image = global::WinformsUI.Properties.Resources.SmallRefreshIcon;
            this.toolStripSplitButton1.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripSplitButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripSplitButton1.Name = "toolStripSplitButton1";
            this.toolStripSplitButton1.Size = new System.Drawing.Size(94, 43);
            this.toolStripSplitButton1.Text = "Exportar";
            // 
            // fsdsfgdfgssdfgToolStripMenuItem
            // 
            this.fsdsfgdfgssdfgToolStripMenuItem.Name = "fsdsfgdfgssdfgToolStripMenuItem";
            this.fsdsfgdfgssdfgToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.fsdsfgdfgssdfgToolStripMenuItem.Text = "fsdsfgdfgssdfg";
            // 
            // asdasdasdToolStripMenuItem
            // 
            this.asdasdasdToolStripMenuItem.Name = "asdasdasdToolStripMenuItem";
            this.asdasdasdToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.asdasdasdToolStripMenuItem.Text = "asdasdasd";
            // 
            // toolStripButton5
            // 
            this.toolStripButton5.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.toolStripButton5.Font = new System.Drawing.Font("Microsoft YaHei UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolStripButton5.Image = global::WinformsUI.Properties.Resources.SmallEditIcon;
            this.toolStripButton5.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.toolStripButton5.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripButton5.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton5.Margin = new System.Windows.Forms.Padding(0);
            this.toolStripButton5.Name = "toolStripButton5";
            this.toolStripButton5.Padding = new System.Windows.Forms.Padding(0, 10, 20, 10);
            this.toolStripButton5.Size = new System.Drawing.Size(78, 46);
            this.toolStripButton5.Text = "Mail";
            this.toolStripButton5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnRibbonExpander
            // 
            this.btnRibbonExpander.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRibbonExpander.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnRibbonExpander.BackColor = System.Drawing.Color.Transparent;
            this.btnRibbonExpander.FlatAppearance.BorderSize = 0;
            this.btnRibbonExpander.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRibbonExpander.Location = new System.Drawing.Point(1341, 6);
            this.btnRibbonExpander.Margin = new System.Windows.Forms.Padding(0);
            this.btnRibbonExpander.MaximumSize = new System.Drawing.Size(46, 46);
            this.btnRibbonExpander.Name = "btnRibbonExpander";
            this.btnRibbonExpander.Size = new System.Drawing.Size(46, 46);
            this.btnRibbonExpander.TabIndex = 39;
            this.btnRibbonExpander.Tag = "NonPaintable";
            this.btnRibbonExpander.Text = "▼";
            this.btnRibbonExpander.UseVisualStyleBackColor = false;
            // 
            // pnlDgv
            // 
            this.pnlDgv.BackColor = System.Drawing.Color.IndianRed;
            this.pnlDgv.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlDgv.Location = new System.Drawing.Point(0, 162);
            this.pnlDgv.Margin = new System.Windows.Forms.Padding(0);
            this.pnlDgv.Name = "pnlDgv";
            this.pnlDgv.Size = new System.Drawing.Size(1387, 598);
            this.pnlDgv.TabIndex = 57;
            // 
            // SaleForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1387, 638);
            this.Controls.Add(this.mainTLP);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "SaleForm";
            this.Text = "SaleForm";
            this.mainTLP.ResumeLayout(false);
            this.mainTLP.PerformLayout();
            this.pnlExpandedRibbons.ResumeLayout(false);
            this.pnlExpandedRibbons.PerformLayout();
            this.tlpExpandedRibbon.ResumeLayout(false);
            this.tlpExpandedRibbon.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.pnlCollapsedRibbons.ResumeLayout(false);
            this.pnlCollapsedRibbons.PerformLayout();
            this.miniTLP.ResumeLayout(false);
            this.miniTLP.PerformLayout();
            this.collapsedRibbon.ResumeLayout(false);
            this.collapsedRibbon.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel mainTLP;
        private System.Windows.Forms.Panel pnlFeedbackBar;
        private System.Windows.Forms.Panel pnlExpandedRibbons;
        private System.Windows.Forms.TableLayoutPanel tlpExpandedRibbon;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ToolStrip toolStrip2;
        private UserControls.Ribbon.EyeRestRibbon ribbonEyeRest;
        private UserControls.Ribbon.CommunicationRibbon communicationFunctions1;
        private UserControls.CustomDGV.CustomDgvRibbon ribbonDgvFunctions;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton btnCreate;
        private System.Windows.Forms.ToolStripButton btnDelete;
        private System.Windows.Forms.ToolStripButton btnRefresh;
        private System.Windows.Forms.ToolStripButton btnUpdate;
        private System.Windows.Forms.Button btnRibbonCollapser;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label5;
        private UserControls.Ribbon.ExportRibbon ExportRibbon;
        private System.Windows.Forms.Panel pnlCollapsedRibbons;
        private System.Windows.Forms.TableLayoutPanel miniTLP;
        private System.Windows.Forms.ToolStrip collapsedRibbon;
        private System.Windows.Forms.ToolStripButton toolStripButton8;
        private System.Windows.Forms.ToolStripButton toolStripButton7;
        private System.Windows.Forms.ToolStripButton toolStripButton6;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        private System.Windows.Forms.ToolStripButton toolStripButton2;
        private System.Windows.Forms.ToolStripSplitButton toolStripSplitButton1;
        private System.Windows.Forms.ToolStripMenuItem fsdsfgdfgssdfgToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem asdasdasdToolStripMenuItem;
        private System.Windows.Forms.ToolStripButton toolStripButton5;
        private System.Windows.Forms.Button btnRibbonExpander;
        private System.Windows.Forms.Panel pnlDgv;
    }
}