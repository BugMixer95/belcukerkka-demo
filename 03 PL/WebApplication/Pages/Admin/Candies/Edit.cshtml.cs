using Belcukerkka.Models.Entities;
using Belcukerkka.Repositories.Interfaces;
using Belcukerkka.Services.Extras;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace WebApplication.Pages.Admin.Candies
{
    public class EditModel : PageModel
    {
        private readonly IEntityRepository<Candy> _candyRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public EditModel(IEntityRepository<Candy> candyRepository, IWebHostEnvironment webHostEnvironment)
        {
            _candyRepository = candyRepository;
            _webHostEnvironment = webHostEnvironment;
        }

        private readonly string imageFolder = "images\\candies";

        [BindProperty]
        public Candy Candy { get; set; }

        [BindProperty]
        [Display(Name = "Изображение")]
        public IFormFile Image { get; set; }

        public IActionResult OnGet(int? id)
        {
            if (id.HasValue)
                Candy = _candyRepository.Get(id);
            else
                Candy = new Candy();

            if (Candy == null)
                return RedirectToPage("/Admin/Candies/Index");

            return Page();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
                return Page();

            if (Image != null)
            {
                string webRootPath = _webHostEnvironment.WebRootPath;
                Candy.ImagePath = UploadImageHandler.ProcessUploadedImage(Candy, Image, webRootPath, imageFolder);
            }

            if (Candy.Id > 0)
                Candy = _candyRepository.Update(Candy);
            else
                Candy = _candyRepository.Create(Candy);

            return RedirectToPage("Index");
        }
    }
}
