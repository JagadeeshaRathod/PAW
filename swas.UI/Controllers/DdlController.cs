using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using swas.BAL.DTO;
using swas.BAL.Helpers;
using swas.BAL.Interfaces;
using swas.BAL.Repository;
using swas.DAL.Models;

namespace swas.UI.Controllers
{
    public class DdlController : ControllerBase
    {
        private readonly IDdlRepository _ddlRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public DdlController(IDdlRepository ddlRepository, IHttpContextAccessor httpContextAccessor)
        {
            _ddlRepository = ddlRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        //ddlStackholder xfgfdgdfgdf
    
        public async Task<List<UnitDtl>> ddlStackHlder(int id)
        {
            Login Logins = SessionHelper.GetObjectFromJson<Login>(HttpContext.Session, "User");

            var project = await _ddlRepository.ddlStackholder(Logins.unitid??0);

            return project;
           
        }
       
        public async Task<List<tbl_mActions>> ddlActions(int id)
        {
            var project = await _ddlRepository.ddlActions();

            return project;
            
        }
       
        public async Task<List<mHostType>> ddlmHostType(int id)
        {
            var project = await _ddlRepository.ddlmHostType(0);

            return project;

        }
    
        public async Task<List<tbl_mStatus>> ddlStatus(int id)
        {
            var project = await _ddlRepository.ddlStatus();

            return project;

        }

        public async Task<List<UnitDtl>> ddlUnit()
        {
          
            var project = await _ddlRepository.ddlUnit();

            return project;

        }
        
        public async Task<List<UnitDtl>> ddlFwdUnit(int Unitid)
        {
            Login Logins = SessionHelper.GetObjectFromJson<Login>(HttpContext.Session, "User");

            var project = await _ddlRepository.ddlFwdUnit(Logins.unitid ?? 0);

            return project;

        }

        public async Task<List<UnitDtl>> ddlFwdUnits(int Unitid)
        {
            var project = await _ddlRepository.ddlUnit();

            return project;

        }
        
        public async Task<List<UnitDtl>> ddlResFwdUnit(int ProjIds)
        {
            
            Login Logins = SessionHelper.GetObjectFromJson<Login>(_httpContextAccessor.HttpContext.Session, "User");

            var project = await _ddlRepository.ddlResFwdUnit(Logins.unitid??0, ProjIds);

            return project;

        }


        public async Task<List<mCorps>> ddlCorps(int Command)
        {
            var project = await _ddlRepository.ddlCorps(Command);

            return project;

        }


        public Task<List<tbl_mStatus>> GetActionsByStatus(int selectedStatusId)
        {
            var actionsList = _ddlRepository.GetActionsByStatus(selectedStatusId);

            return actionsList;
        }
 
        public Task<List<tbl_mStatus>> GetActiByStageStat(int selectedStatusId, int selectedStageId ,int projIds)
        {
            var actionsList = _ddlRepository.GetActiByStageStat(selectedStatusId, selectedStageId , projIds);

            return actionsList;
        }

        public Task<List<tbl_mStatus>> GetStatusByStage(int stageIds)
        {
            var statusList = _ddlRepository.GetStatusByStage(stageIds);
            return statusList;
        }

        public Task<List<tbl_mStages>> GetAllStages(int projIds)
        {
            var ss =  _ddlRepository.ddlStages(projIds);

            
            return ss;
            //return RedirectToAction(nameof(Index));
        }
        public Task<List<mAppType>> DdlAppType()
        {
            var ss = _ddlRepository.DdlAppType();
            return ss;
            //return RedirectToAction(nameof(Index));
        }

    }
}
