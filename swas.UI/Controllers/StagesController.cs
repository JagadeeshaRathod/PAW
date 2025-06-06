using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using swas.BAL.DTO;
using swas.BAL.Helpers;
using swas.BAL.Interfaces;
using swas.DAL;
using swas.DAL.Models;
using System.Threading.Tasks;


namespace swas.UI.Controllers
{
    ///Created and Reviewed by : Sub Maj M Sanal
    ///Reviewed Date : 31 Jul 23
    ///Tested By :- 
    ///Tested Date : 
    ///Start
   
    public class StagesController : Controller
    {
        private readonly IStagesRepository _stagesRepository;
        private readonly ApplicationDbContext _context;
        private readonly IDataProtector _dataProtector;
        private readonly ILogger<StagesController> _logger;
        public StagesController(ApplicationDbContext context, IDataProtectionProvider DataProtector, IStagesRepository stagesRepository, ILogger<StagesController> logger)
        {
            _context = context;
            _dataProtector = DataProtector.CreateProtector("swas.UI.Controllers.UnitDtlsController");
            _stagesRepository = stagesRepository;
            _logger = logger;
        }
        ///Created and Reviewed by : Sub Maj Sanal
        //Reviewed Date : 31 Jul 23
        // GET: Stages
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> Index()
        {
            var stages = await _stagesRepository.GetAll();
            return View(stages);
        }
        ///Created and Reviewed by : Sub Maj Sanal
        //Reviewed Date : 31 Jul 23
        // GET: Stages/Details/5
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> Details(int id)
        {
            var stage = await _stagesRepository.Get(id);
            if (stage == null)
            {
                return NotFound();
            }

            return View(stage);
        }
        ///Created and Reviewed by : Sub Maj Sanal
        //Reviewed Date : 31 Jul 23
        // GET: Stages/Create
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> Create()
        {
            var stages = await _stagesRepository.GetAll();
            return View(stages);
        }
        ///Created and Reviewed by : Sub Maj Sanal
        //Reviewed Date : 31 Jul 23
        // POST: Stages/Create
      
        [HttpPost]
        public async Task<IActionResult> CreateStage(tbl_mStages stage)
        {
            Login Logins = SessionHelper.GetObjectFromJson<Login>(HttpContext.Session, "User");
            try
            {
                if (ModelState.IsValid)
                {
                    stage.EditDeleteBy = (int)Logins.unitid;
                    stage.UpdatedByUserId = (int)Logins.unitid;
                    stage.IsDeleted = false;
                    stage.IsActive = true;
                    stage.DateTimeOfUpdate = DateTime.Now;
                    stage.EditDeleteDate = DateTime.Now;
                    stage.InitiaalID = false;
                    stage.FininshID = false;

                    await _stagesRepository.Add(stage);
                    return RedirectToAction(nameof(Create));
                }
                return View(stage);
            }
            catch (Exception ex)
            {
                int dynamicEventId = DateTime.UtcNow.Ticks.GetHashCode();
                var eventId = new EventId(dynamicEventId, "CreateStage");
                _logger.Log(LogLevel.Error, eventId, "An error occurred while on CreateStage in StagesController.", ex, (s, e) => $"{s} - {e?.Message}");

                return RedirectToAction("Error", "Home");
            }
           
        }
        ///Created and Reviewed by : Sub Maj Sanal
        //Reviewed Date : 31 Jul 23
        // GET: Stages/Edit/5
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> Edit(int id)
        {
            var stage = await _stagesRepository.Get(id);
            if (stage == null)
            {
                return NotFound();
            }
            return View(stage);
        }
        ///Created and Reviewed by : Sub Maj Sanal
        //Reviewed Date : 31 Jul 23
        // POST: Stages/Edit/5
        [Authorize(Policy = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, tbl_mStages stage)
        {
            try
            {
                if (id != stage.StagesId)
                {
                    return NotFound();
                }

                if (ModelState.IsValid)
                {
                    await _stagesRepository.UpdateWithReturn(stage);
                    return RedirectToAction(nameof(Index));
                }

                return View(stage);
            }
            catch (Exception ex)
            {
                int dynamicEventId = DateTime.UtcNow.Ticks.GetHashCode();
                var eventId = new EventId(dynamicEventId, "Edit");
                _logger.Log(LogLevel.Error, eventId, "An error occurred while on Edit in StagesController.", ex, (s, e) => $"{s} - {e?.Message}");

                return RedirectToAction("Error", "Home");
            }
            
        }
        ///Created and Reviewed by : Sub Maj Sanal
        //Reviewed Date : 31 Jul 23
        // GET: Stages/Delete/5
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var stage = await _stagesRepository.Delete(id);
            if (stage == null)
            {
                return NotFound();
            }
            return View(stage);
        }
        ///Created and Reviewed by : Sub Maj Sanal
        //Reviewed Date : 31 Jul 23
        // POST: Stages/Delete/5
        [Authorize(Policy = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await _stagesRepository.Delete(id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                int dynamicEventId = DateTime.UtcNow.Ticks.GetHashCode();
                var eventId = new EventId(dynamicEventId, "DeleteConfirmed");
                _logger.Log(LogLevel.Error, eventId, "An error occurred while on DeleteConfirmed in StagesController.", ex, (s, e) => $"{s} - {e?.Message}");
                return RedirectToAction("Error", "Home");
            }
         
        }
    }

}
