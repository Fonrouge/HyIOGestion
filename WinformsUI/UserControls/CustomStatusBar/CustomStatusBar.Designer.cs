namespace WinformsUI.UserControls.CustomStatusBar
{
    partial class CustomStatusBar
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

        #region Código generado por el Diseñador de componentes

        /// <summary> 
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.mainTableLayoutPnl = new System.Windows.Forms.TableLayoutPanel();
            this.txtUserName = new System.Windows.Forms.Label();
            this.txtWelcome = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.txtLoginTime = new System.Windows.Forms.Label();
            this.txtLoginTimeIndicator = new System.Windows.Forms.Label();
            this.txtBackUpStatus = new System.Windows.Forms.Label();
            this.txtBackUpName = new System.Windows.Forms.Label();
            this.btnMyAccount = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.lblBackUpTrafficLight = new System.Windows.Forms.Label();
            this.mainTableLayoutPnl.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainTableLayoutPnl
            // 
            this.mainTableLayoutPnl.BackColor = System.Drawing.Color.Transparent;
            this.mainTableLayoutPnl.ColumnCount = 11;
            this.mainTableLayoutPnl.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.mainTableLayoutPnl.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.mainTableLayoutPnl.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.mainTableLayoutPnl.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.mainTableLayoutPnl.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.mainTableLayoutPnl.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.mainTableLayoutPnl.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 11F));
            this.mainTableLayoutPnl.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.mainTableLayoutPnl.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.mainTableLayoutPnl.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 11F));
            this.mainTableLayoutPnl.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.mainTableLayoutPnl.Controls.Add(this.txtUserName, 5, 0);
            this.mainTableLayoutPnl.Controls.Add(this.txtWelcome, 4, 0);
            this.mainTableLayoutPnl.Controls.Add(this.label6, 9, 0);
            this.mainTableLayoutPnl.Controls.Add(this.txtLoginTime, 8, 0);
            this.mainTableLayoutPnl.Controls.Add(this.txtLoginTimeIndicator, 7, 0);
            this.mainTableLayoutPnl.Controls.Add(this.txtBackUpStatus, 2, 0);
            this.mainTableLayoutPnl.Controls.Add(this.txtBackUpName, 1, 0);
            this.mainTableLayoutPnl.Controls.Add(this.btnMyAccount, 10, 0);
            this.mainTableLayoutPnl.Controls.Add(this.label5, 3, 0);
            this.mainTableLayoutPnl.Controls.Add(this.lblBackUpTrafficLight, 0, 0);
            this.mainTableLayoutPnl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainTableLayoutPnl.Font = new System.Drawing.Font("Microsoft YaHei UI", 8F);
            this.mainTableLayoutPnl.Location = new System.Drawing.Point(0, 0);
            this.mainTableLayoutPnl.Margin = new System.Windows.Forms.Padding(3, 3, 0, 3);
            this.mainTableLayoutPnl.Name = "mainTableLayoutPnl";
            this.mainTableLayoutPnl.RowCount = 1;
            this.mainTableLayoutPnl.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.mainTableLayoutPnl.Size = new System.Drawing.Size(1266, 31);
            this.mainTableLayoutPnl.TabIndex = 2;
            this.mainTableLayoutPnl.Tag = "NonPaintable";
            // 
            // txtUserName
            // 
            this.txtUserName.AutoSize = true;
            this.txtUserName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtUserName.Font = new System.Drawing.Font("Microsoft YaHei UI", 8F);
            this.txtUserName.Location = new System.Drawing.Point(839, 0);
            this.txtUserName.Name = "txtUserName";
            this.txtUserName.Size = new System.Drawing.Size(75, 31);
            this.txtUserName.TabIndex = 10;
            this.txtUserName.Text = "txtUserName";
            this.txtUserName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtWelcome
            // 
            this.txtWelcome.AutoSize = true;
            this.txtWelcome.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtWelcome.Font = new System.Drawing.Font("Microsoft YaHei UI", 8F);
            this.txtWelcome.Location = new System.Drawing.Point(764, 0);
            this.txtWelcome.Name = "txtWelcome";
            this.txtWelcome.Size = new System.Drawing.Size(69, 31);
            this.txtWelcome.TabIndex = 9;
            this.txtWelcome.Text = "Bienvenido:";
            this.txtWelcome.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label6.ForeColor = System.Drawing.Color.Red;
            this.label6.Location = new System.Drawing.Point(1153, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(5, 31);
            this.label6.TabIndex = 8;
            this.label6.Text = "(Label no visible) - Recuadro de separación absoluto";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.label6.Visible = false;
            // 
            // txtLoginTime
            // 
            this.txtLoginTime.AutoSize = true;
            this.txtLoginTime.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtLoginTime.Font = new System.Drawing.Font("Microsoft YaHei UI", 8F);
            this.txtLoginTime.Location = new System.Drawing.Point(1071, 0);
            this.txtLoginTime.Name = "txtLoginTime";
            this.txtLoginTime.Size = new System.Drawing.Size(76, 31);
            this.txtLoginTime.TabIndex = 7;
            this.txtLoginTime.Text = "txtLoginTime";
            this.txtLoginTime.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtLoginTimeIndicator
            // 
            this.txtLoginTimeIndicator.AutoSize = true;
            this.txtLoginTimeIndicator.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtLoginTimeIndicator.Font = new System.Drawing.Font("Microsoft YaHei UI", 8F);
            this.txtLoginTimeIndicator.Location = new System.Drawing.Point(931, 0);
            this.txtLoginTimeIndicator.Name = "txtLoginTimeIndicator";
            this.txtLoginTimeIndicator.Size = new System.Drawing.Size(134, 31);
            this.txtLoginTimeIndicator.TabIndex = 6;
            this.txtLoginTimeIndicator.Text = "Hora de inicio se sesión:";
            this.txtLoginTimeIndicator.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtBackUpStatus
            // 
            this.txtBackUpStatus.AutoSize = true;
            this.txtBackUpStatus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtBackUpStatus.Font = new System.Drawing.Font("Microsoft YaHei UI", 8F);
            this.txtBackUpStatus.Location = new System.Drawing.Point(142, 0);
            this.txtBackUpStatus.Name = "txtBackUpStatus";
            this.txtBackUpStatus.Size = new System.Drawing.Size(52, 31);
            this.txtBackUpStatus.TabIndex = 4;
            this.txtBackUpStatus.Text = "Correcto";
            this.txtBackUpStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtBackUpName
            // 
            this.txtBackUpName.AutoSize = true;
            this.txtBackUpName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtBackUpName.Font = new System.Drawing.Font("Microsoft YaHei UI", 8F);
            this.txtBackUpName.Location = new System.Drawing.Point(41, 0);
            this.txtBackUpName.Name = "txtBackUpName";
            this.txtBackUpName.Size = new System.Drawing.Size(95, 31);
            this.txtBackUpName.TabIndex = 3;
            this.txtBackUpName.Text = "Backup semanal:";
            this.txtBackUpName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnMyAccount
            // 
            this.btnMyAccount.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnMyAccount.FlatAppearance.BorderSize = 0;
            this.btnMyAccount.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMyAccount.Location = new System.Drawing.Point(1164, 3);
            this.btnMyAccount.Name = "btnMyAccount";
            this.btnMyAccount.Size = new System.Drawing.Size(99, 25);
            this.btnMyAccount.TabIndex = 0;
            this.btnMyAccount.Tag = "Accentuable";
            this.btnMyAccount.Text = "Mi cuenta";
            this.btnMyAccount.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label5.ForeColor = System.Drawing.Color.Red;
            this.label5.Location = new System.Drawing.Point(200, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(558, 31);
            this.label5.TabIndex = 5;
            this.label5.Text = "(Label no visible) - Recuadro de separación porcentual\r\n";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.label5.Visible = false;
            // 
            // lblBackUpTrafficLight
            // 
            this.lblBackUpTrafficLight.AutoSize = true;
            this.lblBackUpTrafficLight.BackColor = System.Drawing.Color.Transparent;
            this.lblBackUpTrafficLight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblBackUpTrafficLight.Font = new System.Drawing.Font("Microsoft YaHei", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBackUpTrafficLight.ForeColor = System.Drawing.Color.LawnGreen;
            this.lblBackUpTrafficLight.Location = new System.Drawing.Point(3, 0);
            this.lblBackUpTrafficLight.Name = "lblBackUpTrafficLight";
            this.lblBackUpTrafficLight.Size = new System.Drawing.Size(32, 31);
            this.lblBackUpTrafficLight.TabIndex = 2;
            this.lblBackUpTrafficLight.Tag = "NonPaintable";
            this.lblBackUpTrafficLight.Text = "🟢";
            this.lblBackUpTrafficLight.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // CustomStatusBar
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.mainTableLayoutPnl);
            this.Name = "CustomStatusBar";
            this.Size = new System.Drawing.Size(1266, 31);
            this.mainTableLayoutPnl.ResumeLayout(false);
            this.mainTableLayoutPnl.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel mainTableLayoutPnl;
        private System.Windows.Forms.Label txtUserName;
        private System.Windows.Forms.Label txtWelcome;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label txtLoginTime;
        private System.Windows.Forms.Label txtLoginTimeIndicator;
        private System.Windows.Forms.Label txtBackUpStatus;
        private System.Windows.Forms.Label txtBackUpName;
        private System.Windows.Forms.Button btnMyAccount;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label lblBackUpTrafficLight;
    }
}
