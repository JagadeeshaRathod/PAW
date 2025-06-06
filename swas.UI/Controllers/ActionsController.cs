using Microsoft.AspNetCore.Mvc;

namespace swas.UI.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Org.BouncyCastle.Utilities;
    using swas.BAL.DTO;
    using swas.BAL.Helpers;
    using swas.BAL.Interfaces;
    using swas.BAL.Repository;
    using swas.DAL.Models;
    using System.Threading.Tasks;
  
    public class ActionsController : Controller
    {
        private readonly IActionsRepository _actionsRepository;

        private readonly IStagesRepository _stagesRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<ActionsController> _logger;
        public ActionsController(IActionsRepository actionsRepository, IHttpContextAccessor httpContextAccessor, IStagesRepository stagesRepository, ILogger<ActionsController> logger)
        {
            _actionsRepository = actionsRepository;
            _stagesRepository = stagesRepository;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }

        ///Created and Reviewed by : Sub Maj Sanal
        ///Reviewed Date : 31 Jul 23
        // GET: Actions
        [Authorize(Policy = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            
            var ipAddress = HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
            var currentDatetime = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");
            var watermarkText = $" {ipAddress}\n  {currentDatetime}";

            TempData["ipadd"] = watermarkText;
        
            var actions = await _actionsRepository.GetAll();
            return View(actions);
        }
        ///Created and Reviewed by : Sub Maj Sanal
        ///Reviewed Date : 31 Jul 23
        // GET: Actions/Details/5
        [Authorize(Policy = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var actions = await _actionsRepository.Get(id);
            if (actions == null)
            {
                return NotFound();
            }

            return View(actions);
        }

        ///Created and Reviewed by : Sub Maj Sanal
        ///Reviewed Date : 31 Jul 23
        // GET: Actions/Create

        //public async Task<IActionResult> Create()
        //{
        //    var stages = _stagesRepository.GetAllStages(); 
        //    ViewBag.StagesList = new SelectList(stages, "StagesId", "Stages");
        //    return View();
        //}

        ///Created and Reviewed by : Sub Maj Sanal
        ///Reviewed Date : 31 Jul 23
        // POST: Actions/Create

        [HttpPost]
        public async Task<IActionResult> Create(tbl_mActions model)
        {
            try
            {
                Login Logins = SessionHelper.GetObjectFromJson<Login>(_httpContextAccessor.HttpContext.Session, "User");

                if (ModelState.IsValid)
                {

                    var existingAction = await _actionsRepository.getActionByName(model.Actions);
                    if (existingAction != null)
                    {
                        ModelState.AddModelError("ActionName", "Action already exists in the table.");
                        return View(model);
                    }

                    model.ActionDesc = model.Actions;
                    model.IsDeleted = false;
                    model.IsActive = true;
                    model.UpdatedByUserId = Logins.unitid;
                    model.DateTimeOfUpdate = DateTime.Now;
                    model.EditDeleteDate = DateTime.Now;
                    model.EditDeleteBy = Logins.unitid;
                    if (model.ActionsId == 0)
                    {
                        await _actionsRepository.AddWithReturn(model);
                        return Json(nmum.Save);
                    }

                    else
                    {
                        await _actionsRepository.UpdateWithReturn(model);
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
                _logger.Log(LogLevel.Error, eventId, "An error occurred while on Create on ActionsController.", ex, (s, e) => $"{s} - {e?.Message}");
                return RedirectToAction("Error", "Home");
            }            
        }




        [Authorize(Policy = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            

            var actions = await _actionsRepository.Get(id);
            if (actions == null)
            {
                return NotFound();
            }
            return View(actions);
        }
        [Authorize(Policy = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, tbl_mActions model)
        {
            try
            {
                if (id != model.ActionsId)
                {
                    return NotFound();
                }

                if (ModelState.IsValid)
                {
                    await _actionsRepository.UpdateWithReturn(model);
                    return RedirectToAction(nameof(Index));
                }

                return View(model);
            }
            catch (Exception ex)
            {
                int dynamicEventId = DateTime.UtcNow.Ticks.GetHashCode();
                var eventId = new EventId(dynamicEventId, "Edit");
                _logger.Log(LogLevel.Error, eventId, "An error occurred while on Edit on ActionsController.", ex, (s, e) => $"{s} - {e?.Message}");

                return RedirectToAction("Error", "Home");
            }
            
        }

        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var actions = await _actionsRepository.Delete(id);
            if (actions == null)
            {
                return NotFound();
            }


            TempData["DeleteMessage"] = $"Are you sure you want to delete action : {actions.Actions}?";
            TempData["DeleteId"] = id;
            return RedirectToAction(nameof(Index));
        }

        #region Delete ACtion
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int ActionsId)
        {
            try
            {
                await _actionsRepository.Delete(ActionsId);
                return Json(1);
            }
            catch (Exception ex)
            {
                int dynamicEventId = DateTime.UtcNow.Ticks.GetHashCode();
                var eventId = new EventId(dynamicEventId, "DeleteConfirmed");
                _logger.Log(LogLevel.Error, eventId, "An error occurred while on DeleteConfirmed on ActionsController.", ex, (s, e) => $"{s} - {e?.Message}");

                return RedirectToAction("Error", "Home");
            }            
        }

        #endregion

        #region Action Seq

        public async Task<IActionResult> ActionSeqView()
        { 
            var ipAddress = HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
            var currentDatetime = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");
            var watermarkText = $" {ipAddress}\n  {currentDatetime}";
            TempData["ipadd"] = watermarkText;

            List<ActionsSeq> resultList = await _actionsRepository.GetActionresp();

            string animatedView = "<div class=\"container\" id=\"container\">";

            for (int i = 0; i < resultList.Count; i++)
            {
                ActionsSeq item = resultList[i];

                animatedView += $"<div class=\"box\" id=\"box{i + 1}\" style=\"text-align: center;\">{item.ActionDesc}<br/>{item.UnitName}</div>";

                if (i < resultList.Count - 1)
                {
                    animatedView += "<div class=\"link-line\"></div>";
                }
            }

            animatedView += "</div>";

            ViewBag.AnimatedView = resultList.ToList();

            return View();
        }



        #endregion
    }

}
