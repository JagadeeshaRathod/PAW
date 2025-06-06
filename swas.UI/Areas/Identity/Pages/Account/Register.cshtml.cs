// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
///Created and Reviewed by : Sub Maj Sanal
///Reviewed Date : 30 Jul 23
///Tested By :- 
///Tested Date : 
///Start
#nullable disable

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ASPNetCoreIdentityCustomFields.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using swas.BAL.Helpers;
using swas.DAL;
using swas.BAL.Utility;
using swas.DAL.Models;
using swas.BAL.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using swas.BAL.Interfaces;
using swas.BAL.Repository;
using Microsoft.AspNetCore.DataProtection;
using System.Linq.Expressions;
using swas.BAL;
using System.Text;
using System.Security.Cryptography;
using Org.BouncyCastle.Utilities;


namespace swas.Areas.Identity.Pages.Account
{
    [Route("Identity/Account/Register")]
    ///Created and Reviewed by : Sub Maj Sanal
    ///Reviewed Date : 30 Jul 23
    ///Tested By :- 
    ///Tested Date : 
    ///Start

    // [Authorize(Policy = "Admin")]
    //[TypeFilter(typeof(SessionAuthorize))]
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly Microsoft.AspNetCore.Identity.UserManager<ApplicationUser> _userManager;
        private readonly Microsoft.AspNetCore.Identity.IUserStore<ApplicationUser> _userStore;
        private readonly Microsoft.AspNetCore.Identity.IUserEmailStore<ApplicationUser> _emailStore;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        private readonly ApplicationDbContext _context;
        private readonly Microsoft.AspNetCore.Identity.RoleManager<IdentityRole> _roleManager;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IUnitRepository _unitRepository;
        private readonly IDdlRepository _DdlRepostory;
        private readonly IUnitStatusMapping _unitStatusMapping;

        public RegisterModel(
            Microsoft.AspNetCore.Identity.UserManager<ApplicationUser> userManager,
            Microsoft.AspNetCore.Identity.IUserStore<ApplicationUser> userStore,
            SignInManager<ApplicationUser> signInManager,
            ILogger<RegisterModel> logger,
            ApplicationDbContext context,
             Microsoft.AspNetCore.Identity.RoleManager<IdentityRole> roleManager,
            IEmailSender emailSender,
            IHttpContextAccessor httpContextAccessor,
            IUnitRepository unitRepository, IDdlRepository ddlRepository, IUnitStatusMapping unitStatusMapping)
        {
            _userManager = userManager;
            _userStore = userStore;
            _emailStore = GetEmailStore();
            _signInManager = signInManager;
            _logger = logger;
            _context = context;
            _emailSender = emailSender;
            _roleManager = roleManager;
            _contextAccessor = httpContextAccessor;
            _unitRepository = unitRepository;
            _DdlRepostory = ddlRepository;
            _unitStatusMapping = unitStatusMapping;
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

        public string ReturnUrl { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        [NotMapped]
        public string Command { get; set; }

        [NotMapped]
        public string Corps { get; set; }



        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [AllowAnonymous]
        public class InputModel
        {
            [Required(ErrorMessage = "UserName is required.")]
           
            [Display(Name = "UserName")]
            public string UserName { get; set; }

            [Required(ErrorMessage = "Password is required.")]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }

            [Required(ErrorMessage = "RoleName is required.")]
            [Display(Name = "RoleName")]
            public string RoleName { get; set; }

            [Required(ErrorMessage = "OfficerName is required.")]
          
            [Display(Name = "OfficerName")]
            public string OfficerName { get; set; }

            [Display(Name = "IAM User ID")]
            public string domain_iam { get; set; }

            [Display(Name = "Description IAM")]
            public string description_iam { get; set; }

            [Display(Name = "Role IAM")]
            public string RoleName_IAM { get; set; }

            [Display(Name = "Unit")]
            public int unitId { get; set; }

            [RegularExpression(@"^[a-zA-Z0-9\s]*$", ErrorMessage = "Appointment should contain only letters and numbers.")]
            [Display(Name = "Appointment")]
            public string? appointment { get; set; }


            [Required(ErrorMessage = "Rank is required.")]
            [RegularExpression(@"^[a-zA-Z]*$", ErrorMessage = "Rank should contain only letters.")]
            [Display(Name = "Rank")]
            public string Rank { get; set; }

          
            [Display(Name = "Tele No (Army)")]
            public string? Tele_Army { get; set; }

            [NotMapped]
            [Display(Name = "Existing Regn Id of the unit(If already Regd)")]
            public string ExRegnId { get; set; }

            public DateTime? CreatedDate { get; set; }
        }

        ///Created and Reviewed by : Sub Maj Sanal
        ///Reviewed Date : 30 Jul 23
        ///Tested By :- 
        ///Tested Date : 
        ///Start
        ///

        public async Task<IActionResult> OnGetAsync(string returnUrl = null)
        {
            Login Logins = SessionHelper.GetObjectFromJson<Login>(HttpContext.Session, "User");
            string userName = HttpContext.Session.GetString("UserName");

            if (Logins.IsNotNull())
            {

                ReturnUrl = returnUrl;
                ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

                await Task.Delay(1000);
                var addlroles = from Users in _userManager.Users
                                select new
                                {
                                    domain = Users.UserName,
                                    Name = Users.UserName
                                };
                List<AddlTask> tasks = new List<AddlTask>();
                foreach (var item in addlroles)
                {
                    AddlTask dt = new AddlTask();
                    dt.Id = item.domain;
                    dt.Name = item.Name;
                    tasks.Add(dt);
                }
                ViewData["unitdtl"] = _context.tbl_mUnitBranch.ToList();
                ViewData["Unit"] = _context.tbl_mUnitBranch.ToList();
                Input = new InputModel();
                //Input.UserName = userName;

                ViewData["IAMUser"] = tasks.ToList();

                if (Logins.unitid == 1)
                {
                    ViewData["roles"] = _roleManager.Roles
                            .Where(a => a.Name == "Unit" || a.Name == "Dte")
                            .Select(a => a)
                            .ToList();
                }
                else
                {
                    ViewData["roles"] = _roleManager.Roles
                           .Where(a => a.Name == "Unit")
                           .Select(a => a)
                           .ToList();
                }


                ViewData["rank"] = _context.mRank.ToList();


                List<mCommand> cl = new List<mCommand>();

                //---------------Getting Data From Database Using EntityFrameworkCore----------------------
                cl = await _DdlRepostory.ddlCommand();

                //-------------------Inserting Select Item in List-------------------------
                cl.Insert(0, new mCommand { comdid = 0, Command_Name = "--Select--" });
                //--------------Assigning categorylist to ViewBag.ListofCategory --------------------------
                ViewData["cl"] = _context.mCommand.ToList();

                ViewData["ty"] = _context.tbl_Type.ToList();
                List<Types> ty = new List<Types>();

                //---------------Getting Data From Database Using EntityFrameworkCore----------------------
                ty = await _DdlRepostory.ddlType();

                //-------------------Inserting Select Item in List-------------------------
                ty.Insert(0, new Types { Id = 0, Name = "--Select--" });
                //--------------Assigning categorylist to ViewBag.ListofCategory --------------------------



                List<UnitDtl> udtl = new List<UnitDtl>();

                udtl = await _unitRepository.GetAllUnitAsync();
                //  ViewData["roles"] = _roleManager.Roles.Select(a => a.Name == "Unit" || a.Name == "Dte").ToList();


                // ViewData["roles"] = _roleManager.Roles.ToList();

                return Page();
            }

            else
            {
                ReturnUrl = returnUrl;
                ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();



                ViewData["unitdtl"] = _context.tbl_mUnitBranch.ToList();
                await Task.Delay(1000);
                Input = new InputModel();
                Input.UserName = userName;
                //var adminUserName = User.Identity.Name;
                //Input.adminUserName = adminUserName;


                var addlroles = from Users in _userManager.Users
                                select new
                                {
                                    domain = Users.UserName,
                                    Name = Users.UserName
                                };


                List<AddlTask> tasks = new List<AddlTask>();
                foreach (var item in addlroles)
                {
                    AddlTask dt = new AddlTask();
                    dt.Id = item.domain;
                    dt.Name = item.Name;
                    tasks.Add(dt);
                }

                ViewData["rank"] = _context.mRank.ToList();


                ViewData["Unit"] = _context.tbl_mUnitBranch.ToList();
                ViewData["IAMUser"] = tasks.ToList();

                ViewData["Corps"] = _context.mCorps.ToList();


                ViewData["corpsId"] = 0;
                List<mCommand> cl = new List<mCommand>();

                //---------------Getting Data From Database Using EntityFrameworkCore----------------------
                cl = await _DdlRepostory.ddlCommand();

                //-------------------Inserting Select Item in List-------------------------
                cl.Insert(0, new mCommand { comdid = 0, Command_Name = "--Select--" });
                //--------------Assigning categorylist to ViewBag.ListofCategory --------------------------
                ViewData["cl"] = _context.mCommand.ToList();

                ViewData["ty"] = _context.tbl_Type.ToList();


                List<Types> ty = new List<Types>();

                //---------------Getting Data From Database Using EntityFrameworkCore----------------------
                ty = await _DdlRepostory.ddlType();

                //-------------------Inserting Select Item in List-------------------------
                ty.Insert(0, new Types { Id = 0, Name = "--Select--" });
                //--------------Assigning categorylist to ViewBag.ListofCategory --------------------------



                List<UnitDtl> udtl = new List<UnitDtl>();

                udtl = await _unitRepository.GetAllUnitAsync();


                //if (Logins.unitid == 1)
                //{
                //    ViewData["roles"] = _roleManager.Roles
                //            .Where(a => a.Name == "Unit" || a.Name == "Dte")
                //            .Select(a => a)
                //            .ToList();
                //}
                //else
                //{
                //    ViewData["roles"] = _roleManager.Roles
                //           .Where(a => a.Name == "Unit")
                //           .Select(a => a)
                //           .ToList();
                //}
                //  ViewData["roles"] = _roleManager.Roles.Select(a => a.Name == "Unit" || a.Name == "Dte").ToList();


                ViewData["roles"] = _roleManager.Roles.ToList();

                return Page();
            }
            //}
            //else
            //    TempData["FailureMessage"] = "Registration Failed.....";
            //// return Redirect("/Identity/Account/Register");
            //return Page();

        }



        ///Created and Reviewed by : Sub Maj Sanal
        ///Reviewed Date : 16 Aug 23
        ///Tested By :- 
        ///Tested Date : 
        ///Start <summary>
        /// Created and Reviewed by : Sub Maj Sanal
        /// </summary>
        /// <param name="returnUrl"></param>
        /// <returns></returns>

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            TempData["Tabshift"] = 0;
            ViewData["unitdtl"] = _context.tbl_mUnitBranch.ToList();
            ViewData["Command"] = _context.mCommand.ToList();
            ViewData["Corps"] = _context.mCorps.ToList();

            ViewData["roles"] = _roleManager.Roles
                .Where(a => a.Name == "Unit" || a.Name == "Dte")
                .Select(a => a)
                .ToList();

            returnUrl ??= Url.Content("~/Identity/Account/Register");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            if (Input.RoleName == null)
            {
                Input.RoleName = "1789a675-9951-42a7-b064-8d7da156521f";
                Input.Password = "Dte@123";
            }
            else
            {
                Input.Password = "Dte@123";
            }

            var role = await _roleManager.FindByIdAsync(Input.RoleName);

            if (Input.RoleName != null && Input.Password != null && Input.unitId > 0 && Input.UserName != null && Input.Rank != null && Input.OfficerName != null && Input.Tele_Army != null)
            {
                // Assuming adminUserName is the admin's UserName
                var adminUserName = User.Identity.Name; // You might need to adjust this based on how you get the admin's UserName

                var rankId = _context.mRank.FirstOrDefault(x => x.RankName == Input.Rank).Id;
                bool flg = true;
                ApplicationUser inputModel = new ApplicationUser();

                inputModel = new ApplicationUser();

                var list = (from a in _userManager.Users
                            where a.unitid == Input.unitId
                            select new ApplicationUser()
                            {
                                UserName = a.UserName,
                            }).ToList();

                if (list.Count > 0)
                {
                    // Remove the reference to Input.ExRegnId here
                    inputModel = await _userManager.Users
                        .Where(a => a.unitid == Input.unitId)
                        .Select(a => a)
                        .FirstOrDefaultAsync();

                    if (inputModel != null)
                    {
                        flg = true;
                    }
                    else
                    {
                        flg = false;
                    }
                }
                else
                {
                    flg = true;
                }

                if (flg == true)
                {
                    var user = CreateUser();

                    // Set UserName to be the same as the admin's UserName

                    user.EmailConfirmed = true;
                    user.RoleName = Input.RoleName;
                    user.domain_iam = Input.UserName;
                    user.appointment = Input.appointment;
                    user.unitid = Input.unitId;
                    user.UserName = Input.UserName;
                    //user.Rank = Input.Rank;
                    user.Rank = rankId;
                    user.Offr_Name = Input.OfficerName;
                    user.Tele_Army = Input.Tele_Army;                    
                    user.CreatedDate = DateTime.Now;

                    await _userStore.SetUserNameAsync(user, Input.UserName, CancellationToken.None);
                    await _emailStore.SetEmailAsync(user, Input.UserName, CancellationToken.None);

                    string pwd = "Dte@123";
                    var result = await _userManager.CreateAsync(user, pwd);

                    if (result.Succeeded)
                    {
                        _logger.LogInformation("User created a new account with password.");

                        await _userManager.AddToRoleAsync(user, role.Name);

                        var userId = await _userManager.GetUserIdAsync(user);

                        TempData["SuccessMessage"] = "User successfully Registered!";
                        //TempData["SuccessMessage"] = "User successfully Registered! <br> Pls Approach ASDC for necessary Approval";


                        return LocalRedirect("~/Identity/Account/Register");
                    }
                    else
                    {
                        TempData["FailureMessage"] = "Registration Failed!";
                        return LocalRedirect("~/Identity/Account/Register");
                    }
                }
                else
                {
                    TempData["FailureMessage"] = "Existing Regn ID Incorrect!";
                    return LocalRedirect("~/Identity/Account/Register");
                }
            }
            else
            {
                TempData["FailureMessage"] = "One of the important input missing!";
                return LocalRedirect("~/Identity/Account/Register");
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }




        ///Created and Reviewed by : Sub Maj Sanal
        ///Reviewed Date : 30 Jul 23
        ///Tested By :- 
        ///Tested Date : 
        ///Start    
        [AllowAnonymous]
        private ApplicationUser CreateUser()
        {
            try
            {
                return Activator.CreateInstance<ApplicationUser>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(ApplicationUser)}'. " +
                    $"Ensure that '{nameof(ApplicationUser)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                    $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
            }
        }
        ///Created and Reviewed by : Sub Maj Sanal
        ///Reviewed Date : 30 Jul 23
        ///Tested By :- 
        ///Tested Date : 
        ///Start
        private Microsoft.AspNetCore.Identity.IUserEmailStore<ApplicationUser> GetEmailStore()
        {
            if (!_userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }
            return (Microsoft.AspNetCore.Identity.IUserEmailStore<ApplicationUser>)_userStore;
        }



        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> OnPostAddOrEdit(UnitDtl UnitData)
        {
            var ipAddress = HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
            var currentDatetime = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");
            var watermarkText = $" {ipAddress}\n  {currentDatetime}";

            TempData["ipadd"] = watermarkText;
            List<UnitDtl> udtl = new List<UnitDtl>();



            Login Logins = SessionHelper.GetObjectFromJson<Login>(HttpContext.Session, "User");
            if (Logins.IsNotNull())
            {
                UnitData.UpdatedBy = Logins.ActualUserName + "(" + Logins.Unit + ")";
                UnitData.UpdatedDate = DateTime.Now;


                using (SHA256 sha256Hash = SHA256.Create())
                {
                    byte[] hashedBytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(UnitData.UnitSusNo));
                    StringBuilder builder = new StringBuilder();
                    for (int i = 0; i < hashedBytes.Length; i++)
                    {
                        builder.Append(hashedBytes[i].ToString("x2"));
                    }
                    UnitData.UnitSusNo = builder.ToString();
                }

                int result = await _unitRepository.Save(UnitData);


                var newunit = await _unitRepository.GetUnitDtlwithname(UnitData.UnitName);

                //if (UnitData.unitid == newunit.unitid)
                //{
                //    List<int> statusIds = new List<int> { 33, 2, 3, 24, 25, 26, 27, 28, 29, 22, 31 };


                //    foreach (var statusId in statusIds)
                //    {
                //        TrnUnitStatusMapping trnUnitStatusMapping = new TrnUnitStatusMapping
                //        {
                //            UnitId = UnitData.unitid,
                //            StatusId = statusId,
                //            UnitStatusMappingId = 0
                //        };

                //        if (trnUnitStatusMapping.UnitStatusMappingId == 0)
                //        {
                //            var ret = await _unitStatusMapping.AddWithReturn(trnUnitStatusMapping);
                //        }
                //    }
                //}


                if (result == 1)
                {
                    TempData["SuccessMessage"] = "Unit successfully created!";
                }
                else
                {
                    TempData["FailureMessage"] = "Unit Already Exist... Check SUS No or Unit Name!";
                }

                udtl = await _unitRepository.GetAllUnitAsync();

                ViewData["corpsId"] = 0;
                List<mCommand> cl = new List<mCommand>();
                cl = await _DdlRepostory.ddlCommand();
                cl.Insert(0, new mCommand { comdid = 0, Command_Name = "--Select--" });
                ViewData["cl"] = cl.ToList();

                List<Types> ty = new List<Types>();
                ty = await _DdlRepostory.ddlType();
                ty.Insert(0, new Types { Id = 0, Name = "--Select--" });
                ViewData["ty"] = ty.ToList();

                if (UnitData.Updatecde == 1)
                {
                    return Redirect("OnPostAsync");
                }
                else
                {
                    return LocalRedirect("~/Identity/Account/Register");
                }
            }
            else
            {


                UnitData.UpdatedBy = HttpContext.Session.GetString("UserName");
                UnitData.UpdatedDate = DateTime.Now;

                using (SHA256 sha256Hash = SHA256.Create())
                {
                    byte[] hashedBytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(UnitData.UnitSusNo));
                    StringBuilder builder = new StringBuilder();
                    for (int i = 0; i < hashedBytes.Length; i++)
                    {
                        builder.Append(hashedBytes[i].ToString("x2"));
                    }
                    UnitData.UnitSusNo = builder.ToString();
                }

                int result = await _unitRepository.Save(UnitData);

                var newunit = await _unitRepository.GetUnitDtlwithname(UnitData.UnitName);

                //if (UnitData.unitid == newunit.unitid)
                //{
                //    List<int> statusIds = new List<int> { 33, 2, 3, 24, 25, 26, 27, 28, 29, 22, 31 };


                //    foreach (var statusId in statusIds)
                //    {
                //        TrnUnitStatusMapping trnUnitStatusMapping = new TrnUnitStatusMapping
                //        {
                //            UnitId = UnitData.unitid,
                //            StatusId = statusId,
                //            UnitStatusMappingId = 0
                //        };

                //        if (trnUnitStatusMapping.UnitStatusMappingId == 0)
                //        {
                //            var ret = await _unitStatusMapping.AddWithReturn(trnUnitStatusMapping);
                //        }
                //    }
                //}

                if (result == 1)
                {
                    TempData["SuccessMessage"] = "Unit successfully created!";
                }
                else
                {
                    TempData["FailureMessage"] = "Unit Already Exist... Check SUS No or Unit Name!";
                }

                udtl = await _unitRepository.GetAllUnitAsync();

                ViewData["corpsId"] = 0;
                List<mCommand> cl = new List<mCommand>();
                cl = await _DdlRepostory.ddlCommand();
                cl.Insert(0, new mCommand { comdid = 0, Command_Name = "--Select--" });
                ViewData["cl"] = cl.ToList();

                List<Types> ty = new List<Types>();
                ty = await _DdlRepostory.ddlType();
                ty.Insert(0, new Types { Id = 0, Name = "--Select--" });
                ViewData["ty"] = ty.ToList();

                if (UnitData.Updatecde == 1)
                {
                    return LocalRedirect("~/Identity/Account/Register");
                }
                else
                {
                    return LocalRedirect("~/Identity/Account/Register");
                }
            }
        }

        public async Task<IActionResult> OnPostCheckUserName()
        {
            Login Logins = SessionHelper.GetObjectFromJson<Login>(HttpContext.Session, "User");
            string userName = HttpContext.Session.GetString("UserName");


            var user = _userManager.Users.FirstOrDefault(u => u.UserName == userName);

            if (user != null)
            {
                return LocalRedirect("/Identity/Account/Login");
            }
            else
            {
                TempData["Message"] = "Please register yourself first.";

                var addlroles = from Users in _userManager.Users
                                select new
                                {
                                    domain = Users.UserName,
                                    Name = Users.UserName
                                };
                List<AddlTask> tasks = new List<AddlTask>();
                foreach (var item in addlroles)
                {
                    AddlTask dt = new AddlTask();
                    dt.Id = item.domain;
                    dt.Name = item.Name;
                    tasks.Add(dt);
                }
                ViewData["unitdtl"] = _context.tbl_mUnitBranch.ToList();
                ViewData["Unit"] = _context.tbl_mUnitBranch.ToList();
                Input = new InputModel();
                Input.UserName = userName;

                ViewData["IAMUser"] = tasks.ToList();

                List<mCommand> cl = new List<mCommand>();

                //---------------Getting Data From Database Using EntityFrameworkCore----------------------
                cl = await _DdlRepostory.ddlCommand();

                //-------------------Inserting Select Item in List-------------------------
                cl.Insert(0, new mCommand { comdid = 0, Command_Name = "--Select--" });
                //--------------Assigning categorylist to ViewBag.ListofCategory --------------------------
                ViewData["cl"] = _context.mCommand.ToList();

                ViewData["ty"] = _context.tbl_Type.ToList();
                List<Types> ty = new List<Types>();

                //---------------Getting Data From Database Using EntityFrameworkCore----------------------
                ty = await _DdlRepostory.ddlType();

                //-------------------Inserting Select Item in List-------------------------
                ty.Insert(0, new Types { Id = 0, Name = "--Select--" });
                //--------------Assigning categorylist to ViewBag.ListofCategory --------------------------

                List<UnitDtl> udtl = new List<UnitDtl>();

                udtl = await _unitRepository.GetAllUnitAsync();

                return Page();
            }

        }

    }
}
