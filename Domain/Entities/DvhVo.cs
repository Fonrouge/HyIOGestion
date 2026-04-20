namespace Domain.Entities
{
    public class DvhVo : IValueObject
    {
        public object Value { get; }

        private DvhVo(string value)
        {
            Value = value;
        }

        public static DvhVo Create(string dvh)
        {
            // Validaciones (descomenta cuando estés listo)
            //   if (string.IsNullOrWhiteSpace(dvh))
            //throw new DomainException("El DVH no puede estar vacío.");


            return new DvhVo(dvh);
        }
    }
}