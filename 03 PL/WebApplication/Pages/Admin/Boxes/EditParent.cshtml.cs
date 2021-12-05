using System.Collections.Generic;
using Belcukerkka.Models.Entities;
using Belcukerkka.Repositories.Interfaces;
using Belcukerkka.Services.Extras;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApplication.Pages.Admin.Boxes
{
    public class EditParentModel : PageModel
    {
        private readonly IEntityRepository<BoxParent> _boxParentRepository;
        private readonly IEntityRepository<BoxPackage> _boxPackageRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public EditParentModel(IEntityRepository<BoxParent> boxParentRepository, 
            IEntityRepository<BoxPackage> boxPackageRepository,
            IWebHostEnvironment webHostEnvironment)
        {
            _boxParentRepository = boxParentRepository;
            _boxPackageRepository = boxPackageRepository;
            _webHostEnvironment = webHostEnvironment;
        }

        private readonly string imageFolder = "images\\boxes";

        [BindProperty]
        public BoxParent BoxParent { get; set; }

        [BindProperty]
        public IFormFile Image { get; set; }

        public IEnumerable<SelectListItem> BoxPackageNames { get; set; }

        public IActionResult OnGet(int? id)
        {
            BoxPackageNames = DropdownHandler.GetSelectList(_boxPackageRepository, "Name");

            if (id.HasValue)
                BoxParent = _boxParentRepository.GetWithDependencies((int)id);
            else
                BoxParent = new BoxParent();

            return Page();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
                return Page();

            if (Image != null)
            {
                string webRootPath = _webHostEnvironment.WebRootPath;
                BoxParent.ImagePath = UploadImageHandler.ProcessUploadedImage(BoxParent, Image, webRootPath, imageFolder);
            }

            if (BoxParent.Id > 0)
                BoxParent = _boxParentRepository.Update(BoxParent);
            else
                BoxParent = _boxParentRepository.Create(BoxParent);

            return RedirectToPage("Index");
        }
    }
}
