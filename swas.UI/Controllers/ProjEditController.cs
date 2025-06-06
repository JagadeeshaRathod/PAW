using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using swas.UI.Models;
using swas.BAL.Helpers;
using swas.BAL.Utility;
using swas.BAL.Interfaces;
using swas.BAL.Repository;
using swas.DAL.Models;
using swas.BAL;
using Microsoft.AspNetCore.Authorization;
using swas.BAL.DTO;
using ASPNetCoreIdentityCustomFields.Data;
using Microsoft.AspNetCore.Identity.UI.V4.Pages.Account.Internal;
using Microsoft.AspNetCore.Identity;
using swas.DAL;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using System.Net.NetworkInformation;
using Humanizer;
using System.Globalization;
using System;
using NuGet.Packaging.Signing;
using Microsoft.AspNetCore.Mvc.Rendering;
using iText.Commons.Actions.Contexts;
using Microsoft.AspNetCore.DataProtection;
using static iText.StyledXmlParser.Jsoup.Select.Evaluator;

namespace swas.UI.Controllers
{
    public class ProjEditController : Controller
    {
        //private readonly ILogger<HomeController> _logger;
        private readonly IProjectsRepository _projectsRepository;
        //private readonly RepositoryUser _repositoryUser;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<LoginModel> _logger;
        private readonly IDataProtector _dataProtector;
        private readonly IDdlRepository _dlRepository;
        private readonly ApplicationDbContext _context;
        private readonly IUnitRepository _unitRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;


        public ProjEditController(IProjectsRepository projectsRepository, SignInManager<ApplicationUser> signInManager, ILogger<LoginModel> logger, UserManager<ApplicationUser> userManager, IDdlRepository dlRepository, ApplicationDbContext context, IUnitRepository unitRepository, IHttpContextAccessor httpContextAccessor, IDataProtectionProvider DataProtector)
        {
            //  _logger = logger; _repositoryUser = repositoryUser;
            _projectsRepository = projectsRepository;

            _signInManager = signInManager;
            _logger = logger;
            _userManager = userManager;

            _dlRepository = dlRepository;
            _context = context;
            _unitRepository = unitRepository;
            _httpContextAccessor = httpContextAccessor;
            _dataProtector = DataProtector.CreateProtector("swas.UI.Controllers.ProjectsController");

        }

        [Authorize(Policy = "StakeHolders")]
        public async Task<IActionResult> Index()
        {
            var dateTime = DateTime.Now;
            try
            {
                Login Logins = SessionHelper.GetObjectFromJson<Login>(HttpContext.Session, "User");
                var ipAddress = HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
                var currentDatetime = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");
                var watermarkText = $" {ipAddress}\n  {currentDatetime}";

                TempData["ipadd"] = watermarkText;

                if (Logins.IsNotNull())
                {
                    int? stakeholderId = Logins.unitid;


                    var proj = _context.Projects.ToList();
                    ViewBag.proj = proj;

                    var stack = _context.tbl_mUnitBranch.Select(e => e.UnitName).ToList();
                    ViewBag.stack = stack;

                    var stage = _context.mStages.Select(e => e.Stages).ToList();
                    ViewBag.stage = stage;

                    var Status = _context.mStatus.Select(e => e.Status).ToList();
                    ViewBag.status = Status;

                    var action = _context.mActions.Select(e => e.Actions).ToList();
                    ViewBag.action = action;
                    List<tbl_Projects> modelproj = new List<tbl_Projects>();
                    ///modelproj =  await _projectsRepository.GetProjforEditAsync();

                    return View(modelproj);
                }
                else
                {
                    return Redirect("/Identity/Account/Login");
                }
            }
            catch (Exception ex)
            {
                swas.BAL.Utility.Error.ExceptionHandle(ex.Message);
                return RedirectToAction("Error", "Home");
            }
        }
    }
}
