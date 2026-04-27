using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;

namespace WinformsUI.UserControls.CustomDGV
{
    public partial class CustomDgvRibbon : UserControl, IDisposable
    {
        private CustomDGVForm _targetDGV;

        // Handlers guardados para poder desuscribir correctamente
        private EventHandler _tallerRowHandler;
        private EventHandler _shorterRowHandler;
        private EventHandler _longerColumnHandler;
        private EventHandler _shorterColumnHandler;
        private EventHandler _autoRowHandler;
        private EventHandler _autoColumnHandler;
        private EventHandler _alignLeftHandler;
        private EventHandler _alignCenterHandler;
        private EventHandler _alignRightHandler;

        // NUEVOS handlers
        private EventHandler _allBordersHandler;
        private EventHandler _verticalBordersHandler;
        private EventHandler _noBordersHandler;
        private EventHandler _copyAsTextHandler;
        private EventHandler _copyAsCellHandler;

        public CustomDGVForm TargetDGV
        {
            get => _targetDGV;
            set
            {
                if (_targetDGV == value) return;

                UnwireAllButtons();

                _targetDGV = value;

                if (_targetDGV != null)
                    WireAllButtons();
            }
        }

        public CustomDgvRibbon()
        {
            SuspendLayout();
            InitializeComponent();
            ResumeLayout();
        }

        private void WireAllButtons()
        {
            // Celdas (ya existentes)
            _tallerRowHandler = (s, e) => _targetDGV.NudgeRowHeight(4);
            _shorterRowHandler = (s, e) => _targetDGV.NudgeRowHeight(-4);
            _longerColumnHandler = (s, e) => _targetDGV.NudgeColumnWidth(16);
            _shorterColumnHandler = (s, e) => _targetDGV.NudgeColumnWidth(-16);
            _autoRowHandler = (s, e) => _targetDGV.AutoFitRowAndHeader();
            _autoColumnHandler = (s, e) => _targetDGV.AutoFitColumns();

            btnTallerRow.Click += _tallerRowHandler;
            btnShorterRow.Click += _shorterRowHandler;
            btnLongerColumn.Click += _longerColumnHandler;
            btnShorterColumn.Click += _shorterColumnHandler;
            btnAutoRow.Click += _autoRowHandler;
            btnAutoColumn.Click += _autoColumnHandler;

            // Texto (ya existentes)
            _alignLeftHandler = (s, e) => _targetDGV.AlignCellsLeft();
            _alignCenterHandler = (s, e) => _targetDGV.AlignCellsCenter();
            _alignRightHandler = (s, e) => _targetDGV.AlignCellsRight();

            btnAlignLeft.Click += _alignLeftHandler;
            btnAlignCenter.Click += _alignCenterHandler;
            btnAlignRight.Click += _alignRightHandler;

            // === NUEVOS: Bordes ===
            _allBordersHandler = (s, e) => _targetDGV.SetAllBorders();
            _verticalBordersHandler = (s, e) => _targetDGV.SetVerticalBordersOnly();
            _noBordersHandler = (s, e) => _targetDGV.SetNoBorders();

            btnAllBorders.Click += _allBordersHandler;
            btnJustVerticalBorders.Click += _verticalBordersHandler;
            btnNoBorders.Click += _noBordersHandler;

            // === NUEVOS: Copiar ===
            _copyAsTextHandler = (s, e) => _targetDGV.CopyAsText();
            _copyAsCellHandler = (s, e) => _targetDGV.CopyAsCell();

            btnCopyAsText.Click += _copyAsTextHandler;
            btnCopyAsCell.Click += _copyAsCellHandler;
        }

        private void UnwireAllButtons()
        {
            if (_targetDGV == null) return;

            // Celdas + Texto (ya existentes)
            btnTallerRow.Click -= _tallerRowHandler;
            btnShorterRow.Click -= _shorterRowHandler;
            btnLongerColumn.Click -= _longerColumnHandler;
            btnShorterColumn.Click -= _shorterColumnHandler;
            btnAutoRow.Click -= _autoRowHandler;
            btnAutoColumn.Click -= _autoColumnHandler;

            btnAlignLeft.Click -= _alignLeftHandler;
            btnAlignCenter.Click -= _alignCenterHandler;
            btnAlignRight.Click -= _alignRightHandler;

            // Bordes
            btnAllBorders.Click -= _allBordersHandler;
            btnJustVerticalBorders.Click -= _verticalBordersHandler;
            btnNoBorders.Click -= _noBordersHandler;

            // Copiar
            btnCopyAsText.Click -= _copyAsTextHandler;
            btnCopyAsCell.Click -= _copyAsCellHandler;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                UnwireAllButtons();
                _targetDGV = null;
            }
            base.Dispose(disposing);
        }

    
    }
}