using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Winforms.Theme;
using WinformsUI.Infrastructure.Culture;
using WinformsUI.Infrastructure.Localization;
using WinformsUI.Infrastructure.Translations;
using static Winforms.Theme.DarkTheme;

namespace WinformsUI.Features.ConfigurationsMenu
{
    public partial class ConfigurationsForm : Form
    {
        private readonly ITranslatableControlsManager _transMgr;
        private readonly ICultureSwitcher _cultureSwitcher;
        private readonly ILocalizationService _localServ;

        public ConfigurationsForm
        (
            ITranslatableControlsManager transMgr,
            ICultureSwitcher cultureSwitcher,
            ILocalizationService localServ
        )
        {

            _transMgr = transMgr;
            _cultureSwitcher = cultureSwitcher;
            _localServ = localServ;


            InitializeComponent();

            List<string> langCodes = new List<string>() {"es", "en" };            
            cbLang.DataSource = langCodes;


            cbLang.SelectedIndexChanged += (sender, e) => SelectedIndesChanged();
            AddTranslatables();
            InitializeLists();
            WirePalettesSelectionEvents();
            ApplyTheme();
            lbLightPalettes.ClearSelected();
            lbDarkPalettes.ClearSelected();


            this.FormClosed += (s, e) =>
            {
                _transMgr.RemoveFormNotify(this);
            };

            btnApply.Click += (s, e) =>
            {
                if (_pendingPalette.HasValue)
                {
                    DarkTheme.SetGlobalPalette(_pendingPalette.Value);
                    ApplyToOpenForms(true);
                }
            };
            btnTry.Click += (s, e) =>
            {
                if (_pendingPalette.HasValue)
                {
                    ApplyInternal(this, _pendingPalette.Value, VisualDepth.ThreeD);
                }
            };

        }

        private void AddTranslatables()  //Se desuscribe en la clase padre BaseManagementForm FormClosed() => _transMgr.RemoveFormNotify(this);
        {
            _transMgr.AddParentedObjects<Label>(this.Controls, "Text");
            _transMgr.AddParentedObjects<Button>(this.Controls, "Text");

            _transMgr.AddFormNotify(this);

            ApplyTranslation();
        }

        //Notified by reflection on TranslatorManager class. That's why it has 0 references.
        public void NotifiedByTranslationManager() => ApplyTranslation();
        public void ApplyTranslation() => _transMgr.Apply();


        private void SelectedIndesChanged()
        {
            _cultureSwitcher.SetUICulture(cbLang.SelectedValue.ToString());
            _transMgr.Apply();
            _transMgr.Notify();
        }

        private void InitializeLists()
        {
            // Lista Oscura: Agregadas Volcanic, Nordic, Cyber y DeepContrast
            var srcDark = new BindingSource(DarkTheme.GetAllPalettes().Where(kv => new[]
            {
                "Graphite",
                "Oceanic",
                "Forest",
                "Aubergine",
                "Volcanic",      // <--- Nuevo (Rojo/Alerta)
                "Nordic",        // <--- Nuevo (Dev/Frio)
                "Cyber",         // <--- Nuevo (Neon)
                "DeepContrast",  // <--- Nuevo (OLED)
                "EyeRestDark",
                "Obsidian"

            }.Contains(kv.Key)).ToList(), null);

            // Lista Clara: Agregadas Solar y Berry
            var srcLight = new BindingSource(DarkTheme.GetAllPalettes().Where(kv => new[]
            {
                "Classic",
                "Paper",
                "Solar",         // <--- Nuevo (Cálido/Crema)
                "Mint",
                "Berry",         // <--- Nuevo (Rosa/Vino)
                "Grape",
                "HighContrast",
                "EyeRestLight"
            }.Contains(kv.Key)).ToList(), null);

            FillListBoxes(srcDark, srcLight);
        }
        private void FillListBoxes(BindingSource darkPalettes, BindingSource lightPalettes)
        {
            lbDarkPalettes.DataSource = darkPalettes;
            lbDarkPalettes.DisplayMember = "Key";   // nombre visible (Graphite, Oceanic, ...)
            lbDarkPalettes.ValueMember = "Value"; // la Palette

            lbLightPalettes.DataSource = lightPalettes;
            lbLightPalettes.DisplayMember = "Key";   // nombre visible (Graphite, Oceanic, ...)
            lbLightPalettes.ValueMember = "Value"; // la Palette
        }


        private void ApplyTheme() => DarkTheme.Apply(this, DarkTheme.GetCurrentPalette());   //_thmManager.ApplyTheme(this);
        private void WirePalettesSelectionEvents()
        {
            lbDarkPalettes.SelectedIndexChanged += LbDarkPalettes_SelectedIndexChanged;
            lbLightPalettes.SelectedIndexChanged += LbLightPalettes_SelectedIndexChanged;
        }


        private bool _syncing;

        private DarkTheme.Palette? _pendingPalette;   // la paleta elegida (pendiente de aplicar)
        private string _pendingPaletteName;           // nombre (Key)



        private void LbDarkPalettes_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_syncing) return;
            if (lbDarkPalettes.SelectedIndex < 0) { _pendingPalette = null; return; }

            _syncing = true;
            try
            {
                lbLightPalettes.ClearSelected();

                var val = lbDarkPalettes.SelectedValue;        // <-- NO usar SelectedItem
                if (val is DarkTheme.Palette pal)
                {
                    _pendingPalette = pal;
                    _pendingPaletteName = ResolvePaletteName(pal);   // no depende del texto traducido
                }
            }
            finally { _syncing = false; }
        }

        private void LbLightPalettes_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_syncing) return;
            if (lbLightPalettes.SelectedIndex < 0) { _pendingPalette = null; return; }

            _syncing = true;
            try
            {
                lbDarkPalettes.SelectedIndex = -1;

                var val = lbLightPalettes.SelectedValue;       // <-- NO usar SelectedItem
                if (val is DarkTheme.Palette pal)
                {
                    _pendingPalette = pal;
                    _pendingPaletteName = ResolvePaletteName(pal);
                }
            }
            finally { _syncing = false; }
        }
        // Busca el nombre de la paleta a partir del struct Palette
        private static string ResolvePaletteName(DarkTheme.Palette pal)
        {
            // Igualdad de struct por valor: matchea por campos
            var kv = DarkTheme.GetAllPalettes().FirstOrDefault(p => p.Value.Equals(pal));
            return kv.Equals(default(KeyValuePair<string, DarkTheme.Palette>)) ? null : kv.Key;
        }

        private void ApplyToOpenForms(bool forceOverride)
        {
            // Usamos .ToList() para crear una copia segura de la colección y evitar excepciones
            // si la cantidad de formularios abiertos cambia durante la iteración.
            var forms = Application.OpenForms.Cast<Form>().ToList();

            foreach (var form in forms)
            {
                var type = form.GetType();

                // Buscamos el método. No hace falta pasar binder, types y modifiers si no hay sobrecargas complejas.
                var mi = type.GetMethod("ApplyGlobalPalette",
                                        BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

                if (mi != null)
                {
                    // Si el método existe, lo ejecutamos sobre la instancia actual del formulario
                    mi.Invoke(form, null);
                }
            }
        }
        private void ApplyInternal(Control root, Palette p, VisualDepth d)
        {
            if (root == null || root.IsDisposed) return;

            if (root.InvokeRequired)
            {
                try
                {
                    root.Invoke(new Action(() => DarkTheme.Apply(root, p, d)));
                }
                catch (ObjectDisposedException)
                { }
            }
            else
            {
                DarkTheme.Apply(root, p, d);
            }
        }


    }
}
