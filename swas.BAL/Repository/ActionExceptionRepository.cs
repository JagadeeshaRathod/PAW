using Microsoft.EntityFrameworkCore;
using swas.BAL.DTO;
using swas.BAL.Interfaces;
using swas.DAL;
using swas.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace swas.BAL.Repository
{

    public class ActionExceptionRepository : GenericRepositoryDL<TrnUnitStatusMapping>, IActionExceptionRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public ActionExceptionRepository(ApplicationDbContext context) : base(context)
        {
            _dbContext = context;
        }


        public async Task<List<ActionExceptionDTO>> GetUnitStatusMapping()
        {
           
            var result = from a in _dbContext.TrnUnitStatusMapping
                         join b in _dbContext.TrnStatusActionsMapping on a.StatusActionsMappingId equals b.StatusActionsMappingId
                         join c in _dbContext.tbl_mUnitBranch on a.UnitId equals c.unitid
                         join d in _dbContext.mStatus on b.StatusId equals d.StatusId
                         join e in _dbContext.mActions on b.ActionsId equals e.ActionsId
                         join f in _dbContext.mStages on d.StageId equals f.StagesId
                         where d.IsActive == true
                         orderby a.UnitStatusMappingId ascending
                         select new ActionExceptionDTO
                         {
                             UnitStatusMappingId = a.UnitStatusMappingId,
                             UnitName = c.UnitName,
                             SubStage = d.Status,
                             Action = e.ActionDesc,
                             Stage = f.Stages,
                             StageId = f.StagesId,
                             SubStageId = d.StatusId,
                             ActionId = a.StatusActionsMappingId,
                             UnitId = a.UnitId

                         };


            var actionsList = await result.ToListAsync();

            return actionsList;
        }


        public async Task<bool> CheckProjectExists(int UnitId, int ActionId)
        {
            var result = await (from a in _dbContext.TrnUnitStatusMapping
                                join b in _dbContext.TrnStatusActionsMapping
                                on a.StatusActionsMappingId equals b.StatusActionsMappingId
                                where a.UnitId == UnitId && a.StatusActionsMappingId == ActionId
                                select new TrnUnitStatusMapping
                                {
                                    UnitId = a.UnitId,
                                    StatusActionsMappingId = a.StatusActionsMappingId
                                }).FirstOrDefaultAsync();
            if (result != null)
            {
                return true;
            }
            return false;



        }

    }
}
