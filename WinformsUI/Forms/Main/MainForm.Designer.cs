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
            this.tlpMenu = new System.Windows.Forms.TableLayoutPanel();
            this.btnMenu = new System.Windows.Forms.Button();
            this.btnOpenClients = new System.Windows.Forms.Button();
            this.btnOpenEmployees = new System.Windows.Forms.Button();
            this.btnOpenProducts = new System.Windows.Forms.Button();
            this.btnOpenSuppliers = new System.Windows.Forms.Button();
            this.btnOpenSales = new System.Windows.Forms.Button();
            this.btnOpenPayments = new System.Windows.Forms.Button();
            this.btnOpenConfigs = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.btnChangeWindowManagementMode = new System.Windows.Forms.Button();
            this.btnAutoArragement = new System.Windows.Forms.Button();
            this.pnlDashboard = new System.Windows.Forms.Panel();
            this.pnlSlotForTabs = new System.Windows.Forms.FlowLayoutPanel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnExpand = new System.Windows.Forms.Button();
            this.btnMinimize = new System.Windows.Forms.Button();
            this.customStatusBar = new WinformsUI.UserControls.CustomStatusBar.CustomStatusBar();
            this.tlpMenu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
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
            this.tlpMenu.Controls.Add(this.btnOpenProducts, 0, 10);
            this.tlpMenu.Controls.Add(this.btnOpenSuppliers, 0, 11);
            this.tlpMenu.Controls.Add(this.btnOpenSales, 0, 8);
            this.tlpMenu.Controls.Add(this.btnOpenPayments, 0, 9);
            this.tlpMenu.Controls.Add(this.btnOpenConfigs, 0, 12);
            this.tlpMenu.Controls.Add(this.pictureBox1, 1, 5);
            this.tlpMenu.Controls.Add(this.pictureBox2, 1, 1);
            this.tlpMenu.Controls.Add(this.btnChangeWindowManagementMode, 0, 3);
            this.tlpMenu.Controls.Add(this.btnAutoArragement, 0, 4);
            this.tlpMenu.Dock = System.Windows.Forms.DockStyle.Left;
            this.tlpMenu.Location = new System.Drawing.Point(0, 0);
            this.tlpMenu.MaximumSize = new System.Drawing.Size(165, 0);
            this.tlpMenu.MinimumSize = new System.Drawing.Size(51, 0);
            this.tlpMenu.Name = "tlpMenu";
            this.tlpMenu.RowCount = 15;
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
            this.tlpMenu.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 55F));
            this.tlpMenu.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpMenu.Size = new System.Drawing.Size(165, 835);
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
            this.btnOpenClients.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.tlpMenu.SetColumnSpan(this.btnOpenClients, 2);
            this.btnOpenClients.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnOpenClients.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnOpenClients.FlatAppearance.BorderSize = 0;
            this.btnOpenClients.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOpenClients.Font = new System.Drawing.Font("Microsoft YaHei UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOpenClients.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.btnOpenClients.Location = new System.Drawing.Point(5, 327);
            this.btnOpenClients.Margin = new System.Windows.Forms.Padding(5);
            this.btnOpenClients.Name = "btnOpenClients";
            this.btnOpenClients.Padding = new System.Windows.Forms.Padding(43, 0, 0, 0);
            this.btnOpenClients.Size = new System.Drawing.Size(155, 45);
            this.btnOpenClients.TabIndex = 0;
            this.btnOpenClients.Tag = "NonPaintable";
            this.btnOpenClients.Text = "Clientes";
            this.btnOpenClients.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnOpenClients.UseVisualStyleBackColor = false;
            // 
            // btnOpenEmployees
            // 
            this.btnOpenEmployees.BackColor = System.Drawing.Color.Transparent;
            this.btnOpenEmployees.BackgroundImage = global::WinformsUI.Properties.Resources.EmployeeIcon;
            this.btnOpenEmployees.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.tlpMenu.SetColumnSpan(this.btnOpenEmployees, 2);
            this.btnOpenEmployees.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnOpenEmployees.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnOpenEmployees.FlatAppearance.BorderSize = 0;
            this.btnOpenEmployees.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOpenEmployees.Font = new System.Drawing.Font("Microsoft YaHei UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOpenEmployees.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.btnOpenEmployees.Location = new System.Drawing.Point(5, 382);
            this.btnOpenEmployees.Margin = new System.Windows.Forms.Padding(5);
            this.btnOpenEmployees.Name = "btnOpenEmployees";
            this.btnOpenEmployees.Padding = new System.Windows.Forms.Padding(43, 0, 0, 0);
            this.btnOpenEmployees.Size = new System.Drawing.Size(155, 45);
            this.btnOpenEmployees.TabIndex = 0;
            this.btnOpenEmployees.Tag = "NonPaintable";
            this.btnOpenEmployees.Text = "Empleados";
            this.btnOpenEmployees.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnOpenEmployees.UseVisualStyleBackColor = false;
            // 
            // btnOpenProducts
            // 
            this.btnOpenProducts.BackColor = System.Drawing.Color.Transparent;
            this.btnOpenProducts.BackgroundImage = global::WinformsUI.Properties.Resources.ProductsIcon;
            this.btnOpenProducts.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.tlpMenu.SetColumnSpan(this.btnOpenProducts, 2);
            this.btnOpenProducts.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnOpenProducts.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnOpenProducts.FlatAppearance.BorderSize = 0;
            this.btnOpenProducts.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOpenProducts.Font = new System.Drawing.Font("Microsoft YaHei UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOpenProducts.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.btnOpenProducts.Location = new System.Drawing.Point(5, 547);
            this.btnOpenProducts.Margin = new System.Windows.Forms.Padding(5);
            this.btnOpenProducts.Name = "btnOpenProducts";
            this.btnOpenProducts.Padding = new System.Windows.Forms.Padding(43, 0, 0, 0);
            this.btnOpenProducts.Size = new System.Drawing.Size(155, 45);
            this.btnOpenProducts.TabIndex = 0;
            this.btnOpenProducts.Tag = "NonPaintable";
            this.btnOpenProducts.Text = "Productos";
            this.btnOpenProducts.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnOpenProducts.UseVisualStyleBackColor = false;
            // 
            // btnOpenSuppliers
            // 
            this.btnOpenSuppliers.BackColor = System.Drawing.Color.Transparent;
            this.btnOpenSuppliers.BackgroundImage = global::WinformsUI.Properties.Resources.SuppliersIcon;
            this.btnOpenSuppliers.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.tlpMenu.SetColumnSpan(this.btnOpenSuppliers, 2);
            this.btnOpenSuppliers.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnOpenSuppliers.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnOpenSuppliers.FlatAppearance.BorderSize = 0;
            this.btnOpenSuppliers.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOpenSuppliers.Font = new System.Drawing.Font("Microsoft YaHei UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOpenSuppliers.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.btnOpenSuppliers.Location = new System.Drawing.Point(5, 602);
            this.btnOpenSuppliers.Margin = new System.Windows.Forms.Padding(5);
            this.btnOpenSuppliers.Name = "btnOpenSuppliers";
            this.btnOpenSuppliers.Padding = new System.Windows.Forms.Padding(43, 0, 0, 0);
            this.btnOpenSuppliers.Size = new System.Drawing.Size(155, 45);
            this.btnOpenSuppliers.TabIndex = 12;
            this.btnOpenSuppliers.Tag = "NonPaintable";
            this.btnOpenSuppliers.Text = "Proveedores";
            this.btnOpenSuppliers.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnOpenSuppliers.UseVisualStyleBackColor = false;
            // 
            // btnOpenSales
            // 
            this.btnOpenSales.BackColor = System.Drawing.Color.Transparent;
            this.btnOpenSales.BackgroundImage = global::WinformsUI.Properties.Resources.SaleIcon;
            this.btnOpenSales.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.tlpMenu.SetColumnSpan(this.btnOpenSales, 2);
            this.btnOpenSales.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnOpenSales.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnOpenSales.FlatAppearance.BorderSize = 0;
            this.btnOpenSales.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOpenSales.Font = new System.Drawing.Font("Microsoft YaHei UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOpenSales.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.btnOpenSales.Location = new System.Drawing.Point(5, 437);
            this.btnOpenSales.Margin = new System.Windows.Forms.Padding(5);
            this.btnOpenSales.Name = "btnOpenSales";
            this.btnOpenSales.Padding = new System.Windows.Forms.Padding(43, 0, 0, 0);
            this.btnOpenSales.Size = new System.Drawing.Size(155, 45);
            this.btnOpenSales.TabIndex = 12;
            this.btnOpenSales.Tag = "NonPaintable";
            this.btnOpenSales.Text = "Ventas";
            this.btnOpenSales.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnOpenSales.UseVisualStyleBackColor = false;
            // 
            // btnOpenPayments
            // 
            this.btnOpenPayments.BackColor = System.Drawing.Color.Transparent;
            this.btnOpenPayments.BackgroundImage = global::WinformsUI.Properties.Resources.PaymentsIcon;
            this.btnOpenPayments.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.tlpMenu.SetColumnSpan(this.btnOpenPayments, 2);
            this.btnOpenPayments.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnOpenPayments.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnOpenPayments.FlatAppearance.BorderSize = 0;
            this.btnOpenPayments.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOpenPayments.Font = new System.Drawing.Font("Microsoft YaHei UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOpenPayments.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.btnOpenPayments.Location = new System.Drawing.Point(5, 492);
            this.btnOpenPayments.Margin = new System.Windows.Forms.Padding(5);
            this.btnOpenPayments.Name = "btnOpenPayments";
            this.btnOpenPayments.Padding = new System.Windows.Forms.Padding(43, 0, 0, 0);
            this.btnOpenPayments.Size = new System.Drawing.Size(155, 45);
            this.btnOpenPayments.TabIndex = 16;
            this.btnOpenPayments.Tag = "NonPaintable";
            this.btnOpenPayments.Text = "Pagos";
            this.btnOpenPayments.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnOpenPayments.UseVisualStyleBackColor = false;
            // 
            // btnOpenConfigs
            // 
            this.btnOpenConfigs.BackColor = System.Drawing.Color.Transparent;
            this.btnOpenConfigs.BackgroundImage = global::WinformsUI.Properties.Resources.configIcon;
            this.btnOpenConfigs.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.tlpMenu.SetColumnSpan(this.btnOpenConfigs, 2);
            this.btnOpenConfigs.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnOpenConfigs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnOpenConfigs.FlatAppearance.BorderSize = 0;
            this.btnOpenConfigs.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOpenConfigs.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOpenConfigs.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.btnOpenConfigs.Location = new System.Drawing.Point(5, 657);
            this.btnOpenConfigs.Margin = new System.Windows.Forms.Padding(5);
            this.btnOpenConfigs.Name = "btnOpenConfigs";
            this.btnOpenConfigs.Padding = new System.Windows.Forms.Padding(43, 0, 0, 0);
            this.btnOpenConfigs.Size = new System.Drawing.Size(155, 45);
            this.btnOpenConfigs.TabIndex = 12;
            this.btnOpenConfigs.Tag = "NonPaintable";
            this.btnOpenConfigs.Text = "Configuraciones";
            this.btnOpenConfigs.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnOpenConfigs.UseVisualStyleBackColor = false;
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
            // btnAutoArragement
            // 
            this.btnAutoArragement.AutoSize = true;
            this.btnAutoArragement.BackColor = System.Drawing.Color.Transparent;
            this.tlpMenu.SetColumnSpan(this.btnAutoArragement, 2);
            this.btnAutoArragement.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnAutoArragement.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnAutoArragement.FlatAppearance.BorderSize = 0;
            this.btnAutoArragement.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAutoArragement.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAutoArragement.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.btnAutoArragement.Location = new System.Drawing.Point(3, 203);
            this.btnAutoArragement.Name = "btnAutoArragement";
            this.btnAutoArragement.Size = new System.Drawing.Size(159, 44);
            this.btnAutoArragement.TabIndex = 19;
            this.btnAutoArragement.Tag = "NonPaintable";
            this.btnAutoArragement.Text = "Acomodar Automáticamente";
            this.btnAutoArragement.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnAutoArragement.UseVisualStyleBackColor = false;
            // 
            // pnlDashboard
            // 
            this.pnlDashboard.BackColor = System.Drawing.Color.DarkGray;
            this.pnlDashboard.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlDashboard.Location = new System.Drawing.Point(165, 44);
            this.pnlDashboard.Margin = new System.Windows.Forms.Padding(0);
            this.pnlDashboard.Name = "pnlDashboard";
            this.pnlDashboard.Padding = new System.Windows.Forms.Padding(0, 30, 0, 0);
            this.pnlDashboard.Size = new System.Drawing.Size(1288, 760);
            this.pnlDashboard.TabIndex = 14;
            this.pnlDashboard.Tag = "NonPaintable";
            // 
            // pnlSlotForTabs
            // 
            this.pnlSlotForTabs.BackColor = System.Drawing.Color.Transparent;
            this.pnlSlotForTabs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlSlotForTabs.Location = new System.Drawing.Point(2, 2);
            this.pnlSlotForTabs.Margin = new System.Windows.Forms.Padding(2);
            this.pnlSlotForTabs.Name = "pnlSlotForTabs";
            this.pnlSlotForTabs.Size = new System.Drawing.Size(1164, 40);
            this.pnlSlotForTabs.TabIndex = 6;
            this.pnlSlotForTabs.Tag = "NonPaintable";
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
            this.tableLayoutPanel1.Controls.Add(this.pnlSlotForTabs, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnExpand, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnMinimize, 1, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(165, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1288, 44);
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
            this.btnClose.Location = new System.Drawing.Point(1251, 3);
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
            this.btnExpand.Location = new System.Drawing.Point(1211, 3);
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
            this.btnMinimize.Location = new System.Drawing.Point(1171, 3);
            this.btnMinimize.Name = "btnMinimize";
            this.btnMinimize.Size = new System.Drawing.Size(34, 38);
            this.btnMinimize.TabIndex = 16;
            this.btnMinimize.Tag = "NonPaintable";
            this.btnMinimize.UseVisualStyleBackColor = false;
            // 
            // customStatusBar
            // 
            this.customStatusBar.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.customStatusBar.Location = new System.Drawing.Point(165, 804);
            this.customStatusBar.Name = "customStatusBar";
            this.customStatusBar.Size = new System.Drawing.Size(1288, 31);
            this.customStatusBar.TabIndex = 15;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.ClientSize = new System.Drawing.Size(1455, 837);
            this.Controls.Add(this.pnlDashboard);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.customStatusBar);
            this.Controls.Add(this.tlpMenu);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MainForm";
            this.Padding = new System.Windows.Forms.Padding(0, 0, 2, 2);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Tag = "NonPaintable";
            this.Text = "ITHES - Lector";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.tlpMenu.ResumeLayout(false);
            this.tlpMenu.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button btnOpenEmployees;
        private System.Windows.Forms.Button btnOpenClients;
        private System.Windows.Forms.Button btnOpenSales;
        private System.Windows.Forms.TableLayoutPanel tlpMenu;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button btnMenu;
        private System.Windows.Forms.Panel pnlDashboard;
        private System.Windows.Forms.Button btnOpenProducts;
        private System.Windows.Forms.Button btnOpenSuppliers;
        private System.Windows.Forms.Button btnMinimize;
        private System.Windows.Forms.Button btnExpand;
        private System.Windows.Forms.FlowLayoutPanel pnlSlotForTabs;
        private UserControls.CustomStatusBar.CustomStatusBar customStatusBar;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button btnOpenPayments;
        private System.Windows.Forms.Button btnOpenConfigs;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Button btnChangeWindowManagementMode;
        private System.Windows.Forms.Button btnAutoArragement;
    
    }
}

