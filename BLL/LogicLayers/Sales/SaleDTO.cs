using BLL.DTOs;
using Domain.Entities;
using SharedAbstractions.ArchitecturalMarkers;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace BLL.LogicLayers
{
    public class SaleDTO : IDto
    {
        [Browsable(false)]
        public Guid Id { get; set; } = Guid.NewGuid();
        
        
        [Browsable(false)]
        public Guid ClientId { get; set; }
        public ClientDTO Client { get; set; }

        
        [Browsable(false)]
        public Guid EmployeeId { get; set; } 
        public EmployeeDTO Employee { get; set; }


        public DateTime Date { get; set; } = DateTime.UtcNow;                
        public decimal TotalAmount { get; set; }


        public IEnumerable<SaleDetailDTO> Items { get; set; }
    }
}
