namespace WinformsUI.Forms.SaleCRUDL
{
    partial class CreateSaleForm
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
            this.tlpSelectClient = new System.Windows.Forms.TableLayoutPanel();
            this.btnNextPnl1 = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.dgvSelectClient = new System.Windows.Forms.DataGridView();
            this.tbSearchBarClient = new System.Windows.Forms.TextBox();
            this.tlpSelectProds = new System.Windows.Forms.TableLayoutPanel();
            this.btnBackPnl2 = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.tbSearchBarProducts = new System.Windows.Forms.TextBox();
            this.dgvSelectProduct = new System.Windows.Forms.DataGridView();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.label8 = new System.Windows.Forms.Label();
            this.lbAddedProds = new System.Windows.Forms.ListBox();
            this.btnAddProd = new System.Windows.Forms.Button();
            this.btnRemoveProd = new System.Windows.Forms.Button();
            this.tbQuantity = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label10 = new System.Windows.Forms.Label();
            this.tbSubTotal = new System.Windows.Forms.TextBox();
            this.btnFinish = new System.Windows.Forms.Button();
            this.tlpAddPayment = new System.Windows.Forms.TableLayoutPanel();
            this.button1 = new System.Windows.Forms.Button();
            this.btnBackContact = new System.Windows.Forms.Button();
            this.label9 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtReference = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.label5 = new System.Windows.Forms.Label();
            this.txtSummaryAmount = new System.Windows.Forms.TextBox();
            this.txtClientsummary = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.cbPaymentMethods = new System.Windows.Forms.ComboBox();
            this.tlpSelectClient.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSelectClient)).BeginInit();
            this.tlpSelectProds.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSelectProduct)).BeginInit();
            this.tableLayoutPanel4.SuspendLayout();
            this.panel1.SuspendLayout();
            this.tlpAddPayment.SuspendLayout();
            this.SuspendLayout();
            // 
            // tlpSelectClient
            // 
            this.tlpSelectClient.ColumnCount = 1;
            this.tlpSelectClient.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpSelectClient.Controls.Add(this.btnNextPnl1, 0, 3);
            this.tlpSelectClient.Controls.Add(this.label6, 0, 0);
            this.tlpSelectClient.Controls.Add(this.dgvSelectClient, 0, 2);
            this.tlpSelectClient.Controls.Add(this.tbSearchBarClient, 0, 1);
            this.tlpSelectClient.Location = new System.Drawing.Point(0, 0);
            this.tlpSelectClient.Name = "tlpSelectClient";
            this.tlpSelectClient.RowCount = 4;
            this.tlpSelectClient.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25.35971F));
            this.tlpSelectClient.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tlpSelectClient.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 74.64029F));
            this.tlpSelectClient.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 89F));
            this.tlpSelectClient.Size = new System.Drawing.Size(977, 589);
            this.tlpSelectClient.TabIndex = 9;
            // 
            // btnNextPnl1
            // 
            this.btnNextPnl1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.btnNextPnl1.Location = new System.Drawing.Point(19, 509);
            this.btnNextPnl1.Margin = new System.Windows.Forms.Padding(19, 6, 19, 6);
            this.btnNextPnl1.Name = "btnNextPnl1";
            this.btnNextPnl1.Size = new System.Drawing.Size(939, 69);
            this.btnNextPnl1.TabIndex = 5;
            this.btnNextPnl1.Tag = "Accentuable";
            this.btnNextPnl1.Text = "Siguiente";
            this.btnNextPnl1.UseVisualStyleBackColor = true;
            // 
            // label6
            // 
            this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft YaHei", 27.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(18, 34);
            this.label6.Margin = new System.Windows.Forms.Padding(18, 0, 5, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(954, 48);
            this.label6.TabIndex = 5;
            this.label6.Text = "Seleccionar cliente";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // dgvSelectClient
            // 
            this.dgvSelectClient.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvSelectClient.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvSelectClient.Location = new System.Drawing.Point(19, 159);
            this.dgvSelectClient.Margin = new System.Windows.Forms.Padding(19, 3, 19, 3);
            this.dgvSelectClient.Name = "dgvSelectClient";
            this.dgvSelectClient.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvSelectClient.Size = new System.Drawing.Size(939, 337);
            this.dgvSelectClient.TabIndex = 6;
            // 
            // tbSearchBarClient
            // 
            this.tbSearchBarClient.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.tbSearchBarClient.Location = new System.Drawing.Point(19, 126);
            this.tbSearchBarClient.Margin = new System.Windows.Forms.Padding(19, 3, 19, 3);
            this.tbSearchBarClient.Name = "tbSearchBarClient";
            this.tbSearchBarClient.Size = new System.Drawing.Size(939, 20);
            this.tbSearchBarClient.TabIndex = 7;
            // 
            // tlpSelectProds
            // 
            this.tlpSelectProds.ColumnCount = 2;
            this.tlpSelectProds.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 102F));
            this.tlpSelectProds.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpSelectProds.Controls.Add(this.btnBackPnl2, 0, 0);
            this.tlpSelectProds.Controls.Add(this.label2, 1, 0);
            this.tlpSelectProds.Controls.Add(this.tbSearchBarProducts, 0, 1);
            this.tlpSelectProds.Controls.Add(this.dgvSelectProduct, 0, 2);
            this.tlpSelectProds.Controls.Add(this.tableLayoutPanel4, 0, 3);
            this.tlpSelectProds.Controls.Add(this.btnFinish, 0, 4);
            this.tlpSelectProds.Location = new System.Drawing.Point(983, 0);
            this.tlpSelectProds.Name = "tlpSelectProds";
            this.tlpSelectProds.RowCount = 5;
            this.tlpSelectProds.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 115F));
            this.tlpSelectProds.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 42F));
            this.tlpSelectProds.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 255F));
            this.tlpSelectProds.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 214F));
            this.tlpSelectProds.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 54F));
            this.tlpSelectProds.Size = new System.Drawing.Size(977, 751);
            this.tlpSelectProds.TabIndex = 9;
            // 
            // btnBackPnl2
            // 
            this.btnBackPnl2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBackPnl2.BackColor = System.Drawing.Color.Transparent;
            this.btnBackPnl2.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnBackPnl2.FlatAppearance.BorderSize = 0;
            this.btnBackPnl2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBackPnl2.Image = global::WinformsUI.Properties.Resources.BackArrow;
            this.btnBackPnl2.Location = new System.Drawing.Point(4, 18);
            this.btnBackPnl2.Margin = new System.Windows.Forms.Padding(4);
            this.btnBackPnl2.Name = "btnBackPnl2";
            this.btnBackPnl2.Size = new System.Drawing.Size(94, 79);
            this.btnBackPnl2.TabIndex = 10;
            this.btnBackPnl2.Tag = "IsImageColorable";
            this.btnBackPnl2.UseVisualStyleBackColor = false;
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft YaHei", 27.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(120, 33);
            this.label2.Margin = new System.Windows.Forms.Padding(18, 0, 5, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(852, 48);
            this.label2.TabIndex = 5;
            this.label2.Text = "Seleccionar productos";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tbSearchBarProducts
            // 
            this.tbSearchBarProducts.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.tlpSelectProds.SetColumnSpan(this.tbSearchBarProducts, 2);
            this.tbSearchBarProducts.Location = new System.Drawing.Point(19, 126);
            this.tbSearchBarProducts.Margin = new System.Windows.Forms.Padding(19, 3, 19, 3);
            this.tbSearchBarProducts.Name = "tbSearchBarProducts";
            this.tbSearchBarProducts.Size = new System.Drawing.Size(939, 20);
            this.tbSearchBarProducts.TabIndex = 7;
            // 
            // dgvSelectProduct
            // 
            this.dgvSelectProduct.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.tlpSelectProds.SetColumnSpan(this.dgvSelectProduct, 2);
            this.dgvSelectProduct.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvSelectProduct.Location = new System.Drawing.Point(19, 160);
            this.dgvSelectProduct.Margin = new System.Windows.Forms.Padding(19, 3, 19, 3);
            this.dgvSelectProduct.Name = "dgvSelectProduct";
            this.dgvSelectProduct.Size = new System.Drawing.Size(939, 249);
            this.dgvSelectProduct.TabIndex = 9;
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.ColumnCount = 3;
            this.tlpSelectProds.SetColumnSpan(this.tableLayoutPanel4, 2);
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 13.55499F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 86.44501F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 113F));
            this.tableLayoutPanel4.Controls.Add(this.label8, 0, 1);
            this.tableLayoutPanel4.Controls.Add(this.lbAddedProds, 1, 1);
            this.tableLayoutPanel4.Controls.Add(this.btnAddProd, 2, 0);
            this.tableLayoutPanel4.Controls.Add(this.btnRemoveProd, 2, 1);
            this.tableLayoutPanel4.Controls.Add(this.tbQuantity, 1, 0);
            this.tableLayoutPanel4.Controls.Add(this.label7, 0, 0);
            this.tableLayoutPanel4.Controls.Add(this.panel1, 1, 2);
            this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel4.Location = new System.Drawing.Point(3, 415);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 3;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 52F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 120F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 36F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(971, 208);
            this.tableLayoutPanel4.TabIndex = 8;
            // 
            // label8
            // 
            this.label8.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft YaHei UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(4, 102);
            this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(108, 19);
            this.label8.TabIndex = 17;
            this.label8.Text = "Agregados";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lbAddedProds
            // 
            this.lbAddedProds.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbAddedProds.Font = new System.Drawing.Font("Microsoft YaHei UI", 9.75F);
            this.lbAddedProds.FormattingEnabled = true;
            this.lbAddedProds.ItemHeight = 19;
            this.lbAddedProds.Location = new System.Drawing.Point(135, 55);
            this.lbAddedProds.Margin = new System.Windows.Forms.Padding(19, 3, 19, 3);
            this.lbAddedProds.Name = "lbAddedProds";
            this.lbAddedProds.Size = new System.Drawing.Size(703, 114);
            this.lbAddedProds.TabIndex = 16;
            // 
            // btnAddProd
            // 
            this.btnAddProd.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnAddProd.FlatAppearance.BorderSize = 0;
            this.btnAddProd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddProd.Font = new System.Drawing.Font("Microsoft YaHei UI", 9.75F);
            this.btnAddProd.Location = new System.Drawing.Point(860, 3);
            this.btnAddProd.Margin = new System.Windows.Forms.Padding(3, 3, 15, 3);
            this.btnAddProd.Name = "btnAddProd";
            this.btnAddProd.Size = new System.Drawing.Size(96, 46);
            this.btnAddProd.TabIndex = 14;
            this.btnAddProd.Text = "Agregar";
            this.btnAddProd.UseVisualStyleBackColor = true;
            // 
            // btnRemoveProd
            // 
            this.btnRemoveProd.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnRemoveProd.FlatAppearance.BorderSize = 0;
            this.btnRemoveProd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRemoveProd.Font = new System.Drawing.Font("Microsoft YaHei UI", 9.75F);
            this.btnRemoveProd.Location = new System.Drawing.Point(860, 55);
            this.btnRemoveProd.Margin = new System.Windows.Forms.Padding(3, 3, 15, 3);
            this.btnRemoveProd.Name = "btnRemoveProd";
            this.btnRemoveProd.Size = new System.Drawing.Size(96, 114);
            this.btnRemoveProd.TabIndex = 13;
            this.btnRemoveProd.Text = "Quitar";
            this.btnRemoveProd.UseVisualStyleBackColor = true;
            // 
            // tbQuantity
            // 
            this.tbQuantity.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.tbQuantity.Font = new System.Drawing.Font("Microsoft YaHei UI", 9.75F);
            this.tbQuantity.Location = new System.Drawing.Point(135, 14);
            this.tbQuantity.Margin = new System.Windows.Forms.Padding(19, 3, 19, 3);
            this.tbQuantity.Name = "tbQuantity";
            this.tbQuantity.Size = new System.Drawing.Size(703, 24);
            this.tbQuantity.TabIndex = 10;
            // 
            // label7
            // 
            this.label7.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft YaHei UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(4, 16);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(108, 19);
            this.label7.TabIndex = 6;
            this.label7.Text = "Cantidad";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label10);
            this.panel1.Controls.Add(this.tbSubTotal);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(119, 175);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(735, 30);
            this.panel1.TabIndex = 18;
            // 
            // label10
            // 
            this.label10.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft YaHei UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(525, 6);
            this.label10.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(61, 19);
            this.label10.TabIndex = 19;
            this.label10.Text = "Subtotal";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tbSubTotal
            // 
            this.tbSubTotal.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.tbSubTotal.Enabled = false;
            this.tbSubTotal.Font = new System.Drawing.Font("Microsoft YaHei UI", 9.75F);
            this.tbSubTotal.Location = new System.Drawing.Point(590, 3);
            this.tbSubTotal.Margin = new System.Windows.Forms.Padding(19, 3, 19, 3);
            this.tbSubTotal.Name = "tbSubTotal";
            this.tbSubTotal.ReadOnly = true;
            this.tbSubTotal.Size = new System.Drawing.Size(126, 24);
            this.tbSubTotal.TabIndex = 10;
            // 
            // btnFinish
            // 
            this.btnFinish.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.tlpSelectProds.SetColumnSpan(this.btnFinish, 2);
            this.btnFinish.Location = new System.Drawing.Point(19, 654);
            this.btnFinish.Margin = new System.Windows.Forms.Padding(19, 6, 19, 6);
            this.btnFinish.Name = "btnFinish";
            this.btnFinish.Size = new System.Drawing.Size(939, 69);
            this.btnFinish.TabIndex = 5;
            this.btnFinish.Tag = "Accentuable";
            this.btnFinish.Text = "Finalizar";
            this.btnFinish.UseVisualStyleBackColor = true;
            // 
            // tlpAddPayment
            // 
            this.tlpAddPayment.ColumnCount = 2;
            this.tlpAddPayment.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 19.15368F));
            this.tlpAddPayment.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 80.84633F));
            this.tlpAddPayment.Controls.Add(this.button1, 0, 6);
            this.tlpAddPayment.Controls.Add(this.btnBackContact, 0, 0);
            this.tlpAddPayment.Controls.Add(this.label9, 1, 0);
            this.tlpAddPayment.Controls.Add(this.label3, 0, 5);
            this.tlpAddPayment.Controls.Add(this.txtReference, 1, 5);
            this.tlpAddPayment.Controls.Add(this.label1, 0, 4);
            this.tlpAddPayment.Controls.Add(this.label4, 0, 3);
            this.tlpAddPayment.Controls.Add(this.dateTimePicker1, 1, 3);
            this.tlpAddPayment.Controls.Add(this.label5, 0, 2);
            this.tlpAddPayment.Controls.Add(this.txtSummaryAmount, 1, 2);
            this.tlpAddPayment.Controls.Add(this.txtClientsummary, 1, 1);
            this.tlpAddPayment.Controls.Add(this.label11, 0, 1);
            this.tlpAddPayment.Controls.Add(this.cbPaymentMethods, 1, 4);
            this.tlpAddPayment.Font = new System.Drawing.Font("Microsoft YaHei UI", 9.75F);
            this.tlpAddPayment.Location = new System.Drawing.Point(2003, 18);
            this.tlpAddPayment.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.tlpAddPayment.Name = "tlpAddPayment";
            this.tlpAddPayment.Padding = new System.Windows.Forms.Padding(0, 7, 13, 7);
            this.tlpAddPayment.RowCount = 7;
            this.tlpAddPayment.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 64.0884F));
            this.tlpAddPayment.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 35.9116F));
            this.tlpAddPayment.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 59F));
            this.tlpAddPayment.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 66F));
            this.tlpAddPayment.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 69F));
            this.tlpAddPayment.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 51F));
            this.tlpAddPayment.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 120F));
            this.tlpAddPayment.Size = new System.Drawing.Size(462, 589);
            this.tlpAddPayment.TabIndex = 10;
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.tlpAddPayment.SetColumnSpan(this.button1, 2);
            this.button1.Font = new System.Drawing.Font("Microsoft YaHei UI", 9.75F);
            this.button1.Location = new System.Drawing.Point(19, 487);
            this.button1.Margin = new System.Windows.Forms.Padding(19, 6, 5, 6);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(425, 69);
            this.button1.TabIndex = 5;
            this.button1.Tag = "Accentuable";
            this.button1.Text = "Finalizar";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // btnBackContact
            // 
            this.btnBackContact.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBackContact.BackColor = System.Drawing.Color.Transparent;
            this.btnBackContact.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnBackContact.FlatAppearance.BorderSize = 0;
            this.btnBackContact.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBackContact.Image = global::WinformsUI.Properties.Resources.BackArrow;
            this.btnBackContact.Location = new System.Drawing.Point(4, 34);
            this.btnBackContact.Margin = new System.Windows.Forms.Padding(4);
            this.btnBackContact.Name = "btnBackContact";
            this.btnBackContact.Size = new System.Drawing.Size(78, 79);
            this.btnBackContact.TabIndex = 7;
            this.btnBackContact.Tag = "IsImageColorable";
            this.btnBackContact.UseVisualStyleBackColor = false;
            // 
            // label9
            // 
            this.label9.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft YaHei", 27.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(104, 50);
            this.label9.Margin = new System.Windows.Forms.Padding(18, 0, 5, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(340, 48);
            this.label9.TabIndex = 4;
            this.label9.Text = "Ingresar pago";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft YaHei UI", 9.75F);
            this.label3.Location = new System.Drawing.Point(5, 416);
            this.label3.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(76, 38);
            this.label3.TabIndex = 2;
            this.label3.Text = "Nro.\r\nReferencia";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtReference
            // 
            this.txtReference.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtReference.Font = new System.Drawing.Font("Microsoft YaHei UI", 9.75F);
            this.txtReference.Location = new System.Drawing.Point(91, 423);
            this.txtReference.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.txtReference.Name = "txtReference";
            this.txtReference.Size = new System.Drawing.Size(353, 24);
            this.txtReference.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft YaHei UI", 9.75F);
            this.label1.Location = new System.Drawing.Point(5, 366);
            this.label1.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(76, 19);
            this.label1.TabIndex = 2;
            this.label1.Text = "Método";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft YaHei UI", 9.75F);
            this.label4.Location = new System.Drawing.Point(5, 298);
            this.label4.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(76, 19);
            this.label4.TabIndex = 2;
            this.label4.Text = "Fecha";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.dateTimePicker1.Font = new System.Drawing.Font("Microsoft YaHei UI", 9.75F);
            this.dateTimePicker1.Location = new System.Drawing.Point(89, 296);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.Size = new System.Drawing.Size(357, 24);
            this.dateTimePicker1.TabIndex = 8;
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft YaHei UI", 9.75F);
            this.label5.Location = new System.Drawing.Point(5, 236);
            this.label5.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(76, 19);
            this.label5.TabIndex = 2;
            this.label5.Text = "Monto";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtSummaryAmount
            // 
            this.txtSummaryAmount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSummaryAmount.Font = new System.Drawing.Font("Microsoft YaHei UI", 9.75F);
            this.txtSummaryAmount.Location = new System.Drawing.Point(91, 233);
            this.txtSummaryAmount.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.txtSummaryAmount.Name = "txtSummaryAmount";
            this.txtSummaryAmount.Size = new System.Drawing.Size(353, 24);
            this.txtSummaryAmount.TabIndex = 0;
            // 
            // txtClientsummary
            // 
            this.txtClientsummary.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtClientsummary.Enabled = false;
            this.txtClientsummary.Font = new System.Drawing.Font("Microsoft YaHei UI", 9.75F);
            this.txtClientsummary.Location = new System.Drawing.Point(91, 166);
            this.txtClientsummary.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.txtClientsummary.Name = "txtClientsummary";
            this.txtClientsummary.ReadOnly = true;
            this.txtClientsummary.Size = new System.Drawing.Size(353, 24);
            this.txtClientsummary.TabIndex = 0;
            // 
            // label11
            // 
            this.label11.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Microsoft YaHei UI", 9.75F);
            this.label11.Location = new System.Drawing.Point(5, 169);
            this.label11.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(76, 19);
            this.label11.TabIndex = 2;
            this.label11.Text = "Cliente";
            this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cbPaymentMethods
            // 
            this.cbPaymentMethods.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.cbPaymentMethods.Font = new System.Drawing.Font("Microsoft YaHei UI", 9.75F);
            this.cbPaymentMethods.FormattingEnabled = true;
            this.cbPaymentMethods.Location = new System.Drawing.Point(89, 362);
            this.cbPaymentMethods.Name = "cbPaymentMethods";
            this.cbPaymentMethods.Size = new System.Drawing.Size(357, 27);
            this.cbPaymentMethods.TabIndex = 9;
            // 
            // CreateSaleForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(3238, 860);
            this.Controls.Add(this.tlpAddPayment);
            this.Controls.Add(this.tlpSelectProds);
            this.Controls.Add(this.tlpSelectClient);
            this.Name = "CreateSaleForm";
            this.Text = "CreateSaleForm";
            this.tlpSelectClient.ResumeLayout(false);
            this.tlpSelectClient.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSelectClient)).EndInit();
            this.tlpSelectProds.ResumeLayout(false);
            this.tlpSelectProds.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSelectProduct)).EndInit();
            this.tableLayoutPanel4.ResumeLayout(false);
            this.tableLayoutPanel4.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.tlpAddPayment.ResumeLayout(false);
            this.tlpAddPayment.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.TableLayoutPanel tlpSelectClient;
        private System.Windows.Forms.Button btnNextPnl1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.DataGridView dgvSelectClient;
        private System.Windows.Forms.TextBox tbSearchBarClient;
        private System.Windows.Forms.TableLayoutPanel tlpSelectProds;
        private System.Windows.Forms.Button btnFinish;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbSearchBarProducts;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ListBox lbAddedProds;
        private System.Windows.Forms.Button btnAddProd;
        private System.Windows.Forms.Button btnRemoveProd;
        private System.Windows.Forms.TextBox tbQuantity;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox tbSubTotal;
        private System.Windows.Forms.DataGridView dgvSelectProduct;
        private System.Windows.Forms.Button btnBackPnl2;
        private System.Windows.Forms.TableLayoutPanel tlpAddPayment;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button btnBackContact;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtReference;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.DateTimePicker dateTimePicker1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtSummaryAmount;
        private System.Windows.Forms.TextBox txtClientsummary;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.ComboBox cbPaymentMethods;
    }
}