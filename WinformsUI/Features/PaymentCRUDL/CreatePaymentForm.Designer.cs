namespace WinformsUI.Features.PaymentCRUDL
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
            this.tlpAddPayment = new System.Windows.Forms.TableLayoutPanel();
            this.btnFinish = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtAmount = new System.Windows.Forms.TextBox();
            this.txtEffectiveDate = new System.Windows.Forms.TextBox();
            this.txtClient = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.txtMethod = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtReference = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.tlpAddPayment.SuspendLayout();
            this.SuspendLayout();
            // 
            // tlpAddPayment
            // 
            this.tlpAddPayment.ColumnCount = 2;
            this.tlpAddPayment.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 15.76433F));
            this.tlpAddPayment.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 84.23567F));
            this.tlpAddPayment.Controls.Add(this.btnFinish, 0, 6);
            this.tlpAddPayment.Controls.Add(this.label7, 0, 3);
            this.tlpAddPayment.Controls.Add(this.label1, 0, 1);
            this.tlpAddPayment.Controls.Add(this.label2, 0, 2);
            this.tlpAddPayment.Controls.Add(this.txtAmount, 1, 1);
            this.tlpAddPayment.Controls.Add(this.txtEffectiveDate, 1, 2);
            this.tlpAddPayment.Controls.Add(this.txtClient, 1, 3);
            this.tlpAddPayment.Controls.Add(this.label8, 0, 4);
            this.tlpAddPayment.Controls.Add(this.txtMethod, 1, 4);
            this.tlpAddPayment.Controls.Add(this.label3, 0, 5);
            this.tlpAddPayment.Controls.Add(this.txtReference, 1, 5);
            this.tlpAddPayment.Controls.Add(this.label9, 0, 0);
            this.tlpAddPayment.Location = new System.Drawing.Point(0, 0);
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
            this.tlpAddPayment.Size = new System.Drawing.Size(592, 589);
            this.tlpAddPayment.TabIndex = 6;
            // 
            // btnFinish
            // 
            this.btnFinish.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.tlpAddPayment.SetColumnSpan(this.btnFinish, 2);
            this.btnFinish.Location = new System.Drawing.Point(19, 487);
            this.btnFinish.Margin = new System.Windows.Forms.Padding(19, 6, 5, 6);
            this.btnFinish.Name = "btnFinish";
            this.btnFinish.Size = new System.Drawing.Size(555, 69);
            this.btnFinish.TabIndex = 5;
            this.btnFinish.Tag = "Accentuable";
            this.btnFinish.Text = "Finalizar";
            this.btnFinish.UseVisualStyleBackColor = true;
            // 
            // label7
            // 
            this.label7.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(5, 298);
            this.label7.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(81, 19);
            this.label7.TabIndex = 2;
            this.label7.Text = "Cliente";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(5, 169);
            this.label1.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(81, 19);
            this.label1.TabIndex = 2;
            this.label1.Text = "Monto";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(5, 236);
            this.label2.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(81, 19);
            this.label2.TabIndex = 2;
            this.label2.Text = "Fecha";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtAmount
            // 
            this.txtAmount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtAmount.Location = new System.Drawing.Point(96, 166);
            this.txtAmount.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.txtAmount.Name = "txtAmount";
            this.txtAmount.Size = new System.Drawing.Size(478, 24);
            this.txtAmount.TabIndex = 0;
            // 
            // txtEffectiveDate
            // 
            this.txtEffectiveDate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtEffectiveDate.Location = new System.Drawing.Point(96, 233);
            this.txtEffectiveDate.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.txtEffectiveDate.Name = "txtEffectiveDate";
            this.txtEffectiveDate.Size = new System.Drawing.Size(478, 24);
            this.txtEffectiveDate.TabIndex = 0;
            // 
            // txtClient
            // 
            this.txtClient.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtClient.FormattingEnabled = true;
            this.txtClient.Location = new System.Drawing.Point(96, 297);
            this.txtClient.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.txtClient.Name = "txtClient";
            this.txtClient.Size = new System.Drawing.Size(478, 27);
            this.txtClient.TabIndex = 1;
            // 
            // label8
            // 
            this.label8.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(5, 366);
            this.label8.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(81, 19);
            this.label8.TabIndex = 2;
            this.label8.Text = "Método";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtMethod
            // 
            this.txtMethod.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtMethod.Location = new System.Drawing.Point(96, 363);
            this.txtMethod.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.txtMethod.Name = "txtMethod";
            this.txtMethod.Size = new System.Drawing.Size(478, 24);
            this.txtMethod.TabIndex = 0;
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(5, 426);
            this.label3.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(81, 19);
            this.label3.TabIndex = 2;
            this.label3.Text = "#Referencia";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtReference
            // 
            this.txtReference.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtReference.Location = new System.Drawing.Point(96, 423);
            this.txtReference.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.txtReference.Name = "txtReference";
            this.txtReference.Size = new System.Drawing.Size(478, 24);
            this.txtReference.TabIndex = 0;
            // 
            // label9
            // 
            this.label9.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.label9.AutoSize = true;
            this.tlpAddPayment.SetColumnSpan(this.label9, 2);
            this.label9.Font = new System.Drawing.Font("Microsoft YaHei", 27.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(18, 50);
            this.label9.Margin = new System.Windows.Forms.Padding(18, 0, 5, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(556, 48);
            this.label9.TabIndex = 4;
            this.label9.Text = "Ingresar pago";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // CreatePaymentForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 19F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(733, 682);
            this.Controls.Add(this.tlpAddPayment);
            this.Font = new System.Drawing.Font("Microsoft YaHei UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "CreatePaymentForm";
            this.Text = "CreatePaymentForm";
            this.tlpAddPayment.ResumeLayout(false);
            this.tlpAddPayment.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tlpAddPayment;
        private System.Windows.Forms.Button btnFinish;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtAmount;
        private System.Windows.Forms.TextBox txtEffectiveDate;
        private System.Windows.Forms.ComboBox txtClient;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtMethod;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtReference;
    }
}