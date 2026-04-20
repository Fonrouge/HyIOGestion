using SharedAbstractions.ArchitecturalMarkers;
using System;
using System.Windows.Forms;
using Winforms.Theme;
using WinformsUI.Forms.Base;

namespace WinformsUI.UserControls.Ribbon
{
    public partial class EyeRestRibbon : UserControl
    {
        public Form TargetForm { get; set; } 

        public EyeRestRibbon()
        {
            InitializeComponent();

            btnDarkRestMode.Click += ApplyDarkPalette;
            btnLightRestMode.Click += ApplyLightPalette;
            btnCurrentMode.Click += ApplyCurrentPalette;
        }
        private void ApplyDarkPalette(object sender, EventArgs e)
        {
            SuspendLayout();
            DarkTheme.RedrawBorders = true;
            DarkTheme.Apply(TargetForm, DarkTheme.PalettesDark.EyeRest());
          //  TargetForm.ThemingNotifiedByConfigurationsModule();
            ResumeLayout();
        }
        private void ApplyLightPalette(object sender, EventArgs e)
        {
            SuspendLayout();
            DarkTheme.RedrawBorders = true;
            DarkTheme.Apply(TargetForm, DarkTheme.PalettesLight.EyeRest());
            ResumeLayout();
        }
        private void ApplyCurrentPalette(object sender, EventArgs e)
        {
            SuspendLayout();
            DarkTheme.RedrawBorders = true;
            DarkTheme.Apply(TargetForm, DarkTheme.GetCurrentPalette());
            ResumeLayout();
        }

    }
}
