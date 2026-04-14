using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using WinformsUI.Infrastructure.Localization;

namespace WinformsUI.Infrastructure.Translations
{
    /// <summary>
    /// Servicio de traducción de propiedades de objetos WinForms y de strings sueltos.
    /// Primero se registran los textos base (idealmente en ES) y luego se aplican según la cultura actual.
    /// </summary>
    public interface ITranslatableControlsManager
    {
        // -------- Registro de objetos/propiedades --------

        /// <summary>Registra un objeto y la propiedad a traducir tomando su valor actual como base ES.</summary>
        void AddSingleObject(object target, string propertyName);

        /// <summary>Registra un objeto y la propiedad a traducir tomando su valor actual como base ES.
        /// Con registrar el Form mediante este mismo método e inlcuir un método "NotifiedByTranslationManager() {}" para elegir los comportamientos que tendrá el formulario,
        /// ya estará en condiciones de ser notificado.</summary>
        void AddFormNotify(object frm);

        /// <summary>Registra un objeto y la propiedad a traducir tomando su valor actual como base ES.</summary>
        void RemoveFormNotify(object frm);
        void Notify();

        /// <summary>Registra un objeto y propiedad indicando explícitamente el texto base ES.</summary>
        void AddSingleObject(object target, string propertyName, string baseSpanishValue);

        /// <summary>Registra en cascada todos los controles de tipo T dentro de un contenedor para la propiedad indicada (p.ej. "Text").</summary>
        void AddParentedObjects<T>(Control.ControlCollection controls, string property) where T : Control;

        /// <summary>Registra todas las columnas actuales de un DataGridView (HeaderText como base ES).</summary>
        void AddDataGridViewColumns(DataGridView dgv);

        /// <summary>Vuelve a registrar las columnas del DataGridView (útil cuando se recrean dinámicamente).</summary>
        void RefreshDataGridViewColumns(DataGridView dgv);

        // -------- Strings sueltos --------

        /// <summary>Registra/actualiza un string base por id (en ES). <paramref name="overwrite"/> controla si se sobrescribe.</summary>
        void AddString(string id, string baseSpanishValue, bool overwrite = true);

        /// <summary>Obtiene el string traducido al idioma actual; si no existe el id, devuelve el id.</summary>
        string GetString(string id);

        /// <summary>Vincula un destino (setter) para que reciba la traducción del id en cada Apply().</summary>
        void AddStringBinding(string id, Action<string> setter);

        /// <summary>Vincula un destino con marshaling seguro al hilo UI.</summary>
        void AddStringBinding(ISynchronizeInvoke sync, string id, Action<string> setter);

        /// <summary>Elimina un string registrado por id (y sus bindings).</summary>
        void RemoveString(string id);

        /// <summary>Limpia todos los strings registrados y sus bindings.</summary>
        void ClearStrings();

        // -------- Aplicación --------

        /// <summary>Aplica las traducciones a todos los objetos y strings vinculados según la cultura actual.</summary>
        void Apply();

        void UnsubscribeTarget(ISynchronizeInvoke target, string specificId = null);
        List<LanguageInfo> GetAvailableLanguages();


    }
}
