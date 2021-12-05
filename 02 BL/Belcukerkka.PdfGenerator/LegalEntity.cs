using System.ComponentModel.DataAnnotations;

namespace Belcukerkka.PdfGenerator
{
    /// <summary>
    /// Specifies all legal entities that are used in documents creation.
    /// </summary>
    public enum LegalEntity : byte
    {
        [Display(Name = "ООО \"Белцукерка\"")] Belcukerka = 1,
        [Display(Name = "ООО \"Ориол Трейд\"")] OriolTrade
    }
}
