using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using swas.BAL.DTO;
using swas.BAL.Helpers;
using swas.BAL.Interfaces;
using swas.BAL.Repository;
using swas.DAL;
using swas.DAL.Models;
using System.Threading.Tasks;

namespace swas.UI.Controllers
{

    public class StatusController : Controller
    {
        private readonly IStatusRepository _statusRepository;
        private readonly ApplicationDbContext _context;
        private readonly IDataProtector _dataProtector;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<StatusController> _logger;
        public StatusController( ApplicationDbContext context, IDataProtectionProvider DataProtector , IStatusRepository statusRepository, IHttpContextAccessor httpContextAccessor, ILogger<StatusController> logger)
        {
            
            _context = context;
            _dataProtector = DataProtector.CreateProtector("swas.UI.Controllers.UnitDtlsController");
            _statusRepository = statusRepository;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;

        }

        [Authorize(Policy = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Index()
        {

            var ipAddress = HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
            var currentDatetime = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");
            var watermarkText = $" {ipAddress}\n  {currentDatetime}";

            TempData["ipadd"] = watermarkText;

            var status = await _statusRepository.GetAll();
            return View(status);
        }


        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> Details(int id)
        {
            var status = await _statusRepository.Get(id);
            if (status == null)
            {
                return NotFound();
            }

            return View(status);
        }

     

        //[Authorize(Policy = "Admin")]
        //public async Task<IActionResult> Create()
        //{
        //    var Status = await _statusRepository.GetAll();
        //    return View(Status);
            
        //}


        
        //[HttpPost]
        
        //public async Task<IActionResult> CreateStatus (tbl_mStatus status)
        //{
        //    Login Logins = SessionHelper.GetObjectFromJson<Login>(HttpContext.Session, "User");
        //    if (ModelState.IsValid)
        //    {
        //        status.EditDeleteBy = (int)Logins.unitid;
        //        status.UpdatedByUserId = (int)Logins.unitid;
        //        status.IsDeleted = false;
        //        status.IsActive = true;
        //        status.DateTimeOfUpdate = DateTime.Now;
        //        status.EditDeleteDate = DateTime.Now;
        //        status.InitiaalID = false;
        //        status.FininshID = false;

        //        await _statusRepository.Add(status);
        //        return RedirectToAction(nameof(Create));
        //    }
        //    //return View(status);
        //    return RedirectToAction(nameof(Create));
        //}

        [HttpPost]
        public async Task<IActionResult> Create(tbl_mStatus model)
        {
            Login Logins = SessionHelper.GetObjectFromJson<Login>(_httpContextAccessor.HttpContext.Session, "User");
            try
            {
                if (ModelState.IsValid)
                {
                    model.Status = model.Status;
                    model.IsDeleted = false;
                    model.IsActive = true;
                    model.UpdatedByUserId = (int)Logins.unitid;
                    model.DateTimeOfUpdate = DateTime.Now;
                    model.EditDeleteDate = DateTime.Now;
                    model.EditDeleteBy = (int)Logins.unitid;

                    model.InitiaalID = false;
                    model.FininshID = false;

                    if (model.StatusId == 0)
                    {
                        await _statusRepository.AddWithReturn(model);
                        //await _actionsRepository.AddWithReturn(model);
                        return Json(nmum.Save);
                    }

                    else
                    {
                        await _statusRepository.UpdateWithReturn(model);
                        return Json(nmum.Update);
                    }
                }
                else
                {
                    return Json(nmum.NotSave);
                }
            }
            catch (Exception ex)
            {
                int dynamicEventId = DateTime.UtcNow.Ticks.GetHashCode();
                var eventId = new EventId(dynamicEventId, "Create");
                _logger.Log(LogLevel.Error, eventId, "An error occurred while on Create in StatusController.", ex, (s, e) => $"{s} - {e?.Message}");

                return RedirectToAction("Error", "Home");
            }
        }


        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> Edit(int id)
        {
            var status = await _statusRepository.Get(id);
            if (status == null)
            {
                return NotFound();
            }
            return View(status);
        }


        [Authorize(Policy = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
     
        public async Task<IActionResult> Edit(int id, tbl_mStatus status)
        {
            try
            {
                if (id != status.StatusId)
                {
                    return NotFound();
                }

                if (ModelState.IsValid)
                {
                    await _statusRepository.UpdateWithReturn(status);
                    return RedirectToAction(nameof(Index));
                }

                //return View(status);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                int dynamicEventId = DateTime.UtcNow.Ticks.GetHashCode();
                var eventId = new EventId(dynamicEventId, "Edit");
                _logger.Log(LogLevel.Error, eventId, "An error occurred while on Edit in StatusController.", ex, (s, e) => $"{s} - {e?.Message}");

                return RedirectToAction("Error", "Home");
            }
            
        }


        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var status = await _statusRepository.Delete(id);
            if (status == null)
            {
                return NotFound();
            }
            return View(status);
        }

        

        [Authorize(Policy = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await _statusRepository.Delete(id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                int dynamicEventId = DateTime.UtcNow.Ticks.GetHashCode();
                var eventId = new EventId(dynamicEventId, "DeleteConfirmed");
                _logger.Log(LogLevel.Error, eventId, "An error occurred while on DeleteConfirmed in StatusController.", ex, (s, e) => $"{s} - {e?.Message}");

                return RedirectToAction("Error", "Home");
            }
           
        }

        
    }

}
