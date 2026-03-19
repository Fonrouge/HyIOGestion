using Shared.Enums;

namespace WinformsUI.Infrastructure.UserInterface.Windowing
{
    public interface ILayoutStrategyFactory
    {
        /// <summary>
        /// Obtiene la estrategia de layout correspondiente al tipo solicitado.
        /// </summary>
        ILayoutStrategy Create(LayoutTypeEnum type);
    }
}
