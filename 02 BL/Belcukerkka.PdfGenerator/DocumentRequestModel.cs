using Belcukerkka.Models.Entities;
using System;
using System.ComponentModel.DataAnnotations;

namespace Belcukerkka.PdfGenerator
{
    public class DocumentRequestModel
    {
        public Order Order { get; set; }

        [Required(ErrorMessage = "Поле \"Реквизиты заказчика\" обязательно к заполнению.")]
        [Display(Name = "Реквизиты заказчика")]
        public string CustomerInfo { get; set; }
        
        public DateTime DocumentDate { get; set; }
        public bool IsSignatureNeeded { get; set; }
        public bool IsAppendixNeeded { get; set; }

        [Required(ErrorMessage = "Поле \"Тип документа\" обязательно к заполнению.")]
        [Display(Name = "Тип документа")]
        public DocumentType DocumentType { get; set; }
        
        [Required(ErrorMessage = "Поле \"Организация\" обязательно к заполнению.")]
        [Display(Name = "Организация")]
        public LegalEntity LegalEntity { get; set; }

        public ContractTermsModel ContractTerms { get; set; }
    }
}
