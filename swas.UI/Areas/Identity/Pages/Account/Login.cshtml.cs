///Created and Reviewed by : Sub Maj Sanal
///Reviewed Date : 30 Jul 23
///Tested By :- 
///Tested Date : 
///Start

#nullable disable
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ASPNetCoreIdentityCustomFields.Data;
using swas.UI.Controllers;
using swas.BAL.Helpers;
using swas.BAL.Utility;
using swas.DAL.Models;
using swas.BAL.DTO;
using swas.BAL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using swas.DAL;

namespace swas.Areas.Identity.Pages.Account
{
    ///Created and Reviewed by : Sub Maj Sanal
    ///Reviewed Date : 30 Jul 23
    ///Tested By :- 
    ///Tested Date : 
    ///Start
    // Customised by Sub Maj M Sanal Kumar on 29 Jul 23

    public class LoginModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<LoginModel> _logger;
        private readonly IUnitRepository _unitRepository;

        private readonly IUserRepository _userRepository;
        public readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        public LoginModel(SignInManager<ApplicationUser> signInManager, ILogger<LoginModel> logger, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IUnitRepository unitRepository, IUserRepository userRepository, ApplicationDbContext context, IConfiguration configuration)
        {
            _signInManager = signInManager;
            _logger = logger;
            _userManager = userManager;
            _roleManager = roleManager;
            _unitRepository = unitRepository;
            _userRepository = userRepository;
            _context = context;
            _configuration = configuration;
            _configuration = configuration;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public string ReturnUrl { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [TempData]
        public string ErrorMessage { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public class InputModel
        {
            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required]
            //[EmailAddress]
            public string UserName { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Display(Name = "Remember me?")]
            public bool RememberMe { get; set; } = true;


            ///
            ///Created and Reviewed by : Sub Maj Sanal
            ///Reviewed Date : 30 Jul 23
            ///Tested By :- 
            ///Tested Date : 
            ///Start



        }
        [AllowAnonymous]
        public async Task OnGetAsync(string returnUrl = null)
        {
            //Response.Redirect("https://iam2.army.mil/IAM/User", true);
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }

            //returnUrl ??= Url.Content("~/");

            //// Clear the existing external cookie to ensure a clean login process
            //await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);
            //// ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            //ReturnUrl = returnUrl;

            //var data = _configuration["debugWithIAMflag"];
            //if (data == "true") {
            //    Response.Redirect("https://iam2.army.mil/IAM/User", true);
            //}
            //else
            //{
            //    if (!string.IsNullOrEmpty(ErrorMessage))
            //    {
            //        ModelState.AddModelError(string.Empty, ErrorMessage);
            //    }

            //    returnUrl ??= Url.Content("~/");

            //    // Clear the existing external cookie to ensure a clean login process
            //    await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            //    // ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();


            //    ReturnUrl = returnUrl;
            //}
        }

        //public async Task<string> GetUserRoles(string username)
        //{
        //    var user = await _userManager.FindByNameAsync(username);

        //    if (user == null)
        //    {
        //        // Handle the case where the user is not found
        //        return null;
        //    }
        //    var ss = await _userManager.GetRolesAsync(user);
        //    string opt = ss[0].ToString();
        //    return opt;
        //}

        ///Created and Reviewed by : Sub Maj Sanal
        ///Reviewed Date : 30 Jul 23
        ///Tested By :- 
        ///Tested Date : 
        ///Start 
        [AllowAnonymous]
        public async Task<IActionResult> OnPostAsync(string returnUrl = null)        
        {
            try
            {
                returnUrl ??= Url.Content("~/");
                //await _signInManager.SignOutAsync();
                //await HttpContext.SignOutAsync();
                var ipAddress = HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
                var currentDatetime = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");
                var watermarkText = $" {ipAddress}\n  {currentDatetime}";


                Login Logins = SessionHelper.GetObjectFromJson<Login>(HttpContext.Session, "User");
                ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

                if (ModelState.IsValid)
                {
                    if (Logins.IsNotNull())
                    {
                        // ModelState.AddModelError("", "Other User Already Logged In Or Not Properly Logged Out");
                    }
                    else
                    {
                        ViewData["UserName"] = Input.UserName;

                        var result = await _signInManager.PasswordSignInAsync(Input.UserName, Input.Password, Input.RememberMe, lockoutOnFailure: true);

                        if (result.Succeeded)  /// registered user
                        {
                            ApplicationUser userdet = await _userManager.FindByNameAsync(Input.UserName);

                            if (userdet != null)
                            {
                                var unitdetl = await _unitRepository.GetUnitDtl(userdet.unitid);
                                int cla = await _unitRepository.GetIdCalendar();
                                if (unitdetl != null)
                                {
                                    Login Db = new Login();
                                    userdet.domain_iam = userdet.UserName;
                                    CommonHelper commonHelper = new CommonHelper(_context);
                                    var userRank = commonHelper.UserRankDetail(userdet);

                                    if (userdet.domain_iam != null)   // domain_iam available after registration
                                    {
                                        Db.UserName = userdet.UserName;
                                        Db.Comdid = unitdetl.unitid;
                                        Db.Corpsid = unitdetl.CorpsId;
                                        Db.Iamuserid = userdet.domain_iam;
                                        Db.Unit = unitdetl.UnitName;        
                                        Db.unitid = userdet.unitid;
                                        Db.UserIntId = userdet.unitid;
                                        //Db.Rank=userdet.Rank;
                                        Db.Rank=userRank;
                                        Db.IcNo = userdet.Icno;
                                        Db.Offr_Name = userdet.Offr_Name;
                                        var users = await _userManager.FindByNameAsync(userdet.UserName);
                                        var usroles = await _userManager.GetRolesAsync(users);
                                        Db.Role = usroles.Any() ? usroles[0] : "Unit";
                                        Db.IpAddress = watermarkText;
                                        Db.cla=cla;
                                        if (Db.ActualUserName == null)
                                        {
                                            Db.ActualUserName = Input.UserName;
                                        }
                                    }
                                    ///////////////login log//////////////////////
                                    tbl_LoginLog logs = new tbl_LoginLog();
                                    var Role = await _userManager.GetRolesAsync(userdet);
                                    logs.UserId = userdet.UserIntId;
                                    logs.IP = HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
                                    logs.IsActive = true;
                                    logs.Updatedby = userdet.unitid;
                                    logs.UpdatedOn = DateTime.Now;
                                    logs.logindate = DateTime.Now;
                                    logs.userName = userdet.UserName;
                                    logs.unitid = userdet.unitid;
                                    await _userRepository.Add(logs);
                                    ////////////////End Log////////////////////////
                                    SessionHelper.SetObjectAsJson(HttpContext.Session, "User", Db);
                                    

                                    if (Db.Role == "Dte")
                                    {
                                        HttpContext.Session.SetString("UserName", Input.UserName);
                                        return RedirectToAction("NewProject", "Home");
                                    }
                                    else
                                    {
                                        return RedirectToAction("NewProject", "Home");
                                    }
                                }
                            }
                        }
                        else // application identity failed but IAM login found correct ..  Allow as a StakeHolder
                        {
                            if (Input.UserName != null)
                            {
                                TempData["UserName"] = Input.UserName;
                                HttpContext.Session.SetString("UserName", Input.UserName);

                                return RedirectToAction("NewProject", "Home");
                            }
                        }

                        if (result.IsLockedOut)
                        {
                            ModelState.AddModelError("", "The account is locked out");
                            return Page();
                        }

                        if (result.RequiresTwoFactor)
                        {
                            return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, RememberMe = Input.RememberMe });
                        }

                        
                        ModelState.AddModelError(string.Empty, "Invalid login attempt");
                        return Page();
                    }
                }
            }
            catch (Exception ex)
            {
                swas.BAL.Utility.Error.ExceptionHandle(ex.Message);
                //return Redirect("/Home/Error");
            }

            return Page();
        }


    }
}
