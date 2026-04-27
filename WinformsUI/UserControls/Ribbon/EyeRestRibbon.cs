using System;
using System.Windows.Forms;
using static Winforms.Theme.DarkTheme;

namespace WinformsUI.UserControls.Ribbon
{
    public partial class EyeRestRibbon : UserControl
    {
        public event EventHandler<Palette> DarkPaletteAsked;
        public event EventHandler<Palette> LightPaletteAsked;
        public event EventHandler<Palette> GlobalPaletteAsked;

        public EyeRestRibbon()
        {
            InitializeComponent();

            btnDarkRestMode.Click += ApplyDarkPalette;
            btnLightRestMode.Click += ApplyLightPalette;
            btnCurrentMode.Click += ApplyCurrentPalette;
        }
        
        private void ApplyDarkPalette(object sender, EventArgs e) => DarkPaletteAsked?.Invoke(this, PalettesDark.EyeRest());
        private void ApplyLightPalette(object sender, EventArgs e) => DarkPaletteAsked?.Invoke(this, PalettesLight.EyeRest());
        private void ApplyCurrentPalette(object sender, EventArgs e) => DarkPaletteAsked?.Invoke(this, GetCurrentPalette());


    }
}
