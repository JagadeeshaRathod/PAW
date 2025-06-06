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
using Microsoft.AspNetCore.DataProtection;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace swas.UI.Controllers
{
    public class SearchController : Controller
    {
        //private readonly ILogger<HomeController> _logger;
        private readonly IProjectsRepository _projectsRepository;
        //private readonly RepositoryUser _repositoryUser;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<LoginModel> _logger;

        private readonly IDdlRepository _dlRepository;
        private readonly ApplicationDbContext _context;
        private readonly IUnitRepository _unitRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IDataProtector _dataProtector;
        private readonly IProjStakeHolderMovRepository _psmRepository;
        private readonly IAttHistoryRepository _attHistoryRepository;
        private readonly IProjStakeHolderMovRepository _stkholdmove;
        public SearchController(IProjectsRepository projectsRepository, SignInManager<ApplicationUser> signInManager, ILogger<LoginModel> logger, UserManager<ApplicationUser> userManager, IDdlRepository dlRepository, ApplicationDbContext context, IUnitRepository unitRepository, IHttpContextAccessor httpContextAccessor, IProjStakeHolderMovRepository psmRepository, IAttHistoryRepository attHistoryRepository, IProjStakeHolderMovRepository stkholdmove, IDataProtectionProvider DataProtector)
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
            _psmRepository = psmRepository;
            _attHistoryRepository = attHistoryRepository;
            _stkholdmove = stkholdmove;
            _dataProtector = DataProtector.CreateProtector("swas.UI.Controllers.ProjectsController");

        }

        [Authorize(Policy = "Admin")]
        [HttpGet]
        public IActionResult CurrentSearch()
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


                    var proj = _context.Projects.FirstOrDefault();
                    ViewBag.proj = proj;

                    var stack = _context.tbl_mUnitBranch.Select(e => e.UnitName).ToList();
                    ViewBag.stack = stack;

                    var stage = _context.mStages.Select(e => e.Stages).ToList();
                    ViewBag.stage = stage;

                    var Status = _context.mStatus.Select(e => e.Status).ToList();
                    ViewBag.status = Status;

                    var action = _context.mActions.Select(e => e.Actions).ToList();
                    ViewBag.action = action;

                    return View();
                }
                else
                {
                    return Redirect("/Identity/Account/Login");
                }
            }
            catch (Exception ex)
            {
                swas.BAL.Utility.Error.ExceptionHandle(ex.Message);
                int dynamicEventId = DateTime.UtcNow.Ticks.GetHashCode();
                var eventId = new EventId(dynamicEventId, "Edit");
                _logger.Log(LogLevel.Error, eventId, "An error occurred while on CurrentSearch in SearchController.", ex, (s, e) => $"{s} - {e?.Message}");
                return RedirectToAction("Error", "Home");
            }
        }

      
        [Authorize(Policy = "StakeHolders")]
        [HttpPost]
        public async Task<IActionResult> SearchActionResult([FromBody] SearObj seo)
        {
            string[] searchActionResult = seo.searchStakename ?? Array.Empty<string>();
            string TimeStampFrom = seo.TimeStampFrom;
            string TimeStampTo = seo.TimeStampTo;
            var ipAddress = HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
            var currentDatetime = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");
            var watermarkText = $" {ipAddress}\n  {currentDatetime}";
            TempData["ipadd"] = watermarkText;
            try
            {
                Login Logins = SessionHelper.GetObjectFromJson<Login>(HttpContext.Session, "User");

                if (Logins.IsNotNull())
                {
                    int? stakeholderId = Logins.unitid;

                    DateTime fromDate;
                    DateTime toDate;

                    if (string.IsNullOrWhiteSpace(TimeStampFrom) || string.IsNullOrWhiteSpace(TimeStampTo))
                    {
                        // If TimeStampFrom or TimeStampTo is not provided or empty, set a wide date range to include all dates.
                        fromDate = DateTime.MinValue;
                        toDate = DateTime.MaxValue;
                    }
                    else
                    {
                        fromDate = DateTime.ParseExact(TimeStampFrom, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                        toDate = DateTime.ParseExact(TimeStampTo, "yyyy-MM-dd", CultureInfo.InvariantCulture);

                    }

                    var projectsQueryAction = await (from a in _context.Projects
                                                     join b in _context.ProjStakeHolderMov on a.CurrentPslmId equals b.PsmId into bs
                                                     from e in bs.DefaultIfEmpty()
                                                     join actm in _context.TrnStatusActionsMapping on e.StatusActionsMappingId equals actm.StatusActionsMappingId 
                                                     join d in _context.mStatus on actm.StatusId equals d.StatusId into ds
                                                     from eWithStatus in ds.DefaultIfEmpty()
                                                     join c in _context.tbl_mUnitBranch on e.ToUnitId equals c.unitid into cs
                                                     from eWithUnit in cs.DefaultIfEmpty()
                                                     join g in _context.tbl_mUnitBranch on e.FromUnitId equals g.unitid into cg
                                                     from eWithUnits in cg.DefaultIfEmpty()
                                                     join j in _context.mStages on eWithStatus.StageId equals j.StagesId into js
                                                     from eWithStages in js.DefaultIfEmpty()
                                                     join k in _context.mActions on actm.ActionsId equals k.ActionsId into ks
                                                     from eWithAction in ks.DefaultIfEmpty()
                                                     join f in _context.Comment on a.CurrentPslmId equals f.PsmId into fs
                                                     from eWithComment in fs.DefaultIfEmpty()
                                                     where a.IsActive && !a.IsDeleted
                                                         && (searchActionResult == null || searchActionResult.Length == 0 || searchActionResult.Contains(eWithAction.Actions))
                                                        && (string.IsNullOrWhiteSpace(TimeStampFrom) || string.IsNullOrWhiteSpace(TimeStampTo) ||
                                                        (e.TimeStamp >= fromDate && e.TimeStamp <= toDate) &&
                                                         (searchActionResult == null || searchActionResult.Length == 0 || searchActionResult.Any(selectedAction => eWithAction.Actions.Contains(selectedAction)))
                                                           )
                                                     orderby e.DateTimeOfUpdate ascending
                                                     select new
                                                     {
                                                         ProjectId = a.ProjId,
                                                         ProjectName = a.ProjName,
                                                         InitiatedDate = a.InitiatedDate,
                                                         Stage = eWithStages.Stages,
                                                         Status = eWithStatus.Status,
                                                         HeldWith = eWithUnit.UnitName,
                                                         Action = eWithAction.Actions,
                                                         Comment = eWithComment.Comment,
                                                         EncyID = _dataProtector.Protect(a.ProjId.ToString())
                                                     }).ToListAsync();

                    return Json(projectsQueryAction);
                }
                else
                {
                    return Json(0);
                    //return Redirect("/Identity/Account/Login");
                }
            }
            catch (Exception ex)
            {
                swas.BAL.Utility.Error.ExceptionHandle(ex.Message);

                int dynamicEventId = DateTime.UtcNow.Ticks.GetHashCode();
                var eventId = new EventId(dynamicEventId, "Edit");
                _logger.Log(LogLevel.Error, eventId, "An error occurred while on SearchActionResult in SearchController.", ex, (s, e) => $"{s} - {e?.Message}");

                return RedirectToAction("Error", "Home");
            }
        }

     
        ///Created and Reviewed by : Sub Maj Sanal
        [Authorize(Policy = "Admin")]
        [HttpGet]
        public IActionResult GetProjLogview()
        {
            var ipAddress = HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
            var currentDatetime = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");
            var watermarkText = $" {ipAddress}\n  {currentDatetime}";
            TempData["fromDate"] = "";
            TempData["toDate"] = "";
            TempData["ipadd"] = watermarkText;
            List<ProjLogView> ProjLVs = new List<ProjLogView>();
            return View(ProjLVs);


        }
        ///Created and Reviewed by : Sub Maj Sanal
        [Authorize(Policy = "Admin")]
        [HttpPost]
        public async Task<IActionResult> GetProjLogview(string fromDate, string toDate)
        {
            try
            {
                TempData["fromDate"] = fromDate;
                TempData["toDate"] = toDate;
                var ipAddress = HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
                var currentDatetime = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");
                var watermarkText = $" {ipAddress}\n  {currentDatetime}";

                TempData["ipadd"] = watermarkText;
                List<ProjLogView> ProjLV = await _psmRepository.GetProjLogviewAsync(fromDate, toDate);

                if (ProjLV.IsNotNull())
                {
                    return View(ProjLV);
                }
                else
                {
                    List<ProjLogView> ProjLVs = new List<ProjLogView>();
                    return View(ProjLVs);
                }
            }
            catch (Exception ex)
            {
                int dynamicEventId = DateTime.UtcNow.Ticks.GetHashCode();
                var eventId = new EventId(dynamicEventId, "GetProjLogview");
                _logger.Log(LogLevel.Error, eventId, "An error occurred while on GetProjLogview in SearchController.", ex, (s, e) => $"{s} - {e?.Message}");

                return RedirectToAction("Error", "Home");
            }
            
        }

    }
}
