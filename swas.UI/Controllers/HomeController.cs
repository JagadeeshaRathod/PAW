using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using swas.UI.Models;
using swas.BAL.Helpers;
using swas.BAL.Utility;
using swas.BAL.Interfaces;
using swas.DAL.Models;
using Microsoft.AspNetCore.Authorization;
using swas.BAL.DTO;
using ASPNetCoreIdentityCustomFields.Data;

using Microsoft.AspNetCore.Identity;
using swas.DAL;

using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;


using iText.IO.Font;
using iText.IO.Font.Constants;
using iText.Kernel.Font;

using iText.Kernel.Pdf;

using iText.Kernel.Pdf.Canvas;
using iText.Kernel.Pdf.Extgstate;
using iText.Layout.Properties;


using Microsoft.AspNetCore.DataProtection;
//using Org.BouncyCastle.Asn1.Ocsp;
using System.Timers;
using Color = iText.Kernel.Colors.Color;
using Document = iText.Layout.Document;
using Paragraph = iText.Layout.Element.Paragraph;
using Rectangle = iText.Kernel.Geom.Rectangle;
using Microsoft.AspNetCore.Mvc.Rendering;
using NuGet.Protocol.Plugins;
using MessagePack.Formatters;
using swas.BAL.Repository;
using Org.BouncyCastle.Utilities;
using iText.Commons.Actions.Contexts;

using iText.StyledXmlParser.Jsoup.Nodes;
using Grpc.Core;

namespace swas.UI.Controllers
{
    ///
    ///Developer Name :- Sub Maj M Sanal Kumar
    ///Purpose :-
    ///Authority & Reference :- 
    ///Kind Of Request :- 
    ///Version :- 
    ///Dated :- 29/07/2023  
    ///Remarks :- 
    //manish

    public class HomeController : Controller
    {
        //private readonly ILogger<HomeController> _logger;
        private readonly IProjectsRepository _projectsRepository;
        private readonly IProjStakeHolderMovRepository _stkholdmove;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly Microsoft.AspNetCore.Identity.UserManager<ApplicationUser> _userManager;
        private readonly IChartService _chartService;
        private readonly IDataProtector _dataProtector;
        private readonly IWebHostEnvironment _env;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private IWebHostEnvironment webHostEnvironment;
        private readonly IActionsRepository _ActionsRepository;
        private readonly IAttHistoryRepository _attHistoryRepository;
        private readonly Microsoft.AspNetCore.Identity.RoleManager<IdentityRole> _roleManager;
        private readonly IDdlRepository _dlRepository;
        private readonly ApplicationDbContext _context;
        private readonly IUnitRepository _unitRepository;
        private readonly ICommentRepository _commentRepository;
        DateTime Currentdate = DateTime.Now;
        private System.Timers.Timer aTimer;
        private readonly ILogger<HomeController> _logger;
        //private readonly Microsoft.AspNetCore.Identity.UserManager<ApplicationUser> userManager;

        public HomeController(IProjectsRepository projectsRepository, ICommentRepository commentRepository, SignInManager<ApplicationUser> signInManager, Microsoft.AspNetCore.Identity.UserManager<ApplicationUser> userManager, IDdlRepository dlRepository, ApplicationDbContext context, IUnitRepository unitRepository, IProjStakeHolderMovRepository stkholdmove, IChartService chartService, IWebHostEnvironment _webHostEnvironment, IHttpContextAccessor httpContextAccessor, IWebHostEnvironment env, Microsoft.AspNetCore.Identity.RoleManager<IdentityRole> roleManager, IDataProtectionProvider dataProtector, IActionsRepository actionsRepository, IAttHistoryRepository attHistoryRepository, ILogger<HomeController> logger)
        {
            //  _logger = logger; _repositoryUser = repositoryUser;
            _projectsRepository = projectsRepository;
            _commentRepository = commentRepository;
            _signInManager = signInManager;
            _userManager = userManager;
            _roleManager = roleManager;
            webHostEnvironment = _webHostEnvironment;
            _dlRepository = dlRepository;
            _context = context;
            _unitRepository = unitRepository;
            _stkholdmove = stkholdmove;
            _chartService = chartService;
            _httpContextAccessor = httpContextAccessor;
            _env = env;
            _dataProtector = dataProtector.CreateProtector("swas.UI.Controllers.ProjectsController");
            _ActionsRepository = actionsRepository;
            _attHistoryRepository = attHistoryRepository;
            _logger = logger;
        }



        ///Developer Name :- Sub Maj M Sanal Kumar
        ///Revised on :- 01/10/2023ddgf 
        ///    chart generation corrected df fgdgd sdfsdf dsfdsfsdfsfd dsfdsfdsf fgfdgdfgdfg
        ///    

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {
                Login Logins = SessionHelper.GetObjectFromJson<Login>(HttpContext.Session, "User");
                if (Logins == null)
                {
                    return Redirect("/Identity/Account/Login");
                }

                //ApplicationUser user = await _userManager.FindByNameAsync(Logins.UserName);
                //if (user == null || user.Flag == false)
                //{
                //    return Redirect("/Identity/Account/Login");
                //}
                ViewBag.UnitId = Logins?.unitid;
                return View();
            }
            catch (Exception ex)
            {
                swas.BAL.Utility.Error.ExceptionHandle(ex.Message);
                return Redirect("/Home/Error");
            }
        }
        public async Task<IActionResult> GetDashboardStatusDetails(int StatusId, bool IsDuplicate)
        {
            Login Logins = SessionHelper.GetObjectFromJson<Login>(HttpContext.Session, "User");
            var ss = await _projectsRepository.GetDashboardStatusDetails(StatusId, Convert.ToInt32(Logins.unitid), IsDuplicate);

            return Json(ss);

        }

        public async Task<IActionResult> GetDashboardApproved(int StatusId, int statusActionsMappingId)
        {
            Login Logins = SessionHelper.GetObjectFromJson<Login>(HttpContext.Session, "User");
            var ss = await _projectsRepository.GetDashboardApproved(StatusId, statusActionsMappingId);

            return Json(ss);

        }
        public async Task<IActionResult> GetProjectWiseStatus(int Id)
        {
            Login Logins = SessionHelper.GetObjectFromJson<Login>(HttpContext.Session, "User");
            var ss = await _projectsRepository.GetProjectWiseStatus();

            return Json(ss);

        }
        public async Task<IActionResult> GetProjHoldStatus(int ProjId)
        {
            var ss = await _stkholdmove.ProjectHolsTimeCalculate(ProjId);
            return Json(ss);
        }
        public async Task<IActionResult> ProjectWiseReport()
        {
            return View();
        }
        public async Task<IActionResult> GetDashboardCount(int Id)
        {
            try
            {
                Login Logins = SessionHelper.GetObjectFromJson<Login>(HttpContext.Session, "User");


                return Json(await _stkholdmove.DashboardCount(Convert.ToInt32(Logins.unitid)));
            }
            catch (Exception ex)
            {
                return Json(nmum.Exception);
            }
        }

        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> ProjComments()
        {
            try
            {
                Login Logins = SessionHelper.GetObjectFromJson<Login>(HttpContext.Session, "User");

                var ipAddress = HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
                var currentDatetime = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");
                var watermarkText = $" {ipAddress}\n  {currentDatetime}";
                TempData["ipadd"] = watermarkText;
                var projects = await _projectsRepository.GetProcProjAsync();
                ViewBag.projects = projects;
                var dateTime = DateTime.Now;

                var stakeholders = await _context.tbl_mUnitBranch.Where(a => a.commentreqdid == true || a.unitid == Logins.unitid).ToListAsync();

                ViewBag.stakeholders = stakeholders;


                var Stk_Status = _context.StkStatus.ToList();
                ViewBag.Stk_Status = Stk_Status;

                var Stk_StatusForDte = _context.StkStatus.Where(s => s.StkStatusId == 4).ToList();
                ViewBag.Stk_StatusForDte = Stk_StatusForDte;


                //var queryes = (from comment in _context.StkComment
                //               join stakeholder in _context.tbl_mUnitBranch on comment.StakeHolderId equals stakeholder.unitid
                //               join status in _context.StkStatus on comment.StkStatusId equals status.StkStatusId into statusGroup
                //               from status in statusGroup.DefaultIfEmpty() // Left Join
                //               join project in _context.Projects on comment.ProjId equals project.ProjId // Assuming 'ProjId' is in the 'Stk_Comments' table
                //               orderby comment.StkCommentId descending
                //               select new
                //               {
                //                   StakeholderName = stakeholder.UnitName,
                //                   StatusName = status != null ? status.Status : null,
                //                   Comments = comment.Comments ?? "",
                //                   ProjId = comment.ProjId ?? 0, // Include ProjId
                //                   PsmId = project.CurrentPslmId,
                //                   Date = comment.DateTimeOfUpdate,
                //                   CommentId = comment.StkCommentId,
                //                   StakeholderId = comment.StakeHolderId

                //               }).ToList();

                //var queryes = (from proj in _context.Projects
                //               join mov in _context.ProjStakeHolderMov on proj.ProjId equals mov.ProjId
                //               join stakeholder in _context.tbl_mUnitBranch on proj.StakeHolderId equals stakeholder.unitid
                //               join comment in _context.StkComment on mov.PsmId equals comment.PsmId into com
                //               from comment in com.DefaultIfEmpty()
                //               join status in _context.StkStatus on comment.StkStatusId equals status.StkStatusId into statusGroup
                //               from status in statusGroup.DefaultIfEmpty()
                //               select new
                //               {
                //                   StakeholderName = stakeholder.UnitName,
                //                   StatusName = status.Status ?? "" ,
                //                   Comments = comment.Comments ?? "",
                //                   ProjId = comment.ProjId ?? 0, // Include ProjId
                //                   PsmId = mov.PsmId,
                //                   Date = comment.DateTimeOfUpdate ?? null,
                //                   CommentId = comment.StkCommentId == null ? 0 : comment.StkCommentId,
                //                   StakeholderId = stakeholder.unitid == null ? 0 : stakeholder.unitid
                //               }).ToList();



                //ViewBag.queryes = queryes;
                StkComment stkcm = new StkComment();

                return View("ProjComments", stkcm);
            }
            catch (Exception ex)
            {
                swas.BAL.Utility.Error.ExceptionHandle(ex.Message);
                return Redirect("/Home/Error");
            }
        }

        //public async Task<IActionResult> GetNotification(int ProjId)
        //{
        //    List<Notification> notifications = await _commentRepository.GetNotificationAsync(ProjId);
        //    return View();


        //}

        public async Task<IActionResult> GetNotificationInbox(int ProjId)
        {
            List<Notification> notifications = await _commentRepository.GetNotificationInbox(ProjId);
            return View();


        }




        ///Developer Name :- Sub Maj M Sanal Kumar

        ///Revised on :- 17 & 23 Sep 2023

        ///    chart generation full dynamic tested and error rectified
        ///    
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> indexToPieChart()
        {
            try
            {
                Login Logins = SessionHelper.GetObjectFromJson<Login>(HttpContext.Session, "User");


                var stageNamesList = _context.mStages.Select(stage => stage.Stages).ToList();
                ViewBag.stageNamesList = stageNamesList;

                if (Logins.IsNotNull())
                {

                    //var g = stageNamesList.GroupBy(i => i);
                    //var countDictionary = new Dictionary<string, int>();

                    //foreach (var grp in g)
                    //{
                    //    countDictionary[grp.Key] = grp.Count();
                    //    Console.WriteLine("{0} {1}", grp.Key, grp.Count());
                    //}

                    var sql = @"
SELECT
        s.StatusId,
        s.StageId,
        s.Status,
    s.IsDeleted,
    s.IsActive,
    s.EditDeleteBy,
    s.EditDeleteDate,
    s.UpdatedByUserId,
    s.DateTimeOfUpdate,
    s.InitiaalID,
    s.FininshID,
	s.IsDashboard,
	s.Icon,

s.Statseq,

        COUNT(p.ProjId) AS TotalProj
    FROM
        mStatus s
    LEFT JOIN
        TrnStatusActionsMapping t ON s.StatusId = t.StatusId
		LEFT join 
		ProjStakeHolderMov m on t.StatusActionsMappingId =m.StatusActionsMappingId

    LEFT JOIN
        projects p ON m.PsmId = p.CurrentPslmId AND m.IsActive > 0
    GROUP BY
        s.StatusId,
        s.StageId,
        s.Status,
s.IsDeleted,
    s.IsActive,
    s.EditDeleteBy,
    s.EditDeleteDate,
    s.UpdatedByUserId,
    s.DateTimeOfUpdate,
    s.InitiaalID,
    s.FininshID,
	
s.Statseq,
s.IsDashboard,
	s.Icon

";
                    List<tbl_mStatus> statuses = new List<tbl_mStatus>();

                    statuses = _context.mStatus.FromSqlRaw(sql).ToList();
                    string jsonStatuses = JsonConvert.SerializeObject(statuses); // Serialize to JSON

                    return Content(jsonStatuses, "application/json");

                    //return Json(statuses);
                }

                else
                    return Redirect("/Identity/Account/Login");
            }
            catch (Exception ex)
            {

                swas.BAL.Utility.Error.ExceptionHandle(ex.Message);
                return RedirectToAction("Error", "Home");
            }


        }


        public async Task<IActionResult> indexTo()
        {
            try
            {
                Login Logins = SessionHelper.GetObjectFromJson<Login>(HttpContext.Session, "User");


                if (Logins.IsNotNull())
                {
                    var projects = await _projectsRepository.GetActProjectsAsync();
                    ViewBag.projects = projects;

                    return Json(projects);
                }


                else
                    return Redirect("/Identity/Account/Login");
            }
            catch (Exception ex)
            {

                swas.BAL.Utility.Error.ExceptionHandle(ex.Message);
                return RedirectToAction("Error", "Home");
            }


        }

        //public IActionResult Index()
        //{
        //    return View();
        //}

        ///Developer Name :- Sub Maj M Sanal Kumar

        ///Revised on :- 03 & 10 Sep 2023

        ///    chart generation full dynamic tested and error rectified
        ///    



        [HttpGet]
        public async Task<IActionResult> NewProject()
        {
            try
            {
                Login Logins = SessionHelper.GetObjectFromJson<Login>(HttpContext.Session, "User");
                string userName = TempData["UserName"]?.ToString();
                var ipAddress = HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
                var currentDatetime = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");
                var watermarkText = $" {ipAddress}\n  {currentDatetime}";
                if (Logins.IsNotNull())
                {
                    var userflag = await _userManager.FindByNameAsync(Logins.UserName);

                    var flag = userflag.Flag;
                    ViewBag.flag = flag;

                    //  bool isUserRegistered = await _projectsRepository.UserRegistered(Logins.UserName); comment by kapoor
                    if (true)
                    {
                        ViewBag.ProcessButtonColor = "green";
                        ViewBag.ButtonText = "Sign In";
                        ViewBag.flag = userflag.Flag;

                    }
                    else
                    {

                        ViewBag.ProcessButtonColor = "red";
                        ViewBag.ButtonText = "Sign Up";
                    }



                    ViewBag.LoggedInUserName = Logins.UserName;

                    var users = await _userManager.FindByNameAsync(Logins.UserName);
                    var list = new List<Users>();

                    if (users != null)
                    {
                        var roles = await _roleManager.FindByIdAsync(users.RoleName);
                        var userRole = roles; // Get the first role found for the user
                        ViewBag.userRole = userRole;

                    }
                    // Pass roles data to the view

                    // var data = await _projectsRepository.GetWhitelistedProjAsync();
                    // ViewBag.ProjectList = data;
                    ViewBag.WhiteListed = await _projectsRepository.GetWhiteListedActionProj();
                    // ViewBag.RecentAction = await _projectsRepository.GetRecentActionProj();

                    //ViewBag.RFPProj = await _projectsRepository.GetHoldRFPProj();

                    ViewBag.HoldProj = await _projectsRepository.GetHoldActionProj();


                    ViewBag.ipadd = watermarkText;

                    string path = System.IO.Path.Combine(this.webHostEnvironment.WebRootPath, "PDF");

                    string path1 = System.IO.Path.Combine(this.webHostEnvironment.WebRootPath, "PDFWhiteListed");

                    List<string> WhiteList = new List<string>();

                    List<string> Courses = new List<string>();

                    foreach (string file in Directory.EnumerateFiles(path, "*", SearchOption.AllDirectories))
                    {
                        Courses.Add(file.Replace(path + "\\", ""));
                    }

                    foreach (string file in Directory.EnumerateFiles(path1, "*", SearchOption.AllDirectories))
                    {
                        WhiteList.Add(file.Replace(path1 + "\\", ""));
                    }

                    ViewBag.PdfWhiteListed = WhiteList;

                    ViewBag.Courses = Courses;
                    return View();
                }
                else
                {
                    bool isUserRegistered = false; // Default value if Logins or UserName is null
                    if (!string.IsNullOrEmpty(userName))
                    {

                        ViewBag.ProcessButtonColor = "red";
                        ViewBag.ButtonText = "Sign Up";

                        ViewBag.LoggedInUserName = userName;
                    }


                    TempData["Tabshift"] = 0;
                    ViewBag.corpsId = 0;
                    List<mCommand> cl = await _dlRepository.ddlCommand();
                    if (cl == null)
                    {
                        cl = new List<mCommand>();
                    }

                    // Insert item only if cl is not null
                    if (cl != null)
                    {
                        cl.Insert(0, new mCommand { comdid = 0, Command_Name = "--Select--" });
                    }
                    ViewBag.cl = cl.ToList();

                    List<Types> ty = await _dlRepository.ddlType();
                    ty.Insert(0, new Types { Id = 0, Name = "--Select--" });
                    ViewBag.ty = ty.ToList();
                    ViewBag.ty = ty.ToList();

                    // var data = await _projectsRepository.GetWhitelistedProjAsync();
                    // ViewBag.ProjectList = data;
                    ViewBag.WhiteListed = await _projectsRepository.GetWhiteListedActionProj();
                    // ViewBag.RecentAction = await _projectsRepository.GetRecentActionProj();
                    ViewBag.HoldProj = await _projectsRepository.GetHoldActionProj();
                    //ViewBag.RFPProj = await _projectsRepository.GetHoldRFPProj();



                    ViewBag.ipadd = watermarkText;

                    string path = System.IO.Path.Combine(this.webHostEnvironment.WebRootPath, "PDF");

                    string path1 = System.IO.Path.Combine(this.webHostEnvironment.WebRootPath, "PDFWhiteListed");

                    List<string> WhiteList = new List<string>();

                    List<string> Courses = new List<string>();

                    foreach (string file in Directory.EnumerateFiles(path, "*", SearchOption.AllDirectories))
                    {
                        Courses.Add(file.Replace(path + "\\", ""));
                    }

                    foreach (string file in Directory.EnumerateFiles(path1, "*", SearchOption.AllDirectories))
                    {
                        WhiteList.Add(file.Replace(path1 + "\\", ""));
                    }

                    ViewBag.PdfWhiteListed = WhiteList;

                    ViewBag.Courses = Courses;

                    return View();
                }

            }
            catch (Exception ex)
            {
                swas.BAL.Utility.Error.ExceptionHandle(ex.Message);
                //return Redirect("/Home/Error");
                return View();
            }
        }

        [AllowAnonymous]
        public async Task<IActionResult> UnitAddView()
        {

            ViewBag.corpsId = 0;
            List<mCommand> cl = new List<mCommand>();

            cl = await _dlRepository.ddlCommand();

            cl.Insert(0, new mCommand { comdid = 0, Command_Name = "--Select--" });

            ViewBag.cl = cl.ToList();

            List<Types> ty = new List<Types>();
            ty = await _dlRepository.ddlType();


            ty.Insert(0, new Types { Id = 0, Name = "--Select--" });
            ViewBag.ty = ty.ToList();
            ViewBag.ty = ty.ToList();

            UnitDtl ude = new UnitDtl();
            ude.UnitName = "";


            return View("_unitdetls", ude);
        }

        ///Developer Name :- Sub Maj M Sanal Kumar

        ///Revised on :- 19,20 & 26, 27 Aug 2023

        ///    New Project Creation with multiple forms and upload errors
        ///    



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            swas.BAL.Utility.Error.ExceptionHandle("Request failed & Status Code : " + HttpContext.Response.StatusCode.ToString() ?? Activity.Current?.Id);
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        ///Developer Name :- Sub Maj M Sanal Kumar

        ///Revised on :- 19,20 & 26, 27 Aug 2023

        ///   Required code sync before release *****************  imp
        /// 

        public async void ddlGen()
        {

            ViewBag.corpsId = 0;
            List<mCommand> cl = new List<mCommand>();

            cl = await _dlRepository.ddlCommand();

            cl.Insert(0, new mCommand { comdid = 0, Command_Name = "--Select--" });

            ViewBag.cl = cl.ToList();

            List<Types> ty = new List<Types>();
            ty = await _dlRepository.ddlType();


            ty.Insert(0, new Types { Id = 0, Name = "--Select--" });
            ViewBag.ty = ty.ToList();
            ViewBag.ty = ty.ToList();

        }


        ///Created and Reviewed by : Sub Maj Sanal
        ///Reviewed Date : 30 Jul 23
        ///Tested By :- 
        ///Tested Date : 
        ///Start <summary>
        /// Created and Reviewed by : Sub Maj Sanal
        /// </summary>
        /// <param name="r"></param>
        /// <returns></returns>
        [AllowAnonymous]
        public IActionResult CheckLogin(int r)
        {
            Login Logins = SessionHelper.GetObjectFromJson<Login>(HttpContext.Session, "User");
            if (Logins.IsNotNull())
            {
                //ModelState.AddModelError("", "Other User Already Login Or Not Proper Logout");
                return Json("1");
            }
            else
            {
                return Json("0");

            }

        }
        ///Created and Reviewed by : Sub Maj Sanal
        ///Reviewed Date : 07 Aug 23
        ///Tested By :- 
        ///Tested Date : 
        ///Start <summary>
        /// Created and Reviewed by : Sub Maj Sanal
        /// </summary>
        /// <returns></returns>

        public IActionResult LogOut()
        {
            Login Logins = SessionHelper.GetObjectFromJson<Login>(HttpContext.Session, "User");

            if (Logins.IsNotNull())
            {

                SessionHelper.ClaerObjectAsJson(HttpContext.Session, "User");
                return View();
            }
            else
                return Redirect("/Identity/Account/Login");
        }
        ///end
        ///Created and Reviewed by : Sub Maj Sanal
        ///Reviewed Date : 07 Aug 23
        ///Tested By :- 
        ///Tested Date : 
        ///Start
        //public int GetRoleId(string rolename)
        //{   
        //    int roleid = _repositoryUser.GetRoleId(rolename);
        //    return roleid;

        //}
        ///Created and Reviewed by : Sub Maj Sanal
        ///Reviewed Date : 07 Aug 23
        ///Tested By :- 
        ///Tested Date : 
        ///Start


        string filepathpdf = "";

        public IActionResult WaterMark2(string id)
        {
            //var stream = new FileStream(@"path\to\file", FileMode.Open);
            //return new FileStreamResult(stream, "application/pdf");
            try
            {
                var ip = _httpContextAccessor.HttpContext?.Connection?.RemoteIpAddress?.ToString();
                // var filePath = System.IO.Path.Combine(_env.ContentRootPath, "wwwroot/PDF/" + id + "");
                var filePath = System.IO.Path.Combine(_env.WebRootPath, "PDF\\" + id + "");
                //  var filePath = System.IO.Path.Combine(_env.WebRootPath, "PDF\\" + id + "");
                filepathpdf = generate2(filePath, ip);

                aTimer = new System.Timers.Timer(60000);
                // Hook up the Elapsed event for the timer.
                aTimer.Elapsed += OnTimer;

                aTimer.Enabled = true;
                return Redirect("../../Download/" + filepathpdf + ".pdf");
            }
            catch (Exception ex)
            {
                swas.BAL.Utility.Error.ExceptionHandle(ex.Message);
                //Comman.ExceptionHandle(ex.Message);
                return Json(0);
            }
        }


        public IActionResult WaterMark(string id)
        {
            //var stream = new FileStream(@"path\to\file", FileMode.Open);
            //return new FileStreamResult(stream, "application/pdf");
            try
            {
                var ip = _httpContextAccessor.HttpContext?.Connection?.RemoteIpAddress?.ToString();
                // var filePath = System.IO.Path.Combine(_env.ContentRootPath, "wwwroot/PDF/" + id + "");
                var filePath = System.IO.Path.Combine(_env.WebRootPath, "PDFWhiteListed\\" + id + "");
                //  var filePath = System.IO.Path.Combine(_env.WebRootPath, "PDF\\" + id + "");
                filepathpdf = generate2(filePath, ip);

                aTimer = new System.Timers.Timer(60000);
                // Hook up the Elapsed event for the timer.
                aTimer.Elapsed += OnTimer;

                aTimer.Enabled = true;
                return Redirect("../../Download/" + filepathpdf + ".pdf");
            }
            catch (Exception ex)
            {
                swas.BAL.Utility.Error.ExceptionHandle(ex.Message);
                //Comman.ExceptionHandle(ex.Message);
                return Json(0);
            }
        }


        public void OnTimer(Object source, ElapsedEventArgs e)
        {

            try
            {
                var filePath1 = System.IO.Path.Combine(_env.ContentRootPath, "wwwroot/Download/" + filepathpdf + ".pdf");

                if (System.IO.File.Exists(filePath1))
                {
                    // If file found, delete it    

                    System.IO.File.Delete(filePath1);


                }
            }
            catch (Exception ex)
            {
                //Comman.ExceptionHandle(ex.Message);
            }
        }


        public string generate2(string Path, string ip)
        {
            try
            {
                Random rnd = new Random();
                string Dfilename = rnd.Next(1, 1000).ToString();


                var filePath1 = System.IO.Path.Combine(_env.ContentRootPath, "wwwroot/Download/" + Dfilename + ".pdf");
                PdfDocument pdfDoc = new PdfDocument(new PdfReader(Path), new PdfWriter(filePath1));
                Document doc = new Document(pdfDoc);
                PdfFont font = PdfFontFactory.CreateFont(FontProgramFactory.CreateFont(StandardFonts.HELVETICA));
                Paragraph paragraph = new Paragraph(ip + " " + DateTime.Now).SetFont(font).SetFontSize(30);

                PdfExtGState gs1 = new PdfExtGState().SetFillOpacity(0.2f);
                for (int i = 1; i <= pdfDoc.GetNumberOfPages(); i++)
                {
                    PdfPage pdfPage = pdfDoc.GetPage(i);
                    Rectangle pageSize = pdfPage.GetPageSize();
                    float x = (pageSize.GetLeft() + pageSize.GetRight()) / 2;
                    float y = (pageSize.GetTop() + pageSize.GetBottom()) / 2;
                    PdfCanvas over = new PdfCanvas(pdfPage);
                    over.SaveState();
                    over.SetExtGState(gs1);

                    doc.ShowTextAligned(paragraph, 297, 450, i, TextAlignment.CENTER, VerticalAlignment.MIDDLE, 45);

                    over.RestoreState();
                }

                doc.Close();
                return Dfilename;
            }
            catch (Exception ex)
            {
                swas.BAL.Utility.Error.ExceptionHandle(ex.Message);
                //Comman.ExceptionHandle(ex.Message);
                return "";
            }
        }

        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> indexToBarChartS()
        {
            try
            {
                Login Logins = SessionHelper.GetObjectFromJson<Login>(HttpContext.Session, "User");

                if (Logins.IsNotNull())
                {

                    List<ChartModelS> chtmod = await _chartService.GetChartDataS();





                    string jsonchtmod = JsonConvert.SerializeObject(chtmod); // Serialize to JSON

                    return Content(jsonchtmod, "application/json");


                }

                else
                    return Redirect("/Identity/Account/Login");
            }
            catch (Exception ex)
            {

                swas.BAL.Utility.Error.ExceptionHandle(ex.Message);
                return RedirectToAction("Error", "Home");
            }

        }



        [Authorize(Policy = "Admin")]

        public async Task<IActionResult> indexToBarChart()
        {
            try
            {
                Login Logins = SessionHelper.GetObjectFromJson<Login>(HttpContext.Session, "User");

                if (Logins.IsNotNull())
                {

                    List<ChartModel> chtmod = await _chartService.GetChartData();
                    string jsonchtmod = JsonConvert.SerializeObject(chtmod); // Serialize to JSON

                    return Content(jsonchtmod, "application/json");


                }

                else
                    return Redirect("/Identity/Account/Login");
            }
            catch (Exception ex)
            {

                swas.BAL.Utility.Error.ExceptionHandle(ex.Message);
                return RedirectToAction("Error", "Home");
            }

        }




        [Authorize(Policy = "Admin")]
        public IActionResult StratgicAnalysis()
        {
            return View();
        }

        // Added by Manish  Reviewed by Sub Maj Sanal
        // Reviewed..   Incorrect Stakeholder join corrected && All stkholder comment reqd chjanged on 24 Nov 23
        //  Comment Error Rectified on 20 Nov 23

        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> GetComments(int psmId, int stakeholderId, int projId)
        {
            try
            {
                Login Logins = SessionHelper.GetObjectFromJson<Login>(HttpContext.Session, "User");

                var query = (from comment in _context.StkComment
                             join stakeholder in _context.tbl_mUnitBranch on comment.StakeHolderId equals stakeholder.unitid
                             join status in _context.StkStatus on comment.StkStatusId equals status.StkStatusId into statusGroup
                             from status in statusGroup.DefaultIfEmpty() // Left Join
                             join project in _context.Projects on comment.ProjId equals project.ProjId // Assuming 'ProjId' is in the 'Stk_Comments' table
                             where comment.ProjId == projId // && comment.StakeHolderId == stakeholderId
                             orderby comment.StkCommentId descending
                             select new
                             {
                                 StakeholderName = stakeholder.UnitName,
                                 StatusName = status != null ? status.Status : null,
                                 Comments = comment.Comments,
                                 ProjId = comment.ProjId,
                                 PsmId = project.CurrentPslmId,
                                 Date = comment.DateTimeOfUpdate,
                                 CommentId = comment.StkCommentId,
                                 StakeholderId = comment.StakeHolderId,
                                 state = comment != null ? comment.Attpath : null

                             }).ToList();

                tbl_Projects stkc = new tbl_Projects();
                stkc = await _projectsRepository.GetProjectByPsmIdAsync(psmId);

                //int cntcomment = await _commentRepository.GetNotificationAsync(projId);   //    12/08/24

                ViewBag.ProjDetl = stkc;
                ViewBag.query = query.OrderByDescending(s => s.CommentId).ToList();

                var stakeholders = await _context.tbl_mUnitBranch.Where(a => a.commentreqdid == true || a.unitid == Logins.unitid).ToListAsync();
                ViewBag.stakeholders = stakeholders;


                var notificationsToUpdate = _context.Notification
        .Where(notification => notification.ProjId == projId && notification.IsRead == false && notification.NotificationFrom == stakeholderId)
        .ToList();

                // Update IsRead property to true and set ReadDateTime to current time
                foreach (var notification in notificationsToUpdate)
                {
                    notification.IsRead = true;
                    notification.ReadDateTime = DateTime.Now;
                }

                // Save changes to the database
                await _context.SaveChangesAsync();

                // Return partial view with the filtered comments
                return Json(query);


            }
            catch (Exception ex)
            {
                swas.BAL.Utility.Error.ExceptionHandle(ex.Message);
                return Redirect("/Home/Error");
            }
        }





        public async Task<IActionResult> ProjUnitComments()
        {
            try
            {
                Login Logins = SessionHelper.GetObjectFromJson<Login>(HttpContext.Session, "User");

                var ipAddress = HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
                var currentDatetime = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");
                var watermarkText = $" {ipAddress}\n  {currentDatetime}";
                TempData["ipadd"] = watermarkText;
                var projects = await _projectsRepository.GetProjforCommentsAsync();
                ViewBag.projects = projects;
                var dateTime = DateTime.Now;
                var stakeholders = await _context.tbl_mUnitBranch.Where(a => a.commentreqdid == true || a.unitid == Logins.unitid).ToListAsync();
                ViewBag.stakeholders = stakeholders;
                var Stk_Status = _context.StkStatus.FirstOrDefault(status => status.StkStatusId == 4);
                ViewBag.Stk_Status = Stk_Status;

                var queryes = (from comment in _context.StkComment
                               join stakeholder in _context.tbl_mUnitBranch on comment.StakeHolderId equals stakeholder.unitid
                               join status in _context.StkStatus on comment.StkStatusId equals status.StkStatusId into statusGroup
                               from status in statusGroup.DefaultIfEmpty() // Left Join
                               join project in _context.Projects on comment.ProjId equals project.ProjId // Assuming 'ProjId' is in the 'Stk_Comments' table
                               select new
                               {
                                   StakeholderName = stakeholder.UnitName,
                                   StatusName = status != null ? status.Status : null,
                                   Comments = comment.Comments,
                                   ProjId = comment.ProjId, // Include ProjId
                                   PsmId = project.CurrentPslmId,
                                   Date = comment.DateTimeOfUpdate,
                                   CommentId = comment.StkCommentId,
                                   StakeholderId = comment.StakeHolderId
                                   // Include PsmId (assuming it's in the 'Projects' table)
                               }).ToList();


                ViewBag.queryes = queryes;


                return View("ProjUnitComments");
            }
            catch (Exception ex)
            {
                swas.BAL.Utility.Error.ExceptionHandle(ex.Message);
                return Redirect("/Home/Error");
            }

        }


        public async Task<IActionResult> GetUnitComments(int psmId, int stakeholderId, int projId)
        {
            // Your existing code for getting comments
            try
            {
                Login Logins = SessionHelper.GetObjectFromJson<Login>(HttpContext.Session, "User");

                var query = (from comment in _context.StkComment
                             join stakeholder in _context.tbl_mUnitBranch on comment.StakeHolderId equals stakeholder.unitid
                             join status in _context.StkStatus on comment.StkStatusId equals status.StkStatusId into statusGroup
                             from status in statusGroup.DefaultIfEmpty() // Left Join
                             join project in _context.Projects on comment.ProjId equals project.ProjId // Assuming 'ProjId' is in the 'Stk_Comments' table
                             where comment.ProjId == projId
                             //where project.CurrentPslmId == psmId && comment.StakeHolderId == stakeholderId
                             orderby comment.StkCommentId descending                                                                     //where project.ProjId == projId
                             select new
                             {
                                 StakeholderName = stakeholder.UnitName,
                                 StatusName = status != null ? status.Status : null,
                                 Comments = comment.Comments,
                                 ProjId = comment.ProjId, // Include ProjId
                                 PsmId = project.CurrentPslmId,
                                 Date = comment.DateTimeOfUpdate,
                                 CommentId = comment.StkCommentId,
                                 StakeholderId = comment.StakeHolderId,
                                 state = comment != null ? comment.Attpath : null

                             }).ToList();


                ViewBag.query = query.OrderByDescending(s => s.CommentId).ToList();

                var stakeholders = await _context.tbl_mUnitBranch.Where(a => a.commentreqdid == true || a.unitid == Logins.unitid).ToListAsync();
                ViewBag.stakeholders = stakeholders;


                return Json(query);
            }
            catch (Exception ex)
            {
                swas.BAL.Utility.Error.ExceptionHandle(ex.Message);
                return Redirect("/Home/Error");
            }
        }


        [HttpPost]
        public async Task<IActionResult> UpdateUnitStatus(int StatusId, string Comment, int StakeholderId, int ProjId, int PsmID)
        {
            try
            {
                Login Logins = SessionHelper.GetObjectFromJson<Login>(HttpContext.Session, "User");

                var stkComment = new StkComment
                {
                    StkStatusId = StatusId,
                    Comments = Comment,
                    StakeHolderId = StakeholderId,
                    ProjId = ProjId,
                    PsmId = PsmID,
                    DateTimeOfUpdate = DateTime.Now
                };

                if (ModelState.IsValid && stkComment.Comments != null)
                {
                    // Model is valid, proceed with processing
                    // ...

                    _context.StkComment.Add(stkComment);
                    _context.SaveChanges();
                    var projects = await _projectsRepository.GetActProjectsAsync();
                    ViewBag.projects = projects;

                    var stakeholders = await _context.tbl_mUnitBranch.Where(a => a.commentreqdid == true || a.unitid == Logins.unitid).ToListAsync();
                    ViewBag.stakeholders = stakeholders;


                    var Stk_Status = _context.StkStatus.ToList();
                    ViewBag.Stk_Status = Stk_Status;

                    var model = _context.StkComment.ToList();
                    ViewBag.model = model;

                    var query = (from comment in _context.StkComment
                                 join stakeholder in _context.tbl_mUnitBranch on comment.StakeHolderId equals stakeholder.unitid
                                 join status in _context.StkStatus on comment.StkStatusId equals status.StkStatusId into statusGroup
                                 from status in statusGroup.DefaultIfEmpty() // Left Join
                                 join project in _context.Projects on comment.ProjId equals project.ProjId // Assuming 'ProjId' is in the 'Stk_Comments' table
                                 select new
                                 {
                                     StakeholderName = stakeholder.UnitName,
                                     StatusName = status != null ? status.Status : null,
                                     Comments = comment.Comments,
                                     ProjId = comment.ProjId, // Include ProjId
                                     PsmId = project.CurrentPslmId,
                                     Date = comment.DateTimeOfUpdate
                                 }).ToList();

                    ViewBag.query = query;
                }
                else
                {
                    var projects = await _projectsRepository.GetActProjectsAsync();
                    ViewBag.projects = projects;

                    var stakeholders = await _context.tbl_mUnitBranch.Where(a => a.commentreqdid == true || a.unitid == Logins.unitid).ToListAsync();
                    ViewBag.stakeholders = stakeholders;


                    var Stk_Status = _context.StkStatus.ToList();
                    ViewBag.Stk_Status = Stk_Status;


                    var query = (from comment in _context.StkComment
                                 join stakeholder in _context.tbl_mUnitBranch on comment.StakeHolderId equals stakeholder.unitid
                                 join status in _context.StkStatus on comment.StkStatusId equals status.StkStatusId into statusGroup
                                 from status in statusGroup.DefaultIfEmpty() // Left Join
                                 join project in _context.Projects on comment.ProjId equals project.ProjId // Assuming 'ProjId' is in the 'Stk_Comments' table
                                 select new
                                 {
                                     StakeholderName = stakeholder.unitid,
                                     StatusName = status != null ? status.Status : null,
                                     Comments = comment.Comments,
                                     ProjId = comment.ProjId, // Include ProjId
                                     PsmId = project.CurrentPslmId // Include PsmId (assuming it's in the 'Projects' table)
                                 }).ToList();

                    ViewBag.query = query;
                    return RedirectToActionPermanent("ProjUnitComments", "Home");

                }
                if (stkComment.StkStatusId > 0)
                {

                    return RedirectToAction("Error", "Home");
                }
                else
                {

                    return RedirectToAction("Error", "Home");

                }

            }
            catch (Exception ex)
            {
                int dynamicEventId = DateTime.UtcNow.Ticks.GetHashCode();
                var eventId = new EventId(dynamicEventId, "UpdateUnitStatus");
                _logger.Log(LogLevel.Error, eventId, "An error occurred while on Get All UpdateUnitStatus in CommentController.", ex, (s, e) => $"{s} - {e?.Message}");

                return RedirectToAction("Error", "Home");
            }
        }


        //   created by ajay for unit comments on 24 Nov 23
        string filepathpdf1 = "";

        public IActionResult WaterMark3(string id)
        {
            try
            {
                var ip = _httpContextAccessor.HttpContext?.Connection?.RemoteIpAddress?.ToString();
                var filePath = System.IO.Path.Combine(_env.WebRootPath, "Uploads\\" + id + "");
                
                if (System.IO.File.Exists(filePath))
                {
                    Random rnd = new Random();
                    string Dfilename = rnd.Next(1, 1000).ToString() + ".pdf";
                    var pdfBytes = GeneratePdfInMemory(filePath, ip);                    
                    Response.Headers["Content-Disposition"] = $"inline; filename={Dfilename}";
                    return File(pdfBytes, "application/pdf");
                }
                else
                {
                    return Content("PDF IS NOT IN FOLDER");
                }  
            }
            catch (Exception ex)
            {
                swas.BAL.Utility.Error.ExceptionHandle(ex.Message);
                return Json(0);
            }
        }

        //   created by ajay for unit comments on 24 Nov 23
        public void OnTimer1(Object source, ElapsedEventArgs e)
        {
            try
            {
                var filePath1 = System.IO.Path.Combine(_env.ContentRootPath, "wwwroot\\Download\\" + filepathpdf + ".pdf");
                if (System.IO.File.Exists(filePath1))
                {
                    // If file found, delete it    
                    System.IO.File.Delete(filePath1);
                }
            }
            catch (Exception ex)
            {
                swas.BAL.Utility.Error.ExceptionHandle(ex.Message);
            }
        }


        //   created by ajay for unit comments on 24 Nov 23

        public string generate3(string Path, string ip)
        {
            try
            {
                Random rnd = new Random();
                string Dfilename = rnd.Next(1, 1000).ToString();
                var filePath1 = System.IO.Path.Combine(_env.ContentRootPath, "wwwroot\\Download\\" + Dfilename + ".pdf");
                // var filePath1 = System.IO.Path.Combine(_env.ContentRootPath, "wwwroot/Download/" + Dfilename + ".pdf");
                PdfDocument pdfDoc = new PdfDocument(new PdfReader(Path), new PdfWriter(filePath1));
                Document doc = new Document(pdfDoc);
                PdfFont font = PdfFontFactory.CreateFont(FontProgramFactory.CreateFont(StandardFonts.HELVETICA));
                Paragraph paragraph = new Paragraph(ip + " " + DateTime.Now).SetFont(font).SetFontSize(30);

                PdfExtGState gs1 = new PdfExtGState().SetFillOpacity(0.2f);
                for (int i = 1; i <= pdfDoc.GetNumberOfPages(); i++)
                {
                    PdfPage pdfPage = pdfDoc.GetPage(i);
                    Rectangle pageSize = pdfPage.GetPageSize();
                    float x = (pageSize.GetLeft() + pageSize.GetRight()) / 2;
                    float y = (pageSize.GetTop() + pageSize.GetBottom()) / 2;
                    PdfCanvas over = new PdfCanvas(pdfPage);
                    over.SaveState();
                    over.SetExtGState(gs1);

                    doc.ShowTextAligned(paragraph, 297, 450, i, TextAlignment.CENTER, VerticalAlignment.MIDDLE, 45);

                    over.RestoreState();
                }

                doc.Close();
                return Dfilename;
            }
            catch (Exception ex)
            {

                swas.BAL.Utility.Error.ExceptionHandle(ex.Message);
                return "";
            }
        }


        public byte[] GeneratePdfInMemory(string path, string ip)
        {
            try
            {
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    PdfDocument pdfDoc = new PdfDocument(new PdfReader(path), new PdfWriter(memoryStream));
                    Document doc = new Document(pdfDoc);
                    PdfFont font = PdfFontFactory.CreateFont(FontProgramFactory.CreateFont(StandardFonts.HELVETICA));
                    Paragraph paragraph = new Paragraph(ip + " " + DateTime.Now)
                                            .SetFont(font)
                                            .SetFontSize(30);

                    PdfExtGState gs1 = new PdfExtGState().SetFillOpacity(0.2f);
                    for (int i = 1; i <= pdfDoc.GetNumberOfPages(); i++)
                    {
                        PdfPage pdfPage = pdfDoc.GetPage(i);
                        PdfCanvas over = new PdfCanvas(pdfPage);
                        over.SaveState();
                        over.SetExtGState(gs1);

                        doc.ShowTextAligned(paragraph, 297, 450, i, TextAlignment.CENTER, VerticalAlignment.MIDDLE, 45);

                        over.RestoreState();
                    }

                    doc.Close();
                    return memoryStream.ToArray();
                }
            }
            catch (Exception ex)
            {
                swas.BAL.Utility.Error.ExceptionHandle(ex.Message);
                return null;
            }
        }


        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> StkHDashbard()
        {
            try
            {
                Login Logins = SessionHelper.GetObjectFromJson<Login>(HttpContext.Session, "User");

                if (Logins != null)
                {
                    if (Logins.Role == "Unit")
                    {
                        Login Db = new Login();
                        Db.UserName = Logins.UserName;
                        Db.Comdid = Logins.unitid;
                        Db.Corpsid = Logins.Corpsid;
                        Db.Iamuserid = Logins.Iamuserid;
                        Db.Unit = Logins.Unit;
                        Db.unitid = Logins.unitid;
                        Db.ActualUserName = Logins.ActualUserName;
                        Db.Role = "Dte";

                        SessionHelper.SetObjectAsJson(HttpContext.Session, "User", Db);

                        return Redirect("/Home/Index");
                    }
                    else
                    {


                        return Redirect("/Home/Index");
                    }

                }
                else
                {
                    return Redirect("/Identity/Account/Login");
                }
            }
            catch (Exception ex)
            {
                swas.BAL.Utility.Error.ExceptionHandle(ex.Message);
                return Redirect("/Home/Error");
            }
        }
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> UnitDashbard()
        {
            try
            {
                Login Logins = SessionHelper.GetObjectFromJson<Login>(HttpContext.Session, "User");

                if (Logins != null)
                {
                    if (Logins.Role == "Dte")
                    {
                        Login Db = new Login();
                        Db.UserName = Logins.UserName;
                        Db.Comdid = Logins.unitid;
                        Db.Corpsid = Logins.Corpsid;
                        Db.Iamuserid = Logins.Iamuserid;
                        Db.Unit = Logins.Unit;
                        Db.unitid = Logins.unitid;
                        Db.ActualUserName = Logins.ActualUserName;
                        Db.Role = "Unit";

                        SessionHelper.SetObjectAsJson(HttpContext.Session, "User", Db);

                        return Redirect("/Home/Index");
                    }
                    else
                    {
                        Login Db = new Login();
                        Db.UserName = Logins.UserName;
                        Db.Comdid = Logins.unitid;
                        Db.Corpsid = Logins.Corpsid;
                        Db.Iamuserid = Logins.Iamuserid;
                        Db.Unit = Logins.Unit;
                        Db.unitid = Logins.unitid;
                        Db.ActualUserName = Logins.ActualUserName;
                        Db.Role = "Unit";

                        SessionHelper.SetObjectAsJson(HttpContext.Session, "User", Db);

                        return RedirectToActionPermanent("Index", "Home");
                    }

                }
                else
                {
                    return Redirect("/Identity/Account/Login");
                }
            }
            catch (Exception ex)
            {
                swas.BAL.Utility.Error.ExceptionHandle(ex.Message);
                return Redirect("/Home/Error");
            }
        }

        //public async Task<IActionResult> GetNotification(int stakeHolderId)
        //{
        //    List<Notification> notifications = await _commentRepository.GetNotificationAsync(stakeHolderId);
        //    return View();
        //}

        [HttpGet]
        public async Task<IActionResult> AllProjectDetails()
        {
            try
            {
                Login Logins = SessionHelper.GetObjectFromJson<Login>(HttpContext.Session, "User");
                string userName = TempData["UserName"]?.ToString();
                var ipAddress = HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
                var currentDatetime = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");
                var watermarkText = $" {ipAddress}\n  {currentDatetime}";
                if (Logins.IsNotNull())
                {
                    ViewBag.WhiteListed = await _projectsRepository.GetWhiteListedActionProj();
                    ViewBag.HoldProj = await _projectsRepository.GetHoldActionProj();
                    ViewBag.ipadd = watermarkText;

                    string path = System.IO.Path.Combine(this.webHostEnvironment.WebRootPath, "PDF");

                    string path1 = System.IO.Path.Combine(this.webHostEnvironment.WebRootPath, "PDFWhiteListed");

                    List<string> WhiteList = new List<string>();

                    List<string> Courses = new List<string>();

                    foreach (string file in Directory.EnumerateFiles(path, "*", SearchOption.AllDirectories))
                    {
                        Courses.Add(file.Replace(path + "\\", ""));
                    }

                    foreach (string file in Directory.EnumerateFiles(path1, "*", SearchOption.AllDirectories))
                    {
                        WhiteList.Add(file.Replace(path1 + "\\", ""));
                    }

                    ViewBag.PdfWhiteListed = WhiteList;

                    ViewBag.Courses = Courses;
                    return View();
                }
                else
                {
                    bool isUserRegistered = false; // Default value if Logins or UserName is null


                    TempData["Tabshift"] = 0;
                    ViewBag.corpsId = 0;
                    List<mCommand> cl = await _dlRepository.ddlCommand();
                    if (cl == null)
                    {
                        cl = new List<mCommand>();
                    }

                    // Insert item only if cl is not null
                    if (cl != null)
                    {
                        cl.Insert(0, new mCommand { comdid = 0, Command_Name = "--Select--" });
                    }
                    ViewBag.cl = cl.ToList();

                    List<Types> ty = await _dlRepository.ddlType();
                    ty.Insert(0, new Types { Id = 0, Name = "--Select--" });
                    ViewBag.ty = ty.ToList();
                    ViewBag.ty = ty.ToList();
                    ViewBag.WhiteListed = await _projectsRepository.GetWhiteListedActionProj();
                    // ViewBag.RecentAction = await _projectsRepository.GetRecentActionProj();
                    ViewBag.HoldProj = await _projectsRepository.GetHoldActionProj();
                    //ViewBag.RFPProj = await _projectsRepository.GetHoldRFPProj();



                    ViewBag.ipadd = watermarkText;

                    string path = System.IO.Path.Combine(this.webHostEnvironment.WebRootPath, "PDF");

                    string path1 = System.IO.Path.Combine(this.webHostEnvironment.WebRootPath, "PDFWhiteListed");

                    List<string> WhiteList = new List<string>();

                    List<string> Courses = new List<string>();

                    foreach (string file in Directory.EnumerateFiles(path, "*", SearchOption.AllDirectories))
                    {
                        Courses.Add(file.Replace(path + "\\", ""));
                    }

                    foreach (string file in Directory.EnumerateFiles(path1, "*", SearchOption.AllDirectories))
                    {
                        WhiteList.Add(file.Replace(path1 + "\\", ""));
                    }

                    ViewBag.PdfWhiteListed = WhiteList;

                    ViewBag.Courses = Courses;

                    return View();
                }

            }
            catch (Exception ex)
            {
                swas.BAL.Utility.Error.ExceptionHandle(ex.Message);

                return View();
            }
        }

        [HttpGet]
        public IActionResult getCountertoday()
        {
            var todayStart = DateTime.Today;
            var todayEnd = DateTime.Today.AddDays(1).AddSeconds(-1);
            var lastMonthEnd = DateTime.Now.AddMonths(-1).AddMonths(1).AddDays(-DateTime.Now.AddMonths(-1).AddMonths(1).Day);

            int visitorsToday = _context.tbl_LoginLog
                .Where(ul => ul.logindate >= todayStart && ul.logindate <= todayEnd)
                .Select(ul => ul.unitid)
                .Distinct()
                .Count();

            int visitorsThisMonth = _context.tbl_LoginLog
                .Where(ul => ul.logindate >= lastMonthEnd && ul.logindate <= DateTime.Now.AddMonths(1).AddDays(-DateTime.Now.Day))
                .GroupBy(ul => ul.logindate.Date)
                .Select(g => g.Select(x => x.unitid).Distinct().Count())
                .Sum();

            int totalVisitors = _context.tbl_LoginLog
                .GroupBy(ul => ul.logindate.Date)
                .Select(g => g.Select(x => x.unitid).Distinct().Count())
                .Sum();

            // Populate HitCounterDaily object
            var counter = new HitCounterDaily
            {
                Today = visitorsToday,
                CurrentMonth = visitorsThisMonth,
                Total = totalVisitors
            };

            return Ok(counter);
        }



        public IActionResult SOPDownload()
        {
            try
            {
                string inputPdfPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/SOP/SOP on Whitelisitng of Appl Sw in IA dt 28 May 24.pdf");
                string ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();
                if (System.IO.File.Exists(inputPdfPath))
                {
                    Random rnd = new Random();
                    string Dfilename = rnd.Next(1, 1000).ToString() + ".pdf";
                    var pdfBytes = GeneratePdfInMemory(inputPdfPath, ipAddress);
                    Response.Headers["Content-Disposition"] = $"inline; filename={Dfilename}";
                    return File(pdfBytes, "application/pdf");
                }
                else
                {
                    return Content("PDF IS NOT IN FOLDER");
                }
            }
            catch
            {
                return View("Error");
            }
        }

        [HttpPost]
        public IActionResult ClearSession()
        {
            HttpContext.Session.Clear();
            return Ok();
        }


        [HttpGet]
        public IActionResult GetWhiteListedProjectById(int id)
        {
            var record = _context.trnWhiteListed.FirstOrDefault(x => x.Id == id);
            return Json(record);
        }

        [HttpPost]
        public IActionResult UpdateWhiteListedProject(trnWhiteListed model)
        {
            try
            {
                var record = _context.trnWhiteListed.FirstOrDefault(x => x.Id == model.Id);
                if (record != null)
                {   
                    record.mHostTypeId = model.mHostTypeId;   
                    record.Appt = model.Appt;
                    record.Fmn = model.Fmn;
                    record.ContactNo = model.ContactNo;
                    record.Clearence = model.Clearence;
                    record.CertNo = model.CertNo;
                    record.ValidUpto = model.ValidUpto;
                    record.Remarks = model.Remarks;
                    _context.SaveChanges();

                    return Json(1);
                }
                return Json(-1);
            }
            catch (Exception ex)
            {
                int dynamicEventId = DateTime.UtcNow.Ticks.GetHashCode();
                var eventId = new EventId(dynamicEventId, "UpdateWhiteListedProject");
                _logger.Log(LogLevel.Error, eventId, "An error occurred while on UpdateWhiteListedProject in HomeController.", ex, (s, e) => $"{s} - {e?.Message}");
                return Json(-1);
            }           
        }
    }
}