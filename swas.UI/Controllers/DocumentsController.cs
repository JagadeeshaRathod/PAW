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
using System.Collections.Generic;
using System.IO;
using iText.Kernel.Pdf;
using iText.Layout;
using iText;
using iText.Layout.Element;
using Grpc.Core;
using Microsoft.Extensions.Hosting.Internal;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using iText.Kernel.Pdf.Canvas;
using iText.Kernel.Geom;
using iText.Kernel.Colors;
using iText.Layout.Properties;
using iText.IO.Font.Constants;
using iText.IO.Font;
using iText.Kernel.Font;
using iText.Kernel.Pdf.Extgstate;
using System.Timers;
using static iText.StyledXmlParser.Jsoup.Select.Evaluator;
using iText.Layout.Font;
using Microsoft.AspNetCore.DataProtection;
using Timer = System.Threading.Timer;
using System.Net.Http.Headers;
using System.Net;

namespace swas.UI.Controllers
{
    public class DocumentsController : Controller
    {
        //private readonly ILogger<HomeController> _logger;
        private readonly IProjectsRepository _projectsRepository;
        //private readonly RepositoryUser _repositoryUser;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<LoginModel> _logger;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IDataProtector _dataProtector;
        private readonly IDdlRepository _dlRepository;
        private readonly ApplicationDbContext _context;
        private readonly IUnitRepository _unitRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        private readonly IProjStakeHolderMovRepository _psmRepository;
        private readonly IAttHistoryRepository _attHistoryRepository;
        private readonly IProjStakeHolderMovRepository _stkholdmove;
        private readonly IWebHostEnvironment _environment;
        private readonly double rotationAngle;
        private System.Timers.Timer aTimer;
        private System.Threading.Timer pdfDeleteTimer;
        private string downloadFolderPath;
        public DocumentsController(IWebHostEnvironment hostingEnvironment, IDataProtectionProvider DataProtector, IProjectsRepository projectsRepository, SignInManager<ApplicationUser> signInManager, ILogger<LoginModel> logger, UserManager<ApplicationUser> userManager, IDdlRepository dlRepository, ApplicationDbContext context, IUnitRepository unitRepository, IHttpContextAccessor httpContextAccessor, IProjStakeHolderMovRepository psmRepository, IAttHistoryRepository attHistoryRepository, IProjStakeHolderMovRepository stkholdmove, IWebHostEnvironment environment)
        {
            //  _logger = logger; _repositoryUser = repositoryUser;
            _projectsRepository = projectsRepository;
            _hostingEnvironment = hostingEnvironment;
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
            _environment = environment;
            pdfDeleteTimer = new Timer(OnTimer, null, TimeSpan.Zero, TimeSpan.FromSeconds(60));
            _dataProtector = DataProtector.CreateProtector("swas.UI.Controllers.ProjectsController");
        }

        [Authorize(Policy = "Admin")]
        [HttpGet]
        ///Created by Mr Manish  
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


                    var AttHistry = _context.AttHistory.FirstOrDefault();
                    ViewBag.AttHistry = AttHistry;

                    // var proj = await _projectsRepository.GetProjforDocView();
                    ViewBag.proj = null;

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
                return RedirectToAction("Error", "Home");
            }
        }

        [Authorize(Policy = "Admin")]
        [HttpGet]
        ///Created by Mr Manish  
        public async Task<IActionResult> DocumentHistory(string projName, string projId)
        {
            int ProjIdi = 0;
            if (projId != null)
            {
                ProjIdi = int.Parse(_dataProtector.Unprotect(projId));


                Login Logins = SessionHelper.GetObjectFromJson<Login>(HttpContext.Session, "User");
                var ipAddress = HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
                var currentDatetime = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");
                var watermarkText = $" {ipAddress}  {currentDatetime}";

                var proj = _context.Projects.FirstOrDefault(p => p.ProjId == ProjIdi);

                if (proj != null)
                {
                    var Query = from project in _context.Projects
                                join psm in _context.ProjStakeHolderMov on project.ProjId equals psm.ProjId
                                join attHistory in _context.AttHistory on psm.PsmId equals attHistory.PsmId
                                where project.ProjId == ProjIdi
                                select new
                                {
                                    project.ProjId,
                                    psm.PsmId,
                                    attHistory.AttPath,
                                    attHistory.Reamarks,
                                    attHistory.ActFileName
                                };

                    var result = Query.ToList();
                    ViewBag.proj = proj;
                    ViewBag.ProjName = projName;
                    ViewBag.result = result;
                    return View(result);
                }
                else
                {
                    return NotFound();
                }
            }
            else
            {
                return Redirect("~/Identity/Account/Login");
            }
        }


        [HttpPost]
        ///Created by Mr Manish  
        public async Task<ActionResult> DownloadAction(string[] selectedCheckboxes)
        {
            try
            {
                {
                    Login Logins = SessionHelper.GetObjectFromJson<Login>(HttpContext.Session, "User");
                    var ipAddress = HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
                    var currentDatetime = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");
                    var watermarkText = $" {ipAddress}\n {currentDatetime}";

                    if (selectedCheckboxes == null || selectedCheckboxes.Length < 2)
                    {
                        return View("Error");
                    }

                    MemoryStream mergedPdfStream = new MemoryStream();
                    using (PdfWriter pdfWriter = new PdfWriter(mergedPdfStream))
                    using (PdfDocument pdf = new PdfDocument(pdfWriter))
                    using (Document document = new Document(pdf))
                    {
                        string uploadsFolder = System.IO.Path.Combine(_environment.WebRootPath, "Uploads");

                        foreach (var fileName in selectedCheckboxes)
                        {
                            string filePath = System.IO.Path.Combine(uploadsFolder, fileName);

                            if (!System.IO.File.Exists(filePath))
                            {
                                return Json(new { error = true, errorMessage = "PDF File Not Found" });
                            }

                            string watermarkedFilePath = await generate2(filePath, watermarkText);

                            using (PdfDocument sourcePdf = new PdfDocument(new PdfReader(watermarkedFilePath)))
                            {
                                int numberOfPages = sourcePdf.GetNumberOfPages();

                                for (int i = 1; i <= numberOfPages; i++)
                                {
                                    PdfPage page = sourcePdf.GetPage(i);
                                    PdfPage newPage = pdf.AddNewPage(new PageSize(page.GetPageSize()));
                                    PdfCanvas canvas = new PdfCanvas(newPage);
                                    canvas.AddXObject(page.CopyAsFormXObject(pdf));
                                }
                            }

                            System.IO.File.Delete(watermarkedFilePath);
                        }
                    }

                    var baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}";
                    var mergedPdfBytes = mergedPdfStream.ToArray();
                    Random rnd = new Random();
                    string Dfilename = rnd.Next(1, 1000).ToString();
                    downloadFolderPath = System.IO.Path.Combine(_environment.ContentRootPath, "wwwroot/Download/" + Dfilename + ".pdf");

                    System.IO.File.WriteAllBytes(downloadFolderPath, mergedPdfBytes);

                    // Perform file download using HttpClient with TLS 1.2
                    string downloadLink = await DownloadFileWithHttpClient(baseUrl, Dfilename);


                    // Provide a download link to the user



                    if (downloadLink != null)
                    {
                        // Provide a download link to the user
                        return Json(new { downloadLink = downloadLink.ToString() });
                    }
                    else
                    {
                        // Handle the case where downloadLink is null (e.g., return an error message)
                        return View("Error");
                    }
                }
            }
            catch (Exception ex)
            {
                int dynamicEventId = DateTime.UtcNow.Ticks.GetHashCode();
                var eventId = new EventId(dynamicEventId, "DownloadAction");
                _logger.Log(LogLevel.Error, eventId, "An error occurred while on Get All DownloadAction on DocumentsController.", ex, (s, e) => $"{s} - {e?.Message}");
                return RedirectToAction("Error", "Home");
            }
        }


        [Authorize(Policy = "StakeHolders")]
        public async Task<string> DownloadFileWithHttpClient(string baseUrl, string Dfilename)
        {
            try
            {
                // Use HttpClient with a custom handler to customize certificate validation
                using (HttpClient httpClient = new HttpClient(new HttpClientHandler
                {
                    ClientCertificateOptions = ClientCertificateOption.Manual,
                    ServerCertificateCustomValidationCallback =
                        (httpRequestMessage, cert, certChain, policyErrors) => true
                }))
                {
                    // Set up SSL/TLS and other settings
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                    httpClient.BaseAddress = new Uri(baseUrl);
                    httpClient.DefaultRequestHeaders.Accept.Clear();
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/pdf"));

                    // Create a URI for the download link
                    Uri downloadUri = new Uri(new Uri(baseUrl), $"/Download/{Dfilename}.pdf");

                    // Perform the file download
                    HttpResponseMessage response = await httpClient.GetAsync(downloadUri);

                    if (response.StatusCode == HttpStatusCode.NotFound)
                    {
                        // Handle the 404 error (e.g., log, return a specific value, etc.)
                        swas.BAL.Utility.Error.ExceptionHandle($"File not found: {downloadUri}");
                        return null; // Change the return type to Uri, so return null instead of an empty string
                    }

                    // Ensure a successful response
                    response.EnsureSuccessStatusCode();

                    // Save the downloaded file
                    return downloadUri.ToString();
                }
            }
            catch (HttpRequestException ex)
            {
                // Handle HTTP request exceptions
                swas.BAL.Utility.Error.ExceptionHandle($"HttpRequestException: {ex.Message}");
                return "";
            }
        }


        [Authorize(Policy = "StakeHolders")]
        public async Task<string> generate2(string originalFilePath, string watermarkText)
        {
            try
            {
                Random rnd = new Random();
                string Dfilename = rnd.Next(1, 1000).ToString();

                var watermarkedFilePath = System.IO.Path.Combine(_environment.ContentRootPath, "wwwroot/Download/" + Dfilename + ".pdf");
                PdfDocument pdfDoc = new PdfDocument(new PdfReader(originalFilePath), new PdfWriter(watermarkedFilePath));
                Document doc = new Document(pdfDoc);
                PdfFont font = PdfFontFactory.CreateFont(FontProgramFactory.CreateFont(StandardFonts.HELVETICA_BOLD));
                Paragraph paragraph = new Paragraph(watermarkText).SetFont(font).SetFontSize(50);

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
                return watermarkedFilePath;
            }
            catch (Exception ex)
            {
                swas.BAL.Utility.Error.ExceptionHandle(ex.Message);

                return "";
            }

        }


        public void OnTimer(object state)
        {
            try
            {
                if (!string.IsNullOrEmpty(downloadFolderPath) && System.IO.File.Exists(downloadFolderPath))
                {
                    // If file found, delete it
                    System.IO.File.Delete(downloadFolderPath);
                    downloadFolderPath = null; // Clear the path after deletion
                }
            }
            catch (Exception ex)
            {
                // Log or handle the exception as needed
                // Comman.ExceptionHandle(ex.Message);
            }
        }
    }
}






