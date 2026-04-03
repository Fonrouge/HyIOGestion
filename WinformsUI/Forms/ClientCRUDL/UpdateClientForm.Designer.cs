namespace WinformsUI.Forms.ClientCRUDL
{
    partial class UpdateClientForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UpdateClientForm));
            this.tlpTitleBar = new System.Windows.Forms.TableLayoutPanel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnMinimize = new System.Windows.Forms.Button();
            this.pnlBackground = new System.Windows.Forms.Panel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.txtLastName = new System.Windows.Forms.TextBox();
            this.txtDocType = new System.Windows.Forms.TextBox();
            this.txtDocNumber = new System.Windows.Forms.TextBox();
            this.txtShipCountry = new System.Windows.Forms.TextBox();
            this.txtShipState = new System.Windows.Forms.TextBox();
            this.txtShipAddress = new System.Windows.Forms.TextBox();
            this.txtZipCode = new System.Windows.Forms.TextBox();
            this.txtEmail = new System.Windows.Forms.TextBox();
            this.txtPhone = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnEditName = new System.Windows.Forms.Button();
            this.btnEditLastName = new System.Windows.Forms.Button();
            this.btnEditDocType = new System.Windows.Forms.Button();
            this.btnEditDocNumber = new System.Windows.Forms.Button();
            this.btnEditShipCountry = new System.Windows.Forms.Button();
            this.btnEditShipState = new System.Windows.Forms.Button();
            this.btnEditShipAddress = new System.Windows.Forms.Button();
            this.btnEditZipCode = new System.Windows.Forms.Button();
            this.btnEditEmail = new System.Windows.Forms.Button();
            this.btnEditPhone = new System.Windows.Forms.Button();
            this.btnEditObservations = new System.Windows.Forms.Button();
            this.txtObservations = new System.Windows.Forms.TextBox();
            this.btnConfirm = new System.Windows.Forms.Button();
            this.tlpTitleBar.SuspendLayout();
            this.pnlBackground.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
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
            this.tlpTitleBar.Size = new System.Drawing.Size(567, 31);
            this.tlpTitleBar.TabIndex = 24;
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
            this.btnClose.Location = new System.Drawing.Point(535, 2);
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
            this.btnMinimize.Location = new System.Drawing.Point(501, 2);
            this.btnMinimize.Margin = new System.Windows.Forms.Padding(2);
            this.btnMinimize.Name = "btnMinimize";
            this.btnMinimize.Size = new System.Drawing.Size(30, 27);
            this.btnMinimize.TabIndex = 0;
            this.btnMinimize.Tag = "NonPaintable";
            this.btnMinimize.UseVisualStyleBackColor = false;
            // 
            // pnlBackground
            // 
            this.pnlBackground.Controls.Add(this.tableLayoutPanel1);
            this.pnlBackground.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlBackground.Location = new System.Drawing.Point(0, 31);
            this.pnlBackground.Name = "pnlBackground";
            this.pnlBackground.Padding = new System.Windows.Forms.Padding(3, 0, 3, 3);
            this.pnlBackground.Size = new System.Drawing.Size(567, 596);
            this.pnlBackground.TabIndex = 25;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.label3, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.label4, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.label5, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.label6, 0, 6);
            this.tableLayoutPanel1.Controls.Add(this.label7, 0, 7);
            this.tableLayoutPanel1.Controls.Add(this.label8, 0, 8);
            this.tableLayoutPanel1.Controls.Add(this.label9, 0, 9);
            this.tableLayoutPanel1.Controls.Add(this.label10, 0, 10);
            this.tableLayoutPanel1.Controls.Add(this.label11, 0, 11);
            this.tableLayoutPanel1.Controls.Add(this.txtName, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.txtLastName, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.txtDocType, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.txtDocNumber, 1, 4);
            this.tableLayoutPanel1.Controls.Add(this.txtShipCountry, 1, 5);
            this.tableLayoutPanel1.Controls.Add(this.txtShipState, 1, 6);
            this.tableLayoutPanel1.Controls.Add(this.txtShipAddress, 1, 7);
            this.tableLayoutPanel1.Controls.Add(this.txtZipCode, 1, 8);
            this.tableLayoutPanel1.Controls.Add(this.txtEmail, 1, 9);
            this.tableLayoutPanel1.Controls.Add(this.txtPhone, 1, 10);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.btnEditName, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.btnEditLastName, 2, 2);
            this.tableLayoutPanel1.Controls.Add(this.btnEditDocType, 2, 3);
            this.tableLayoutPanel1.Controls.Add(this.btnEditDocNumber, 2, 4);
            this.tableLayoutPanel1.Controls.Add(this.btnEditShipCountry, 2, 5);
            this.tableLayoutPanel1.Controls.Add(this.btnEditShipState, 2, 6);
            this.tableLayoutPanel1.Controls.Add(this.btnEditShipAddress, 2, 7);
            this.tableLayoutPanel1.Controls.Add(this.btnEditZipCode, 2, 8);
            this.tableLayoutPanel1.Controls.Add(this.btnEditEmail, 2, 9);
            this.tableLayoutPanel1.Controls.Add(this.btnEditPhone, 2, 10);
            this.tableLayoutPanel1.Controls.Add(this.btnEditObservations, 2, 11);
            this.tableLayoutPanel1.Controls.Add(this.txtObservations, 1, 11);
            this.tableLayoutPanel1.Controls.Add(this.btnConfirm, 0, 12);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 13;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 112F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(561, 593);
            this.tableLayoutPanel1.TabIndex = 24;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(3, 80);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(94, 40);
            this.label2.TabIndex = 1;
            this.label2.Text = "Apellido";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label3.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(3, 120);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(94, 40);
            this.label3.TabIndex = 2;
            this.label3.Text = "Tipo doc";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label4.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(3, 160);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(94, 40);
            this.label4.TabIndex = 3;
            this.label4.Text = "Nº Doc";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label5.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(3, 200);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(94, 40);
            this.label5.TabIndex = 4;
            this.label5.Text = "País de envío";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label6.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(3, 240);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(94, 40);
            this.label6.TabIndex = 5;
            this.label6.Text = "Pcia de envío";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label7.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(3, 280);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(94, 40);
            this.label7.TabIndex = 6;
            this.label7.Text = "Dirección de envío";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label8.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(3, 320);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(94, 40);
            this.label8.TabIndex = 7;
            this.label8.Text = "Código postal";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label9.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(3, 360);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(94, 40);
            this.label9.TabIndex = 8;
            this.label9.Text = "Email";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label10.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(3, 400);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(94, 40);
            this.label10.TabIndex = 9;
            this.label10.Text = "Teléfono";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label11.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.Location = new System.Drawing.Point(3, 449);
            this.label11.Margin = new System.Windows.Forms.Padding(3, 9, 3, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(94, 103);
            this.label11.TabIndex = 10;
            this.label11.Text = "Observaciones";
            this.label11.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // txtName
            // 
            this.txtName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtName.Enabled = false;
            this.txtName.Font = new System.Drawing.Font("Microsoft YaHei UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtName.Location = new System.Drawing.Point(103, 46);
            this.txtName.Name = "txtName";
            this.txtName.ReadOnly = true;
            this.txtName.Size = new System.Drawing.Size(420, 27);
            this.txtName.TabIndex = 11;
            // 
            // txtLastName
            // 
            this.txtLastName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtLastName.Enabled = false;
            this.txtLastName.Font = new System.Drawing.Font("Microsoft YaHei UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtLastName.Location = new System.Drawing.Point(103, 86);
            this.txtLastName.Name = "txtLastName";
            this.txtLastName.ReadOnly = true;
            this.txtLastName.Size = new System.Drawing.Size(420, 27);
            this.txtLastName.TabIndex = 12;
            // 
            // txtDocType
            // 
            this.txtDocType.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtDocType.Enabled = false;
            this.txtDocType.Font = new System.Drawing.Font("Microsoft YaHei UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDocType.Location = new System.Drawing.Point(103, 126);
            this.txtDocType.Name = "txtDocType";
            this.txtDocType.ReadOnly = true;
            this.txtDocType.Size = new System.Drawing.Size(420, 27);
            this.txtDocType.TabIndex = 13;
            // 
            // txtDocNumber
            // 
            this.txtDocNumber.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtDocNumber.Enabled = false;
            this.txtDocNumber.Font = new System.Drawing.Font("Microsoft YaHei UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDocNumber.Location = new System.Drawing.Point(103, 166);
            this.txtDocNumber.Name = "txtDocNumber";
            this.txtDocNumber.ReadOnly = true;
            this.txtDocNumber.Size = new System.Drawing.Size(420, 27);
            this.txtDocNumber.TabIndex = 14;
            // 
            // txtShipCountry
            // 
            this.txtShipCountry.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtShipCountry.Enabled = false;
            this.txtShipCountry.Font = new System.Drawing.Font("Microsoft YaHei UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtShipCountry.Location = new System.Drawing.Point(103, 206);
            this.txtShipCountry.Name = "txtShipCountry";
            this.txtShipCountry.ReadOnly = true;
            this.txtShipCountry.Size = new System.Drawing.Size(420, 27);
            this.txtShipCountry.TabIndex = 15;
            // 
            // txtShipState
            // 
            this.txtShipState.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtShipState.Enabled = false;
            this.txtShipState.Font = new System.Drawing.Font("Microsoft YaHei UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtShipState.Location = new System.Drawing.Point(103, 246);
            this.txtShipState.Name = "txtShipState";
            this.txtShipState.ReadOnly = true;
            this.txtShipState.Size = new System.Drawing.Size(420, 27);
            this.txtShipState.TabIndex = 16;
            // 
            // txtShipAddress
            // 
            this.txtShipAddress.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtShipAddress.Enabled = false;
            this.txtShipAddress.Font = new System.Drawing.Font("Microsoft YaHei UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtShipAddress.Location = new System.Drawing.Point(103, 286);
            this.txtShipAddress.Name = "txtShipAddress";
            this.txtShipAddress.ReadOnly = true;
            this.txtShipAddress.Size = new System.Drawing.Size(420, 27);
            this.txtShipAddress.TabIndex = 17;
            // 
            // txtZipCode
            // 
            this.txtZipCode.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtZipCode.Enabled = false;
            this.txtZipCode.Font = new System.Drawing.Font("Microsoft YaHei UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtZipCode.Location = new System.Drawing.Point(103, 326);
            this.txtZipCode.Name = "txtZipCode";
            this.txtZipCode.ReadOnly = true;
            this.txtZipCode.Size = new System.Drawing.Size(420, 27);
            this.txtZipCode.TabIndex = 18;
            // 
            // txtEmail
            // 
            this.txtEmail.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtEmail.Enabled = false;
            this.txtEmail.Font = new System.Drawing.Font("Microsoft YaHei UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtEmail.Location = new System.Drawing.Point(103, 366);
            this.txtEmail.Name = "txtEmail";
            this.txtEmail.ReadOnly = true;
            this.txtEmail.Size = new System.Drawing.Size(420, 27);
            this.txtEmail.TabIndex = 19;
            // 
            // txtPhone
            // 
            this.txtPhone.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtPhone.Enabled = false;
            this.txtPhone.Font = new System.Drawing.Font("Microsoft YaHei UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPhone.Location = new System.Drawing.Point(103, 406);
            this.txtPhone.Name = "txtPhone";
            this.txtPhone.ReadOnly = true;
            this.txtPhone.Size = new System.Drawing.Size(420, 27);
            this.txtPhone.TabIndex = 20;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(3, 40);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(94, 40);
            this.label1.TabIndex = 0;
            this.label1.Text = "Nombre";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnEditName
            // 
            this.btnEditName.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnEditName.BackgroundImage")));
            this.btnEditName.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnEditName.Location = new System.Drawing.Point(529, 45);
            this.btnEditName.Margin = new System.Windows.Forms.Padding(3, 5, 3, 6);
            this.btnEditName.Name = "btnEditName";
            this.btnEditName.Size = new System.Drawing.Size(29, 29);
            this.btnEditName.TabIndex = 22;
            this.btnEditName.UseVisualStyleBackColor = true;
            // 
            // btnEditLastName
            // 
            this.btnEditLastName.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnEditLastName.BackgroundImage")));
            this.btnEditLastName.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnEditLastName.Location = new System.Drawing.Point(529, 85);
            this.btnEditLastName.Margin = new System.Windows.Forms.Padding(3, 5, 3, 6);
            this.btnEditLastName.Name = "btnEditLastName";
            this.btnEditLastName.Size = new System.Drawing.Size(29, 29);
            this.btnEditLastName.TabIndex = 22;
            this.btnEditLastName.UseVisualStyleBackColor = true;
            // 
            // btnEditDocType
            // 
            this.btnEditDocType.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnEditDocType.BackgroundImage")));
            this.btnEditDocType.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnEditDocType.Location = new System.Drawing.Point(529, 125);
            this.btnEditDocType.Margin = new System.Windows.Forms.Padding(3, 5, 3, 6);
            this.btnEditDocType.Name = "btnEditDocType";
            this.btnEditDocType.Size = new System.Drawing.Size(29, 29);
            this.btnEditDocType.TabIndex = 22;
            this.btnEditDocType.UseVisualStyleBackColor = true;
            // 
            // btnEditDocNumber
            // 
            this.btnEditDocNumber.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnEditDocNumber.BackgroundImage")));
            this.btnEditDocNumber.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnEditDocNumber.Location = new System.Drawing.Point(529, 165);
            this.btnEditDocNumber.Margin = new System.Windows.Forms.Padding(3, 5, 3, 6);
            this.btnEditDocNumber.Name = "btnEditDocNumber";
            this.btnEditDocNumber.Size = new System.Drawing.Size(29, 29);
            this.btnEditDocNumber.TabIndex = 22;
            this.btnEditDocNumber.UseVisualStyleBackColor = true;
            // 
            // btnEditShipCountry
            // 
            this.btnEditShipCountry.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnEditShipCountry.BackgroundImage")));
            this.btnEditShipCountry.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnEditShipCountry.Location = new System.Drawing.Point(529, 205);
            this.btnEditShipCountry.Margin = new System.Windows.Forms.Padding(3, 5, 3, 6);
            this.btnEditShipCountry.Name = "btnEditShipCountry";
            this.btnEditShipCountry.Size = new System.Drawing.Size(29, 29);
            this.btnEditShipCountry.TabIndex = 22;
            this.btnEditShipCountry.UseVisualStyleBackColor = true;
            // 
            // btnEditShipState
            // 
            this.btnEditShipState.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnEditShipState.BackgroundImage")));
            this.btnEditShipState.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnEditShipState.Location = new System.Drawing.Point(529, 245);
            this.btnEditShipState.Margin = new System.Windows.Forms.Padding(3, 5, 3, 6);
            this.btnEditShipState.Name = "btnEditShipState";
            this.btnEditShipState.Size = new System.Drawing.Size(29, 29);
            this.btnEditShipState.TabIndex = 22;
            this.btnEditShipState.UseVisualStyleBackColor = true;
            // 
            // btnEditShipAddress
            // 
            this.btnEditShipAddress.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnEditShipAddress.BackgroundImage")));
            this.btnEditShipAddress.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnEditShipAddress.Location = new System.Drawing.Point(529, 285);
            this.btnEditShipAddress.Margin = new System.Windows.Forms.Padding(3, 5, 3, 6);
            this.btnEditShipAddress.Name = "btnEditShipAddress";
            this.btnEditShipAddress.Size = new System.Drawing.Size(29, 29);
            this.btnEditShipAddress.TabIndex = 22;
            this.btnEditShipAddress.UseVisualStyleBackColor = true;
            // 
            // btnEditZipCode
            // 
            this.btnEditZipCode.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnEditZipCode.BackgroundImage")));
            this.btnEditZipCode.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnEditZipCode.Location = new System.Drawing.Point(529, 325);
            this.btnEditZipCode.Margin = new System.Windows.Forms.Padding(3, 5, 3, 6);
            this.btnEditZipCode.Name = "btnEditZipCode";
            this.btnEditZipCode.Size = new System.Drawing.Size(29, 29);
            this.btnEditZipCode.TabIndex = 22;
            this.btnEditZipCode.UseVisualStyleBackColor = true;
            // 
            // btnEditEmail
            // 
            this.btnEditEmail.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnEditEmail.BackgroundImage")));
            this.btnEditEmail.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnEditEmail.Location = new System.Drawing.Point(529, 365);
            this.btnEditEmail.Margin = new System.Windows.Forms.Padding(3, 5, 3, 6);
            this.btnEditEmail.Name = "btnEditEmail";
            this.btnEditEmail.Size = new System.Drawing.Size(29, 29);
            this.btnEditEmail.TabIndex = 22;
            this.btnEditEmail.UseVisualStyleBackColor = true;
            // 
            // btnEditPhone
            // 
            this.btnEditPhone.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnEditPhone.BackgroundImage")));
            this.btnEditPhone.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnEditPhone.Location = new System.Drawing.Point(529, 405);
            this.btnEditPhone.Margin = new System.Windows.Forms.Padding(3, 5, 3, 6);
            this.btnEditPhone.Name = "btnEditPhone";
            this.btnEditPhone.Size = new System.Drawing.Size(29, 29);
            this.btnEditPhone.TabIndex = 22;
            this.btnEditPhone.UseVisualStyleBackColor = true;
            // 
            // btnEditObservations
            // 
            this.btnEditObservations.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.btnEditObservations.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnEditObservations.BackgroundImage")));
            this.btnEditObservations.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnEditObservations.Location = new System.Drawing.Point(529, 481);
            this.btnEditObservations.Name = "btnEditObservations";
            this.btnEditObservations.Size = new System.Drawing.Size(29, 29);
            this.btnEditObservations.TabIndex = 22;
            this.btnEditObservations.UseVisualStyleBackColor = true;
            // 
            // txtObservations
            // 
            this.txtObservations.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtObservations.Enabled = false;
            this.txtObservations.Font = new System.Drawing.Font("Microsoft YaHei UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtObservations.Location = new System.Drawing.Point(103, 451);
            this.txtObservations.Multiline = true;
            this.txtObservations.Name = "txtObservations";
            this.txtObservations.ReadOnly = true;
            this.txtObservations.Size = new System.Drawing.Size(420, 90);
            this.txtObservations.TabIndex = 21;
            // 
            // btnConfirm
            // 
            this.btnConfirm.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.SetColumnSpan(this.btnConfirm, 3);
            this.btnConfirm.Location = new System.Drawing.Point(4, 556);
            this.btnConfirm.Margin = new System.Windows.Forms.Padding(4);
            this.btnConfirm.Name = "btnConfirm";
            this.btnConfirm.Size = new System.Drawing.Size(553, 33);
            this.btnConfirm.TabIndex = 25;
            this.btnConfirm.Tag = "Accentuable";
            this.btnConfirm.Text = "Confirmar";
            this.btnConfirm.UseVisualStyleBackColor = true;
            // 
            // UpdateClientForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(567, 627);
            this.Controls.Add(this.pnlBackground);
            this.Controls.Add(this.tlpTitleBar);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "UpdateClientForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "UpdateClientForm";
            this.tlpTitleBar.ResumeLayout(false);
            this.tlpTitleBar.PerformLayout();
            this.pnlBackground.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        protected System.Windows.Forms.TableLayoutPanel tlpTitleBar;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnMinimize;
        private System.Windows.Forms.Panel pnlBackground;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.TextBox txtLastName;
        private System.Windows.Forms.TextBox txtDocType;
        private System.Windows.Forms.TextBox txtDocNumber;
        private System.Windows.Forms.TextBox txtShipCountry;
        private System.Windows.Forms.TextBox txtShipState;
        private System.Windows.Forms.TextBox txtShipAddress;
        private System.Windows.Forms.TextBox txtZipCode;
        private System.Windows.Forms.TextBox txtEmail;
        private System.Windows.Forms.TextBox txtPhone;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnEditName;
        private System.Windows.Forms.Button btnEditLastName;
        private System.Windows.Forms.Button btnEditDocType;
        private System.Windows.Forms.Button btnEditDocNumber;
        private System.Windows.Forms.Button btnEditShipCountry;
        private System.Windows.Forms.Button btnEditShipState;
        private System.Windows.Forms.Button btnEditShipAddress;
        private System.Windows.Forms.Button btnEditZipCode;
        private System.Windows.Forms.Button btnEditEmail;
        private System.Windows.Forms.Button btnEditPhone;
        private System.Windows.Forms.Button btnEditObservations;
        private System.Windows.Forms.TextBox txtObservations;
        private System.Windows.Forms.Button btnConfirm;
    }
}