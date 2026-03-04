using Shared.ArchitecturalMarkers;
using System;
using System.Drawing;
using System.Windows.Forms;
using Winforms.Theme;
using WinformsUI.Forms.Main;
using static Winforms.Theme.DarkTheme;

namespace WinformsUI.Infrastructure
{
    /// <summary>
    /// Encapsula el contexto de infraestructura de la interfaz de usuario (UI Context).
    /// <br/>
    /// Este objeto implementa el patrón <i>Parameter Object</i> para agrupar y transportar
    /// las dependencias estructurales (contenedor, menú) y visuales (paleta) 
    /// necesarias para inicializar los formularios hijos en el AppShell.
    /// </summary>
    public class AppEnvironment : IAppEnvironment
    {
        /// <summary>
        /// Obtiene la configuración de colores y estilos visuales activa.
        /// Utilizada por el motor de renderizado para aplicar el tema a los controles.
        /// </summary>
        public Palette InternalPalette { get; private set; }

        /// <summary>
        /// Referencia al contenedor principal (Dashboard) donde se renderizarán 
        /// y gestionarán las ventanas hijas o módulos de la aplicación.
        /// </summary>
        public Panel DashboardContainer { get; private set; }

        /// <summary>
        /// Referencia al panel de flujo lateral destinado a alojar menús contextuales,
        /// herramientas o las representaciones minimizadas de los formularios activos.
        /// </summary>
        public FlowLayoutPanel RightMenu { get; private set; }

        /// <summary>
        /// Parámetro marcado por defecto como "Módulo" que representa al tipo de formuarlio
        /// para que HostForm sepa cómo debe comportarse.
        /// </summary>
        public FormTypeEnum FormType { get; private set; }


        /// <summary>
        /// Parámetro optativo para ícono que representa al módulo o formulario en caso de ser necesario
        /// para poder mostrar al momento de minizar o agregar a la Custom Title Bar en un futuro. 
        /// </summary>
        public Image ModuleIcon { get; private set; }

        public bool IsInitialized { get; private set; } = false;

        /// <summary>
        /// Inicializa una nueva instancia del entorno de la aplicación.
        /// </summary>
        /// <param name="InternalPalette">La paleta de colores definida por el <see cref="Winforms.Theme.DarkTheme"/>.</param>
        /// <param name="Dashboard">El panel contenedor principal que actuará como host de los formularios.</param>
        /// <param name="RightMenu">El panel lateral para la gestión de ítems minimizados o herramientas.</param>

        public AppEnvironment() { }

        public void Initialize(object dashBoard, object rightToolBar, object palette, int formType, object icon = null)
        {
            if (IsInitialized) return;

            // 1. Casteo de Contenedores (UI-Specific)
            this.DashboardContainer = dashBoard as Panel ?? throw new ArgumentException("El DashBoard proporcionado no es un Panel de WinForms.");

            this.RightMenu = rightToolBar as FlowLayoutPanel ?? throw new ArgumentException("El RightToolBar proporcionado no es un FlowLayoutPanel.");

            // 2. Manejo de la Paleta (Agnóstico mediante object)
            if (palette is Palette p)
                this.InternalPalette = p;

            else
                throw new ArgumentException("La paleta proporcionada no es del tipo esperado (Palette).");


            // 3. Conversión de Tipos Primitivos a Enums de Dominio
            this.FormType = (FormTypeEnum)formType;

            // 4. Manejo del Icono (Viene como object para no referenciar System.Drawing en la interfaz)
            if (icon != null)
                this.ModuleIcon = icon as Image ?? throw new ArgumentException("El objeto icon no es una instancia de System.Drawing.Image.");


            this.IsInitialized = true;
        }

    }
}