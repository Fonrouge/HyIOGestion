using System;
using System.Collections.Generic;
using System.IO;

namespace WinformsUI.Infrastructure.Localization
{
    /// <summary>
    /// Servicio de localización que traduce textos dinámicamente entre idiomas
    /// utilizando archivos de recursos planos (.txt) con formato clave=valor.
    /// El idioma por default es español (es.txt), por tanto, corresponde a un nuevo archivo de idioma estar en formato "hola=HolaEnNuevoIdioma".
    /// </summary>
    public class LocalizationService : ILocalizationService
    {
        /// <summary>
        /// Traduce un valor original al idioma de destino especificado.
        /// Busca la clave correspondiente en el archivo de idioma base (es.txt),
        /// y luego obtiene la traducción desde el archivo del idioma destino.
        /// </summary>
        /// <param name="originalValue">Texto original en idioma base.</param>
        /// <param name="targetLanguage">Código del idioma destino (ej. "en").</param>
        /// <returns>Texto traducido si se encuentra; de lo contrario, devuelve el original.</returns>
        public string Translate(string originalValue, string targetLanguage)
        {
            //if (originalValue == "Clientes") 
            //    Debugger.Break();
            
            // Paso 1: identificar la "clave" original desde es.txt
            string basePath = AppDomain.CurrentDomain.BaseDirectory;
            string defaultLangPath = Path.Combine(basePath, "Infrastructure\\Localization", "es.txt");

            string key = FindKeyByValue(defaultLangPath, originalValue);


            if (key == null) return originalValue;

            // Paso 2: obtener la traducción usando esa clave en targetLanguage.txt
            string targetLangPath = Path.Combine(basePath, "Infrastructure\\Localization", $"{targetLanguage}.txt");
            string translated = FindValueByKey(targetLangPath, key);

            return translated ?? originalValue;
        }

        public List<LanguageInfo> GetAvailableLanguages()
        {
            var languages = new List<LanguageInfo>();
            string basePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Infrastructure\\Localization");

            if (!Directory.Exists(basePath)) return languages;

            // Buscamos todos los archivos .txt
            string[] files = Directory.GetFiles(basePath, "*.txt");

            foreach (var file in files)
            {
                string langCode = Path.GetFileNameWithoutExtension(file);
                string displayName = null;

                // Leemos el archivo buscando la etiqueta LanguageDisplayName
                foreach (var line in File.ReadLines(file))
                {
                    if (line.Contains("LanguageDisplayName="))
                    {
                        displayName = line.Split('=')[1].Trim();
                        break;
                    }
                }

                languages.Add(new LanguageInfo
                {
                    LangCode = langCode,
                    DisplayName = displayName ?? langCode // Fallback al código si no hay nombre
                });
            }

            return languages;
        }

        /// <summary>
        /// Busca la clave asociada a un valor en un archivo de idioma.
        /// </summary>
        /// <param name="filePath">Ruta del archivo de idioma base.</param>
        /// <param name="value">Valor textual a buscar.</param>
        /// <returns>Clave asociada si se encuentra; de lo contrario, null.</returns>
        private string FindKeyByValue(string filePath, string value)
        {
            if (!File.Exists(filePath)) 
                return null;

            foreach (var line in File.ReadLines(filePath))
            {
                var parts = line.Split('=');
                if (parts.Length == 2 && parts[1].Trim().Equals(value.Trim(), StringComparison.OrdinalIgnoreCase))
                    return parts[0].Trim();
            }
            return null;
        }

        /// <summary>
        /// Busca el valor traducido asociado a una clave en un archivo de idioma.
        /// </summary>
        /// <param name="filePath">Ruta del archivo del idioma destino.</param>
        /// <param name="key">Clave textual a buscar.</param>
        /// <returns>Valor traducido si se encuentra; de lo contrario, null.</returns>
        private string FindValueByKey(string filePath, string key)
        {
            if (!File.Exists(filePath)) return null;

            foreach (var line in File.ReadLines(filePath))
            {
                var parts = line.Split('=');
                if (parts.Length == 2 && parts[0].Trim().Equals(key.Trim(), StringComparison.OrdinalIgnoreCase))
                    return parts[1].Trim();
            }
            return null;
        }
    }
}