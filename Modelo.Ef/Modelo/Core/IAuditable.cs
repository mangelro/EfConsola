

namespace Modelo.Ef.Core

{
    public interface IAuditable
    {
        //Solo para marcado Ef crea propiedades Shadows
    }

    public static class AuditableField
    {
        public const string CREADOPOR_FIELD = "CreadoPor";
        public const string CREADOEN_FIELD = "CreadoEn";

    }
}
