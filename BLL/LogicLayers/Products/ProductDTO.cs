using Domain.Entities;
using SharedAbstractions.ArchitecturalMarkers;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace BLL.LogicLayers
{
    public class ProductDTO : IDto
    {
        [Browsable(false)]
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public List<CategoryDTO> Categories { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
