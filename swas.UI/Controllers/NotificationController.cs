using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using swas.BAL.DTO;
using swas.BAL.Helpers;
using swas.BAL.Interfaces;
using swas.BAL.Repository;
using swas.DAL.Models;
using System.Security.Claims;

namespace swas.UI.Controllers
{
    public class NotificationController : Controller
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly INotificationRepository _notificationRepository;
        private readonly IProjectsRepository _projectsRepository;
        private readonly ITrnChatMsgRepository _trnChatMsg;
        private readonly ILogger<NotificationController> _logger;

        public NotificationController(IHttpContextAccessor httpContextAccessor, INotificationRepository notificationRepository, IProjectsRepository projectsRepository,
              ITrnChatMsgRepository trnChatMsg, ILogger<NotificationController> logger)
        {
            _httpContextAccessor = httpContextAccessor;
            _notificationRepository = notificationRepository;
            _projectsRepository = projectsRepository;
            _trnChatMsg = trnChatMsg;
            _logger = logger;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<JsonResult> GetNotificationCount(int type)
        {
            int count = 0;
            try
            {
                //if (type == 3)
                //{
                //    string id = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
                //    var data = await _trnChatMsg.GetIsChat(id);
                //    if (data != null)
                //    {
                //        count = data.Count;
                //    }
                //    return new JsonResult(count);
                //}
                var loginUser = SessionHelper.GetObjectFromJson<Login>(_httpContextAccessor.HttpContext.Session, "User");
                count = await _notificationRepository.GetNotificationCountByType(type, loginUser.unitid);
                return new JsonResult(count);
            }
            catch (Exception ex)
            {
                return new JsonResult(new { message = ex.Message })
                {
                    StatusCode = 500
                };
            }
        }

        public async Task<IActionResult> AddNotification(int type, int ProjId, int unitid)
        {

            try
            {
                Login Logins = SessionHelper.GetObjectFromJson<Login>(_httpContextAccessor.HttpContext.Session, "User");
                if (Logins != null)
                {

                    if (ProjId != null)
                    {

                        if (type == 1)
                        {
                            var project = await _projectsRepository.GetProjectByIdAsync(ProjId);

                            unitid = project.StakeHolderId;
                            if (unitid == 1 || unitid == 3 || unitid == 4 || unitid == 5)
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
                                    notify.ReadDateTime = DateTime.Now;
                                    notify.NotificationType = 1;

                                    await _notificationRepository.AddNotification(notify);

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
                                    notify.ReadDateTime = DateTime.Now;
                                    notify.NotificationType = 1;

                                    await _notificationRepository.AddNotification(notify);

                                }
                            }

                            return Json(1);
                        }
                        else
                        {
                            Notification notify = new Notification();
                            notify.ProjId = ProjId;
                            notify.NotificationFrom = Logins.unitid ?? 0;
                            notify.NotificationTo = unitid;
                            notify.IsRead = false;
                            notify.ReadDateTime = DateTime.Now;
                            notify.NotificationType = 2;
                            await _notificationRepository.AddNotification(notify);
                            return Json(1);

                        }
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
                swas.BAL.Utility.Error.ExceptionHandle("Add Notification:-" + ex.Message);
                int dynamicEventId = DateTime.UtcNow.Ticks.GetHashCode();
                var eventId = new EventId(dynamicEventId, "AddNotification");
                _logger.Log(LogLevel.Error, eventId, "An error occurred while on Get All AddNotification in NotificationController.", ex, (s, e) => $"{s} - {e?.Message}");
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpPost]
        public async Task<IActionResult> IsReadNotification(int ProjId, int type)
        {
            var loginUser = SessionHelper.GetObjectFromJson<Login>(_httpContextAccessor.HttpContext.Session, "User");
            if (loginUser != null)
            {
                try
                {
                    if (ProjId != null)
                    {

                        if (type == 1)
                        {
                            var notify = await _notificationRepository.GetNotifByToUnitAndType(type, loginUser.unitid, ProjId);

                            if (notify != null)
                            {
                                notify.ReadDateTime = DateTime.Now;
                                notify.IsRead = true;

                                var updateResult = await _notificationRepository.UpdateNotification(notify);
                                if (updateResult)
                                {
                                    return Json(ProjId);
                                }
                            }
                        }

                        else if (type == 2)
                        {
                            var notify = await _notificationRepository.GetNotifByToUnitAndType(type, loginUser.unitid, ProjId);

                            if (notify != null)
                            {
                                notify.ReadDateTime = DateTime.Now;
                                notify.IsRead = true;

                                var updateResult = await _notificationRepository.UpdateNotification(notify);
                                if (updateResult)
                                {
                                    return Json(ProjId);
                                }
                            }
                        }
                    }

                    return Json(0);
                }
                catch (Exception ex)
                {
                    swas.BAL.Utility.Error.ExceptionHandle(ex.Message);
                    int dynamicEventId = DateTime.UtcNow.Ticks.GetHashCode();
                    var eventId = new EventId(dynamicEventId, "IsReadNotification");
                    _logger.Log(LogLevel.Error, eventId, "An error occurred while on IsReadNotification in NotificationController.", ex, (s, e) => $"{s} - {e?.Message}");
                    return RedirectToAction("Error", "Home");
                }
            }
            else
            {
                return Redirect("/Identity/Account/login");
            }
        }

        [HttpPost]
        public async Task<IActionResult> UnReadNotification(int type, int ProjId)
        {
            var loginUser = SessionHelper.GetObjectFromJson<Login>(_httpContextAccessor.HttpContext.Session, "User");
            if (loginUser != null)
            {
                try
                {
                    if (ProjId != null)
                    {

                        if (type == 1)
                        {
                            var notifications = await _notificationRepository.GetNotifExcludingToUnit(loginUser.unitid, ProjId);
                            if (notifications != null && notifications.Any())
                            {
                                foreach (var notify in notifications)
                                {
                                    notify.ReadDateTime = DateTime.Now;
                                    notify.IsRead = false;
                                    var updateResult = await _notificationRepository.UpdateNotification(notify);
                                    if (!updateResult)
                                    {
                                        return Json(new { success = false, message = "Failed to update notification." });
                                    }
                                }
                                return Json(new { success = true, projId = ProjId });
                            }
                            else
                            {
                                return Json(new { success = false, message = "No notifications found." });
                            }
                        }
                        if (type == 2)
                        {
                            var notifications = await _notificationRepository.GetNotifExcludingToUnit(loginUser.unitid, ProjId);
                            if (notifications != null && notifications.Any())
                            {
                                foreach (var notify in notifications)
                                {
                                    if (notify.NotificationType == 2) continue;
                                    notify.ReadDateTime = DateTime.Now;
                                    notify.IsRead = false;
                                    var updateResult = await _notificationRepository.UpdateNotification(notify);
                                    if (!updateResult)
                                    {
                                        return Json(new { success = false, message = "Failed to update notification." });
                                    }
                                }
                                return Json(new { success = true, projId = ProjId });
                            }
                            else
                            {
                                return Json(new { success = false, message = "No notifications found." });
                            }
                        }

                    }

                    return Json(0);
                }
                catch (Exception ex)
                {
                    swas.BAL.Utility.Error.ExceptionHandle(ex.Message);
                    int dynamicEventId = DateTime.UtcNow.Ticks.GetHashCode();
                    var eventId = new EventId(dynamicEventId, "UnReadNotification");
                    _logger.Log(LogLevel.Error, eventId, "An error occurred while on UnRead Notification in NotificationController.", ex, (s, e) => $"{s} - {e?.Message}");
                    return RedirectToAction("Error", "Home");
                }
            }
            else
            {
                return Redirect("/Identity/Account/login");
            }
        }

        [HttpPost]
        public async Task<IActionResult> UndoNotification(int ProjId, int type, int ToUnitId)
        {
            var loginUser = SessionHelper.GetObjectFromJson<Login>(_httpContextAccessor.HttpContext.Session, "User");
            if (loginUser != null)
            {
                try
                {
                    if (ProjId != null)
                    {
                        if (type == 2)
                        {
                            var notify = await _notificationRepository.GetNotifByToAndFormId(type, ToUnitId, ProjId, loginUser.unitid);

                            if (notify != null)
                            {
                                notify.ReadDateTime = DateTime.Now;
                                //notify.IsDeleted = true;
                                notify.IsRead = true;  // Added 12thNov

                                var updateResult = await _notificationRepository.UpdateNotification(notify);
                                if (updateResult)
                                {
                                    return Json(ProjId);
                                }
                            }
                        }

                    }

                    return Json(0);
                }
                catch (Exception ex)
                {
                    swas.BAL.Utility.Error.ExceptionHandle(ex.Message);
                    int dynamicEventId = DateTime.UtcNow.Ticks.GetHashCode();
                    var eventId = new EventId(dynamicEventId, "UndoNotification");
                    _logger.Log(LogLevel.Error, eventId, "An error occurred while on Undo Notification in NotificationController.", ex, (s, e) => $"{s} - {e?.Message}");

                    return RedirectToAction("Error", "Home");
                }
            }
            else
            {
                return Redirect("/Identity/Account/login");
            }
        }

        [HttpGet]
        public async Task<JsonResult> GetNotificationCountForChat()
        {
            int count = 0;
            try
            {
                string id = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
                var data = await _trnChatMsg.GetIsChat(id);
                if (data != null && data.Count > 0)
                {
                    count = data.Count;
                }
                return new JsonResult(count);
            }
            catch (Exception ex)
            {
                int dynamicEventId = DateTime.UtcNow.Ticks.GetHashCode();
                var eventId = new EventId(dynamicEventId, "GetNotificationCountForChat");
                _logger.Log(LogLevel.Error, eventId, "An error occurred while on Undo GetNotificationCountForChat in NotificationController.", ex, (s, e) => $"{s} - {e?.Message}");

                return new JsonResult(new { message = ex.Message })
                {
                    StatusCode = 500
                };
            }
        }
    }
}
