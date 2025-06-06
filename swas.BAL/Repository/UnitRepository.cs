using Microsoft.IdentityModel.Tokens;
using swas.DAL.Models;
using swas.BAL.Utility;
using swas.BAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using swas.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using swas.BAL.DTO;
using System;
using swas.BAL.Repository;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace swas.BAL
{
    ///Created and Reviewed by : Sub Maj Sanal
    ///Reviewed Date : 10 Aug 23
    ///Tested By :-  
    ///Tested Date : 
    ///Start
    public class UnitRepository : IUnitRepository
    {
        public readonly ApplicationDbContext _context;
        public readonly ApplicationDbContext _Dcontext;

        public UnitRepository(ApplicationDbContext context, ApplicationDbContext Dcontext)
        {
            _context = context;
            _Dcontext = Dcontext;
            
        }
        public async Task<int> Save(UnitDtl Db)
        {


            if (!CheckUserExist(Db.UnitName)) /* Db.UnitSusNo*/
            {
                Db.Status = true;
                Db.CorpsId = 0;

                _context.Add(Db);
                _context.SaveChanges();


                return Convert.ToInt32(EnumHelper.SaveData.Save);
            }

            else
            {
                //_context.Update(Db);
                //await _context.SaveChangesAsync();
                return Convert.ToInt32(EnumHelper.SaveData.Duplicate);
            }

        }
        public bool CheckUserExist(string unitname)   /*, string SusNo*/
        {

            return _context.tbl_mUnitBranch.Any(e => e.UnitName == unitname ); /*|| e.UnitSusNo == SusNo*/

        }

        public async Task<UnitDtl> GetUnitDtl(int unitid)
        {
            UnitDtl logdet = new UnitDtl();
            try
            {
                
                logdet = await _context.tbl_mUnitBranch.FirstOrDefaultAsync(a => a.unitid == unitid);
            }
            catch (Exception ex)
            {
                string ss = ex.Message;
            }
            return logdet;

        }

        public async Task<int> GetIdCalendar()
        {
            int ret = 0;
            try
            {

                ret = await _context.mCalendar.Select(i=>i.Type).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                return 0;
            }
            return ret;

        }



        public async Task<UnitDtl> GetUnitDtlwithname(string unitname)
        {
            UnitDtl logdet = new UnitDtl();
            try
            {

                logdet = await _context.tbl_mUnitBranch.FirstOrDefaultAsync(a => a.UnitName == unitname);
            }
            catch (Exception ex)
            {
                string ss = ex.Message;
            }
            return logdet;

        }



        public async Task<UnitDtl> GetUnitDtls(int unitid)
        {
          
            
            UnitDtl logdet =  _Dcontext.tbl_mUnitBranch.FirstOrDefault(a => a.unitid == unitid);
            
            return logdet;

        }





        public async Task<int> del(UnitDtl Db)
        {
            var query =
            from ord in _context.tbl_mUnitBranch
            where ord.unitid == Db.unitid
            select ord;

                _context.SaveChanges();
                return Convert.ToInt32(EnumHelper.SaveData.Delete); 
            
        }


        public async Task<List<UnitDtl>> GetAllUnitAsync()
        {


            var Unit = await _context.tbl_mUnitBranch.ToListAsync();
            var units = (from d in _context.tbl_mUnitBranch
                         join c in _context.mCommand on d.Command equals c.comdid into comd
                         from c in comd.DefaultIfEmpty()
                         join s in _context.mCorps on d.CorpsId equals s.corpsid into coprs
                         from s in coprs.DefaultIfEmpty()
                         join t in _context.tbl_Type on d.TypeId equals t.Id
                         
                         select new UnitDtl
                         {
                             EnctyptID = d.unitid.ToString(),
                             unitid = d.unitid,
                             UnitName = d.UnitName,
                             ComdAbbreviation = c.Command_Name,
                             Command = d.Command,
                             CorpsId = d.CorpsId,
                             Types = t.Name,

                             UnitSusNo = d.UnitSusNo,
                             //Loc = d.Loc,
                             Corps = s.corpsname,
                             Status = d.Status,
                             UpdatedBy = d.UpdatedBy,
                             UpdatedDate = d.UpdatedDate

                         });

            var unitsresult = await units.ToListAsync();
            return unitsresult;
        }

        public async Task<List<UnitDtl>> GetAllStakeHolderComment()
        {
            return await _context.tbl_mUnitBranch.Where(i=>i.commentreqdid==true).ToListAsync();
        }

        public async Task<List<DTOUnitMapping>> GetallUnitwithmap()
        {
            throw new NotImplementedException();
        }

       
         
        public async Task<List<DTOUnitMapping>> GetallUnitwithmap1(int StageId , int StatusId)
        {
            //var ret1 = await(from usm in _context.TrnUnitStatusMapping
            //                 join ms in _context.mStatus on usm.StatusId equals ms.StatusId
            //                 join sam in _context.TrnStatusActionsMapping on ms.StatusId equals sam.StatusId
            //                 join s in _context.mStages on ms.StageId equals s.StagesId
            //                 join a in _context.mActions on sam.ActionsId equals a.ActionsId
            //                 join ub in _context.tbl_mUnitBranch on usm.UnitId equals ub.unitid
            //                 where usm.UnitId==unitId &&
            //                  !(from ub in _context.tbl_mUnitBranch
            //                    where ub.unitid == usm.UnitId && ub.TypeId == 6
            //                    select 1).Any()
            //                 orderby ub.unitid descending
            //                 select new DTOUnitMapping
            //                 {
            //                     StatusActionsMappingId = sam.StatusActionsMappingId,
            //                     UnitStatusMappingId = usm.UnitStatusMappingId,  // Keep this property
            //                     UnitName = ub.UnitName,  // Changed property name to UnitName
            //                     UnitId = usm.UnitId,
            //                     StagesName = s.Stages,
            //                     StagesId = s.StagesId,  // Changed property name to StagesId
            //                     SubStagesName = ms.Status,
            //                     SubStagesId = ms.StatusId,  // Changed property name to SubStagesId
            //                     ActionsName = a.Actions,
            //                     ActionsId = a.ActionsId
            //                 }).ToListAsync();

            var result = from sam in _context.TrnStatusActionsMapping
                         join ms in _context.mStatus on sam.StatusId equals ms.StatusId
                         join s in _context.mStages on ms.StageId equals s.StagesId
                         join a in _context.mActions on sam.ActionsId equals a.ActionsId
                         where s.StagesId == StageId && ms.StatusId == StatusId
                         select new DTOUnitMapping
                         {
                             StatusActionsMappingId = sam.StatusActionsMappingId, 
                             StagesName = s.Stages,
                             StagesId = s.StagesId,
                             SubStagesName = ms.Status,
                             SubStagesId = ms.StatusId,
                             ActionsName = a.Actions,
                             ActionsId = a.ActionsId
                         };

            var resultList = await result.ToListAsync();



            return resultList;
        }

       






        //public async Task<List<UnitDtl>> GetFindUnitAsync(string UserName)
        //{


        //    var query = from user in _context.Users where user.UserName == UserName
        //                join unit in _context.tbl_mUnitBranch
        //                on user.UnitId equals unit.unitid
        //                select new
        //                {
        //                    UserName = user.UserName,
        //                    UnitName = unit.UnitName
        //                };

        //    var result = query.ToList();

        //    return result;

        //}

    }
}
