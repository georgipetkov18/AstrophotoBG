using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using AstrophotoBG.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace AstrophotoBG.Areas.Identity.Pages.Account.Manage
{
    public class DescAndLocationModel : PageModel
    {
        private const string AuthenicatorUriFormat = "otpauth://totp/{0}:{1}?secret={2}&issuer={0}";

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger<DescAndLocationModel> _logger;

        public DescAndLocationModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<DescAndLocationModel> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
        }
        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            public string Location { get; set; }

            public string Description { get; set; }
        }

        [Display(Name = "Описание")]
        [MaxLength(150, ErrorMessage = "Описанието трябва да съдържа максимум 150 символа.")]
        public string Description { get; set; }

        [Display(Name = "Местоположение")]
        public string Location { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (Input.Location != null) user.Location = Input.Location;
            if (Input.Description != null) user.ShortDescription = Input.Description;

            await _userManager.UpdateAsync(user);
            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Вашият профил беше успешно актуализиран.";
            return RedirectToPage();
        }
    }
}