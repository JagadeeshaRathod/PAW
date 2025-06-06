using Microsoft.AspNetCore.Mvc;
using swas.BAL.DTO;
using swas.BAL.Helpers;
using swas.BAL.Interfaces;
using swas.DAL.Models;

namespace swas.UI.Controllers
{
    public class MasterController : Controller
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IDdlRepository _dlRepository;
        private readonly IStkStatusRepository _stkStatusRepository;
        private readonly IStagesRepository _stagesRepository;
        private readonly IStatusRepository _statusRepository;
        private readonly IActionsRepository _actionsRepository;
        public MasterController(IHttpContextAccessor httpContextAccessor, IDdlRepository ddlRepository,
            IStkStatusRepository stkStatusRepository, IStagesRepository stagesRepository, IStatusRepository statusRepository, IActionsRepository actionsRepository)
        {
            _httpContextAccessor = httpContextAccessor;
            _dlRepository = ddlRepository;
            _stkStatusRepository = stkStatusRepository;
            _stagesRepository = stagesRepository;
            _statusRepository = statusRepository;
            _actionsRepository = actionsRepository;
        }
        public IActionResult Index()
        {
            return View();
        }
        #region Master Table For DDL
        public async Task<IActionResult> GetStagebyStakeHolderId(int id, int ParentId,int StakeHolderId)
        {
            Login Logins = SessionHelper.GetObjectFromJson<Login>(_httpContextAccessor.HttpContext.Session, "User");
            if(StakeHolderId==100)
            {
                return Json(await _statusRepository.GetAllbyParentId(ParentId));
            }
            else if(StakeHolderId==Logins.unitid)
                return Json(await _statusRepository.GetAllByStages_takeHolder(ParentId, Convert.ToInt32(Logins.unitid),true));
            else
                return Json(await _statusRepository.GetAllByStages_takeHolder(ParentId, Convert.ToInt32(Logins.unitid),false));
            
        }
        public async Task<IActionResult> GetFwdTo(int id, int ParentId, int StakeHolderId, string Value, int Type)
        {
            Login Logins = SessionHelper.GetObjectFromJson<Login>(_httpContextAccessor.HttpContext.Session, "User");
           
           return Json(await _dlRepository.GetFwdTo(StakeHolderId, (int)Logins.unitid, Value, Type));
            

        }
      
        public async Task<IActionResult> GetAllMasterTableforddl(int id, int ParentId ,int unitId)
        {
            try
            {
                Login Logins = SessionHelper.GetObjectFromJson<Login>(_httpContextAccessor.HttpContext.Session, "User");
                List<DTODDLComman> lst = new List<DTODDLComman>();
                if (id == Mastertablenmumcs.Unit)
                {
                    var ret = await _dlRepository.ddlLimitUnit(Logins.unitid, 0);

                    foreach (var cmd in ret)
                    {

                        DTODDLComman db = new DTODDLComman();
                        db.Name = cmd.UnitName;
                        db.Id = cmd.unitid;
                        lst.Add(db);
                    }
                    return Json(lst);
                }
                else if (id == Mastertablenmumcs.HostType)
                {
                    var ret = await _dlRepository.ddlmHostType(0);
                    foreach (var cmd in ret)
                    {

                        DTODDLComman db = new DTODDLComman();
                        db.Name = cmd.HostingDesc;
                        db.Id = cmd.HostTypeID;
                        lst.Add(db);
                    }
                    return Json(lst);


                }
                else if (id == Mastertablenmumcs.AppType)
                {
                    var ret = await _dlRepository.DdlAppType();

                    foreach (var cmd in ret)
                    {

                        DTODDLComman db = new DTODDLComman();
                        db.Name = cmd.AppDesc;
                        db.Id = cmd.Apptype;
                        lst.Add(db);
                    }
                    return Json(lst);

                }
                else if (id == Mastertablenmumcs.stkStatus)
                {
                    var ret = await _stkStatusRepository.GetAll();

                    foreach (var cmd in ret)
                    {

                        DTODDLComman db = new DTODDLComman();
                        db.Name = cmd.Status;
                        db.Id = cmd.StkStatusId;
                        lst.Add(db);
                    }
                    return Json(lst);

                }
                else if (id == Mastertablenmumcs.mStages)
                {
                    var ret = await _stagesRepository.GetAll();

                    foreach (var cmd  in ret)
                    {

                        DTODDLComman db = new DTODDLComman();
                        db.Name = cmd.Stages;
                        db.Id = cmd.StagesId;
                        lst.Add(db);
                    }
                    return Json(lst);

                }
                else if (id == Mastertablenmumcs.mStatus)
                {
                    if(ParentId==0)
                    {
                        var ret = await _statusRepository.GetAll();

                       
                        return Json(ret);
                    }
                    else
                    {
                        var ret = await _statusRepository.GetAllByStages_takeHolder(ParentId, Convert.ToInt32(Logins.unitid), false);


                        return Json(ret);

                    }
                    

                }
                else if (id == Mastertablenmumcs.mActions)
                {
                    var ret = await _actionsRepository.GetActionByStatusIdlogin(ParentId, Convert.ToInt32(Logins.unitid));
                    return Json(ret);

                }
                else if (id == Mastertablenmumcs.ProjMovement_mActions)
                {
                    var ret = await _actionsRepository.ProjMovement_GetActionByStatusIdlogin(ParentId, Convert.ToInt32(Logins.unitid));
                    return Json(ret);

                }


                else if (id == Mastertablenmumcs.mMappingActionsException)
                {
                    var ret = await _actionsRepository.GetActionByStatusId(ParentId);
                    return Json(ret);
                }
                //else if (id == Mastertablenmumcs.mMappingActions)
                //{
                //    var ret = await _actionsRepository.GetActionsMappingIdByStatusId(ParentId);


                //    return Json(ret);

                //}
                return Json(null);
            }
            catch(Exception ex)
            {
                return Json(nmum.Exception);
            }
        }

        #endregion
    }
}
