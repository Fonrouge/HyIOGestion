namespace WinformsUI.Forms.Main
{
    partial class MainForm
    {
        /// <summary>
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.txtSales = new System.Windows.Forms.Label();
            this.txtPayments = new System.Windows.Forms.Label();
            this.txtEmployees = new System.Windows.Forms.Label();
            this.txtClients = new System.Windows.Forms.Label();
            this.tlpMenu = new System.Windows.Forms.TableLayoutPanel();
            this.btnMenu = new System.Windows.Forms.Button();
            this.btnOpenClients = new System.Windows.Forms.Button();
            this.btnOpenEmployees = new System.Windows.Forms.Button();
            this.txtSuppliers = new System.Windows.Forms.Label();
            this.txtProducts = new System.Windows.Forms.Label();
            this.btnOpenProducts = new System.Windows.Forms.Button();
            this.btnOpenSuppliers = new System.Windows.Forms.Button();
            this.btnOpenSales = new System.Windows.Forms.Button();
            this.btnOpenPayments = new System.Windows.Forms.Button();
            this.btnOpenConfigs = new System.Windows.Forms.Button();
            this.txtConfigs = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.btnChangeWindowManagementMode = new System.Windows.Forms.Button();
            this.btnVerticalTileWindows = new System.Windows.Forms.Button();
            this.mainSs = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel8 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripDropDownButton1 = new System.Windows.Forms.ToolStripDropDownButton();
            this.solicitarPermisosToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.modificarCuentaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripStatusLabel6 = new System.Windows.Forms.ToolStripStatusLabel();
            this.txtLoginTime = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel3 = new System.Windows.Forms.ToolStripStatusLabel();
            this.txtCurrentUserName = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel4 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel7 = new System.Windows.Forms.ToolStripStatusLabel();
            this.DashboardPnl = new System.Windows.Forms.Panel();
            this.FLPsideTools = new System.Windows.Forms.FlowLayoutPanel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnExpand = new System.Windows.Forms.Button();
            this.btnMinimize = new System.Windows.Forms.Button();
            this.tlpMenu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.mainSs.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtSales
            // 
            this.txtSales.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.txtSales.AutoSize = true;
            this.txtSales.BackColor = System.Drawing.Color.Transparent;
            this.txtSales.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSales.ForeColor = System.Drawing.SystemColors.Control;
            this.txtSales.Location = new System.Drawing.Point(51, 461);
            this.txtSales.Name = "txtSales";
            this.txtSales.Size = new System.Drawing.Size(49, 55);
            this.txtSales.TabIndex = 10;
            this.txtSales.Tag = "NonPaintable";
            this.txtSales.Text = "Ventas";
            this.txtSales.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtPayments
            // 
            this.txtPayments.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.txtPayments.AutoSize = true;
            this.txtPayments.BackColor = System.Drawing.Color.Transparent;
            this.txtPayments.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPayments.ForeColor = System.Drawing.SystemColors.Control;
            this.txtPayments.Location = new System.Drawing.Point(51, 516);
            this.txtPayments.Name = "txtPayments";
            this.txtPayments.Size = new System.Drawing.Size(47, 55);
            this.txtPayments.TabIndex = 5;
            this.txtPayments.Tag = "NonPaintable";
            this.txtPayments.Text = "Pagos";
            this.txtPayments.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtEmployees
            // 
            this.txtEmployees.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.txtEmployees.AutoSize = true;
            this.txtEmployees.BackColor = System.Drawing.Color.Transparent;
            this.txtEmployees.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtEmployees.ForeColor = System.Drawing.SystemColors.Control;
            this.txtEmployees.Location = new System.Drawing.Point(51, 406);
            this.txtEmployees.Name = "txtEmployees";
            this.txtEmployees.Size = new System.Drawing.Size(77, 55);
            this.txtEmployees.TabIndex = 2;
            this.txtEmployees.Tag = "NonPaintable";
            this.txtEmployees.Text = "Empleados";
            this.txtEmployees.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtClients
            // 
            this.txtClients.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.txtClients.AutoSize = true;
            this.txtClients.BackColor = System.Drawing.Color.Transparent;
            this.txtClients.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtClients.ForeColor = System.Drawing.SystemColors.Control;
            this.txtClients.Location = new System.Drawing.Point(51, 351);
            this.txtClients.Name = "txtClients";
            this.txtClients.Size = new System.Drawing.Size(55, 55);
            this.txtClients.TabIndex = 1;
            this.txtClients.Tag = "NonPaintable";
            this.txtClients.Text = "Clientes";
            this.txtClients.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tlpMenu
            // 
            this.tlpMenu.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.tlpMenu.ColumnCount = 2;
            this.tlpMenu.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 48F));
            this.tlpMenu.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpMenu.Controls.Add(this.btnMenu, 0, 1);
            this.tlpMenu.Controls.Add(this.btnOpenClients, 0, 6);
            this.tlpMenu.Controls.Add(this.btnOpenEmployees, 0, 7);
            this.tlpMenu.Controls.Add(this.txtClients, 1, 6);
            this.tlpMenu.Controls.Add(this.txtEmployees, 1, 7);
            this.tlpMenu.Controls.Add(this.txtSales, 1, 8);
            this.tlpMenu.Controls.Add(this.txtSuppliers, 1, 11);
            this.tlpMenu.Controls.Add(this.txtPayments, 1, 9);
            this.tlpMenu.Controls.Add(this.txtProducts, 1, 10);
            this.tlpMenu.Controls.Add(this.btnOpenProducts, 0, 10);
            this.tlpMenu.Controls.Add(this.btnOpenSuppliers, 0, 11);
            this.tlpMenu.Controls.Add(this.btnOpenSales, 0, 8);
            this.tlpMenu.Controls.Add(this.btnOpenPayments, 0, 9);
            this.tlpMenu.Controls.Add(this.btnOpenConfigs, 0, 12);
            this.tlpMenu.Controls.Add(this.txtConfigs, 1, 12);
            this.tlpMenu.Controls.Add(this.pictureBox1, 1, 5);
            this.tlpMenu.Controls.Add(this.pictureBox2, 1, 1);
            this.tlpMenu.Controls.Add(this.btnChangeWindowManagementMode, 0, 3);
            this.tlpMenu.Controls.Add(this.btnVerticalTileWindows, 0, 4);
            this.tlpMenu.Dock = System.Windows.Forms.DockStyle.Left;
            this.tlpMenu.Location = new System.Drawing.Point(0, 0);
            this.tlpMenu.MaximumSize = new System.Drawing.Size(165, 0);
            this.tlpMenu.MinimumSize = new System.Drawing.Size(51, 0);
            this.tlpMenu.Name = "tlpMenu";
            this.tlpMenu.RowCount = 14;
            this.tlpMenu.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tlpMenu.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tlpMenu.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tlpMenu.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tlpMenu.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tlpMenu.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpMenu.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 55F));
            this.tlpMenu.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 55F));
            this.tlpMenu.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 55F));
            this.tlpMenu.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 55F));
            this.tlpMenu.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 55F));
            this.tlpMenu.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 55F));
            this.tlpMenu.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 55F));
            this.tlpMenu.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpMenu.Size = new System.Drawing.Size(165, 837);
            this.tlpMenu.TabIndex = 5;
            this.tlpMenu.Tag = "ExternalTitleBar";
            // 
            // btnMenu
            // 
            this.btnMenu.BackColor = System.Drawing.Color.Transparent;
            this.btnMenu.BackgroundImage = global::WinformsUI.Properties.Resources.menuIcon;
            this.btnMenu.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnMenu.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnMenu.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnMenu.FlatAppearance.BorderSize = 0;
            this.btnMenu.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMenu.Location = new System.Drawing.Point(3, 55);
            this.btnMenu.Margin = new System.Windows.Forms.Padding(3, 5, 3, 3);
            this.btnMenu.Name = "btnMenu";
            this.btnMenu.Size = new System.Drawing.Size(42, 42);
            this.btnMenu.TabIndex = 15;
            this.btnMenu.Tag = "NonPaintable";
            this.btnMenu.UseVisualStyleBackColor = false;
            // 
            // btnOpenClients
            // 
            this.btnOpenClients.BackColor = System.Drawing.Color.Transparent;
            this.btnOpenClients.BackgroundImage = global::WinformsUI.Properties.Resources.BusinessPartnerIcon;
            this.btnOpenClients.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnOpenClients.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnOpenClients.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnOpenClients.FlatAppearance.BorderSize = 0;
            this.btnOpenClients.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOpenClients.ForeColor = System.Drawing.Color.Transparent;
            this.btnOpenClients.Location = new System.Drawing.Point(5, 356);
            this.btnOpenClients.Margin = new System.Windows.Forms.Padding(5);
            this.btnOpenClients.Name = "btnOpenClients";
            this.btnOpenClients.Size = new System.Drawing.Size(38, 45);
            this.btnOpenClients.TabIndex = 0;
            this.btnOpenClients.Tag = "NonPaintable";
            this.btnOpenClients.UseVisualStyleBackColor = false;
            // 
            // btnOpenEmployees
            // 
            this.btnOpenEmployees.BackColor = System.Drawing.Color.Transparent;
            this.btnOpenEmployees.BackgroundImage = global::WinformsUI.Properties.Resources.EmployeeIcon;
            this.btnOpenEmployees.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnOpenEmployees.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnOpenEmployees.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnOpenEmployees.FlatAppearance.BorderSize = 0;
            this.btnOpenEmployees.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOpenEmployees.Location = new System.Drawing.Point(5, 411);
            this.btnOpenEmployees.Margin = new System.Windows.Forms.Padding(5);
            this.btnOpenEmployees.Name = "btnOpenEmployees";
            this.btnOpenEmployees.Size = new System.Drawing.Size(38, 45);
            this.btnOpenEmployees.TabIndex = 0;
            this.btnOpenEmployees.Tag = "NonPaintable";
            this.btnOpenEmployees.UseVisualStyleBackColor = false;
            // 
            // txtSuppliers
            // 
            this.txtSuppliers.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.txtSuppliers.AutoSize = true;
            this.txtSuppliers.BackColor = System.Drawing.Color.Transparent;
            this.txtSuppliers.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSuppliers.ForeColor = System.Drawing.SystemColors.Control;
            this.txtSuppliers.Location = new System.Drawing.Point(51, 626);
            this.txtSuppliers.Name = "txtSuppliers";
            this.txtSuppliers.Size = new System.Drawing.Size(86, 55);
            this.txtSuppliers.TabIndex = 5;
            this.txtSuppliers.Tag = "NonPaintable";
            this.txtSuppliers.Text = "Proveedores";
            this.txtSuppliers.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtProducts
            // 
            this.txtProducts.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.txtProducts.AutoSize = true;
            this.txtProducts.BackColor = System.Drawing.Color.Transparent;
            this.txtProducts.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtProducts.ForeColor = System.Drawing.SystemColors.Control;
            this.txtProducts.Location = new System.Drawing.Point(51, 571);
            this.txtProducts.Name = "txtProducts";
            this.txtProducts.Size = new System.Drawing.Size(68, 55);
            this.txtProducts.TabIndex = 5;
            this.txtProducts.Tag = "NonPaintable";
            this.txtProducts.Text = "Productos";
            this.txtProducts.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnOpenProducts
            // 
            this.btnOpenProducts.BackColor = System.Drawing.Color.Transparent;
            this.btnOpenProducts.BackgroundImage = global::WinformsUI.Properties.Resources.ProductsIcon;
            this.btnOpenProducts.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnOpenProducts.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnOpenProducts.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnOpenProducts.FlatAppearance.BorderSize = 0;
            this.btnOpenProducts.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOpenProducts.Location = new System.Drawing.Point(5, 576);
            this.btnOpenProducts.Margin = new System.Windows.Forms.Padding(5);
            this.btnOpenProducts.Name = "btnOpenProducts";
            this.btnOpenProducts.Size = new System.Drawing.Size(38, 45);
            this.btnOpenProducts.TabIndex = 0;
            this.btnOpenProducts.Tag = "NonPaintable";
            this.btnOpenProducts.UseVisualStyleBackColor = false;
            // 
            // btnOpenSuppliers
            // 
            this.btnOpenSuppliers.BackColor = System.Drawing.Color.Transparent;
            this.btnOpenSuppliers.BackgroundImage = global::WinformsUI.Properties.Resources.SuppliersIcon;
            this.btnOpenSuppliers.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnOpenSuppliers.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnOpenSuppliers.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnOpenSuppliers.FlatAppearance.BorderSize = 0;
            this.btnOpenSuppliers.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOpenSuppliers.Location = new System.Drawing.Point(5, 631);
            this.btnOpenSuppliers.Margin = new System.Windows.Forms.Padding(5);
            this.btnOpenSuppliers.Name = "btnOpenSuppliers";
            this.btnOpenSuppliers.Size = new System.Drawing.Size(38, 45);
            this.btnOpenSuppliers.TabIndex = 12;
            this.btnOpenSuppliers.Tag = "NonPaintable";
            this.btnOpenSuppliers.UseVisualStyleBackColor = false;
            // 
            // btnOpenSales
            // 
            this.btnOpenSales.BackColor = System.Drawing.Color.Transparent;
            this.btnOpenSales.BackgroundImage = global::WinformsUI.Properties.Resources.SaleIcon;
            this.btnOpenSales.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnOpenSales.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnOpenSales.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnOpenSales.FlatAppearance.BorderSize = 0;
            this.btnOpenSales.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOpenSales.Location = new System.Drawing.Point(5, 466);
            this.btnOpenSales.Margin = new System.Windows.Forms.Padding(5);
            this.btnOpenSales.Name = "btnOpenSales";
            this.btnOpenSales.Size = new System.Drawing.Size(38, 45);
            this.btnOpenSales.TabIndex = 12;
            this.btnOpenSales.Tag = "NonPaintable";
            this.btnOpenSales.UseVisualStyleBackColor = false;
            // 
            // btnOpenPayments
            // 
            this.btnOpenPayments.BackColor = System.Drawing.Color.Transparent;
            this.btnOpenPayments.BackgroundImage = global::WinformsUI.Properties.Resources.PaymentsIcon;
            this.btnOpenPayments.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnOpenPayments.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnOpenPayments.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnOpenPayments.FlatAppearance.BorderSize = 0;
            this.btnOpenPayments.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOpenPayments.Location = new System.Drawing.Point(5, 521);
            this.btnOpenPayments.Margin = new System.Windows.Forms.Padding(5);
            this.btnOpenPayments.Name = "btnOpenPayments";
            this.btnOpenPayments.Size = new System.Drawing.Size(38, 45);
            this.btnOpenPayments.TabIndex = 16;
            this.btnOpenPayments.Tag = "NonPaintable";
            this.btnOpenPayments.UseVisualStyleBackColor = false;
            // 
            // btnOpenConfigs
            // 
            this.btnOpenConfigs.BackColor = System.Drawing.Color.Transparent;
            this.btnOpenConfigs.BackgroundImage = global::WinformsUI.Properties.Resources.configIcon;
            this.btnOpenConfigs.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnOpenConfigs.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnOpenConfigs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnOpenConfigs.FlatAppearance.BorderSize = 0;
            this.btnOpenConfigs.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOpenConfigs.Location = new System.Drawing.Point(5, 686);
            this.btnOpenConfigs.Margin = new System.Windows.Forms.Padding(5);
            this.btnOpenConfigs.Name = "btnOpenConfigs";
            this.btnOpenConfigs.Size = new System.Drawing.Size(38, 45);
            this.btnOpenConfigs.TabIndex = 12;
            this.btnOpenConfigs.Tag = "NonPaintable";
            this.btnOpenConfigs.UseVisualStyleBackColor = false;
            // 
            // txtConfigs
            // 
            this.txtConfigs.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.txtConfigs.AutoSize = true;
            this.txtConfigs.BackColor = System.Drawing.Color.Transparent;
            this.txtConfigs.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtConfigs.ForeColor = System.Drawing.SystemColors.Control;
            this.txtConfigs.Location = new System.Drawing.Point(51, 681);
            this.txtConfigs.Name = "txtConfigs";
            this.txtConfigs.Size = new System.Drawing.Size(104, 55);
            this.txtConfigs.TabIndex = 5;
            this.txtConfigs.Tag = "NonPaintable";
            this.txtConfigs.Text = "Configuraciones";
            this.txtConfigs.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox1.BackgroundImage = global::WinformsUI.Properties.Resources.IconUnLogo;
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pictureBox1.Location = new System.Drawing.Point(48, 250);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(1, 41);
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Tag = "NonPaintable";
            // 
            // pictureBox2
            // 
            this.pictureBox2.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox2.BackgroundImage = global::WinformsUI.Properties.Resources.HyIOWhite;
            this.pictureBox2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pictureBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox2.Location = new System.Drawing.Point(51, 53);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(111, 44);
            this.pictureBox2.TabIndex = 17;
            this.pictureBox2.TabStop = false;
            this.pictureBox2.Tag = "NonPaintable";
            // 
            // btnChangeWindowManagementMode
            // 
            this.btnChangeWindowManagementMode.AutoSize = true;
            this.btnChangeWindowManagementMode.BackColor = System.Drawing.Color.Transparent;
            this.tlpMenu.SetColumnSpan(this.btnChangeWindowManagementMode, 2);
            this.btnChangeWindowManagementMode.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnChangeWindowManagementMode.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnChangeWindowManagementMode.FlatAppearance.BorderSize = 0;
            this.btnChangeWindowManagementMode.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnChangeWindowManagementMode.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnChangeWindowManagementMode.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.btnChangeWindowManagementMode.Location = new System.Drawing.Point(3, 153);
            this.btnChangeWindowManagementMode.Name = "btnChangeWindowManagementMode";
            this.btnChangeWindowManagementMode.Size = new System.Drawing.Size(159, 44);
            this.btnChangeWindowManagementMode.TabIndex = 18;
            this.btnChangeWindowManagementMode.Tag = "NonPaintable";
            this.btnChangeWindowManagementMode.Text = "Modo Tablero";
            this.btnChangeWindowManagementMode.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnChangeWindowManagementMode.UseVisualStyleBackColor = false;
            // 
            // btnVerticalTileWindows
            // 
            this.btnVerticalTileWindows.AutoSize = true;
            this.btnVerticalTileWindows.BackColor = System.Drawing.Color.Transparent;
            this.tlpMenu.SetColumnSpan(this.btnVerticalTileWindows, 2);
            this.btnVerticalTileWindows.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnVerticalTileWindows.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnVerticalTileWindows.FlatAppearance.BorderSize = 0;
            this.btnVerticalTileWindows.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnVerticalTileWindows.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnVerticalTileWindows.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.btnVerticalTileWindows.Location = new System.Drawing.Point(3, 203);
            this.btnVerticalTileWindows.Name = "btnVerticalTileWindows";
            this.btnVerticalTileWindows.Size = new System.Drawing.Size(159, 44);
            this.btnVerticalTileWindows.TabIndex = 19;
            this.btnVerticalTileWindows.Tag = "NonPaintable";
            this.btnVerticalTileWindows.Text = "Acomodar Automáticamente";
            this.btnVerticalTileWindows.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnVerticalTileWindows.UseVisualStyleBackColor = false;
            // 
            // mainSs
            // 
            this.mainSs.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.mainSs.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.mainSs.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel8,
            this.toolStripDropDownButton1,
            this.toolStripStatusLabel6,
            this.txtLoginTime,
            this.toolStripStatusLabel1,
            this.toolStripStatusLabel3,
            this.txtCurrentUserName,
            this.toolStripStatusLabel4,
            this.toolStripStatusLabel7});
            this.mainSs.Location = new System.Drawing.Point(165, 811);
            this.mainSs.Name = "mainSs";
            this.mainSs.RenderMode = System.Windows.Forms.ToolStripRenderMode.ManagerRenderMode;
            this.mainSs.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.mainSs.Size = new System.Drawing.Size(1290, 26);
            this.mainSs.TabIndex = 11;
            this.mainSs.Text = "statusStrip1";
            // 
            // toolStripStatusLabel8
            // 
            this.toolStripStatusLabel8.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.toolStripStatusLabel8.Name = "toolStripStatusLabel8";
            this.toolStripStatusLabel8.Size = new System.Drawing.Size(60, 21);
            this.toolStripStatusLabel8.Text = "Mi cuenta";
            // 
            // toolStripDropDownButton1
            // 
            this.toolStripDropDownButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripDropDownButton1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.solicitarPermisosToolStripMenuItem,
            this.modificarCuentaToolStripMenuItem});
            this.toolStripDropDownButton1.Image = global::WinformsUI.Properties.Resources.MyProfileIcon;
            this.toolStripDropDownButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripDropDownButton1.Name = "toolStripDropDownButton1";
            this.toolStripDropDownButton1.Size = new System.Drawing.Size(33, 24);
            this.toolStripDropDownButton1.Text = "Administrar mi cuenta";
            // 
            // solicitarPermisosToolStripMenuItem
            // 
            this.solicitarPermisosToolStripMenuItem.Name = "solicitarPermisosToolStripMenuItem";
            this.solicitarPermisosToolStripMenuItem.Size = new System.Drawing.Size(167, 22);
            this.solicitarPermisosToolStripMenuItem.Text = "Solicitar permisos";
            // 
            // modificarCuentaToolStripMenuItem
            // 
            this.modificarCuentaToolStripMenuItem.Name = "modificarCuentaToolStripMenuItem";
            this.modificarCuentaToolStripMenuItem.Size = new System.Drawing.Size(167, 22);
            this.modificarCuentaToolStripMenuItem.Text = "Modificar cuenta";
            // 
            // toolStripStatusLabel6
            // 
            this.toolStripStatusLabel6.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.toolStripStatusLabel6.Name = "toolStripStatusLabel6";
            this.toolStripStatusLabel6.Size = new System.Drawing.Size(10, 21);
            this.toolStripStatusLabel6.Text = "|";
            // 
            // txtLoginTime
            // 
            this.txtLoginTime.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.txtLoginTime.Name = "txtLoginTime";
            this.txtLoginTime.Size = new System.Drawing.Size(51, 21);
            this.txtLoginTime.Text = "04:41am";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(134, 21);
            this.toolStripStatusLabel1.Text = ":Hora de inicio se sesión";
            // 
            // toolStripStatusLabel3
            // 
            this.toolStripStatusLabel3.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.toolStripStatusLabel3.Name = "toolStripStatusLabel3";
            this.toolStripStatusLabel3.Size = new System.Drawing.Size(10, 21);
            this.toolStripStatusLabel3.Text = "|";
            // 
            // txtCurrentUserName
            // 
            this.txtCurrentUserName.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.txtCurrentUserName.Name = "txtCurrentUserName";
            this.txtCurrentUserName.Size = new System.Drawing.Size(37, 21);
            this.txtCurrentUserName.Text = "Julián";
            // 
            // toolStripStatusLabel4
            // 
            this.toolStripStatusLabel4.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.toolStripStatusLabel4.Name = "toolStripStatusLabel4";
            this.toolStripStatusLabel4.Size = new System.Drawing.Size(69, 21);
            this.toolStripStatusLabel4.Text = ":Bienvenido";
            // 
            // toolStripStatusLabel7
            // 
            this.toolStripStatusLabel7.Name = "toolStripStatusLabel7";
            this.toolStripStatusLabel7.Size = new System.Drawing.Size(118, 21);
            this.toolStripStatusLabel7.Tag = "Accented";
            this.toolStripStatusLabel7.Text = "toolStripStatusLabel7";
            // 
            // DashboardPnl
            // 
            this.DashboardPnl.BackColor = System.Drawing.Color.DarkGray;
            this.DashboardPnl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DashboardPnl.Location = new System.Drawing.Point(165, 44);
            this.DashboardPnl.Margin = new System.Windows.Forms.Padding(0);
            this.DashboardPnl.Name = "DashboardPnl";
            this.DashboardPnl.Padding = new System.Windows.Forms.Padding(0, 30, 0, 0);
            this.DashboardPnl.Size = new System.Drawing.Size(1290, 767);
            this.DashboardPnl.TabIndex = 14;
            this.DashboardPnl.Tag = "NonPaintable";
            // 
            // FLPsideTools
            // 
            this.FLPsideTools.BackColor = System.Drawing.Color.Transparent;
            this.FLPsideTools.Dock = System.Windows.Forms.DockStyle.Fill;
            this.FLPsideTools.Location = new System.Drawing.Point(2, 2);
            this.FLPsideTools.Margin = new System.Windows.Forms.Padding(2);
            this.FLPsideTools.Name = "FLPsideTools";
            this.FLPsideTools.Size = new System.Drawing.Size(1166, 40);
            this.FLPsideTools.TabIndex = 6;
            this.FLPsideTools.Tag = "NonPaintable";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 4;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Controls.Add(this.btnClose, 3, 0);
            this.tableLayoutPanel1.Controls.Add(this.FLPsideTools, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnExpand, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnMinimize, 1, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(165, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1290, 44);
            this.tableLayoutPanel1.TabIndex = 12;
            this.tableLayoutPanel1.Tag = "ExternalTitleBar";
            // 
            // btnClose
            // 
            this.btnClose.BackColor = System.Drawing.Color.Transparent;
            this.btnClose.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnClose.BackgroundImage")));
            this.btnClose.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnClose.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnClose.FlatAppearance.BorderSize = 0;
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.Location = new System.Drawing.Point(1253, 3);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(34, 38);
            this.btnClose.TabIndex = 18;
            this.btnClose.Tag = "NonPaintable";
            this.btnClose.UseVisualStyleBackColor = false;
            // 
            // btnExpand
            // 
            this.btnExpand.BackColor = System.Drawing.Color.Transparent;
            this.btnExpand.BackgroundImage = global::WinformsUI.Properties.Resources.btnRestoreClassic;
            this.btnExpand.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnExpand.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnExpand.FlatAppearance.BorderSize = 0;
            this.btnExpand.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExpand.Location = new System.Drawing.Point(1213, 3);
            this.btnExpand.Name = "btnExpand";
            this.btnExpand.Size = new System.Drawing.Size(34, 38);
            this.btnExpand.TabIndex = 17;
            this.btnExpand.Tag = "NonPaintable";
            this.btnExpand.UseVisualStyleBackColor = false;
            // 
            // btnMinimize
            // 
            this.btnMinimize.BackColor = System.Drawing.Color.Transparent;
            this.btnMinimize.BackgroundImage = global::WinformsUI.Properties.Resources.btnMinimizeClassic;
            this.btnMinimize.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnMinimize.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnMinimize.FlatAppearance.BorderSize = 0;
            this.btnMinimize.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMinimize.Location = new System.Drawing.Point(1173, 3);
            this.btnMinimize.Name = "btnMinimize";
            this.btnMinimize.Size = new System.Drawing.Size(34, 38);
            this.btnMinimize.TabIndex = 16;
            this.btnMinimize.Tag = "NonPaintable";
            this.btnMinimize.UseVisualStyleBackColor = false;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.ClientSize = new System.Drawing.Size(1455, 837);
            this.Controls.Add(this.DashboardPnl);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.mainSs);
            this.Controls.Add(this.tlpMenu);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Tag = "NonPaintable";
            this.Text = "ITHES - Lector";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.tlpMenu.ResumeLayout(false);
            this.tlpMenu.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.mainSs.ResumeLayout(false);
            this.mainSs.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label txtSales;
        private System.Windows.Forms.Label txtPayments;
        private System.Windows.Forms.Label txtEmployees;
        private System.Windows.Forms.Label txtClients;
        private System.Windows.Forms.Button btnOpenEmployees;
        private System.Windows.Forms.Button btnOpenClients;
        private System.Windows.Forms.Button btnOpenSales;
        private System.Windows.Forms.TableLayoutPanel tlpMenu;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.StatusStrip mainSs;
        private System.Windows.Forms.Button btnMenu;
        private System.Windows.Forms.Panel DashboardPnl;
        private System.Windows.Forms.Label txtSuppliers;
        private System.Windows.Forms.Button btnOpenProducts;
        private System.Windows.Forms.Label txtProducts;
        private System.Windows.Forms.Button btnOpenSuppliers;
        private System.Windows.Forms.Button btnMinimize;
        private System.Windows.Forms.Button btnExpand;
        private System.Windows.Forms.FlowLayoutPanel FLPsideTools;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button btnOpenPayments;
        private System.Windows.Forms.Button btnOpenConfigs;
        private System.Windows.Forms.Label txtConfigs;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel4;
        private System.Windows.Forms.ToolStripStatusLabel txtCurrentUserName;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel3;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel txtLoginTime;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel6;
        private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownButton1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel7;
        private System.Windows.Forms.ToolStripMenuItem solicitarPermisosToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem modificarCuentaToolStripMenuItem;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel8;
        private System.Windows.Forms.Button btnChangeWindowManagementMode;
        private System.Windows.Forms.Button btnVerticalTileWindows;
    }
}

