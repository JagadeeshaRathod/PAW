using Grpc.Core;
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


    public class DdlRepository : IDdlRepository
    {
        private readonly ApplicationDbContext _dbContext;


        public DdlRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;

        }
        public async Task<List<tbl_mStatus>> ddlStatus()
        {
            return await _dbContext.mStatus.Where(a => a.IsActive == true).ToListAsync();
        }

        public async Task<List<UnitDtl>> ddlStackholder(int uunitid)
        {
            return await _dbContext.tbl_mUnitBranch.Where(a => a.Status == true && a.unitid != uunitid).ToListAsync();
        }

        public async Task<List<mHostType>> ddlmHostType(int id)
        {
            return await _dbContext.mHostType.ToListAsync();
        }

        public async Task<List<tbl_mActions>> ddlActions()
        {
            return await _dbContext.mActions.Where(a => a.IsActive == true).ToListAsync();
        }

        public async Task<List<mCommand>> ddlCommand()
        {
            try
            {
                return await _dbContext.mCommand.ToListAsync();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public Task<List<Types>> ddlType()
        {
            try
            {
                return _dbContext.tbl_Type.ToListAsync();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<List<mCorps>> ddlCorps(int commandId)
        {
            var corpsOptions = await _dbContext.mCorps
                .Where(c => c.comdid == commandId)
                .Select(c => new mCorps { corpsid = c.corpsid, corpsname = c.corpsname })
                .ToListAsync();

            return corpsOptions;
        }
        public async Task<List<UnitDtl>> ddlUnit()
        {

            var unitOptions = await _dbContext.tbl_mUnitBranch

                .Select(c => new UnitDtl { unitid = c.unitid, UnitName = c.UnitName })
                .ToListAsync();

            return unitOptions;
        }


        public async Task<List<UnitDtl>> ddlLimitUnit(int? unitid, int? projid)
        {
            if (unitid != null)
            {
                var unitOptions = await _dbContext.tbl_mUnitBranch.
                     Where(d => d.TypeId == 1 || d.unitid == unitid)
                    .Select(c => new UnitDtl { unitid = c.unitid, UnitName = c.UnitName })
                   .ToListAsync();

                return unitOptions;
            }
            else if (projid != null)
            {
                int unitId = _dbContext.Projects.Where(a => a.ProjId == projid)
                    .Select(b => b.StakeHolderId)
                    .FirstOrDefault();

                var unitOptions = await _dbContext.tbl_mUnitBranch

                    .Select(c => new UnitDtl { unitid = c.unitid, UnitName = c.UnitName }).
                    Where(d => d.TypeId == 1 || d.unitid == unitId).ToListAsync();

                return unitOptions;
            }
            else
            {
                return null;
            }

        }


        public async Task<List<UnitDtl>> ddlFwdUnit(int unitid)
        {
            var unitOptions = await _dbContext.tbl_mUnitBranch.Where(c => c.Status == true && c.unitid != unitid)

                .Select(c => new UnitDtl { unitid = c.unitid, UnitName = c.UnitName })
                .ToListAsync();

            return unitOptions;
        }


        public async Task<List<UnitDtl>> ddlResFwdUnit(int unitid, int ProjIds)
        {
            int unitId = _dbContext.Projects.Where(a => a.ProjId == ProjIds)
                    .Select(b => b.StakeHolderId)
                    .FirstOrDefault();

            var unitOptions = await _dbContext.tbl_mUnitBranch.Where(c => c.Status == true && c.unitid != unitid
            && (c.TypeId == 1 || c.unitid == unitId)
            )

                .Select(c => new UnitDtl { unitid = c.unitid, UnitName = c.UnitName })
                .ToListAsync();

            return unitOptions;
        }

        public async Task<List<tbl_mStages>> ddlStages(int projIds)
        {
            bool excludeStage1 = await _dbContext.ProjStakeHolderMov
                                                   .AnyAsync(m => m.ProjId == projIds);

            var stagesQuery = _dbContext.mStages
                                        .Where(a => a.IsActive);

            if (excludeStage1)
            {
                stagesQuery = stagesQuery.Where(a => a.StagesId != 1 && a.StagesId != 3);
            }
            else
            {
                stagesQuery = stagesQuery.Where(a => a.StagesId == 1);
            }

            var stages = await stagesQuery.ToListAsync();
            return stages;
        }




        public Task<List<tbl_mStatus>> GetActionsByStatus(int status)
        {
            var actions = _dbContext.mStatus.Where(a => a.StatusId == status).ToListAsync();
            return actions;
        }

        public Task<List<tbl_mStatus>> GetActiByStageStat(int status, int stageid, int projIds)
        {
            var actions = _dbContext.mStatus.Where(a => a.StatusId == status && a.StageId == stageid).ToListAsync();


            return actions;
        }


        public async Task<List<tbl_mStatus>> GetStatusByStage(int stageId)
        {
            var stages = await _dbContext.mStatus.Where(a => a.StageId == stageId).ToListAsync();
            return stages;
        }

        public async Task<List<mAppType>> DdlAppType()
        {
            return await _dbContext.mAppType.ToListAsync();
        }

        public async Task<List<DTODDLComman>> GetFwdTo(int UnitId, int LoginUnitId, string Value, int Type)
        {
            List<DTODDLComman> list = new List<DTODDLComman>();
            if (Type == 1)
            {
                var usedUnitIds = await _dbContext.Users.Select(u => u.unitid).Distinct().ToListAsync();
                list = await _dbContext.tbl_mUnitBranch.Where(c => c.unitid != LoginUnitId && usedUnitIds.Contains(c.unitid) &&
                c.UnitName.ToLower().Contains(Value.ToLower())).Select(c => new DTODDLComman
                {
                    Id = c.unitid,
                    Name = c.UnitName
                }).ToListAsync();
            }
            else
            {
                list = await _dbContext.tbl_mUnitBranch.Where(c => c.TypeId == 1 || c.unitid == UnitId)
              .Select(c => new DTODDLComman { Id = c.unitid, Name = c.UnitName })
              .ToListAsync();

            }
            return list;
        }
    }
}
