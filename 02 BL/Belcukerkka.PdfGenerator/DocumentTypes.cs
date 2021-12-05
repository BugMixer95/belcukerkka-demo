using System.ComponentModel.DataAnnotations;

namespace Belcukerkka.PdfGenerator
{
    public enum DocumentType : byte
    {
        [Display(Name = "Счёт")] Invoice = 1,
        [Display(Name = "Коммерческое предложение")] CommercialOffer,
        [Display(Name = "Договор")] Contract
    }
}
