using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Winforms.Theme;

namespace WinformsUI.UserControls.CustomDGV
{
    public class DGVCellsStyle
    {
        // ===== Alturas  =====
        private const int RowMinPx = 18;
        private const int RowMaxPx = 72;
        private const int RowStepPx = 4;

        private const int HeaderMinPx = 22;
        private const int HeaderMaxPx = 64;
        private const int HeaderStepPx = 4;

        // ===== NUEVO: Anchos de columnas =====
        private const int ColMinPx = 48;
        private const int ColMaxPx = 600;
        private const int ColStepPx = 16;

        // Extra para medir ancho (padding/bordes/glyph)
        private const int SortGlyphExtraPx = 18;  // aproximación del icono de orden
        private const int CellBorderExtraPx = 2;  // border interno

        private enum HAlign { Left, Center, Right }
        private DataGridView _dgv;

        private static int Clamp(int v, int min, int max) => v < min ? min : (v > max ? max : v);

        public void SetDGV(DataGridView dgv) => _dgv = dgv;

        // ---------- ESTADO (filas / header) ----------
        public int CurrentRowHeight
        {
            get
            {
                if (_dgv == null) return RowMinPx;
                if (_dgv.RowTemplate?.Height > 0) return _dgv.RowTemplate.Height;
                var first = _dgv.Rows.Cast<DataGridViewRow>().FirstOrDefault();
                return first?.Height > 0 ? first.Height : RowMinPx;
            }
        }
        public int CurrentHeaderHeight => _dgv?.ColumnHeadersHeight ?? HeaderMinPx;

        public bool CanIncreaseRowHeight(int step = RowStepPx)
        {
            if (_dgv == null) return false;
            var s = step < 0 ? -step : step;
            return CurrentRowHeight + s <= RowMaxPx;
        }
        public bool CanDecreaseRowHeight(int step = RowStepPx)
        {
            if (_dgv == null) return false;
            var s = step < 0 ? -step : step;
            return CurrentRowHeight - s >= RowMinPx;
        }
        public bool CanIncreaseHeaderHeight(int step = HeaderStepPx)
        {
            if (_dgv == null) return false;
            var s = step < 0 ? -step : step;
            return CurrentHeaderHeight + s <= HeaderMaxPx;
        }
        public bool CanDecreaseHeaderHeight(int step = HeaderStepPx)
        {
            if (_dgv == null) return false;
            var s = step < 0 ? -step : step;
            return CurrentHeaderHeight - s >= HeaderMinPx;
        }
        public (bool canGrowRows, bool canShrinkRows, bool canGrowHeader, bool canShrinkHeader)
            GetCanNudgeState(int rowStep = RowStepPx, int headerStep = HeaderStepPx)
            => (CanIncreaseRowHeight(rowStep), CanDecreaseRowHeight(rowStep),
                CanIncreaseHeaderHeight(headerStep), CanDecreaseHeaderHeight(headerStep));
        // ====================== NUEVO: BORDES ======================
        public void SetGridAllBorders()
        {
            if (_dgv == null) return;
            _dgv.CellBorderStyle = DataGridViewCellBorderStyle.None;
            _dgv.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            _dgv.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            _dgv.Invalidate();
        }

        public void SetGridVerticalBordersOnly()
        {
            if (_dgv == null) return;
            _dgv.CellBorderStyle = DataGridViewCellBorderStyle.None;
            _dgv.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            _dgv.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            _dgv.Invalidate();
        }

        public void SetGridNoBorders()
        {
            if (_dgv == null) return;
            _dgv.EnableHeadersVisualStyles = false;

            _dgv.CellBorderStyle = DataGridViewCellBorderStyle.None;
            _dgv.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            _dgv.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;

            _dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.Green;
            _dgv.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;

            _dgv.Invalidate();
            
        }

        // ====================== NUEVO: COPIAR ======================
        public void CopySelectedToClipboardAsText(bool includeHeaders = true)
        {
            if (_dgv == null || _dgv.SelectedCells.Count == 0) return;

            var sb = new System.Text.StringBuilder();

            // Headers (opcional)
            if (includeHeaders)
            {
                var headers = _dgv.Columns.Cast<DataGridViewColumn>()
                    .Where(c => c.Visible)
                    .Select(c => c.HeaderText);

                sb.AppendLine(string.Join("\t", headers));
            }

            // Filas seleccionadas (ordenadas)
            var rows = _dgv.SelectedCells.Cast<DataGridViewCell>()
                .Select(c => c.OwningRow)
                .Distinct()
                .OrderBy(r => r.Index);

            foreach (var row in rows)
            {
                var values = row.Cells.Cast<DataGridViewCell>()
                    .Where(c => c.Visible && c.OwningColumn.Visible)
                    .Select(c => Convert.ToString(c.Value) ?? "");

                sb.AppendLine(string.Join("\t", values));
            }

            if (sb.Length > 0)
                Clipboard.SetText(sb.ToString().TrimEnd());
        }

        public void CopySelectedToClipboardStandard()
        {
            if (_dgv == null) return;

            // Comportamiento nativo de Ctrl+C del DataGridView
            if (_dgv.GetClipboardContent() is DataObject dataObj)
                Clipboard.SetDataObject(dataObj);
        }


        /// <summary>
        /// Ajusta alturas (delta +/−). Opcional: tocar header también.
        /// </summary>
        public void NudgeHeights(int deltaRowsPx, bool alsoHeader = false, int deltaHeaderPx = 0)
        {
            if (_dgv == null) return;

            if (_dgv.AutoSizeRowsMode != DataGridViewAutoSizeRowsMode.None)
                _dgv.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;

            try
            {
                var newRowH = Clamp(CurrentRowHeight + deltaRowsPx, RowMinPx, RowMaxPx);
                _dgv.RowTemplate.Height = newRowH;
                foreach (DataGridViewRow r in _dgv.Rows) r.Height = newRowH;

                if (alsoHeader)
                {
                    if (_dgv.ColumnHeadersHeightSizeMode != DataGridViewColumnHeadersHeightSizeMode.EnableResizing)
                        _dgv.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;

                    var step = deltaHeaderPx == 0 ? (deltaRowsPx >= 0 ? HeaderStepPx : -HeaderStepPx) : deltaHeaderPx;
                    _dgv.ColumnHeadersHeight = Clamp(_dgv.ColumnHeadersHeight + step, HeaderMinPx, HeaderMaxPx);
                }
            }
            catch { /* swallow */ }
        }

        // ---------- NUEVO: ESTADO (columnas) ----------
        /// <summary>Devuelve las columnas objetivo (visibles) según onlySelected.</summary>
        private DataGridViewColumn[] TargetColumns(bool onlySelected)
        {
            if (_dgv == null) return new DataGridViewColumn[0];

            var cols = onlySelected && _dgv.SelectedColumns.Count > 0
                ? _dgv.SelectedColumns.Cast<DataGridViewColumn>()
                : _dgv.Columns.Cast<DataGridViewColumn>();

            return cols.Where(c => c.Visible).ToArray();
        }

        /// <summary>¿Se puede ensanchar al menos una columna con el paso dado?</summary>
        public bool CanIncreaseAnyColumnWidth(int step = ColStepPx, bool onlySelected = false)
        {
            var s = step < 0 ? -step : step;
            return TargetColumns(onlySelected).Any(c => (c.Width + s) <= ColMaxPx);
        }

        /// <summary>¿Se puede achicar al menos una columna con el paso dado?</summary>
        public bool CanDecreaseAnyColumnWidth(int step = ColStepPx, bool onlySelected = false)
        {
            var s = step < 0 ? -step : step;
            return TargetColumns(onlySelected).Any(c => (c.Width - s) >= ColMinPx);
        }

        /// <summary>Conveniencia: estado de columnas de una sola vez.</summary>
        public (bool canWider, bool canNarrower) GetCanNudgeColumnsState(int step = ColStepPx, bool onlySelected = false)
            => (CanIncreaseAnyColumnWidth(step, onlySelected), CanDecreaseAnyColumnWidth(step, onlySelected));

        // ---------- NUEVO: ACCIÓN (columnas) ----------
        /// <summary>
        /// Ajusta el ancho de columnas con un delta (+/−).
        /// - `onlySelected=true` → afecta solo columnas seleccionadas (o la de la celda actual si no hay selección).
        /// - Fuerza modo manual (AutoSize OFF) para que el ancho quede persistente y aparezca scroll horizontal.
        /// </summary>
        public void NudgeColumnWidths(int deltaPx, bool onlySelected = false)
        {
            if (_dgv == null) return;

            // Asegurar modo manual para que respete Width y habilite scroll horizontal
            if (_dgv.AutoSizeColumnsMode != DataGridViewAutoSizeColumnsMode.None)
                _dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;

            var targets = TargetColumns(onlySelected);
            if (onlySelected && targets.Length == 0 && _dgv.CurrentCell != null)
                targets = new[] { _dgv.CurrentCell.OwningColumn };

            foreach (var col in targets)
            {
                // Si la columna tiene su propio autosize, quitalo para que Width aplique.
                if (col.AutoSizeMode != DataGridViewAutoSizeColumnMode.None)
                    col.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;

                var newW = Clamp(col.Width + deltaPx, ColMinPx, ColMaxPx);
                col.Width = newW;
            }

            _dgv.Invalidate();
        }
        // ======== Helpers de medición ========
        private static int MeasureTextWidth(string text, Font font)
        {
            if (font == null) return 0;
            var sz = TextRenderer.MeasureText(text ?? string.Empty, font,
                new Size(int.MaxValue, int.MaxValue),
                TextFormatFlags.NoPrefix | TextFormatFlags.EndEllipsis | TextFormatFlags.NoPadding);
            return sz.Width;
        }
        private static int MeasureTextHeight(Font font)
        {
            if (font == null) return 0;
            var sz = TextRenderer.MeasureText("Ag", font,
                new Size(int.MaxValue, int.MaxValue),
                TextFormatFlags.NoPrefix | TextFormatFlags.NoPadding);
            return sz.Height;
        }

        // ======== RECOMENDADOS: ALTURA ========
        /// <summary>Altura recomendada para filas según fuente/padding actuales.</summary>
        public int GetRecommendedRowHeight(int extraPx = 2)
        {
            if (_dgv == null) return RowMinPx;

            var font = _dgv.DefaultCellStyle?.Font ?? _dgv.Font;
            int textH = MeasureTextHeight(font);
            var pad = _dgv.DefaultCellStyle?.Padding ?? Padding.Empty;

            int h = textH + pad.Vertical + CellBorderExtraPx + extraPx;
            return Clamp(h, RowMinPx, RowMaxPx);
        }

        /// <summary>Altura recomendada para el header de columnas según fuente/padding actuales.</summary>
        public int GetRecommendedHeaderHeight(int extraPx = 2)
        {
            if (_dgv == null) return HeaderMinPx;

            var font = _dgv.ColumnHeadersDefaultCellStyle?.Font ?? _dgv.Font;
            int textH = MeasureTextHeight(font);
            var pad = _dgv.ColumnHeadersDefaultCellStyle?.Padding ?? Padding.Empty;

            int h = textH + pad.Vertical + CellBorderExtraPx + extraPx;
            return Clamp(h, HeaderMinPx, HeaderMaxPx);
        }

        /// <summary>Aplica las alturas recomendadas (y opcionalmente el header).</summary>
        public void ApplyRecommendedRowHeight(bool alsoHeader = false, int extraRowPx = 2, int extraHeaderPx = 2)
        {
            if (_dgv == null) return;

            if (_dgv.AutoSizeRowsMode != DataGridViewAutoSizeRowsMode.None)
                _dgv.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;

            var recRow = GetRecommendedRowHeight(extraRowPx);
            _dgv.RowTemplate.Height = recRow;
            foreach (DataGridViewRow r in _dgv.Rows) r.Height = recRow;

            if (alsoHeader)
            {
                if (_dgv.ColumnHeadersHeightSizeMode != DataGridViewColumnHeadersHeightSizeMode.EnableResizing)
                    _dgv.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;

                _dgv.ColumnHeadersHeight = GetRecommendedHeaderHeight(extraHeaderPx);
            }
        }

        // ======== RECOMENDADOS: ANCHO ========
        /// <summary>
        /// Ancho recomendado para una columna, midiendo header y muestras de filas.
        /// - sampleRows: cuántas filas medir (para no recorrer todo). Toma las primeras visibles.
        /// - includeHeader: considera también el ancho del HeaderText.
        /// - extraPx: margen de aire a sumar.
        /// </summary>
        public int GetRecommendedColumnWidth(DataGridViewColumn col, int sampleRows = 200, bool includeHeader = true, int extraPx = 12)
        {
            if (_dgv == null || col == null) return ColMinPx;

            // Tomar fuentes/estilos
            var cellFont = col.DefaultCellStyle?.Font ?? _dgv.DefaultCellStyle?.Font ?? _dgv.Font;
            var headerFont = _dgv.ColumnHeadersDefaultCellStyle?.Font ?? _dgv.Font;

            // Padding horizontales
            var cellPad = col.DefaultCellStyle?.Padding ?? _dgv.DefaultCellStyle?.Padding ?? Padding.Empty;
            var hdrPad = _dgv.ColumnHeadersDefaultCellStyle?.Padding ?? Padding.Empty;

            int maxCellW = 0;

            // Medir algunas filas visibles (rápido)
            int measured = 0;
            foreach (DataGridViewRow row in _dgv.Rows)
            {
                if (sampleRows > 0 && measured >= sampleRows) break;
                if (row.IsNewRow || !row.Visible) continue;

                var cell = row.Cells[col.Index];
                string txt = Convert.ToString(cell?.Value) ?? string.Empty;
                int w = MeasureTextWidth(txt, cellFont);
                if (w > maxCellW) maxCellW = w;

                measured++;
            }

            // Header
            int headerW = 0;
            if (includeHeader)
            {
                headerW = MeasureTextWidth(col.HeaderText ?? string.Empty, headerFont);
                headerW += hdrPad.Horizontal + CellBorderExtraPx;
            }

            // Sumar paddings/glyph
            int contentW = maxCellW + cellPad.Horizontal + CellBorderExtraPx;
            if (col.SortMode != DataGridViewColumnSortMode.NotSortable)
                contentW += SortGlyphExtraPx;

            int target = Math.Max(contentW, headerW) + extraPx;
            return Clamp(target, ColMinPx, ColMaxPx);
        }

        /// <summary>
        /// Aplica anchos recomendados a columnas (todas o sólo seleccionadas).
        /// Respeta scroll horizontal (forzando manual).
        /// </summary>
        public void ApplyRecommendedColumnWidths(bool onlySelected = false, int sampleRows = 200, bool includeHeader = true, int extraPx = 12)
        {
            if (_dgv == null) return;

            if (_dgv.AutoSizeColumnsMode != DataGridViewAutoSizeColumnsMode.None)
                _dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;

            var cols = (onlySelected && _dgv.SelectedColumns.Count > 0
                        ? _dgv.SelectedColumns.Cast<DataGridViewColumn>()
                        : _dgv.Columns.Cast<DataGridViewColumn>())
                        .Where(c => c.Visible);

            foreach (var col in cols)
            {
                if (col.AutoSizeMode != DataGridViewAutoSizeColumnMode.None)
                    col.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;

                col.Width = GetRecommendedColumnWidth(col, sampleRows, includeHeader, extraPx);
            }
        }

        private static DataGridViewContentAlignment ToCellAlign(HAlign h)
        {
            switch (h)
            {
                case HAlign.Left:
                    return DataGridViewContentAlignment.MiddleLeft;
                case HAlign.Center:
                    return DataGridViewContentAlignment.MiddleCenter;
                case HAlign.Right:
                    return DataGridViewContentAlignment.MiddleRight;
                default:
                    return DataGridViewContentAlignment.MiddleLeft;
            }
        }

        private void ApplyAlignment(HAlign h, bool onlySelected = false, bool includeHeader = false)
        {
            if (_dgv == null) return;

            var cellAlign = ToCellAlign(h);
            var headerAlign = cellAlign; // vertical middle fijo

            // Columnas objetivo (respeta onlySelected)
            var cols = TargetColumns(onlySelected);
            if (onlySelected && cols.Length == 0 && _dgv.CurrentCell != null)
                cols = new[] { _dgv.CurrentCell.OwningColumn };

            if (onlySelected)
            {
                foreach (var c in cols)
                {
                    c.DefaultCellStyle.Alignment = cellAlign;
                    if (includeHeader)
                        c.HeaderCell.Style.Alignment = headerAlign;
                }
            }
            else
            {
                // Default para nuevas columnas + aplicar a todas las existentes
                _dgv.DefaultCellStyle.Alignment = cellAlign;

                foreach (DataGridViewColumn c in _dgv.Columns)
                {
                    c.DefaultCellStyle.Alignment = cellAlign;
                    if (includeHeader)
                        c.HeaderCell.Style.Alignment = headerAlign;
                }

                if (includeHeader)
                    _dgv.ColumnHeadersDefaultCellStyle.Alignment = headerAlign;
            }

            _dgv.Invalidate();
        }

        // Helpers públicos (nombres nuevos, no reemplazan nada existente):
        public void AlignCellsLeft(bool onlySelected = false, bool includeHeader = false)
            => ApplyAlignment(HAlign.Left, onlySelected, includeHeader);

        public void AlignCellsCenter(bool onlySelected = false, bool includeHeader = false)
            => ApplyAlignment(HAlign.Center, onlySelected, includeHeader);

        public void AlignCellsRight(bool onlySelected = false, bool includeHeader = false)
            => ApplyAlignment(HAlign.Right, onlySelected, includeHeader);



        public void ZoomGrid(DataGridView dgv, float delta)
        {
            if (dgv == null) return;

            // 1. Obtenemos la fuente actual (usamos la de celdas como base)
            var currentFont = dgv.DefaultCellStyle.Font ?? dgv.Font;

            // 2. Calculamos el nuevo tamaño con límites (ej: entre 6 y 30 puntos)
            float newSize = currentFont.Size + delta;
            if (newSize < 6) newSize = 6;
            if (newSize > 30) newSize = 30;

            if (newSize == currentFont.Size) return;

            var newFont = new Font(currentFont.FontFamily, newSize, currentFont.Style);

            // 3. Aplicamos a celdas y headers
            dgv.DefaultCellStyle.Font = newFont;
            dgv.ColumnHeadersDefaultCellStyle.Font = newFont;
            dgv.RowHeadersDefaultCellStyle.Font = newFont;

            // 4. OPCIONAL: "Nudge" automático de la altura de filas para que acompañe el texto
            // Si la letra crece, necesitamos que la fila crezca proporcionalmente (aprox delta * 1.5)
            int rowDelta = (int)Math.Ceiling(delta * 1.5f);
            NudgeHeights(rowDelta, alsoHeader: true, deltaHeaderPx: rowDelta);
        }
    }
}
