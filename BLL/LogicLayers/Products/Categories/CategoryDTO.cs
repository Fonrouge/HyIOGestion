using SharedAbstractions.ArchitecturalMarkers;
using System;

namespace BLL.LogicLayers
{
    public class CategoryDTO : IDto
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; }
    }
}
