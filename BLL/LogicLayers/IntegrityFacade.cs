using Domain.Contracts;
using Domain.Entities;
using Shared.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BLL.LogicLayers
{
    public static class IntegrityFacade
    {
        public static void RecalculateAndSetEntityDVH<T>(T entity) where T : IIntegrityCheckable
        {
            entity.UpdateDVH(IntegrityService.GetIntegrityHash(entity.GetDvhSerialization()));
        }

        public static string ProductsCategoriesDVHCalculator(Guid idPadre, Guid idHijo)
        {
            // Concatenamos los IDs para crear la "semilla" del hash
            string data = $"{idPadre}|{idHijo}";
            return IntegrityService.GetIntegrityHash(data);
        }


        public static string GetDelta(object original, object dto, params string[] ignoreProperties)
        {
            var changes = new List<string>();
            var dtoProps = dto.GetType().GetProperties();
            var entityProps = original.GetType().GetProperties();

            foreach (var dtoProp in dtoProps)
            {
                // 1. Saltamos si la propiedad está en la lista de ignorados
                if (ignoreProperties.Contains(dtoProp.Name)) continue;

                // 2. Buscamos la propiedad espejo en la entidad
                var entityProp = entityProps.FirstOrDefault(p => p.Name == dtoProp.Name);
                if (entityProp == null) continue;

                var newValue = dtoProp.GetValue(dto);
                var originalValue = entityProp.GetValue(original);

                // 3. Extraemos el valor si es un Value Object
                if (originalValue is IValueObject vo)
                {
                    originalValue = vo.Value;
                }

                // 4. Comparamos
                if (!Equals(originalValue?.ToString(), newValue?.ToString()))
                {
                    changes.Add($"{dtoProp.Name}: [{originalValue ?? "NULO"}] -> [{newValue ?? "NULO"}]");
                }
            }

            return changes.Any() ? string.Join(" | ", changes) : "Sin cambios detectados";
        }
    }
}
