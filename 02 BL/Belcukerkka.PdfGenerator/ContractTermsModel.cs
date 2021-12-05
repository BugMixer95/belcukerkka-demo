using System.ComponentModel.DataAnnotations;

namespace Belcukerkka.PdfGenerator
{
    public class ContractTermsModel
    {
        [Required(ErrorMessage = "Поле \"Организация-заказчик\" обязательно к заполнению.")]
        [Display(Name = "Организация-заказчик")]
        public string ContractCustomerCompany { get; set; }

        [Required(ErrorMessage = "Поле \"Представитель заказчика\" обязательно к заполнению.")]
        [Display(Name = "Представитель заказчика")]
        public string ContractCustomerName { get; set; }

        [Required(ErrorMessage = "Поле \"Основания\" обязательно к заполнению.")]
        [Display(Name = "Основания")]
        public string ContractBasedOn { get; set; }

        [Required(ErrorMessage = "Поле \"Дата поставки\" обязательно к заполнению.")]
        [Display(Name = "Дата поставки")]
        public string ContractSupplyDate { get; set; }

        [Required(ErrorMessage = "Поле \"Условия оплаты\" обязательно к заполнению.")]
        [Display(Name = "Условия оплаты")]
        public string ContractPaymentTerms { get; set; }
    }
}
