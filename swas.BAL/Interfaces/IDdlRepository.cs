using swas.BAL.DTO;
using swas.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace swas.BAL.Interfaces
{

    public interface IDdlRepository
    {
        Task<List<DTODDLComman>> GetFwdTo(int UnitId, int LoginUnitId, string Value, int Type);
        Task<List<tbl_mStatus>>ddlStatus();
        Task<List<tbl_mStages>> ddlStages(int projIds);

        Task<List<UnitDtl>> ddlStackholder(int unitid);
        Task<List<tbl_mActions>> ddlActions();
        Task<List<mCommand>> ddlCommand();
        Task<List<Types>> ddlType();
        Task<List<mCorps>> ddlCorps(int commandId);
        Task<List<UnitDtl>> ddlUnit();
        Task<List<UnitDtl>> ddlFwdUnit(int Unitid);
        
        Task<List<tbl_mStatus>> GetActionsByStatus(int status);

        Task<List<tbl_mStatus>> GetStatusByStage(int stageId);

        Task<List<mAppType>> DdlAppType();

        Task<List<mHostType>> ddlmHostType(int id);

        Task<List<tbl_mStatus>> GetActiByStageStat(int status, int stageid , int projIds);

        Task<List<UnitDtl>> ddlLimitUnit(int? unitid, int? projid);

        Task<List<UnitDtl>> ddlResFwdUnit(int unitid, int ProjIds);
    }
}
