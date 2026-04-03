namespace WinformsUI.UserControls.CustomDGV
{
    partial class CustomDGVFunctions
    {
        /// <summary> 
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;


        #region Código generado por el Diseñador de componentes

        /// <summary> 
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.toolStrip6 = new System.Windows.Forms.ToolStrip();
            this.btnCells = new System.Windows.Forms.ToolStripDropDownButton();
            this.btnTallerRow = new System.Windows.Forms.ToolStripMenuItem();
            this.btnShorterRow = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.btnLongerColumn = new System.Windows.Forms.ToolStripMenuItem();
            this.btnShorterColumn = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.btnAutoRow = new System.Windows.Forms.ToolStripMenuItem();
            this.btnAutoColumn = new System.Windows.Forms.ToolStripMenuItem();
            this.btnGrids = new System.Windows.Forms.ToolStripDropDownButton();
            this.btnAllBorders = new System.Windows.Forms.ToolStripMenuItem();
            this.btnJustVerticalBorders = new System.Windows.Forms.ToolStripMenuItem();
            this.btnNoBorders = new System.Windows.Forms.ToolStripMenuItem();
            this.btnText = new System.Windows.Forms.ToolStripDropDownButton();
            this.btnAlignLeft = new System.Windows.Forms.ToolStripMenuItem();
            this.btnAlignCenter = new System.Windows.Forms.ToolStripMenuItem();
            this.btnAlignRight = new System.Windows.Forms.ToolStripMenuItem();
            this.btnCopy = new System.Windows.Forms.ToolStripDropDownButton();
            this.btnCopyAsText = new System.Windows.Forms.ToolStripMenuItem();
            this.btnCopyAsCell = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStrip6.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip6
            // 
            this.toolStrip6.BackColor = System.Drawing.SystemColors.Control;
            this.toolStrip6.CanOverflow = false;
            this.toolStrip6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStrip6.Font = new System.Drawing.Font("Microsoft YaHei UI", 8F);
            this.toolStrip6.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip6.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnCells,
            this.btnGrids,
            this.btnText,
            this.btnCopy});
            this.toolStrip6.Location = new System.Drawing.Point(0, 0);
            this.toolStrip6.Name = "toolStrip6";
            this.toolStrip6.Padding = new System.Windows.Forms.Padding(0);
            this.toolStrip6.Size = new System.Drawing.Size(394, 87);
            this.toolStrip6.TabIndex = 118;
            this.toolStrip6.Tag = "NoZoom";
            this.toolStrip6.Text = "Gestionar";
            // 
            // btnCells
            // 
            this.btnCells.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnTallerRow,
            this.btnShorterRow,
            this.toolStripSeparator2,
            this.btnLongerColumn,
            this.btnShorterColumn,
            this.toolStripSeparator4,
            this.btnAutoRow,
            this.btnAutoColumn});
            this.btnCells.Font = new System.Drawing.Font("Microsoft YaHei UI", 8F);
            this.btnCells.Image = global::WinformsUI.Properties.Resources.BigIconCell;
            this.btnCells.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.btnCells.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnCells.Name = "btnCells";
            this.btnCells.Padding = new System.Windows.Forms.Padding(15, 0, 20, 0);
            this.btnCells.Size = new System.Drawing.Size(89, 84);
            this.btnCells.Tag = "";
            this.btnCells.Text = "Celdas";
            this.btnCells.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            // 
            // btnTallerRow
            // 
            this.btnTallerRow.Image = global::WinformsUI.Properties.Resources.ColumnaMasAlturaIcon;
            this.btnTallerRow.Name = "btnTallerRow";
            this.btnTallerRow.Size = new System.Drawing.Size(240, 22);
            this.btnTallerRow.Text = "Más alta (Alt+↑)";
            // 
            // btnShorterRow
            // 
            this.btnShorterRow.Image = global::WinformsUI.Properties.Resources.ColumnaMenosAlturaIcon;
            this.btnShorterRow.Name = "btnShorterRow";
            this.btnShorterRow.Size = new System.Drawing.Size(240, 22);
            this.btnShorterRow.Text = "Más baja (Alt+↓)";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(237, 6);
            // 
            // btnLongerColumn
            // 
            this.btnLongerColumn.Image = global::WinformsUI.Properties.Resources.FilaExpandir;
            this.btnLongerColumn.Name = "btnLongerColumn";
            this.btnLongerColumn.Size = new System.Drawing.Size(240, 22);
            this.btnLongerColumn.Text = "Más larga (Alt+→)";
            // 
            // btnShorterColumn
            // 
            this.btnShorterColumn.Image = global::WinformsUI.Properties.Resources.FilaContraer;
            this.btnShorterColumn.Name = "btnShorterColumn";
            this.btnShorterColumn.Size = new System.Drawing.Size(240, 22);
            this.btnShorterColumn.Text = "Más corta (Alt+←)";
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(237, 6);
            // 
            // btnAutoRow
            // 
            this.btnAutoRow.Image = global::WinformsUI.Properties.Resources.FilaDefecto;
            this.btnAutoRow.Name = "btnAutoRow";
            this.btnAutoRow.Size = new System.Drawing.Size(240, 22);
            this.btnAutoRow.Text = "Altura automática (Alt+Shift+↑)";
            // 
            // btnAutoColumn
            // 
            this.btnAutoColumn.Image = global::WinformsUI.Properties.Resources.ColumnaDefecto;
            this.btnAutoColumn.Name = "btnAutoColumn";
            this.btnAutoColumn.Size = new System.Drawing.Size(240, 22);
            this.btnAutoColumn.Text = "Largo automático (Alt+Shift+↓)";
            // 
            // btnGrids
            // 
            this.btnGrids.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnAllBorders,
            this.btnJustVerticalBorders,
            this.btnNoBorders});
            this.btnGrids.Font = new System.Drawing.Font("Microsoft YaHei UI", 8F);
            this.btnGrids.Image = global::WinformsUI.Properties.Resources.BigIconGrid;
            this.btnGrids.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.btnGrids.ImageTransparentColor = System.Drawing.Color.DarkTurquoise;
            this.btnGrids.Name = "btnGrids";
            this.btnGrids.Padding = new System.Windows.Forms.Padding(20, 0, 20, 0);
            this.btnGrids.Size = new System.Drawing.Size(87, 84);
            this.btnGrids.Tag = "";
            this.btnGrids.Text = "Grilla";
            this.btnGrids.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            // 
            // btnAllBorders
            // 
            this.btnAllBorders.Image = global::WinformsUI.Properties.Resources.cellAllBordersIcon;
            this.btnAllBorders.Name = "btnAllBorders";
            this.btnAllBorders.Size = new System.Drawing.Size(163, 22);
            this.btnAllBorders.Text = "Todos los bordes";
            // 
            // btnJustVerticalBorders
            // 
            this.btnJustVerticalBorders.Image = global::WinformsUI.Properties.Resources.cellVerticalBordersIcon;
            this.btnJustVerticalBorders.Name = "btnJustVerticalBorders";
            this.btnJustVerticalBorders.Size = new System.Drawing.Size(163, 22);
            this.btnJustVerticalBorders.Text = "Sólo verticales";
            // 
            // btnNoBorders
            // 
            this.btnNoBorders.Image = global::WinformsUI.Properties.Resources.cellNoBordersIcon;
            this.btnNoBorders.Name = "btnNoBorders";
            this.btnNoBorders.Size = new System.Drawing.Size(163, 22);
            this.btnNoBorders.Text = "Sin bordes";
            // 
            // btnText
            // 
            this.btnText.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnAlignLeft,
            this.btnAlignCenter,
            this.btnAlignRight});
            this.btnText.Font = new System.Drawing.Font("Microsoft YaHei UI", 8F);
            this.btnText.Image = global::WinformsUI.Properties.Resources.BigIconText;
            this.btnText.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.btnText.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnText.Name = "btnText";
            this.btnText.Padding = new System.Windows.Forms.Padding(20, 0, 20, 0);
            this.btnText.Size = new System.Drawing.Size(91, 84);
            this.btnText.Tag = "";
            this.btnText.Text = "Texto";
            this.btnText.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            // 
            // btnAlignLeft
            // 
            this.btnAlignLeft.Image = global::WinformsUI.Properties.Resources.alinearIzqIcon;
            this.btnAlignLeft.Name = "btnAlignLeft";
            this.btnAlignLeft.Size = new System.Drawing.Size(162, 22);
            this.btnAlignLeft.Text = "Alinear izquierda";
            // 
            // btnAlignCenter
            // 
            this.btnAlignCenter.Image = global::WinformsUI.Properties.Resources.alinearCenIcon;
            this.btnAlignCenter.Name = "btnAlignCenter";
            this.btnAlignCenter.Size = new System.Drawing.Size(162, 22);
            this.btnAlignCenter.Text = "Alinear centro";
            // 
            // btnAlignRight
            // 
            this.btnAlignRight.Image = global::WinformsUI.Properties.Resources.alinearDerIcon;
            this.btnAlignRight.Name = "btnAlignRight";
            this.btnAlignRight.Size = new System.Drawing.Size(162, 22);
            this.btnAlignRight.Text = "Alinear derecha";
            // 
            // btnCopy
            // 
            this.btnCopy.BackColor = System.Drawing.SystemColors.Control;
            this.btnCopy.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnCopyAsText,
            this.btnCopyAsCell});
            this.btnCopy.Font = new System.Drawing.Font("Microsoft YaHei UI", 8F);
            this.btnCopy.Image = global::WinformsUI.Properties.Resources.BigIconCopy;
            this.btnCopy.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.btnCopy.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnCopy.Name = "btnCopy";
            this.btnCopy.Padding = new System.Windows.Forms.Padding(15, 0, 20, 0);
            this.btnCopy.Size = new System.Drawing.Size(89, 84);
            this.btnCopy.Tag = "";
            this.btnCopy.Text = "Copiar";
            this.btnCopy.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            // 
            // btnCopyAsText
            // 
            this.btnCopyAsText.Name = "btnCopyAsText";
            this.btnCopyAsText.Size = new System.Drawing.Size(232, 22);
            this.btnCopyAsText.Text = "Copiar en Texto (Ctrl+Shift+C)";
            // 
            // btnCopyAsCell
            // 
            this.btnCopyAsCell.Name = "btnCopyAsCell";
            this.btnCopyAsCell.Size = new System.Drawing.Size(232, 22);
            this.btnCopyAsCell.Text = "Copiar para celda (Ctrl+C)";
            // 
            // CustomDGVFunctions
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.Controls.Add(this.toolStrip6);
            this.Name = "CustomDGVFunctions";
            this.Size = new System.Drawing.Size(394, 87);
            this.toolStrip6.ResumeLayout(false);
            this.toolStrip6.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip6;
        private System.Windows.Forms.ToolStripDropDownButton btnCells;
        private System.Windows.Forms.ToolStripMenuItem btnTallerRow;
        private System.Windows.Forms.ToolStripMenuItem btnShorterRow;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem btnLongerColumn;
        private System.Windows.Forms.ToolStripMenuItem btnShorterColumn;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripMenuItem btnAutoRow;
        private System.Windows.Forms.ToolStripMenuItem btnAutoColumn;
        private System.Windows.Forms.ToolStripDropDownButton btnGrids;
        private System.Windows.Forms.ToolStripMenuItem btnAllBorders;
        private System.Windows.Forms.ToolStripMenuItem btnJustVerticalBorders;
        private System.Windows.Forms.ToolStripMenuItem btnNoBorders;
        private System.Windows.Forms.ToolStripDropDownButton btnText;
        private System.Windows.Forms.ToolStripMenuItem btnAlignLeft;
        private System.Windows.Forms.ToolStripMenuItem btnAlignCenter;
        private System.Windows.Forms.ToolStripMenuItem btnAlignRight;
        private System.Windows.Forms.ToolStripDropDownButton btnCopy;
        private System.Windows.Forms.ToolStripMenuItem btnCopyAsText;
        private System.Windows.Forms.ToolStripMenuItem btnCopyAsCell;
    }
}
