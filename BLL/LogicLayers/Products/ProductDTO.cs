using BLL.LogicLayers;
using SharedAbstractions.ArchitecturalMarkers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

public class ProductDTO : IDto
{
    [Browsable(false)]
    public Guid Id { get; set; } = Guid.Empty;

    public string  Name { get; set; } = string.Empty;
    public string  Description { get; set; }
    public decimal Price { get; set; }

    // Corregido a decimal para mantener coherencia con el dominio
    public decimal Stock { get; set; }

    public List<CategoryDTO> Categories { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // --- CAMPOS DE CONTROL (Sincronizados con la Entidad) ---
    [Browsable(false)]
    public bool IsDeleted { get; set; }

    [Browsable(false)]
    public string DVH { get; set; }

    public string CategoriesName
    {
        get
        {
            if (Categories == null || !Categories.Any()) return "Sin categorizar";
            return string.Join(", ", Categories.Select(c => c.Name));
        }
    }
}