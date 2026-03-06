namespace WinformsUI.UserControls.CustomDGV
{
    partial class CustomDGVForm
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.mainDGV = new System.Windows.Forms.DataGridView();
            this.tableLayoutPanelSearchControls = new System.Windows.Forms.TableLayoutPanel();
            this.tbSearchBar = new System.Windows.Forms.TextBox();
            this.btnShowFilters = new System.Windows.Forms.Button();
            this.cbColumnsNameSearch = new System.Windows.Forms.ComboBox();
            this.dateTimePickerUpTo = new System.Windows.Forms.DateTimePicker();
            this.dateTimePickerSince = new System.Windows.Forms.DateTimePicker();
            this.panelHorDivider = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.tableLayoutPanelFilters = new System.Windows.Forms.TableLayoutPanel();
            this.checkedListBoxFilters = new System.Windows.Forms.CheckedListBox();
            this.btnApplyFilter = new System.Windows.Forms.Button();
            this.btnCleanFilters = new System.Windows.Forms.Button();
            this.cbColumnsNameFilterDate = new System.Windows.Forms.ComboBox();
            this.rbOnlyActives = new System.Windows.Forms.RadioButton();
            this.rbOnlyDeleted = new System.Windows.Forms.RadioButton();
            this.rbAllEntities = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.rbAtLeastOneCategory = new System.Windows.Forms.RadioButton();
            this.rbAllCategories = new System.Windows.Forms.RadioButton();
            this.label5 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.mainDGV)).BeginInit();
            this.tableLayoutPanelSearchControls.SuspendLayout();
            this.tableLayoutPanelFilters.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainDGV
            // 
            this.mainDGV.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.ColumnHeader;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft YaHei UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.mainDGV.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.mainDGV.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft YaHei UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.mainDGV.DefaultCellStyle = dataGridViewCellStyle2;
            this.mainDGV.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainDGV.Location = new System.Drawing.Point(251, 75);
            this.mainDGV.Margin = new System.Windows.Forms.Padding(15, 3, 3, 3);
            this.mainDGV.Name = "mainDGV";
            this.mainDGV.Size = new System.Drawing.Size(1125, 732);
            this.mainDGV.TabIndex = 23;
            // 
            // tableLayoutPanelSearchControls
            // 
            this.tableLayoutPanelSearchControls.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.tableLayoutPanelSearchControls.ColumnCount = 3;
            this.tableLayoutPanelSearchControls.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelSearchControls.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelSearchControls.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 54.49306F));
            this.tableLayoutPanelSearchControls.Controls.Add(this.tbSearchBar, 2, 0);
            this.tableLayoutPanelSearchControls.Controls.Add(this.btnShowFilters, 0, 0);
            this.tableLayoutPanelSearchControls.Controls.Add(this.cbColumnsNameSearch, 1, 0);
            this.tableLayoutPanelSearchControls.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanelSearchControls.Location = new System.Drawing.Point(0, 11);
            this.tableLayoutPanelSearchControls.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanelSearchControls.Name = "tableLayoutPanelSearchControls";
            this.tableLayoutPanelSearchControls.RowCount = 1;
            this.tableLayoutPanelSearchControls.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelSearchControls.Size = new System.Drawing.Size(1376, 59);
            this.tableLayoutPanelSearchControls.TabIndex = 24;
            this.tableLayoutPanelSearchControls.Tag = "";
            // 
            // tbSearchBar
            // 
            this.tbSearchBar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.tbSearchBar.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tbSearchBar.Font = new System.Drawing.Font("Microsoft YaHei UI", 10F);
            this.tbSearchBar.Location = new System.Drawing.Point(258, 21);
            this.tbSearchBar.Margin = new System.Windows.Forms.Padding(20, 3, 20, 3);
            this.tbSearchBar.Name = "tbSearchBar";
            this.tbSearchBar.Size = new System.Drawing.Size(1098, 17);
            this.tbSearchBar.TabIndex = 15;
            // 
            // btnShowFilters
            // 
            this.btnShowFilters.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.btnShowFilters.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnShowFilters.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnShowFilters.Image = global::WinformsUI.Properties.Resources.FiltersIcon;
            this.btnShowFilters.Location = new System.Drawing.Point(3, 11);
            this.btnShowFilters.Name = "btnShowFilters";
            this.btnShowFilters.Size = new System.Drawing.Size(37, 37);
            this.btnShowFilters.TabIndex = 12;
            this.btnShowFilters.Tag = "IsImageColorable";
            this.btnShowFilters.UseVisualStyleBackColor = true;
            // 
            // cbColumnsNameSearch
            // 
            this.cbColumnsNameSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.cbColumnsNameSearch.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            this.cbColumnsNameSearch.FormattingEnabled = true;
            this.cbColumnsNameSearch.Location = new System.Drawing.Point(46, 17);
            this.cbColumnsNameSearch.Name = "cbColumnsNameSearch";
            this.cbColumnsNameSearch.Size = new System.Drawing.Size(189, 25);
            this.cbColumnsNameSearch.TabIndex = 13;
            // 
            // dateTimePickerUpTo
            // 
            this.dateTimePickerUpTo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.dateTimePickerUpTo.Cursor = System.Windows.Forms.Cursors.Hand;
            this.dateTimePickerUpTo.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            this.dateTimePickerUpTo.Location = new System.Drawing.Point(3, 478);
            this.dateTimePickerUpTo.Name = "dateTimePickerUpTo";
            this.dateTimePickerUpTo.Size = new System.Drawing.Size(240, 23);
            this.dateTimePickerUpTo.TabIndex = 2;
            // 
            // dateTimePickerSince
            // 
            this.dateTimePickerSince.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.dateTimePickerSince.Cursor = System.Windows.Forms.Cursors.Hand;
            this.dateTimePickerSince.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            this.dateTimePickerSince.Location = new System.Drawing.Point(3, 444);
            this.dateTimePickerSince.Name = "dateTimePickerSince";
            this.dateTimePickerSince.Size = new System.Drawing.Size(240, 23);
            this.dateTimePickerSince.TabIndex = 0;
            // 
            // panelHorDivider
            // 
            this.panelHorDivider.BackColor = System.Drawing.Color.Transparent;
            this.panelHorDivider.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelHorDivider.Location = new System.Drawing.Point(246, 75);
            this.panelHorDivider.Name = "panelHorDivider";
            this.panelHorDivider.Size = new System.Drawing.Size(5, 732);
            this.panelHorDivider.TabIndex = 25;
            this.panelHorDivider.Tag = "";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Transparent;
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 70);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1376, 5);
            this.panel1.TabIndex = 28;
            this.panel1.Tag = "";
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.Transparent;
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1376, 11);
            this.panel2.TabIndex = 27;
            this.panel2.Tag = "NonPaintable";
            // 
            // tableLayoutPanelFilters
            // 
            this.tableLayoutPanelFilters.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanelFilters.BackColor = System.Drawing.Color.Transparent;
            this.tableLayoutPanelFilters.ColumnCount = 1;
            this.tableLayoutPanelFilters.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelFilters.Controls.Add(this.checkedListBoxFilters, 0, 4);
            this.tableLayoutPanelFilters.Controls.Add(this.btnApplyFilter, 0, 14);
            this.tableLayoutPanelFilters.Controls.Add(this.btnCleanFilters, 0, 13);
            this.tableLayoutPanelFilters.Controls.Add(this.cbColumnsNameFilterDate, 0, 6);
            this.tableLayoutPanelFilters.Controls.Add(this.dateTimePickerUpTo, 0, 8);
            this.tableLayoutPanelFilters.Controls.Add(this.rbOnlyActives, 0, 10);
            this.tableLayoutPanelFilters.Controls.Add(this.rbOnlyDeleted, 0, 11);
            this.tableLayoutPanelFilters.Controls.Add(this.dateTimePickerSince, 0, 7);
            this.tableLayoutPanelFilters.Controls.Add(this.rbAllEntities, 0, 12);
            this.tableLayoutPanelFilters.Controls.Add(this.label1, 0, 1);
            this.tableLayoutPanelFilters.Controls.Add(this.label2, 0, 3);
            this.tableLayoutPanelFilters.Controls.Add(this.label3, 0, 9);
            this.tableLayoutPanelFilters.Controls.Add(this.label4, 0, 0);
            this.tableLayoutPanelFilters.Controls.Add(this.tableLayoutPanel1, 0, 2);
            this.tableLayoutPanelFilters.Controls.Add(this.label5, 0, 5);
            this.tableLayoutPanelFilters.Dock = System.Windows.Forms.DockStyle.Left;
            this.tableLayoutPanelFilters.Location = new System.Drawing.Point(0, 75);
            this.tableLayoutPanelFilters.Margin = new System.Windows.Forms.Padding(3, 10, 10, 3);
            this.tableLayoutPanelFilters.Name = "tableLayoutPanelFilters";
            this.tableLayoutPanelFilters.RowCount = 15;
            this.tableLayoutPanelFilters.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanelFilters.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelFilters.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 70F));
            this.tableLayoutPanelFilters.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanelFilters.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelFilters.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 56F));
            this.tableLayoutPanelFilters.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 42F));
            this.tableLayoutPanelFilters.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32F));
            this.tableLayoutPanelFilters.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tableLayoutPanelFilters.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 34F));
            this.tableLayoutPanelFilters.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 33F));
            this.tableLayoutPanelFilters.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32F));
            this.tableLayoutPanelFilters.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanelFilters.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tableLayoutPanelFilters.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 51F));
            this.tableLayoutPanelFilters.Size = new System.Drawing.Size(246, 732);
            this.tableLayoutPanelFilters.TabIndex = 29;
            this.tableLayoutPanelFilters.Tag = "NonPaintable";
            // 
            // checkedListBoxFilters
            // 
            this.checkedListBoxFilters.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.checkedListBoxFilters.Cursor = System.Windows.Forms.Cursors.Hand;
            this.checkedListBoxFilters.Dock = System.Windows.Forms.DockStyle.Fill;
            this.checkedListBoxFilters.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            this.checkedListBoxFilters.FormattingEnabled = true;
            this.checkedListBoxFilters.Location = new System.Drawing.Point(3, 173);
            this.checkedListBoxFilters.Name = "checkedListBoxFilters";
            this.checkedListBoxFilters.Size = new System.Drawing.Size(240, 166);
            this.checkedListBoxFilters.TabIndex = 0;
            // 
            // btnApplyFilter
            // 
            this.btnApplyFilter.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnApplyFilter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnApplyFilter.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            this.btnApplyFilter.Location = new System.Drawing.Point(3, 684);
            this.btnApplyFilter.Name = "btnApplyFilter";
            this.btnApplyFilter.Size = new System.Drawing.Size(240, 45);
            this.btnApplyFilter.TabIndex = 2;
            this.btnApplyFilter.Tag = "LowAccented";
            this.btnApplyFilter.Text = "Filtrar";
            this.btnApplyFilter.UseVisualStyleBackColor = true;
            // 
            // btnCleanFilters
            // 
            this.btnCleanFilters.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCleanFilters.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnCleanFilters.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            this.btnCleanFilters.Location = new System.Drawing.Point(3, 649);
            this.btnCleanFilters.Name = "btnCleanFilters";
            this.btnCleanFilters.Size = new System.Drawing.Size(240, 29);
            this.btnCleanFilters.TabIndex = 1;
            this.btnCleanFilters.Tag = "HighAccented";
            this.btnCleanFilters.Text = "Limpiar Filtros";
            this.btnCleanFilters.UseVisualStyleBackColor = true;
            // 
            // cbColumnsNameFilterDate
            // 
            this.cbColumnsNameFilterDate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.cbColumnsNameFilterDate.Cursor = System.Windows.Forms.Cursors.Hand;
            this.cbColumnsNameFilterDate.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            this.cbColumnsNameFilterDate.FormattingEnabled = true;
            this.cbColumnsNameFilterDate.Location = new System.Drawing.Point(3, 406);
            this.cbColumnsNameFilterDate.Name = "cbColumnsNameFilterDate";
            this.cbColumnsNameFilterDate.Size = new System.Drawing.Size(240, 25);
            this.cbColumnsNameFilterDate.TabIndex = 13;
            // 
            // rbOnlyActives
            // 
            this.rbOnlyActives.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.rbOnlyActives.BackColor = System.Drawing.Color.Transparent;
            this.rbOnlyActives.Checked = true;
            this.rbOnlyActives.Cursor = System.Windows.Forms.Cursors.Hand;
            this.rbOnlyActives.Font = new System.Drawing.Font("Microsoft YaHei UI", 8.25F);
            this.rbOnlyActives.Location = new System.Drawing.Point(3, 549);
            this.rbOnlyActives.Name = "rbOnlyActives";
            this.rbOnlyActives.Size = new System.Drawing.Size(240, 17);
            this.rbOnlyActives.TabIndex = 0;
            this.rbOnlyActives.TabStop = true;
            this.rbOnlyActives.Tag = "NonPaintable";
            this.rbOnlyActives.Text = "Ver sólo activos";
            this.rbOnlyActives.UseVisualStyleBackColor = false;
            // 
            // rbOnlyDeleted
            // 
            this.rbOnlyDeleted.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.rbOnlyDeleted.BackColor = System.Drawing.Color.Transparent;
            this.rbOnlyDeleted.Cursor = System.Windows.Forms.Cursors.Hand;
            this.rbOnlyDeleted.Font = new System.Drawing.Font("Microsoft YaHei UI", 8.25F);
            this.rbOnlyDeleted.Location = new System.Drawing.Point(3, 581);
            this.rbOnlyDeleted.Name = "rbOnlyDeleted";
            this.rbOnlyDeleted.Size = new System.Drawing.Size(240, 17);
            this.rbOnlyDeleted.TabIndex = 0;
            this.rbOnlyDeleted.Tag = "NonPaintable";
            this.rbOnlyDeleted.Text = "Ver sólo borrados";
            this.rbOnlyDeleted.UseVisualStyleBackColor = false;
            // 
            // rbAllEntities
            // 
            this.rbAllEntities.BackColor = System.Drawing.Color.Transparent;
            this.rbAllEntities.Cursor = System.Windows.Forms.Cursors.Hand;
            this.rbAllEntities.Font = new System.Drawing.Font("Microsoft YaHei UI", 8.25F);
            this.rbAllEntities.Location = new System.Drawing.Point(3, 615);
            this.rbAllEntities.Margin = new System.Windows.Forms.Padding(3, 9, 3, 3);
            this.rbAllEntities.Name = "rbAllEntities";
            this.rbAllEntities.Size = new System.Drawing.Size(239, 17);
            this.rbAllEntities.TabIndex = 0;
            this.rbAllEntities.Tag = "NonPaintable";
            this.rbAllEntities.Text = "Ver ambos";
            this.rbAllEntities.UseVisualStyleBackColor = false;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft YaHei UI", 8.25F);
            this.label1.Location = new System.Drawing.Point(3, 40);
            this.label1.Margin = new System.Windows.Forms.Padding(3, 0, 3, 5);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(116, 15);
            this.label1.TabIndex = 4;
            this.label1.Tag = "Accented";
            this.label1.Text = "Tipo de coincidencia";
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft YaHei UI", 8.25F);
            this.label2.Location = new System.Drawing.Point(3, 149);
            this.label2.Margin = new System.Windows.Forms.Padding(3, 0, 3, 5);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(124, 16);
            this.label2.TabIndex = 4;
            this.label2.Tag = "Accented";
            this.label2.Text = "Seleccionar categorías";
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft YaHei UI", 8.25F);
            this.label3.Location = new System.Drawing.Point(3, 520);
            this.label3.Margin = new System.Windows.Forms.Padding(3, 0, 3, 5);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(96, 16);
            this.label3.TabIndex = 4;
            this.label3.Tag = "Accented";
            this.label3.Text = "Filtrar por estado";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label4.Font = new System.Drawing.Font("Microsoft YaHei UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(3, 5);
            this.label4.Margin = new System.Windows.Forms.Padding(3, 5, 3, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(240, 35);
            this.label4.TabIndex = 4;
            this.label4.Text = "Búsqueda Avanzada";
            this.label4.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.rbAtLeastOneCategory, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.rbAllCategories, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 63);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(240, 64);
            this.tableLayoutPanel1.TabIndex = 5;
            this.tableLayoutPanel1.Tag = "NonPaintable";
            // 
            // rbAtLeastOneCategory
            // 
            this.rbAtLeastOneCategory.BackColor = System.Drawing.Color.Transparent;
            this.rbAtLeastOneCategory.Checked = true;
            this.rbAtLeastOneCategory.Cursor = System.Windows.Forms.Cursors.Hand;
            this.rbAtLeastOneCategory.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rbAtLeastOneCategory.Font = new System.Drawing.Font("Microsoft YaHei UI", 8.25F);
            this.rbAtLeastOneCategory.Location = new System.Drawing.Point(3, 3);
            this.rbAtLeastOneCategory.Name = "rbAtLeastOneCategory";
            this.rbAtLeastOneCategory.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.rbAtLeastOneCategory.Size = new System.Drawing.Size(234, 26);
            this.rbAtLeastOneCategory.TabIndex = 0;
            this.rbAtLeastOneCategory.TabStop = true;
            this.rbAtLeastOneCategory.Tag = "NonPaintable";
            this.rbAtLeastOneCategory.Text = "Al menos una categoría";
            this.rbAtLeastOneCategory.UseVisualStyleBackColor = false;
            // 
            // rbAllCategories
            // 
            this.rbAllCategories.BackColor = System.Drawing.Color.Transparent;
            this.rbAllCategories.Cursor = System.Windows.Forms.Cursors.Hand;
            this.rbAllCategories.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rbAllCategories.Font = new System.Drawing.Font("Microsoft YaHei UI", 8.25F);
            this.rbAllCategories.Location = new System.Drawing.Point(3, 35);
            this.rbAllCategories.Name = "rbAllCategories";
            this.rbAllCategories.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.rbAllCategories.Size = new System.Drawing.Size(234, 26);
            this.rbAllCategories.TabIndex = 0;
            this.rbAllCategories.Tag = "NonPaintable";
            this.rbAllCategories.Text = "Todas las seleccionadas";
            this.rbAllCategories.UseVisualStyleBackColor = false;
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft YaHei UI", 8.25F);
            this.label5.Location = new System.Drawing.Point(3, 377);
            this.label5.Margin = new System.Windows.Forms.Padding(3, 0, 3, 5);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(80, 16);
            this.label5.TabIndex = 4;
            this.label5.Tag = "Accented";
            this.label5.Text = "Elegir periodo";
            // 
            // CustomDGVForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1376, 807);
            this.Controls.Add(this.mainDGV);
            this.Controls.Add(this.panelHorDivider);
            this.Controls.Add(this.tableLayoutPanelFilters);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.tableLayoutPanelSearchControls);
            this.Controls.Add(this.panel2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "CustomDGVForm";
            this.Text = "CustomDGV";
            ((System.ComponentModel.ISupportInitialize)(this.mainDGV)).EndInit();
            this.tableLayoutPanelSearchControls.ResumeLayout(false);
            this.tableLayoutPanelSearchControls.PerformLayout();
            this.tableLayoutPanelFilters.ResumeLayout(false);
            this.tableLayoutPanelFilters.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView mainDGV;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelSearchControls;
        private System.Windows.Forms.ComboBox cbColumnsNameSearch;
        private System.Windows.Forms.Button btnShowFilters;
        private System.Windows.Forms.DateTimePicker dateTimePickerUpTo;
        private System.Windows.Forms.DateTimePicker dateTimePickerSince;
        private System.Windows.Forms.Panel panelHorDivider;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.TextBox tbSearchBar;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelFilters;
        private System.Windows.Forms.CheckedListBox checkedListBoxFilters;
        private System.Windows.Forms.Button btnApplyFilter;
        private System.Windows.Forms.Button btnCleanFilters;
        private System.Windows.Forms.RadioButton rbOnlyActives;
        private System.Windows.Forms.RadioButton rbOnlyDeleted;
        private System.Windows.Forms.RadioButton rbAllEntities;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.RadioButton rbAtLeastOneCategory;
        private System.Windows.Forms.RadioButton rbAllCategories;
        private System.Windows.Forms.ComboBox cbColumnsNameFilterDate;
        private System.Windows.Forms.Label label5;
    }
}