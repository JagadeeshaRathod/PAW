// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
///Created and Reviewed by : Sub Maj Sanal
///Reviewed Date : 30 Jul 23
///Tested By :- 
///Tested Date : 
///Start
#nullable disable

using System;
using System.Threading.Tasks;
using ASPNetCoreIdentityCustomFields.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace swas.Areas.Identity.Pages.Account
{
    ///Reviewed by : Sub Maj Sanal
    ///Reviewed Date : 09 Jan 23
    ///Tested By :- 
    ///Tested Date : 10 Jan 23
    ///Start
    public class LogoutModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger<LogoutModel> _logger;

        public LogoutModel(SignInManager<ApplicationUser> signInManager, ILogger<LogoutModel> logger)
        {
            _signInManager = signInManager;
            _logger = logger;
        }
        [TempData]
        public string ErrorMessage { get; set; }

        public string ReturnUrl { get; set; }

        [AllowAnonymous]
        public async Task OnGetAsync(string returnUrl = null)
        {
            // Response.Redirect("https://iam3.army.mil/IAM/User", true);
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }

            returnUrl ??= Url.Content("~/");

            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            // ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();


            ReturnUrl = returnUrl;


        }


        [AllowAnonymous]
        public async Task<IActionResult> OnPost(string returnUrl = null)
        {
            await _signInManager.SignOutAsync();
            await HttpContext.SignOutAsync();
            _logger.LogInformation("User logged out.");
            if (returnUrl != null)
            {

                return LocalRedirect("/Identity/Account/Login");
              //  return LocalRedirect(returnUrl);
            }
            else
            {
                // This needs to be a redirect so that the browser performs a new
                // request and the identity for the user gets updated.
                return RedirectToPage("/Identity/Account/Logout");
            }
        }
    }
}

