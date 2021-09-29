using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using AstrophotoBG.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using System;

namespace AstrophotoBG.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<RegisterModel> _logger;

        public RegisterModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<RegisterModel> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public class InputModel
        {
            [Required(ErrorMessage = "Въведете име!")]
            [Display(Name = "Име *")]
            public string Name { get; set; }

            [Required(ErrorMessage = "Въведете Email!")]
            [EmailAddress(ErrorMessage = "Въведете валиден Email адрес!")]
            [Display(Name = "Email *")]
            public string Email { get; set; }

            [Required(ErrorMessage = "Въведете парола!")]
            [StringLength(20, ErrorMessage = "Паролата трябва да бъде между {2} и {1} символа.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Парола *")]
            public string Password { get; set; }

            [Required(ErrorMessage = "Потвърдете парола!")]
            [DataType(DataType.Password)]
            [Display(Name = "Потвърди парола *")]
            [Compare("Password", ErrorMessage = "Въвели сте различни пароли.")]
            public string ConfirmPassword { get; set; }

            [Display(Name = "Местоположение")]
            public string Location { get; set; }

            [Display(Name = "Кратко описание")]
            [MaxLength(150, ErrorMessage = "Описанието трябва да съдържа максимум 150 символа.")]
            public string ShortDescription { get; set; }

            [Required]
            public bool HasReadThermsOfUse { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (Input.HasReadThermsOfUse == false)
            {
                ModelState.AddModelError("HasReadThermsOfUse", "Трябва да се съгласите с правилата за ползване.");
            }

            if (ModelState.IsValid)
            {
                var user = new ApplicationUser {
                    UserName = Input.Name,
                    Email = Input.Email,
                    Location = Input.Location,
                    ShortDescription = Input.ShortDescription,
                    CreatedOn = DateTime.UtcNow,
                    ModifiedOn = DateTime.UtcNow,
                    UsernameModifiedOn = DateTime.UtcNow
                };
                user.EmailConfirmed = true;       // ADDED JUST FOR TESTING
                var result = await _userManager.CreateAsync(user, Input.Password);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");

                   // var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                   // code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = user.Id, returnUrl = returnUrl },
                        protocol: Request.Scheme);

                   /* await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                        $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");*/

                   // if (_userManager.Options.SignIn.RequireConfirmedAccount)
                   // {
                   //     return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
                   // }
                   // else
                   // {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return LocalRedirect(returnUrl);
                   // }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }

    }
}
