using AstrophotoBG.Data.Models;
using AstrophotoBG.Data.Services;
using AstrophotoBG.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AstrophotoBG.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class AdminController : Controller
    {
        private readonly IPictureService pictureService;
        private readonly IUserService userService;

        public AdminController(IPictureService pictureService, IUserService userService)
        {
            this.pictureService = pictureService;
            this.userService = userService;
        }

        public IActionResult DeleteUser()
        {
            return this.View();
        }

        public async Task<IActionResult> DelUser(DeleteUserViewModel input)
        {
            await this.userService.DeleteUserAsync(input.UserName);
            return this.RedirectToAction("DeleteUser");
        }

        public async Task<IActionResult> GetPictureInfo(int id)
        {
            var picture = await this.pictureService.GetPictureByIdAsync(id);
            var jsonResult = new
            {
                picture.User.UserName,
                picture.Name,
                picture.Description,
                picture.Technique
            };
            return this.Json(jsonResult);
        }

        [HttpGet]
        public IActionResult DeletePicture()
        {
            return this.View();
        }

        public async Task<IActionResult> DelPicture(int id)
        {
            await this.pictureService.DeletePicture(new Picture { Id = id});
            return this.Json(new { });
        }
    }
}
