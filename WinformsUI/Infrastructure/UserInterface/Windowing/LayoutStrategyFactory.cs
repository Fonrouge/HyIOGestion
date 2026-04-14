using Shared.Enums;
using System.Collections.Generic;

namespace WinformsUI.Infrastructure.UserInterface.Windowing
{
    public class LayoutStrategyFactory : ILayoutStrategyFactory
    {

        private readonly Dictionary<LayoutTypeEnum, ILayoutStrategy> _strategies;

        public LayoutStrategyFactory()
        {
            _strategies = new Dictionary<LayoutTypeEnum, ILayoutStrategy>
            {
                { LayoutTypeEnum.Cascade, new CascadeStrategy() },
                { LayoutTypeEnum.VerticalTile, new VerticalTileStrategy() },
                { LayoutTypeEnum.SmartGrid, new SmartGridStrategy() }
            };
        }

        public ILayoutStrategy Create(LayoutTypeEnum type)
        {
            if (_strategies.TryGetValue(type, out var strategy))
            {
                return strategy;
            }

            return _strategies[LayoutTypeEnum.VerticalTile];
        }
    }
}
