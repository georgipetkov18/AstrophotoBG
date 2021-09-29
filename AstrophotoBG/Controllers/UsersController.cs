using AstrophotoBG.Data.Models;
using AstrophotoBG.Data.Services;
using AstrophotoBG.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AstrophotoBG.Controllers
{
    [Route("[controller]/[action]/{userName?}")]
    public class UsersController : Controller
    {
        private readonly IUserService userService;
        private readonly IPictureService pictureService;
        private readonly IGalleryService galleryService;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly IConfiguration configuration;

        public UsersController(IUserService userService, IPictureService pictureService, IGalleryService galleryService, SignInManager<ApplicationUser> signInManager, IConfiguration configuration)
        {
            this.userService = userService;
            this.pictureService = pictureService;
            this.galleryService = galleryService;
            this.signInManager = signInManager;
            this.configuration = configuration;
        }
        
        public async Task<IActionResult> Preview(string userName)
        {
            try
            {
                var user = await userService.GetUserAsync(userName);
                var userPicturesCount = await this.pictureService.CountPictures(user.Id);
                var recentPictures = await galleryService.GetLastPicturesAsync(4, user.Id);
                var viewModel = new UserViewModel {
                    UserName = user.UserName,
                    Description = user.ShortDescription,
                    Location = user.Location,
                    Rating = user.Raiting,
                    CreatedOn = user.CreatedOn,
                    PicturesCount = userPicturesCount,
                    RecentPictures = recentPictures.ToList(),
                };
                return this.View(viewModel);
            }
            catch (Exception)
            {
                return this.Redirect("/Home/Error");
            }
        }

        public async Task<IActionResult> Pictures(string userName)
        {
            try
            {
                var user = await userService.GetUserAsync(userName);
                var pictures = await this.galleryService.GetLastPicturesAsync(24, user.Id);
                var viewModel = new UserPicturesViewModel
                {
                    UserName = userName,
                    Pictures = pictures.ToList(),
                };
                return this.View(viewModel);
            }
            catch (Exception)
            {
                return this.Redirect("/Home/Error");
            }
        }

        public IActionResult ExternalLogin(string provider)
        {
            if (!this.configuration.GetSection("Authentication").Exists())
            {
                return this.Redirect("/Account/Login");
            }

            var redirectUri = Url.Action("ExternalLoginResponse");

            var properties = new AuthenticationProperties { RedirectUri = redirectUri };
            return new ChallengeResult(provider, properties);
        }

        public async Task<IActionResult> ExternalLoginResponse()
        {
            var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            var claims = result.Principal.Identities
                .FirstOrDefault().Claims.Select(claim => new
                {
                    claim.Issuer,
                    claim.OriginalIssuer,
                    claim.Type,
                    claim.Value
                });
            var providerName = claims.FirstOrDefault().Issuer;
            var providerKey = claims.FirstOrDefault(c => c.Type.Contains("/nameidentifier")).Value;
            var fullName = claims.FirstOrDefault(c => Regex.IsMatch(c.Type, @"\bgivenname\b")).Value;
            var email = claims.FirstOrDefault(c => c.Type.Contains("/emailaddress")).Value;

            bool isRegistered = await this.userService.IsRegistered(email);
            var user = isRegistered ? await this.userService.GetUserAsync(fullName) :
                await this.userService.ExternalRegisterAsync(providerKey, providerName, fullName, email);

            await this.signInManager.RefreshSignInAsync(user);
            return this.Redirect("/");
        }
    }
}
