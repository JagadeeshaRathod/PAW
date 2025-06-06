using Microsoft.AspNetCore.Mvc;
using swas.BAL.Interfaces;
using swas.DAL.Models;
using swas.BAL.DTO;
using swas.BAL.Helpers;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;

using iText.IO.Font;
using iText.IO.Font.Constants;
using iText.Kernel.Font;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas;
using iText.Kernel.Pdf.Extgstate;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using System.Timers;

using System.Web;
using static iText.StyledXmlParser.Jsoup.Select.Evaluator;
using Org.BouncyCastle.Utilities;
using Microsoft.CodeAnalysis;
using Microsoft.Build.Evaluation;
using swas.BAL.Repository;
using Microsoft.EntityFrameworkCore;
using swas.UI.Helpers;
using System.Threading;
using System.Security.Cryptography.Xml;
using iText.Commons.Actions.Contexts;
using Grpc.Core;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using ASPNetCoreIdentityCustomFields.Data;
using System.Globalization;
using System.Configuration;
using Microsoft.Extensions.Options;
using swas.DAL;
using Document = iText.Layout.Document;
using System.IO;
using Microsoft.Extensions.Logging;
using iText.Kernel.Events;

namespace swas.UI.Controllers
{
    public class ProjectsController : Controller
    {

        private readonly IProjectsRepository _projectsRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IDdlRepository _DDLRepository;
        private readonly IDdlRepository _dlRepository;
        private readonly IProjStakeHolderMovRepository _psmRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IAttHistoryRepository _attHistoryRepository;
        private readonly IWebHostEnvironment _environment;
        private readonly IDataProtector _dataProtector;
        private readonly ICommentRepository _commentRepository;
        private readonly IActionsRepository _actionsRepository;
        private readonly IProjComments _projComments;
        private readonly IProjStakeHolderMovRepository _projStakeHolderMovRepository;
        private readonly ApplicationDbContext _dbContext;

        private IWebHostEnvironment webHostEnvironment;
        private System.Timers.Timer aTimer;
        private readonly IStkCommentRepository _stkCommentRepository;
        private readonly IProjStakeHolderMovRepository _stkholdmove;

        private readonly IUnitRepository _unitRepository;
        private readonly IConfiguration _configuration;
        private readonly ILogger<ProjectsController> _logger;

        public ProjectsController(IProjectsRepository projectsRepository, IDdlRepository ddlRepository,
            IProjStakeHolderMovRepository psmRepository, IHttpContextAccessor httpContextAccessor,
            IDdlRepository DDLRepository, IAttHistoryRepository attHistoryRepository,
            IWebHostEnvironment environment, IProjStakeHolderMovRepository stkholdmove,
            IDataProtectionProvider DataProtector, IWebHostEnvironment _webHostEnvironment,
            ICommentRepository commentRepository, IActionsRepository actionsRepository,
            IProjComments projComments, IStkCommentRepository stkCommentRepository,
            IProjStakeHolderMovRepository projStakeHolderMovRepository,
            UserManager<ApplicationUser> userManager, IUnitRepository unitRepository, IConfiguration configuration, ApplicationDbContext context,
            ILogger<ProjectsController> logger

            )
        {
            _projectsRepository = projectsRepository;
            _dlRepository = ddlRepository;
            _psmRepository = psmRepository;
            _httpContextAccessor = httpContextAccessor;
            webHostEnvironment = _webHostEnvironment;

            _DDLRepository = ddlRepository;
            _stkholdmove = stkholdmove;
            _attHistoryRepository = attHistoryRepository;
            _environment = environment;
            _commentRepository = commentRepository;
            _actionsRepository = actionsRepository;
            _dataProtector = DataProtector.CreateProtector("swas.UI.Controllers.ProjectsController");
            _projComments = projComments;
            _stkCommentRepository = stkCommentRepository;
            _projStakeHolderMovRepository = projStakeHolderMovRepository;
            _userManager = userManager;
            _unitRepository = unitRepository;
            _configuration = configuration;
            _dbContext = context;
            _logger = logger;

        }


        [HttpGet]
        public async Task<IActionResult> Index()

        {
            CommonDTO dto = new CommonDTO();

            dto.Projects = await _projectsRepository.GetAllProjectsAsync();

            return View(dto);
        }

        public async Task<IActionResult> Details(int id)
        {

            var project = await _projectsRepository.GetProjectByIdAsync(id);
            if (project == null)
            {
                return NotFound();
            }

            return Json(new { success = true, project });
        }



        [HttpGet]

        public async Task<IActionResult> ProjStatDashBdView(string? id, string? status)
        {
            string EncyID = id;
            Login Logins = SessionHelper.GetObjectFromJson<Login>(_httpContextAccessor.HttpContext.Session, "User");

            if (Logins.UserName != null && EncyID != null)
            {
                int dataProjId = 0;

                var ipAddress = HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
                var currentDatetime = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");
                var watermarkText = $" {ipAddress}\n  {currentDatetime}";
                TempData["ipadd"] = watermarkText;

                if (EncyID != null)
                {
                    try
                    {
                        string decryptedValue = _dataProtector.Unprotect(EncyID);
                        dataProjId = int.Parse(decryptedValue);
                    }
                    catch (Exception ex)
                    {
                        swas.BAL.Utility.Error.ExceptionHandle(ex.Message);
                        return Redirect("~/Home/Error");
                    }

                    // sanal
                    MailBox mbx = new MailBox();

                    mbx.SendItems = null;

                    return View(mbx);
                }
                return null;
            }

            else
            {
                return Redirect("~/Identity/Account/Login");
            }

        }

        [HttpGet]
        public async Task<IActionResult> ProjDetails()
        {
            try
            {
                Login Logins = SessionHelper.GetObjectFromJson<Login>(_httpContextAccessor.HttpContext.Session, "User");
                if (Logins != null)
                {
                    var ipAddress = HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
                    var currentDatetime = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");
                    var watermarkText = $" {ipAddress}\n  {currentDatetime}";
                    TempData["ipadd"] = watermarkText;
                    ViewBag.SubmitCde = "0";

                    MailBox mbx = new MailBox();

                    //ViewBag.unitid = Logins.unitid;

                    var notificationContent = _configuration.GetSection("NotificationContent").Get<NotificationContent>();
                    ViewBag.NotificationContent = notificationContent;


                    if (Logins != null && Logins.unitid != null)
                    {
                        ViewBag.unitid = Logins.unitid;
                    }


                    mbx.InBox = await _projectsRepository.GetActInboxAsync();

                    mbx.Draft = await _projectsRepository.GetActDraftItemsAsync();


                    mbx.SendItems = await _projectsRepository.GetActSendItemsAsync();
                    mbx.CompletedItems = await _projectsRepository.GetActComplettemsAsync();


                    return View(mbx);


                    //var projects = await _projectsRepository.GetActProjectsAsync();
                    //return View(projects);
                }
                else
                {
                    return LocalRedirect("~/Identity/Account/Login");
                }
            }
            catch (Exception ex)
            {
                swas.BAL.Utility.Error.ExceptionHandle(ex.Message);
                return Redirect("/Home/Error");
            }
        }



        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GetAllStatus()
        {
            var ss = await _dlRepository.ddlStatus();
            return View(ss);

        }

        #region CreateProject
        [HttpGet]
        //[Authorize(Policy = "StakeHolders")]
        public async Task<IActionResult> Create(string id)
        {
            try
            {


                var options = _configuration.GetSection("WhitelistStatusOptions").Get<List<SelectListItem>>();
                options.Insert(0, new SelectListItem { Text = "--Select--", Value = "", Disabled = true, Selected = true });
                ViewBag.WhitelistOptions = options;


                var TypeofSW = _configuration.GetSection("TypeofSWOptions").Get<List<SelectListItem>>();
                TypeofSW.Insert(0, new SelectListItem { Text = "--Select--", Value = "", Disabled = true, Selected = true });
                ViewBag.TypeofSWOption = TypeofSW;

                var BeingDevpInhouse = _configuration.GetSection("BeingDevpInhouseOptions").Get<List<SelectListItem>>();
                BeingDevpInhouse.Insert(0, new SelectListItem { Text = "--Select--", Value = "", Disabled = true, Selected = true });
                ViewBag.BeingDevpInhouseOption = BeingDevpInhouse;


                var EndorsmentbyHeadof = _configuration.GetSection("EndorsmentbyHeadofOptions").Get<List<SelectListItem>>();
                EndorsmentbyHeadof.Insert(0, new SelectListItem { Text = "--Select--", Value = "", Disabled = true, Selected = true });
                ViewBag.EndorsmentbyHeadofOption = EndorsmentbyHeadof;

                var notificationContent = _configuration.GetSection("NotificationContent").Get<NotificationContent>();
                ViewBag.NotificationContent = notificationContent;



                int ids = 0;
                if (id != null)
                {
                    string decryptedValue = _dataProtector.Unprotect(id);
                    ids = int.Parse(decryptedValue);
                    tbl_Projects tbl_Projects = new tbl_Projects();
                    tbl_Projects = await _projectsRepository.GetProjectByPsmIdAsync(ids);


                    //ViewBag.Projects = await _projectsRepository.GetMyProjectsAsync();
                    ViewBag.ProjectEncyId = id;
                    ViewBag.Projects = await _projectsRepository.GetMyProjects();
                    return View(tbl_Projects);

                }
                TempData["SubCde"] = false;
                TempData.Keep("SubCde");



                Login Logins = SessionHelper.GetObjectFromJson<Login>(_httpContextAccessor.HttpContext.Session, "User");
                if (Logins != null)
                {
                    //UnitDdlGet(); projpsmided
                    var ipAddress = HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
                    var currentDatetime = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");
                    var watermarkText = $" {ipAddress}\n  {currentDatetime}";

                    //ViewBag.Projects = await _projectsRepository.GetMyProjectsAsync();
                    ViewBag.Projects = await _projectsRepository.GetMyProjects();
                    return View(null);

                }
                else
                {
                    return Redirect("/Identity/Account/login");
                }

            }
            catch (Exception ex)
            {
                swas.BAL.Utility.Error.ExceptionHandle(ex.Message);
                return Redirect("/Home/Error");
            }

        }
        //[Authorize(Policy = "StakeHolders")]
        [HttpPost]
        [RequestFormLimits(MultipartBodyLengthLimit = 83886080)]
        public async Task<IActionResult> UploadMultiFile(IFormFile uploadfile, string Reamarks, int PsmId)
        {
            Login Logins = SessionHelper.GetObjectFromJson<Login>(_httpContextAccessor.HttpContext.Session, "User");
            if (uploadfile != null && uploadfile.Length <= 10 * 1024 * 1024)
            {
                if (uploadfile != null && uploadfile.Length > 0)
                {

                    string uniqueFileName = $"{"Swas"}_{Guid.NewGuid()}{System.IO.Path.GetExtension(uploadfile.FileName)}";
                    if (System.IO.Path.GetExtension(uniqueFileName).ToLower() == ".pdf")
                    {
                        string filePath = System.IO.Path.Combine(_environment.ContentRootPath, "wwwroot/Uploads/", uniqueFileName);


                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            uploadfile.CopyTo(stream);
                        }

                        // var project = await _projectsRepository.GetProjectByIdAsync(ProjectId);
                        if (PsmId != null && PsmId != 0)
                        {
                            tbl_AttHistory atthis = new tbl_AttHistory();
                            atthis.ActionId = 0;
                            atthis.AttPath = uniqueFileName;

                            atthis.Reamarks = Reamarks;
                            atthis.PsmId = PsmId;
                            atthis.UpdatedByUserId = Logins.unitid;
                            //atthis.DateTimeOfUpdate = DateTime.Now;
                            atthis.IsDeleted = false;
                            atthis.IsActive = true;
                            atthis.EditDeleteBy = Logins.unitid;
                            atthis.EditDeleteDate = DateTime.Now;
                            atthis.TimeStamp = DateTime.Now;
                            atthis.ActFileName = uploadfile.FileName;

                            await _attHistoryRepository.AddAttHistoryAsync(atthis);
                        }
                        else
                        {
                            return Json(-1);
                        }
                    }
                    else
                    {
                        return Json(-2);
                    }
                }
            }
            else
            {
                return Json(-5);
            }
            return Json(1);

        }
        public async Task<IActionResult> AddProject(tbl_Projects Data)
        {

            try
            {
                //int i = 20;
                //int j = i / Convert.ToInt32("K");
                Login Logins = SessionHelper.GetObjectFromJson<Login>(_httpContextAccessor.HttpContext.Session, "User");
                int projid = 0;
                Data.DateTimeOfUpdate = Data.InitiatedDate;
                Data.StakeHolderId = Logins.unitid ?? 0;
                Data.IsActive = true;
                Data.EditDeleteDate = Data.InitiatedDate;
                Data.EditDeleteBy = 0;
                Data.IsDeleted = false;
                Data.IsSubmited = false;
                Data.UpdatedByUserId = Logins.UserIntId;
                Data.Comments = Data.InitialRemark;
                Data.InitiatedDate = Data.InitiatedDate;
                if (!await _projectsRepository.ProjectNameExists(Data))
                {
                    if (Data.ProjId == 0)
                    {
                        Data.CurrentPslmId = 0;
                        projid = await _projectsRepository.AddProjectAsync(Data);
                        Data = await _projectsRepository.GetProjectByIdAsync(projid);
                    }
                    else
                    {

                        Data.EditDeleteDate = DateTime.Now;
                        await _projectsRepository.UpdateProjectAsync(Data);
                        Data = await _projectsRepository.GetProjectByIdAsync(Data.ProjId);

                    }
                    return Json(Data);
                }
                else
                {
                    return Json(-3);
                }
            }
            catch (Exception ex)
            {
                int dynamicEventId = DateTime.UtcNow.Ticks.GetHashCode();
                var eventId = new EventId(dynamicEventId, "AddProjectError");

                _logger.Log(LogLevel.Error, eventId, "An error occurred while adding a project in ProjectsController.", ex, (s, e) => $"{s} - {e?.Message}");
                swas.BAL.Utility.Error.ExceptionHandle("Add Project:-" + ex.Message);
                return Json("Error :-" + ex.Message);
            }
        }

        public async Task<IActionResult> GetAtthHistoryByProjectId(int PslmId)
        {
            try
            {

                return Json(await _attHistoryRepository.GetAttHistoryByIdAsync(PslmId));

            }
            catch (Exception ex)
            {
                return Json(-1);
            }
        }
        public async Task<IActionResult> DeleteProjects(int ProjectId)
        {
            var ret = await _projectsRepository.DeleteProjectAsync(ProjectId);
            if (ret == true)
                return Json(1);
            else
                return Json(0);
        }

        [HttpPost]
        public async Task<IActionResult> ProjectSubmited(int projid, int type)
        {
            try
            {
                var project = await _projectsRepository.GetProjectByIdAsync(projid);
                if (type == 1)
                {
                    project.IsSubmited = true;
                }
                else
                {
                    project.IsSubmited = false;
                }
                await _projectsRepository.UpdateProjectAsync(project);
                return Json(project.ProjId);
            }
            catch (Exception ex)
            {
                int dynamicEventId = DateTime.UtcNow.Ticks.GetHashCode();
                var eventId = new EventId(dynamicEventId, "ProjectSubmited");
                _logger.Log(LogLevel.Error, eventId, "An error occurred while Projected Submited in ProjectsController.", ex, (s, e) => $"{s} - {e?.Message}");
                return Json(-1);
            }

        }
        [HttpPost]
        public async Task<IActionResult> FwdProjConfirm(int PslmId)
        {
            try
            {
                Login Logins = SessionHelper.GetObjectFromJson<Login>(_httpContextAccessor.HttpContext.Session, "User");
                var ipAddress = HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
                if (Logins != null)
                {
                    try
                    {
                        tbl_ProjStakeHolderMov psmove = new tbl_ProjStakeHolderMov();
                        // var project = await _projectsRepository.GetProjectByIdAsync(projid);
                        psmove = await _projectsRepository.GettXNByPsmIdAsync(PslmId);
                        //psmove.DateTimeOfUpdate = DateTime.Now;
                        psmove.IsComplete = true;
                        await _projectsRepository.UpdateTxnAsync(psmove);
                        return Json(PslmId);
                    }
                    catch (Exception ex)
                    {
                        swas.BAL.Utility.Error.ExceptionHandle(ex.Message);
                        return Json(0);
                    }
                }
                else
                {
                    return Redirect("/Identity/Account/login");
                }
            }
            catch (Exception ex)
            {
                int dynamicEventId = DateTime.UtcNow.Ticks.GetHashCode();
                var eventId = new EventId(dynamicEventId, "FwdProjConfirm");
                _logger.Log(LogLevel.Error, eventId, "An error occurred while Fwd Proj Confirm in ProjectsController.", ex, (s, e) => $"{s} - {e?.Message}");
                return Json(-1);
            }

        }

        [HttpPost]
        public async Task<IActionResult> IsReadInbox(int PsmId)
        {
            try
            {
                Login Logins = SessionHelper.GetObjectFromJson<Login>(_httpContextAccessor.HttpContext.Session, "User");
                if (Logins != null)
                {
                    try
                    {
                        tbl_ProjStakeHolderMov psmove = new tbl_ProjStakeHolderMov();
                        // var project = await _projectsRepository.GetProjectByIdAsync(projid);
                        psmove = await _projectsRepository.GettXNByPsmIdwithUnitId(PsmId, Convert.ToInt32(Logins.unitid));
                        if (psmove != null)
                        {
                            //psmove.DateTimeOfUpdate = DateTime.Now;
                            psmove.IsRead = true;
                            await _projectsRepository.UpdateTxnAsync(psmove);
                            return Json(PsmId);
                        }
                        else
                            return Json(0);
                    }
                    catch (Exception ex)
                    {
                        swas.BAL.Utility.Error.ExceptionHandle(ex.Message);
                        return Json(0);
                    }
                }
                else
                {
                    return Redirect("/Identity/Account/login");
                }
            }
            catch (Exception ex)
            {
                int dynamicEventId = DateTime.UtcNow.Ticks.GetHashCode();
                var eventId = new EventId(dynamicEventId, "IsReadInbox");
                _logger.Log(LogLevel.Error, eventId, "An error occurred while IsRead Inbox in ProjectsController.", ex, (s, e) => $"{s} - {e?.Message}");
                return Json(-1);
            }

        }


        //[HttpPost]
        //public async Task<IActionResult> IsReadNotificationInbox(int ProjId)
        //{
        //    var loginUser = SessionHelper.GetObjectFromJson<Login>(_httpContextAccessor.HttpContext.Session, "User");
        //    if (loginUser == null)
        //    {
        //        return Redirect("/Identity/Account/login");
        //    }

        //    try
        //    {
        //        var notify = await _projectsRepository.GetNotificationByProjId(ProjId);
        //        if (notify != null)
        //        {
        //            notify.ReadDateTime = DateTime.Now;
        //            notify.IsRead = true;

        //            var updateResult = await _projectsRepository.UpdateNotificationByProjID(notify);
        //            if (updateResult)
        //            {
        //                return Json(ProjId);
        //            }
        //        }

        //        return Json(0);
        //    }
        //    catch (Exception ex)
        //    {
        //        swas.BAL.Utility.Error.ExceptionHandle(ex.Message);
        //        return Json(0);
        //    }
        //}


        //[HttpPost]
        //public async Task<IActionResult> IsReadNotification(int ProjId)
        //{
        //    var loginUser = SessionHelper.GetObjectFromJson<Login>(_httpContextAccessor.HttpContext.Session, "User");
        //    if (loginUser == null)
        //    {
        //        return Redirect("/Identity/Account/login");
        //    }

        //    try
        //    {
        //        var notify = await _projectsRepository.GetNotificationByProjId(ProjId);
        //        if (notify != null)
        //        {
        //            //notify.ReadDateTime = DateTime.Now;
        //            //notify.IsRead = true;

        //            var updateResult = await _projectsRepository.UpdateNotification(notify);
        //            if (updateResult)
        //            {
        //                return Json(ProjId);
        //            }
        //        }

        //        return Json(0);
        //    }
        //    catch (Exception ex)
        //    {
        //        swas.BAL.Utility.Error.ExceptionHandle(ex.Message);
        //        return Json(0);
        //    }
        //}

        [HttpPost]
        public async Task<IActionResult> IsProcessProjConfirm(int ProjId)
        {
            try
            {
                Login Logins = SessionHelper.GetObjectFromJson<Login>(_httpContextAccessor.HttpContext.Session, "User");

                var ipAddress = HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
                var currentDatetime = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");
                var watermarkText = $" {ipAddress}\n  {currentDatetime}";
                TempData["ipadd"] = watermarkText;
                if (Logins != null)
                {

                    try
                    {
                        tbl_Projects proj = new tbl_Projects();
                        // var project = await _projectsRepository.GetProjectByIdAsync(projid);
                        proj = await _projectsRepository.GetProjectByIdAsync(ProjId);
                        //proj.DateTimeOfUpdate = DateTime.Now;
                        proj.IsProcess = true;

                        await _projectsRepository.UpdateProjectAsync(proj);
                        return Json(ProjId);
                    }
                    catch (Exception ex)
                    {
                        swas.BAL.Utility.Error.ExceptionHandle(ex.Message);
                        return Json(0);
                    }
                }
                else
                {
                    return Redirect("/Identity/Account/login");
                }
            }
            catch (Exception ex)
            {
                int dynamicEventId = DateTime.UtcNow.Ticks.GetHashCode();
                var eventId = new EventId(dynamicEventId, "IsProcessProjConfirm");
                _logger.Log(LogLevel.Error, eventId, "An error occurred while Process Project Confirm in ProjectsController.", ex, (s, e) => $"{s} - {e?.Message}");
                return Json(-1);
            }

        }
        public async Task<IActionResult> DeleteAttech(int AttechId)
        {
            try
            {
                Login Logins = SessionHelper.GetObjectFromJson<Login>(_httpContextAccessor.HttpContext.Session, "User");

                if (Logins != null)
                {
                    var ret = await _attHistoryRepository.DeleteAttHistoryAsync(AttechId);

                    if (ret == null)
                    {
                        return Json(0);
                    }
                    else return Json(1);
                }
                else
                {
                    return Redirect("/Identity/Account/login");
                }

            }
            catch (Exception ex)
            {
                swas.BAL.Utility.Error.ExceptionHandle(ex.Message);
                return Redirect("/Home/Error");
            }


        }
        #endregion

        public byte[] generate2(string Path, string ip)
        {
            try
            {
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    PdfDocument pdfDoc = new PdfDocument(new PdfReader(Path), new PdfWriter(memoryStream));
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
                //Comman.ExceptionHandle(ex.Message);
                return null;
            }
        }

        #region Project Movment For PROcess For Comment
        public async Task<IActionResult> ProcessMail(int ProjId, int unitid, DateTime FwdDateForComment)
        {
            //**
            try
            {
                Login Logins = SessionHelper.GetObjectFromJson<Login>(_httpContextAccessor.HttpContext.Session, "User");
                if (Logins != null)
                {
                    if (ProjId != null)
                    {
                        var project = await _projectsRepository.GetProjectByIdAsync(ProjId);
                        unitid = project.StakeHolderId;
                        int[] stausid = { 26, 31, 37, 21, 21 }; /*  23, 22, 25, 27, 27*/ //ajayupdate
                        int[] unitids = { 4, 3, 5, 1, unitid }; //1,3,4,5
                        int[] skipUnitIds = { 4, 3, 5, 1 };
                        //int[] unitids = { 1, 3, 5, 4, unitid }; //1,3,4,5

                        //bool first21Skipped = false;

                        for (int i = 0; i < stausid.Length; i++)
                        {
                            if (i == 4)
                            {
                                if (skipUnitIds.Contains(unitid) && stausid[i] == 21)
                                {
                                    continue;
                                }
                            }
                            tbl_ProjStakeHolderMov psmove = new tbl_ProjStakeHolderMov();

                            psmove.ProjId = ProjId;
                            psmove.StatusActionsMappingId = stausid[i];
                            //psmove.ActionId = 1;
                            psmove.Remarks = "";
                            psmove.FromUnitId = Logins.unitid ?? 0;
                            psmove.UserDetails = Helper1.LoginDetails(Logins);
                            //psmove.TostackholderDt = DateTime.Now;  

                            psmove.UpdatedByUserId = Logins.unitid; // change with userid
                            psmove.DateTimeOfUpdate = FwdDateForComment;
                            psmove.IsActive = true;

                            psmove.EditDeleteDate = FwdDateForComment;
                            psmove.EditDeleteBy = Logins.unitid;
                            psmove.TimeStamp = FwdDateForComment;
                            psmove.IsComplete = false;
                            psmove.ToUnitId = unitids[i];
                            psmove.IsComment = true;
                            await _psmRepository.AddProjStakeHolderMovAsync(psmove);
                            /* first21Skipped = true; */// Set the flag to true after skipping the first 21
                        }
                        return Json(1);
                    }
                    else
                    {
                        return Json(0);
                    }
                }
                else
                {
                    return Redirect("/Identity/Account/login");
                }
            }
            catch (Exception ex)
            {
                int dynamicEventId = DateTime.UtcNow.Ticks.GetHashCode();
                var eventId = new EventId(dynamicEventId, "ProcessMail");
                _logger.Log(LogLevel.Error, eventId, "An error occurred while ProcessMail in ProjectsController.", ex, (s, e) => $"{s} - {e?.Message}");

                swas.BAL.Utility.Error.ExceptionHandle("Process Mail:-" + ex.Message);
                return Json(-1);
            }
        }


        public async Task<IActionResult> CheckFwdCondition(int ProjId, int StatusId)
        {
            var Ret = await _psmRepository.CheckFwdCondition(ProjId, StatusId);
            return Json(Ret);
        }


        public async Task<IActionResult> FwdToProject(tbl_ProjStakeHolderMov psmove)
        {
            Login Logins = SessionHelper.GetObjectFromJson<Login>(_httpContextAccessor.HttpContext.Session, "User");

            psmove.ProjId = psmove.ProjId;
            psmove.StatusActionsMappingId = psmove.StatusActionsMappingId;
            // psmove.ActionId = psmove.ActionId;
            psmove.Remarks = psmove.Remarks;
            psmove.FromUnitId = Logins.unitid ?? 0;
            psmove.ToUnitId = psmove.ToUnitId; //  
            //psmove.TostackholderDt = DateTime.Now;  
            psmove.UserDetails = Helper.LoginDetails(Logins);
            psmove.UpdatedByUserId = Logins.UserIntId; // change with userid
            psmove.DateTimeOfUpdate = psmove.TimeStamp;
            psmove.IsActive = true;

            psmove.EditDeleteDate = psmove.TimeStamp;
            psmove.EditDeleteBy = Logins.UserIntId;
            psmove.TimeStamp = psmove.TimeStamp;
            psmove.IsComplete = false;
            psmove.IsComment = false;
            psmove.IsPullBack = false;
            if (psmove.FromUnitId == psmove.ToUnitId)
            {
                psmove.IsRead = true;
            }

            var Ret = await _psmRepository.AddWithReturn(psmove);
            if (Ret != null)
            {
                return Json(Ret);
            }
            else
            {
                return Json(nmum.NotSave);
            }

        }

        public async Task<IActionResult> ProjectMovHistory(int ProjectId)
        {
            //  var Ret1 = await _psmRepository.UndoProjectMov(ProjectId);
            var Ret = await _psmRepository.ProjectMovHistory(ProjectId);
            return Json(Ret);
        }
        public async Task<IActionResult> UndoProject(int ProjectId, int PsmId, string Remarks, int StageId)
        {
            try
            {

                if (StageId == 1)
                {
                    //var proj = await _projectsRepository.GetProjectByIdAsync(ProjectId);
                    //proj.IsSubmited = false;

                    //await _projectsRepository.UpdateProjectAsync(proj);

                    // var ret = await _psmRepository.DeleteProjStakeHolderMovAsync(PsmId);
                    return Json(nmum.NotSave);
                }
                else
                {
                    var movent = await _psmRepository.GetByByte(PsmId);
                    //movent.IsActive = false; removed by 12th nov
                    //movent.IsDeleted = true; removed by 12th nov
                    movent.IsRead = false;
                    movent.UndoRemarks = Remarks;
                    movent.IsComplete = true;
                    //movent.DateTimeOfUpdate = DateTime.Now;
                    movent.IsPullBack = true;
                    var Ret = await _psmRepository.UpdateWithReturn(movent);

                    Login Logins = SessionHelper.GetObjectFromJson<Login>(_httpContextAccessor.HttpContext.Session, "User");

                    var psmidold = await _psmRepository.GetLastRecProjectMovForUnod(ProjectId, Logins.unitid);
                    var movent1 = await _psmRepository.GetByByte(psmidold);
                    movent1.Remarks = "";
                    movent1.IsComplete = false;
                    movent1.IsRead = false;
                    var Ret1 = await _psmRepository.UpdateWithReturn(movent);
                }


                return Json(nmum.Update);
            }
            catch (Exception ex)
            {
                int dynamicEventId = DateTime.UtcNow.Ticks.GetHashCode();
                var eventId = new EventId(dynamicEventId, "UndoProject");
                _logger.Log(LogLevel.Error, eventId, "An error occurred while UndoProject in ProjectsController.", ex, (s, e) => $"{s} - {e?.Message}");

                return Json(nmum.Exception);
            }
        }
        #endregion
        #region PullBack

        public async Task<IActionResult> PullBAckProject(int ProjectId, int PsmId, string Remarks, int StageId)
        {
            try
            {
                Login Logins = SessionHelper.GetObjectFromJson<Login>(_httpContextAccessor.HttpContext.Session, "User");
                var movent = new tbl_ProjStakeHolderMov();
                if (StageId == 1)
                {
                    return Json(nmum.NotSave);
                }
                else
                {
                    int psmData = _psmRepository.GetLastRecProjectMov(ProjectId);
                    if (psmData != 0 )
                    {
                        if (psmData == PsmId)
                        {
                            movent = await _psmRepository.GetByByte(psmData);
                            movent.IsRead = true;
                            movent.UndoRemarks = Remarks;
                            movent.IsComplete = true;
                            movent.DateTimeOfUpdate = DateTime.Now;
                            movent.IsPullBack = true;
                            var Ret = await _psmRepository.UpdateWithReturn(movent);
                        }
                        else
                        {
                            movent = await _psmRepository.GetByByte(psmData);
                            movent.IsComplete = true;
                            await _psmRepository.UpdateWithReturn(movent);

                            movent = await _psmRepository.GetByByte(PsmId);                          
                            movent.IsRead = true;
                            movent.UndoRemarks = Remarks;
                            movent.IsComplete = true;
                            movent.DateTimeOfUpdate = DateTime.Now;
                            movent.IsPullBack = true;
                            var Ret = await _psmRepository.UpdateWithReturn(movent);
                        }
                       
                    
                        UnitDtl unitDetail = new UnitDtl();
                        if (psmData != PsmId)
                        {
                            movent = await _psmRepository.GetByByte(psmData);
                            unitDetail = await _unitRepository.GetUnitDtl(movent.ToUnitId);
                        }
                        else
                        {
                            unitDetail = await _unitRepository.GetUnitDtl(movent.ToUnitId);
                        }                       
                        if (unitDetail != null)
                        {
                            //ApplicationUser userdet = await _userManager.FindByNameAsync(unitDetail.UnitName);
                            ApplicationUser userdet = await _projectsRepository.GetUserByUnitId(unitDetail.unitid);
                            if (userdet != null)
                            {
                                //var rankId = Convert.ToInt32(userdet.Rank?.Trim());
                                var rankName = _dbContext.mRank.FirstOrDefault(x => x.Id == userdet.Rank);
                                if (rankName != null)
                                {
                                    movent.UserDetails = rankName.RankName + " " + userdet.Offr_Name.Trim() + " / " + userdet.UserName.Trim() + "";
                                }
                                //movent.UserDetails = Helper.UserInfoDetails(userdet);
                            }
                            else
                            {
                                movent.UserDetails = "";
                            }
                        }
                        //Add New Record For Pull Request                       
                        if (psmData != PsmId)
                        {
                            movent = await _psmRepository.GetByByte(psmData);
                            movent.FromUnitId = movent.ToUnitId;
                        }
                        else
                        {
                            movent.FromUnitId = movent.ToUnitId;
                        }
                        movent.PsmId = 0;
                        movent.ToUnitId = Convert.ToInt32(Logins.unitid);
                        movent.IsComplete = false;
                        movent.IsRead = false;
                        movent.IsPullBack = true;
                        movent.UndoRemarks = null;
                        movent.Remarks = Remarks; /* as discussed with Lt Col Jasjeet sir (keep pulled back remark in Remarks column)*/
                        //movent.UserDetails = Helper.LoginDetails(Logins);
                        movent.UpdatedByUserId = Logins.UserIntId;
                        movent.DateTimeOfUpdate = DateTime.Now;


                        movent.EditDeleteDate = DateTime.Now;
                        movent.EditDeleteBy = Logins.UserIntId;
                        movent.TimeStamp = DateTime.Now;
                        movent.IsComplete = false;
                        movent.IsComment = false;
                        var Ret1 = await _psmRepository.AddWithReturn(movent);
                        return Json(nmum.Update);
                    }
                    return Json(nmum.NotSave);
                }
            }
            catch (Exception ex)
            {
                int dynamicEventId = DateTime.UtcNow.Ticks.GetHashCode();
                var eventId = new EventId(dynamicEventId, "PullBAckProject");
                _logger.Log(LogLevel.Error, eventId, "An error occurred while Pull Back Project in ProjectsController.", ex, (s, e) => $"{s} - {e?.Message}");

                return Json(nmum.Exception);
            }
        }
        #endregion

        #region ProjComments
        public async Task<IActionResult> ProjComments()
        {

            return View();
        }
        public async Task<IActionResult> GetProjCommentsByUnitId(int Id)
        {
            try
            {
                Login Logins = SessionHelper.GetObjectFromJson<Login>(_httpContextAccessor.HttpContext.Session, "User");

                return Json(await _projComments.GetAllStkForComment(Convert.ToInt32(Logins.unitid)));
            }
            catch (Exception ex)
            {
                return Json(nmum.Exception);
            }
        }

        [HttpPost]
        //[Authorize(Policy = "StakeHolders")]
        [RequestFormLimits(MultipartBodyLengthLimit = 26214400)]
        public async Task<IActionResult> SendCommentonProject(IFormFile uploadfile, string Comments, int StkStatusId, int ProjectId, int psmid, DateTime CommentDate)
        {
            try
            {
                StkComment cmmets = new StkComment();
                string uniqueFileName = "";
                Login Logins = SessionHelper.GetObjectFromJson<Login>(_httpContextAccessor.HttpContext.Session, "User");
                ///var psmove = await _projectsRepository.GettXNByPsmIdAsync(psmid);  old code change by kapoor
                int psmove = await _stkCommentRepository.GetCommentStatusByPsmiId(psmid);
                int allowForInfo = _stkCommentRepository.IsAllowForCommentByStkStatusId(StkStatusId);
                if (psmove != 1 || allowForInfo == 1)
                {
                    if (uploadfile != null)
                    {
                        if (uploadfile.Length <= 10485760)
                        {
                            uniqueFileName = $"{"Swas"}_{Guid.NewGuid()}{System.IO.Path.GetExtension(uploadfile.FileName)}";

                            string filePath = System.IO.Path.Combine(_environment.ContentRootPath, "wwwroot/Uploads/", uniqueFileName);


                            using (var stream = new FileStream(filePath, FileMode.Create))
                            {
                                uploadfile.CopyTo(stream);
                            }


                            cmmets.ActFileName = uploadfile.FileName;
                        }
                        else
                        {
                            return Json(nmum.PdfSizeEx);
                        }
                    }

                    cmmets.Attpath = uniqueFileName;
                    cmmets.Comments = Comments;
                    cmmets.PsmId = psmid;
                    cmmets.ProjId = ProjectId;
                    cmmets.UpdatedByUserId = Logins.UserIntId;
                    cmmets.DateTimeOfUpdate = CommentDate;
                    cmmets.IsDeleted = false;
                    cmmets.IsActive = true;
                    cmmets.EditDeleteBy = Logins.unitid;
                    cmmets.EditDeleteDate = CommentDate;
                    cmmets.StkStatusId = StkStatusId;
                    cmmets.UserDetails = Helper.LoginDetails(Logins);
                    cmmets.StakeHolderId = Logins.unitid; ;

                    var projectStkHolderMovementData = await _projectsRepository.GetProjStkHolderMovmentByPsmiId(cmmets.PsmId);
                    if (projectStkHolderMovementData != null)
                    {
                        projectStkHolderMovementData.DateTimeOfUpdate = CommentDate; // To show the comment date on the dashboard btnGetsummay 
                        //projectStkHolderMovementData.TimeStamp = DateTime.Now; // no need to update the TimeStamp on ProjectComment, this will affect the MovHistory of project update by Divyanshu on 12/03/2025
                        var rets = await _projectsRepository.UpdateProjectStkMovementAsync(projectStkHolderMovementData);
                        if (rets != null)
                        {
                            var ret = await _stkCommentRepository.AddWithReturn(cmmets);
                            if (ret != null)
                                return Json(nmum.Save);
                            else
                                return Json(0);
                        }
                        else
                        {
                            return Json(0);
                        }
                    }
                    return Json(0);

                    //if (ret != null)
                    //    return Json(nmum.Save);
                    //else
                    //    return Json(0);
                }

                else
                {
                    return Json(nmum.NotSave);
                }

            }
            catch (Exception ex)
            {
                int dynamicEventId = DateTime.UtcNow.Ticks.GetHashCode();
                var eventId = new EventId(dynamicEventId, "SendCommentonProject");
                _logger.Log(LogLevel.Error, eventId, "An error occurred while Send Comment on Project in ProjectsController.", ex, (s, e) => $"{s} - {e?.Message}");

                return Json(nmum.Exception);
            }
        }


        public async Task<IActionResult> GetAllCommentBypsmId_UnitId(int psmId, int stakeholderId, int projId)
        {
            try
            {
                Login Logins = SessionHelper.GetObjectFromJson<Login>(HttpContext.Session, "User");
                StkComment stkComment = new StkComment();
                stkComment.ProjId = projId;
                //stkComment.StakeHolderId = Logins.unitid;
                var ret = await _stkCommentRepository.GetAllCommentBypsmId_UnitId(stkComment);

                return Json(ret);

            }
            catch (Exception ex)
            {
                return Json(nmum.Exception);
            }
        }
        public async Task<IActionResult> GetCommentStatus(int UnitId)
        {
            var ret = await _projComments.GetCommentStatus(UnitId);
            return Json(ret);
        }
        #endregion



        #region Project History
        [HttpGet]

        public async Task<IActionResult> ProjHistory(string userid, int? dataProjId, int? dtaProjID, string? AttPath, int? psmid, string? Projpin, string? EncyID, EncryModel? encryModel, string Type)
        {
            try
            {
                Login Logins = SessionHelper.GetObjectFromJson<Login>(_httpContextAccessor.HttpContext.Session, "User");

                ViewBag.logins = Logins;
                string actufilename = "";
                string AttDocuDescs = "";

                if (EncyID != null)
                {
                    ViewBag.SubmitCde = true;
                    ViewBag.EncyID = EncyID;
                    ViewBag.Type = Type;
                }
                if (userid == null && dataProjId == null && dtaProjID == null && AttPath == null && psmid == null && EncyID == null)
                {
                    EncyID = ViewBag.EncyID;
                    if (TempData.ContainsKey("Psmiiddel"))
                    {
                        if (TempData["Psmiiddel"] is int)
                        {

                            psmid = (int)TempData["Psmiiddel"];
                            TempData.Remove("Psmiiddel");
                            dataProjId = null;
                            userid = Logins.UserName;
                            ViewBag.SubmitCde = true;
                        }
                    }
                }

                if (encryModel.EncryItem != null)
                {
                    var UnprotectedValue = _dataProtector.Unprotect(encryModel.EncryItem.ToString() ?? "");
                    // Assuming your model type is MyModelClass
                    var originalData = JsonConvert.DeserializeObject<MyRequestModel>(UnprotectedValue);
                    dtaProjID = originalData.DtaProjID;
                    if (dtaProjID == 0)
                    {
                        dtaProjID = null;
                    }

                    AttPath = originalData.AttPath;
                    Projpin = originalData.Projpin;
                    psmid = originalData.PsmId;
                    actufilename = originalData.ActFileName;
                    AttDocuDescs = originalData.AttDocuDesc;

                    ViewBag.SubmitCde = true;
                    encryModel.EncryItem = null;
                }
                else
                {
                    ViewBag.SubmitCde = false;
                }

                if (EncyID != null)
                {

                    TempData["EncyID"] = EncyID;
                }
                else
                {
                    //ViewBag.SubmitCde = false;
                    dataProjId = dataProjId;
                }

                var ipAddress = HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
                var currentDatetime = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");
                var watermarkText = $" {ipAddress}\n  {currentDatetime}";
                TempData["ipadd"] = watermarkText;

                if (EncyID != null)
                {
                    try
                    {
                        string decryptedValue = _dataProtector.Unprotect(EncyID);
                        dataProjId = int.Parse(decryptedValue);
                        ViewBag.IsCommentPsmiId = await _projectsRepository.GetIsCommentPsmiId(dataProjId, Logins.unitid);
                        //dataProjId = await _projectsRepository.GetProjIdByPsmiId(psmId, Logins.unitid);
                    }
                    catch (Exception ex)
                    {
                        string ss = ex.Message;
                        swas.BAL.Utility.Error.ExceptionHandle(ex.Message);
                    }

                }
                else
                {
                    dataProjId = dataProjId;
                }


                int statgeIDMAx = await _stkholdmove.GetlaststageId(dataProjId);
                ViewBag.stageid = statgeIDMAx;
                var projdetails = await _projectsRepository.GetProjectByIdAsync1(dataProjId);


                var dto3 = await _commentRepository.GetCommentByPsmIdAsync(projdetails.CurrentPslmId);

                ViewBag.CommentByStakeholderList = dto3;

                var ProjMovementHist = await _projStakeHolderMovRepository.ProjectMovHistory(dataProjId);
                ViewBag.ProjMovementHist = ProjMovementHist.DTOProjectMovHistorypsmlst;
                ViewBag.ProjMovementHistcomd = ProjMovementHist.DTOProjectMovHistorycmdlst;

                ViewBag.PsmId = psmid ?? 0;
                ViewBag.PjIR = Projpin;





                List<tbl_AttHistory> atthis = new List<tbl_AttHistory>();
                if (dtaProjID != null)
                {

                    List<ProjHistory> prohis = await _projectsRepository.GetProjectHistorybyID(dtaProjID);
                    tbl_Projects projects = await _projectsRepository.GetProjectByIdAsync(dtaProjID ?? 0);


                    if (prohis.Count > 0)
                    {
                        prohis[0].Attachments = AttPath;
                        prohis[0].ActFileName = actufilename;
                        prohis[0].DocumentDesc = AttDocuDescs;
                        //prohis[0].ActFileName = uplo
                    }

                    atthis = await _attHistoryRepository.GetAttHistoryByIdAsync(psmid ?? 0);
                    prohis[0].Atthistory = atthis;
                    prohis[0].ProjectDetl.Add(projects);

                    return View(prohis);
                }


                ViewBag.DataProjId = dataProjId;
                List<ProjHistory> projHistory = await _projectsRepository.GetProjectHistorybyID(Logins.unitid);
                if (dataProjId == null && userid != null)
                {
                    if (projHistory == null)
                    {
                        ViewBag.DataProjId = projHistory.Select(a => a.ProjId).FirstOrDefault();

                        return View(new List<ProjHistory>());
                    }
                }
                else if (psmid > 0)
                {
                    int psmId = psmid ?? 0;

                    tbl_ProjStakeHolderMov psmove = new tbl_ProjStakeHolderMov();

                    psmove = await _psmRepository.GetProjStakeHolderMovByIdAsync(psmId);
                    List<ProjHistory> projHist = await _projectsRepository.GetProjectHistorybyID(psmove.ProjId);
                    tbl_Projects projects = await _projectsRepository.GetProjectByIdAsync(psmove.ProjId);
                    projHist[0].ProjectDetl.Add(projects);
                    ViewBag.DataProjId = projHist.Select(a => a.ProjId).FirstOrDefault();

                    if (projHist != null)
                    {
                        projHist[0].Attachments = AttPath;
                        projHist[0].ActFileName = actufilename;
                        projHist[0].DocumentDesc = AttDocuDescs;
                        atthis = await _attHistoryRepository.GetAttHistoryByIdAsync(psmove.PsmId);
                        projHist[0].Atthistory = atthis;
                    }


                    projHist[0].Attachments = AttPath;
                    projHist[0].ActFileName = actufilename;
                    projHist[0].DocumentDesc = AttDocuDescs;
                    return View(projHist);
                }
                else if (dataProjId > 0)
                {

                    List<ProjHistory> projHist = await _projectsRepository.GetProjectHistorybyID(dataProjId);
                    tbl_Projects projects = await _projectsRepository.GetProjectByIdAsync(dataProjId ?? 0);
                    projHist[0].ProjectDetl.Add(projects);

                    var stholder = await _psmRepository.GetProjStakeHolderMovByIdAsync(projects.CurrentPslmId);

                    ViewBag.DataProjId = projHist.Select(a => a.ProjId).FirstOrDefault();
                    return View(projHist);

                }
                else
                {
                    List<ProjHistory> projHist = await _projectsRepository.GetProjectHistorybyID(dataProjId);
                    tbl_Projects projects = await _projectsRepository.GetProjectByIdAsync(projHist[0].ProjId);
                    projHist[0].ProjectDetl.Add(projects);

                    ViewBag.DataProjId = projHist.Select(a => a.ProjId).FirstOrDefault();
                    if (projHist != null)
                        projHist[0].Attachments = AttPath;
                    projHist[0].ActFileName = actufilename;
                    projHist[0].DocumentDesc = AttDocuDescs;
                    return View(projHist);

                }
                return null;
            }
            catch (Exception ex)
            {
                swas.BAL.Utility.Error.ExceptionHandle(ex.Message);
                return Redirect("/Home/Error");
            }
        }


        #endregion


        #region Attchment Document

        public async Task<IActionResult> AttDetails(int Id)
        {
            Login Logins = SessionHelper.GetObjectFromJson<Login>(_httpContextAccessor.HttpContext.Session, "User");
            if (Logins != null)
            {
                List<tbl_AttHistory> atthis = new List<tbl_AttHistory>();
                atthis = await _attHistoryRepository.GetAttHistoryByIdAsync(Id);
                return PartialView("_attachmetsview", atthis);
            }
            else
            {
                return Redirect("/Identity/Account/login");
            }
        }

        public async Task<IActionResult> AttDetailsRead(int Id)
        {
            Login Logins = SessionHelper.GetObjectFromJson<Login>(_httpContextAccessor.HttpContext.Session, "User");
            if (Logins != null)
            {
                List<tbl_AttHistory> atthis = new List<tbl_AttHistory>();
                atthis = await _attHistoryRepository.GetAttHistoryByIdAsync(Id);
                return PartialView("_attachmetsread", atthis);
            }
            else
            {
                return Redirect("/Identity/Account/login");
            }
        }

        #endregion


        #region Watermarkpdf for attach

        string filepathpdf = "";

        public IActionResult WatermarkWithPdf(string id)
        {
            Login Logins = SessionHelper.GetObjectFromJson<Login>(_httpContextAccessor.HttpContext.Session, "User");
            if (Logins != null)
            {
                try
                {
                    var ip = _httpContextAccessor.HttpContext?.Connection?.RemoteIpAddress?.ToString();

                    var filePath = System.IO.Path.Combine(_environment.WebRootPath, "Uploads\\" + id + "");
                    if (System.IO.File.Exists(filePath))
                    {
                        //filepathpdf = generate2(filePath, ip);
                        Random rnd = new Random();
                        string Dfilename = rnd.Next(1, 1000).ToString() + ".pdf";
                        var pdfBytes = generate2(filePath, ip);
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

                    int dynamicEventId = DateTime.UtcNow.Ticks.GetHashCode();
                    var eventId = new EventId(dynamicEventId, "WatermarkWithPdf");
                    _logger.Log(LogLevel.Error, eventId, "An error occurred while Watermark With Pdf in ProjectsController.", ex, (s, e) => $"{s} - {e?.Message}");

                    //Comman.ExceptionHandle(ex.Message);
                    return Json(0);
                }
            }
            else
            {
                return Redirect("/Identity/Account/login");
            }
        }

        public void OnTimer(Object source, ElapsedEventArgs e)
        {

            try
            {
                var filePath1 = System.IO.Path.Combine(_environment.ContentRootPath, "wwwroot\\DownloadFile\\" + filepathpdf + ".pdf");

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

        #endregion


        [HttpPost]
        public async Task<IActionResult> IsUnReadInbox(int PsmId)
        {
            try
            {
                Login Logins = SessionHelper.GetObjectFromJson<Login>(_httpContextAccessor.HttpContext.Session, "User");

                if (Logins != null)
                {
                    try
                    {
                        tbl_ProjStakeHolderMov psmove = new tbl_ProjStakeHolderMov();
                        psmove = await _projectsRepository.GettXNByPsmIdAsync(PsmId);
                        //psmove.DateTimeOfUpdate = DateTime.Now;
                        psmove.IsRead = false;
                        await _projectsRepository.UpdateTxnAsync(psmove);

                        return Json(PsmId);
                    }
                    catch (Exception ex)
                    {
                        swas.BAL.Utility.Error.ExceptionHandle(ex.Message);
                        return Json(0);
                    }
                }
                else
                {
                    return Redirect("/Identity/Account/login");
                }
            }
            catch (Exception ex)
            {
                int dynamicEventId = DateTime.UtcNow.Ticks.GetHashCode();
                var eventId = new EventId(dynamicEventId, "IsUnReadInbox");
                _logger.Log(LogLevel.Error, eventId, "An error occurred while on IsUnReadInbox in ProjectsController.", ex, (s, e) => $"{s} - {e?.Message}");
                return Json(-1);
            }

        }


        [HttpPost]
        public async Task<IActionResult> IsUnReadComment(int Projid, int PsmId)
        {
            Login Logins = SessionHelper.GetObjectFromJson<Login>(_httpContextAccessor.HttpContext.Session, "User");
            if (Logins != null)
            {
                try
                {
                    // Get all records for the given Projid
                    List<tbl_ProjStakeHolderMov> inboxComments = await _projectsRepository.GetCommentByExcludingPsmId(Projid, Logins.unitid);

                    // Update IsRead to false for all records
                    foreach (var comment in inboxComments)
                    {
                        //comment.DateTimeOfUpdate = DateTime.Now;
                        comment.IsRead = false;
                        await _projectsRepository.UpdateTxnAsync(comment);
                    }

                    return Json(Projid);
                }
                catch (Exception ex)
                {
                    swas.BAL.Utility.Error.ExceptionHandle(ex.Message);
                    int dynamicEventId = DateTime.UtcNow.Ticks.GetHashCode();
                    var eventId = new EventId(dynamicEventId, "IsUnReadComment");
                    _logger.Log(LogLevel.Error, eventId, "An error occurred while on IsUnReadComment in ProjectsController.", ex, (s, e) => $"{s} - {e?.Message}");
                    return Json(0);
                }
            }
            else
            {
                return Redirect("/Identity/Account/login");
            }
        }



        [HttpGet]
        public async Task<JsonResult> GetProjectCommentCount()
        {
            try
            {
                int count = await _projectsRepository.GetNotificationCommentCount();
                return new JsonResult(count); // Returns the count as JSON
            }
            catch (Exception ex)
            {
                // Handle exception and return an appropriate JSON error response
                return new JsonResult(new { message = ex.Message })
                {
                    StatusCode = 500 // HTTP 500 Internal Server Error
                };
            }
        }

        [HttpGet]
        public async Task<JsonResult> GetProjectInboxCount()
        {
            try
            {
                int count = await _commentRepository.GetNotificationInboxCount();
                return new JsonResult(count); // Returns the count as JSON
            }
            catch (Exception ex)
            {
                // Handle exception and return an appropriate JSON error response
                return new JsonResult(new { message = ex.Message })
                {
                    StatusCode = 500 // HTTP 500 Internal Server Error
                };
            }
        }


        [HttpGet]

        public async Task<IActionResult> ProjectMovement(string? ProjName)
        {
            //var ProjectMovementDetail  = await _projStakeHolderMovRepository.ProjectMovement(ProjId);
            return View();

        }
        public async Task<IActionResult> GetProjectMov(int Id)
        {

            try
            {
                var ret = await _projStakeHolderMovRepository.ProjectMovement(Id);
                return Json(ret);
            }
            catch (Exception ex)
            {
                return Json(-1);
            }
        }

        public async Task<IActionResult> ProjectMovementUpdate(tbl_ProjStakeHolderMov psmove)
        {
            Login Logins = SessionHelper.GetObjectFromJson<Login>(_httpContextAccessor.HttpContext.Session, "User");
            var data = await _projectsRepository.GettXNByPsmIdAsync(psmove.PsmId);
            data.ProjId = psmove.ProjId;
            data.StatusActionsMappingId = psmove.StatusActionsMappingId;
            data.Remarks = psmove.Remarks;
            data.ToUnitId = psmove.ToUnitId;
            data.UpdatedByUserId = Logins.UserIntId;
            data.DateTimeOfUpdate = psmove.TimeStamp;
            data.IsActive = true;
            data.EditDeleteDate = psmove.TimeStamp;
            data.EditDeleteBy = Logins.UserIntId;
            data.TimeStamp = psmove.TimeStamp;

            var Ret = await _psmRepository.UpdateWithReturn(data);

            if (Ret != null)
            {

                var nextPsmMove = await _projectsRepository.GetNextPsmMoveAsync(psmove.ProjId, psmove.PsmId);


                if (nextPsmMove != null)
                {

                    UnitDtl unitDetail = new UnitDtl();
                    unitDetail = await _unitRepository.GetUnitDtl(psmove.ToUnitId);
                    if (unitDetail != null)
                    {
                        ApplicationUser userdet = await _userManager.FindByNameAsync(unitDetail.UnitName);
                        if (userdet != null)
                        {
                            //nextPsmMove.UserDetails = Helper.UserInfoDetails(userdet);
                            //var rankId = Convert.ToInt32(userdet.Rank?.Trim());

                            var rankName = _dbContext.mRank.FirstOrDefault(x => x.Id == userdet.Rank);
                            if (rankName != null)
                            {
                                nextPsmMove.UserDetails = rankName.RankName + " " + userdet.Offr_Name.Trim() + " / " + userdet.UserName.Trim() + "";
                            }
                        }

                    }

                    nextPsmMove.FromUnitId = psmove.ToUnitId;

                    await _psmRepository.UpdateWithReturn(nextPsmMove);
                }

                return Json(Ret);
            }


            else
            {
                return Json(nmum.NotSave);
            }
        }


        public async Task<IActionResult> GetALLByProjectName(string? ProjName)
        {
            var ProjectName = await _projectsRepository.GetALLByProjectName(ProjName);
            return Json(ProjectName);
        }


        public async Task<IActionResult> ProcessNotification(int ProjId, int unitid, DateTime FwdDateForComment)
        {
            //**
            try
            {
                Login Logins = SessionHelper.GetObjectFromJson<Login>(_httpContextAccessor.HttpContext.Session, "User");
                if (Logins != null)
                {
                    if (ProjId != null)
                    {
                        var project = await _projectsRepository.GetProjectByIdAsync(ProjId);

                        unitid = project.StakeHolderId;
                        if (unitid == 1)
                        {
                            int[] stausid = { 26, 31, 37, 21 };
                            int[] unitids = { 4, 3, 5, 1 };
                            for (int i = 0; i < stausid.Length; i++)
                            {
                                Notification notify = new Notification();

                                notify.ProjId = ProjId;
                                notify.NotificationFrom = Logins.unitid ?? 0;
                                notify.NotificationTo = unitids[i];
                                notify.IsRead = false;
                                notify.ReadDateTime = FwdDateForComment;
                                notify.NotificationType = 1;

                                await _psmRepository.AddNotificationCommentAsync(notify);

                            }
                        }
                        else
                        {
                            int[] stausid = { 26, 31, 37, 21, 21 };
                            int[] unitids = { 4, 3, 5, 1, unitid };
                            for (int i = 0; i < stausid.Length; i++)
                            {
                                Notification notify = new Notification();

                                notify.ProjId = ProjId;
                                notify.NotificationFrom = Logins.unitid ?? 0;
                                notify.NotificationTo = unitids[i];
                                notify.IsRead = false;
                                notify.ReadDateTime = FwdDateForComment;
                                notify.NotificationType = 1;

                                await _psmRepository.AddNotificationCommentAsync(notify);

                            }
                        }


                        return Json(1);
                    }
                    else
                    {
                        return Json(0);
                    }
                }
                else
                {
                    return Redirect("/Identity/Account/login");
                }
            }
            catch (Exception ex)
            {
                int dynamicEventId = DateTime.UtcNow.Ticks.GetHashCode();
                var eventId = new EventId(dynamicEventId, "ProcessNotification");
                _logger.Log(LogLevel.Error, eventId, "An error occurred while on Process Notification in ProjectsController.", ex, (s, e) => $"{s} - {e?.Message}");
                return Json(-1);
            }
        }

        //[HttpPost]
        //public async Task<IActionResult> IsUnReadNotification(int ProjId)
        //{
        //    var loginUser = SessionHelper.GetObjectFromJson<Login>(_httpContextAccessor.HttpContext.Session, "User");
        //    if (loginUser == null)
        //    {
        //        return Redirect("/Identity/Account/login");
        //    }

        //    try
        //    {
        //        var notify = await _projectsRepository.GetNotificationByProjId(ProjId);
        //        if (notify != null)
        //        {
        //            notify.ReadDateTime = DateTime.Now;
        //            notify.IsRead = false;

        //            var updateResult = await _projectsRepository.UpdateUnReadNotification(notify);
        //            if (updateResult)
        //            {
        //                return Json(ProjId);
        //            }
        //        }

        //        return Json(0);
        //    }
        //    catch (Exception ex)
        //    {
        //        swas.BAL.Utility.Error.ExceptionHandle(ex.Message);
        //        return Json(0);
        //    }
        //}



        //[HttpPost]
        //public async Task<IActionResult> IsCommentedUnreadNotification (int ProjId)
        //{
        //    var loginUser = SessionHelper.GetObjectFromJson<Login>(_httpContextAccessor.HttpContext.Session, "User");
        //    if (loginUser == null)
        //    {
        //        return Redirect("/Identity/Account/login");
        //    }

        //    try
        //    {
        //        var notify = await _projectsRepository.GetNotificationByProjId(ProjId);
        //        if (notify != null)
        //        {
        //            notify.ReadDateTime = DateTime.Now;
        //            notify.IsRead = false;

        //            var updateResult = await _projectsRepository.UpdateCommentedUnReadNotification(notify);
        //            if (updateResult)
        //            {
        //                return Json(ProjId);
        //            }
        //        }

        //        return Json(0);
        //    }
        //    catch (Exception ex)
        //    {
        //        swas.BAL.Utility.Error.ExceptionHandle(ex.Message);
        //        return Json(0);
        //    }
        //}

        [HttpPost]
        public async Task<IActionResult> IsReadComment(int ProjId, int PsmId)
        {

            Login Logins = SessionHelper.GetObjectFromJson<Login>(_httpContextAccessor.HttpContext.Session, "User");
            if (Logins != null)
            {
                try
                {
                    // Get all records for the given Projid
                    tbl_ProjStakeHolderMov inboxComments = await _projectsRepository.GettXNByPsmIdAsync(PsmId);

                    // Update IsRead to false for all records
                    //foreach (var comment in inboxComments)
                    //{
                    if (inboxComments != null)
                    {
                        //inboxComments.DateTimeOfUpdate = DateTime.Now;
                        if (inboxComments.IsRead == false)
                        {
                            inboxComments.IsRead = true;
                            await _projectsRepository.UpdateTxnAsync(inboxComments);
                        }
                    }
                    return Json(ProjId);
                }
                catch (Exception ex)
                {
                    swas.BAL.Utility.Error.ExceptionHandle(ex.Message);
                    int dynamicEventId = DateTime.UtcNow.Ticks.GetHashCode();
                    var eventId = new EventId(dynamicEventId, "IsReadComment");
                    _logger.Log(LogLevel.Error, eventId, "An error occurred while on IsReadComment in ProjectsController.", ex, (s, e) => $"{s} - {e?.Message}");
                    return Json(0);
                }
            }
            else
            {
                return Redirect("/Identity/Account/login");
            }
        }
    }

}
