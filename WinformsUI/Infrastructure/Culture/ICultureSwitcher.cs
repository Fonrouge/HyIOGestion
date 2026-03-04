using System.Globalization;

namespace WinformsUI.Infrastructure.Culture
{
    /// <summary>
    /// Contrato para la gestión del perfil cultural de la interfaz de usuario.
    /// Permite establecer la cultura visual del sistema (idioma, formato) y recuperar la configuración de persistencia.
    /// </summary>
    /// <remarks>
    /// Esta interfaz abstrae la lógica para definir el contexto cultural de presentación, incluyendo idioma,
    /// formatos de fecha/hora y estilo de la UI. Es especialmente útil en entornos multilingües o donde se requiere
    /// consistencia cultural entre capas técnicas y de usuario.
    /// </remarks>
    public interface ICultureSwitcher
    {
        /// <summary>
        /// Establece la cultura visual de la interfaz de usuario.
        /// </summary>
        /// <param name="cultureCode">
        /// Código de cultura (por ejemplo, <c>"es-AR"</c>, <c>"en-US"</c>) compatible con <see cref="CultureInfo"/>.
        /// </param>
        void SetUICulture(string userInterfaceCultCode);

        /// <summary>
        /// Obtiene la cultura que debe utilizarse para persistir datos sensibles a formato (fechas, números, etc.).
        /// </summary>
        /// <returns>
        /// Instancia de <see cref="CultureInfo"/> definida como cultura de persistencia.
        /// </returns>
        CultureInfo GetPersistenceCulture();


        CultureInfo GetCurrentUICulture();
    }
}