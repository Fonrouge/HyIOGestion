namespace Domain.Entities
{
    public class DvhVo : IValueObject
    {
        public string Value { get; }

        private DvhVo(string value)
        {
            Value = value;
        }

        public static DvhVo Create(string dvh)
        {
            // Validaciones (descomenta cuando estés listo)
            if (string.IsNullOrWhiteSpace(dvh))
                dvh = "123456789";//throw new ArgumentException("El DVH no puede estar vacío.");
            

            return new DvhVo(dvh);
        }
    }
}