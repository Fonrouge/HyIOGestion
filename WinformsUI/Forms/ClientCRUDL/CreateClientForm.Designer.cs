namespace WinformsUI.Forms.ClientCRUDL
{
    partial class CreateClientForm
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
            this.pnlIdentification = new System.Windows.Forms.Panel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.btnNextId = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.txtLastName = new System.Windows.Forms.TextBox();
            this.cbTaxId = new System.Windows.Forms.ComboBox();
            this.label9 = new System.Windows.Forms.Label();
            this.txtDocNumber = new System.Windows.Forms.TextBox();
            this.pnlShipAddress = new System.Windows.Forms.Panel();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.btnBackShipAddress = new System.Windows.Forms.Button();
            this.btnNextShip = new System.Windows.Forms.Button();
            this.cbZipCode = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.cbCountrySelector = new System.Windows.Forms.ComboBox();
            this.txtShipAddreess = new System.Windows.Forms.TextBox();
            this.tbState = new System.Windows.Forms.TextBox();
            this.pnlContact = new System.Windows.Forms.Panel();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.btnBackContact = new System.Windows.Forms.Button();
            this.txtEmail = new System.Windows.Forms.TextBox();
            this.label21 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.txtPhone = new System.Windows.Forms.TextBox();
            this.btnFinish = new System.Windows.Forms.Button();
            this.txtObservations = new System.Windows.Forms.TextBox();
            this.pnlIdentification.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.pnlShipAddress.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.pnlContact.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlIdentification
            // 
            this.pnlIdentification.Controls.Add(this.tableLayoutPanel1);
            this.pnlIdentification.Location = new System.Drawing.Point(0, 0);
            this.pnlIdentification.Margin = new System.Windows.Forms.Padding(4);
            this.pnlIdentification.Name = "pnlIdentification";
            this.pnlIdentification.Size = new System.Drawing.Size(492, 563);
            this.pnlIdentification.TabIndex = 0;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 15.76433F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 84.23567F));
            this.tableLayoutPanel1.Controls.Add(this.btnNextId, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.label7, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.label8, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.txtName, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.txtLastName, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.cbTaxId, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.label9, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.txtDocNumber, 1, 4);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(4);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.Padding = new System.Windows.Forms.Padding(0, 5, 10, 5);
            this.tableLayoutPanel1.RowCount = 6;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 64.08839F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 35.9116F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 74F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 70F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 69F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 136F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(492, 563);
            this.tableLayoutPanel1.TabIndex = 5;
            // 
            // btnNextId
            // 
            this.btnNextId.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.SetColumnSpan(this.btnNextId, 2);
            this.btnNextId.Location = new System.Drawing.Point(14, 453);
            this.btnNextId.Margin = new System.Windows.Forms.Padding(14, 4, 4, 4);
            this.btnNextId.Name = "btnNextId";
            this.btnNextId.Size = new System.Drawing.Size(464, 73);
            this.btnNextId.TabIndex = 5;
            this.btnNextId.Tag = "Accentuable";
            this.btnNextId.Text = "Siguiente";
            this.btnNextId.UseVisualStyleBackColor = true;
            // 
            // label7
            // 
            this.label7.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(4, 307);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(67, 19);
            this.label7.TabIndex = 2;
            this.label7.Text = "Tipo Doc";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label8
            // 
            this.label8.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(4, 377);
            this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(67, 19);
            this.label8.TabIndex = 2;
            this.label8.Text = "N° Doc";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(4, 162);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(67, 19);
            this.label1.TabIndex = 2;
            this.label1.Text = "Nombre";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(4, 235);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(67, 19);
            this.label2.TabIndex = 2;
            this.label2.Text = "Apellido";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtName
            // 
            this.txtName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtName.Location = new System.Drawing.Point(79, 159);
            this.txtName.Margin = new System.Windows.Forms.Padding(4);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(399, 24);
            this.txtName.TabIndex = 0;
            // 
            // txtLastName
            // 
            this.txtLastName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtLastName.Location = new System.Drawing.Point(79, 233);
            this.txtLastName.Margin = new System.Windows.Forms.Padding(4);
            this.txtLastName.Name = "txtLastName";
            this.txtLastName.Size = new System.Drawing.Size(399, 24);
            this.txtLastName.TabIndex = 0;
            // 
            // cbTaxId
            // 
            this.cbTaxId.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.cbTaxId.FormattingEnabled = true;
            this.cbTaxId.Location = new System.Drawing.Point(79, 306);
            this.cbTaxId.Margin = new System.Windows.Forms.Padding(4);
            this.cbTaxId.Name = "cbTaxId";
            this.cbTaxId.Size = new System.Drawing.Size(399, 27);
            this.cbTaxId.TabIndex = 1;
            // 
            // label9
            // 
            this.label9.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft YaHei", 27.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(79, 46);
            this.label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(399, 48);
            this.label9.TabIndex = 4;
            this.label9.Text = "Identificación";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtDocNumber
            // 
            this.txtDocNumber.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtDocNumber.Location = new System.Drawing.Point(79, 374);
            this.txtDocNumber.Margin = new System.Windows.Forms.Padding(4);
            this.txtDocNumber.Name = "txtDocNumber";
            this.txtDocNumber.Size = new System.Drawing.Size(399, 24);
            this.txtDocNumber.TabIndex = 0;
            // 
            // pnlShipAddress
            // 
            this.pnlShipAddress.Controls.Add(this.tableLayoutPanel3);
            this.pnlShipAddress.Location = new System.Drawing.Point(515, 0);
            this.pnlShipAddress.Margin = new System.Windows.Forms.Padding(4);
            this.pnlShipAddress.Name = "pnlShipAddress";
            this.pnlShipAddress.Size = new System.Drawing.Size(492, 563);
            this.pnlShipAddress.TabIndex = 3;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 2;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 15.76433F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 84.23567F));
            this.tableLayoutPanel3.Controls.Add(this.btnBackShipAddress, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.btnNextShip, 0, 5);
            this.tableLayoutPanel3.Controls.Add(this.cbZipCode, 1, 4);
            this.tableLayoutPanel3.Controls.Add(this.label13, 1, 0);
            this.tableLayoutPanel3.Controls.Add(this.label15, 0, 1);
            this.tableLayoutPanel3.Controls.Add(this.label16, 0, 2);
            this.tableLayoutPanel3.Controls.Add(this.label17, 0, 3);
            this.tableLayoutPanel3.Controls.Add(this.label18, 0, 4);
            this.tableLayoutPanel3.Controls.Add(this.cbCountrySelector, 1, 1);
            this.tableLayoutPanel3.Controls.Add(this.txtShipAddreess, 1, 3);
            this.tableLayoutPanel3.Controls.Add(this.tbState, 1, 2);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel3.Margin = new System.Windows.Forms.Padding(4);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.Padding = new System.Windows.Forms.Padding(0, 5, 10, 5);
            this.tableLayoutPanel3.RowCount = 6;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 64.08839F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 35.9116F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 74F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 70F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 69F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 136F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(492, 563);
            this.tableLayoutPanel3.TabIndex = 5;
            // 
            // btnBackShipAddress
            // 
            this.btnBackShipAddress.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBackShipAddress.BackColor = System.Drawing.Color.Transparent;
            this.btnBackShipAddress.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnBackShipAddress.FlatAppearance.BorderSize = 0;
            this.btnBackShipAddress.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBackShipAddress.Image = global::WinformsUI.Properties.Resources.BackArrow;
            this.btnBackShipAddress.Location = new System.Drawing.Point(4, 30);
            this.btnBackShipAddress.Margin = new System.Windows.Forms.Padding(4);
            this.btnBackShipAddress.Name = "btnBackShipAddress";
            this.btnBackShipAddress.Size = new System.Drawing.Size(67, 79);
            this.btnBackShipAddress.TabIndex = 7;
            this.btnBackShipAddress.Tag = "IsImageColorable";
            this.btnBackShipAddress.UseVisualStyleBackColor = false;
            // 
            // btnNextShip
            // 
            this.btnNextShip.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel3.SetColumnSpan(this.btnNextShip, 2);
            this.btnNextShip.Location = new System.Drawing.Point(14, 453);
            this.btnNextShip.Margin = new System.Windows.Forms.Padding(14, 4, 4, 4);
            this.btnNextShip.Name = "btnNextShip";
            this.btnNextShip.Size = new System.Drawing.Size(464, 73);
            this.btnNextShip.TabIndex = 5;
            this.btnNextShip.Tag = "Accentuable";
            this.btnNextShip.Text = "Siguiente";
            this.btnNextShip.UseVisualStyleBackColor = true;
            // 
            // cbZipCode
            // 
            this.cbZipCode.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.cbZipCode.Location = new System.Drawing.Point(79, 374);
            this.cbZipCode.Margin = new System.Windows.Forms.Padding(4);
            this.cbZipCode.Name = "cbZipCode";
            this.cbZipCode.Size = new System.Drawing.Size(399, 24);
            this.cbZipCode.TabIndex = 0;
            // 
            // label13
            // 
            this.label13.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("Microsoft YaHei", 27.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.Location = new System.Drawing.Point(79, 46);
            this.label13.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(399, 48);
            this.label13.TabIndex = 4;
            this.label13.Text = "Dirección de envío";
            this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label15
            // 
            this.label15.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(4, 162);
            this.label15.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(67, 19);
            this.label15.TabIndex = 2;
            this.label15.Text = "País";
            this.label15.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label16
            // 
            this.label16.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(4, 235);
            this.label16.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(67, 19);
            this.label16.TabIndex = 2;
            this.label16.Text = "Provincia";
            this.label16.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label17
            // 
            this.label17.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(4, 307);
            this.label17.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(67, 19);
            this.label17.TabIndex = 2;
            this.label17.Text = "Dirección";
            this.label17.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label18
            // 
            this.label18.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(4, 367);
            this.label18.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(67, 38);
            this.label18.TabIndex = 2;
            this.label18.Text = "Código Postal";
            this.label18.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cbCountrySelector
            // 
            this.cbCountrySelector.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.cbCountrySelector.FormattingEnabled = true;
            this.cbCountrySelector.Location = new System.Drawing.Point(79, 158);
            this.cbCountrySelector.Margin = new System.Windows.Forms.Padding(4);
            this.cbCountrySelector.Name = "cbCountrySelector";
            this.cbCountrySelector.Size = new System.Drawing.Size(399, 27);
            this.cbCountrySelector.TabIndex = 1;
            // 
            // txtShipAddreess
            // 
            this.txtShipAddreess.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtShipAddreess.Location = new System.Drawing.Point(79, 305);
            this.txtShipAddreess.Margin = new System.Windows.Forms.Padding(4);
            this.txtShipAddreess.Name = "txtShipAddreess";
            this.txtShipAddreess.Size = new System.Drawing.Size(399, 24);
            this.txtShipAddreess.TabIndex = 0;
            // 
            // tbState
            // 
            this.tbState.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.tbState.Location = new System.Drawing.Point(79, 233);
            this.tbState.Margin = new System.Windows.Forms.Padding(4);
            this.tbState.Name = "tbState";
            this.tbState.Size = new System.Drawing.Size(399, 24);
            this.tbState.TabIndex = 0;
            // 
            // pnlContact
            // 
            this.pnlContact.Controls.Add(this.tableLayoutPanel4);
            this.pnlContact.Location = new System.Drawing.Point(1040, 0);
            this.pnlContact.Margin = new System.Windows.Forms.Padding(4);
            this.pnlContact.Name = "pnlContact";
            this.pnlContact.Size = new System.Drawing.Size(492, 563);
            this.pnlContact.TabIndex = 8;
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.ColumnCount = 2;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.95721F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 83.04279F));
            this.tableLayoutPanel4.Controls.Add(this.btnBackContact, 0, 0);
            this.tableLayoutPanel4.Controls.Add(this.txtEmail, 1, 1);
            this.tableLayoutPanel4.Controls.Add(this.label21, 0, 3);
            this.tableLayoutPanel4.Controls.Add(this.label4, 0, 2);
            this.tableLayoutPanel4.Controls.Add(this.label11, 1, 0);
            this.tableLayoutPanel4.Controls.Add(this.label5, 0, 1);
            this.tableLayoutPanel4.Controls.Add(this.txtPhone, 1, 2);
            this.tableLayoutPanel4.Controls.Add(this.btnFinish, 0, 5);
            this.tableLayoutPanel4.Controls.Add(this.txtObservations, 1, 3);
            this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel4.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel4.Margin = new System.Windows.Forms.Padding(4);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.Padding = new System.Windows.Forms.Padding(0, 5, 10, 5);
            this.tableLayoutPanel4.RowCount = 6;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 64.08839F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 35.9116F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 74F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 70F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 69F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 136F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(492, 563);
            this.tableLayoutPanel4.TabIndex = 5;
            // 
            // btnBackContact
            // 
            this.btnBackContact.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBackContact.BackColor = System.Drawing.Color.Transparent;
            this.btnBackContact.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnBackContact.FlatAppearance.BorderSize = 0;
            this.btnBackContact.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBackContact.Image = global::WinformsUI.Properties.Resources.BackArrow;
            this.btnBackContact.Location = new System.Drawing.Point(4, 30);
            this.btnBackContact.Margin = new System.Windows.Forms.Padding(4);
            this.btnBackContact.Name = "btnBackContact";
            this.btnBackContact.Size = new System.Drawing.Size(73, 79);
            this.btnBackContact.TabIndex = 8;
            this.btnBackContact.Tag = "IsImageColorable";
            this.btnBackContact.UseVisualStyleBackColor = false;
            // 
            // txtEmail
            // 
            this.txtEmail.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtEmail.Location = new System.Drawing.Point(85, 159);
            this.txtEmail.Margin = new System.Windows.Forms.Padding(4);
            this.txtEmail.Name = "txtEmail";
            this.txtEmail.Size = new System.Drawing.Size(393, 24);
            this.txtEmail.TabIndex = 0;
            // 
            // label21
            // 
            this.label21.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.label21.AutoSize = true;
            this.label21.Location = new System.Drawing.Point(4, 307);
            this.label21.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(73, 19);
            this.label21.TabIndex = 2;
            this.label21.Text = "Notas";
            this.label21.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(4, 235);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(73, 19);
            this.label4.TabIndex = 2;
            this.label4.Text = "Teléfono";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label11
            // 
            this.label11.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Microsoft YaHei", 27.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.Location = new System.Drawing.Point(85, 46);
            this.label11.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(393, 48);
            this.label11.TabIndex = 4;
            this.label11.Text = "Contacto";
            this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(4, 162);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(73, 19);
            this.label5.TabIndex = 2;
            this.label5.Text = "Email";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtPhone
            // 
            this.txtPhone.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtPhone.Location = new System.Drawing.Point(85, 233);
            this.txtPhone.Margin = new System.Windows.Forms.Padding(4);
            this.txtPhone.Name = "txtPhone";
            this.txtPhone.Size = new System.Drawing.Size(393, 24);
            this.txtPhone.TabIndex = 0;
            // 
            // btnFinish
            // 
            this.btnFinish.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel4.SetColumnSpan(this.btnFinish, 2);
            this.btnFinish.Location = new System.Drawing.Point(14, 453);
            this.btnFinish.Margin = new System.Windows.Forms.Padding(14, 4, 4, 4);
            this.btnFinish.Name = "btnFinish";
            this.btnFinish.Size = new System.Drawing.Size(464, 73);
            this.btnFinish.TabIndex = 5;
            this.btnFinish.Tag = "Accentuable";
            this.btnFinish.Text = "Finalizar";
            this.btnFinish.UseVisualStyleBackColor = true;
            // 
            // txtObservations
            // 
            this.txtObservations.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtObservations.Location = new System.Drawing.Point(85, 286);
            this.txtObservations.Margin = new System.Windows.Forms.Padding(4);
            this.txtObservations.Multiline = true;
            this.txtObservations.Name = "txtObservations";
            this.tableLayoutPanel4.SetRowSpan(this.txtObservations, 2);
            this.txtObservations.Size = new System.Drawing.Size(393, 131);
            this.txtObservations.TabIndex = 0;
            // 
            // CreateClientForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 19F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1571, 605);
            this.Controls.Add(this.pnlContact);
            this.Controls.Add(this.pnlShipAddress);
            this.Controls.Add(this.pnlIdentification);
            this.Font = new System.Drawing.Font("Microsoft YaHei UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CreateClientForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "CreateClientForm";
            this.pnlIdentification.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.pnlShipAddress.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            this.pnlContact.ResumeLayout(false);
            this.tableLayoutPanel4.ResumeLayout(false);
            this.tableLayoutPanel4.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlIdentification;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cbTaxId;
        private System.Windows.Forms.TextBox txtLastName;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Button btnNextId;
        private System.Windows.Forms.Panel pnlShipAddress;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.Button btnNextShip;
        private System.Windows.Forms.TextBox cbZipCode;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.ComboBox cbCountrySelector;
        private System.Windows.Forms.Panel pnlContact;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private System.Windows.Forms.Button btnFinish;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtEmail;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox txtPhone;
        private System.Windows.Forms.TextBox txtObservations;
        private System.Windows.Forms.TextBox txtDocNumber;
        private System.Windows.Forms.TextBox txtShipAddreess;
        private System.Windows.Forms.Button btnBackShipAddress;
        private System.Windows.Forms.Button btnBackContact;
        private System.Windows.Forms.TextBox tbState;
    }
}