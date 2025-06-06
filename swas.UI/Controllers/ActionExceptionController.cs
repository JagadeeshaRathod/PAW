using Microsoft.AspNetCore.Mvc;
using swas.BAL.DTO;
using swas.BAL.Helpers;
using swas.BAL.Interfaces;
using swas.BAL.Repository;
using swas.DAL.Models;
using swas.UI.Helpers;

namespace swas.UI.Controllers
{
    public class ActionExceptionController : Controller
    {
        private readonly IActionExceptionRepository _actionsException ;
        public ActionExceptionController(IActionExceptionRepository ActionExceptionRepository)
        {
            _actionsException = ActionExceptionRepository;
        }
        public  async Task<IActionResult> Index()
        {
            var actions = await _actionsException.GetUnitStatusMapping();
            return View(actions);
            
        }

     
        public async Task<IActionResult> CheckProjectExists (int UnitId, int ActionId)
        {
            var Ret = await _actionsException.CheckProjectExists(UnitId, ActionId);
            return Json(Ret);
        }

        public async Task<IActionResult> SaveActionException(TrnUnitStatusMapping unitMapping )
        {
            unitMapping.UnitStatusMappingId = unitMapping.UnitStatusMappingId;
            unitMapping.UnitId = unitMapping.UnitId;
            unitMapping.StatusActionsMappingId = unitMapping.StatusActionsMappingId;

            var Ret = await _actionsException.AddWithReturn(unitMapping);
            if (Ret != null)
            {
                return Json(Ret);
            }
            else
            {
                return Json(nmum.NotSave);
            }

        }

        public async Task<IActionResult> UpdateActionException(TrnUnitStatusMapping unitMapping)
        {
            unitMapping.UnitStatusMappingId = unitMapping.UnitStatusMappingId;
            unitMapping.UnitId = unitMapping.UnitId;
            unitMapping.StatusActionsMappingId = unitMapping.StatusActionsMappingId;

            var Ret = await _actionsException.UpdateWithReturn(unitMapping);
            if (Ret != null)
            {
                return Json(Ret);
            }
            else
            {
                return Json(nmum.NotSave);
            }

        }

    }
}
