using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using WinformsUI.Infrastructure.UserInterface.Theming;

namespace Winforms.Theme
{
    /// Motor de theming para WinForms + recoloreo de iconografía en runtime (duotone).
    public static class DarkTheme
    {

        //Lìnea 461 puedo determinar color de las ToolStrip
        //Línea 987 color Header DataGridView

        // ============================
        // 0) PROFUNDIDAD / MODO
        // ============================
        public enum VisualDepth { Plain, ThreeD }

        // ============================
        // 1) PALETAS (con Low/HighAccent)
        // ============================
        public static IReadOnlyDictionary<string, Palette> GetAllPalettes()
        {
            return new Dictionary<string, Palette>
            {
                // ============================
                // CLARAS
                // ============================
                ["Classic"] = PalettesLight.Classic(),
                ["Paper"] = PalettesLight.Paper(),
                ["Mint"] = PalettesLight.Mint(),
                ["Grape"] = PalettesLight.Grape(),
                ["Solar"] = PalettesLight.Solar(),
                ["Berry"] = PalettesLight.Berry(),
                // Especiales (Accesibilidad + Descanso ocular)
                ["HighContrast"] = PalettesLight.HighContrast(),
                ["EyeRestLight"] = PalettesLight.EyeRest(),


                // ============================
                // OSCURAS
                // ============================
                ["Graphite"] = PalettesDark.Graphite(),
                ["Oceanic"] = PalettesDark.Oceanic(),
                ["Forest"] = PalettesDark.Forest(),
                ["Aubergine"] = PalettesDark.Aubergine(),
                ["Obsidian"] = PalettesDark.Obsidian(),
                ["Volcanic"] = PalettesDark.Volcanic(),
                ["Nordic"] = PalettesDark.Nordic(),
                ["Cyber"] = PalettesDark.Cyber(),
                // Especiales (Accesibilidad + Descanso ocular)
                ["DeepContrast"] = PalettesDark.DeepHighContrast(),
                ["EyeRestDark"] = PalettesDark.EyeRest()
            };
        }

        public struct Palette
        {
            public string Name;
            public readonly Color Background, SurfaceAlt, Surface, Border;
            public readonly Color TextPrimary, TextSecondary;
            public Color Accent, AccentHover, LowAccent, HighAccent, GridLine;

            public Palette
            (
                string name, string bg, string sf, string sfAlt, string brd,
                string txt, string txt2, string acc, string accHover, string grid,
                string accDark = null,
                string accLight = null
            )
            {
                Name = name;
                Background = ColorTranslator.FromHtml(bg);
                Surface = ColorTranslator.FromHtml(sf);
                SurfaceAlt = ColorTranslator.FromHtml(sfAlt);
                Border = ColorTranslator.FromHtml(brd);
                TextPrimary = ColorTranslator.FromHtml(txt);
                TextSecondary = ColorTranslator.FromHtml(txt2);

                Accent = ColorTranslator.FromHtml(acc);
                AccentHover = ColorTranslator.FromHtml(accHover);

                GridLine = ColorTranslator.FromHtml(grid);

                LowAccent = accDark != null ? ColorTranslator.FromHtml(accDark) : Darken(Accent, 0.18);
                HighAccent = accLight != null ? ColorTranslator.FromHtml(accLight) : Darken(Accent, -0.70);
            }
        }

        public static class PalettesDark
        {
            public static Palette Graphite() => new Palette("Graphite", "#16191F", "#171A21", "#1F2430", "#2B3240", "#E6EAF0", "#9AA4B2", "#6EA8FE", "#8DBBFF", "#2B3240", "#3B82F6");
            public static Palette Oceanic() => new Palette("Oceanic", "#161C26", "#161B22", "#1E2630", "#2A3441", "#E3E8EF", "#97A3B6", "#3DA1FF", "#69B6FF", "#2A3441", "#1E7EEB");
            public static Palette Forest() => new Palette("Forest", "#0E1210", "#1F2620", "#2E3831", "#293128", "#E7EFE8", "#9DB3A4", "#44D18D", "#63E2A4", "#293128", "#1FA971");
            public static Palette Aubergine() => new Palette("Aubergine", "#18151F", "#0A060D", "#1F1B2B", "#302A40", "#EEE9F8", "#B6A9D0", "#C07DFF", "#D29EFF", "#302A40", "#9B5AE6");
            public static Palette EyeRest() => new Palette("DarkEyeRest", "#282420", "#3A3531", "#312D2A", "#4A443E", "#D8CFC3", "#8A7F72", "#D9A265", "#EAC189", "#4A443E");
            public static Palette Volcanic() => new Palette("Volcanic", "#120B0B", "#1F1616", "#2B1E1E", "#402E2E", "#F0E6E6", "#A89292", "#FF5F57", "#FF7B75", "#402E2E", "#D13830");
            public static Palette Nordic() => new Palette("Nordic", "#2E3440", "#3B4252", "#434C5E", "#4C566A", "#ECEFF4", "#D8DEE9", "#88C0D0", "#81A1C1", "#4C566A", "#5E81AC");
            public static Palette Obsidian() => new Palette("Obsidian", "#09090B", "#18181B", "#27272A", "#3F3F46", "#FAFAFA", "#A1A1AA", "#6366F1", "#818CF8", "#27272A", "#4F46E5");
            public static Palette Amber() => new Palette("Amber", "#14110F", "#1F1A16", "#2E2621", "#453A32", "#FFF8E1", "#BCAAA4", "#FFC107", "#FFD54F", "#453A32", "#FF8F00");
            public static Palette Cyber() => new Palette("Cyber", "#0F0B14", "#181221", "#21192E", "#362947", "#EAE0F5", "#9D8BA3", "#F72585", "#B5179E", "#362947", "#7209B7");
            public static Palette DeepHighContrast() => new Palette("DarkContrast", "#000000", "#000000", "#121212", "#333333", "#FFFFFF", "#B3B3B3", "#00E5FF", "#69F0AE", "#333333", "#00B8D4");
        }

        public static class PalettesLight
        {
            public static Palette Classic() => new Palette("Classic", "#F5F7FA", "#FFFFFF", "#F0F2F5", "#D8DEE7", "#0F172A", "#475569", "#2563EB", "#1D4ED8", "#E5E7EB", "#1E40AF");
            public static Palette Paper() => new Palette("Paper", "#FAFAF7", "#FFFFFF", "#F8F7F4", "#E8E5DA", "#2B2B2B", "#6B7280", "#0EA5E9", "#0284C7", "#EAEAEA", "#0369A1");
            public static Palette Mint() => new Palette("Mint", "#F6FBF8", "#FFFFFF", "#F0FAF4", "#DAE7DF", "#111827", "#64748B", "#10B981", "#059669", "#E5E7EB", "#047857");
            public static Palette Grape() => new Palette("Grape", "#F8F7FB", "#FFFFFF", "#F3F1F9", "#E2E0EA", "#1F2937", "#6B7280", "#8B5CF6", "#7C3AED", "#E5E7EB", "#5B21B6");
            public static Palette HighContrast() => new Palette("LightContrast", "#FFFFFF", "#FFFFFF", "#F2F2F2", "#A3A3A3", "#000000", "#333333", "#0A84FF", "#0066CC", "#D9D9D9", "#0052A3");
            public static Palette Citrine() => new Palette("Citrine", "#FEFCE8", "#FFFFFF", "#FFFBEB", "#EDE9D5", "#422006", "#854D0E", "#D97706", "#B45309", "#E5E7EB", "#92400E");
            public static Palette EyeRest() => new Palette("LightEyeRest", "#F5F0E8", "#F9F7F3", "#FFFFFF", "#DCD5CB", "#4A443E", "#9E9489", "#8A9A5B", "#A0B26B", "#DCD5CB");
            public static Palette Berry() => new Palette("Berry", "#FFF5F7", "#FFFFFF", "#FFF0F3", "#F3D9E2", "#4A1522", "#8A4D5D", "#D6336C", "#C2255C", "#F3D9E2", "#A61E4D");
            public static Palette Solar() => new Palette("Solar", "#FDF6E3", "#EEE8D5", "#EEE8D5", "#D3CDBC", "#657B83", "#93A1A1", "#B58900", "#CB4B16", "#D3CDBC", "#859900");
        }

        // ============================
        // 2) ESTADO (PALETA ACTUAL)
        // ============================
        private sealed class _PaletteHolder { public Palette Value; public VisualDepth Depth; }
        private static readonly ConditionalWeakTable<Form, _PaletteHolder> _palettesByForm =
            new ConditionalWeakTable<Form, _PaletteHolder>();
        private static Palette _globalCurrent { get; set; } = PalettesLight.Classic();
        private static VisualDepth _globalDepth = VisualDepth.ThreeD;

        public static Palette GetCurrentPalette(Form form = null)
            => (form != null && _palettesByForm.TryGetValue(form, out var h)) ? h.Value : _globalCurrent;
        public static VisualDepth GetCurrentDepth(Form form = null)
            => (form != null && _palettesByForm.TryGetValue(form, out var h)) ? h.Depth : _globalDepth;
        public static Palette GetPaletteFor(Control c) => GetCurrentPalette(c == null ? null : c.FindForm());
        public static bool RedrawBorders = false;

        // ============================
        // 3) API PRINCIPAL
        // ============================

        public static void SetGlobalPalette(Palette p) => _globalCurrent = p;

        public static void Apply(Form form, Palette p, VisualDepth depth = VisualDepth.ThreeD)
        {
            form.SuspendLayout(); //Evitar flickering hasta el final del ciclo. Suspensión de dibujado

            if (form == null) return; //Fail fast sin exception

            var holder = _palettesByForm.GetOrCreateValue(form);
            bool paletteChanged = !holder.Value.Equals(p);
            holder.Value = p; holder.Depth = depth;

            if (paletteChanged) ClearIconCache();

            if (!(form.Tag is "NonPaintable"))
            {
                form.BackColor = Darken(p.LowAccent, 0.6);
            }

            PaintControlTree(form, p);
            StyleToolStrips(form, p, depth);

            foreach (var ts in FindAll<ToolStrip>(form))
            {
                foreach (ToolStripItem it in ts.Items)
                    if (it is ToolStripDropDownItem ddi && ddi.DropDown is ToolStripDropDownMenu dd)
                        StyleDropDownMenu(dd, p, depth);
            }

            if (depth == VisualDepth.ThreeD) StylePanels(form, p, depth); //Cambio en línea 916 para usar 'p' y no CurrentPalette

            foreach (var dgv in FindAll<DataGridView>(form))
                DataGridViewThemer.StyleDgv(dgv, p, depth);

            form.ResumeLayout();//Reanudación de dibujado de UI.



            RedrawBorders = false; //Al ser una clase estática "singleton" de facto, es necesario resetear el flag para que cada formulario lo pueda usar libremente
                                   //sin depender de si otro formulario lo usó antes o no. Se asume que el uso típico será:
                                   //1) Seteo el flag, 2) Aplico el tema a un formulario, 3) El flag se resetea automáticamente para que no afecte a otros formularios.
        }



        public static void Apply(Control root, Palette p, VisualDepth depth = VisualDepth.ThreeD)
        {
            //  var form = root as Form ?? root.FindForm();
            //  if (form != null) { Apply(form, p, depth); return; }

            ClearIconCache();
            PaintControlTree(root, p);

            foreach (var ts in FindAll<ToolStrip>(root)) SetupToolStrip(ts, p, depth);
            foreach (var ss in FindAll<StatusStrip>(root)) SetupStatusStrip(ss, p, depth);

            if (depth == VisualDepth.ThreeD)
            {
                foreach (var pnl in FindAll<Panel>(root))
                {
                    ApplyGradientBackground(pnl, p.Surface, p.SurfaceAlt, LinearGradientMode.Vertical, false);
                }
            }

            foreach (var dgv in FindAll<DataGridView>(root)) DataGridViewThemer.StyleDgv(dgv, p, depth);




            RedrawBorders = false; //Al ser una clase estática "singleton" de facto, es necesario resetear el flag para que cada formulario lo pueda usar libremente
                                   //sin depender de si otro formulario lo usó antes o no. Se asume que el uso típico será:
                                   //1) Seteo el flag, 2) Aplico el tema a un formulario, 3) El flag se resetea automáticamente para que no afecte a otros formularios.
        }

        private static void OnFormDisposed(object sender, EventArgs e)
        {
            var f = sender as Form;
            if (f != null) _palettesByForm.Remove(f);
        }

        // ============================
        // 4) PINTURA DEL ÁRBOL
        // ============================
        public static void PaintControlTree(Control root, Palette p)
        {
            foreach (Control c in root.Controls)
            {

                //Los controles NonPaintable tienen un tratado "especial". Pese a que no se deseé que se pinten, pueden alterárseles ciertos parámetros por conveniencia
                //(por ejemplo, un botón no se pinta para ser transparentepero pero su texto sí y Hover sí lo hacen).
                if (IsNonPaintable(c.Tag))
                {
                    if (c is Panel || c is TableLayoutPanel)
                    {
                        HasChildren(c, p);
                        continue; //No se rompe el bucle con Break para que los hijos continúen interando.
                    }
                }

                // Estilos base por tipo
                if (c is StatusStrip || c is ToolStrip)
                {
                    c.ForeColor = p.TextPrimary; // Fondo lo maneja el renderer. Especial para todo tipo de "Strips".
                }

                else if (c is CheckedListBox clb)
                {
                    clb.BorderStyle = BorderStyle.None;
                    clb.BackColor = p.SurfaceAlt;
                    clb.ForeColor = p.TextSecondary;
                }

                else if (c is CheckBox chb)
                {
                    chb.BackColor = Color.Transparent;
                    chb.ForeColor = p.TextSecondary;
                    continue;
                }

                else if (c is RadioButton rb)
                {
                    rb.BackColor = Color.Transparent;
                    rb.ForeColor = p.TextSecondary;
                    continue;
                }

                else if (c is GroupBox || c is Panel)
                {
                    c.ForeColor = p.TextPrimary;
                    c.BackColor = p.Surface;

                    if (c.Tag is "TitleBar")
                    {
                        c.BackColor = Darken(p.LowAccent, 0.4);
                    }
                }

                else if (c is PictureBox)
                {
                    if (c.Tag is "NonPaintable")
                    {
                        continue;
                    }
                }

                else if (c is Button)
                {
                    var b = (Button)c;

                    if (b.Tag is "NonPaintable")
                    {
                        b.FlatStyle = FlatStyle.Flat;
                        b.FlatAppearance.BorderSize = 0;
                        b.BackColor = Color.Transparent;
                        b.ForeColor = Darken(p.TextSecondary, -0.4);

                        Color hoverColor = Darken(p.LowAccent, 0.2);
                        Color pressColor = p.LowAccent;

                        b.FlatAppearance.MouseOverBackColor = hoverColor;
                        b.FlatAppearance.MouseDownBackColor = pressColor;

                        continue;
                    }

                    if (b.Tag is "ExternalTitleBar")
                    {
                        b.BackColor = Darken(p.LowAccent, 0.8);
                        continue;
                    }

                    if (b.Tag is "IsImageColorable")
                    {
                        MaybeRecolorControlImages(c, p, 1.6f, 0.10f);

                        b.FlatStyle = FlatStyle.Flat;
                        //b.FlatAppearance.BorderColor = p.Border;
                        b.FlatAppearance.BorderSize = 0;
                        b.BackColor = Color.Transparent;
                        b.ForeColor = p.TextSecondary;


                        Color hoverColor = p.HighAccent;
                        Color pressColor = Color.Transparent;

                        b.FlatAppearance.MouseOverBackColor = hoverColor;
                        b.FlatAppearance.MouseDownBackColor = pressColor;

                        continue;
                    }

                    if (b.Tag is "HighAccented")
                    {
                        b.FlatStyle = FlatStyle.Flat;
                        b.FlatAppearance.BorderColor = p.HighAccent;
                        b.FlatAppearance.BorderSize = 1;
                        b.BackColor = p.HighAccent;
                        b.ForeColor = Darken(ChooseReadableForeground(p.TextSecondary), -0.25);
                        continue;
                    }

                    b.FlatStyle = FlatStyle.Flat;
                    b.FlatAppearance.BorderColor = p.Border;
                    b.FlatAppearance.BorderSize = 1;
                    b.BackColor = p.Surface;
                    b.ForeColor = p.TextSecondary;
                }

                else if (c is TextBox)
                {
                    var tb = (TextBox)c;

                    tb.BackColor = p.Surface;
                    tb.ForeColor = p.TextPrimary;
                    tb.BorderStyle = BorderStyle.FixedSingle;

                    if (RedrawBorders)
                    {
                        Color borderColor = p.Border;
                        if (IsAccentuable(tb.Tag)) borderColor = p.Accent;
                        else if (IsLowAccented(tb.Tag)) borderColor = p.LowAccent;
                        else if (IsHighAccented(tb.Tag)) borderColor = p.HighAccent;

                        AttachCustomBorder(tb, borderColor);
                    }
                }

                else if (c is DateTimePicker)
                {
                    var dtp = (DateTimePicker)c;

                    DateTimePickerThemer.ThemeDateTimePicker(dtp, p);
                }

                else if (c is Label)
                {
                    var lbl = (Label)c;

                    lbl.ForeColor = p.TextSecondary;
                    lbl.BackColor = Color.Transparent;

                    if (lbl.Tag is "InternalTitleBar")
                    {
                        lbl.ForeColor = Darken(p.Accent, -0.85);
                        continue;
                    }

                    if (lbl.Tag is "Accentuable")
                    {
                        lbl.ForeColor = p.LowAccent;
                        continue;
                    }
                }
                else if (c is ComboBox)
                {
                    var cb = (ComboBox)c;

                    cb.FlatStyle = FlatStyle.Flat; // Importante para que acepte mejor el pintado

                    cb.BackColor = p.Surface;
                    cb.ForeColor = p.TextSecondary;


                    ComboBoxThemer.ThemeComboBox(cb, p);

                }
                else if (c is MdiClient)
                {
                    var mdi = (MdiClient)c;

                    c.BackColor = Color.Red;
                    c.ForeColor = p.TextSecondary;


                }
                else if (c is Chart)
                {
                    var chart = (Chart)c;

                    // 1. Fondo base del control (el lienzo exterior)
                    chart.BackColor = p.Surface;
                    chart.ForeColor = p.TextPrimary;

                    // 2. Títulos del gráfico
                    foreach (var t in chart.Titles)
                    {
                        t.ForeColor = p.TextPrimary;
                        t.BackColor = Color.Transparent;
                    }

                    // 3. Leyendas (el recuadro que dice qué es cada color)
                    foreach (var l in chart.Legends)
                    {
                        l.BackColor = p.Surface; // O Color.Transparent si prefieres
                        l.ForeColor = p.TextSecondary;
                        l.TitleForeColor = p.TextPrimary;
                    }

                    // 4. ChartAreas (Aquí vive el gráfico real: Ejes y Grilla)
                    foreach (var ca in chart.ChartAreas)
                    {
                        // El fondo donde se dibujan las barras/líneas
                        ca.BackColor = Color.Transparent; // O p.SurfaceAlt si quieres un recuadro distinguido

                        // --- EJE X ---
                        ca.AxisX.LabelStyle.ForeColor = p.TextSecondary;
                        ca.AxisX.LineColor = p.Border;          // La línea base del eje
                        ca.AxisX.MajorGrid.LineColor = p.GridLine;  // Las líneas de la grilla vertical
                        ca.AxisX.TitleForeColor = p.TextSecondary;

                        // MinorGrid (si la usas)
                        ca.AxisX.MinorGrid.LineColor = Darken(p.GridLine, 0.2);

                        // --- EJE Y ---
                        ca.AxisY.LabelStyle.ForeColor = p.TextSecondary;
                        ca.AxisY.LineColor = p.Border;
                        ca.AxisY.MajorGrid.LineColor = p.GridLine;  // Las líneas de la grilla horizontal
                        ca.AxisY.TitleForeColor = p.TextSecondary;

                        // MinorGrid (si la usas)
                        ca.AxisY.MinorGrid.LineColor = Darken(p.GridLine, 0.2);
                    }

                    int i = 0;
                    foreach (var series in chart.Series)
                    {
                        // Si tienes una sola línea, usa el color de acento principal
                        if (chart.Series.Count == 1)
                        {
                            series.Color = p.Accent;
                        }
                        else
                        {
                            // Si tienes varias líneas, rotamos colores para que no sean todas iguales
                            Color[] seriesColors =
                            {
                                p.Accent, 
                                // Si HighAccent está vacío (Color.Empty), usamos TextPrimary como fallback
                                p.HighAccent.IsEmpty ? p.TextPrimary : p.HighAccent,
                                p.LowAccent.IsEmpty ? p.TextSecondary : p.LowAccent
                            };
                        }

                        // Opcional: Colorear los marcadores (los puntos)
                        series.MarkerColor = series.Color;
                        series.MarkerBorderColor = p.Surface; // Un borde del color de fondo hace que el punto se "separe" visualmente

                        // Color de las etiquetas de valor si las tuvieras activas
                        series.LabelForeColor = p.TextPrimary;

                        i++;
                    }
                }
                else
                {
                    c.ForeColor = p.TextPrimary;
                    c.BackColor = (c.Parent is Panel || c.Parent is GroupBox) ? p.SurfaceAlt : p.Background;
                }

                // Acentos por tag
                if (IsAccentuable(c.Tag) || IsLowAccented(c.Tag) || IsHighAccented(c.Tag))
                    ApplyAccentToControl(c, p);

                // ToolStrip: acentos + recolor íconos
                var ts2 = c as ToolStrip;

                if (ts2 != null)
                {
                    AccentToolStripItemsRecursive(ts2, p);

                    RecolorToolStripIconsRecursive(ts2, p);
                    if (IsDarkPalette(p)) SimpleDarkMenuPass(ts2, p, GetCurrentDepth(c.FindForm()));
                }


                if (c.ContextMenuStrip != null)
                    WireAndStyleContextMenu(c.ContextMenuStrip, p, GetCurrentDepth(c.FindForm()));


                //Si el control tiene hijos se aplica de forma recursiva
                HasChildren(c, p);

                var dgv = c as DataGridView;

                if (dgv != null)
                {
                    DataGridViewThemer.AccentDgvColumns(dgv, p);
                    dgv.ScrollBars = ScrollBars.None;
                }



            }
        }

        private static void HasChildren(Control c, Palette p)
        {
            // Descender
            if (c.HasChildren) PaintControlTree(c, p);
        }

        // ============================
        // 5) TAG HELPERS
        // ============================
        private static readonly StringComparer _ic = StringComparer.OrdinalIgnoreCase;
        private static readonly char[] _tagSep = { ' ', ',', ';' };

        internal static bool IsNonPaintable(object tag) => HasTag(tag, "NonPaintable");
        internal static bool IsAccentuable(object tag) => HasTag(tag, "Accentuable");
        internal static bool IsLowAccented(object tag) => HasTag(tag, "LowAccented");
        internal static bool IsHighAccented(object tag) => HasTag(tag, "HighAccented");


        internal static bool HasTag(object tag, string wanted)
        {
            if (tag == null) return false;

            var s = tag as string;
            if (s != null)
                return s.Split(_tagSep, StringSplitOptions.RemoveEmptyEntries).Any(t => _ic.Equals(t, wanted));

            var list = tag as IEnumerable<string>;
            if (list != null)
                return list.Any(t => _ic.Equals(t, wanted));

            return _ic.Equals(tag.ToString(), wanted);
        }

        private static void ApplyAccentToControl(Control c, Palette p)
        {
            Color accentColor = p.Accent;
            if (IsLowAccented(c.Tag)) accentColor = p.LowAccent;
            else if (IsHighAccented(c.Tag)) accentColor = p.HighAccent;

            c.BackColor = accentColor;
            c.ForeColor = ChooseReadableForeground(accentColor);

            var b = c as Button;

            if (b != null)
            {
                b.FlatStyle = FlatStyle.Flat;
                b.FlatAppearance.BorderColor = p.LowAccent;
                b.FlatAppearance.BorderSize = 1;
            }
        }

        // ============================
        // 6) TOOLSTRIP / MENUSTRIP
        // ============================
        private static void AccentToolStripItemsRecursive(ToolStrip ts, Palette p)
        {
            foreach (ToolStripItem it in ts.Items)
                AccentToolStripItem(it, p);
        }
        private static void AccentToolStripItem(ToolStripItem item, Palette p)
        {
            if (item == null) return;

            // Default legible
            item.ForeColor = item.Enabled ? p.TextSecondary : p.TextPrimary;

            if (IsLowAccented(item.Tag)) item.ForeColor = ChooseReadableForeground(p.LowAccent);
            else if (IsHighAccented(item.Tag)) item.ForeColor = ChooseReadableForeground(p.HighAccent);
            else if (IsAccentuable(item.Tag)) item.ForeColor = ChooseReadableForeground(p.Accent);

            var drop = item as ToolStripDropDownItem;
            if (drop != null)
                foreach (ToolStripItem child in drop.DropDownItems)
                    AccentToolStripItem(child, p);

            var host = item as ToolStripControlHost;
            if (host != null && host.Control != null)
            {
                if (!IsAccentuable(host.Control.Tag) && !IsLowAccented(host.Control.Tag) && !IsHighAccented(host.Control.Tag))
                {
                    if (IsLowAccented(item.Tag)) host.Control.Tag = "LowAccented";
                    else if (IsHighAccented(item.Tag)) host.Control.Tag = "HighAccented";
                    else if (IsAccentuable(item.Tag)) host.Control.Tag = "Accentuable";
                }
                if (IsAccentuable(host.Control.Tag) || IsLowAccented(host.Control.Tag) || IsHighAccented(host.Control.Tag))
                    ApplyAccentToControl(host.Control, p);
            }
        }

        private static void StyleToolStrips(Form form, Palette p, VisualDepth depth)
        {
            foreach (var ts in FindAll<ToolStrip>(form))
            {
                if (IsNonPaintable(ts.Tag))
                {
                    ts.RenderMode = ToolStripRenderMode.Professional;
                    ts.Renderer = new ToolStripProfessionalRenderer();
                    continue;
                }
                SetupToolStrip(ts, p, depth);
            }
            foreach (var ss in FindAll<StatusStrip>(form))
                SetupStatusStrip(ss, p, depth);
        }

        private static void SetupToolStrip(ToolStrip ts, Palette p, VisualDepth depth)
        {
            if (IsNonPaintable(ts.Tag)) return;

            ts.RenderMode = ToolStripRenderMode.Professional;
            ts.BackColor = Color.Transparent;
            ts.ForeColor = p.TextPrimary;
            ts.Renderer = (depth == VisualDepth.ThreeD)
                ? (ToolStripProfessionalRenderer)new StripBackgroundRenderer3D(p)
                : new StripBackgroundRendererPlain(p);

            // Recolorear íconos top-level
            RecolorToolStripIconsRecursive(ts, p);

            //SIEMPRE cablear y estilizar drop-downs (no solo en dark)
            SimpleDarkMenuPass(ts, p, depth);
        }

        private static void SetupStatusStrip(StatusStrip ss, Palette p, VisualDepth depth)
        {
            if (IsNonPaintable(ss.Tag)) return;
            ss.RenderMode = ToolStripRenderMode.Professional;
            ss.BackColor = Color.Transparent;
            ss.ForeColor = p.TextPrimary;
            ss.Renderer = (depth == VisualDepth.ThreeD)
                ? (ToolStripProfessionalRenderer)new StripBackgroundRenderer3D(p)
                : new StripBackgroundRendererPlain(p);
        }

        private sealed class StripBackgroundRenderer3D : ToolStripProfessionalRenderer
        {
            private readonly Color _begin, _mid, _end, _border, _accent, _lowAccent, _highAccent;
            private readonly Color _hoverOverlay, _pressOverlay;
            private readonly Color _menuBg; // para drop-down

            public StripBackgroundRenderer3D(Palette p)
            {
                if (!IsDarkPalette(GetCurrentPalette()))
                {
                    _begin = p.Surface;
                    _end = p.SurfaceAlt;
                }
                else
                {
                    _begin = p.SurfaceAlt;
                    _end = p.Surface;
                }

                _mid = Blend(_begin, _end, .5);
                _border = p.Border;
                _accent = p.Accent;
                _lowAccent = p.LowAccent;
                _highAccent = p.HighAccent;
                _hoverOverlay = Color.FromArgb(28, p.AccentHover);
                _pressOverlay = Color.FromArgb(40, p.Accent);
                _menuBg = p.Background; // fondo liso para drop-down
            }

            protected override void OnRenderImageMargin(ToolStripRenderEventArgs e)
            {
                // No dibujar el gutter de imagen para que se vea el fondo del ítem
                // (evita el color distinto a la izquierda de cada ToolStripMenuItem).
                // Intencionalmente vacío.
            }

            protected override void OnRenderToolStripBackground(ToolStripRenderEventArgs e)
            {
                if (e.ToolStrip is ToolStripDropDownMenu)
                {
                    using (var br = new SolidBrush(_menuBg))
                        e.Graphics.FillRectangle(br, e.AffectedBounds);
                    return;
                }
                using (var br = new LinearGradientBrush(e.AffectedBounds, _begin, _end, LinearGradientMode.Vertical))
                    e.Graphics.FillRectangle(br, e.AffectedBounds);
            }

            protected override void OnRenderToolStripBorder(ToolStripRenderEventArgs e)
            {
                using (var pen = new Pen(_border))
                    e.Graphics.DrawRectangle(pen, 0, 0, e.ToolStrip.Width - 1, e.ToolStrip.Height - 1);
            }

            protected override void OnRenderItemText(ToolStripItemTextRenderEventArgs e)
            {
                // Evita Color?; resolvé a Color concreto con fallback
                Color want = (e.Item != null && !e.Item.ForeColor.IsEmpty)
                    ? e.Item.ForeColor
                    : (e.Item?.Owner?.ForeColor ?? SystemColors.ControlText);

                e.TextColor = want;
                base.OnRenderItemText(e);
            }

            protected override void OnRenderButtonBackground(ToolStripItemRenderEventArgs e) { Overlay(e); }
            protected override void OnRenderDropDownButtonBackground(ToolStripItemRenderEventArgs e) { Overlay(e); }
            protected override void OnRenderSplitButtonBackground(ToolStripItemRenderEventArgs e) { Overlay(e); }
            protected override void OnRenderMenuItemBackground(ToolStripItemRenderEventArgs e) { Overlay(e); }

            private void Overlay(ToolStripItemRenderEventArgs e)
            {
                var r = new Rectangle(Point.Empty, e.Item.Size);
                r.Inflate(-1, -1);

                Color accentColor = _accent;
                bool isAccented = false;
                var ownerTag = e.Item.Owner == null ? null : e.Item.Owner.Tag;

                if (IsLowAccented(e.Item.Tag) || IsLowAccented(ownerTag)) { accentColor = _lowAccent; isAccented = true; }
                else if (IsHighAccented(e.Item.Tag) || IsHighAccented(ownerTag)) { accentColor = _highAccent; isAccented = true; }
                else if (IsAccentuable(e.Item.Tag) || IsAccentuable(ownerTag)) { isAccented = true; }

                if (isAccented)
                {
                    using (var br = new SolidBrush(accentColor)) e.Graphics.FillRectangle(br, r);
                    using (var pen = new Pen(_border)) e.Graphics.DrawRectangle(pen, r.X, r.Y, r.Width - 1, r.Height - 1);
                    e.Item.ForeColor = ChooseReadableForeground(accentColor);
                    return;
                }

                if (!e.Item.Enabled) return;

                if (e.Item.Pressed)
                {
                    using (var br = new SolidBrush(_pressOverlay)) e.Graphics.FillRectangle(br, r);
                }
                else if (e.Item.Selected)
                {
                    using (var br = new SolidBrush(_hoverOverlay)) e.Graphics.FillRectangle(br, r);
                }
            }
        }

        private sealed class StripBackgroundRendererPlain : ToolStripProfessionalRenderer
        {
            private readonly Color _fill, _border, _accent, _lowAccent, _highAccent;
            private readonly Color _menuBg; // para drop-down

            public StripBackgroundRendererPlain(Palette p)
            {
                _fill = p.SurfaceAlt;
                _border = p.Border;
                _accent = p.Accent;
                _lowAccent = p.LowAccent;
                _highAccent = p.HighAccent;
                _menuBg = p.Background; // fondo liso para drop-down
            }

            protected override void OnRenderImageMargin(ToolStripRenderEventArgs e)
            {
                // Ídem: no pintar margen de imagen.
            }

            protected override void OnRenderToolStripBackground(ToolStripRenderEventArgs e)
            {
                if (e.ToolStrip is ToolStripDropDownMenu)
                {
                    using (var br = new SolidBrush(_menuBg))
                        e.Graphics.FillRectangle(br, e.AffectedBounds);
                    return;
                }
                using (var br = new SolidBrush(_fill)) e.Graphics.FillRectangle(br, e.AffectedBounds);
            }

            protected override void OnRenderToolStripBorder(ToolStripRenderEventArgs e)
            {
                using (var pen = new Pen(_border))
                    e.Graphics.DrawRectangle(pen, 0, 0, e.ToolStrip.Width - 1, e.ToolStrip.Height - 1);
            }

            protected override void OnRenderItemText(ToolStripItemTextRenderEventArgs e)
            {
                Color want = (e.Item != null && !e.Item.ForeColor.IsEmpty)
                    ? e.Item.ForeColor
                    : (e.Item?.Owner?.ForeColor ?? SystemColors.ControlText);

                e.TextColor = want;
                base.OnRenderItemText(e);
            }

            protected override void OnRenderItemBackground(ToolStripItemRenderEventArgs e)
            {
                Color accentColor = _accent;
                bool isAccented = false;
                var ownerTag = e.Item.Owner == null ? null : e.Item.Owner.Tag;

                if (IsLowAccented(e.Item.Tag) || IsLowAccented(ownerTag)) { accentColor = _lowAccent; isAccented = true; }
                else if (IsHighAccented(e.Item.Tag) || IsHighAccented(ownerTag)) { accentColor = _highAccent; isAccented = true; }
                else if (IsAccentuable(e.Item.Tag) || IsAccentuable(ownerTag)) { isAccented = true; }

                if (isAccented)
                {
                    var r = new Rectangle(Point.Empty, e.Item.Size);
                    r.Inflate(-1, -1);
                    using (var br = new SolidBrush(accentColor)) e.Graphics.FillRectangle(br, r);
                    using (var pen = new Pen(_border)) e.Graphics.DrawRectangle(pen, r.X, r.Y, r.Width - 1, r.Height - 1);
                    e.Item.ForeColor = ChooseReadableForeground(accentColor);
                }
            }
        }

        // ============================
        // 7) PANELES (gradiente opcional)
        // ============================

        private static void StylePanels(Form form, Palette p, VisualDepth depth)
        {
            foreach (var pnl in FindAll<Panel>(form))
            {

                if (IsNonPaintable(pnl.Tag))
                {
                    RemoveGradientBackground(pnl);
                    continue;
                }

                bool isDarkPalette = IsDarkPalette(p);

                // Panel común 3D o plano - Revisa taggeos, de no haberlos, pinta por default.

                if (pnl.Tag is "SubPanel")
                {
                    if (isDarkPalette)
                        ApplyGradientBackground(pnl, Darken(p.Accent, 0.6), Darken(p.LowAccent, 0.6), LinearGradientMode.Horizontal, false);
                    else
                        ApplyGradientBackground(pnl, Darken(p.Accent, 0.6), Darken(p.LowAccent, 0.6), LinearGradientMode.Horizontal, false);
                }

                if (pnl.Tag is "SearchBar")
                            {
                    if (isDarkPalette)
                        ApplyGradientBackground(pnl, Darken(p.Surface, -0.05), Darken(p.Surface, 0.2), LinearGradientMode.Vertical, false);
                    else
                        ApplyGradientBackground(pnl, Darken(p.Surface, 0.03), Darken(p.SurfaceAlt, -0.01), LinearGradientMode.Vertical, false);
                }

                else if (pnl.Tag is "InternalTitleBar")
                {
                    if (isDarkPalette)
                        ApplyGradientBackground(pnl, Darken(p.LowAccent, 0.65), Darken(p.Accent, 0.65), LinearGradientMode.Vertical, false);
                    else
                        ApplyGradientBackground(pnl, Darken(p.LowAccent, 0.65), Darken(p.Accent, 0.65), LinearGradientMode.Vertical, false);
                }


                else if (pnl.Tag is "ExternalTitleBar")
                {
                    ApplyGradientBackground(pnl, Darken(p.LowAccent, 0.8), Darken(p.Accent, 0.8), LinearGradientMode.Horizontal, false);
                }

                else if (pnl.Tag is "LowAccented")
                {
                    ApplyGradientBackground(pnl, Darken(p.LowAccent, 0.8), Darken(p.Accent, 0.8), LinearGradientMode.Vertical, false);
                }

                else
                {
                    if (isDarkPalette)
                        ApplyGradientBackground(pnl, Darken(p.Surface, -0.1), Darken(p.Surface, 0.3), LinearGradientMode.Vertical, false);
                    else
                        ApplyGradientBackground(pnl, p.Surface, p.SurfaceAlt, LinearGradientMode.Vertical, false);
                }


            }
        }



        // ============================
        // 9) GRADIENTE REUTILIZABLE
        // ============================
        private sealed class _GradientAttachment
        {
            public Color Begin, End, Border;
            public LinearGradientMode Direction;
            public bool WithBorder;
            private readonly Control _c;
            private bool _isMouseOver;

            public _GradientAttachment(Control c)
            {
                _c = c;

                // Solo suscribe si es un botón. De ser Panel o Form, no se necesitan estos eventos.
                if (_c is Button)
                {
                    _c.MouseEnter += OnMouseEnter;
                    _c.MouseLeave += OnMouseLeave;
                }
            }

            private void OnMouseEnter(object sender, EventArgs e)
            {
                _isMouseOver = true;
                _c.Invalidate();
            }

            private void OnMouseLeave(object sender, EventArgs e)
            {
                _isMouseOver = false;
                _c.Invalidate();
            }

            public void OnPaint(object sender, PaintEventArgs e)
            {
                if (_c.IsDisposed) return;
                var r = _c.ClientRectangle;
                if (r.Width <= 0 || r.Height <= 0) return;

                // --- LÓGICA DE COLORES RESTRINGIDA ---
                // Solo aplicamos el aclarado si el ratón está encima Y es un botón.
                bool applyHover = _isMouseOver && _c is Button;

                Color drawBegin = applyHover ? Darken(Begin, -0.15) : Begin;
                Color drawEnd = applyHover ? Darken(End, -0.15) : End;
                Color drawBorder = applyHover ? Darken(Border, -0.2) : Border;

                // 1. Dibujar el fondo
                using (var br = new LinearGradientBrush(r, drawBegin, drawEnd, Direction))
                {
                    e.Graphics.FillRectangle(br, r);
                }

                // 2. Dibujar el contenido (Texto e Icono)
                if (_c is Button btn)
                {
                    // (Aquí va tu lógica de layout estilo Chrome que ya teníamos...)
                    if (btn.Image != null)
                    {
                        // --- 1. Configuración de márgenes ---
                        const int LEFT_MARGIN = 8;
                        const int ICON_TEXT_GAP = 5;
                        const int V_PADDING = 4; // Espacio de seguridad arriba y abajo
                        Rectangle textRect = r;

                        // --- 2. Cálculo de Escala ---
                        // Altura máxima permitida para el icono
                        int maxHeight = r.Height - (V_PADDING * 2);

                        // Calculamos el ancho proporcional para no deformar la imagen
                        float ratio = (float)btn.Image.Width / btn.Image.Height;
                        int destHeight = Math.Min(btn.Image.Height, maxHeight);
                        int destWidth = (int)(destHeight * ratio);

                        // Si por alguna razón el ancho calculado es mayor al del botón (caso raro), re-escalamos
                        if (destWidth > r.Width / 3) // Limitar a un tercio del botón máximo
                        {
                            destWidth = r.Width / 3;
                            destHeight = (int)(destWidth / ratio);
                        }

                        // --- 3. Posicionamiento ---
                        int iconY = (r.Height - destHeight) / 2;

                        // Rectángulo de destino para el dibujo
                        Rectangle iconDestRect = new Rectangle(LEFT_MARGIN, iconY, destWidth, destHeight);

                        // --- 4. Renderizado de Alta Calidad ---
                        // Es vital para que al achicar el icono no se vea "serruchado" (pixelado)
                        var oldInterpolation = e.Graphics.InterpolationMode;
                        e.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;

                        e.Graphics.DrawImage(btn.Image, iconDestRect);

                        e.Graphics.InterpolationMode = oldInterpolation; // Restauramos

                        // --- 5. Ajuste del área de texto ---
                        // Ahora el texto empieza después del icono escalado
                        int textStartX = LEFT_MARGIN + destWidth + ICON_TEXT_GAP;
                        int availableWidth = r.Width - textStartX - LEFT_MARGIN;
                        textRect = new Rectangle(textStartX, 0, availableWidth, r.Height);


                        TextFormatFlags flags = TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter |
                             TextFormatFlags.SingleLine | TextFormatFlags.EndEllipsis;

                        TextRenderer.DrawText(e.Graphics, btn.Text, btn.Font, textRect, btn.ForeColor, flags);
                        /*
                                            if (btn.Image != null)
                    {
                        int iconY = Math.Max(0, (r.Height - btn.Image.Height) / 2);
                        e.Graphics.DrawImage(btn.Image, LEFT_MARGIN, iconY);

                        int textStartX = LEFT_MARGIN + btn.Image.Width + ICON_TEXT_GAP;
                        textRect = new Rectangle(textStartX, 0, r.Width - textStartX - LEFT_MARGIN, r.Height);
                    }

                    TextFormatFlags flags = TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter |
                                            TextFormatFlags.SingleLine | TextFormatFlags.EndEllipsis;

                    TextRenderer.DrawText(e.Graphics, btn.Text, btn.Font, textRect, btn.ForeColor, flags);
                }
                        */
                    }
                }

                // 3. Dibujar el borde
                if (WithBorder)
                {
                    using (var pen = new Pen(drawBorder))
                        e.Graphics.DrawRectangle(pen, 0, 0, r.Width - 1, r.Height - 1);
                }
            }

            public void OnDisposed(object sender, EventArgs e)
            {
                try
                {
                    _c.Paint -= OnPaint;


                    // Solo desuscribir si es botón (limpieza segura)
                    if (_c is Button)
                    {
                        _c.MouseEnter -= OnMouseEnter;
                        _c.MouseLeave -= OnMouseLeave;
                    }
                    _c.Disposed -= OnDisposed;
                }
                catch { }
            }
        }
        private static readonly ConditionalWeakTable<Control, _GradientAttachment> _gradients =
            new ConditionalWeakTable<Control, _GradientAttachment>();

        public static void ApplyGradientBackground(Control c, Color begin, Color end,
                                                LinearGradientMode direction = LinearGradientMode.Vertical,
                                                bool withBorder = true, Color? borderColor = null)
        {
            if (c == null || c.IsDisposed) return;

            _GradientAttachment att;

            if (_gradients.TryGetValue(c, out att))
            {
                att.Begin = begin; att.End = end;
                att.Direction = direction; att.WithBorder = withBorder;
                att.Border = borderColor ?? Color.FromArgb(60, Color.Black);
                c.Invalidate();
                return;
            }
            var attach = new _GradientAttachment(c)
            {
                Begin = begin,
                End = end,
                Direction = direction,
                WithBorder = withBorder,
                Border = borderColor ?? Color.FromArgb(60, Color.DarkGray)
            };
            TryEnableDoubleBuffering(c);
            c.Paint += attach.OnPaint;
            c.Disposed += attach.OnDisposed;
            _gradients.Add(c, attach);
            c.Invalidate();
        }

        public static void ApplyGradientBackground
        (
            Control c,
            Palette p,
            LinearGradientMode direction = LinearGradientMode.Vertical
        )
           => ApplyGradientBackground(c, p.Surface, p.SurfaceAlt, direction, true, p.Border);


        public static void RemoveGradientBackground(Control c)
        {
            if (c == null) return;
            _GradientAttachment att;
            if (_gradients.TryGetValue(c, out att))
            {
                c.Paint -= att.OnPaint;
                c.Disposed -= att.OnDisposed;
                _gradients.Remove(c);
                c.Invalidate();
            }
        }

        // ============================
        // 10) UTILIDADES
        // ============================
        private static IEnumerable<T> FindAll<T>(Control root) where T : Control
        {
            foreach (Control c in root.Controls)
            {
                var t = c as T;
                if (t != null) yield return t;
                foreach (var child in FindAll<T>(c)) yield return child;
            }
        }

        internal static void TryEnableDoubleBuffering(Control c)
        {
            try
            {
                var prop = c.GetType().GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic);
                if (prop != null) prop.SetValue(c, true, null);
            }
            catch { }
        }

        internal static Color Blend(Color a, Color b, double t)
        {
            if (t < 0) t = 0; if (t > 1) t = 1;
            Func<int, int, int> Lerp = (x, y) => (int)Math.Round(x + (y - x) * t);
            return Color.FromArgb(255, Lerp(a.R, b.R), Lerp(a.G, b.G), Lerp(a.B, b.B));
        }

        // t > 0 -> oscurece; t < 0 -> aclara. |t| en [0..1].
        public static Color Darken(Color c, double t)
        {
            if (t < -1) t = -1; if (t > 1) t = 1;
            Func<int, int> Chan = v =>
            {
                if (t >= 0) return (int)Math.Round(v * (1 - t));
                var amt = -t; // aclarar
                return (int)Math.Round(v + (255 - v) * amt);
            };
            return Color.FromArgb(255, Clamp0_255(Chan(c.R)), Clamp0_255(Chan(c.G)), Clamp0_255(Chan(c.B)));
        }
        private static int Clamp0_255(int v) { return v < 0 ? 0 : (v > 255 ? 255 : v); }

        public static Color ChooseReadableForeground(Color bg, double threshold = 4.5)
        {
            double cBlack = ContrastRatio(bg, Color.Black);
            double cWhite = ContrastRatio(bg, Color.White);
            if (cBlack >= threshold && cBlack >= cWhite) return Color.Black;
            if (cWhite >= threshold && cWhite > cBlack) return Color.White;
            return cBlack >= cWhite ? SystemColors.ControlText : Color.White;
        }

        private static double ContrastRatio(Color a, Color b)
        {
            double L1 = RelativeLuminance(a), L2 = RelativeLuminance(b);
            if (L1 < L2) { var tmp = L1; L1 = L2; L2 = tmp; }
            return (L1 + 0.05) / (L2 + 0.05);
        }

        private static double RelativeLuminance(Color c)
        {
            double r = SrgbToLinear(c.R / 255.0);
            double g = SrgbToLinear(c.G / 255.0);
            double b = SrgbToLinear(c.B / 255.0);
            return 0.2126 * r + 0.7152 * g + 0.0722 * b;
        }

        private static double SrgbToLinear(double v)
        {
            return (v <= 0.03928) ? v / 12.92 : Math.Pow((v + 0.055) / 1.055, 2.4);
        }

        internal static bool IsDarkPalette(Palette p) => RelativeLuminance(p.Background) < 0.25;

        // ============================
        // 11) ICONOGRAFÍA - DUOTONE + CACHE (con ORIGINAL preservado)
        // ============================
        private static readonly object _iconCacheLock = new object();
        private static readonly Dictionary<string, Bitmap> _iconCache = new Dictionary<string, Bitmap>();
        private static void ClearIconCache()
        {
            lock (_iconCacheLock)
            {
                foreach (var kv in _iconCache) { try { kv.Value.Dispose(); } catch { } }
                _iconCache.Clear();
            }
        }

        // Fuerza que las altas luces sean exactamente HI
        private const float ICON_GAMMA = 0.65f;  // empuja rápidamente hacia HI
        private const float ICON_SNAP_HI = 0.85f;

        // Mantener referencia al ORIGINAL (para no “apilar” tintes)
        private static readonly ConditionalWeakTable<ToolStripItem, Image> _origIconByItem = new ConditionalWeakTable<ToolStripItem, Image>();
        private static readonly ConditionalWeakTable<Control, Image> _origBgByControl = new ConditionalWeakTable<Control, Image>();
        private static readonly ConditionalWeakTable<Control, Image> _origImgByControl = new ConditionalWeakTable<Control, Image>();

        // Últimos tintados (para dispose seguro)
        private static readonly ConditionalWeakTable<ToolStripItem, Bitmap> _tintedByItem = new ConditionalWeakTable<ToolStripItem, Bitmap>();
        private static readonly ConditionalWeakTable<Control, Image> _tintedBgByControl = new ConditionalWeakTable<Control, Image>();

        private static string IconKey(Image img, Color hi, Color lo, float gain, float bias)
        {
            return RuntimeHelpers.GetHashCode(img).ToString("X") + "|" +
                   hi.ToArgb().ToString("X8") + "|" + lo.ToArgb().ToString("X8") + "|" +
                   gain.ToString("F3") + "|" + bias.ToString("F3");
        }

        // Colores HI/IO para iconografía
        private static readonly Color IconHi_Light = Color.FromArgb(163, 173, 186);
        private static readonly Color IconHi_Dark = Color.FromArgb(210, 216, 224);

        private static void MaybeInvertForDark(Palette p, ref Color hi, ref Color lo)
        {
            if (!IsDarkPalette(p)) return;
            var tmp = hi; hi = lo; lo = tmp;
        }

        private static Color PickHiFixedForPalette(Palette p) => IsDarkPalette(p) ? IconHi_Dark : IconHi_Light;
        private static Color PickLowForPalette(Palette p) => Darken(p.Accent, 0.5);
        private static Color PickLowBaseForIcons(Palette p, ToolStrip owner = null, Control container = null) => p.Accent;

        private static Image RememberOriginalItemImage(ToolStripItem it)
        {
            Image orig;
            if (!_origIconByItem.TryGetValue(it, out orig) || orig == null)
            {
                if (it.Image == null) return null;
                orig = (Bitmap)it.Image.Clone();
                _origIconByItem.Add(it, orig);
            }
            return orig;
        }
        private static Image RememberOriginalControlBg(Control c)
        {
            Image orig;
            if (!_origBgByControl.TryGetValue(c, out orig) || orig == null)
            {
                if (c.BackgroundImage == null) return null;
                orig = (Bitmap)c.BackgroundImage.Clone();
                _origBgByControl.Add(c, orig);
            }
            return orig;
        }
        private static Image RememberOriginalControlImg(Control c, Image current)
        {
            Image orig;
            if (!_origImgByControl.TryGetValue(c, out orig) || orig == null)
            {
                if (current == null) return null;
                orig = (Bitmap)current.Clone();
                _origImgByControl.Add(c, orig);
            }
            return orig;
        }

        private static void SafeAssignTinted(ToolStripItem it, Bitmap tinted)
        {
            Bitmap prev;
            if (_tintedByItem.TryGetValue(it, out prev) && prev != null)
            {
                try { prev.Dispose(); } catch { }
                _tintedByItem.Remove(it);
            }
            it.Image = tinted;
            _tintedByItem.Add(it, tinted);
        }

        private static Bitmap GetTintedCached(Image src, Color hi, Color lo, float gain, float bias)
        {
            using (var base32 = new Bitmap(src.Width, src.Height, PixelFormat.Format32bppArgb))
            using (var g = Graphics.FromImage(base32))
            {
                g.DrawImage(src, 0, 0, src.Width, src.Height);

                var key = IconKey(src, hi, lo, gain, bias);
                lock (_iconCacheLock)
                {
                    Bitmap cached;
                    if (_iconCache.TryGetValue(key, out cached) && cached != null)
                        return (Bitmap)cached.Clone();

                    var tinted = DuoTone(base32, hi, lo, gain, bias);
                    _iconCache[key] = (Bitmap)tinted.Clone();
                    return tinted;
                }
            }
        }

        private const float ICON_NORM_BIAS = 0.06f;
        private static Bitmap GetTintedCachedNormalized(Image src, Color hi, Color lo, float extraBiasIgnored = 0f)
        {
            using (var base32 = new Bitmap(src.Width, src.Height, PixelFormat.Format32bppArgb))
            using (var g = Graphics.FromImage(base32))
            {
                g.DrawImage(src, 0, 0, src.Width, src.Height);

                ComputeLumaRange(base32, out var lmin, out var lmax);
                string key = RuntimeHelpers.GetHashCode(src).ToString("X") + "|" +
                             hi.ToArgb().ToString("X8") + "|" + lo.ToArgb().ToString("X8") + "|" +
                             ICON_GAMMA.ToString("F3") + "|" + ICON_SNAP_HI.ToString("F3") + "|LERP";

                lock (_iconCacheLock)
                {
                    if (_iconCache.TryGetValue(key, out var cached) && cached != null)
                        return (Bitmap)cached.Clone();

                    var tinted = DuoToneLerpFixedHi(base32, hi, lo, lmin, lmax, ICON_GAMMA, ICON_SNAP_HI);
                    _iconCache[key] = (Bitmap)tinted.Clone();
                    return tinted;
                }
            }
        }

        private static Bitmap DuoToneLerpFixedHi(Bitmap src, Color hi, Color lo, float lmin, float lmax, float gamma, float snapHi)
        {
            var wr = 0.2126f; var wg = 0.7152f; var wb = 0.0722f;
            var dst = new Bitmap(src.Width, src.Height, PixelFormat.Format32bppArgb);
            var rect = new Rectangle(0, 0, src.Width, src.Height);
            var s = src.LockBits(rect, ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
            var d = dst.LockBits(rect, ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);

            try
            {
                unsafe
                {
                    byte* sp = (byte*)s.Scan0;
                    byte* dp = (byte*)d.Scan0;
                    int sw = s.Stride, dw = d.Stride;
                    float denom = Math.Max(1e-6f, (lmax - lmin));

                    for (int y = 0; y < src.Height; y++)
                    {
                        byte* srow = sp + y * sw;
                        byte* drow = dp + y * dw;

                        for (int x = 0; x < src.Width; x++)
                        {
                            byte* spx = srow + x * 4;
                            byte* dpx = drow + x * 4;

                            byte a = spx[3];
                            if (a == 0)
                            {
                                dpx[0] = dpx[1] = dpx[2] = 0;
                                dpx[3] = 0;
                                continue;
                            }

                            float b = spx[0] / 255f, g = spx[1] / 255f, r = spx[2] / 255f;
                            float L = r * wr + g * wg + b * wb;
                            float Ln = (L - lmin) / denom;
                            Ln = Math.Max(0f, Math.Min(1f, Ln));
                            Ln = (float)Math.Pow(Ln, gamma);

                            if (Ln >= snapHi)
                            {
                                dpx[2] = (byte)hi.R; dpx[1] = (byte)hi.G; dpx[0] = (byte)hi.B; dpx[3] = a;
                            }
                            else
                            {
                                float t = Ln;
                                int R = (int)Math.Round(lo.R + (hi.R - lo.R) * t);
                                int G = (int)Math.Round(lo.G + (hi.G - lo.G) * t);
                                int B = (int)Math.Round(lo.B + (hi.B - lo.B) * t);
                                dpx[2] = (byte)R; dpx[1] = (byte)G; dpx[0] = (byte)B; dpx[3] = a;
                            }
                        }
                    }
                }
            }
            finally
            {
                src.UnlockBits(s);
                dst.UnlockBits(d);
            }
            return dst;
        }

        private static void RecolorToolStripIconsRecursive(ToolStrip ts, Palette p, float gain = 1.6f, float bias = 0.10f)
        {
            //  return;
            foreach (ToolStripItem it in ts.Items)
                RecolorToolStripItem(it, p, gain, bias);
        }
        private static void RecolorToolStripItem(ToolStripItem it, Palette p, float gain, float bias)
        {
            //return;
            var drop = it as ToolStripDropDownItem;
            if (drop != null)
                foreach (ToolStripItem child in drop.DropDownItems)
                    RecolorToolStripItem(child, p, gain, bias);

            if (it.Image == null) return;


            Image src = RememberOriginalItemImage(it);

            if (src == null) return;

            Color hi = PickHiFixedForPalette(p);
            Color lo = PickLowForPalette(p);

            MaybeInvertForDark(p, ref hi, ref lo);
            TryIconColorOverrides(it.Tag, p, ref hi, ref lo);

            Bitmap tinted = GetTintedCachedNormalized(src, hi, lo, ICON_NORM_BIAS);
            SafeAssignTinted(it, tinted);
        }



        private static void MaybeRecolorControlImages(Control c, Palette p, float gain = 1.6f, float bias = 0.10f)
        {
            if (c.BackgroundImage != null)
            {
                var srcBg = RememberOriginalControlBg(c);
                if (srcBg != null)
                {
                    Color hi = PickHiFixedForPalette(p);
                    Color lo = PickLowForPalette(p);
                    MaybeInvertForDark(p, ref hi, ref lo);
                    TryIconColorOverrides(c.Tag, p, ref hi, ref lo);

                    Bitmap tinted = GetTintedCachedNormalized(srcBg, hi, lo, ICON_NORM_BIAS);
                    if (_tintedBgByControl.TryGetValue(c, out var prev) && prev != null) { try { prev.Dispose(); } catch { } _tintedBgByControl.Remove(c); }
                    c.BackgroundImage = tinted;
                    _tintedBgByControl.Add(c, tinted);
                }
            }

            var btn = c as ButtonBase;

            if (btn != null && btn.Image != null)
            {
                var srcImg = RememberOriginalControlImg(btn, btn.Image);
                if (srcImg != null)
                {
                    Color hi = PickHiFixedForPalette(p);
                    Color lo = PickLowForPalette(p);
                    MaybeInvertForDark(p, ref hi, ref lo);
                    TryIconColorOverrides(c.Tag, p, ref hi, ref lo);

                    btn.Image = GetTintedCachedNormalized(srcImg, hi, lo, ICON_NORM_BIAS);
                }
            }

            var pb = c as PictureBox;
            if (pb != null && pb.Image != null)
            {
                var srcImg = RememberOriginalControlImg(pb, pb.Image);
                if (srcImg != null)
                {
                    Color hi = PickHiFixedForPalette(p);
                    Color lo = PickLowForPalette(p);
                    MaybeInvertForDark(p, ref hi, ref lo);
                    TryIconColorOverrides(c.Tag, p, ref hi, ref lo);

                    pb.Image = GetTintedCachedNormalized(srcImg, hi, lo, ICON_NORM_BIAS);
                }
            }
        }

        public static Bitmap DuoTone(Bitmap src, Color high, Color low, float gain = 1.4f, float bias = 0.08f)
        {
            var base32 = new Bitmap(src.Width, src.Height, PixelFormat.Format32bppArgb);
            using (var g = Graphics.FromImage(base32)) g.DrawImage(src, 0, 0);

            float wr = 0.2126f, wg = 0.7152f, wb = 0.0722f;
            float hr = high.R / 255f, hg = high.G / 255f, hb = high.B / 255f;
            float lr = low.R / 255f, lg = low.G / 255f, lb = low.B / 255f;

            float dR = hr - lr, dG = hg - lg, dB = hb - lb;

            var m = new ColorMatrix(new[]
            {
                new float[] { dR*gain*wr, dR*gain*wg, dR*gain*wb, 0, 0 },
                new float[] { dG*gain*wr, dG*gain*wg, dG*gain*wb, 0, 0 },
                new float[] { dB*gain*wr, dB*gain*wg, dB*gain*wb, 0, 0 },
                new float[] { 0,          0,          0,          1, 0 },
                new float[] { lr + dR*bias, lg + dG*bias, lb + dB*bias, 0, 1 }
            });

            using (var ia = new ImageAttributes())
            {
                ia.SetColorMatrix(m, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
                var dst = new Bitmap(base32.Width, base32.Height, PixelFormat.Format32bppArgb);
                using (var g2 = Graphics.FromImage(dst))
                    g2.DrawImage(base32, new Rectangle(0, 0, dst.Width, dst.Height),
                                 0, 0, base32.Width, base32.Height, GraphicsUnit.Pixel, ia);
                base32.Dispose();
                return dst;
            }
        }

        private static bool TryIconColorOverrides(object tag, Palette p, ref Color hi, ref Color lo)
        {
            var s = tag as string;
            if (s == null) return false;

            foreach (var token in s.Split(_tagSep, StringSplitOptions.RemoveEmptyEntries))
            {
                var kv = token.Split('=');
                if (kv.Length != 2) continue;
                var key = kv[0].Trim(); var val = kv[1].Trim();

                if (_ic.Equals(key, "IconHigh")) hi = ResolvePaletteColor(val, p);
                else if (_ic.Equals(key, "IconLow")) lo = ResolvePaletteColor(val, p);
            }
            return true;
        }

        private static Color ResolvePaletteColor(string val, Palette p)
        {
            if (!string.IsNullOrWhiteSpace(val) && val[0] == '#') return ColorTranslator.FromHtml(val);

            if (_ic.Equals(val, "Accent")) return p.Accent;
            if (_ic.Equals(val, "HighAccent")) return p.HighAccent;
            if (_ic.Equals(val, "LowAccent")) return p.LowAccent;
            if (_ic.Equals(val, "TextPrimary")) return p.TextPrimary;
            if (_ic.Equals(val, "TextSecondary")) return p.TextSecondary;
            if (_ic.Equals(val, "Border")) return p.Border;
            if (_ic.Equals(val, "Surface")) return p.SurfaceAlt;
            if (_ic.Equals(val, "SurfaceAlt")) return p.Surface;
            if (_ic.Equals(val, "Background")) return p.Background;
            if (_ic.Equals(val, "Complement")) return Complement(p.Accent);
            return p.TextSecondary;
        }

        private static Color Complement(Color c)
        {
            double h, s, l; RgbToHsl(c, out h, out s, out l);
            h = (h + 0.5) % 1.0; s = Math.Min(1.0, s * 0.8);
            return HslToRgb(h, s, l);
        }
        private static void RgbToHsl(Color c, out double h, out double s, out double l)
        {
            double r = c.R / 255.0, g = c.G / 255.0, b = c.B / 255.0;
            double max = Math.Max(r, Math.Max(g, b)), min = Math.Min(r, Math.Min(g, b));
            l = (max + min) / 2.0;
            if (max == min) { h = 0; s = 0; return; }
            double d = max - min;
            s = l > 0.5 ? d / (2.0 - max - min) : d / (max + min);
            if (max == r) h = (g - b) / d + (g < b ? 6 : 0);
            else if (max == g) h = (b - r) / d + 2;
            else h = (r - g) / d + 4;
            h /= 6.0;
        }
        private static double Hue2Rgb(double p, double q, double t)
        {
            if (t < 0) t += 1; if (t > 1) t -= 1;
            if (t < 1.0 / 6) return p + (q - p) * 6 * t;
            if (t < 1.0 / 2) return q;
            if (t < 2.0 / 3) return p + (q - p) * (2.0 / 3 - t) * 6;
            return p;
        }
        private static Color HslToRgb(double h, double s, double l)
        {
            double r, g, b;
            if (s == 0) { r = g = b = l; }
            else
            {
                double q = l < 0.5 ? l * (1 + s) : l + s - l * s;
                double p = 2 * l - q;
                r = Hue2Rgb(p, q, h + 1.0 / 3); g = Hue2Rgb(p, q, h); b = Hue2Rgb(p, q, h - 1.0 / 3);
            }
            return Color.FromArgb(255, (int)Math.Round(r * 255), (int)Math.Round(g * 255), (int)Math.Round(b * 255));
        }

        private static void ComputeLumaRange(Bitmap bmp, out float lmin, out float lmax)
        {
            const float wr = 0.2126f, wg = 0.7152f, wb = 0.0722f;
            lmin = 1f; lmax = 0f;

            var rect = new Rectangle(0, 0, bmp.Width, bmp.Height);
            var data = bmp.LockBits(rect, ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
            try
            {
                unsafe
                {
                    byte* row = (byte*)data.Scan0;
                    for (int y = 0; y < data.Height; y++, row += data.Stride)
                    {
                        byte* px = row;
                        for (int x = 0; x < data.Width; x++, px += 4)
                        {
                            byte a = px[3]; if (a == 0) continue;
                            float b = px[0] / 255f, g = px[1] / 255f, r = px[2] / 255f;
                            float l = r * wr + g * wg + b * wb;
                            if (l < lmin) lmin = l;
                            if (l > lmax) lmax = l;
                        }
                    }
                }
            }
            catch { }
            finally { bmp.UnlockBits(data); }

            if (lmax - lmin < 1e-3f) { lmin = 0f; lmax = 1f; }
        }

        // ============================
        // 12) PASADA SIMPLE PARA MENÚS DARK
        // ============================
        private static readonly ConditionalWeakTable<ToolStripDropDownItem, object> _wiredDropDowns =
            new ConditionalWeakTable<ToolStripDropDownItem, object>();
        private static readonly ConditionalWeakTable<ContextMenuStrip, object> _wiredContextMenus =
            new ConditionalWeakTable<ContextMenuStrip, object>();

        private static void SimpleDarkMenuPass(ToolStrip ts, Palette p, VisualDepth depth)
        {
            foreach (ToolStripItem it in ts.Items)
                if (it is ToolStripDropDownItem ddi)
                    WireAndStyleDropDown(ddi, p, depth);
        }

        private static void WireAndStyleDropDown(ToolStripDropDownItem ddi, Palette p, VisualDepth depth)
        {
            if (ddi == null) return;

            // Estilo inmediato si ya existe el menú
            if (ddi.DropDown is ToolStripDropDownMenu ddNow)
                StyleDropDownMenu(ddNow, p, depth);

            if (!_wiredDropDowns.TryGetValue(ddi, out _))
            {
                ddi.DropDownOpening += (s, e) =>
                {
                    var owner = (ToolStripDropDownItem)s;

                    //Tomar paleta/profundidad ACTUALES del Form
                    var frm = owner.Owner?.FindForm();
                    var pNow = GetPaletteFor(frm);
                    var dNow = GetCurrentDepth(frm);

                    if (owner.DropDown is ToolStripDropDownMenu dd)
                        StyleDropDownMenu(dd, pNow, dNow);
                };
                _wiredDropDowns.Add(ddi, new object());
            }

            // Cablear recursivamente los hijos (si el menú ya está materializado)
            if (ddi.DropDown is ToolStripDropDownMenu dd2)
                foreach (ToolStripItem item in dd2.Items)
                    if (item is ToolStripDropDownItem child)
                        WireAndStyleDropDown(child, p, depth);
        }

        private static void StyleDropDownMenu(ToolStripDropDownMenu dd, Palette p, VisualDepth depth)
        {
            if (dd == null) return;

            dd.Renderer = (depth == VisualDepth.ThreeD)
                ? (ToolStripProfessionalRenderer)new StripBackgroundRenderer3D(p)
                : new StripBackgroundRendererPlain(p);

            dd.BackColor = p.Background;
            dd.ForeColor = p.TextPrimary;

            foreach (ToolStripItem item in dd.Items)
            {
                item.BackColor = p.Background;
                item.ForeColor = item.Enabled ? p.TextPrimary : p.TextSecondary;

                // Recolorear ícono coherente con la paleta ACTUAL
                RecolorToolStripItem(item, p, 1.6f, 0.10f);

                // Cablear hijos si existen ya
                if (item is ToolStripDropDownItem child && child.DropDown is ToolStripDropDownMenu cdd)
                    StyleDropDownMenu(cdd, p, depth);
            }
        }

        private static void WireAndStyleContextMenu(ContextMenuStrip cms, Palette p, VisualDepth depth)
        {
            if (cms == null) return;

            // Estilo inmediato
            ApplyCmsStyle(cms, p, depth);

            if (!_wiredContextMenus.TryGetValue(cms, out _))
            {
                cms.Opening += (s, e) =>
                {
                    var cm = (ContextMenuStrip)s;
                    var frm = cm.SourceControl?.FindForm();

                    // 🔑 Paleta/profundidad ACTUALES
                    var pNow = GetPaletteFor(frm);
                    var dNow = GetCurrentDepth(frm);

                    ApplyCmsStyle(cm, pNow, dNow);
                };
                _wiredContextMenus.Add(cms, new object());
            }
        }

        private static void ApplyCmsStyle(ContextMenuStrip cms, Palette p, VisualDepth depth)
        {
            cms.Renderer = (depth == VisualDepth.ThreeD)
                ? (ToolStripProfessionalRenderer)new StripBackgroundRenderer3D(p)
                : new StripBackgroundRendererPlain(p);

            cms.BackColor = p.Background;
            cms.ForeColor = p.TextPrimary;

            foreach (ToolStripItem item in cms.Items)
            {
                item.BackColor = p.Background;
                item.ForeColor = item.Enabled ? p.TextPrimary : p.TextSecondary;
                RecolorToolStripItem(item, p, 1.6f, 0.10f);
            }
        }

        // ============================
        // 13) MOTOR DE BORDES (REDRAW BORDERS)
        // ============================

        // Almacenamos los dibujantes asociados a cada control para evitar duplicados y permitir limpieza
        private static readonly ConditionalWeakTable<Control, BorderDrawer> _borderDrawers =
            new ConditionalWeakTable<Control, BorderDrawer>();

        private static void AttachCustomBorder(Control c, Color color)
        {
            if (c == null || c.IsDisposed) return;

            // Si ya tiene uno, lo obtenemos para actualizar el color
            if (_borderDrawers.TryGetValue(c, out var existingDrawer))
            {
                existingDrawer.BorderColor = color;
                c.Invalidate(); // Fuerza repintado
                return;
            }

            // Si no, creamos uno nuevo
            var drawer = new BorderDrawer(c, color);
            _borderDrawers.Add(c, drawer);

            // Forzamos un redibujado inicial
            c.Invalidate();
        }

        /// <summary>
        /// Clase interna que se engancha al handle de Windows del control para interceptar WM_NCPAINT.
        /// Esto permite dibujar bordes de color en controles que normalmente no lo soportan (TextBox, ComboBox).
        /// </summary>
        private class BorderDrawer : NativeWindow, IDisposable
        {
            private readonly Control _control;
            public Color BorderColor { get; set; }

            // Constantes de mensajes de Windows
            private const int WM_NCPAINT = 0x85;  // Pintado del área no-cliente (bordes)
            private const int WM_PAINT = 0x0F;    // Pintado normal
            private const int WM_DESTROY = 0x02;  // Destrucción

            public BorderDrawer(Control control, Color color)
            {
                _control = control;
                BorderColor = color;

                // Nos enganchamos al Handle si ya existe, o esperamos a que se cree
                if (_control.Handle != IntPtr.Zero)
                    AssignHandle(_control.Handle);

                _control.HandleCreated += OnHandleCreated;
                _control.HandleDestroyed += OnHandleDestroyed;
            }

            private void OnHandleCreated(object sender, EventArgs e)
            {
                ReleaseHandle();
                AssignHandle(_control.Handle);
            }

            private void OnHandleDestroyed(object sender, EventArgs e)
            {
                ReleaseHandle();
            }

            // Interceptamos los mensabmlopjes del sistema
            protected override void WndProc(ref Message m)
            {
                base.WndProc(ref m); // Dejar que el control haga su trabajo primero

                if (m.Msg == WM_NCPAINT || m.Msg == WM_PAINT)
                {
                    DrawBorder();
                }
                else if (m.Msg == WM_DESTROY)
                {
                    Dispose();
                }
            }


            private void DrawBorder()
            {
                // Obtenemos el contexto gráfico de toda la ventana (incluyendo bordes)
                IntPtr hdc = GetWindowDC(this.Handle);

                if (hdc == IntPtr.Zero) return;

                try
                {
                    using (Graphics g = Graphics.FromHdc(hdc))// Ancho de 1px
                    using (Pen p = new Pen(BorderColor, 1))
                    {
                        // Dibujamos un rectángulo sobre el borde del control
                        // Restamos 1 al ancho/alto porque el dibujo es indexado en 0
                        g.DrawRectangle(p, 0, 0, _control.Width - 1, _control.Height - 1);
                    }
                }
                finally
                {
                    ReleaseDC(this.Handle, hdc);
                }
            }

            public void Dispose()
            {
                _control.HandleCreated -= OnHandleCreated;
                _control.HandleDestroyed -= OnHandleDestroyed;
                ReleaseHandle();
            }

            // Importaciones necesarias de la API de Windows (P/Invoke) para dibujar fuera del área cliente
            [System.Runtime.InteropServices.DllImport("User32.dll")]
            private static extern IntPtr GetWindowDC(IntPtr hWnd);

            [System.Runtime.InteropServices.DllImport("User32.dll")]
            private static extern int ReleaseDC(IntPtr hWnd, IntPtr hDC);
        }
    }
}