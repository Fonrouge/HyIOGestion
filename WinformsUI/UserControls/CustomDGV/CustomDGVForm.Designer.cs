namespace WinformsUI.UserControls.CustomDGV
{
    partial class CustomDGVForm
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.mainDGV = new System.Windows.Forms.DataGridView();
            this.tableLayoutPanelFilters = new System.Windows.Forms.TableLayoutPanel();
            this.checkedListBoxFilters = new System.Windows.Forms.CheckedListBox();
            this.btnApplyFilter = new System.Windows.Forms.Button();
            this.btnCleanFilters = new System.Windows.Forms.Button();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.radioButton3 = new System.Windows.Forms.RadioButton();
            this.tableLayoutPanelSearchControls = new System.Windows.Forms.TableLayoutPanel();
            this.tbSearchBar = new System.Windows.Forms.TextBox();
            this.cbColumnsName = new System.Windows.Forms.ComboBox();
            this.btnShowFilters = new System.Windows.Forms.Button();
            this.dateTimePickerUpTo = new System.Windows.Forms.DateTimePicker();
            this.lblUpTo = new System.Windows.Forms.Label();
            this.dateTimePickerSince = new System.Windows.Forms.DateTimePicker();
            this.lblSince = new System.Windows.Forms.Label();
            this.panelHorDivider = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.mainDGV)).BeginInit();
            this.tableLayoutPanelFilters.SuspendLayout();
            this.tableLayoutPanelSearchControls.SuspendLayout();
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
            this.mainDGV.Location = new System.Drawing.Point(250, 88);
            this.mainDGV.Margin = new System.Windows.Forms.Padding(15, 3, 3, 3);
            this.mainDGV.Name = "mainDGV";
            this.mainDGV.Size = new System.Drawing.Size(1126, 447);
            this.mainDGV.TabIndex = 23;
            // 
            // tableLayoutPanelFilters
            // 
            this.tableLayoutPanelFilters.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanelFilters.ColumnCount = 1;
            this.tableLayoutPanelFilters.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelFilters.Controls.Add(this.checkedListBoxFilters, 0, 0);
            this.tableLayoutPanelFilters.Controls.Add(this.btnApplyFilter, 0, 5);
            this.tableLayoutPanelFilters.Controls.Add(this.btnCleanFilters, 0, 4);
            this.tableLayoutPanelFilters.Controls.Add(this.radioButton1, 0, 3);
            this.tableLayoutPanelFilters.Controls.Add(this.radioButton2, 0, 2);
            this.tableLayoutPanelFilters.Controls.Add(this.radioButton3, 0, 1);
            this.tableLayoutPanelFilters.Dock = System.Windows.Forms.DockStyle.Left;
            this.tableLayoutPanelFilters.Location = new System.Drawing.Point(0, 88);
            this.tableLayoutPanelFilters.Margin = new System.Windows.Forms.Padding(3, 10, 10, 3);
            this.tableLayoutPanelFilters.Name = "tableLayoutPanelFilters";
            this.tableLayoutPanelFilters.RowCount = 6;
            this.tableLayoutPanelFilters.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelFilters.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tableLayoutPanelFilters.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tableLayoutPanelFilters.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tableLayoutPanelFilters.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 46F));
            this.tableLayoutPanelFilters.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanelFilters.Size = new System.Drawing.Size(245, 447);
            this.tableLayoutPanelFilters.TabIndex = 22;
            // 
            // checkedListBoxFilters
            // 
            this.checkedListBoxFilters.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.checkedListBoxFilters.Dock = System.Windows.Forms.DockStyle.Fill;
            this.checkedListBoxFilters.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            this.checkedListBoxFilters.FormattingEnabled = true;
            this.checkedListBoxFilters.Location = new System.Drawing.Point(3, 3);
            this.checkedListBoxFilters.Name = "checkedListBoxFilters";
            this.checkedListBoxFilters.Size = new System.Drawing.Size(239, 250);
            this.checkedListBoxFilters.TabIndex = 0;
            // 
            // btnApplyFilter
            // 
            this.btnApplyFilter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnApplyFilter.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            this.btnApplyFilter.Location = new System.Drawing.Point(3, 410);
            this.btnApplyFilter.Name = "btnApplyFilter";
            this.btnApplyFilter.Size = new System.Drawing.Size(239, 34);
            this.btnApplyFilter.TabIndex = 2;
            this.btnApplyFilter.Tag = "LowAccented";
            this.btnApplyFilter.Text = "Filtrar";
            this.btnApplyFilter.UseVisualStyleBackColor = true;
            // 
            // btnCleanFilters
            // 
            this.btnCleanFilters.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnCleanFilters.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            this.btnCleanFilters.Location = new System.Drawing.Point(3, 364);
            this.btnCleanFilters.Name = "btnCleanFilters";
            this.btnCleanFilters.Size = new System.Drawing.Size(239, 40);
            this.btnCleanFilters.TabIndex = 1;
            this.btnCleanFilters.Tag = "HighAccented";
            this.btnCleanFilters.Text = "Limpiar Filtros";
            this.btnCleanFilters.UseVisualStyleBackColor = true;
            // 
            // radioButton1
            // 
            this.radioButton1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.radioButton1.BackColor = System.Drawing.Color.Transparent;
            this.radioButton1.Font = new System.Drawing.Font("Microsoft YaHei UI", 8.25F);
            this.radioButton1.Location = new System.Drawing.Point(3, 335);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(239, 17);
            this.radioButton1.TabIndex = 0;
            this.radioButton1.Tag = "NonPaintable";
            this.radioButton1.Text = "Ver sólo borrados";
            this.radioButton1.UseVisualStyleBackColor = false;
            // 
            // radioButton2
            // 
            this.radioButton2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.radioButton2.BackColor = System.Drawing.Color.Transparent;
            this.radioButton2.Font = new System.Drawing.Font("Microsoft YaHei UI", 8.25F);
            this.radioButton2.Location = new System.Drawing.Point(3, 300);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(239, 17);
            this.radioButton2.TabIndex = 0;
            this.radioButton2.Tag = "NonPaintable";
            this.radioButton2.Text = "Incluir borrados";
            this.radioButton2.UseVisualStyleBackColor = false;
            // 
            // radioButton3
            // 
            this.radioButton3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.radioButton3.BackColor = System.Drawing.Color.Transparent;
            this.radioButton3.Checked = true;
            this.radioButton3.Font = new System.Drawing.Font("Microsoft YaHei UI", 8.25F);
            this.radioButton3.Location = new System.Drawing.Point(3, 265);
            this.radioButton3.Name = "radioButton3";
            this.radioButton3.Size = new System.Drawing.Size(239, 17);
            this.radioButton3.TabIndex = 0;
            this.radioButton3.TabStop = true;
            this.radioButton3.Tag = "NonPaintable";
            this.radioButton3.Text = "Ver sólo activos";
            this.radioButton3.UseVisualStyleBackColor = false;
            // 
            // tableLayoutPanelSearchControls
            // 
            this.tableLayoutPanelSearchControls.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.tableLayoutPanelSearchControls.ColumnCount = 7;
            this.tableLayoutPanelSearchControls.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelSearchControls.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelSearchControls.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 54.49306F));
            this.tableLayoutPanelSearchControls.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 7.699115F));
            this.tableLayoutPanelSearchControls.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.81416F));
            this.tableLayoutPanelSearchControls.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 4.690266F));
            this.tableLayoutPanelSearchControls.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.19469F));
            this.tableLayoutPanelSearchControls.Controls.Add(this.tbSearchBar, 2, 0);
            this.tableLayoutPanelSearchControls.Controls.Add(this.cbColumnsName, 1, 0);
            this.tableLayoutPanelSearchControls.Controls.Add(this.btnShowFilters, 0, 0);
            this.tableLayoutPanelSearchControls.Controls.Add(this.dateTimePickerUpTo, 6, 0);
            this.tableLayoutPanelSearchControls.Controls.Add(this.lblUpTo, 5, 0);
            this.tableLayoutPanelSearchControls.Controls.Add(this.dateTimePickerSince, 4, 0);
            this.tableLayoutPanelSearchControls.Controls.Add(this.lblSince, 3, 0);
            this.tableLayoutPanelSearchControls.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanelSearchControls.Location = new System.Drawing.Point(0, 11);
            this.tableLayoutPanelSearchControls.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanelSearchControls.Name = "tableLayoutPanelSearchControls";
            this.tableLayoutPanelSearchControls.RowCount = 1;
            this.tableLayoutPanelSearchControls.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelSearchControls.Size = new System.Drawing.Size(1376, 72);
            this.tableLayoutPanelSearchControls.TabIndex = 24;
            this.tableLayoutPanelSearchControls.Tag = "";
            // 
            // tbSearchBar
            // 
            this.tbSearchBar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.tbSearchBar.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tbSearchBar.Font = new System.Drawing.Font("Microsoft YaHei UI", 10F);
            this.tbSearchBar.Location = new System.Drawing.Point(258, 27);
            this.tbSearchBar.Margin = new System.Windows.Forms.Padding(20, 3, 3, 3);
            this.tbSearchBar.Name = "tbSearchBar";
            this.tbSearchBar.Size = new System.Drawing.Size(597, 17);
            this.tbSearchBar.TabIndex = 15;
            // 
            // cbColumnsName
            // 
            this.cbColumnsName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.cbColumnsName.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            this.cbColumnsName.FormattingEnabled = true;
            this.cbColumnsName.Location = new System.Drawing.Point(46, 23);
            this.cbColumnsName.Name = "cbColumnsName";
            this.cbColumnsName.Size = new System.Drawing.Size(189, 25);
            this.cbColumnsName.TabIndex = 13;
            // 
            // btnShowFilters
            // 
            this.btnShowFilters.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.btnShowFilters.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnShowFilters.Image = global::WinformsUI.Properties.Resources.FiltersIcon;
            this.btnShowFilters.Location = new System.Drawing.Point(3, 17);
            this.btnShowFilters.Name = "btnShowFilters";
            this.btnShowFilters.Size = new System.Drawing.Size(37, 37);
            this.btnShowFilters.TabIndex = 12;
            this.btnShowFilters.Tag = "IsImageColorable";
            this.btnShowFilters.UseVisualStyleBackColor = true;
            // 
            // dateTimePickerUpTo
            // 
            this.dateTimePickerUpTo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.dateTimePickerUpTo.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            this.dateTimePickerUpTo.Location = new System.Drawing.Point(1192, 24);
            this.dateTimePickerUpTo.Name = "dateTimePickerUpTo";
            this.dateTimePickerUpTo.Size = new System.Drawing.Size(181, 23);
            this.dateTimePickerUpTo.TabIndex = 2;
            // 
            // lblUpTo
            // 
            this.lblUpTo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.lblUpTo.AutoSize = true;
            this.lblUpTo.BackColor = System.Drawing.Color.Transparent;
            this.lblUpTo.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            this.lblUpTo.Location = new System.Drawing.Point(1139, 27);
            this.lblUpTo.Name = "lblUpTo";
            this.lblUpTo.Size = new System.Drawing.Size(47, 17);
            this.lblUpTo.TabIndex = 1;
            this.lblUpTo.Tag = "NonPaintable";
            this.lblUpTo.Text = "Hasta";
            this.lblUpTo.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // dateTimePickerSince
            // 
            this.dateTimePickerSince.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.dateTimePickerSince.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            this.dateTimePickerSince.Location = new System.Drawing.Point(948, 24);
            this.dateTimePickerSince.Name = "dateTimePickerSince";
            this.dateTimePickerSince.Size = new System.Drawing.Size(185, 23);
            this.dateTimePickerSince.TabIndex = 0;
            // 
            // lblSince
            // 
            this.lblSince.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.lblSince.AutoSize = true;
            this.lblSince.BackColor = System.Drawing.Color.Transparent;
            this.lblSince.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            this.lblSince.Location = new System.Drawing.Point(861, 27);
            this.lblSince.Name = "lblSince";
            this.lblSince.Size = new System.Drawing.Size(81, 17);
            this.lblSince.TabIndex = 1;
            this.lblSince.Tag = "NonPaintable";
            this.lblSince.Text = "Desde";
            this.lblSince.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // panelHorDivider
            // 
            this.panelHorDivider.BackColor = System.Drawing.Color.Transparent;
            this.panelHorDivider.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelHorDivider.Location = new System.Drawing.Point(245, 88);
            this.panelHorDivider.Name = "panelHorDivider";
            this.panelHorDivider.Size = new System.Drawing.Size(5, 447);
            this.panelHorDivider.TabIndex = 25;
            this.panelHorDivider.Tag = "NonPaintable";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Transparent;
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 83);
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
            // CustomDGVForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1376, 535);
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
            this.tableLayoutPanelFilters.ResumeLayout(false);
            this.tableLayoutPanelSearchControls.ResumeLayout(false);
            this.tableLayoutPanelSearchControls.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView mainDGV;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelFilters;
        private System.Windows.Forms.CheckedListBox checkedListBoxFilters;
        private System.Windows.Forms.Button btnCleanFilters;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelSearchControls;
        private System.Windows.Forms.ComboBox cbColumnsName;
        private System.Windows.Forms.Button btnShowFilters;
        private System.Windows.Forms.Label lblUpTo;
        private System.Windows.Forms.Label lblSince;
        private System.Windows.Forms.Button btnApplyFilter;
        private System.Windows.Forms.DateTimePicker dateTimePickerUpTo;
        private System.Windows.Forms.DateTimePicker dateTimePickerSince;
        private System.Windows.Forms.RadioButton radioButton1;
        private System.Windows.Forms.RadioButton radioButton2;
        private System.Windows.Forms.RadioButton radioButton3;
        private System.Windows.Forms.Panel panelHorDivider;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.TextBox tbSearchBar;
    }
}