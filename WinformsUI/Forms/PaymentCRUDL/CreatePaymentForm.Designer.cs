namespace WinformsUI.Forms.PaymentCRUDL
{
    partial class CreatePaymentForm
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
            this.pnlSelectClient = new System.Windows.Forms.Panel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.btnNextPnl1 = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.tbSearchBar = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.selectClientDGV = new System.Windows.Forms.DataGridView();
            this.tlpAddPayment = new System.Windows.Forms.TableLayoutPanel();
            this.btnFinish = new System.Windows.Forms.Button();
            this.btnBackContact = new System.Windows.Forms.Button();
            this.label9 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtReference = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.txtAmount = new System.Windows.Forms.TextBox();
            this.txtSelectedClient = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.cbPaymentMethods = new System.Windows.Forms.ComboBox();
            this.pnlSelectClient.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.selectClientDGV)).BeginInit();
            this.tlpAddPayment.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlSelectClient
            // 
            this.pnlSelectClient.Controls.Add(this.tableLayoutPanel1);
            this.pnlSelectClient.Location = new System.Drawing.Point(0, 0);
            this.pnlSelectClient.Name = "pnlSelectClient";
            this.pnlSelectClient.Size = new System.Drawing.Size(977, 589);
            this.pnlSelectClient.TabIndex = 7;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.btnNextPnl1, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.label4, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.tbSearchBar, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 2);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25.35971F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 74.64029F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 89F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(977, 589);
            this.tableLayoutPanel1.TabIndex = 0;
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
            this.btnNextPnl1.Text = "Finalizar";
            this.btnNextPnl1.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft YaHei", 27.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(18, 34);
            this.label4.Margin = new System.Windows.Forms.Padding(18, 0, 5, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(954, 48);
            this.label4.TabIndex = 5;
            this.label4.Text = "Seleccionar Venta";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tbSearchBar
            // 
            this.tbSearchBar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.tbSearchBar.Location = new System.Drawing.Point(19, 124);
            this.tbSearchBar.Margin = new System.Windows.Forms.Padding(19, 3, 19, 3);
            this.tbSearchBar.Name = "tbSearchBar";
            this.tbSearchBar.Size = new System.Drawing.Size(939, 24);
            this.tbSearchBar.TabIndex = 7;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.selectClientDGV);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(19, 159);
            this.panel1.Margin = new System.Windows.Forms.Padding(19, 3, 19, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(939, 337);
            this.panel1.TabIndex = 8;
            // 
            // selectClientDGV
            // 
            this.selectClientDGV.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.selectClientDGV.Dock = System.Windows.Forms.DockStyle.Fill;
            this.selectClientDGV.Location = new System.Drawing.Point(0, 0);
            this.selectClientDGV.Name = "selectClientDGV";
            this.selectClientDGV.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.selectClientDGV.Size = new System.Drawing.Size(939, 337);
            this.selectClientDGV.TabIndex = 7;
            // 
            // tlpAddPayment
            // 
            this.tlpAddPayment.ColumnCount = 2;
            this.tlpAddPayment.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 19.15368F));
            this.tlpAddPayment.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 80.84633F));
            this.tlpAddPayment.Controls.Add(this.btnFinish, 0, 6);
            this.tlpAddPayment.Controls.Add(this.btnBackContact, 0, 0);
            this.tlpAddPayment.Controls.Add(this.label9, 1, 0);
            this.tlpAddPayment.Controls.Add(this.label3, 0, 5);
            this.tlpAddPayment.Controls.Add(this.txtReference, 1, 5);
            this.tlpAddPayment.Controls.Add(this.label8, 0, 4);
            this.tlpAddPayment.Controls.Add(this.label2, 0, 3);
            this.tlpAddPayment.Controls.Add(this.dateTimePicker1, 1, 3);
            this.tlpAddPayment.Controls.Add(this.label1, 0, 2);
            this.tlpAddPayment.Controls.Add(this.txtAmount, 1, 2);
            this.tlpAddPayment.Controls.Add(this.txtSelectedClient, 1, 1);
            this.tlpAddPayment.Controls.Add(this.label5, 0, 1);
            this.tlpAddPayment.Controls.Add(this.cbPaymentMethods, 1, 4);
            this.tlpAddPayment.Location = new System.Drawing.Point(1004, 0);
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
            this.tlpAddPayment.TabIndex = 8;
            // 
            // btnFinish
            // 
            this.btnFinish.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.tlpAddPayment.SetColumnSpan(this.btnFinish, 2);
            this.btnFinish.Location = new System.Drawing.Point(19, 487);
            this.btnFinish.Margin = new System.Windows.Forms.Padding(19, 6, 5, 6);
            this.btnFinish.Name = "btnFinish";
            this.btnFinish.Size = new System.Drawing.Size(425, 69);
            this.btnFinish.TabIndex = 5;
            this.btnFinish.Tag = "Accentuable";
            this.btnFinish.Text = "Finalizar";
            this.btnFinish.UseVisualStyleBackColor = true;
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
            this.txtReference.Location = new System.Drawing.Point(91, 423);
            this.txtReference.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.txtReference.Name = "txtReference";
            this.txtReference.Size = new System.Drawing.Size(353, 24);
            this.txtReference.TabIndex = 0;
            // 
            // label8
            // 
            this.label8.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(5, 366);
            this.label8.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(76, 19);
            this.label8.TabIndex = 2;
            this.label8.Text = "Método";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(5, 298);
            this.label2.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(76, 19);
            this.label2.TabIndex = 2;
            this.label2.Text = "Fecha";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.dateTimePicker1.Location = new System.Drawing.Point(89, 296);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.Size = new System.Drawing.Size(357, 24);
            this.dateTimePicker1.TabIndex = 8;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(5, 236);
            this.label1.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(76, 19);
            this.label1.TabIndex = 2;
            this.label1.Text = "Monto";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtAmount
            // 
            this.txtAmount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtAmount.Location = new System.Drawing.Point(91, 233);
            this.txtAmount.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.txtAmount.Name = "txtAmount";
            this.txtAmount.Size = new System.Drawing.Size(353, 24);
            this.txtAmount.TabIndex = 0;
            // 
            // txtSelectedClient
            // 
            this.txtSelectedClient.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSelectedClient.Enabled = false;
            this.txtSelectedClient.Location = new System.Drawing.Point(91, 166);
            this.txtSelectedClient.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.txtSelectedClient.Name = "txtSelectedClient";
            this.txtSelectedClient.ReadOnly = true;
            this.txtSelectedClient.Size = new System.Drawing.Size(353, 24);
            this.txtSelectedClient.TabIndex = 0;
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(5, 169);
            this.label5.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(76, 19);
            this.label5.TabIndex = 2;
            this.label5.Text = "Cliente";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cbPaymentMethods
            // 
            this.cbPaymentMethods.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.cbPaymentMethods.FormattingEnabled = true;
            this.cbPaymentMethods.Location = new System.Drawing.Point(89, 365);
            this.cbPaymentMethods.Name = "cbPaymentMethods";
            this.cbPaymentMethods.Size = new System.Drawing.Size(357, 27);
            this.cbPaymentMethods.TabIndex = 9;
            // 
            // CreatePaymentForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 19F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1699, 714);
            this.Controls.Add(this.tlpAddPayment);
            this.Controls.Add(this.pnlSelectClient);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Microsoft YaHei UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "CreatePaymentForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "CreatePaymentForm";
            this.pnlSelectClient.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.selectClientDGV)).EndInit();
            this.tlpAddPayment.ResumeLayout(false);
            this.tlpAddPayment.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel pnlSelectClient;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button btnNextPnl1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tbSearchBar;
        private System.Windows.Forms.TableLayoutPanel tlpAddPayment;
        private System.Windows.Forms.Button btnFinish;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtAmount;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtReference;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Button btnBackContact;
        private System.Windows.Forms.DateTimePicker dateTimePicker1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.DataGridView selectClientDGV;
        private System.Windows.Forms.TextBox txtSelectedClient;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox cbPaymentMethods;
    }
}