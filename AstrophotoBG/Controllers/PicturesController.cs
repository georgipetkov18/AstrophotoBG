using AstrophotoBG.Data.Models;
using AstrophotoBG.Data.Repository;
using AstrophotoBG.Data.Services;
using AstrophotoBG.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AstrophotoBG.Controllers
{
    public class PicturesController : Controller
    {
        private readonly IRepository repo;
        private readonly IGalleryService galleryService;
        private readonly ICategoryService categoryService;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IPictureService pictureService;

        public PicturesController(IRepository repo,
            IGalleryService galleryService,
            ICategoryService categoryService,
            UserManager<ApplicationUser> userManager,
            IPictureService pictureService)
        {
            this.repo = repo;
            this.repo = repo;
            this.galleryService = galleryService;
            this.categoryService = categoryService;
            this.userManager = userManager;
            this.categoryService = categoryService;
            this.userManager = userManager;
            this.galleryService = galleryService;
            this.pictureService = pictureService;
        }

        [Authorize]
        public async Task<IActionResult> ChooseAction(int id)
        {
            try
            {
                var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
                var isLiked = await this.pictureService.IsLiked(id, userId);
                string status = isLiked switch 
                {
                    true => "liked",
                    _ => "not liked"
                };

                return new JsonResult(new { status = status });
            }
            catch (Exception)
            {
                return this.Redirect("/Home/Error");
            }
        }

        [Authorize]
        public async Task<IActionResult> IncrementLikes(int id)
        {      
            try
            {
                var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
                var likes = await this.pictureService.IncrementLikesAsync(id, userId);
                return new JsonResult(new { likes = likes });
            }
            catch (Exception)
            {
                return this.Redirect("/Home/Error");
            }
        }

        [Authorize]
        public async Task<IActionResult> ReduceLikes(int id)
        {
            try
            {
                var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
                var likes = await this.pictureService.ReduceLikesAsync(id, userId);
                return new JsonResult(new { likes = likes });
            }
            catch (Exception)
            {
                return this.Redirect("/Home/Error");
            }
        }

        [Authorize]
        public IActionResult Upload()
        {
            return this.View();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> UploadPicture(UploadViewModel input)
        {
            if (!this.ModelState.IsValid)
            {
                this.RedirectToAction("Upload");
            }

            try
            {
                var user = await userManager.GetUserAsync(this.User);
                var category = await categoryService.GetCategoryByNameAsync(input.Category);
                byte[] data = default;

                using (MemoryStream ms = new MemoryStream())
                {
                    await input.Data.CopyToAsync(ms);
                    data = ms.ToArray();
                }

                var picture = new Picture
                {
                    Name = input.Name,
                    Category = category,
                    Date = input.Date,
                    Description = input.Description,
                    Technique = input.Technique,
                    PictureData = data,
                    User = user,
                    CreatedOn = DateTime.UtcNow,
                    ModifiedOn = DateTime.UtcNow
                };

                await repo.Db.Pictures.AddAsync(picture);
                await repo.SaveDbChangesAsync();

                return this.Redirect("/Home/Success");
            }

            catch (NullReferenceException)
            {
                return this.Redirect("/Home/Error");   //TODO: Display Appropriate Message
            }

            catch (Exception)
            {
                return this.Redirect("/Home/Error");  //TODO: Display "An error has occured"
            }
        }

        [Authorize]
        public async Task<IActionResult> UpdatePicture(int id)
        {
            var picture = await pictureService.GetPictureByIdAsync(id);
            if (!(User.FindFirst(ClaimTypes.NameIdentifier).Value == picture.User.Id))
            {
                return this.Redirect("/Home/Error");
            }
            var viewModel = new UpdatePictureViewModel()
            {
                Id = picture.Id,
                Name = picture.Name,
                Description = picture.Description,
                Technique = picture.Technique,
            };

            return this.View(viewModel);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> UpdatePicture(UpdatePictureViewModel input)
        {          
            try
            {
                var picture = new Picture()
                {
                    Id = input.Id,
                    Name = input.Name,
                    Technique = input.Technique,
                    Description = input.Description
                };
                await this.pictureService.UpdatePicture(picture);
                return this.Redirect($"/Pictures/UpdatePicture/{input.Id}");
            }
            catch (Exception)
            {
                return this.Redirect("/Home/Error");
            }
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> DeletePicture(int id)
        {
            try
            {
                await this.pictureService.DeletePicture(new Picture()
                {
                    Id = id
                });
                return this.Redirect("/");
            }
            catch (Exception)
            {
                return this.Redirect("/Home/Error");
            }
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

        public async Task<IActionResult> GetData(int pageIndex, int pageSize, string categoryName, string filter)
        {
            try
            {
                IEnumerable<Picture> pictures;
                if (categoryName == "all")
                {
                    pictures = await galleryService.GetPicturesAsync(pageIndex, pageSize, filter);
                }

                else
                {
                    pictures = await galleryService.GetPicturesByCategoryNameAsync(pageIndex, pageSize, categoryName, filter);
                }

                if (pictures.Count() == 0)
                {
                    return this.Json(pictures);
                }
                return this.Json(pictures);
            }
            catch (Exception)
            {
                return this.Redirect("/Home/Error");
            }
        }

        public async Task<IActionResult> ById(int id)
        {
            if (!this.ModelState.IsValid)
            {
                this.RedirectToAction("Pictures");
            }

            try
            {
                var loggedUserId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
                var picture = await pictureService.GetPictureByIdAsync(id);
                var pictureUserId = picture.User.Id;
                var isLikedByCurrentUser = await this.pictureService.IsLiked(id, loggedUserId);
                var recentPictures = await this.galleryService.GetLastPicturesAsync(3, pictureUserId, id);
                var viewModel = new DisplayPictureViewModel
                {
                    Id = picture.Id,
                    Name = picture.Name,
                    User = picture.User,
                    Date = picture.Date,
                    Description = picture.Description,
                    PictureData = picture.PictureData,
                    Technique = picture.Technique,
                    Likes = picture.Likes,
                    IsLikedByCurrentUser = isLikedByCurrentUser,
                    RecentPictures = recentPictures.ToList(),
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
