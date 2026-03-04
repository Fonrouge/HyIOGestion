using Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinformsUI.Infrastructure.UserInterface.Windowing
{
    public class LayoutStrategyFactory : ILayoutStrategyFactory
    {

        private readonly Dictionary<LayoutType, ILayoutStrategy> _strategies;

        public LayoutStrategyFactory()
        {
            _strategies = new Dictionary<LayoutType, ILayoutStrategy>
            {
                { LayoutType.Cascade, new CascadeStrategy() },
                { LayoutType.VerticalTile, new VerticalTileStrategy() },
                { LayoutType.SmartGrid, new SmartGridStrategy() }
            };
        }

        public ILayoutStrategy Create(LayoutType type)
        {
            if (_strategies.TryGetValue(type, out var strategy))
            {
                return strategy;
            }

            return _strategies[LayoutType.VerticalTile];
        }
    }
}
