using Grpc.Core;
using Microsoft.AspNetCore.Http;
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

    public class StatusRepository :GenericRepositoryDL<tbl_mStatus>, IStatusRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public StatusRepository(ApplicationDbContext context) : base(context)
        {
            _dbContext = context;
        }


        public async Task<List<DTODDLComman>> GetAllByStages_takeHolder(int ParentId, int UnitId, bool IsOwnProj)
        {
            
            List<DTODDLComman> lst = new List<DTODDLComman>();
            if (ParentId == 1)
            {
                var ret = (from Status in _dbContext.mStatus
                           join Stages in _dbContext.mStages on Status.StageId equals Stages.StagesId

                           where Status.StageId == ParentId && Status.StatusId == 1 && Status.IsActive == true
                           select new DTODDLComman
                           {
                               Name = Status.Status,
                               Id = Status.StatusId,
                           }
             ).ToListAsync();
                lst = (ret.Result);
            }
            else if (ParentId==2)
            {
                var ret = (from Status in _dbContext.mStatus
                           join Stages in _dbContext.mStages on Status.StageId equals Stages.StagesId

                           // Remove 1039 for Adn Host
                           where Status.StageId == ParentId && Status.StatusId == 20 || Status.StatusId == 21 ||   Status.StatusId == 38 || Status.StatusId == 1039
                           && Status.IsActive==true
                           select new DTODDLComman
                           {
                               Name = Status.Status,
                               Id = Status.StatusId,
                           }
             ).ToListAsync();
                lst = (ret.Result);
            }
            else 
            {
                var ret = (from Status in _dbContext.mStatus
                           join Stages in _dbContext.mStages on Status.StageId equals Stages.StagesId

                           where Status.StageId == ParentId && Status.IsActive == true
                           select new DTODDLComman
                           {
                               Name = Status.Status,
                               Id = Status.StatusId,
                           }
             ).ToListAsync();
                lst = (ret.Result);
            }
         
            
            

            return lst;
        }


        

        public async Task<List<DTODDLComman>> GetAllbyParentId(int ParentId)
        {

            var ret1 = await (from Status in _dbContext.mStatus
                              where Status.StageId == ParentId
                              select new DTODDLComman
                              {
                                  Name = Status.Status,
                                  Id = Status.StatusId,
                              }).ToListAsync();

            return ret1;
        }



        public async Task<List<tbl_mStatus>> GetAllStatusAsync()
        {
            return await _dbContext.mStatus.ToListAsync();
        }


       
        //public async Task<int> AddStatusAsync(tbl_mStatus status)
        //{
        //    _dbContext.mStatus.Add(status);
        //    await _dbContext.SaveChangesAsync();
        //    return status.StatusId;
        //}
       
        //public async Task<tbl_mStatus> GetStatusByIdAsync(int statusId)
        //{
        //    return await _dbContext.mStatus.FindAsync(statusId);
        //}
        


        //public async Task<bool> UpdateStatusAsync(tbl_mStatus status)
        //{
        //    _dbContext.Entry(status).State = EntityState.Modified;
        //    await _dbContext.SaveChangesAsync();
        //    return true;
        //}
       
        //public async Task<bool> DeleteStatusAsync(int statusId)
        //{
        //    var status = await _dbContext.mStatus.FindAsync(statusId);
        //    if (status == null)
        //        return false;

        //    _dbContext.mStatus.Remove(status);
        //    await _dbContext.SaveChangesAsync();
        //    return true;
        //}



        //public async Task<List<DTODDLComman>> GetAll()
        //{

        //    var ret1 =await (from Status in _dbContext.mStatus
        //                select new DTODDLComman
        //                {
        //                    Name = Status.Status,
        //                    Id = Status.StatusId,
        //                }).ToListAsync();

        //    return ret1;
        //}

    }



}
