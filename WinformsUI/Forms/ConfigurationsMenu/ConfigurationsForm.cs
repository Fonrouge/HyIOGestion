using BLL.LogicLayers;
using Presenter.Messaging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using Winforms.Theme;
using WinformsUI.Infrastructure.Culture;
using WinformsUI.Infrastructure.Localization;
using WinformsUI.Infrastructure.Translations;
using static Winforms.Theme.DarkTheme;

namespace WinformsUI.Forms.ConfigurationsMenu
{
    public partial class ConfigurationsForm : Form
    {
        private readonly ITranslatableControlsManager _transMgr;
        private readonly ICultureSwitcher _cultureSwitcher;
        private readonly IMessenger _messenger;

        private bool _syncing;
        private DarkTheme.Palette? _pendingPalette;
        private string _pendingPaletteName;

        public ConfigurationsForm
        (
            ITranslatableControlsManager transMgr,
            ICultureSwitcher cultureSwitcher,
            IMessenger messenger
        )
        {

            _transMgr = transMgr;
            _cultureSwitcher = cultureSwitcher;
            _messenger = messenger;

            InitializeComponent();
            TehmingPanel();
            LanguagePanel();
            ApplyGlobalTheme();




            this.FormClosed += (s, e) =>
            {
                _transMgr.RemoveFormNotify(this);
                lbDarkPalettes.SelectedIndexChanged -= LbDarkPalettes_SelectedIndexChanged;
                lbLightPalettes.SelectedIndexChanged -= LbLightPalettes_SelectedIndexChanged;
            };



            _messenger.Subscribe<TranslationRequestMessage>(SelectedIndexChanged);
        }
        private void ApplyGlobalTheme() => DarkTheme.Apply(this, DarkTheme.GetCurrentPalette());


        //========================================================
        //                       THEMING
        //========================================================
        #region Theming
        private void TehmingPanel()
        {
            InitializeThemingLists();
            WireThemingEvents();
         
            InitializeListBoxes();
        }

        private void InitializeThemingLists()
        {
            // Lista Oscura: Agregadas Volcanic, Nordic, Cyber y DeepContrast
            var srcDark = new BindingSource(DarkTheme.GetAllPalettes().Where(kv => new[]
            {
                "Graphite",
                "Oceanic",
                "Forest",
                "Aubergine",
                "Volcanic",
                "Nordic",
                "Cyber",
                "DeepContrast",
                "EyeRestDark",
                "Obsidian"

            }.Contains(kv.Key)).ToList(), null);

            // Lista Clara: Agregadas Solar y Berry
            var srcLight = new BindingSource(DarkTheme.GetAllPalettes().Where(kv => new[]
            {
                "Classic",
                "Paper",
                "Solar",
                "Mint",
                "Berry",
                "Grape",
                "HighContrast",
                "EyeRestLight"

            }.Contains(kv.Key)).ToList(), null);

            FillListBoxes(srcDark, srcLight);
        }
        private void FillListBoxes(BindingSource darkPalettes, BindingSource lightPalettes)
        {
            lbDarkPalettes.DataSource = darkPalettes;
            lbDarkPalettes.DisplayMember = "Key";
            lbDarkPalettes.ValueMember = "Value";

            lbLightPalettes.DataSource = lightPalettes;
            lbLightPalettes.DisplayMember = "Key";
            lbLightPalettes.ValueMember = "Value";
        }

        private void WireThemingEvents()
        {
            lbDarkPalettes.SelectedIndexChanged += LbDarkPalettes_SelectedIndexChanged;
            lbLightPalettes.SelectedIndexChanged += LbLightPalettes_SelectedIndexChanged;

            btnApply.Click += ClickThemingApply;
            btnTry.Click += ClickThemingTry;
        }

        private void InitializeListBoxes()
        {
            lbLightPalettes.ClearSelected();
            lbDarkPalettes.ClearSelected();
        }


        private void ClickThemingApply(object sender, EventArgs e)
        {
            if (_pendingPalette.HasValue)
            {
                DarkTheme.SetGlobalPalette(_pendingPalette.Value);
                ApplyToOpenForms(true);
            }
        }

        private void ClickThemingTry(object sender, EventArgs e)
        {
            if (_pendingPalette.HasValue)
            {
                ApplyInternal(this, _pendingPalette.Value, VisualDepth.ThreeD);
            }
        }

        private static string ResolvePaletteName(DarkTheme.Palette pal)
        {
            var kv = DarkTheme.GetAllPalettes().FirstOrDefault(p => p.Value.Equals(pal));
            return kv.Equals(default(KeyValuePair<string, DarkTheme.Palette>)) ? null : kv.Key;
        }


        private void ApplyToOpenForms(bool forceOverride)
        {
            

            var forms = Application.OpenForms.Cast<Form>().ToList();

            foreach (var form in forms)
            {
                var type = form.GetType();

                var mi = type.GetMethod("ThemingNotifiedByConfigurationsModule",
                                        BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

                if (mi != null)
                {
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

        private void LbDarkPalettes_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_syncing) return;
            if (lbDarkPalettes.SelectedIndex < 0) { _pendingPalette = null; return; }

            _syncing = true;

            try
            {
                lbLightPalettes.ClearSelected();

                var val = lbDarkPalettes.SelectedValue;

                if (val is DarkTheme.Palette pal)
                {
                    _pendingPalette = pal;
                    _pendingPaletteName = ResolvePaletteName(pal);
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

                var val = lbLightPalettes.SelectedValue;
                if (val is DarkTheme.Palette pal)
                {
                    _pendingPalette = pal;
                    _pendingPaletteName = ResolvePaletteName(pal);
                }
            }
            finally { _syncing = false; }
        }

        #endregion


        //========================================================
        //                      LANGUAGE
        //========================================================
        #region Language
        private void LanguagePanel()
        {
            List<LanguageInfo> langCodes = _transMgr.GetAvailableLanguages();
           

            cbLang.DataSource = langCodes;
            cbLang.DisplayMember = "DisplayName";
            cbLang.ValueMember = "LangCode";

            cbLang.SelectedIndexChanged += (sender, e) => SelectedIndexChanged();
            AddTranslatables();
        }


        private void AddTranslatables()  //Se desuscribe en la clase padre BaseManagementForm FormClosed() => _transMgr.RemoveFormNotify(this);
        {
            _transMgr.AddParentedObjects<Label>(this.Controls, "Text");
            _transMgr.AddParentedObjects<Button>(this.Controls, "Text");

            _transMgr.AddFormNotify(this);

            ApplyTranslation();
        }

        //Notified by reflection on TranslatorManager class. That's why it has 0 references.
        public void ApplyTranslation() => _transMgr.Apply();
        public void NotifiedByTranslationManager() => ApplyTranslation();


        private void SelectedIndexChanged(TranslationRequestMessage message = null)
        {
            if (message == null)
                _cultureSwitcher.SetUICulture(cbLang.SelectedValue.ToString());

            else
                _cultureSwitcher.SetUICulture(message.Payload);

            _transMgr.Apply();
            _transMgr.Notify();
        }
        #endregion





    }
}
