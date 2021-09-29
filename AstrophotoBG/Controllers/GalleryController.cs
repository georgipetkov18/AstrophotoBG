using AstrophotoBG.Data.Services;
using AstrophotoBG.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace AstrophotoBG.Controllers
{
    public class GalleryController : Controller
    {
        private readonly IGalleryService galleryService;

        public GalleryController(IGalleryService galleryService)
        {
            this.galleryService = galleryService;
        }

        public async Task<IActionResult> Pictures()
        {    
            try
            {
                var viewModel = new GalleryViewModel
                {
                    Pictures = await galleryService.GetPicturesAsync()
                };

                return this.View(viewModel);
            }
            catch (Exception)
            {
                return this.Redirect("/Home/Error");
            }
        }
    }
}
