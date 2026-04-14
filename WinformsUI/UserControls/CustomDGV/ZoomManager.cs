using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace WinformsUI.UserControls.CustomDGV
{
    /// <summary>
    /// Gestiona el escalado tipográfico (Zoom) de un Formulario y sus controles hijos.
    /// <para>
    /// Mantiene un snapshot de las fuentes originales para aplicar un escalado relativo sin distorsión acumulativa.
    /// Implementa <see cref="IDisposable"/> para liberar referencias a controles y recursos GDI cacheados.
    /// </para>
    /// </summary>
    public sealed class ZoomManager : IDisposable
    {
        #region === Configuration ===

        public sealed class Options
        {
            /// <summary>Puntos a aumentar/disminuir por paso (Default: 1f).</summary>
            public float StepPts { get; set; } = 1f;

            /// <summary>Tamaño mínimo absoluto en puntos (Default: 8f).</summary>
            public float MinPt { get; set; } = 8f;

            /// <summary>Tamaño máximo absoluto en puntos (Default: 24f).</summary>
            public float MaxPt { get; set; } = 24f;

            /// <summary>Predicado para excluir controles específicos del zoom.</summary>
            public Func<Control, bool> IsExcluded { get; set; }

            /// <summary>Callback para notificar el porcentaje de zoom actual (100% base).</summary>
            public Action<int> OnPercentChanged { get; set; }

            /// <summary>Callback para notificar si es posible seguir haciendo Zoom In/Out.</summary>
            public Action<bool, bool> OnCanZoomChanged { get; set; }
        }

        #endregion

        #region === State ===

        private readonly Options _options;
        private Form _form;
        private bool _disposed;

        // Cache de fuentes originales para evitar degradación por recálculo
        private readonly Dictionary<Control, Font> _baseFonts = new Dictionary<Control, Font>();

        // Cache específico para DataGridViews (tienen múltiples fuentes)
        private readonly Dictionary<DataGridView, (Font header, Font cell, Font alt)> _dgvBaseFonts
            = new Dictionary<DataGridView, (Font, Font, Font)>();

        // Variables de cálculo
        private float _anchorPts = 0f;  // Tamaño promedio base
        private float _deltaPts = 0f;   // Desplazamiento actual
        private float _minDelta = 0f;   // Límite inferior relativo
        private float _maxDelta = 0f;   // Límite superior relativo

        #endregion

        #region === Constructor & Public API ===

        public ZoomManager(Options options = null) => _options = options ?? new Options();

        /// <summary>
        /// Obtiene el porcentaje de zoom actual relativo al tamaño base inicial (100%).
        /// </summary>
        public int ZoomPercent
        {
            get
            {
                if (_anchorPts <= 0f) return 100;
                var current = Clamp(_anchorPts + _deltaPts, _options.MinPt, _options.MaxPt);
                return (int)Math.Round((current / _anchorPts) * 100f, MidpointRounding.AwayFromZero);
            }
        }

        /// <summary>
        /// Vincula el manager a un formulario. Debe llamarse en el evento Shown o Load.
        /// </summary>
        public void Attach(Form form)
        {
            _form = form ?? throw new ArgumentNullException(nameof(form));
            TakeSnapshot();
            UpdateUiState();
        }

        /// <summary>
        /// Fuerza un recálculo del snapshot base. Útil si se cambió el tema o la estructura del Form dinámicamente.
        /// </summary>
        public void RefreshSnapshot()
        {
            _baseFonts.Clear();
            _dgvBaseFonts.Clear();
            _anchorPts = 0f;
            _minDelta = _maxDelta = 0f;
            TakeSnapshot();
            UpdateUiState();
        }

        public void ZoomIn() => ApplyZoomDelta(+_options.StepPts);
        public void ZoomOut() => ApplyZoomDelta(-_options.StepPts);

        #endregion

        #region === Core Logic ===

        private void ApplyZoomDelta(float step)
        {
            if (_form == null) return;
            TakeSnapshot(); // Asegurar que tenemos base

            // Calcular nuevo delta respetando límites globales
            float proposed = _deltaPts + step;
            float saturated = Clamp(proposed, _minDelta, _maxDelta);

            // Optimización: Si estamos en el límite, no redibujar ni acumular deuda flotante
            if (Math.Abs(saturated - _deltaPts) < 1e-6f)
            {
                UpdateUiState();
                return;
            }

            _deltaPts = saturated;

            _form.SuspendLayout();
            try
            {
                ApplyToControlsRecursive(_form);

                // Ajuste específico para DataGridViews
                foreach (var grid in EnumerateControls<DataGridView>(_form))
                {
                    ApplyToDgv(grid);
                    grid.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
                    grid.AutoResizeRows(DataGridViewAutoSizeRowsMode.AllCells);
                }
            }
            finally
            {
                _form.ResumeLayout(true);
            }

            UpdateUiState();
        }

        private void TakeSnapshot()
        {
            if (_form == null || _baseFonts.Count > 0) return;

            // 1. Capturar fuentes base
            foreach (var c in EnumerateControls<Control>(_form))
            {
                if (IsExcluded(c)) continue;

                if (c.Font != null && !_baseFonts.ContainsKey(c))
                    _baseFonts[c] = c.Font;

                if (c is DataGridView grid && !_dgvBaseFonts.ContainsKey(grid))
                {
                    _dgvBaseFonts[grid] = (
                        grid.ColumnHeadersDefaultCellStyle?.Font ?? grid.Font,
                        grid.DefaultCellStyle?.Font ?? grid.Font,
                        grid.AlternatingRowsDefaultCellStyle?.Font ?? grid.DefaultCellStyle?.Font ?? grid.Font
                    );
                }
            }

            // 2. Calcular Anchor (Promedio) y Deltas globales
            if (_baseFonts.Count == 0)
            {
                _anchorPts = _form.Font?.SizeInPoints ?? 9f;
                _minDelta = _options.MinPt - _anchorPts;
                _maxDelta = _options.MaxPt - _anchorPts;
            }
            else
            {
                _anchorPts = (float)_baseFonts.Values.Average(f => f.SizeInPoints);

                // Intersección de intervalos: El zoom permitido es el que mantiene TODOS los controles dentro de [Min, Max]
                float minD = float.NegativeInfinity;
                float maxD = float.PositiveInfinity;

                foreach (var size in _baseFonts.Values.Select(f => f.SizeInPoints))
                {
                    minD = Math.Max(minD, _options.MinPt - size);
                    maxD = Math.Min(maxD, _options.MaxPt - size);
                }

                // Fallback de seguridad
                _minDelta = float.IsNegativeInfinity(minD) ? _options.MinPt - _anchorPts : minD;
                _maxDelta = float.IsPositiveInfinity(maxD) ? _options.MaxPt - _anchorPts : maxD;

                if (_minDelta > _maxDelta) _minDelta = _maxDelta = 0f; // Caso imposible
            }

            // Ajustar delta actual si quedó fuera de rango tras snapshot
            _deltaPts = Clamp(_deltaPts, _minDelta, _maxDelta);
        }

        private void ApplyToControlsRecursive(Control parent)
        {
            foreach (Control c in parent.Controls)
            {
                if (_baseFonts.TryGetValue(c, out var baseFont))
                {
                    float newSize = Clamp(baseFont.SizeInPoints + _deltaPts, _options.MinPt, _options.MaxPt);
                    // Solo aplicar si cambió para evitar GDI thrashing
                    if (Math.Abs(c.Font.SizeInPoints - newSize) > 0.01f)
                        c.Font = new Font(baseFont.FontFamily, newSize, baseFont.Style, GraphicsUnit.Point);
                }

                if (c.HasChildren) ApplyToControlsRecursive(c);
            }
        }

        private void ApplyToDgv(DataGridView grid)
        {
            if (!_dgvBaseFonts.TryGetValue(grid, out var tpl)) return;

            Font Scale(Font f) => new Font(f.FontFamily, Clamp(f.SizeInPoints + _deltaPts, _options.MinPt, _options.MaxPt), f.Style, GraphicsUnit.Point);

            grid.ColumnHeadersDefaultCellStyle.Font = Scale(tpl.header);
            grid.DefaultCellStyle.Font = Scale(tpl.cell);
            grid.AlternatingRowsDefaultCellStyle.Font = Scale(tpl.alt);
        }

        #endregion

        #region === Helpers ===

        private bool IsExcluded(Control c)
        {
            if (_options.IsExcluded?.Invoke(c) ?? false) return true;

            // Exclusiones por defecto de infraestructura UI
            return c is MenuStrip || c is StatusStrip || c is ToolStripContainer ||
                   c.Parent is ToolStrip || c.Parent is MenuStrip || c.Parent is StatusStrip ||
                   Equals(c.Tag, "NoZoom");
        }

        private void UpdateUiState()
        {
            _options.OnPercentChanged?.Invoke(ZoomPercent);

            bool canIn = (_deltaPts + _options.StepPts) <= _maxDelta + 1e-6f;
            bool canOut = (_deltaPts - _options.StepPts) >= _minDelta - 1e-6f;

            _options.OnCanZoomChanged?.Invoke(canIn, canOut);
        }

        private static float Clamp(float val, float min, float max) => val < min ? min : (val > max ? max : val);

        private static IEnumerable<T> EnumerateControls<T>(Control root) where T : Control
        {
            foreach (Control c in root.Controls)
            {
                if (c is T t) yield return t;
                foreach (var child in EnumerateControls<T>(c)) yield return child;
            }
        }

        #endregion

        #region === IDisposable ===

        public void Dispose()
        {
            if (_disposed) return;

            // Limpieza de referencias para ayudar al GC y romper referencias circulares
            _baseFonts.Clear();
            _dgvBaseFonts.Clear();
            _form = null;

            // Desvincular delegados externos
            _options.OnPercentChanged = null;
            _options.OnCanZoomChanged = null;
            _options.IsExcluded = null;

            _disposed = true;
        }

        #endregion
    }
}