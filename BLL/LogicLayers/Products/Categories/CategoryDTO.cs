using SharedAbstractions.ArchitecturalMarkers;
using System;
using System.ComponentModel;

namespace BLL.LogicLayers
{
    public class CategoryDTO : IDto
    {
        [Browsable(false)]
        public Guid Id { get; set; } = Guid.Empty;
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; }

        [Browsable(false)]
        public string DVH { get; set; }

        public override string ToString() => Name;

    }
}
