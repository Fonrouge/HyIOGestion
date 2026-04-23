namespace WinformsUI.UserControls.Ribbon
{
    partial class EyeRestRibbon
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
            this.toolStrip = new System.Windows.Forms.ToolStrip();
            this.btnDarkRestMode = new System.Windows.Forms.ToolStripButton();
            this.btnLightRestMode = new System.Windows.Forms.ToolStripButton();
            this.btnCurrentMode = new System.Windows.Forms.ToolStripButton();
            this.toolStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip
            // 
            this.toolStrip.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.toolStrip.CanOverflow = false;
            this.toolStrip.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnDarkRestMode,
            this.btnLightRestMode,
            this.btnCurrentMode});
            this.toolStrip.Location = new System.Drawing.Point(0, 0);
            this.toolStrip.Name = "toolStrip";
            this.toolStrip.Padding = new System.Windows.Forms.Padding(0);
            this.toolStrip.Size = new System.Drawing.Size(308, 79);
            this.toolStrip.TabIndex = 35;
            this.toolStrip.Text = "toolStrip1";
            // 
            // btnDarkRestMode
            // 
            this.btnDarkRestMode.AutoSize = false;
            this.btnDarkRestMode.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnDarkRestMode.Font = new System.Drawing.Font("Microsoft YaHei UI", 8F);
            this.btnDarkRestMode.Image = global::WinformsUI.Properties.Resources.BigEyeIcon;
            this.btnDarkRestMode.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnDarkRestMode.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.btnDarkRestMode.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnDarkRestMode.Margin = new System.Windows.Forms.Padding(0);
            this.btnDarkRestMode.Name = "btnDarkRestMode";
            this.btnDarkRestMode.Padding = new System.Windows.Forms.Padding(27, 0, 15, 0);
            this.btnDarkRestMode.Size = new System.Drawing.Size(91, 73);
            this.btnDarkRestMode.Text = "Oscuro";
            this.btnDarkRestMode.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnDarkRestMode.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            // 
            // btnLightRestMode
            // 
            this.btnLightRestMode.AutoSize = false;
            this.btnLightRestMode.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnLightRestMode.Font = new System.Drawing.Font("Microsoft YaHei UI", 8F);
            this.btnLightRestMode.Image = global::WinformsUI.Properties.Resources.BigEyeIcon;
            this.btnLightRestMode.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnLightRestMode.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.btnLightRestMode.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnLightRestMode.Margin = new System.Windows.Forms.Padding(0);
            this.btnLightRestMode.Name = "btnLightRestMode";
            this.btnLightRestMode.Padding = new System.Windows.Forms.Padding(25, 0, 25, 0);
            this.btnLightRestMode.Size = new System.Drawing.Size(92, 73);
            this.btnLightRestMode.Text = "Claro";
            this.btnLightRestMode.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnLightRestMode.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            // 
            // btnCurrentMode
            // 
            this.btnCurrentMode.AutoSize = false;
            this.btnCurrentMode.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnCurrentMode.Font = new System.Drawing.Font("Microsoft YaHei UI", 8F);
            this.btnCurrentMode.Image = global::WinformsUI.Properties.Resources.BigEyeIcon;
            this.btnCurrentMode.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnCurrentMode.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.btnCurrentMode.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnCurrentMode.Margin = new System.Windows.Forms.Padding(0);
            this.btnCurrentMode.Name = "btnCurrentMode";
            this.btnCurrentMode.Padding = new System.Windows.Forms.Padding(39, 0, 0, 0);
            this.btnCurrentMode.Size = new System.Drawing.Size(92, 73);
            this.btnCurrentMode.Text = "Original";
            this.btnCurrentMode.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnCurrentMode.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            // 
            // EyeRestRibbon
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.Controls.Add(this.toolStrip);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "EyeRestRibbon";
            this.Size = new System.Drawing.Size(308, 79);
            this.toolStrip.ResumeLayout(false);
            this.toolStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip;
        private System.Windows.Forms.ToolStripButton btnDarkRestMode;
        private System.Windows.Forms.ToolStripButton btnLightRestMode;
        private System.Windows.Forms.ToolStripButton btnCurrentMode;
    }
}
