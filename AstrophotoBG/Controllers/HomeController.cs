using AstrophotoBG.Data.Services;
using AstrophotoBG.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace AstrophotoBG.Controllers
{
    public class HomeController : Controller
    {
        private readonly IGalleryService galleryService;
        private readonly IPictureService pictureService;

        public HomeController(IGalleryService galleryService, IPictureService pictureService)
        {
            this.galleryService = galleryService;
            this.pictureService = pictureService;
        }

        public async Task<IActionResult> Index()
        {
            var pictures = await galleryService.GetLastPicturesAsync(4);
            var viewModel = new IndexPictureListViewModel
            {
                LastPictures = pictures
            };

            return View(viewModel);
        }

        public IActionResult FAQ()
        {
            return this.View();
        }

        public IActionResult ThermsOfUse()
        {
            return View();
        }

        public IActionResult Success()
        {
            return this.View();
        }

        public async Task<IActionResult> DisplayPicture(int id)
        {
            try
            {
                var bytes = await pictureService.GetPictureDataByIdAsync(id);
                return this.File(bytes, "image/png");
            }

            catch (Exception)
            {
                return this.Redirect("/Home/Error");
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
