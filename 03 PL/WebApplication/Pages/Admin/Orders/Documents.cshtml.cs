using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Belcukerkka.Models.Entities;
using Belcukerkka.PdfGenerator;
using Belcukerkka.PdfGenerator.Creators;
using Belcukerkka.Repositories.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApplication.Pages.Admin.Orders
{
    public class DocumentsModel : PageModel
    {
        public DocumentsModel(IEntityRepository<Order> orderRepository,
            IWebHostEnvironment webHostEnvironment)
        {
            _orderRepository = orderRepository;
            _webHostEnvironment = webHostEnvironment;
        }

        private readonly IEntityRepository<Order> _orderRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;

        [BindProperty]
        public Order Order { get; set; }

        [BindProperty]
        public DocumentRequestModel DocumentRequestModel { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Поле \"Дата документа\" обязательно к заполнению.")]
        [Display(Name = "Дата документа")]
        public string DateAsString { get; set; }

        public IActionResult OnGet(int id)
        {
            Order = _orderRepository.GetWithDependencies(id);
            DocumentRequestModel = new DocumentRequestModel();

            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            // setting document model order
            var order = _orderRepository.GetWithDependencies(Order.Id);
            DocumentRequestModel.Order = order;

            // setting document model date
            try
            {
                string[] dateParamsAsString = DateAsString.Split("/");
                int[] dateParamsAsInt = new int[dateParamsAsString.Length];

                for (int i = 0; i < dateParamsAsInt.Length; i++)
                {
                    dateParamsAsInt[i] = int.Parse(dateParamsAsString[i]);
                }

                DocumentRequestModel.DocumentDate = new DateTime(dateParamsAsInt[2], dateParamsAsInt[1], dateParamsAsInt[0]);
            }
            catch
            {
                DocumentRequestModel.DocumentDate = DateTime.Now;
            }

            // creating document
            BaseDocumentCreator creator = DocumentRequestModel.DocumentType switch
            {
                DocumentType.Invoice => new InvoiceCreator(_webHostEnvironment),
                DocumentType.CommercialOffer => new CommercialOfferCreator(_webHostEnvironment),
                DocumentType.Contract => new ContractCreator(_webHostEnvironment),
                _ => throw new ArgumentException("Unrecognized Enum value!")
            };

            var doc = await creator.CreateAsync(DocumentRequestModel);

            return File(doc, "application/pdf");
        }
    }
}
