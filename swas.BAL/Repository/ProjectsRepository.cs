using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using swas.BAL.DTO;
using swas.BAL.Helpers;
using swas.BAL.Interfaces;
using swas.DAL;
using swas.DAL.Models;
using System.Net.Mail;
using System.Data;
using System.Linq;
using Microsoft.AspNetCore.DataProtection;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using Microsoft.AspNetCore.Identity;
using System.Xml.Linq;
using System;
using Microsoft.AspNetCore.Mvc;
using swas.BAL.Utility;
using static Grpc.Core.Metadata;
using Grpc.Core;
using System.Diagnostics;
using System.Threading;
using swas.UI.Helpers;
using Microsoft.EntityFrameworkCore.Query.Internal;
using ASPNetCoreIdentityCustomFields.Data;
using Microsoft.Data.SqlClient;
using swas.DAL.Mapper;
using Microsoft.Extensions.Configuration;

namespace swas.BAL.Repository
{

    ///Created and Reviewed by : Sub Maj M Sanal
    ///Reviewed Date : 31 Jul 23
    ///Tested By :- 
    ///Tested Date : 
    ///Start
    public class ProjectsRepository : IProjectsRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ApplicationDbContext _DBContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IDataProtector _dataProtector;
        private readonly IProjStakeHolderMovRepository _psmRepository;
        private readonly IConfiguration _configuration;
        public ProjectsRepository(ApplicationDbContext dbContext, IHttpContextAccessor httpContextAccessor,
            ApplicationDbContext DBContext, IDataProtectionProvider dataProtector, IProjStakeHolderMovRepository psmRepository, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _httpContextAccessor = httpContextAccessor;
            _DBContext = DBContext;
            _dataProtector = dataProtector.CreateProtector("swas.UI.Controllers.ProjectsController");
            _psmRepository = psmRepository;
            _configuration = configuration;
        }
        public async Task<DTOProjectWiseStatus> GetProjectWiseStatus()
        {
            #region GetProjectWiseStatusWithLinq

            //DTOProjectWiseStatus lst = new DTOProjectWiseStatus();
            //var status = await (from stg in _dbContext.mStages
            //                    join sts in _dbContext.mStatus on stg.StagesId equals sts.StageId
            //                    where sts.IsDashboard == true
            //                    && sts.StatusId != 2 && sts.StatusId != 3 && sts.StatusId != 22 && sts.StatusId != 31
            //                    && sts.StatusId != 37
            //                    orderby sts.StageId, sts.Statseq
            //                    select new StatusProject
            //                    {
            //                        StatusId = sts.StatusId,
            //                        StageName = stg.Stages,
            //                        Status = sts.Status
            //                    }
            //                   ).ToListAsync();

            //lst.StatusProjectlst = status;

            //var movent = await (from proj in _dbContext.Projects
            //                    join mov in _dbContext.ProjStakeHolderMov on proj.ProjId equals mov.ProjId
            //                    where proj.IsSubmited == true
            //                    orderby proj.ProjId descending
            //                    select new MovProject
            //                    {

            //                        ProjId = proj.ProjId,
            //                        ProjName = proj.ProjName,
            //                        TimeStamp = mov.TimeStamp,
            //                        StatusId = (mov.StatusActionsMappingId == 21) ? 1 ://New Projects
            //                                                                           // (mov.StatusActionsMappingId == 9) ? 2 ://Obsn
            //                                                                           // (mov.StatusActionsMappingId == 113) ? 3 ://Obsn Rectified
            //                    (mov.StatusActionsMappingId == 48) ? 20 ://Auto Committee
            //                    (mov.StatusActionsMappingId == 53) ? 21 ://IPA Stage
            //                                                             //(mov.StatusActionsMappingId == 60) ? 22 ://Closed
            //                    (mov.StatusActionsMappingId == 63) ? 24 ://AHCC (Arch Vetting)
            //                    (mov.StatusActionsMappingId == 68) ? 25 ://ACG (Lab Test)
            //                    (mov.StatusActionsMappingId == 73) ? 26 ://AHCC (IAM Integ)
            //                    (mov.StatusActionsMappingId == 78) ? 27 ://ACG (Remote Test)
            //                    (mov.StatusActionsMappingId == 83) ? 28 ://MI-11 Clearance
            //                    (mov.StatusActionsMappingId == 88) ? 29 ://Whitelisting Completed
            //                    (mov.StatusActionsMappingId == 26 && mov.IsComplete == true) ? 6 ://ASDC Vetting
            //                    (mov.StatusActionsMappingId == 31 && mov.IsComplete == true) ? 7 :// ACG Vetting
            //                    (mov.StatusActionsMappingId == 37 && mov.IsComplete == true) ? 11 : 0//AHCC Vetting

            //                        //  StatusId = mov.StatusActionsMappingId==1? "Yes" : "No";

            //                    }).ToListAsync();
            //lst.MovProjectlst = movent;

            //return lst;

            #endregion

            #region GetProjectWiseStatusWithProc

            DTOProjectWiseStatus result = new DTOProjectWiseStatus();
            result.StatusProjectlst = new List<StatusProject>();
            result.MovProjectlst = new List<MovProject>();

            using (SqlConnection conn = new SqlConnection(_dbContext.Database.GetConnectionString()))
            {
                await conn.OpenAsync();

                using (SqlCommand cmd = new SqlCommand("usp_GetProjectWiseStatus", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            result.StatusProjectlst.Add(new StatusProject
                            {
                                StatusId = reader.GetInt32(reader.GetOrdinal("StatusId")),
                                StageName = reader.GetString(reader.GetOrdinal("StageName")),
                                Status = reader.GetString(reader.GetOrdinal("Status"))
                            });
                        }

                        if (await reader.NextResultAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                result.MovProjectlst.Add(new MovProject
                                {
                                    ProjId = reader.GetInt32(reader.GetOrdinal("ProjId")),
                                    ProjName = reader.GetString(reader.GetOrdinal("ProjName")),
                                    TimeStamp = reader.GetDateTime(reader.GetOrdinal("TimeStamp")),
                                    StatusId = reader.GetInt32(reader.GetOrdinal("StatusId"))
                                });
                            }
                        }
                    }
                }
            }

            return result;
            #endregion
        }
        public async Task<List<DTOProjectsFwd>> GetDashboardApproved(int StatuId, int statusActionsMappingId)
        {
            #region GetDashBoardApprovedWithLinq
            //List<DTOProjectsFwd> lst = new List<DTOProjectsFwd>();
            //var status = _dbContext.mStatus.FirstOrDefault(x => x.StatusId == StatuId).Status;
            //if (status == "BISAG-N")
            //{
            //    lst = await (from p in _dbContext.Projects
            //                 join stk in _dbContext.tbl_mUnitBranch on p.StakeHolderId equals stk.unitid
            //                 where /*p.IsProcess == true*/ p.IsSubmited == true && p.BeingDevpInhouse == "BISAG-N"
            //                 select new DTOProjectsFwd
            //                 {
            //                     ProjId = p.ProjId,
            //                     ProjName = p.ProjName,
            //                     StakeHolder = stk.UnitName,
            //                     TimeStamp = p.InitiatedDate,
            //                     Status = "BISAG-N",
            //                     EncyID = _dataProtector.Protect(p.ProjId.ToString())
            //                 }).ToListAsync();
            //}
            //else if (status == "Re-Vetting")
            //{
            //    lst = await (from p in _dbContext.Projects
            //                 join stk in _dbContext.tbl_mUnitBranch on p.StakeHolderId equals stk.unitid
            //                 where /*p.IsProcess == true*/ p.IsSubmited == true && p.IsWhitelisted == "Re-Vetted"
            //                 select new DTOProjectsFwd
            //                 {
            //                     ProjId = p.ProjId,
            //                     ProjName = p.ProjName,
            //                     StakeHolder = stk.UnitName,
            //                     TimeStamp = p.InitiatedDate,
            //                     Status = "Re-Vetting",
            //                     EncyID = _dataProtector.Protect(p.ProjId.ToString())
            //                 }).ToListAsync();
            //}
            //else
            //{
            //    if (statusActionsMappingId == 1)
            //        statusActionsMappingId = 21;
            //    if (statusActionsMappingId == 26 || statusActionsMappingId == 31 || statusActionsMappingId == 37)
            //    {
            //        lst = await (from a in _dbContext.Projects
            //                     join mov in _dbContext.ProjStakeHolderMov on a.ProjId equals mov.ProjId
            //                     join stackc in _dbContext.tbl_mUnitBranch on a.StakeHolderId equals stackc.unitid into cs1
            //                     from stackcs in cs1.DefaultIfEmpty()
            //                     let datetime = (from mov1 in _dbContext.ProjStakeHolderMov
            //                                     join pro1 in _dbContext.Projects on mov1.ProjId equals pro1.ProjId
            //                                     where pro1.IsProcess == true && mov1.StatusActionsMappingId == statusActionsMappingId
            //                                    && pro1.ProjId == a.ProjId && mov.IsActive == true
            //                                     orderby mov1.PsmId
            //                                     select mov1.TimeStamp).FirstOrDefault()
            //                     where a.IsProcess == true && mov.StatusActionsMappingId == statusActionsMappingId
            //                     && mov.IsComplete == true && mov.IsActive == true
            //                     group mov by new
            //                     {
            //                         a.ProjId,
            //                         a.ProjName,
            //                         a.StakeHolderId,
            //                         stackcs.UnitName,
            //                         datetime
            //                     } into gr
            //                     select new DTOProjectsFwd
            //                     {
            //                         ProjId = gr.Key.ProjId,
            //                         ProjName = gr.Key.ProjName,
            //                         StakeHolderId = gr.Key.StakeHolderId,
            //                         StakeHolder = gr.Key.UnitName,
            //                         TimeStamp = gr.Key.datetime,
            //                         EncyID = _dataProtector.Protect(gr.Key.ProjId.ToString())
            //                     }).ToListAsync();
            //    }
            //    else if (statusActionsMappingId == 21)
            //    {
            //        lst = await (from a in _dbContext.Projects
            //                     join mov in _dbContext.ProjStakeHolderMov on a.ProjId equals mov.ProjId
            //                     join stackc in _dbContext.tbl_mUnitBranch on a.StakeHolderId equals stackc.unitid into cs1
            //                     from stackcs in cs1.DefaultIfEmpty()
            //                     let datetime = (from mov1 in _dbContext.ProjStakeHolderMov
            //                                     join pro1 in _dbContext.Projects on mov1.ProjId equals pro1.ProjId
            //                                     where pro1.IsProcess == true && mov1.StatusActionsMappingId == statusActionsMappingId
            //                                    && pro1.ProjId == a.ProjId && mov.IsActive == true
            //                                     orderby mov1.PsmId
            //                                     select mov1.TimeStamp).FirstOrDefault()

            //                     where a.IsProcess == true && mov.StatusActionsMappingId == statusActionsMappingId && mov.IsActive == true
            //                     group mov by new
            //                     {
            //                         a.ProjId,
            //                         a.ProjName,
            //                         a.StakeHolderId,
            //                         stackcs.UnitName,
            //                         datetime
            //                     } into gr
            //                     select new DTOProjectsFwd
            //                     {
            //                         ProjId = gr.Key.ProjId,
            //                         ProjName = gr.Key.ProjName,
            //                         StakeHolderId = gr.Key.StakeHolderId,
            //                         StakeHolder = gr.Key.UnitName,
            //                         TimeStamp = gr.Key.datetime,
            //                         EncyID = _dataProtector.Protect(gr.Key.ProjId.ToString())
            //                     }).ToListAsync();
            //    }
            //    else if (statusActionsMappingId == 9 || statusActionsMappingId == 15 || statusActionsMappingId == 48 ||
            //        statusActionsMappingId == 53 || statusActionsMappingId == 60 || statusActionsMappingId == 63 || statusActionsMappingId == 68 ||
            //        statusActionsMappingId == 73 || statusActionsMappingId == 78 || statusActionsMappingId == 83 ||
            //        statusActionsMappingId == 88)
            //    {
            //        lst = await (from a in _dbContext.Projects
            //                     join mov in _dbContext.ProjStakeHolderMov on a.ProjId equals mov.ProjId
            //                     join stackc in _dbContext.tbl_mUnitBranch on a.StakeHolderId equals stackc.unitid into cs1
            //                     from stackcs in cs1.DefaultIfEmpty()

            //                     let datetime = (from mov1 in _dbContext.ProjStakeHolderMov
            //                                     join pro1 in _dbContext.Projects on mov1.ProjId equals pro1.ProjId
            //                                     where pro1.IsProcess == true && mov1.StatusActionsMappingId == statusActionsMappingId
            //                                    && pro1.ProjId == a.ProjId && mov.IsActive == true
            //                                     orderby mov1.PsmId
            //                                     select mov1.TimeStamp).FirstOrDefault()
            //                     where a.IsProcess == true && mov.StatusActionsMappingId == statusActionsMappingId
            //                     && mov.IsActive == true
            //                     group mov by new
            //                     {
            //                         a.ProjId,
            //                         a.ProjName,
            //                         a.StakeHolderId,
            //                         stackcs.UnitName,
            //                         datetime
            //                     } into gr
            //                     select new DTOProjectsFwd
            //                     {
            //                         ProjId = gr.Key.ProjId,
            //                         ProjName = gr.Key.ProjName,
            //                         StakeHolderId = gr.Key.StakeHolderId,
            //                         StakeHolder = gr.Key.UnitName,
            //                         TimeStamp = gr.Key.datetime,
            //                         EncyID = _dataProtector.Protect(gr.Key.ProjId.ToString())
            //                     }).ToListAsync();
            //    }
            //}

            //return lst.OrderByDescending(i => i.TimeStamp).ToList();
            #endregion

            #region GetDashBoardApprovedWithProc

            try
            {
                //   var lst = await _dbContext.Set<DTOProjectsFwd>()
                //.FromSqlRaw("EXEC dbo.usp_GetDashboardApproved @StatusId = {0}, @StatusActionsMappingId = {1}", StatuId, statusActionsMappingId)
                //.ToListAsync();

                //   foreach (var item in lst)
                //   {
                //       item.EncyID = _dataProtector.Protect(item.ProjId.ToString());
                //   }
                //   return lst.OrderByDescending(i => i.TimeStamp).ToList();


                var lst = new List<DTOProjectsFwd>();

                using (var conn = _dbContext.Database.GetDbConnection())
                {
                    await conn.OpenAsync();

                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = "dbo.usp_GetDashboardApproved";
                        cmd.CommandType = CommandType.StoredProcedure;

                        var paramStatusId = cmd.CreateParameter();
                        paramStatusId.ParameterName = "@StatusId";
                        paramStatusId.Value = StatuId;
                        cmd.Parameters.Add(paramStatusId);

                        var paramStatusActionId = cmd.CreateParameter();
                        paramStatusActionId.ParameterName = "@StatusActionsMappingId";
                        paramStatusActionId.Value = statusActionsMappingId;
                        cmd.Parameters.Add(paramStatusActionId);


                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                var item = new DTOProjectsFwd
                                {
                                    ProjId = reader.GetInt32(reader.GetOrdinal("ProjId")),
                                    ProjName = reader.IsDBNull(reader.GetOrdinal("ProjName")) ? null : reader.GetString(reader.GetOrdinal("ProjName")),
                                    StakeHolder = reader.IsDBNull(reader.GetOrdinal("StakeHolder")) ? null : reader.GetString(reader.GetOrdinal("StakeHolder")),
                                    TimeStamp = reader.IsDBNull(reader.GetOrdinal("TimeStamp")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("TimeStamp")),
                                };

                                item.EncyID = _dataProtector.Protect(item.ProjId.ToString());

                                lst.Add(item);
                            }
                        }
                    }
                }

                return lst.OrderByDescending(i => i.TimeStamp).ToList();
            }
            catch (Exception ex)
            {

                throw;
            }

            #endregion
        }
        public async Task<List<DTOProjectsFwd>> GetDashboardStatusDetails(int StatuId, int UnitId, bool IsDuplicate)
        {
            Login Logins = SessionHelper.GetObjectFromJson<Login>(_httpContextAccessor.HttpContext.Session, "User");

            List<DTOProjectsFwd> lst = new List<DTOProjectsFwd>();

            if (Logins != null)
            {
                #region GetDashboardStatusDetialsWithLinq
                int stkholder = Logins.unitid.HasValue ? Logins.unitid.Value : 0;

                string username = Logins.UserName;

                if (StatuId == 2 || StatuId == 3 || StatuId == 22 || StatuId == 31 || StatuId == 37)
                {
                    int[] StatusActionsMappingId = null;
                    if (StatuId == 2)
                        StatusActionsMappingId = new int[] { 4 };
                    else if (StatuId == 3)
                        StatusActionsMappingId = new int[] { 118 };
                    else if (StatuId == 22)
                        StatusActionsMappingId = new int[] { 49, 54 };
                    else if (StatuId == 31)
                        StatusActionsMappingId = new int[] { 64, 69, 74, 79, 84, 89 };
                    else if (StatuId == 37)
                        StatusActionsMappingId = new int[] { 3 };

                    var query = await (from a in _dbContext.Projects
                                       join b in _dbContext.ProjStakeHolderMov on a.ProjId equals b.ProjId
                                       join stackc in _dbContext.tbl_mUnitBranch on a.StakeHolderId equals stackc.unitid into cs1
                                       from stackcs in cs1.DefaultIfEmpty()
                                       join actm in _dbContext.TrnStatusActionsMapping
                                       on
                                       b.StatusActionsMappingId equals actm.StatusActionsMappingId
                                       join d in _dbContext.mStatus on actm.StatusId equals d.StatusId

                                       join k in _dbContext.mActions on actm.ActionsId equals k.ActionsId

                                       join c in _dbContext.tbl_mUnitBranch on b.ToUnitId equals c.unitid into cs
                                       from toUnit in cs.DefaultIfEmpty()

                                       join g in _dbContext.tbl_mUnitBranch on b.FromUnitId equals g.unitid into cg
                                       from fromUnits in cg.DefaultIfEmpty()

                                       join j in _dbContext.mStages on d.StageId equals j.StagesId

                                       join f in _dbContext.Comment on b.PsmId equals f.PsmId into fs
                                       from eWithComment in fs.DefaultIfEmpty()

                                       let StkStatusId =
                                      (from cr1 in _dbContext.StkComment
                                       join Stdkst in _dbContext.StkStatus on cr1.StkStatusId equals Stdkst.StkStatusId
                                       where cr1.StakeHolderId == b.ToUnitId && cr1.PsmId == b.PsmId
                                       orderby cr1.StkCommentId descending
                                       select cr1.StkStatusId
                                      ).FirstOrDefault()

                                       where a.IsActive && !a.IsDeleted && b.IsActive && !b.IsDeleted && a.IsSubmited == true //&& b.IsComplete == false
                                                                                                                              //&& b.ToUnitId == Logins.unitid 
                                       && StatusActionsMappingId.Contains(b.StatusActionsMappingId)

                                       orderby a.ProjName, b.DateTimeOfUpdate descending

                                       select new DTOProjectsFwd
                                       {
                                           ProjId = a.ProjId,
                                           PsmIds = b.PsmId,
                                           ProjName = a.ProjName,
                                           StakeHolderId = a.StakeHolderId,
                                           StakeHolder = stackcs.UnitName,
                                           //Remarks= b != null ? b.Remarks : null,
                                           Status = d.Status,
                                           Stage = j.Stages,
                                           FromUnitId = b.FromUnitId,
                                           FromUnitName = fromUnits.UnitName,
                                           ToUnitId = b.ToUnitId,
                                           ToUnitName = toUnit.UnitName,
                                           Action = k.Actions,
                                           TotalDays = 0,
                                           StageId = j.StagesId,
                                           EncyID = _dataProtector.Protect(a.ProjId.ToString()),
                                           EncyPsmID = _dataProtector.Protect(b.PsmId.ToString()),
                                           IsProcess = a.IsProcess,
                                           IsRead = b.IsRead,
                                           IsComplete = b.IsComplete,
                                           StkStatusId = Convert.ToInt32(StkStatusId),
                                           DateTimeOfUpdate = b.DateTimeOfUpdate
                                           //DateTimeOfUpdate = _dbContext.ProjStakeHolderMov.Where(i => i.ProjId == a.ProjId).Select(x => x.DateTimeOfUpdate).Max()
                                       }).ToListAsync();

                    lst = query;
                }
                else
                {
                    if (IsDuplicate == false)
                    {
                        var query = await (from a in _dbContext.Projects
                                           join b in _dbContext.ProjStakeHolderMov on a.ProjId equals b.ProjId
                                           join stackc in _dbContext.tbl_mUnitBranch on a.StakeHolderId equals stackc.unitid into cs1
                                           from stackcs in cs1.DefaultIfEmpty()
                                           join actm in _dbContext.TrnStatusActionsMapping on b.StatusActionsMappingId equals actm.StatusActionsMappingId
                                           join d in _dbContext.mStatus on actm.StatusId equals d.StatusId
                                           join k in _dbContext.mActions on actm.ActionsId equals k.ActionsId

                                           join c in _dbContext.tbl_mUnitBranch on b.ToUnitId equals c.unitid into cs
                                           from toUnit in cs.DefaultIfEmpty()

                                           join g in _dbContext.tbl_mUnitBranch on b.FromUnitId equals g.unitid into cg
                                           from fromUnits in cg.DefaultIfEmpty()

                                           join j in _dbContext.mStages on d.StageId equals j.StagesId


                                           join f in _dbContext.Comment on b.PsmId equals f.PsmId into fs
                                           from eWithComment in fs.DefaultIfEmpty()

                                           let StkStatusId =
                                          (from cr1 in _dbContext.StkComment
                                           join Stdkst in _dbContext.StkStatus on cr1.StkStatusId equals Stdkst.StkStatusId
                                           where cr1.StakeHolderId == b.ToUnitId && cr1.PsmId == b.PsmId
                                           orderby cr1.StkCommentId descending
                                           select cr1.StkStatusId
                                          ).FirstOrDefault()

                                           let psmiis = (from mov1 in _dbContext.ProjStakeHolderMov
                                                         join stst1 in _dbContext.TrnStatusActionsMapping on mov1.StatusActionsMappingId equals stst1.StatusActionsMappingId
                                                         where mov1.ProjId == a.ProjId && stst1.StatusId == StatuId
                                                         orderby mov1.PsmId descending
                                                         select mov1.PsmId).FirstOrDefault()

                                           where a.IsActive && !a.IsDeleted && b.IsActive && !b.IsDeleted && a.IsSubmited == true //&& b.IsComplete == false
                                                    && b.StatusActionsMappingId != 118 && b.StatusActionsMappingId != 4                                                                              //&& b.ToUnitId == Logins.unitid 
                                            && actm.StatusId == StatuId
                                            && b.PsmId == psmiis
                                           orderby a.ProjName, b.DateTimeOfUpdate descending

                                           select new DTOProjectsFwd
                                           {
                                               ProjId = a.ProjId,
                                               PsmIds = b.PsmId,
                                               ProjName = a.ProjName,
                                               StakeHolderId = a.StakeHolderId,
                                               StakeHolder = stackcs.UnitName,
                                               //Remarks= b != null ? b.Remarks : null,
                                               Status = d.Status,
                                               Stage = j.Stages,
                                               FromUnitId = b.FromUnitId,
                                               FromUnitName = fromUnits.UnitName,
                                               ToUnitId = b.ToUnitId,
                                               ToUnitName = toUnit.UnitName,
                                               Action = k.Actions,
                                               TotalDays = 0,
                                               StageId = j.StagesId,
                                               EncyID = _dataProtector.Protect(a.ProjId.ToString()),
                                               EncyPsmID = _dataProtector.Protect(b.PsmId.ToString()),
                                               IsProcess = a.IsProcess,
                                               IsRead = b.IsRead,
                                               IsComplete = b.IsComplete,
                                               StkStatusId = Convert.ToInt32(StkStatusId),
                                               DateTimeOfUpdate = b.DateTimeOfUpdate
                                               //DateTimeOfUpdate = _dbContext.ProjStakeHolderMov.Where(i => i.ProjId == a.ProjId).Select(x => x.DateTimeOfUpdate).Max()
                                           }).ToListAsync();

                        lst = query;
                    }
                    else
                    {
                        var query = await (from a in _dbContext.Projects
                                           join b in _dbContext.ProjStakeHolderMov on a.ProjId equals b.ProjId
                                           join stackc in _dbContext.tbl_mUnitBranch on a.StakeHolderId equals stackc.unitid into cs1
                                           from stackcs in cs1.DefaultIfEmpty()
                                           join actm in _dbContext.TrnStatusActionsMapping on b.StatusActionsMappingId equals actm.StatusActionsMappingId
                                           join d in _dbContext.mStatus on actm.StatusId equals d.StatusId
                                           join k in _dbContext.mActions on actm.ActionsId equals k.ActionsId

                                           join c in _dbContext.tbl_mUnitBranch on b.ToUnitId equals c.unitid into cs
                                           from toUnit in cs.DefaultIfEmpty()

                                           join g in _dbContext.tbl_mUnitBranch on b.FromUnitId equals g.unitid into cg
                                           from fromUnits in cg.DefaultIfEmpty()

                                           join j in _dbContext.mStages on d.StageId equals j.StagesId


                                           join f in _dbContext.Comment on b.PsmId equals f.PsmId into fs
                                           from eWithComment in fs.DefaultIfEmpty()

                                           let StkStatusId =
                                          (from cr1 in _dbContext.StkComment
                                           join Stdkst in _dbContext.StkStatus on cr1.StkStatusId equals Stdkst.StkStatusId
                                           where cr1.StakeHolderId == b.ToUnitId && cr1.PsmId == b.PsmId
                                           orderby cr1.StkCommentId descending
                                           select cr1.StkStatusId
                                          ).FirstOrDefault()

                                           where a.IsActive && !a.IsDeleted && b.IsActive && !b.IsDeleted && a.IsSubmited == true //&& b.IsComplete == false
                                                    && b.StatusActionsMappingId != 118 && b.StatusActionsMappingId != 4                                                                                        //&& b.ToUnitId == Logins.unitid 
                                            && actm.StatusId == StatuId

                                           orderby a.ProjName, b.DateTimeOfUpdate descending

                                           select new DTOProjectsFwd
                                           {
                                               ProjId = a.ProjId,
                                               PsmIds = b.PsmId,
                                               ProjName = a.ProjName,
                                               StakeHolderId = a.StakeHolderId,
                                               StakeHolder = stackcs.UnitName,
                                               //Remarks= b != null ? b.Remarks : null,
                                               Status = d.Status,
                                               Stage = j.Stages,
                                               FromUnitId = b.FromUnitId,
                                               FromUnitName = fromUnits.UnitName,
                                               ToUnitId = b.ToUnitId,
                                               ToUnitName = toUnit.UnitName,
                                               Action = k.Actions,
                                               TotalDays = 0,
                                               StageId = j.StagesId,
                                               EncyID = _dataProtector.Protect(a.ProjId.ToString()),
                                               EncyPsmID = _dataProtector.Protect(b.PsmId.ToString()),
                                               IsProcess = a.IsProcess,
                                               IsRead = b.IsRead,
                                               IsComplete = b.IsComplete,
                                               StkStatusId = Convert.ToInt32(StkStatusId),
                                               DateTimeOfUpdate = b.DateTimeOfUpdate
                                               //DateTimeOfUpdate = _dbContext.ProjStakeHolderMov.Where(i => i.ProjId == a.ProjId).Select(x => x.DateTimeOfUpdate).Max()
                                           }).ToListAsync();

                        lst = query;
                    }
                }
                var RETT = lst.OrderByDescending(i => i.DateTimeOfUpdate).ToList();

                return RETT;/*.OrderBy(i => i.DateTimeOfUpdate).ToList();*/
                #endregion

                #region GetDashboardStatusDetialsWithProcedure 

                //using (var conn = _dbContext.Database.GetDbConnection())
                //{
                //    await conn.OpenAsync();

                //    using (var cmd = conn.CreateCommand())
                //    {
                //        cmd.CommandText = "dbo.usp_GetDashboardStatusDetails";
                //        cmd.CommandType = CommandType.StoredProcedure;

                //        // Add parameters
                //        var p1 = cmd.CreateParameter();
                //        p1.ParameterName = "@StatusId";
                //        p1.Value = StatuId;
                //        cmd.Parameters.Add(p1);

                //        var p2 = cmd.CreateParameter();
                //        p2.ParameterName = "@UnitId";
                //        p2.Value = UnitId;
                //        cmd.Parameters.Add(p2);

                //        var p3 = cmd.CreateParameter();
                //        p3.ParameterName = "@IsDuplicate";
                //        p3.Value = IsDuplicate;
                //        cmd.Parameters.Add(p3);

                //        using (var reader = await cmd.ExecuteReaderAsync())
                //        {
                //            while (await reader.ReadAsync())
                //            {
                //                var item = new DTOProjectsFwd
                //                {
                //                    ProjId = reader.GetInt32(reader.GetOrdinal("ProjId")),
                //                    PsmIds = reader.GetInt32(reader.GetOrdinal("PsmIds")),
                //                    ProjName = reader.IsDBNull(reader.GetOrdinal("ProjName")) ? null : reader.GetString(reader.GetOrdinal("ProjName")),
                //                    StakeHolderId = reader.GetInt32(reader.GetOrdinal("StakeHolderId")),
                //                    StakeHolder = reader.IsDBNull(reader.GetOrdinal("StakeHolder")) ? null : reader.GetString(reader.GetOrdinal("StakeHolder")),
                //                    Status = reader.IsDBNull(reader.GetOrdinal("Status")) ? null : reader.GetString(reader.GetOrdinal("Status")),
                //                    Stage = reader.IsDBNull(reader.GetOrdinal("Stage")) ? null : reader.GetString(reader.GetOrdinal("Stage")),
                //                    FromUnitId = reader.GetInt32(reader.GetOrdinal("FromUnitId")),
                //                    FromUnitName = reader.IsDBNull(reader.GetOrdinal("FromUnitName")) ? null : reader.GetString(reader.GetOrdinal("FromUnitName")),
                //                    ToUnitId = reader.GetInt32(reader.GetOrdinal("ToUnitId")),
                //                    ToUnitName = reader.IsDBNull(reader.GetOrdinal("ToUnitName")) ? null : reader.GetString(reader.GetOrdinal("ToUnitName")),
                //                    Action = reader.IsDBNull(reader.GetOrdinal("Action")) ? null : reader.GetString(reader.GetOrdinal("Action")),
                //                    StageId = reader.GetInt32(reader.GetOrdinal("StageId")),
                //                    IsRead = reader.GetBoolean(reader.GetOrdinal("IsRead")),
                //                    IsComplete = reader.GetBoolean(reader.GetOrdinal("IsComplete")),
                //                    StkStatusId = reader.GetInt32(reader.GetOrdinal("StkStatusId")),
                //                    DateTimeOfUpdate = reader.IsDBNull(reader.GetOrdinal("DateTimeOfUpdate")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("DateTimeOfUpdate")),
                //                    EncyID = _dataProtector.Protect(reader.GetInt32(reader.GetOrdinal("ProjId")).ToString()),
                //                    EncyPsmID = _dataProtector.Protect(reader.GetInt32(reader.GetOrdinal("PsmIds")).ToString())
                //                };

                //                lst.Add(item);
                //            }
                //        }
                //    }
                //}

                //return lst.OrderByDescending(i => i.DateTimeOfUpdate).ToList();
                #endregion
            }
            else
            {
                return null;
            }
        }
        public async Task<bool> ProjectNameExists(tbl_Projects project)
        {
            var ret = await _dbContext.Projects.AnyAsync(i => i.ProjName.Trim().ToUpper() == project.ProjName.Trim().ToUpper() && i.ProjId != project.ProjId);
            return ret;
        }
        public async Task<int> AddProjectAsync(tbl_Projects project)
        {
            try
            {
                Login Logins = SessionHelper.GetObjectFromJson<Login>(_httpContextAccessor.HttpContext.Session, "User");


                tbl_Comment cmt = new tbl_Comment();
                _dbContext.Projects.Add(project);
                await _dbContext.SaveChangesAsync();

                ProjIDRes PjIR = new ProjIDRes();
                PjIR = swas.BAL.Utility.ExtensionMethods.FirstSecond(project.ProjName, project.ProjId, 0);

                if (PjIR.PorjPin == null)
                {

                    project.ProjCode = "Error";

                }
                else
                {
                    project.ProjCode = PjIR.PorjPin;

                }

                tbl_ProjStakeHolderMov psmove = new tbl_ProjStakeHolderMov();


                psmove.ProjId = project.ProjId;
                psmove.StatusActionsMappingId = 1; //21  //ajayupdate
                                                   //psmove.ActionId = 1;
                psmove.Remarks = project.InitialRemark;
                psmove.FromUnitId = Logins.unitid ?? 0;
                psmove.ToUnitId = 1; //  
                                     //psmove.TostackholderDt = DateTime.Now;  
                psmove.UserDetails = Helper1.LoginDetails(Logins);
                psmove.UpdatedByUserId = Logins.unitid; // change with userid
                psmove.DateTimeOfUpdate = project.InitiatedDate;
                psmove.IsActive = true;

                psmove.EditDeleteDate = project.InitiatedDate;
                psmove.EditDeleteBy = Logins.unitid;
                psmove.TimeStamp = project.InitiatedDate;
                psmove.IsComplete = false;
                psmove.IsComment = false;
                psmove.IsPullBack = false;
                _dbContext.ProjStakeHolderMov.Add(psmove);
                await _dbContext.SaveChangesAsync();

                var projectToUpdate = await _dbContext.Projects.FirstOrDefaultAsync(a => a.ProjId == project.ProjId);
                if (projectToUpdate != null)
                {
                    projectToUpdate.CurrentPslmId = psmove.PsmId;
                    await _dbContext.SaveChangesAsync();
                }

                cmt.EditDeleteDate = project.InitiatedDate;
                cmt.IsDeleted = false;
                cmt.IsActive = true;
                cmt.DateTimeOfUpdate = project.InitiatedDate;
                cmt.Comment = project.InitialRemark;
                cmt.PsmId = psmove.PsmId;
                cmt.UpdatedByUserId = Logins.UserIntId;
                cmt.EditDeleteBy = 0;
                _dbContext.Comment.Add(cmt);
                await _dbContext.SaveChangesAsync();
                if (project.UploadedFile != null)
                {
                    tbl_AttHistory atthis = new tbl_AttHistory();
                    atthis.AttPath = project.UploadedFile;
                    atthis.ActFileName = project.ActFileName;
                    atthis.PsmId = psmove.PsmId;
                    atthis.UpdatedByUserId = Logins.unitid;
                    atthis.DateTimeOfUpdate = project.InitiatedDate;
                    atthis.IsDeleted = false;
                    atthis.IsActive = true;
                    atthis.EditDeleteBy = Logins.unitid;
                    atthis.ActionId = 1;
                    atthis.TimeStamp = project.InitiatedDate;
                    atthis.Reamarks = psmove.Remarks ?? "File Attached";

                    _dbContext.AttHistory.Add(atthis);
                    await _dbContext.SaveChangesAsync();
                }
                project.CurrentPslmId = psmove.PsmId;

                return project.ProjId;
            }
            catch (Exception ex)
            {
                return nmum.Exception;
            }
        }

        public async Task<bool> DeleteProjectAsync(int projectId)
        {
            Login Logins = SessionHelper.GetObjectFromJson<Login>(_httpContextAccessor.HttpContext.Session, "User");
            if (Logins != null)
            {
                var project = await _dbContext.Projects.FindAsync(projectId);
                if (project == null)
                    return false;

                _dbContext.Projects.Remove(project);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<List<tbl_Projects>> GetActComplettemsAsync()
        {
            Login Logins = SessionHelper.GetObjectFromJson<Login>(_httpContextAccessor.HttpContext.Session, "User");
            #region GetActComplettemsAsyncWithLinq

            //if (Logins != null)
            //{
            //    int stkholder = Logins.unitid.HasValue ? Logins.unitid.Value : 0;

            //    var querys = from a in _dbContext.ProjStakeHolderMov
            //                 join b in _dbContext.Projects on a.ProjId equals b.ProjId
            //                 join c in _dbContext.tbl_mUnitBranch on a.ToUnitId equals c.unitid into currentStakeHolder
            //                 from current in currentStakeHolder.DefaultIfEmpty()
            //                 join d in _dbContext.tbl_mUnitBranch on a.ToUnitId equals d.unitid into toStakeHolder
            //                 from to in toStakeHolder.DefaultIfEmpty()
            //                 join e in _dbContext.tbl_mUnitBranch on a.FromUnitId equals e.unitid into fromStakeHolder
            //                 from fromStake in fromStakeHolder.DefaultIfEmpty()
            //                 join l in _dbContext.tbl_mUnitBranch on b.StakeHolderId equals l.unitid into stkhol
            //                 from stk in stkhol.DefaultIfEmpty()


            //                 join f in _dbContext.Comment on a.PsmId equals f.PsmId into fs
            //                 from eWithComment in fs.DefaultIfEmpty()
            //                 join actm in _dbContext.TrnStatusActionsMapping on a.StatusActionsMappingId equals actm.StatusActionsMappingId
            //                 join h in _dbContext.mStatus on actm.StatusId equals h.StatusId into hs
            //                 from eWithStatus in hs.DefaultIfEmpty()
            //                 join j in _dbContext.mStages on eWithStatus.StageId equals j.StagesId into js
            //                 from eWithStages in js.DefaultIfEmpty()
            //                 join k in _dbContext.mActions on actm.ActionsId equals k.ActionsId into ks
            //                 from eWithAction in ks.DefaultIfEmpty()
            //                 where b.IsSubmited == true
            //                 && actm.StatusActionsMappingId == 103
            //                 // where a.ActionId == dft.ActionId
            //                 select new tbl_Projects
            //                 {
            //                     ProjId = b.ProjId,
            //                     PsmIds = a.PsmId,
            //                     ProjName = b.ProjName,
            //                     StakeHolderId = b.StakeHolderId,
            //                     CurrentPslmId = b.CurrentPslmId,
            //                     InitiatedDate = b.InitiatedDate,
            //                     CompletionDate = b.CompletionDate,
            //                     IsWhitelisted = b.IsWhitelisted,
            //                     InitialRemark = b.InitialRemark,
            //                     EditDeleteBy = a.EditDeleteBy,
            //                     EditDeleteDate = a.TimeStamp,
            //                     UpdatedByUserId = a.UpdatedByUserId,
            //                     DateTimeOfUpdate = a.DateTimeOfUpdate,
            //                     ProjCode = b.ProjCode,
            //                     RecdFmUser = fromStake != null ? fromStake.UnitName : null,
            //                     FwdtoUser = to != null ? to.UnitName : null,
            //                     FwdBy = current != null ? current.UnitName : null,
            //                     AdRemarks = a.Remarks,
            //                     Comments = eWithComment.Comment,
            //                     Status = eWithStatus.Status,
            //                     Stages = eWithStages.Stages,
            //                     Action = eWithAction.Actions,
            //                     StakeHolder = stk.UnitName,
            //                     AttCnt = _dbContext.AttHistory.Count(f => f.PsmId == b.CurrentPslmId),
            //                     HostTypeID = b.HostTypeID,
            //                     EncyID = _dataProtector.Protect(b.ProjId.ToString())
            //                 };
            //    var projectsWithDetails = await querys.ToListAsync();
            //    return projectsWithDetails;
            //}
            //else
            //{
            //    return null;
            //}
            #endregion

            #region GetActComplettemsAsyncWithProc

            if (Logins != null)
            {
                List<tbl_Projects> result = new List<tbl_Projects>();

                using (SqlConnection conn = new SqlConnection(_dbContext.Database.GetConnectionString()))
                {
                    await conn.OpenAsync();
                    using (SqlCommand cmd = new SqlCommand("usp_GetActCompleteItems", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@UnitId", Logins.unitid ?? 0);

                        using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                tbl_Projects project = new tbl_Projects
                                {
                                    ProjId = reader.GetInt32(reader.GetOrdinal("ProjId")),
                                    PsmIds = reader.GetInt32(reader.GetOrdinal("PsmIds")),
                                    ProjName = reader.GetString(reader.GetOrdinal("ProjName")),
                                    StakeHolderId = reader.GetInt32(reader.GetOrdinal("StakeHolderId")),
                                    CurrentPslmId = reader.GetInt32(reader.GetOrdinal("CurrentPslmId")),
                                    InitiatedDate = reader.IsDBNull(reader.GetOrdinal("InitiatedDate")) ? null : reader.GetDateTime(reader.GetOrdinal("InitiatedDate")),
                                    CompletionDate = reader.IsDBNull(reader.GetOrdinal("CompletionDate")) ? null : reader.GetDateTime(reader.GetOrdinal("CompletionDate")),
                                    IsWhitelisted = reader.GetOrdinal("IsWhitelisted").ToString(),
                                    InitialRemark = reader.IsDBNull(reader.GetOrdinal("InitialRemark")) ? null : reader.GetString(reader.GetOrdinal("InitialRemark")),
                                    EditDeleteBy = reader.IsDBNull(reader.GetOrdinal("EditDeleteBy")) ? null : reader.GetOrdinal("EditDeleteBy"),
                                    EditDeleteDate = reader.IsDBNull(reader.GetOrdinal("EditDeleteDate")) ? null : reader.GetDateTime(reader.GetOrdinal("EditDeleteDate")),
                                    UpdatedByUserId = reader.IsDBNull(reader.GetOrdinal("UpdatedByUserId")) ? null : reader.GetOrdinal("UpdatedByUserId"),
                                    DateTimeOfUpdate = reader.GetDateTime(reader.GetOrdinal("DateTimeOfUpdate")),
                                    ProjCode = reader.IsDBNull(reader.GetOrdinal("ProjCode")) ? null : reader.GetString(reader.GetOrdinal("ProjCode")),
                                    RecdFmUser = reader.IsDBNull(reader.GetOrdinal("RecdFmUser")) ? null : reader.GetString(reader.GetOrdinal("RecdFmUser")),
                                    FwdtoUser = reader.IsDBNull(reader.GetOrdinal("FwdtoUser")) ? null : reader.GetString(reader.GetOrdinal("FwdtoUser")),
                                    FwdBy = reader.IsDBNull(reader.GetOrdinal("FwdBy")) ? null : reader.GetString(reader.GetOrdinal("FwdBy")),
                                    AdRemarks = reader.IsDBNull(reader.GetOrdinal("AdRemarks")) ? null : reader.GetString(reader.GetOrdinal("AdRemarks")),
                                    Comments = reader.IsDBNull(reader.GetOrdinal("Comment")) ? null : reader.GetString(reader.GetOrdinal("Comment")),
                                    Status = reader.IsDBNull(reader.GetOrdinal("Status")) ? null : reader.GetString(reader.GetOrdinal("Status")),
                                    Stages = reader.IsDBNull(reader.GetOrdinal("Stages")) ? null : reader.GetString(reader.GetOrdinal("Stages")),
                                    Action = reader.IsDBNull(reader.GetOrdinal("Actions")) ? null : reader.GetString(reader.GetOrdinal("Actions")),
                                    StakeHolder = reader.IsDBNull(reader.GetOrdinal("StakeHolder")) ? null : reader.GetString(reader.GetOrdinal("StakeHolder")),
                                    AttCnt = reader.GetInt32(reader.GetOrdinal("AttCnt")),
                                    HostTypeID = reader.GetInt32(reader.GetOrdinal("HostTypeID"))
                                };

                                // Protect IDs
                                project.EncyID = _dataProtector.Protect(project.ProjId.ToString());

                                result.Add(project);
                            }
                        }
                    }
                }

                return result;
            }
            else
            {
                return null;
            }
            #endregion
        }

        public async Task<List<tbl_Projects>> GetActDraftItemsAsync()
        {
            Login Logins = SessionHelper.GetObjectFromJson<Login>(_httpContextAccessor.HttpContext.Session, "User");

            #region GetActDraftItemAsyncWithLinq

            //int stkholder = Logins.unitid.HasValue ? Logins.unitid.Value : 0;
            //string username = Logins.UserName;

            //if (Logins != null)
            //{
            //    var querys = from a in _dbContext.ProjStakeHolderMov
            //                 join b in _dbContext.Projects on a.ProjId equals b.ProjId
            //                 join c in _dbContext.tbl_mUnitBranch on a.ToUnitId equals c.unitid into currentStakeHolder
            //                 from current in currentStakeHolder.DefaultIfEmpty()
            //                 join m in _DBContext.tbl_mUnitBranch on b.StakeHolderId equals m.unitid into stakehold
            //                 from stk in stakehold.DefaultIfEmpty()
            //                 join d in _dbContext.tbl_mUnitBranch on a.ToUnitId equals d.unitid into toStakeHolder
            //                 from to in toStakeHolder.DefaultIfEmpty()
            //                 join e in _dbContext.tbl_mUnitBranch on a.FromUnitId equals e.unitid into fromStakeHolder
            //                 from fromStake in fromStakeHolder.DefaultIfEmpty()
            //                 join f in _dbContext.Comment on a.PsmId equals f.PsmId into fs
            //                 from eWithComment in fs.DefaultIfEmpty()
            //                 join actm in _dbContext.TrnStatusActionsMapping on a.StatusActionsMappingId equals actm.StatusActionsMappingId
            //                 join h in _dbContext.mStatus on actm.StatusId equals h.StatusId into hs
            //                 from eWithStatus in hs.DefaultIfEmpty()
            //                 join j in _dbContext.mStages on eWithStatus.StageId equals j.StagesId into js
            //                 from eWithStages in js.DefaultIfEmpty()
            //                 join k in _dbContext.mActions on actm.ActionsId equals k.ActionsId into ks
            //                 from eWithAction in ks.DefaultIfEmpty()
            //                 where a.FromUnitId == Logins.unitid && b.IsSubmited == false && a.IsComplete == false
            //                 select new tbl_Projects
            //                 {
            //                     ProjId = b.ProjId,
            //                     PsmIds = a.PsmId,
            //                     ProjName = b.ProjName,
            //                     StakeHolderId = b.StakeHolderId,
            //                     CurrentPslmId = b.CurrentPslmId,
            //                     InitiatedDate = b.InitiatedDate,
            //                     CompletionDate = b.CompletionDate,
            //                     IsWhitelisted = b.IsWhitelisted,
            //                     InitialRemark = b.InitialRemark,
            //                     EditDeleteBy = a.EditDeleteBy,
            //                     EditDeleteDate = a.TimeStamp,
            //                     UpdatedByUserId = a.UpdatedByUserId,
            //                     DateTimeOfUpdate = a.DateTimeOfUpdate,
            //                     ProjCode = b.ProjCode,
            //                     RecdFmUser = fromStake != null ? fromStake.UnitName : null,
            //                     FwdtoUser = to != null ? to.UnitName : null,
            //                     FwdBy = current != null ? current.UnitName : null,
            //                     AdRemarks = a.Remarks,
            //                     Comments = eWithComment.Comment,
            //                     // FwdtoDate = a.TostackholderDt,
            //                     Status = eWithStatus.Status,
            //                     Stages = eWithStages.Stages,
            //                     StakeHolder = stk.UnitName,
            //                     Action = eWithAction.Actions,
            //                     TotalDays = 15,
            //                     //ActionCde = a.ActionCde,
            //                     AttCnt = _dbContext.AttHistory.Count(f => f.PsmId == b.CurrentPslmId),
            //                     HostTypeID = b.HostTypeID,
            //                     EncyID = _dataProtector.Protect(b.CurrentPslmId.ToString()),
            //                     EncyPsmID = _dataProtector.Protect(a.PsmId.ToString()),
            //                 };

            //    var projectsWithDetails = await querys.ToListAsync();
            //    return projectsWithDetails;
            //}
            //else
            //{
            //    return null;
            //}
            #endregion


            #region GetActDraftItemsAsyncWithProc

            var result = new List<tbl_Projects>();

            if (Logins == null)
                return null;
            int unitId = Logins.unitid ?? 0;
            using (SqlConnection connection = new SqlConnection(_dbContext.Database.GetConnectionString()))
            {
                //using var connection = _dbContext.Database.GetDbConnection();
                await connection.OpenAsync();

                using var command = connection.CreateCommand();
                command.CommandText = "usp_GetActDraftItems";
                command.CommandType = CommandType.StoredProcedure;

                var unitParam = command.CreateParameter();
                unitParam.ParameterName = "@UnitId";
                unitParam.Value = unitId;
                command.Parameters.Add(unitParam);

                using var reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    int psmId = reader.GetInt32(reader.GetOrdinal("PsmIds"));
                    int projId = reader.GetInt32(reader.GetOrdinal("ProjId"));
                    int currentPslmId = reader.GetInt32(reader.GetOrdinal("CurrentPslmId"));

                    result.Add(new tbl_Projects
                    {
                        ProjId = projId,
                        PsmIds = psmId,
                        ProjName = reader["ProjName"]?.ToString(),
                        StakeHolderId = reader.GetInt32(reader.GetOrdinal("StakeHolderId")),
                        CurrentPslmId = currentPslmId,
                        InitiatedDate = reader.GetDateTime(reader.GetOrdinal("InitiatedDate")),
                        CompletionDate = reader.IsDBNull("CompletionDate") ? null : reader.GetDateTime(reader.GetOrdinal("CompletionDate")),
                        IsWhitelisted = reader["IsWhitelisted"]?.ToString(),
                        InitialRemark = reader["InitialRemark"]?.ToString(),
                        EditDeleteBy = (int)reader["EditDeleteBy"],
                        EditDeleteDate = reader.GetDateTime(reader.GetOrdinal("EditDeleteDate")),
                        UpdatedByUserId = (int)reader["UpdatedByUserId"],
                        DateTimeOfUpdate = reader.GetDateTime(reader.GetOrdinal("DateTimeOfUpdate")),
                        ProjCode = reader["ProjCode"]?.ToString(),
                        RecdFmUser = reader["RecdFmUser"]?.ToString(),
                        FwdtoUser = reader["FwdtoUser"]?.ToString(),
                        FwdBy = reader["FwdBy"]?.ToString(),
                        AdRemarks = reader["AdRemarks"]?.ToString(),
                        Comments = reader["Comment"]?.ToString(),
                        Status = reader["Status"]?.ToString(),
                        Stages = reader["Stages"]?.ToString(),
                        StakeHolder = reader["StakeHolder"]?.ToString(),
                        Action = reader["Action"]?.ToString(),
                        TotalDays = 15,
                        AttCnt = reader.GetInt32(reader.GetOrdinal("AttCnt")),
                        HostTypeID = reader.GetInt32(reader.GetOrdinal("HostTypeID")),
                        EncyID = _dataProtector.Protect(currentPslmId.ToString()),
                        EncyPsmID = _dataProtector.Protect(psmId.ToString())
                    });
                }

                return result;
            }
            
            #endregion

        }

        public async Task<List<DTOProjectsFwd>> GetActInboxAsync()
        {
            #region GetActInboxAsyncWithLinq
            //Login Logins = SessionHelper.GetObjectFromJson<Login>(_httpContextAccessor.HttpContext.Session, "User");
            //if (Logins != null)
            //{
            //    int stkholder = Logins.unitid.HasValue ? Logins.unitid.Value : 0;

            //    string username = Logins.UserName;

            //    var query = from a in _dbContext.Projects
            //                join b in _dbContext.ProjStakeHolderMov on a.ProjId equals b.ProjId
            //                join stackc in _dbContext.tbl_mUnitBranch on a.StakeHolderId equals stackc.unitid into cs1
            //                from stackcs in cs1.DefaultIfEmpty()

            //                //join da in _dbContext.DateApproval.Where(x => x.DDGIT_approval == true)
            //                //on a.ProjId equals da.ProjId into approvalGroup
            //                //from adminApproval in approvalGroup.DefaultIfEmpty()

            //                join actm in _dbContext.TrnStatusActionsMapping on b.StatusActionsMappingId equals actm.StatusActionsMappingId
            //                join d in _dbContext.mStatus on actm.StatusId equals d.StatusId
            //                join k in _dbContext.mActions on actm.ActionsId equals k.ActionsId
            //                join c in _dbContext.tbl_mUnitBranch on b.ToUnitId equals c.unitid into cs
            //                from toUnit in cs.DefaultIfEmpty()
            //                join g in _dbContext.tbl_mUnitBranch on b.FromUnitId equals g.unitid into cg
            //                from fromUnits in cg.DefaultIfEmpty()
            //                join j in _dbContext.mStages on d.StageId equals j.StagesId into js
            //                from eWithStages in js.DefaultIfEmpty()
            //                join f in _dbContext.Comment on b.PsmId equals f.PsmId into fs
            //                from eWithComment in fs.DefaultIfEmpty()
            //                where a.IsActive && !a.IsDeleted && b.IsActive && !b.IsDeleted && a.IsSubmited == true && b.IsComplete == false
            //                && b.ToUnitId == Logins.unitid && b.IsComment == false
            //                orderby b.TimeStamp descending
            //                select new DTOProjectsFwd
            //                {
            //                    ProjId = a.ProjId,
            //                    PsmIds = b.PsmId,
            //                    ProjName = a.ProjName,
            //                    StakeHolderId = a.StakeHolderId,
            //                    StakeHolder = stackcs.UnitName,
            //                    //Remarks= b != null ? b.Remarks : null,
            //                    Status = d.Status,
            //                    StatusId = d.StatusId,
            //                    FromUnitId = b.FromUnitId,
            //                    ToUnitId = b.ToUnitId,
            //                    ToUnitName = toUnit.UnitName,
            //                    Action = k.Actions,
            //                    TotalDays = 0,
            //                    ActionId = actm.ActionsId,
            //                    Sponsor = a.Sponsor,
            //                    //tooltip start
            //                    UnitName = _psmRepository.GetSponsorUnitName(a.StakeHolderId),
            //                    FromUnitUserDetail = fromUnits.UnitName,
            //                    FromUnitName = fromUnits.UnitName + " ( " + b.UserDetails + ")",
            //                    //end
            //                    Stage = eWithStages.Stages,
            //                    StageId = eWithStages.StagesId,
            //                    EncyID = _dataProtector.Protect(a.ProjId.ToString()),
            //                    EncyPsmID = _dataProtector.Protect(b.PsmId.ToString()),
            //                    IsProcess = a.IsProcess,
            //                    IsRead = b.IsRead,
            //                    //IsCommentRead =a.IsComment,
            //                    TimeStamp = b.TimeStamp,


            //                    //Date_type = a.Date_type,
            //                    //AdminApprovalStatus = adminApproval != null && adminApproval.DDGIT_approval == true,
            //                    //UserRequest = _dbContext.DateApproval.Any(x => x.ProjId == a.ProjId && (x.DDGIT_approval == false || x.DDGIT_approval == null)),
            //                    //RequestUnitId = _dbContext.DateApproval.Where(x => x.ProjId == a.ProjId && (x.DDGIT_approval == false || x.DDGIT_approval == null))
            //                    //    .OrderByDescending(x => x.Request_Date)
            //                    //    .Select(x => x.UnitId)
            //                    //    .FirstOrDefault()
            //                };

            //    //var projectsWithDetails = await query.OrderByDescending(x => x.PsmIds).ToListAsync();
            //    var projectsWithDetails = query.ToList();
            //    return projectsWithDetails;
            //}
            //else
            //{
            //    return null;
            //}
            #endregion

            #region GetActInboxAsyncWithProc

            try
            {
                var result = new List<DTOProjectsFwd>();
                var login = SessionHelper.GetObjectFromJson<Login>(_httpContextAccessor.HttpContext.Session, "User");
                if (login == null)
                    return result;

                var unitId = login.unitid ?? 0;

                using (var connection = _dbContext.Database.GetDbConnection())
                {
                    await connection.OpenAsync();

                    using var command = connection.CreateCommand();
                    command.CommandText = "usp_GetActInbox";
                    command.CommandType = CommandType.StoredProcedure;

                    var param = command.CreateParameter();
                    param.ParameterName = "@UnitId";
                    param.Value = unitId;
                    command.Parameters.Add(param);

                    using var reader = await command.ExecuteReaderAsync();

                    while (await reader.ReadAsync())
                    {
                        var projId = reader.GetInt32(reader.GetOrdinal("ProjId"));
                        var psmId = reader.GetInt32(reader.GetOrdinal("PsmIds"));
                        var stakeholderId = reader.GetInt32(reader.GetOrdinal("StakeHolderId"));

                        result.Add(new DTOProjectsFwd
                        {
                            ProjId = projId,
                            PsmIds = psmId,
                            ProjName = reader["ProjName"]?.ToString(),
                            StakeHolderId = stakeholderId,
                            StakeHolder = reader["StakeHolder"]?.ToString(),
                            Status = reader["Status"]?.ToString(),
                            StatusId = reader.GetInt32(reader.GetOrdinal("StatusId")),
                            FromUnitId = reader.GetInt32(reader.GetOrdinal("FromUnitId")),
                            ToUnitId = reader.GetInt32(reader.GetOrdinal("ToUnitId")),
                            ToUnitName = reader["ToUnitName"]?.ToString(),
                            Action = reader["Action"]?.ToString(),
                            TotalDays = 0,
                            ActionId = reader.GetInt32(reader.GetOrdinal("ActionId")),
                            Sponsor = reader["Sponsor"]?.ToString(),
                            Stage = reader["Stage"]?.ToString(),
                            StageId = reader.GetInt32(reader.GetOrdinal("StageId")),
                            IsRead = reader.GetBoolean(reader.GetOrdinal("IsRead")),
                            IsProcess = reader.GetBoolean(reader.GetOrdinal("IsProcess")),
                            TimeStamp = reader.IsDBNull(reader.GetOrdinal("TimeStamp")) ? null : reader.GetDateTime(reader.GetOrdinal("TimeStamp")),
                            UnitName = _psmRepository.GetSponsorUnitName(stakeholderId),
                            FromUnitUserDetail = reader["FromUnitUserDetail"]?.ToString(),
                            FromUnitName = reader["FromUnitName"]?.ToString(),
                            EncyID = _dataProtector.Protect(projId.ToString()),
                            EncyPsmID = _dataProtector.Protect(psmId.ToString()),

                            Date_type = (int)reader["Date_type"],
                            AdminApprovalStatus = reader.GetInt32(reader.GetOrdinal("AdminApprovalStatus")) == 1,
                            UserRequest = reader.GetInt32(reader.GetOrdinal("UserRequest")) == 1,
                            //reader.GetBoolean(reader.GetOrdinal("UserRequest")),
                            RequestUnitId = reader.IsDBNull(reader.GetOrdinal("RequestUnitId")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("RequestUnitId"))
                        });
                    }

                    return result.OrderByDescending(x => x.TimeStamp).ToList();
                };
            }
            catch (Exception)
            {

                throw;
            }
            #endregion
        }

        public async Task<List<DTOProjectsFwd>> GetActSendItemsAsync()
        {
            #region GetActSendItemsWithLinq
            //Login Logins = SessionHelper.GetObjectFromJson<Login>(_httpContextAccessor.HttpContext.Session, "User");
            //List<DTOProjectsFwd> lst = new List<DTOProjectsFwd>();
            //if (Logins != null)
            //{
            //    int stkholder = Logins.unitid.HasValue ? Logins.unitid.Value : 0;

            //    string username = Logins.UserName;
            //    var query = from a in _dbContext.Projects
            //                join b in _dbContext.ProjStakeHolderMov on a.ProjId equals b.ProjId
            //                join stackc in _dbContext.tbl_mUnitBranch on a.StakeHolderId equals stackc.unitid into cs1
            //                from stackcs in cs1.DefaultIfEmpty()
            //                join actm in _dbContext.TrnStatusActionsMapping on b.StatusActionsMappingId equals actm.StatusActionsMappingId
            //                join d in _dbContext.mStatus on actm.StatusId equals d.StatusId into ds
            //                from eWithStatus in ds.DefaultIfEmpty()
            //                join c in _dbContext.tbl_mUnitBranch on b.ToUnitId equals c.unitid into cs
            //                from toUnit in cs.DefaultIfEmpty()

            //                join g in _dbContext.tbl_mUnitBranch on b.FromUnitId equals g.unitid into cg
            //                from fromUnits in cg.DefaultIfEmpty()

            //                join j in _dbContext.mStages on eWithStatus.StageId equals j.StagesId into js
            //                from eWithStages in js.DefaultIfEmpty()
            //                join k in _dbContext.mActions on actm.ActionsId equals k.ActionsId into ks
            //                from eWithAction in ks.DefaultIfEmpty()

            //                join f in _dbContext.Comment on b.PsmId equals f.PsmId into fs
            //                from eWithComment in fs.DefaultIfEmpty()

            //                    //where a.IsActive && !a.IsDeleted && b.IsActive && !b.IsDeleted && a.IsSubmited == true && b.IsComplete == false
            //                    //&& b.FromUnitId == Logins.unitid && b.IsComment == false/* && b.StatusId != 5*/
            //                    ////&& b.UndoRemarks == null // Added here IsPullBack
            //                where a.IsActive && !a.IsDeleted && b.IsActive && !b.IsDeleted && a.IsSubmited == true && b.IsComplete == false
            //               && b.FromUnitId == Logins.unitid && b.IsComment == false/* && b.StatusId != 5*/
            //                //&& b.UndoRemarks == null // Added here IsPullBack

            //                orderby b.PsmId descending

            //                select new DTOProjectsFwd
            //                {
            //                    ProjId = a.ProjId,
            //                    PsmIds = b.PsmId,
            //                    ProjName = a.ProjName,
            //                    StakeHolderId = a.StakeHolderId,
            //                    StakeHolder = stackcs.UnitName,
            //                    //Remarks = b != null ? b.Remarks : null,

            //                    Stage = eWithStages.Stages,

            //                    Sponsor = a.Sponsor,

            //                    //tooltip start
            //                    UnitName = _psmRepository.GetSponsorUnitName(a.StakeHolderId),
            //                    FromUnitUserDetail = fromUnits.UnitName,
            //                    FromUnitName = fromUnits.UnitName + " ( " + b.UserDetails + ")",
            //                    //end

            //                    Status = eWithStatus.Status,
            //                    FromUnitId = b.FromUnitId,

            //                    ToUnitId = b.ToUnitId,
            //                    ToUnitName = toUnit.UnitName,
            //                    Action = eWithAction.Actions,
            //                    ActionId = eWithAction.ActionsId,
            //                    TotalDays = 0,
            //                    EncyID = _dataProtector.Protect(a.ProjId.ToString()),
            //                    EncyPsmID = _dataProtector.Protect(b.PsmId.ToString()),
            //                    IsProcess = a.IsProcess,
            //                    // undopsmId = _psmRepository.GetLastRecProjectMov(a.ProjId),
            //                    StageId = eWithStages.StagesId,
            //                    TimeStamp = b.TimeStamp,
            //                    IsComplete = b.IsComplete,
            //                    IsRead = b.IsRead,
            //                    IsPullBack = b.IsPullBack,
            //                    PullbackAction = _dbContext.ProjStakeHolderMov
            //                    .Where(p => p.ProjId == b.ProjId && !p.IsComment)
            //                    .OrderByDescending(p => p.PsmId)
            //                    .FirstOrDefault() != null  // 1. Check if any movement exists
            //                  && _dbContext.ProjStakeHolderMov
            //                        .Where(p => p.ProjId == b.ProjId && !p.IsComment)
            //                        .OrderByDescending(p => p.PsmId)
            //                        .FirstOrDefault().ToUnitId != Logins.unitid   // 2. Check if latest ToUnitId != current user
            //                    && !_dbContext.ProjStakeHolderMov
            //                        .Any(p => p.ProjId == b.ProjId && !p.IsComment && !p.IsComplete && p.ToUnitId == Logins.unitid) // 3. Make sure no incomplete action is assigned to current user

            //                };

            //    var projectsWithDetails = await query.ToListAsync();
            //    lst = projectsWithDetails;

            //    var queryhist = from a in _dbContext.Projects
            //                    join b in _dbContext.ProjStakeHolderMov on a.ProjId equals b.ProjId

            //                    join stackc in _dbContext.tbl_mUnitBranch on a.StakeHolderId equals stackc.unitid
            //                    join actm in _dbContext.TrnStatusActionsMapping on b.StatusActionsMappingId equals actm.StatusActionsMappingId
            //                    join d in _dbContext.mStatus on actm.StatusId equals d.StatusId into ds
            //                    from eWithStatus in ds.DefaultIfEmpty()
            //                    join c in _dbContext.tbl_mUnitBranch on b.ToUnitId equals c.unitid into cs
            //                    from toUnit in cs.DefaultIfEmpty()

            //                    join g in _dbContext.tbl_mUnitBranch on b.FromUnitId equals g.unitid into cg
            //                    from fromUnits in cg.DefaultIfEmpty()

            //                    join j in _dbContext.mStages on eWithStatus.StageId equals j.StagesId into js
            //                    from eWithStages in js.DefaultIfEmpty()
            //                    join k in _dbContext.mActions on actm.ActionsId equals k.ActionsId into ks
            //                    from eWithAction in ks.DefaultIfEmpty()
            //                    join fromunit in _dbContext.tbl_mUnitBranch on b.FromUnitId equals fromunit.unitid
            //                    join f in _dbContext.Comment on b.PsmId equals f.PsmId into fs
            //                    from eWithComment in fs.DefaultIfEmpty()

            //                    where a.IsActive && !a.IsDeleted && b.IsActive && !b.IsDeleted && a.IsSubmited == true && b.IsComplete == true
            //                    //&& b.UndoRemarks == null
            //                    && b.FromUnitId == Logins.unitid && b.IsComment == false/* && b.StatusId != 5*/

            //                    orderby b.DateTimeOfUpdate descending

            //                    select new DTOProjectsFwd
            //                    {
            //                        ProjId = a.ProjId,
            //                        PsmIds = b.PsmId,
            //                        ProjName = a.ProjName,
            //                        Sponsor = a.Sponsor,

            //                        Stage = eWithStages.Stages,

            //                        UnitName = _psmRepository.GetSponsorUnitName(a.StakeHolderId),
            //                        FromUnitUserDetail = fromunit.UnitName,
            //                        FromUnitName = " " + fromunit.UnitName + " ( " + b.UserDetails + ")",


            //                        StakeHolderId = a.StakeHolderId,
            //                        StakeHolder = stackc.UnitName,
            //                        //Remarks = b != null ? b.Remarks : null,
            //                        Status = eWithStatus.Status,
            //                        FromUnitId = b.FromUnitId,
            //                        //FromUnitName = fromUnits.UnitName + " (" + b.UserDetails + ")",
            //                        ToUnitId = b.ToUnitId,
            //                        ToUnitName = toUnit.UnitName,
            //                        Action = eWithAction.Actions,
            //                        ActionId = eWithAction.ActionsId,
            //                        TotalDays = 0,
            //                        EncyID = _dataProtector.Protect(a.ProjId.ToString()),
            //                        EncyPsmID = _dataProtector.Protect(b.PsmId.ToString()),
            //                        IsProcess = a.IsProcess,
            //                        undopsmId = 0,
            //                        StageId = eWithStages.StagesId,
            //                        TimeStamp = b.TimeStamp,
            //                        IsComplete = b.IsComplete,
            //                        IsRead = b.IsRead,
            //                        IsPullBack = b.IsPullBack,
            //                        PullbackAction = _dbContext.ProjStakeHolderMov
            //                    .Where(p => p.ProjId == b.ProjId && !p.IsComment)
            //                    .OrderByDescending(p => p.PsmId)
            //                    .FirstOrDefault() != null  // 1. Check if any movement exists
            //                  && _dbContext.ProjStakeHolderMov
            //                        .Where(p => p.ProjId == b.ProjId && !p.IsComment)
            //                        .OrderByDescending(p => p.PsmId)
            //                        .FirstOrDefault().ToUnitId != Logins.unitid   // 2. Check if latest ToUnitId != current user
            //                    && !_dbContext.ProjStakeHolderMov
            //                        .Any(p => p.ProjId == b.ProjId && !p.IsComment && !p.IsComplete && p.ToUnitId == Logins.unitid) // 3. Make sure no incomplete action is assigned to current user

            //                    };

            //    var history = await queryhist.ToListAsync();
            //    lst.AddRange(history);

            //    return lst;
            //}
            //else
            //{
            //    return null;
            //}
            #endregion

            #region GetActSendItemsWithProc

            Login Logins = SessionHelper.GetObjectFromJson<Login>(_httpContextAccessor.HttpContext.Session, "User");
            if (Logins == null) return null;

            var lst = new List<DTOProjectsFwd>();
            int unitId = Logins.unitid ?? 0;

            using (SqlConnection conn = new SqlConnection(_dbContext.Database.GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand("usp_GetActSendItems", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@UnitId", unitId);

                    await conn.OpenAsync();
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var dto = new DTOProjectsFwd
                            {
                                ProjId = reader.GetInt32(reader.GetOrdinal("ProjId")),
                                PsmIds = reader.GetInt32(reader.GetOrdinal("PsmIds")),
                                ProjName = reader["ProjName"]?.ToString(),
                                Sponsor = reader["Sponsor"]?.ToString(),
                                StakeHolderId = reader.GetInt32(reader.GetOrdinal("StakeHolderId")),
                                StakeHolder = reader["StakeHolder"]?.ToString(),
                                Stage = reader["Stages"]?.ToString(),
                                StageId = reader.GetInt32(reader.GetOrdinal("StageId")),
                                Status = reader["Status"]?.ToString(),
                                StatusId = reader["StatusId"] != DBNull.Value ? Convert.ToInt32(reader["StatusId"]) : 0,
                                FromUnitId = reader.GetInt32(reader.GetOrdinal("FromUnitId")),
                                FromUnitUserDetail = reader["FromUnitUserDetail"]?.ToString(),
                                FromUnitName = reader["FromUnitName"]?.ToString(),
                                ToUnitId = reader.GetInt32(reader.GetOrdinal("ToUnitId")),
                                ToUnitName = reader["ToUnitName"]?.ToString(),
                                Action = reader["Actions"]?.ToString(),
                                ActionId = reader.GetInt32(reader.GetOrdinal("ActionId")),
                                TotalDays = 0,
                                TimeStamp = reader["TimeStamp"] != DBNull.Value ? Convert.ToDateTime(reader["TimeStamp"]) : DateTime.MinValue,
                                IsProcess = reader["IsProcess"] != DBNull.Value && Convert.ToBoolean(reader["IsProcess"]),
                                IsRead = reader["IsRead"] != DBNull.Value && Convert.ToBoolean(reader["IsRead"]),
                                IsComplete = reader["IsComplete"] != DBNull.Value && Convert.ToBoolean(reader["IsComplete"]),
                                IsPullBack = reader["IsPullBack"] != DBNull.Value && Convert.ToBoolean(reader["IsPullBack"]),
                                UnitName = _psmRepository.GetSponsorUnitName(Convert.ToInt32(reader["StakeHolderId"])),
                                EncyID = _dataProtector.Protect(reader["ProjId"].ToString()),
                                EncyPsmID = _dataProtector.Protect(reader["PsmIds"].ToString())
                            };

                            lst.Add(dto);
                        }
                    }
                }
            }

            return lst;

            #endregion

        }


        public async Task<List<tbl_Projects>> GetAllProjectsAsync()
        {
            return await _dbContext.Projects.ToListAsync();
        }


        public async Task<List<tbl_Projects>> GetMyProjectsAsync()
        {
            Login Logins = SessionHelper.GetObjectFromJson<Login>(_httpContextAccessor.HttpContext.Session, "User");

            if (Logins != null)
            {

                int stkholder = Logins.unitid.HasValue ? Logins.unitid.Value : 0;

                string username = Logins.UserName;

                var querys = from a in _dbContext.ProjStakeHolderMov
                             join b in _dbContext.Projects on a.PsmId equals b.CurrentPslmId
                             join c in _dbContext.tbl_mUnitBranch on a.ToUnitId equals c.unitid into currentStakeHolder
                             from current in currentStakeHolder.DefaultIfEmpty()
                             join m in _DBContext.tbl_mUnitBranch on b.StakeHolderId equals m.unitid into stakehold
                             from stk in stakehold.DefaultIfEmpty()
                             join d in _dbContext.tbl_mUnitBranch on a.ToUnitId equals d.unitid into toStakeHolder
                             from to in toStakeHolder.DefaultIfEmpty()
                             join e in _dbContext.tbl_mUnitBranch on a.FromUnitId equals e.unitid into fromStakeHolder
                             from fromStake in fromStakeHolder.DefaultIfEmpty()
                             join f in _dbContext.Comment on a.PsmId equals f.PsmId into fs
                             from eWithComment in fs.DefaultIfEmpty()
                             join actm in _dbContext.TrnStatusActionsMapping on a.StatusActionsMappingId equals actm.StatusActionsMappingId
                             join h in _dbContext.mStatus on actm.StatusId equals h.StatusId into hs
                             from eWithStatus in hs.DefaultIfEmpty()
                             join j in _dbContext.mStages on eWithStatus.StageId equals j.StagesId into js
                             from eWithStages in js.DefaultIfEmpty()
                             join k in _dbContext.mActions on actm.ActionsId equals k.ActionsId into ks
                             from eWithAction in ks.DefaultIfEmpty()
                             where b.StakeHolderId == Logins.unitid
                             // && a.TostackholderDt !=null

                             select new tbl_Projects

                             {
                                 ProjId = b.ProjId,
                                 ProjName = b.ProjName,
                                 StakeHolderId = b.StakeHolderId,

                                 CurrentPslmId = b.CurrentPslmId,
                                 InitiatedDate = b.InitiatedDate,
                                 CompletionDate = b.CompletionDate,
                                 IsWhitelisted = b.IsWhitelisted,
                                 InitialRemark = b.InitialRemark,
                                 EditDeleteBy = a.EditDeleteBy,
                                 EditDeleteDate = a.EditDeleteDate,
                                 UpdatedByUserId = a.UpdatedByUserId,
                                 DateTimeOfUpdate = a.DateTimeOfUpdate,
                                 ProjCode = b.ProjCode,
                                 RecdFmUser = fromStake != null ? fromStake.UnitName : null,
                                 FwdtoUser = current != null ? current.UnitName : null,

                                 FwdBy = fromStake != null ? fromStake.UnitName : null,

                                 AdRemarks = a.Remarks,
                                 Comments = eWithComment.Comment,
                                 FwdtoDate = a.TimeStamp,
                                 Status = eWithStatus.Status,
                                 Stages = eWithStages.Stages,
                                 StakeHolder = stk.UnitName,
                                 Action = eWithAction.Actions,
                                 TotalDays = EF.Functions.DateDiffDay(a.EditDeleteDate, a.TimeStamp) ?? 0,
                                 //ActionCde = a.ActionCde,
                                 AttCnt = _dbContext.AttHistory.Count(f => f.PsmId == b.CurrentPslmId),
                                 HostTypeID = b.HostTypeID,
                                 EncyID = _dataProtector.Protect(b.CurrentPslmId.ToString()),
                                 ActionId = actm.ActionsId
                             };

                var projectsWithDetails = await querys.ToListAsync();

                return projectsWithDetails;
            }
            else
            {
                return null;
            }
        }

        // Retrive data from only Project table with stored procedure 
        //public async Task<List<AddNewProject>> GetMyProjects()
        //{
        //    Login Logins = SessionHelper.GetObjectFromJson<Login>(_httpContextAccessor.HttpContext.Session, "User");

        //    if (Logins != null)
        //    {
        //        int stkholder = Logins.unitid.HasValue ? Logins.unitid.Value : 0;
        //        string username = Logins.UserName?? "NA";
        //        var querys = from proj in _dbContext.Projects 
        //                     where proj.StakeHolderId == Logins.unitid

        //                     select new AddNewProject
        //                     {
        //                         ProjId = proj.ProjId,
        //                         ProjName = proj.ProjName,
        //                         StakeHolderId = proj.StakeHolderId,
        //                         InitiatedDate = proj.InitiatedDate,
        //                         HostType = _dbContext.mHostType.FirstOrDefault(x => x.HostTypeID == proj.HostTypeID).HostingDesc,
        //                         Apptype = _dbContext.mAppType.FirstOrDefault(x => x.Apptype == proj.Apptype).AppDesc,
        //                         TypeofSW = proj.TypeofSW,
        //                         BeingDevpInhouse = proj.BeingDevpInhouse
        //                     };

        //        var projectsWithDetails = await querys.ToListAsync();

        //        return projectsWithDetails;
        //    }
        //    else
        //    {
        //        return null;
        //    }
        //}

        public async Task<List<AddNewProject>> GetMyProjects()
        {
            Login Logins = SessionHelper.GetObjectFromJson<Login>(_httpContextAccessor.HttpContext.Session, "User");

            if (Logins != null)
            {
                int stkholder = Logins.unitid.HasValue ? Logins.unitid.Value : 0;

                var projects = await _dbContext.AddNewProjects
                    .FromSqlRaw("EXEC GetMyProjects @StakeHolderId", new SqlParameter("@StakeHolderId", stkholder))
                    .ToListAsync();

                foreach (var project in projects)
                {
                    project.EncyID = _dataProtector.Protect(project.CurrentPslmId.ToString());
                }

                return projects;
            }

            return null;
        }



        public async Task<tbl_Projects> GetProjectByIdAsync(int projectId)
        {
            return await _dbContext.Projects.FindAsync(projectId);
        }

        public async Task<tbl_Projects> GetProjectByIdAsync1(int? dataProjId)
        {
            return await _dbContext.Projects.FindAsync(dataProjId);
        }


        public async Task<List<ProjHistory>> GetProjectHistory(string userid)
        {
            Login Logins = SessionHelper.GetObjectFromJson<Login>(_httpContextAccessor.HttpContext.Session, "User");
            int stkholder = Logins.unitid.HasValue ? Logins.unitid.Value : 0;

            if (Logins != null || stkholder != 0)
            {
                try
                {
                    var query = from p in _dbContext.ProjStakeHolderMov
                                join actm in _dbContext.TrnStatusActionsMapping on p.StatusActionsMappingId equals actm.StatusActionsMappingId
                                join b in _dbContext.mStages on actm.StatusId equals b.StagesId into stageJoin
                                from b in stageJoin.DefaultIfEmpty()
                                join c in _dbContext.mStatus on actm.StatusId equals c.StatusId into statusJoin
                                from c in statusJoin.DefaultIfEmpty()
                                join l in _dbContext.mActions on actm.ActionsId equals l.ActionsId into ActionJoin
                                from l in ActionJoin.DefaultIfEmpty()
                                join d in _dbContext.Comment on p.PsmId equals d.PsmId into commentJoin
                                from d in commentJoin.DefaultIfEmpty()
                                join proj in _dbContext.Projects on p.ProjId equals proj.ProjId into projectJoin
                                from proj in projectJoin.DefaultIfEmpty()
                                join fromSH in _dbContext.tbl_mUnitBranch on p.FromUnitId equals fromSH.unitid into fromStakeHolderJoin
                                from fromSH in fromStakeHolderJoin.DefaultIfEmpty()
                                join toSH in _dbContext.tbl_mUnitBranch on p.ToUnitId equals toSH.unitid into toStakeHolderJoin
                                from toSH in toStakeHolderJoin.DefaultIfEmpty()
                                join curSH in _dbContext.tbl_mUnitBranch on p.PsmId equals curSH.unitid into currentStakeHolderJoin
                                from curSH in currentStakeHolderJoin.DefaultIfEmpty()
                                join stakeHolderSH in _dbContext.tbl_mUnitBranch on p.FromUnitId equals stakeHolderSH.unitid into stakeHolderJoin
                                from stakeHolderSH in stakeHolderJoin.DefaultIfEmpty()
                                where proj.ProjName.Length > 1
                                orderby p.ProjId, p.PsmId
                                select new ProjHistory
                                {
                                    ProjId = proj.ProjId,
                                    PsmId = p.PsmId,
                                    ProjName = proj.ProjName,
                                    Stages = b.Stages,
                                    Status = c.Status,
                                    Comment = d.Comment,
                                    FromStakeHolder = fromSH.UnitName,
                                    ToStakeHolder = toSH.UnitName,
                                    CurrentStakeHolder = curSH.UnitName,
                                    InitiatedBy = stakeHolderSH.UnitName,
                                    TimeStamp = p.TimeStamp.HasValue ? p.TimeStamp.Value.ToString("dd-MM-yyyy") : "",
                                    InitialRemarks = p.Remarks,
                                    AttCnt = _dbContext.AttHistory.Count(f => f.PsmId == p.PsmId),
                                    ActionName = l.Actions

                                };

                    var result = query.ToList();

                    var projHistory = await query.ToListAsync();
                    return projHistory;

                }
                catch (Exception ex)
                {

                    return null;
                }
            }
            else
            {
                return null;
            }
        }



        public async Task<List<ProjHistory>> GetProjectHistorybyID(int? dtaProjID)
        {
            Login Logins = SessionHelper.GetObjectFromJson<Login>(_httpContextAccessor.HttpContext.Session, "User");

            int stkholder = Logins.unitid.HasValue ? Logins.unitid.Value : 0;

            if (Logins != null || stkholder != 0)
            {
                try
                {
                    var query = from p in _dbContext.ProjStakeHolderMov
                                join actm in _dbContext.TrnStatusActionsMapping on p.StatusActionsMappingId equals actm.StatusActionsMappingId
                                join c in _dbContext.mStatus on actm.StatusId equals c.StatusId into statusJoin
                                from c in statusJoin.DefaultIfEmpty()
                                join b in _dbContext.mStages on c.StageId equals b.StagesId into stageJoin
                                from b in stageJoin.DefaultIfEmpty()
                                join l in _dbContext.mActions on actm.ActionsId equals l.ActionsId into ActionJoin
                                from l in ActionJoin.DefaultIfEmpty()
                                join d in _dbContext.Comment on p.PsmId equals d.PsmId into commentJoin
                                from d in commentJoin.DefaultIfEmpty()
                                join proj in _dbContext.Projects on p.ProjId equals proj.ProjId into projectJoin
                                from proj in projectJoin.DefaultIfEmpty()
                                join fromSH in _dbContext.tbl_mUnitBranch on p.FromUnitId equals fromSH.unitid into fromStakeHolderJoin
                                from fromSH in fromStakeHolderJoin.DefaultIfEmpty()

                                join h in _DBContext.mHostType on proj.HostTypeID equals h.HostTypeID into hs
                                from hostType in hs.DefaultIfEmpty()

                                join i in _DBContext.mAppType on proj.Apptype equals i.Apptype into ms
                                from eWithAppType in ms.DefaultIfEmpty()

                                join toSH in _dbContext.tbl_mUnitBranch on p.ToUnitId equals toSH.unitid into toStakeHolderJoin
                                from toSH in toStakeHolderJoin.DefaultIfEmpty()
                                join curSH in _dbContext.tbl_mUnitBranch on proj.StakeHolderId equals curSH.unitid into currentStakeHolderJoin
                                from curSH in currentStakeHolderJoin.DefaultIfEmpty()

                                    // join stakeHolderSH in _dbContext.tbl_mUnitBranch on p.CurrentStakeHolderId equals stakeHolderSH.unitid into stakeHolderJoin
                                    //from stakeHolderSH in stakeHolderJoin.DefaultIfEmpty()
                                where proj.ProjName.Length > 1 && proj.ProjId == dtaProjID
                                orderby p.ProjId, p.PsmId
                                select new ProjHistory
                                {
                                    ProjId = proj.ProjId,
                                    PsmId = p.PsmId,
                                    ProjName = proj.ProjName,
                                    Stages = b.Stages,
                                    Status = c.Status,
                                    Comment = d.Comment,
                                    FromStakeHolder = fromSH.UnitName,
                                    ToStakeHolder = toSH.UnitName,
                                    // CurrentStakeHolder = stakeHolderSH.UnitName,
                                    InitiatedBy = curSH.UnitName,
                                    TimeStamp = p.TimeStamp.HasValue ? p.TimeStamp.Value.ToString("dd-MM-yyyy") : "",
                                    InitialRemarks = proj.AdRemarks,
                                    Remarks = p.Remarks,  //  work
                                    AttCnt = _dbContext.AttHistory.Count(f => f.PsmId == p.PsmId),
                                    ActionName = l.Actions,
                                    AppDesc = eWithAppType.AppDesc,
                                    HostedOn = hostType.HostingDesc
                                };


                    var result = await query.ToListAsync();

                    return result;
                }

                catch (Exception ex)
                {
                    return null;
                }

            }
            else
            {
                return null;
            }
        }
        public async Task<tbl_Projects> GetProjectByPsmIdAsync(int psmId)
        {
            return await _dbContext.Projects.FirstOrDefaultAsync(a => a.CurrentPslmId == psmId);

        }

        public Task<List<tbl_Projects>> GetStatusProjAsync(int statusid)
        {
            throw new NotImplementedException();
        }

        public async Task<tbl_ProjStakeHolderMov> GettXNByPsmIdAsync(int psmId)
        {
            return await _dbContext.ProjStakeHolderMov
               .FirstOrDefaultAsync(a => a.PsmId == psmId);
        }
        public async Task<tbl_ProjStakeHolderMov> GettXNByPsmIdwithUnitId(int psmId, int UnitID)
        {
            return await _dbContext.ProjStakeHolderMov
               .FirstOrDefaultAsync(a => a.PsmId == psmId && a.ToUnitId == UnitID);
        }
        public async Task<bool> UpdateProjectAsync(tbl_Projects project)
        {
            Login Logins = SessionHelper.GetObjectFromJson<Login>(_httpContextAccessor.HttpContext.Session, "User");
            if (Logins != null)
            {
                // tbl_ProjStakeHolderMov psmove = _dbContext.ProjStakeHolderMov.FirstOrDefault(x => x.PsmId == project.CurrentPslmId);
                _dbContext.Entry(project).State = EntityState.Modified;
                await _dbContext.SaveChangesAsync();
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> UpdateTxnAsync(tbl_ProjStakeHolderMov psmov)
        {
            _dbContext.ProjStakeHolderMov.Update(psmov);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<List<tbl_Projects>> GetActProjectsAsync()
        {
            Login Logins = SessionHelper.GetObjectFromJson<Login>(_httpContextAccessor.HttpContext.Session, "User");

            if (Logins != null && Logins.Role == "Unit")
            {
                string username = Logins.UserName;

                int stkholder = Logins.unitid.HasValue ? Logins.unitid.Value : 0;

                var projects = await (from a in _DBContext.Projects
                                      join b in _DBContext.ProjStakeHolderMov on a.CurrentPslmId equals b.PsmId into bs
                                      from e in bs.DefaultIfEmpty()
                                      join actm in _dbContext.TrnStatusActionsMapping on e.StatusActionsMappingId equals actm.StatusActionsMappingId
                                      join d in _DBContext.mStatus on actm.StatusId equals d.StatusId into ds
                                      from eWithStatus in ds.DefaultIfEmpty()
                                      join c in _DBContext.tbl_mUnitBranch on a.StakeHolderId equals c.unitid into cs
                                      from eWithUnit in cs.DefaultIfEmpty()

                                      join g in _DBContext.tbl_mUnitBranch on e.ToUnitId equals g.unitid into csh
                                      from curstk in csh.DefaultIfEmpty()

                                      join f in _DBContext.Comment on a.CurrentPslmId equals f.PsmId into fs
                                      from eWithComment in fs.DefaultIfEmpty()
                                      where a.IsActive && !a.IsDeleted && (a.StakeHolderId == stkholder || e.ToUnitId == stkholder || e.ToUnitId == stkholder || e.ToUnitId == stkholder)
                                      && actm.ActionsId > 4
                                      orderby e.TimeStamp descending
                                      select new tbl_Projects
                                      {
                                          ProjId = a.ProjId,
                                          ProjName = a.ProjName,
                                          StakeHolderId = a.StakeHolderId,
                                          CurrentPslmId = a.CurrentPslmId,
                                          InitiatedDate = a.InitiatedDate,
                                          CompletionDate = a.CompletionDate,
                                          IsWhitelisted = a.IsWhitelisted,
                                          InitialRemark = a.InitialRemark,
                                          IsDeleted = a.IsDeleted,
                                          IsActive = a.IsActive,
                                          EditDeleteBy = a.EditDeleteBy,
                                          EditDeleteDate = a.EditDeleteDate,
                                          UpdatedByUserId = a.UpdatedByUserId,
                                          DateTimeOfUpdate = e.DateTimeOfUpdate,
                                          //ToUnitId = a.ToUnitId,
                                          StakeHolder = eWithUnit.UnitName,
                                          FwdtoUser = curstk.UnitName,
                                          Status = eWithStatus.Status,
                                          Comments = eWithComment.Comment,
                                          // ActionCde = e.ActionCde,
                                          AimScope = a.AimScope,
                                          EncyID = _dataProtector.Protect(a.CurrentPslmId.ToString()),

                                          AttCnt = _dbContext.AttHistory.Count(f => f.PsmId == a.CurrentPslmId)
                                      }).ToListAsync();
                return projects;
            }
            else if (Logins != null && Logins.Role == "Dte")
            {
                string username = Logins.UserName;

                int stkholder = Logins.unitid.HasValue ? Logins.unitid.Value : 0;

                var projects = await (from a in _DBContext.Projects
                                      join b in _DBContext.ProjStakeHolderMov on a.CurrentPslmId equals b.PsmId into bs
                                      from e in bs.DefaultIfEmpty()
                                      join actm in _dbContext.TrnStatusActionsMapping on e.StatusActionsMappingId equals actm.StatusActionsMappingId
                                      join d in _DBContext.mStatus on actm.StatusId equals d.StatusId into ds
                                      from eWithStatus in ds.DefaultIfEmpty()
                                      join c in _DBContext.tbl_mUnitBranch on a.StakeHolderId equals c.unitid into cs
                                      from eWithUnit in cs.DefaultIfEmpty()

                                      join g in _DBContext.tbl_mUnitBranch on e.ToUnitId equals g.unitid into csh
                                      from curstk in csh.DefaultIfEmpty()

                                      join f in _DBContext.Comment on a.CurrentPslmId equals f.PsmId into fs
                                      from eWithComment in fs.DefaultIfEmpty()
                                      where a.IsActive && !a.IsDeleted
                                      // && e.ActionCde > 0
                                      orderby e.TimeStamp descending
                                      select new tbl_Projects
                                      {
                                          ProjId = a.ProjId,
                                          ProjName = a.ProjName,
                                          StakeHolderId = a.StakeHolderId,
                                          CurrentPslmId = a.CurrentPslmId,
                                          InitiatedDate = a.InitiatedDate,
                                          CompletionDate = a.CompletionDate,
                                          IsWhitelisted = a.IsWhitelisted,
                                          InitialRemark = a.InitialRemark,
                                          IsDeleted = a.IsDeleted,
                                          IsActive = a.IsActive,
                                          EditDeleteBy = a.EditDeleteBy,
                                          EditDeleteDate = a.EditDeleteDate,
                                          UpdatedByUserId = a.UpdatedByUserId,
                                          DateTimeOfUpdate = e.DateTimeOfUpdate,
                                          // ToUnitId = a.ToUnitId,
                                          StakeHolder = eWithUnit.UnitName,
                                          FwdtoUser = curstk.UnitName,
                                          Status = eWithStatus.Status,
                                          Comments = eWithComment.Comment,
                                          // ActionCde = e.ActionCde,
                                          AimScope = a.AimScope,
                                          EncyID = _dataProtector.Protect(a.CurrentPslmId.ToString()),
                                          AttCnt = _dbContext.AttHistory.Count(f => f.PsmId == a.CurrentPslmId)
                                      }).ToListAsync();

                return projects;
            }
            else
            {

                return null;
            }
        }
        public async Task<List<DToWhiteListeds>> GetWhiteListedActionProj()
        {
            try
            {

                var isAllow = _configuration.GetSection("AllowWhiteListedProjSync");
                if (isAllow.Value == "1")
                {
                    var connectionString = _configuration.GetConnectionString("DB");
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        using (SqlCommand cmd = new SqlCommand("InsertWhiteListedProjects", conn))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            conn.Open();
                            cmd.ExecuteNonQuery();
                        }
                    }
                }

                var results = await _dbContext.WhiteListedProjects.FromSqlRaw("EXEC GetLatestWhiteListedProjects").ToListAsync();
                return results;
            }
            catch (Exception)
            {

                throw;
            }
            //var currentDateMinus15Days = DateTime.Now.AddDays(-30);
            //var results = (from a in _dbContext.trnWhiteListed
            //               join b in _dbContext.mWhiteListedHeader on a.HeaderID equals b.Id
            //               orderby a.HeaderID descending
            //               select new DToWhiteListed
            //               {
            //                   Id = a.Id,
            //                   HeaderID = b.Id,
            //                   ProjName = a.ProjName,
            //                   Appt = a.Appt,
            //                   Fmn = a.Fmn,
            //                   ContactNo = a.ContactNo,
            //                   Clearence = a.Clearence == null ? string.Empty : ((DateTime)a.Clearence).ToString("dd/MM/yy"),
            //                   CertNo = a.CertNo,
            //                   date = a.date == null ? string.Empty : ((DateTime)a.date).ToString("dd/MM/yy"),
            //                   ValidUpto = a.ValidUpto == null ? string.Empty : ((DateTime)a.ValidUpto).ToString("dd/MM/yy"),
            //                   Remarks = a.Remarks,
            //                   Header = b.Header
            //               }).ToList();

            // This is the linq query I call a proc for this linq

            //var results = (from a in _dbContext.Projects
            //               join b in _dbContext.ProjStakeHolderMov on a.ProjId equals b.ProjId
            //               join u in _dbContext.Users on a.StakeHolderId equals u.unitid
            //               where b.StatusActionsMappingId == 88
            //               select new
            //               {
            //                   a,
            //                   b,
            //                   u,
            //                   ACG = _dbContext.ProjStakeHolderMov
            //                           .Where(x => x.ProjId == a.ProjId && x.StatusActionsMappingId == 78)
            //                           .OrderByDescending(x => x.DateTimeOfUpdate)
            //                           .Select(x => x.DateTimeOfUpdate)
            //                           .FirstOrDefault(), 
            //                    HostOn = _dbContext.mHostType.FirstOrDefault(x => x.HostTypeID == a.HostTypeID).HostingDesc
            //               })
            //     .AsEnumerable()
            //     .Select(x => new DToWhiteListed
            //     {
            //         Id = x.b.PsmId,
            //         ProjId = x.a.ProjId,
            //         CertNo = x.b.CertNo,
            //         ProjName = x.a.ProjName,
            //         HostedOn = x.HostOn,
            //         Sponser = x.a.Sponsor,
            //         ContactNo = x.u.Tele_Army,
            //         ACGClearence = x.ACG.HasValue ? x.ACG.Value.ToString("dd/MM/yy") : "",
            //         ValidUpto = x.ACG.HasValue ? x.ACG.Value.AddYears(3).ToString("dd/MM/yy") : "", 
            //         Remarks = x.a.InitialRemark
            //     })
            //     .ToList();
            //return (List<DToWhiteListed>)results.GroupBy(x => x.ProjName).First();



            // 'results' contains the list of TimeExceeds objects with the specified conditions.
            // Actions = "YourActionsValue", 
            //TimeLimit = b.TimeLimit, // Assuming TimeLimit is a property in ProjStakeHolderMov
            //                   dayss = 15, // You can set this to 15 as it is a constant value
            //                   exceeds = (b.TimeLimit - 15) // Calculation for exceeds
        }
        public async Task<List<tbl_Projects>> GetProcProjAsync()
        {
            bool? cmtreqd = false;

            Login Logins = SessionHelper.GetObjectFromJson<Login>(_httpContextAccessor.HttpContext.Session, "User");
            if (Logins != null)
            {

                cmtreqd = await _DBContext.tbl_mUnitBranch
                 .Where(a => a.unitid == Logins.unitid)
                 .Select(b => b.commentreqdid)
                 .FirstOrDefaultAsync();

            }

            if (Logins != null && Logins.Role == "Unit")
            {
                string username = Logins.UserName;

                int stkholder = Logins.unitid.HasValue ? Logins.unitid.Value : 0;

                //   change the query

                var projects = await (from a in _DBContext.Projects
                                      join b in _DBContext.ProjStakeHolderMov on a.CurrentPslmId equals b.PsmId into bs
                                      from e in bs.DefaultIfEmpty()
                                      join actm in _dbContext.TrnStatusActionsMapping on e.StatusActionsMappingId equals actm.StatusActionsMappingId
                                      join d in _DBContext.mStatus on actm.StatusId equals d.StatusId into ds
                                      from eWithStatus in ds.DefaultIfEmpty()
                                      join c in _DBContext.tbl_mUnitBranch on a.StakeHolderId equals c.unitid into cs
                                      from eWithUnit in cs.DefaultIfEmpty()
                                      join h in _DBContext.mHostType on a.HostTypeID equals h.HostTypeID into hs
                                      from hostType in hs.DefaultIfEmpty()
                                      join i in _DBContext.mAppType on a.Apptype equals i.Apptype into ms
                                      from eWithAppType in ms.DefaultIfEmpty()
                                      join g in _DBContext.tbl_mUnitBranch on e.ToUnitId equals g.unitid into csh
                                      from curstk in csh.DefaultIfEmpty()

                                      join f in _DBContext.Comment on a.CurrentPslmId equals f.PsmId into fs
                                      from eWithComment in fs.DefaultIfEmpty()
                                      where a.IsActive && !a.IsDeleted && (a.StakeHolderId == stkholder || e.FromUnitId == stkholder || e.ToUnitId == stkholder || e.ToUnitId == stkholder)
                                     && actm.StatusId == 4
                                      orderby e.TimeStamp descending
                                      select new tbl_Projects
                                      {
                                          ProjId = a.ProjId,
                                          ProjName = a.ProjName,
                                          StakeHolderId = a.StakeHolderId,
                                          CurrentPslmId = a.CurrentPslmId,
                                          InitiatedDate = a.InitiatedDate,
                                          CompletionDate = a.CompletionDate,
                                          IsWhitelisted = a.IsWhitelisted,
                                          InitialRemark = a.InitialRemark,
                                          IsDeleted = a.IsDeleted,
                                          IsActive = a.IsActive,
                                          EditDeleteBy = a.EditDeleteBy,
                                          EditDeleteDate = a.EditDeleteDate,
                                          UpdatedByUserId = a.UpdatedByUserId,
                                          DateTimeOfUpdate = e.DateTimeOfUpdate,
                                          //ToUnitId = a.ToUnitId,
                                          StakeHolder = eWithUnit.UnitName,
                                          FwdtoUser = curstk.UnitName,
                                          Status = eWithStatus.Status,
                                          Comments = eWithComment.Comment,
                                          //ActionCde = e.ActionCde,
                                          AimScope = a.AimScope,
                                          ReqmtJustification = a.ReqmtJustification,
                                          Deplytype = eWithAppType.AppDesc,
                                          Hostedon = hostType.HostingDesc,
                                          EncyID = _dataProtector.Protect(a.CurrentPslmId.ToString()),

                                          AttCnt = _dbContext.AttHistory.Count(f => f.PsmId == a.CurrentPslmId)
                                      }).ToListAsync();

                return projects;

            }

            else if (Logins != null && Logins.Role == "Dte" && cmtreqd == true)

            {
                string username = Logins.UserName;

                int stkholder = Logins.unitid.HasValue ? Logins.unitid.Value : 0;

                var projects = await (from a in _DBContext.Projects
                                      join b in _DBContext.ProjStakeHolderMov on a.CurrentPslmId equals b.PsmId into bs
                                      from e in bs.DefaultIfEmpty()
                                      join actm in _dbContext.TrnStatusActionsMapping on e.StatusActionsMappingId equals actm.StatusActionsMappingId
                                      join d in _DBContext.mStatus on actm.StatusId equals d.StatusId into ds
                                      from eWithStatus in ds.DefaultIfEmpty()
                                      join c in _DBContext.tbl_mUnitBranch on a.StakeHolderId equals c.unitid into cs
                                      from eWithUnit in cs.DefaultIfEmpty()
                                      join h in _DBContext.mHostType on a.HostTypeID equals h.HostTypeID into hs
                                      from hostType in hs.DefaultIfEmpty()

                                      join i in _DBContext.mAppType on a.Apptype equals i.Apptype into ms
                                      from eWithAppType in ms.DefaultIfEmpty()


                                      join g in _DBContext.tbl_mUnitBranch on e.ToUnitId equals g.unitid into csh
                                      from curstk in csh.DefaultIfEmpty()

                                      join f in _DBContext.Comment on a.CurrentPslmId equals f.PsmId into fs
                                      from eWithComment in fs.DefaultIfEmpty()
                                      where a.IsActive && !a.IsDeleted
                                      //&& e.ActionCde > 0 
                                      && actm.StatusId == 4
                                      orderby e.TimeStamp descending
                                      select new tbl_Projects
                                      {
                                          ProjId = a.ProjId,
                                          ProjName = a.ProjName,
                                          StakeHolderId = a.StakeHolderId,
                                          CurrentPslmId = a.CurrentPslmId,
                                          InitiatedDate = a.InitiatedDate,
                                          CompletionDate = a.CompletionDate,
                                          IsWhitelisted = a.IsWhitelisted,
                                          InitialRemark = a.InitialRemark,
                                          IsDeleted = a.IsDeleted,
                                          IsActive = a.IsActive,
                                          EditDeleteBy = a.EditDeleteBy,
                                          EditDeleteDate = a.EditDeleteDate,
                                          UpdatedByUserId = a.UpdatedByUserId,
                                          DateTimeOfUpdate = e.DateTimeOfUpdate,
                                          //ToUnitId = a.ToUnitId,
                                          StakeHolder = eWithUnit.UnitName,
                                          FwdtoUser = curstk.UnitName,
                                          Status = eWithStatus.Status,
                                          Comments = eWithComment.Comment,
                                          //ActionCde = e.ActionCde,
                                          AimScope = a.AimScope,
                                          ReqmtJustification = a.ReqmtJustification,
                                          Deplytype = eWithAppType.AppDesc,
                                          Hostedon = hostType.HostingDesc,
                                          EncyID = _dataProtector.Protect(a.CurrentPslmId.ToString()),

                                          AttCnt = _dbContext.AttHistory.Count(f => f.PsmId == a.CurrentPslmId)
                                      }).ToListAsync();

                return projects;

            }
            else
            {
                var projects = await (from a in _DBContext.Projects
                                      join b in _DBContext.ProjStakeHolderMov on a.CurrentPslmId equals b.PsmId into bs
                                      from e in bs.DefaultIfEmpty()
                                      join actm in _dbContext.TrnStatusActionsMapping on e.StatusActionsMappingId equals actm.StatusActionsMappingId
                                      join d in _DBContext.mStatus on actm.StatusId equals d.StatusId into ds
                                      from eWithStatus in ds.DefaultIfEmpty()
                                      join c in _DBContext.tbl_mUnitBranch on a.StakeHolderId equals c.unitid into cs
                                      from eWithUnit in cs.DefaultIfEmpty()
                                      join h in _DBContext.mHostType on a.HostTypeID equals h.HostTypeID into hs
                                      from hostType in hs.DefaultIfEmpty()
                                      join g in _DBContext.tbl_mUnitBranch on e.ToUnitId equals g.unitid into csh
                                      from curstk in csh.DefaultIfEmpty()

                                      join i in _DBContext.mAppType on a.Apptype equals i.Apptype into ms
                                      from eWithAppType in ms.DefaultIfEmpty()


                                      join f in _DBContext.Comment on a.CurrentPslmId equals f.PsmId into fs
                                      from eWithComment in fs.DefaultIfEmpty()
                                      where a.IsActive && !a.IsDeleted
                                      //&& e.ActionCde > 9999999 
                                      && actm.StatusId == 9999999
                                      orderby e.TimeStamp descending
                                      select new tbl_Projects
                                      {
                                          ProjId = a.ProjId,
                                          ProjName = a.ProjName,
                                          StakeHolderId = a.StakeHolderId,
                                          CurrentPslmId = a.CurrentPslmId,
                                          InitiatedDate = a.InitiatedDate,
                                          CompletionDate = a.CompletionDate,
                                          IsWhitelisted = a.IsWhitelisted,
                                          InitialRemark = a.InitialRemark,
                                          IsDeleted = a.IsDeleted,
                                          IsActive = a.IsActive,
                                          EditDeleteBy = a.EditDeleteBy,
                                          EditDeleteDate = a.EditDeleteDate,
                                          UpdatedByUserId = a.UpdatedByUserId,
                                          DateTimeOfUpdate = e.DateTimeOfUpdate,
                                          //ToUnitId = a.ToUnitId,
                                          StakeHolder = eWithUnit.UnitName,
                                          FwdtoUser = curstk.UnitName,
                                          Status = eWithStatus.Status,
                                          Comments = eWithComment.Comment,
                                          //ActionCde = e.ActionCde,
                                          AimScope = a.AimScope,
                                          ReqmtJustification = a.ReqmtJustification,
                                          Deplytype = eWithAppType.AppDesc,
                                          Hostedon = hostType.HostingDesc,
                                          EncyID = _dataProtector.Protect(a.CurrentPslmId.ToString()),

                                          AttCnt = _dbContext.AttHistory.Count(f => f.PsmId == a.CurrentPslmId)
                                      }).ToListAsync();

                return projects;
            }

        }

        public async Task<List<tbl_Projects>> GetProjforCommentsAsync()
        {

            Login Logins = SessionHelper.GetObjectFromJson<Login>(_httpContextAccessor.HttpContext.Session, "User");

            //if (Logins != null && Logins.Role == "Unit")
            //{
            string username = Logins.UserName;

            int stkholder = Logins.unitid.HasValue ? Logins.unitid.Value : 0;

            var projects = await (from a in _DBContext.Projects
                                  join b in _DBContext.ProjStakeHolderMov on a.CurrentPslmId equals b.PsmId into bs
                                  from e in bs.DefaultIfEmpty()
                                  join actm in _dbContext.TrnStatusActionsMapping on e.StatusActionsMappingId equals actm.StatusActionsMappingId
                                  join d in _DBContext.mStatus on actm.StatusId equals d.StatusId into ds
                                  from eWithStatus in ds.DefaultIfEmpty()
                                  join c in _DBContext.tbl_mUnitBranch on a.StakeHolderId equals c.unitid into cs
                                  from eWithUnit in cs.DefaultIfEmpty()
                                  join g in _DBContext.tbl_mUnitBranch on e.ToUnitId equals g.unitid into csh
                                  from curstk in csh.DefaultIfEmpty()

                                  join f in _DBContext.Comment on a.CurrentPslmId equals f.PsmId into fs
                                  from eWithComment in fs.DefaultIfEmpty()

                                  join i in _DBContext.mAppType on a.Apptype equals i.Apptype into ms
                                  from eWithAppType in ms.DefaultIfEmpty()

                                  where a.IsActive && !a.IsDeleted && a.StakeHolderId == Logins.unitid
                                  //&& e.ActionCde > 0 
                                  //&& e.ActionId > 4
                                  orderby e.TimeStamp descending
                                  select new tbl_Projects
                                  {
                                      ProjId = a.ProjId,
                                      ProjName = a.ProjName,
                                      StakeHolderId = a.StakeHolderId,
                                      CurrentPslmId = a.CurrentPslmId,
                                      InitiatedDate = a.InitiatedDate,
                                      CompletionDate = a.CompletionDate,
                                      IsWhitelisted = a.IsWhitelisted,
                                      InitialRemark = a.InitialRemark,
                                      IsDeleted = a.IsDeleted,
                                      IsActive = a.IsActive,
                                      EditDeleteBy = a.EditDeleteBy,
                                      EditDeleteDate = a.EditDeleteDate,
                                      UpdatedByUserId = a.UpdatedByUserId,
                                      DateTimeOfUpdate = e.DateTimeOfUpdate,
                                      //ToUnitId = a.ToUnitId,
                                      StakeHolder = eWithUnit.UnitName,
                                      FwdtoUser = curstk.UnitName,
                                      Status = eWithStatus.Status,
                                      Comments = eWithComment.Comment,
                                      Deplytype = eWithAppType.AppDesc,
                                      //ActionCde = e.ActionCde,
                                      AimScope = a.AimScope,
                                      ReqmtJustification = a.ReqmtJustification,
                                      EncyID = _dataProtector.Protect(a.CurrentPslmId.ToString()),

                                      AttCnt = _dbContext.AttHistory.Count(f => f.PsmId == a.CurrentPslmId)
                                  }).ToListAsync();

            return projects;

            //}

        }




        public async Task<List<tbl_Projects>> GetProjforCommentsAsync1()
        {

            Login Logins = SessionHelper.GetObjectFromJson<Login>(_httpContextAccessor.HttpContext.Session, "User");

            //if (Logins != null && Logins.Role == "Unit")
            //{
            string username = Logins.UserName;

            int stkholder = Logins.unitid.HasValue ? Logins.unitid.Value : 0;

            var projects = await (from a in _DBContext.Projects
                                  join b in _DBContext.ProjStakeHolderMov on a.CurrentPslmId equals b.PsmId into bs
                                  from e in bs.DefaultIfEmpty()
                                  join actm in _dbContext.TrnStatusActionsMapping on e.StatusActionsMappingId equals actm.StatusActionsMappingId
                                  join d in _DBContext.mStatus on actm.StatusId equals d.StatusId into ds
                                  from eWithStatus in ds.DefaultIfEmpty()
                                  join c in _DBContext.tbl_mUnitBranch on a.StakeHolderId equals c.unitid into cs
                                  from eWithUnit in cs.DefaultIfEmpty()
                                  join g in _DBContext.tbl_mUnitBranch on e.ToUnitId equals g.unitid into csh
                                  from curstk in csh.DefaultIfEmpty()

                                  join f in _DBContext.Comment on a.CurrentPslmId equals f.PsmId into fs
                                  from eWithComment in fs.DefaultIfEmpty()
                                  where a.IsActive && !a.IsDeleted && a.StakeHolderId == Logins.unitid
                                  //&& e.ActionCde > 0 
                                  //  && actm.ActionsId > 4
                                  orderby e.TimeStamp descending
                                  select new tbl_Projects
                                  {
                                      ProjId = a.ProjId,
                                      ProjName = a.ProjName,
                                      StakeHolderId = a.StakeHolderId,
                                      CurrentPslmId = a.CurrentPslmId,
                                      InitiatedDate = a.InitiatedDate,
                                      CompletionDate = a.CompletionDate,
                                      IsWhitelisted = a.IsWhitelisted,
                                      InitialRemark = a.InitialRemark,
                                      IsDeleted = a.IsDeleted,
                                      IsActive = a.IsActive,
                                      EditDeleteBy = a.EditDeleteBy,
                                      EditDeleteDate = a.EditDeleteDate,
                                      UpdatedByUserId = a.UpdatedByUserId,
                                      DateTimeOfUpdate = e.DateTimeOfUpdate,
                                      //ToUnitId = a.ToUnitId,
                                      StakeHolder = eWithUnit.UnitName,
                                      FwdtoUser = curstk.UnitName,
                                      Status = eWithStatus.Status,
                                      Comments = eWithComment.Comment,
                                      //ActionCde = e.ActionCde,
                                      AimScope = a.AimScope,
                                      ReqmtJustification = a.ReqmtJustification,
                                      EncyID = _dataProtector.Protect(a.CurrentPslmId.ToString()),

                                      AttCnt = _dbContext.AttHistory.Count(f => f.PsmId == a.CurrentPslmId)
                                  }).ToListAsync();


            return projects;
            //}

        }

        public async Task<List<DTOUnderProcessProj>> GetHoldActionProj()
        {
            var query = await (from a in _dbContext.Projects
                               join b in _dbContext.ProjStakeHolderMov on a.ProjId equals b.ProjId
                               join stackc in _dbContext.tbl_mUnitBranch on a.StakeHolderId equals stackc.unitid
                               join tounit in _dbContext.tbl_mUnitBranch on b.ToUnitId equals tounit.unitid
                               join fromunit in _dbContext.tbl_mUnitBranch on b.FromUnitId equals fromunit.unitid
                               join actmap in _dbContext.TrnStatusActionsMapping on b.StatusActionsMappingId equals actmap.StatusActionsMappingId
                               join ststus in _dbContext.mStatus on actmap.StatusId equals ststus.StatusId
                               join stge in _dbContext.mStages on ststus.StageId equals stge.StagesId
                               join act in _dbContext.mActions on actmap.ActionsId equals act.ActionsId

                               where !b.IsComplete && !b.IsComment && !b.IsDeleted

                               orderby b.TimeStamp descending
                               select new DTOUnderProcessProj
                               {
                                   ProjName = a.ProjName,
                                   StatusName = ststus.Status,
                                   Sponser = a.Sponsor,
                                   StageName = stge.Stages,
                                   ActionName = act.Actions,

                                   //tooltip start
                                   UnitName = _psmRepository.GetSponsorUnitName(a.StakeHolderId),
                                   FromUnitUserDetail = fromunit.UnitName,
                                   FromUnitName = " " + fromunit.UnitName + " ( " + b.UserDetails + ")",
                                   Remark = b.Remarks,
                                   //end
                                   ToUnitName = tounit.UnitName,
                                   DateTimeOfUpdate = b.TimeStamp,
                               }).ToListAsync();

            return query;

        }

        public async Task<List<tbl_ProjStakeHolderMov>> GetInboxByProjIdAsync(int projId)
        {
            return await _dbContext.ProjStakeHolderMov
                .Where(a => a.ProjId == projId)
                .ToListAsync();
        }



        public async Task<Notification> GetNotificationByProjId(int ProjId)
        {
            return await _dbContext.Notification
               .FirstOrDefaultAsync(a => a.ProjId == ProjId);

        }


        public async Task<int> GetNotificationCommentCount()
        {
            var loginUser = SessionHelper.GetObjectFromJson<Login>(_httpContextAccessor.HttpContext.Session, "User");
            int notificationCount = await _dbContext.Notification
                .Where(n => n.NotificationType == 1 && n.IsRead == false && n.NotificationTo == loginUser.unitid)
                .CountAsync();
            return notificationCount;
        }

        public async Task<bool> UpdateNotification(Notification notify)
        {
            var loginUser = SessionHelper.GetObjectFromJson<Login>(_httpContextAccessor.HttpContext.Session, "User");
            if (loginUser == null)
            {
                return false;
            }

            var existingNotification = await _dbContext.Notification
                .FirstOrDefaultAsync(x => x.NotificationTo == loginUser.unitid && x.ProjId == notify.ProjId && x.NotificationType == 1);

            if (existingNotification != null)
            {
                existingNotification.IsRead = true;
                existingNotification.ReadDateTime = notify.ReadDateTime; // Update ReadDateTime here
                _dbContext.Notification.Update(existingNotification);
                await _dbContext.SaveChangesAsync();
                return true;
            }

            return false;
        }

        public async Task<bool> UpdateUnReadNotification(Notification notify)
        {
            var loginUser = SessionHelper.GetObjectFromJson<Login>(_httpContextAccessor.HttpContext.Session, "User");
            if (loginUser == null)
            {
                return false;
            }

            var existingNotification = await _dbContext.Notification
                .FirstOrDefaultAsync(x => x.NotificationTo == loginUser.unitid && x.ProjId == notify.ProjId);

            if (existingNotification != null)
            {
                existingNotification.IsRead = false;
                existingNotification.ReadDateTime = notify.ReadDateTime; // Update ReadDateTime here
                _dbContext.Notification.Update(existingNotification);
                await _dbContext.SaveChangesAsync();
                return true;
            }

            return false;
        }


        public async Task<tbl_ProjStakeHolderMov> GetNextPsmMoveAsync(int projId, int currentPsmId)
        {
            return await _DBContext.ProjStakeHolderMov
                .Where(x => x.ProjId == projId && x.PsmId > currentPsmId)
                .OrderBy(x => x.PsmId)
                .FirstOrDefaultAsync();
        }


        public async Task<List<DTODDLComman>> GetALLByProjectName(string ProjName)
        {
            try
            {
                var GetALL = (from A in _DBContext.Projects
                              where A.ProjName.Contains(ProjName) && A.IsSubmited == true
                              select new DTODDLComman
                              {
                                  Id = A.ProjId,
                                  Name = A.ProjName,
                              }).Take(5).ToList();
                return await Task.FromResult(GetALL);
            }
            catch (Exception ex)
            {
                return null;
            }

        }



        public async Task<bool> UpdateNotificationByProjID(Notification notify)
        {
            _dbContext.Notification.Update(notify);
            await _dbContext.SaveChangesAsync();
            return true;
        }


        public async Task<List<tbl_ProjStakeHolderMov>> GetInboxByProjIdExcludingPsmIdAsync(int projId, int psmId)
        {
            return await _dbContext.ProjStakeHolderMov
                                   .Where(c => c.ProjId == projId && c.PsmId != psmId)
                                   .ToListAsync();
        }


        public async Task<List<tbl_ProjStakeHolderMov>> GetCommentByExcludingPsmId(int projId, int? ToUnitId)
        {
            // Fetch latest records for each ProjId where IsComment is false and PsmId is not excluded
            // Fetch the latest records for each ProjId where IsComment is false and ToUnitId is not excluded
            var latestType2 = await _dbContext.ProjStakeHolderMov
                .Where(n => n.IsComment == false && n.ToUnitId != ToUnitId && n.ProjId == projId) // Replace psmId with toUnitId
                .GroupBy(n => n.ProjId)
                .Select(g => g.OrderByDescending(n => n.ToUnitId).FirstOrDefault()) // Replace psmId with toUnitId
                .ToListAsync();

            // Fetch comments where IsComment is true and ToUnitId is not excluded
            var comments = await _dbContext.ProjStakeHolderMov
                .Where(n => n.IsComment == true && n.ToUnitId != ToUnitId && n.ProjId == projId) // Replace psmId with toUnitId
                .ToListAsync();

            // Combine results
            var result = comments
                .Union(latestType2)
                .OrderBy(n => n.ToUnitId) // Replace psmId with toUnitId
                .ToList();

            return result;

        }

        public async Task<List<tbl_ProjStakeHolderMov>> GetInboxByProjAndUnitId(int projId, int? ToUnitId)
        {
            return await _dbContext.ProjStakeHolderMov
                .Where(a => a.ProjId == projId && a.ToUnitId == ToUnitId)
                .ToListAsync();
        }

        public async Task<int> GetIsCommentPsmiId(int? ProjId, int? StackHolderId)
        {
            var ret = await _dbContext.ProjStakeHolderMov.Where(i => i.ProjId == ProjId && i.IsComment == true && i.ToUnitId == StackHolderId).SingleOrDefaultAsync();
            if (ret != null)
                return ret.PsmId;
            else
                return 0;
        }

        //public async Task<List<tbl_ProjStakeHolderMov>> GetCommentByProjAndUnitId(int projId, int? ToUnitId)
        //{
        //    return await _dbContext.ProjStakeHolderMov
        //        .Where(a => a.ProjId == projId && a.ToUnitId == ToUnitId && a.IsComment == 1)
        //        .ToListAsync();
        //}

        public async Task<tbl_ProjStakeHolderMov> GetProjStkHolderMovmentByPsmiId(int? PsmId)
        {
            var ret = await _dbContext.ProjStakeHolderMov.Where(i => i.PsmId == PsmId).SingleOrDefaultAsync();
            if (ret != null)
                return ret;
            else
                return new tbl_ProjStakeHolderMov();
        }

        public async Task<bool> UpdateProjectStkMovementAsync(tbl_ProjStakeHolderMov project)
        {
            Login Logins = SessionHelper.GetObjectFromJson<Login>(_httpContextAccessor.HttpContext.Session, "User");
            if (Logins != null)
            {
                _dbContext.Entry(project).State = EntityState.Modified;
                await _dbContext.SaveChangesAsync();
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<ApplicationUser> GetUserByUnitId(int? UnitId)
        {
            var ret = await _dbContext.Users.Where(i => i.unitid == UnitId).FirstOrDefaultAsync();
            if (ret != null)
                return ret;
            else
                return new ApplicationUser();
        }

        //public async Task<int> GetProjIdByPsmiId(int? Psmid, int? StackHolderId)
        //{
        //    var ret = await _dbContext.ProjStakeHolderMov.Where(i => i.PsmId == Psmid).FirstOrDefaultAsync();
        //    if (ret != null)
        //        return ret.ProjId;
        //    else
        //        return 0;
        //}

        //public async Task<List<tbl_ProjStakeHolderMov>> GetProjStkHolderMovmentByProjId(int? ProjId)
        //{
        //    var ret = await _dbContext.ProjStakeHolderMov.Where(i => i.ProjId == ProjId && i.IsComment == true).ToListAsync();
        //    if (ret != null)
        //        return ret;
        //    else
        //        return new List<tbl_ProjStakeHolderMov>();
        //}

    }

}
