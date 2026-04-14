using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using WinformsUI.Infrastructure.Culture;
using WinformsUI.Infrastructure.Localization;
using WinformsUI.UserControls.CustomDGV;

namespace WinformsUI.Infrastructure.Translations
{
    public class TranslationsManager : ITranslatableControlsManager
    {
        private readonly ILocalizationService _loc;
        private readonly ICultureSwitcher _culture;

        /// Mapa (objeto, propiedad) -> texto base ES (capturado una única vez).
        private readonly Dictionary<ObjectPropertyKey, string> _baseES =
            new Dictionary<ObjectPropertyKey, string>(new ObjectPropertyKeyComparer());

        /// id -> texto base ES
        private readonly Dictionary<string, string> _baseStrings =
            new Dictionary<string, string>(StringComparer.Ordinal);

        /// id -> lista de destinos (setter) donde aplicar la traducción al hacer Apply()
        private readonly Dictionary<string, List<(ISynchronizeInvoke sync, Action<string> setter)>> _stringBindings =
            new Dictionary<string, List<(ISynchronizeInvoke, Action<string>)>>(StringComparer.Ordinal);
        
        // ===== ListBox support =====
        private sealed class ListBoxSnapshot
        {
            public ListBox ListBox;                 // referencia al control
            public bool IsDataBound;                // true si usa DataSource
            public string DisplayMember;            // si data-bound
            public string ValueMember;              // si data-bound
            public List<string> BaseEsTexts;        // ES base de cada item (lo que se traduce)
            public List<object> Values;             // Value (objeto) por item si aplica; si no, nulls
        }

        private readonly Dictionary<ListBox, ListBoxSnapshot> _listBoxes =
            new Dictionary<ListBox, ListBoxSnapshot>(new ReferenceEqualityComparer<ListBox>());

        public List<LanguageInfo> GetAvailableLanguages()
        {
            return _loc.GetAvailableLanguages();
        }

        private sealed class ReferenceEqualityComparer<T> : IEqualityComparer<T> where T : class
        {
            public bool Equals(T x, T y) => ReferenceEquals(x, y);
            public int GetHashCode(T obj) => RuntimeHelpers.GetHashCode(obj);
        }

        public TranslationsManager(ILocalizationService loc, ICultureSwitcher culture)
        {
            _loc = loc ?? throw new ArgumentNullException(nameof(loc));
            _culture = culture ?? throw new ArgumentNullException(nameof(culture));
        }


        // ---------- Registro de objetos/propiedades ----------

        private readonly List<Form> _registeredForms = new List<Form>();

        private readonly Dictionary<Type, Action<Form>> _notifyCache
            = new Dictionary<Type, Action<Form>>();

        public void AddFormNotify(object frm)
        {
            if (frm == null) throw new ArgumentNullException(nameof(frm));

            if (frm is Form f)
                _registeredForms.Add(f);
        }

        public void RemoveFormNotify(object frm)
        {
            if (frm == null) throw new ArgumentNullException(nameof(frm));

            if (frm is Form f)
                _registeredForms.Remove(f);
        }

        public void Notify()
        {
            // Tomar snapshot por si durante la notificación se agregan/sacan forms
            var snapshot = _registeredForms.ToArray();

            foreach (var form in snapshot)
            {
                if (form == null || form.IsDisposed)
                    continue;

                NotifyForm(form);
            }
        }

        private void NotifyForm(Form form)
        {
            var type = form.GetType();

            if (!_notifyCache.TryGetValue(type, out var notifier))
            {
                // Buscar método GetNotified (público o privado, instancia, sin parámetros)
                var mi = type.GetMethod(
                    "NotifiedByTranslationManager",
                    BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,
                    binder: null,
                    types: Type.EmptyTypes,
                    modifiers: null);

                if (mi == null)
                {
                    // Si no tiene GetNotified, cacheamos un no-op
                    notifier = f => { };
                }
                else
                {
                    // Creamos el "invocador" y lo cacheamos
                    notifier = f => mi.Invoke(f, null);
                }

                if (form is CustomDGVForm)
                {
                    //     Debugger.Break();
                }

                _notifyCache[type] = notifier;
            }

            if (notifier != null)
                notifier(form);

        }
        public void AddSingleObject(object target, string propertyName)
        {
            if (target is null || string.IsNullOrWhiteSpace(propertyName)) return;

            var prop = GetReadableProperty(target, propertyName);
            if (prop is null) return;

            var key = new ObjectPropertyKey(target, propertyName);

            if (!_baseES.ContainsKey(key))
            {
                var current = prop.GetValue(target)?.ToString() ?? string.Empty;
                _baseES[key] = current;
            }
        }

        public void AddSingleObject(object target, string propertyName, string baseSpanishValue)
        {
            if (target is null || string.IsNullOrWhiteSpace(propertyName)) return;

            var prop = GetReadableProperty(target, propertyName);
            if (prop is null) return;

            var key = new ObjectPropertyKey(target, propertyName);
            _baseES[key] = baseSpanishValue ?? string.Empty;
        }


        /// <summary>Registra/actualiza un string base por id (ES).</summary>
        public void AddString(string id, string baseSpanishValue, bool overwrite = true)
        {
            if (string.IsNullOrWhiteSpace(id)) return;
            if (!_baseStrings.ContainsKey(id))
                _baseStrings[id] = baseSpanishValue ?? string.Empty;
            else if (overwrite)
                _baseStrings[id] = baseSpanishValue ?? string.Empty;


        }

        /// <summary>Obtiene la traducción actual del string (si no existe, devuelve el id).</summary>
        public string GetString(string id)
        {
            if (string.IsNullOrWhiteSpace(id)) return string.Empty;
            if (!_baseStrings.TryGetValue(id, out var baseEs)) return id;
            var lang = _culture.GetCurrentUICulture().Name;
            return _loc.Translate(baseEs, lang) ?? baseEs;
        }

        /// <summary>Vincula un destino para que reciba la traducción en Apply().</summary>
        public void AddStringBinding(string id, Action<string> setter) =>
            AddStringBinding(null, id, setter);

        /// <summary>Vincula un destino con marshaling seguro al hilo UI.</summary>
        public void AddStringBinding(ISynchronizeInvoke sync, string id, Action<string> setter)
        {
            if (string.IsNullOrWhiteSpace(id) || setter is null) return;
            if (!_stringBindings.TryGetValue(id, out var list))
            {
                list = new List<(ISynchronizeInvoke, Action<string>)>();
                _stringBindings[id] = list;
            }
            list.Add((sync, setter));
        }

        /// <summary>Elimina un string registrado por id.</summary>
        public void RemoveString(string id)
        {
            if (string.IsNullOrWhiteSpace(id)) return;
            _baseStrings.Remove(id);
            _stringBindings.Remove(id);

        }

        /// <summary>Limpia todos los strings registrados.</summary>
        public void ClearStrings()
        {
            _baseStrings.Clear();
            _stringBindings.Clear();
        }
        


        // ---------- Aplicación de traducciones ----------

        public void Apply()
        {
            var lang = _culture.GetCurrentUICulture().Name;

            // 1) Propiedades registradas (controles/columnas/etc.)
            foreach (var kv in new List<KeyValuePair<ObjectPropertyKey, string>>(_baseES))
            {
                var (target, property) = kv.Key;
                var baseSpanish = kv.Value;


                var prop = GetWritableProperty(target, property);
                if (prop is null) continue;

                var translated = _loc.Translate(baseSpanish, lang);
                          

                if (target is ISynchronizeInvoke sync && sync.InvokeRequired)
                {
                    try { sync.BeginInvoke(new Action(() => SafeSetValue(prop, target, translated)), null); }
                    catch { /* control disposed, ignorar */ }
                }
                else
                {
                    SafeSetValue(prop, target, translated);
                }
            }

            // 2) Strings sueltos con bindings
            foreach (var kv in new List<KeyValuePair<string, List<(ISynchronizeInvoke sync, Action<string> setter)>>>(_stringBindings))
            {
                var id = kv.Key;
                var setters = kv.Value;
                var text = GetString(id); // ya pasa por _loc

                foreach (var (sync, setter) in setters)
                {
                    if (sync != null && sync.InvokeRequired)
                    {
                        try { sync.BeginInvoke(setter, new object[] { text }); }
                        catch { /* destino disposed, ignorar */ }
                    }
                    else
                    {
                        setter(text);
                    }
                }
            }

            // 3) ListBox: rebind con textos traducidos
            if (_listBoxes.Count > 0)
            {
                var lan = _culture.GetCurrentUICulture().Name;

                foreach (var kv in _listBoxes)
                {
                    var lb = kv.Key;
                    var snap = kv.Value;
                    if (lb.IsDisposed) continue;

                    Action rebind = () =>
                    {
                        var selectedIndex = lb.SelectedIndex; // preservamos selección

                        // Construimos la lista traducida
                        var translated = new List<string>(snap.BaseEsTexts.Count);

                        foreach (var es in snap.BaseEsTexts)
                            translated.Add(_loc.Translate(es ?? string.Empty, lan) ?? (es ?? string.Empty));

                        lb.BeginUpdate();

                        try
                        {
                            if (!snap.IsDataBound)
                            {
                                // Items sueltos: reemplazamos por strings traducidos
                                lb.DataSource = null;
                                lb.Items.Clear();
                                lb.Items.AddRange(translated.Cast<object>().ToArray());
                            }
                            else
                            {
                                // Data-bound: generamos pares (Display, Value) y rebind
                                var pairs = new List<DisplayValuePair>(translated.Count);

                                for (int i = 0; i < translated.Count; i++)
                                    pairs.Add(new DisplayValuePair { Display = translated[i], Value = snap.Values[i] });

                                lb.DataSource = null;
                                lb.DisplayMember = nameof(DisplayValuePair.Display);
                                lb.ValueMember = nameof(DisplayValuePair.Value);
                                lb.DataSource = pairs;
                            }

                            // restaurar selección de ser posible (índice de donde el usuario estaba parado en el listbox)
                            if (selectedIndex >= 0 && selectedIndex < lb.Items.Count)
                                lb.SelectedIndex = selectedIndex;
                            else if (lb.Items.Count > 0)
                                lb.SelectedIndex = 0;
                        }
                        finally { lb.EndUpdate(); }
                    };

                    if (lb is ISynchronizeInvoke sync && sync.InvokeRequired)
                    {
                        try { sync.BeginInvoke(rebind, null); } catch { /* control disposed */ }
                    }
                    else
                    {
                        rebind();
                    }
                }
            }
        }

        private sealed class DisplayValuePair
        {
            public string Display { get; set; }
            public object Value { get; set; }
        }


        // ---------- Helpers de registro masivo ----------

        public void AddParentedObjects<T>(Control.ControlCollection controls, string property) where T : Control
        {
            if (controls is null) return;

            foreach (Control c in controls)
            {
                if (c is T)
                {
                    // Caso especial: si T es ListBox, registramos sus ítems en lugar de una propiedad simple
                    if (c is ListBox lb)
                        RegisterListBox(lb);

                    else
                        AddSingleObject(c, property);
                }
            }

            TranslateControlsRecursive<T>(controls, property);
        }
        public void AddDataGridViewColumns(DataGridView dgv)
        {
            if (dgv is null) return;

            foreach (DataGridViewColumn col in dgv.Columns)
                AddSingleObject(col, nameof(DataGridViewColumn.HeaderText));
        }

        public void RefreshDataGridViewColumns(DataGridView dgv)
        {
            if (dgv is null) return;

            var toRemove = new List<ObjectPropertyKey>();

            foreach (var key in _baseES.Keys)
            {


                if (key.Target is DataGridViewColumn col && ReferenceEquals(col.DataGridView, dgv))
                    toRemove.Add(key);
            }

            foreach (var k in toRemove) _baseES.Remove(k);

            AddDataGridViewColumns(dgv);
        }




        // ---------- Internals ----------

        private static PropertyInfo GetReadableProperty(object target, string propertyName)
        {
            var prop = target.GetType().GetProperty(propertyName, BindingFlags.Instance | BindingFlags.Public);
            return (prop != null && prop.CanRead) ? prop : null;
        }

        private static PropertyInfo GetWritableProperty(object target, string propertyName)
        {
            var prop = target.GetType().GetProperty(propertyName, BindingFlags.Instance | BindingFlags.Public);
            return (prop != null && prop.CanWrite) ? prop : null;
        }

        private static void SafeSetValue(PropertyInfo prop, object target, string value)
        {
            try { prop.SetValue(target, value); }
            catch { /* propiedad no seteable en runtime, ignorar */ }
        }

        private void TranslateControlsRecursive<T>(Control.ControlCollection controls, string property) where T : Control
        {
            foreach (Control c in controls)
            {
                if (c is T)
                {
                    if (c is ListBox lb)
                        RegisterListBox(lb);
                    else
                        AddSingleObject(c, property);
                }
                if (c.HasChildren) TranslateControlsRecursive<T>(c.Controls, property);
            }
        }

        private void RegisterListBox(ListBox lb)
        {
            if (lb == null) return;

            var snap = new ListBoxSnapshot
            {
                ListBox = lb,
                IsDataBound = lb.DataSource != null,
                DisplayMember = lb.DisplayMember,
                ValueMember = lb.ValueMember,
                BaseEsTexts = new List<string>(),
                Values = new List<object>()
            };

            if (!snap.IsDataBound)
            {
                // Ítems sueltos (se asume string.ToString() como ES base; tener en cuenta que la app está por default en español)
                foreach (var it in lb.Items)
                {
                    var es = it?.ToString() ?? string.Empty;
                    snap.BaseEsTexts.Add(es);
                    snap.Values.Add(null);
                }
            }
            else
            {
                // Data-bound: extraemos la lista enumerable y sus textos base por DisplayMember (o ToString)
                var enumerable = (lb.DataSource as System.ComponentModel.IListSource)?.GetList()
                                 ?? lb.DataSource as System.Collections.IEnumerable;

                if (enumerable != null)
                {
                    var dispName = snap.DisplayMember;
                    var valName = snap.ValueMember;

                    foreach (var it in enumerable)
                    {
                        // Display base ES
                        string es;
                        if (!string.IsNullOrWhiteSpace(dispName))
                            es = GetMemberString(it, dispName);
                        else
                            es = it?.ToString() ?? string.Empty;

                        snap.BaseEsTexts.Add(es);

                        // Value (si hay ValueMember); si no, guardamos el mismo item
                        object val = null;
                        if (!string.IsNullOrWhiteSpace(valName))
                            val = GetMemberObject(it, valName);
                        else
                            val = it;

                        snap.Values.Add(val);
                    }
                }
            }

            _listBoxes[lb] = snap;
        }

        private static string GetMemberString(object obj, string memberName)
        {
            if (obj == null) return string.Empty;
            if (string.IsNullOrWhiteSpace(memberName)) return obj.ToString() ?? string.Empty;

            var t = obj.GetType();
            var pi = t.GetProperty(memberName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.IgnoreCase);

            if (pi != null)
            {
                var v = pi.GetValue(obj);
                return v?.ToString() ?? string.Empty;
            }

            var fi = t.GetField(memberName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.IgnoreCase);
            if (fi != null)
            {
                var v = fi.GetValue(obj);
                return v?.ToString() ?? string.Empty;
            }

            return obj.ToString() ?? string.Empty;
        }

        private static object GetMemberObject(object obj, string memberName)
        {
            if (obj == null) return null;
            if (string.IsNullOrWhiteSpace(memberName)) return obj;

            var t = obj.GetType();
            var pi = t.GetProperty(memberName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.IgnoreCase);
            if (pi != null) return pi.GetValue(obj);

            var fi = t.GetField(memberName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.IgnoreCase);
            if (fi != null) return fi.GetValue(obj);

            return obj;
        }


        private readonly struct ObjectPropertyKey
        {
            public readonly object Target;
            public readonly string PropertyName;
            public ObjectPropertyKey(object target, string propertyName) { Target = target; PropertyName = propertyName ?? string.Empty; }
            public void Deconstruct(out object target, out string propertyName) { target = Target; propertyName = PropertyName; }
        }

        private sealed class ObjectPropertyKeyComparer : IEqualityComparer<ObjectPropertyKey>
        {
            public bool Equals(ObjectPropertyKey x, ObjectPropertyKey y) =>
                ReferenceEquals(x.Target, y.Target) &&
                string.Equals(x.PropertyName, y.PropertyName, StringComparison.Ordinal);

            public int GetHashCode(ObjectPropertyKey obj) =>
                (RuntimeHelpers.GetHashCode(obj.Target) * 397) ^ (obj.PropertyName?.GetHashCode() ?? 0);
        }
      
        // Método para desuscribir un objeto específico de TODOS los strings que esté escuchando,
        // o de uno específico si pasas el id.
        public void UnsubscribeTarget(ISynchronizeInvoke target, string specificId = null)
        {
            if (target == null) return;

            // Si pasamos un ID específico, atacamos solo esa lista
            if (!string.IsNullOrEmpty(specificId))
            {
                if (_stringBindings.TryGetValue(specificId, out var list))
                {
                    // Borramos solo las tuplas que pertenecen a este 'target'
                    list.RemoveAll(x => ReferenceEquals(x.sync, target));

                    // Opcional: Si la lista queda vacía, borramos la entrada del diccionario
                    if (list.Count == 0) _baseStrings.Remove(specificId);
                }
                return;
            }

            // Si no pasamos ID (limpieza general al cerrar form), barremos todo
            // Esto es menos performante (O(N)) pero seguro para el FormClosed
            var keys = _stringBindings.Keys.ToList();

            foreach (var key in keys)
            {
                var list = _stringBindings[key];
                list.RemoveAll(x => ReferenceEquals(x.sync, target));

                // Limpieza de claves huérfanas
                if (list.Count == 0)
                {
                    _stringBindings.Remove(key);
                    // Opcional: _baseStrings.Remove(key); // Depende si quieres cachear el texto base
                }
            }
        }
    }

}