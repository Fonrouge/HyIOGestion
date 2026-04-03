namespace WinformsUI.Forms.SupplierCRUDL
{
    partial class UpdateSupplierForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UpdateSupplierForm));
            this.tlpTitleBar = new System.Windows.Forms.TableLayoutPanel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnMinimize = new System.Windows.Forms.Button();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.label2 = new System.Windows.Forms.Label();
            this.txtCompanyName = new System.Windows.Forms.TextBox();
            this.txtContactName = new System.Windows.Forms.TextBox();
            this.txtTaxId = new System.Windows.Forms.TextBox();
            this.txtTaxNumber = new System.Windows.Forms.TextBox();
            this.txtAddress = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnEditCompanyName = new System.Windows.Forms.Button();
            this.btnEditContactName = new System.Windows.Forms.Button();
            this.btnEditTaxNumber = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.txtPhone = new System.Windows.Forms.TextBox();
            this.btnEditPhone = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.btnEditAddress = new System.Windows.Forms.Button();
            this.btnEditTaxId = new System.Windows.Forms.Button();
            this.btnConfirm = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.txtMail = new System.Windows.Forms.TextBox();
            this.btnEditMail = new System.Windows.Forms.Button();
            this.pnlBackground = new System.Windows.Forms.Panel();
            this.label6 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.txtCity = new System.Windows.Forms.TextBox();
            this.txtObservations = new System.Windows.Forms.TextBox();
            this.btnEditCity = new System.Windows.Forms.Button();
            this.btnEditObservations = new System.Windows.Forms.Button();
            this.tlpTitleBar.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.pnlBackground.SuspendLayout();
            this.SuspendLayout();
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
            this.tlpTitleBar.Controls.Add(this.btnClose, 3, 0);
            this.tlpTitleBar.Controls.Add(this.btnMinimize, 2, 0);
            this.tlpTitleBar.Dock = System.Windows.Forms.DockStyle.Top;
            this.tlpTitleBar.Location = new System.Drawing.Point(0, 0);
            this.tlpTitleBar.Margin = new System.Windows.Forms.Padding(0);
            this.tlpTitleBar.Name = "tlpTitleBar";
            this.tlpTitleBar.RowCount = 1;
            this.tlpTitleBar.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpTitleBar.Size = new System.Drawing.Size(523, 31);
            this.tlpTitleBar.TabIndex = 28;
            this.tlpTitleBar.Tag = "InternalTitleBar";
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
            // btnClose
            // 
            this.btnClose.BackColor = System.Drawing.Color.Transparent;
            this.btnClose.BackgroundImage = global::WinformsUI.Properties.Resources.btnCloseModern;
            this.btnClose.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnClose.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnClose.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnClose.FlatAppearance.BorderSize = 0;
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.Location = new System.Drawing.Point(491, 2);
            this.btnClose.Margin = new System.Windows.Forms.Padding(2);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(30, 27);
            this.btnClose.TabIndex = 0;
            this.btnClose.Tag = "NonPaintable";
            this.btnClose.UseVisualStyleBackColor = false;
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
            this.btnMinimize.Location = new System.Drawing.Point(457, 2);
            this.btnMinimize.Margin = new System.Windows.Forms.Padding(2);
            this.btnMinimize.Name = "btnMinimize";
            this.btnMinimize.Size = new System.Drawing.Size(30, 27);
            this.btnMinimize.TabIndex = 0;
            this.btnMinimize.Tag = "NonPaintable";
            this.btnMinimize.UseVisualStyleBackColor = false;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 104F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.txtCompanyName, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.txtContactName, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.txtTaxId, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.txtTaxNumber, 1, 4);
            this.tableLayoutPanel1.Controls.Add(this.txtAddress, 1, 7);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.btnEditCompanyName, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.btnEditContactName, 2, 2);
            this.tableLayoutPanel1.Controls.Add(this.btnEditTaxNumber, 2, 4);
            this.tableLayoutPanel1.Controls.Add(this.label4, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.label3, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.label10, 0, 6);
            this.tableLayoutPanel1.Controls.Add(this.txtPhone, 1, 6);
            this.tableLayoutPanel1.Controls.Add(this.btnEditPhone, 2, 6);
            this.tableLayoutPanel1.Controls.Add(this.label7, 0, 7);
            this.tableLayoutPanel1.Controls.Add(this.btnEditAddress, 2, 7);
            this.tableLayoutPanel1.Controls.Add(this.btnEditTaxId, 2, 3);
            this.tableLayoutPanel1.Controls.Add(this.btnConfirm, 0, 10);
            this.tableLayoutPanel1.Controls.Add(this.label5, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.txtMail, 1, 5);
            this.tableLayoutPanel1.Controls.Add(this.btnEditMail, 2, 5);
            this.tableLayoutPanel1.Controls.Add(this.label6, 0, 8);
            this.tableLayoutPanel1.Controls.Add(this.label8, 0, 9);
            this.tableLayoutPanel1.Controls.Add(this.txtCity, 1, 8);
            this.tableLayoutPanel1.Controls.Add(this.btnEditCity, 2, 8);
            this.tableLayoutPanel1.Controls.Add(this.btnEditObservations, 2, 9);
            this.tableLayoutPanel1.Controls.Add(this.txtObservations, 1, 9);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 11;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 46F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 46F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 46F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 46F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 46F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 46F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 46F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 46F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 105F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(517, 619);
            this.tableLayoutPanel1.TabIndex = 24;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(3, 106);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(98, 46);
            this.label2.TabIndex = 1;
            this.label2.Text = "Nombre contacto";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtCompanyName
            // 
            this.txtCompanyName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtCompanyName.Enabled = false;
            this.txtCompanyName.Font = new System.Drawing.Font("Microsoft YaHei UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCompanyName.Location = new System.Drawing.Point(107, 69);
            this.txtCompanyName.Name = "txtCompanyName";
            this.txtCompanyName.ReadOnly = true;
            this.txtCompanyName.Size = new System.Drawing.Size(372, 27);
            this.txtCompanyName.TabIndex = 11;
            // 
            // txtContactName
            // 
            this.txtContactName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtContactName.Enabled = false;
            this.txtContactName.Font = new System.Drawing.Font("Microsoft YaHei UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtContactName.Location = new System.Drawing.Point(107, 115);
            this.txtContactName.Name = "txtContactName";
            this.txtContactName.ReadOnly = true;
            this.txtContactName.Size = new System.Drawing.Size(372, 27);
            this.txtContactName.TabIndex = 12;
            // 
            // txtTaxId
            // 
            this.txtTaxId.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtTaxId.Enabled = false;
            this.txtTaxId.Font = new System.Drawing.Font("Microsoft YaHei UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTaxId.Location = new System.Drawing.Point(107, 161);
            this.txtTaxId.Name = "txtTaxId";
            this.txtTaxId.ReadOnly = true;
            this.txtTaxId.Size = new System.Drawing.Size(372, 27);
            this.txtTaxId.TabIndex = 13;
            // 
            // txtTaxNumber
            // 
            this.txtTaxNumber.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtTaxNumber.Enabled = false;
            this.txtTaxNumber.Font = new System.Drawing.Font("Microsoft YaHei UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTaxNumber.Location = new System.Drawing.Point(107, 207);
            this.txtTaxNumber.Name = "txtTaxNumber";
            this.txtTaxNumber.ReadOnly = true;
            this.txtTaxNumber.Size = new System.Drawing.Size(372, 27);
            this.txtTaxNumber.TabIndex = 14;
            // 
            // txtAddress
            // 
            this.txtAddress.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtAddress.Enabled = false;
            this.txtAddress.Font = new System.Drawing.Font("Microsoft YaHei UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtAddress.Location = new System.Drawing.Point(107, 345);
            this.txtAddress.Name = "txtAddress";
            this.txtAddress.ReadOnly = true;
            this.txtAddress.Size = new System.Drawing.Size(372, 27);
            this.txtAddress.TabIndex = 16;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(3, 60);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(98, 46);
            this.label1.TabIndex = 0;
            this.label1.Text = "Nombre empresa";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnEditCompanyName
            // 
            this.btnEditCompanyName.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnEditCompanyName.BackgroundImage")));
            this.btnEditCompanyName.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnEditCompanyName.Location = new System.Drawing.Point(485, 65);
            this.btnEditCompanyName.Margin = new System.Windows.Forms.Padding(3, 5, 3, 6);
            this.btnEditCompanyName.Name = "btnEditCompanyName";
            this.btnEditCompanyName.Size = new System.Drawing.Size(29, 29);
            this.btnEditCompanyName.TabIndex = 22;
            this.btnEditCompanyName.UseVisualStyleBackColor = true;
            // 
            // btnEditContactName
            // 
            this.btnEditContactName.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnEditContactName.BackgroundImage")));
            this.btnEditContactName.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnEditContactName.Location = new System.Drawing.Point(485, 111);
            this.btnEditContactName.Margin = new System.Windows.Forms.Padding(3, 5, 3, 6);
            this.btnEditContactName.Name = "btnEditContactName";
            this.btnEditContactName.Size = new System.Drawing.Size(29, 29);
            this.btnEditContactName.TabIndex = 22;
            this.btnEditContactName.UseVisualStyleBackColor = true;
            // 
            // btnEditTaxNumber
            // 
            this.btnEditTaxNumber.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnEditTaxNumber.BackgroundImage")));
            this.btnEditTaxNumber.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnEditTaxNumber.Location = new System.Drawing.Point(485, 203);
            this.btnEditTaxNumber.Margin = new System.Windows.Forms.Padding(3, 5, 3, 6);
            this.btnEditTaxNumber.Name = "btnEditTaxNumber";
            this.btnEditTaxNumber.Size = new System.Drawing.Size(29, 29);
            this.btnEditTaxNumber.TabIndex = 22;
            this.btnEditTaxNumber.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label4.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(3, 152);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(98, 46);
            this.label4.TabIndex = 3;
            this.label4.Text = "Tipo documento";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label3.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(3, 198);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(98, 46);
            this.label3.TabIndex = 26;
            this.label3.Text = "Número documento";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label10.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(3, 290);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(98, 46);
            this.label10.TabIndex = 9;
            this.label10.Text = "Teléfono";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtPhone
            // 
            this.txtPhone.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtPhone.Enabled = false;
            this.txtPhone.Font = new System.Drawing.Font("Microsoft YaHei UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPhone.Location = new System.Drawing.Point(107, 299);
            this.txtPhone.Name = "txtPhone";
            this.txtPhone.ReadOnly = true;
            this.txtPhone.Size = new System.Drawing.Size(372, 27);
            this.txtPhone.TabIndex = 20;
            // 
            // btnEditPhone
            // 
            this.btnEditPhone.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnEditPhone.BackgroundImage")));
            this.btnEditPhone.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnEditPhone.Location = new System.Drawing.Point(485, 295);
            this.btnEditPhone.Margin = new System.Windows.Forms.Padding(3, 5, 3, 6);
            this.btnEditPhone.Name = "btnEditPhone";
            this.btnEditPhone.Size = new System.Drawing.Size(29, 29);
            this.btnEditPhone.TabIndex = 22;
            this.btnEditPhone.UseVisualStyleBackColor = true;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label7.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(3, 336);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(98, 46);
            this.label7.TabIndex = 6;
            this.label7.Text = "Dirección";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnEditAddress
            // 
            this.btnEditAddress.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnEditAddress.BackgroundImage")));
            this.btnEditAddress.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnEditAddress.Location = new System.Drawing.Point(485, 341);
            this.btnEditAddress.Margin = new System.Windows.Forms.Padding(3, 5, 3, 6);
            this.btnEditAddress.Name = "btnEditAddress";
            this.btnEditAddress.Size = new System.Drawing.Size(29, 29);
            this.btnEditAddress.TabIndex = 22;
            this.btnEditAddress.UseVisualStyleBackColor = true;
            // 
            // btnEditTaxId
            // 
            this.btnEditTaxId.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnEditTaxId.BackgroundImage")));
            this.btnEditTaxId.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnEditTaxId.Location = new System.Drawing.Point(485, 157);
            this.btnEditTaxId.Margin = new System.Windows.Forms.Padding(3, 5, 3, 6);
            this.btnEditTaxId.Name = "btnEditTaxId";
            this.btnEditTaxId.Size = new System.Drawing.Size(29, 29);
            this.btnEditTaxId.TabIndex = 22;
            this.btnEditTaxId.UseVisualStyleBackColor = true;
            // 
            // btnConfirm
            // 
            this.btnConfirm.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.SetColumnSpan(this.btnConfirm, 3);
            this.btnConfirm.Location = new System.Drawing.Point(4, 544);
            this.btnConfirm.Margin = new System.Windows.Forms.Padding(4);
            this.btnConfirm.Name = "btnConfirm";
            this.btnConfirm.Size = new System.Drawing.Size(509, 64);
            this.btnConfirm.TabIndex = 25;
            this.btnConfirm.Tag = "Accentuable";
            this.btnConfirm.Text = "Confirmar";
            this.btnConfirm.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label5.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(3, 244);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(98, 46);
            this.label5.TabIndex = 26;
            this.label5.Text = "Email";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtMail
            // 
            this.txtMail.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtMail.Enabled = false;
            this.txtMail.Font = new System.Drawing.Font("Microsoft YaHei UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtMail.Location = new System.Drawing.Point(107, 253);
            this.txtMail.Name = "txtMail";
            this.txtMail.ReadOnly = true;
            this.txtMail.Size = new System.Drawing.Size(372, 27);
            this.txtMail.TabIndex = 14;
            // 
            // btnEditMail
            // 
            this.btnEditMail.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnEditMail.BackgroundImage")));
            this.btnEditMail.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnEditMail.Location = new System.Drawing.Point(485, 249);
            this.btnEditMail.Margin = new System.Windows.Forms.Padding(3, 5, 3, 6);
            this.btnEditMail.Name = "btnEditMail";
            this.btnEditMail.Size = new System.Drawing.Size(29, 29);
            this.btnEditMail.TabIndex = 22;
            this.btnEditMail.UseVisualStyleBackColor = true;
            // 
            // pnlBackground
            // 
            this.pnlBackground.Controls.Add(this.tableLayoutPanel1);
            this.pnlBackground.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlBackground.Location = new System.Drawing.Point(0, 0);
            this.pnlBackground.Name = "pnlBackground";
            this.pnlBackground.Padding = new System.Windows.Forms.Padding(3, 0, 3, 3);
            this.pnlBackground.Size = new System.Drawing.Size(523, 622);
            this.pnlBackground.TabIndex = 29;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label6.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(3, 382);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(98, 46);
            this.label6.TabIndex = 6;
            this.label6.Text = "Ciudad";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label8.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(3, 432);
            this.label8.Margin = new System.Windows.Forms.Padding(3, 4, 3, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(98, 101);
            this.label8.TabIndex = 6;
            this.label8.Text = "Observaciones";
            this.label8.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // txtCity
            // 
            this.txtCity.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtCity.Enabled = false;
            this.txtCity.Font = new System.Drawing.Font("Microsoft YaHei UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCity.Location = new System.Drawing.Point(107, 391);
            this.txtCity.Name = "txtCity";
            this.txtCity.ReadOnly = true;
            this.txtCity.Size = new System.Drawing.Size(372, 27);
            this.txtCity.TabIndex = 16;
            // 
            // txtObservations
            // 
            this.txtObservations.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtObservations.Enabled = false;
            this.txtObservations.Font = new System.Drawing.Font("Microsoft YaHei UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtObservations.Location = new System.Drawing.Point(107, 436);
            this.txtObservations.Multiline = true;
            this.txtObservations.Name = "txtObservations";
            this.txtObservations.ReadOnly = true;
            this.txtObservations.Size = new System.Drawing.Size(372, 89);
            this.txtObservations.TabIndex = 16;
            // 
            // btnEditCity
            // 
            this.btnEditCity.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnEditCity.BackgroundImage")));
            this.btnEditCity.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnEditCity.Location = new System.Drawing.Point(485, 387);
            this.btnEditCity.Margin = new System.Windows.Forms.Padding(3, 5, 3, 6);
            this.btnEditCity.Name = "btnEditCity";
            this.btnEditCity.Size = new System.Drawing.Size(29, 29);
            this.btnEditCity.TabIndex = 22;
            this.btnEditCity.UseVisualStyleBackColor = true;
            // 
            // btnEditObservations
            // 
            this.btnEditObservations.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.btnEditObservations.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnEditObservations.BackgroundImage")));
            this.btnEditObservations.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnEditObservations.Location = new System.Drawing.Point(485, 465);
            this.btnEditObservations.Margin = new System.Windows.Forms.Padding(3, 5, 3, 6);
            this.btnEditObservations.Name = "btnEditObservations";
            this.btnEditObservations.Size = new System.Drawing.Size(29, 29);
            this.btnEditObservations.TabIndex = 22;
            this.btnEditObservations.UseVisualStyleBackColor = true;
            // 
            // UpdateSupplierForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(523, 622);
            this.Controls.Add(this.tlpTitleBar);
            this.Controls.Add(this.pnlBackground);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "UpdateSupplierForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "UpdateSupplierForm";
            this.tlpTitleBar.ResumeLayout(false);
            this.tlpTitleBar.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.pnlBackground.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        protected System.Windows.Forms.TableLayoutPanel tlpTitleBar;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnMinimize;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtCompanyName;
        private System.Windows.Forms.TextBox txtContactName;
        private System.Windows.Forms.TextBox txtTaxId;
        private System.Windows.Forms.TextBox txtTaxNumber;
        private System.Windows.Forms.TextBox txtAddress;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnEditCompanyName;
        private System.Windows.Forms.Button btnEditContactName;
        private System.Windows.Forms.Button btnEditTaxNumber;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox txtPhone;
        private System.Windows.Forms.Button btnEditPhone;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button btnEditAddress;
        private System.Windows.Forms.Button btnEditTaxId;
        private System.Windows.Forms.Button btnConfirm;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtMail;
        private System.Windows.Forms.Button btnEditMail;
        private System.Windows.Forms.Panel pnlBackground;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtCity;
        private System.Windows.Forms.Button btnEditCity;
        private System.Windows.Forms.Button btnEditObservations;
        private System.Windows.Forms.TextBox txtObservations;
    }
}