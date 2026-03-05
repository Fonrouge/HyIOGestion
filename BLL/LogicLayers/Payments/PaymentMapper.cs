using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BLL.LogicLayers
{
    public static class PaymentMapper
    {
        public static PaymentDTO ToDto(Payment payment)
        {
            if (payment == null) return null;

            return new PaymentDTO
            {
                Id = payment.Id,
                // Extraemos el valor primitivo (decimal) del VO
                Amount = payment.Amount?.Value ?? 0m,
                CreationDate = payment.CreationDate,
                EffectiveDate = payment.EffectiveDate,
                ClientId = payment.ClientId,
                Method = payment.Method?.Value,
                Reference = payment.Reference?.Value
            };
        }

        public static Payment ToEntity(PaymentDTO paymentDto)
        {
            if (paymentDto == null) return null;

            // Reconstituimos usando los tipos nativos que ahora tiene el DTO
            return Payment.Reconstitute
            (
                id: paymentDto.Id,
                rawAmount: paymentDto.Amount,
                creationDate: paymentDto.CreationDate,
                effectiveDate: paymentDto.EffectiveDate,
                clientId: paymentDto.ClientId,
                rawMethod: paymentDto.Method,
                rawReference: paymentDto.Reference,
                dvh: string.Empty // Se calcula en la BLL
            );
        }

        public static IEnumerable<PaymentDTO> ToListDto(IEnumerable<Payment> payments)
        {
            if (payments == null) return Enumerable.Empty<PaymentDTO>();
            return payments.Select(ToDto).ToList();
        }

        public static IEnumerable<Payment> ToListEntity(IEnumerable<PaymentDTO> paymentDtos)
        {
            if (paymentDtos == null) return Enumerable.Empty<Payment>();
            return paymentDtos.Select(ToEntity).ToList();
        }
    }
}