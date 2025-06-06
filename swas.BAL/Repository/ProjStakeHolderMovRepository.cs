using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using swas.BAL.DTO;
using swas.BAL.Helpers;
using swas.BAL.Interfaces;
using swas.DAL;
using swas.DAL.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.DataProtection;
using Grpc.Core;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Components.Routing;
using ASPNetCoreIdentityCustomFields.Data;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using System.Xml.Linq;
using System.Net.NetworkInformation;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Diagnostics;

namespace swas.BAL.Repository
{

    ///Created and Reviewed by : Sub Maj M Sanal
    ///Reviewed Date : 31 Jul 23
    ///Tested By :- 
    ///Tested Date : 
    ///Start
    public class ProjStakeHolderMovRepository : GenericRepositoryDL<tbl_ProjStakeHolderMov>, IProjStakeHolderMovRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IDataProtector _dataProtector;

        public ProjStakeHolderMovRepository(ApplicationDbContext dbContext, IDataProtectionProvider DataProtector, IHttpContextAccessor httpContextAccessor) : base(dbContext)
        {
            _dbContext = dbContext;
            _httpContextAccessor = httpContextAccessor;
            _dataProtector = DataProtector.CreateProtector("swas.UI.Controllers.ProjectsController");

        }

        public async Task<List<DTOProjectsFwd>> ProjectMovement(int? ProjectId)
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

                               where b.ProjId == ProjectId & b.IsComment == false
                               orderby b.TimeStamp descending
                               select new DTOProjectsFwd
                               {
                                   PsmIds = b.PsmId,
                                   Stage = stge.Stages,
                                   StageId = stge.StagesId,
                                   ProjId = a.ProjId,
                                   //Status = tounit.UnitName + " " + "For Comments",
                                   Status = ststus.Status,
                                   StatusId = ststus.StatusId,
                                   Action = act.Actions,
                                   ActionId = b.StatusActionsMappingId,
                                   FromUnitName = " " + b.UserDetails + " ( " + fromunit.UnitName + ")",
                                   ToUnitName = tounit.UnitName,
                                   ToUnitId = tounit.unitid,
                                   DateTimeOfUpdate = b.TimeStamp,
                                   Remarks = b.Remarks,
                                   StakeHolderId = a.StakeHolderId,
                                   IsComment = b.IsComment,
                                   AttCnt = _dbContext.AttHistory.Count(f => f.PsmId == b.PsmId)

                               }).ToListAsync();


            return query;
        }


        public async Task<DTOProjectMovHistory> ProjectMovHistory(int? ProjectId)
        {
            DTOProjectMovHistory lst = new DTOProjectMovHistory();
            var queryforstackholderself = await (from a in _dbContext.Projects
                                                 join b in _dbContext.ProjStakeHolderMov on a.ProjId equals b.ProjId
                                                 where a.ProjId == ProjectId && b.IsComment == true
                                                 && a.StakeHolderId == b.ToUnitId //&& b.StatusActionsMappingId == 21
                                                 select new DTOForStackHolderCout
                                                 {
                                                     PsmId = b.PsmId
                                                 }
                                                 ).ToListAsync();




            var query = await (from a in _dbContext.Projects
                               join b in _dbContext.ProjStakeHolderMov on a.ProjId equals b.ProjId
                               join stackc in _dbContext.tbl_mUnitBranch on a.StakeHolderId equals stackc.unitid
                               join tounit in _dbContext.tbl_mUnitBranch on b.ToUnitId equals tounit.unitid
                               join fromunit in _dbContext.tbl_mUnitBranch on b.FromUnitId equals fromunit.unitid
                               join actmap in _dbContext.TrnStatusActionsMapping on b.StatusActionsMappingId equals actmap.StatusActionsMappingId
                               join ststus in _dbContext.mStatus on actmap.StatusId equals ststus.StatusId
                               join stge in _dbContext.mStages on ststus.StageId equals stge.StagesId
                               join act in _dbContext.mActions on actmap.ActionsId equals act.ActionsId

                               where b.ProjId == ProjectId
                               orderby b.TimeStamp descending
                               //orderby b.PsmId descending
                               select new DTOProjectMovHistorypsm
                               {
                                   PsmId = b.PsmId,
                                   Stages = stge.Stages,
                                   //Status = tounit.UnitName + " " + "For Comments",
                                   Status = ststus.Status,
                                   StatusId = ststus.StatusId,
                                   Actions = act.Actions,
                                   FromUnitName = " " + b.UserDetails + " ( " + fromunit.UnitName + ")",
                                   ToUnitName = tounit.UnitName,
                                   FromUser = "",
                                   ToUser = "",
                                   Date = b.TimeStamp,
                                   Remarks = b.Remarks,
                                   UndoRemarks = b.UndoRemarks,
                                   IsComment = b.IsComment,
                                   AttCnt = _dbContext.AttHistory.Count(f => f.PsmId == b.PsmId),
                                   UserDetails = b.UserDetails,
                                   IsPulledBack = b.IsPullBack,
                                   StatusActionsMappingId = b.StatusActionsMappingId

                               }).ToListAsync();
            if (queryforstackholderself != null && queryforstackholderself.Count == 2)
                lst.DTOProjectMovHistorypsmlst = query.Where(i => i.PsmId != queryforstackholderself[1].PsmId).ToList();
            else
                lst.DTOProjectMovHistorypsmlst = query;
            // lst.DTOProjectMovHistorypsmlst = query.GroupBy(x => x.StatusId).Select(grp => grp.FirstOrDefault()).ToList();
            var comments = await (from mov in _dbContext.ProjStakeHolderMov
                                  join stk in _dbContext.StkComment on mov.PsmId equals stk.PsmId
                                  join stksts in _dbContext.StkStatus on stk.StkStatusId equals stksts.StkStatusId
                                  where mov.ProjId == ProjectId
                                  select new DTOProjectMovHistorycmd
                                  {
                                      PsmId = mov.PsmId,
                                      Status = stksts.Status,
                                      Comments = stk.Comments,
                                      DateTimeOfUpdate = stk.DateTimeOfUpdate,
                                      UserDetails = stk.UserDetails != null ? stk.UserDetails : "____",



                                  }).ToListAsync();
            //var lastInitialStageRecord = query.LastOrDefault(record => record.Stages == "Initial Stage");

            //if (lastInitialStageRecord != null)
            //{
            //    lastInitialStageRecord.Status = lastInitialStageRecord.ToUnitName;
            //}
            lst.DTOProjectMovHistorycmdlst = comments;

            return lst;
        }
        public async Task<List<DTOProjectHold>> ProjectHolsTimeCalculate(int ProjectId)
        {
            List<DTOProjectHold> lst = new List<DTOProjectHold>();
            var databyprojectid = await (from mov in _dbContext.ProjStakeHolderMov
                                         join munit1 in _dbContext.tbl_mUnitBranch on mov.ToUnitId equals munit1.unitid
                                         join munit2 in _dbContext.tbl_mUnitBranch on mov.FromUnitId equals munit2.unitid
                                         join stsmap in _dbContext.TrnStatusActionsMapping on mov.StatusActionsMappingId equals stsmap.StatusActionsMappingId
                                         join act in _dbContext.mActions on stsmap.ActionsId equals act.ActionsId
                                         join sts in _dbContext.mStatus on stsmap.StatusId equals sts.StatusId
                                         where
                                         // mov.IsComment==false &&
                                         mov.ProjId == ProjectId

                                         orderby mov.PsmId
                                         select new DTOProjectHold
                                         {
                                             PsmId = mov.PsmId,
                                             TounitId = mov.ToUnitId,
                                             Tounit = munit1.UnitName,
                                             FromunitId = mov.FromUnitId,
                                             Fromunit = munit2.UnitName,
                                             TimeStamp = mov.TimeStamp,
                                             DateTimeOfUpdate = mov.DateTimeOfUpdate,
                                             Status = sts.Status,
                                             Action = act.ActionDesc,
                                             IsComment = mov.IsComment,
                                             IsComplete = mov.IsComplete,
                                             UndoRemarks = mov.UndoRemarks,
                                         }).ToListAsync();


            for (int i = 0; i < databyprojectid.Count(); i++)
            {
                DTOProjectHold db = new DTOProjectHold();
                db.PsmId = databyprojectid[i].PsmId;
                if (databyprojectid[i].IsComment == false)
                {



                    if (i == 0)
                    {
                        db.FromunitId = databyprojectid[i].FromunitId;
                        db.Fromunit = databyprojectid[i].Fromunit;
                        db.TimeStampfrom = databyprojectid[i].TimeStamp;
                        db.IsComment = databyprojectid[i].IsComment;

                        db.IsComplete = databyprojectid[i].IsComplete;
                        db.TimeStampTo = databyprojectid[i].DateTimeOfUpdate;
                        db.TounitId = databyprojectid[i].TounitId;
                        db.Tounit = databyprojectid[i].Tounit;
                        db.Status = databyprojectid[i].Status;
                        db.Action = databyprojectid[i].Action;
                        db.UndoRemarks = databyprojectid[i].UndoRemarks;
                    }
                    else
                    {
                        db.FromunitId = databyprojectid[i].FromunitId;
                        db.Fromunit = databyprojectid[i].Fromunit;
                        db.TimeStampfrom = databyprojectid[i].TimeStamp;
                        db.IsComment = databyprojectid[i].IsComment;
                        db.TimeStampTo = DateTime.Now;
                        db.IsComplete = databyprojectid[i].IsComplete;
                        db.UndoRemarks = databyprojectid[i].UndoRemarks;
                        int j = i;
                        j++;

                        db.TounitId = databyprojectid[i].TounitId;
                        db.Tounit = databyprojectid[i].Tounit;
                        db.Status = databyprojectid[i].Status;
                        db.Action = databyprojectid[i].Action;
                        if (j < databyprojectid.Count())
                        {
                            int @psmid1 = databyprojectid[j].PsmId;
                            db.TimeStampTo = databyprojectid[j].TimeStamp;

                        }
                    }

                }
                else
                {
                    db.FromunitId = databyprojectid[i].FromunitId;
                    db.Fromunit = databyprojectid[i].Fromunit;
                    db.TimeStampfrom = databyprojectid[i].TimeStamp;
                    db.UndoRemarks = databyprojectid[i].UndoRemarks;
                    if (databyprojectid[i].IsComplete == true)
                        db.TimeStampTo = databyprojectid[i].DateTimeOfUpdate;
                    else
                        db.TimeStampTo = DateTime.Now;
                    db.IsComment = databyprojectid[i].IsComment;
                    db.IsComplete = databyprojectid[i].IsComplete;


                    db.TounitId = databyprojectid[i].TounitId;
                    db.Tounit = databyprojectid[i].Tounit;
                    db.Status = databyprojectid[i].Status;
                    db.Action = databyprojectid[i].Action;
                }
                lst.Add(db);
            }


            return lst.OrderByDescending(i => i.PsmId).ToList();
        }
        public int GetLastRecProjectMov(int ProjectId)
        {
            try
            {
                //var query = _context.ProjStakeHolderMov.Where(i => i.ProjId == ProjectId && i.IsActive==true && i.IsComment==false && i.UndoRemarks==null).Max(p => p.PsmId);
                //return query;

                var maxPsmIdParameter = new SqlParameter("@MaxPsmId", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };
                _context.Database.ExecuteSqlRaw(
                    "EXEC GetMaxPsmId @ProjectId = {0}, @MaxPsmId = @MaxPsmId OUTPUT",
                    ProjectId,
                    maxPsmIdParameter
                );
                return (int)(maxPsmIdParameter.Value ?? 0);

            }
            catch (Exception ex) { return 0; }

        }
        public async Task<int> GetLastRecProjectMovForUnod(int ProjectId, int? TounitId)
        {
            try
            {
                // var query = _context.ProjStakeHolderMov.Where(i => i.ProjId == ProjectId && i.PsmId < (_context.ProjStakeHolderMov.Max(p => p.PsmId))).Max(p => p.PsmId);
                var query = await _context.ProjStakeHolderMov.Where(i => i.ProjId == ProjectId && i.IsActive == true && i.IsComment == false && i.ToUnitId == TounitId).OrderByDescending(i => i.PsmId).Take(1).Select(i => i.PsmId).SingleOrDefaultAsync();
                return query;

            }
            catch (Exception ex) { return 0; }

        }
        public async Task<DTODashboard> DashboardCount(int UserId)
        {
            DTODashboard db = new DTODashboard();

            #region DashboaardWithLinq
            //var query = await (from mov in _dbContext.ProjStakeHolderMov
            //                   join proj in _dbContext.Projects on mov.ProjId equals proj.ProjId
            //                   join actmap in _dbContext.TrnStatusActionsMapping on mov.StatusActionsMappingId equals actmap.StatusActionsMappingId
            //                   join ststus in _dbContext.mStatus on actmap.StatusId equals ststus.StatusId
            //                   join stge in _dbContext.mStages on ststus.StageId equals stge.StagesId
            //                   //join act in _dbContext.mActions on actmap.ActionsId equals act.ActionsId
            //                   where
            //                   //mov.ToUnitId == UserId &&
            //                   mov.IsComplete == false
            //                   && mov.IsActive == true
            //                   /*&& mov.ToUnitId == 1 && mov.StatusId != 5*/
            //                   && ststus.IsDashboard == true
            //                   && proj.IsSubmited == true
            //                   && mov.StatusActionsMappingId != 118 && mov.StatusActionsMappingId != 4
            //                   orderby stge.StagesId ascending
            //                   group mov by new
            //                   {
            //                       ststus.StatusId,
            //                       QStages = stge.Stages,
            //                       QStagesId = stge.StagesId,
            //                       QStatus = ststus.Status,

            //                       QIsComplete = mov.IsComplete,
            //                       QprojId = proj.ProjId

            //                   } into gr  //,QActionId= actmap.ActionsId

            //                   select new DTODashboardCount
            //                   {

            //                       StatusId = gr.Key.StatusId,
            //                       Stages = gr.Key.QStages,
            //                       StagesId = gr.Key.QStagesId,
            //                       Status = gr.Key.QStatus,
            //                       IsComplete = gr.Key.QIsComplete,
            //                       //ActionId = gr.Key.QActionId,
            //                       Tot = gr.Count(),

            //                   }).ToListAsync();

            //db.DTODashboardCountlst = (query);

            //var query11 = await (from mov in _dbContext.ProjStakeHolderMov
            //                     join proj in _dbContext.Projects on mov.ProjId equals proj.ProjId
            //                     join actmap in _dbContext.TrnStatusActionsMapping on mov.StatusActionsMappingId equals actmap.StatusActionsMappingId
            //                     join ststus in _dbContext.mStatus on actmap.StatusId equals ststus.StatusId
            //                     join stge in _dbContext.mStages on ststus.StageId equals stge.StagesId
            //                     //join act in _dbContext.mActions on actmap.ActionsId equals act.ActionsId
            //                     where
            //                     // mov.FromUnitId == UserId && 
            //                     mov.IsComplete == true
            //                      && mov.IsActive == true/*&& mov.ToUnitId == 1 && mov.StatusId != 5*/
            //                     && ststus.IsDashboard == true
            //                     && proj.IsSubmited == true
            //                      && mov.StatusActionsMappingId != 118 && mov.StatusActionsMappingId != 4
            //                     orderby stge.StagesId ascending
            //                     group mov by new
            //                     {
            //                         ststus.StatusId,
            //                         QStages = stge.Stages,
            //                         QStagesId = stge.StagesId,
            //                         QStatus = ststus.Status,

            //                         QIsComplete = mov.IsComplete,
            //                         QprojId = proj.ProjId

            //                     } into gr  //,QActionId= actmap.ActionsId

            //                     select new DTODashboardCount
            //                     {

            //                         StatusId = gr.Key.StatusId,
            //                         Stages = gr.Key.QStages,
            //                         StagesId = gr.Key.QStagesId,
            //                         Status = gr.Key.QStatus,
            //                         IsComplete = gr.Key.QIsComplete,
            //                         //ActionId = gr.Key.QActionId,

            //                         Tot = gr.Count()
            //                     }).ToListAsync();


            //db.DTODashboardCountlst.AddRange(query11);
            //db.DTODashboardCountlst = db.DTODashboardCountlst.OrderBy(x => x.StagesId).OrderBy(x => x.StatusId).ToList();

            //var queryForAction = await (from mov in _dbContext.ProjStakeHolderMov
            //                            join proj in _dbContext.Projects on mov.ProjId equals proj.ProjId
            //                            join actmap in _dbContext.TrnStatusActionsMapping on mov.StatusActionsMappingId equals actmap.StatusActionsMappingId
            //                            join ststus in _dbContext.mStatus on actmap.StatusId equals ststus.StatusId
            //                            join stge in _dbContext.mStages on ststus.StageId equals stge.StagesId
            //                            //join act in _dbContext.mActions on actmap.ActionsId equals act.ActionsId
            //                            where
            //                          //mov.ToUnitId == UserId &&
            //                          // mov.IsComplete == false &&
            //                          (mov.StatusActionsMappingId == 4 || mov.StatusActionsMappingId == 118 || mov.StatusActionsMappingId == 3   //-----stage1
            //                         || mov.StatusActionsMappingId == 49 || mov.StatusActionsMappingId == 54 //-----stage2
            //                         || mov.StatusActionsMappingId == 64 || mov.StatusActionsMappingId == 69 //-----stage3
            //                         || mov.StatusActionsMappingId == 74 || mov.StatusActionsMappingId == 79 //-----stage3
            //                         || mov.StatusActionsMappingId == 84 || mov.StatusActionsMappingId == 89 //-----stage3
            //                         )
            //                           && mov.IsActive == true
            //                            /*&& mov.ToUnitId == 1 && mov.StatusId != 5*/

            //                            && proj.IsSubmited == true
            //                            orderby stge.StagesId ascending
            //                            group mov by new
            //                            {
            //                                ststus.StatusId,
            //                                QStages = stge.Stages,
            //                                QStagesId = stge.StagesId,
            //                                QStatus = ststus.Status,
            //                                QActionsId = actmap.ActionsId,
            //                                QIsComplete = mov.IsComplete,
            //                                QprojId = proj.ProjId

            //                            } into gr  //,QActionId= actmap.ActionsId

            //                            select new DTODashboardCount
            //                            {

            //                                StatusId = gr.Key.StatusId,
            //                                Stages = gr.Key.QStages,
            //                                StagesId = gr.Key.QStagesId,
            //                                Status = gr.Key.QStatus,
            //                                IsComplete = gr.Key.QIsComplete,
            //                                ActionId = gr.Key.QActionsId,
            //                                Tot = gr.Count(),

            //                            }).ToListAsync();

            //db.DTODashboardCountlstForAction = (queryForAction);




            //var query1 = await (from ststus in _dbContext.mStatus
            //                    join stge in _dbContext.mStages on ststus.StageId equals stge.StagesId
            //                    where ststus.IsDashboard == true

            //                    select new DTODashboardHeader
            //                    {
            //                        StageId = stge.StagesId,
            //                        StatusId = ststus.StatusId,
            //                        Status = ststus.Status,
            //                        Stages = stge.Stages,
            //                        Icons = ststus.Icon,
            //                        Statseq = ststus.Statseq

            //                    }).ToListAsync();
            //db.DTODashboardHeaderlst = query1;
            //db.DTODashboardHeaderlst = db.DTODashboardHeaderlst.OrderBy(x => x.Statseq).ToList().OrderBy(x => x.StageId).ToList();

            ////db.DTODashboardHeaderlst = (List<DTODashboardHeader>)db.DTODashboardHeaderlst.OrderBy(i => i.Statseq);
            ////var query2 = await (from actmap in _dbContext.TrnStatusActionsMapping
            ////                    join ststus in _dbContext.mStatus on actmap.StatusId equals ststus.StatusId
            ////                    join act in _dbContext.mActions on actmap.ActionsId equals act.ActionsId
            ////                    orderby actmap.ActionsId ascending
            ////                    select new DTODashboardAction
            ////                    {
            ////                        StatusId= ststus.StatusId,
            ////                        ActionId= act.ActionsId,
            ////                        Action=act.Actions
            ////                    }).ToListAsync();
            ////db.DTODashboardActionlst = query2;

            //var approvedcount = await (from mov in _dbContext.ProjStakeHolderMov
            //                           join pro in _context.Projects on mov.ProjId equals pro.ProjId
            //                           join stsmap in _context.TrnStatusActionsMapping on mov.StatusActionsMappingId equals stsmap.StatusActionsMappingId
            //                           where mov.IsActive == true && pro.IsProcess == true &&
            //                           (stsmap.StatusActionsMappingId == 1 ||    //New Projects
            //                           stsmap.StatusActionsMappingId == 9 ||      //Obsn
            //                           stsmap.StatusActionsMappingId == 113 ||   //Obsn Rectified
            //                           stsmap.StatusActionsMappingId == 48 ||     //Auto Committee
            //                           stsmap.StatusActionsMappingId == 53 ||     //IPA Stage
            //                           stsmap.StatusActionsMappingId == 60 ||     //Closed
            //                           stsmap.StatusActionsMappingId == 63 ||     //AHCC (Arch Vetting)
            //                           stsmap.StatusActionsMappingId == 68 ||     //ACG (Lab Test)
            //                           stsmap.StatusActionsMappingId == 73 ||     //AHCC (IAM Integ)
            //                           stsmap.StatusActionsMappingId == 78 ||     //ACG (Remote Test)
            //                           stsmap.StatusActionsMappingId == 83 ||     //MI-11 Clearance
            //                           stsmap.StatusActionsMappingId == 88 ||     //Whitelisting Completed
            //                                                                      // ---CommentAttribute----
            //                           ((stsmap.StatusActionsMappingId == 26 ||//ASDC Vetting
            //                           stsmap.StatusActionsMappingId == 31 ||// ACG Vetting
            //                           stsmap.StatusActionsMappingId == 37) && mov.IsComplete == true)) //AHCC Vetting
            //                           group mov by new
            //                           {
            //                               stsmap.StatusId,
            //                               stsmap.StatusActionsMappingId,
            //                               pro.ProjId

            //                           } into gr
            //                           select new DTOApprovedCount
            //                           {
            //                               ProjId = gr.Key.ProjId,
            //                               StatusId = gr.Key.StatusId,
            //                               StatusActionsMappingId = gr.Key.StatusActionsMappingId,
            //                               Total = gr.Count()

            //                           }).ToListAsync();
            //db.DTOApprovedCountlst = approvedcount
            //    .GroupBy(p => new { p.StatusId, p.StatusActionsMappingId })
            //    .Select(g => new DTOApprovedCount
            //    {
            //        StatusId = g.Key.StatusId,
            //        StatusActionsMappingId = g.Key.StatusActionsMappingId,
            //        Total = g.Count()
            //    }).ToList();

            //int StatusId = _dbContext.mStatus.Where(x => x.Status == "BISAG-N").FirstOrDefault().StatusId;
            ////var bisagNProjId = await (from proj in _dbContext.Projects
            ////                          where proj.BeingDevpInhouse == "BISAG-N" && proj.IsSubmited == true /*&& proj.IsProcess == true*/
            ////                          select proj.ProjId).FirstOrDefaultAsync();

            //var bisagNCount = await (from proj in _dbContext.Projects
            //                         where proj.BeingDevpInhouse == "BISAG-N" && proj.IsSubmited == true /*&& proj.IsProcess == true*/
            //                         select proj).CountAsync();

            //var bisagNEntry = new DTOApprovedCount
            //{
            //    //ProjId = bisagNProjId, 
            //    //StatusId = 1041, // 
            //    StatusId = StatusId,
            //    Status = "BISAG-N",
            //    StatusActionsMappingId = 1,
            //    Total = bisagNCount  // The total count for BISAG-N projects
            //};

            //db.DTOApprovedCountlst.Add(bisagNEntry);

            //int Revettingid = _dbContext.mStatus.Where(x => x.Status == "Re-Vetting").FirstOrDefault().StatusId;          
            //var Revettingcount = await (from proj in _dbContext.Projects
            //                            where proj.IsWhitelisted == "Re-Vetted" && proj.IsSubmited == true /*&& proj.IsProcess == true*/
            //                            select proj).CountAsync();

            //var RevettingEntry = new DTOApprovedCount
            //{
            //    StatusId = Revettingid,
            //    Status = "Re-Vetted",
            //    StatusActionsMappingId = 1,
            //    Total = Revettingcount  
            //};

            //db.DTOApprovedCountlst.Add(RevettingEntry);
            #endregion


            #region DashboardWithStoredProcedure
            
            using (var conn = _dbContext.Database.GetDbConnection())
            {
                await conn.OpenAsync();

                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "dbo.usp_DashboardCount";
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Add parameter
                    var param = cmd.CreateParameter();
                    param.ParameterName = "@UserId";
                    param.Value = UserId;
                    cmd.Parameters.Add(param);

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        // 1st result set => DTODashboardCountlst (incomplete & active)
                        db.DTODashboardCountlst = new List<DTODashboardCount>();
                        while (await reader.ReadAsync())
                        {
                            db.DTODashboardCountlst.Add(new DTODashboardCount
                            {
                                StatusId = reader.GetInt32(reader.GetOrdinal("StatusId")),
                                Stages = reader.GetString(reader.GetOrdinal("Stages")),
                                StagesId = reader.GetInt32(reader.GetOrdinal("StagesId")),
                                Status = reader.GetString(reader.GetOrdinal("Status")),
                                IsComplete = reader.GetBoolean(reader.GetOrdinal("IsComplete")),
                                Tot = reader.GetInt32(reader.GetOrdinal("Tot")),
                                ActionId = 0 // no ActionId in this result set
                            });
                        }

                        // Move to 2nd result set => completed & active
                        await reader.NextResultAsync();
                        while (await reader.ReadAsync())
                        {
                            db.DTODashboardCountlst.Add(new DTODashboardCount
                            {
                                StatusId = reader.GetInt32(reader.GetOrdinal("StatusId")),
                                Stages = reader.GetString(reader.GetOrdinal("Stages")),
                                StagesId = reader.GetInt32(reader.GetOrdinal("StagesId")),
                                Status = reader.GetString(reader.GetOrdinal("Status")),
                                IsComplete = reader.GetBoolean(reader.GetOrdinal("IsComplete")),
                                Tot = reader.GetInt32(reader.GetOrdinal("Tot")),
                                ActionId = 0
                            });
                        }

                        // Move to 3rd result set => counts with ActionId
                        await reader.NextResultAsync();
                        db.DTODashboardCountlstForAction = new List<DTODashboardCount>();
                        while (await reader.ReadAsync())
                        {
                            db.DTODashboardCountlstForAction.Add(new DTODashboardCount
                            {
                                StatusId = reader.GetInt32(reader.GetOrdinal("StatusId")),
                                Stages = reader.GetString(reader.GetOrdinal("Stages")),
                                StagesId = reader.GetInt32(reader.GetOrdinal("StagesId")),
                                Status = reader.GetString(reader.GetOrdinal("Status")),
                                IsComplete = reader.GetBoolean(reader.GetOrdinal("IsComplete")),
                                Tot = reader.GetInt32(reader.GetOrdinal("Tot")),
                                ActionId = reader.GetInt32(reader.GetOrdinal("ActionId"))
                            });
                        }

                        // Move to 4th result set => Dashboard headers
                        await reader.NextResultAsync();
                        db.DTODashboardHeaderlst = new List<DTODashboardHeader>();
                        while (await reader.ReadAsync())
                        {
                            db.DTODashboardHeaderlst.Add(new DTODashboardHeader
                            {
                                StageId = reader.GetInt32(reader.GetOrdinal("StageId")),
                                StatusId = reader.GetInt32(reader.GetOrdinal("StatusId")),
                                Status = reader.GetString(reader.GetOrdinal("Status")),
                                Stages = reader.GetString(reader.GetOrdinal("Stages")),
                                Icons = reader.IsDBNull(reader.GetOrdinal("Icons")) ? null : reader.GetString(reader.GetOrdinal("Icons")),
                                Statseq = reader.GetInt32(reader.GetOrdinal("Statseq"))
                            });
                        }

                        // Move to 5th result set => Approved counts
                        await reader.NextResultAsync();
                        var approvedList = new List<DTOApprovedCount>();
                        while (await reader.ReadAsync())
                        {
                            approvedList.Add(new DTOApprovedCount
                            {
                                ProjId = reader.GetInt32(reader.GetOrdinal("ProjId")),
                                StatusId = reader.GetInt32(reader.GetOrdinal("StatusId")),
                                StatusActionsMappingId = reader.GetInt32(reader.GetOrdinal("StatusActionsMappingId")),
                                Total = reader.GetInt32(reader.GetOrdinal("Total"))
                            });
                        }

                        // Group approved counts by StatusId and StatusActionsMappingId as in your original logic
                        db.DTOApprovedCountlst = approvedList
                            .GroupBy(p => new { p.StatusId, p.StatusActionsMappingId })
                            .Select(g => new DTOApprovedCount
                            {
                                StatusId = g.Key.StatusId,
                                StatusActionsMappingId = g.Key.StatusActionsMappingId,
                                Total = g.Sum(x => x.Total)
                            }).ToList();

                        // Move to 6th result set => BISAG-N count
                        await reader.NextResultAsync();
                        while (await reader.ReadAsync())
                        {
                            db.DTOApprovedCountlst.Add(new DTOApprovedCount
                            {
                                StatusId = reader.GetInt32(reader.GetOrdinal("StatusId")),
                                Status = reader.GetString(reader.GetOrdinal("Status")),
                                StatusActionsMappingId = reader.GetInt32(reader.GetOrdinal("StatusActionsMappingId")),
                                Total = reader.GetInt32(reader.GetOrdinal("Total")),
                                ProjId = 0
                            });
                        }

                        // Move to 7th result set => Re-Vetting count
                        await reader.NextResultAsync();
                        while (await reader.ReadAsync())
                        {
                            db.DTOApprovedCountlst.Add(new DTOApprovedCount
                            {
                                StatusId = reader.GetInt32(reader.GetOrdinal("StatusId")),
                                Status = reader.GetString(reader.GetOrdinal("Status")),
                                StatusActionsMappingId = reader.GetInt32(reader.GetOrdinal("StatusActionsMappingId")),
                                Total = reader.GetInt32(reader.GetOrdinal("Total")),
                                ProjId = 0
                            });
                        }
                    }
                }
            }

            db.DTODashboardCountlst = db.DTODashboardCountlst.OrderBy(x => x.StagesId).ThenBy(x => x.StatusId).ToList();
            db.DTODashboardHeaderlst = db.DTODashboardHeaderlst.OrderBy(x => x.Statseq).ThenBy(x => x.StageId).ToList();


            #endregion

            return db;
        }

        public async Task<bool> CheckFwdCondition(int ProjId, int StatusId)
        {
            if (StatusId != 1)
            {
                //select act.StatusActionsMappingId,sts.StatusId,sts.Status from TrnStatusActionsMapping act
                //inner join mStatus sts on act.StatusId=sts.StatusId
                //inner join ProjStakeHolderMov mov on mov.StatusActionsMappingId=act.StatusActionsMappingId
                //where act.ActionsId=2 and act.StatusId=20 and mov.ProjId=1
                var ret = await (from act in _dbContext.TrnStatusActionsMapping
                                 join sts in _dbContext.mStatus on act.StatusId equals sts.StatusId
                                 join mov in _dbContext.ProjStakeHolderMov on act.StatusActionsMappingId equals mov.StatusActionsMappingId
                                 where act.ActionsId == 2 && act.StatusId == StatusId && mov.ProjId == ProjId
                                 && mov.IsActive == true && mov.UndoRemarks == null
                                 select new TrnStatusActionsMapping
                                 {
                                     StatusActionsMappingId = act.StatusActionsMappingId
                                 }).FirstOrDefaultAsync();
                if (ret != null)
                {
                    return true;
                }
                return false;
            }
            else
            {
                var ret = await (from act in _dbContext.TrnStatusActionsMapping
                                 join sts in _dbContext.mStatus on act.StatusId equals sts.StatusId
                                 join mov in _dbContext.ProjStakeHolderMov on act.StatusActionsMappingId equals mov.StatusActionsMappingId
                                 where act.ActionsId == 1 && mov.ProjId == ProjId && mov.ToUnitId == 1 && mov.IsComment == true
                                 && mov.IsActive == true && mov.UndoRemarks == null
                                 select new TrnStatusActionsMapping
                                 {
                                     StatusActionsMappingId = act.StatusActionsMappingId
                                 }).FirstOrDefaultAsync();
                if (ret != null)
                {
                    return true;
                }
                return false;
            }
        }




        //public async Task<int> IsReadInbox(int psmId)
        //{

        //    var _person = new tbl_ProjStakeHolderMov() {PsmId=psmId, IsRead=true};


        //    _context.ProjStakeHolderMov.Attach(_person);
        //    _context.Entry(_person).Property(X => X.PsmId).IsModified = true;
        //    _context.SaveChanges();
        //    return 1;
        //}
        ///Created and Reviewed by : Sub Maj M Sanal Kumar
        // Reviewed Date : 15,16 & 31 Jul 23
        // Reviewed Date : 27 Aug 23, 02,03, 09 & 10 Oct 23  ---   Flags Error Rectified
        //  Data Transfer Error Rectified...   Get Request error by remove antifurg....  carried out
        //  outside user access error rectified
        //   model validation removed















        ///Created and Reviewed by : Sub Maj M Sanal Kumar
        // Reviewed Date : 17 Nov 23





        ///Created and Reviewed by : Sub Maj M Sanal Kumar
        // Reviewed Date : 31 Jul 23
        public async Task<tbl_ProjStakeHolderMov> GetProjStakeHolderMovByIdAsync(int psmId)
        {
            return await _dbContext.ProjStakeHolderMov.FindAsync(psmId);
        }
        ///Created and Reviewed by : Sub Maj M Sanal Kumar
        // Reviewed Date : 31 Jul 23
        public async Task<List<tbl_ProjStakeHolderMov>> GetAllProjStakeHolderMovAsync()
        {
            return await _dbContext.ProjStakeHolderMov.ToListAsync();
        }
        ///Created and Reviewed by : Sub Maj M Sanal Kumar
        // Reviewed Date : 31 Jul 23
        public async Task<bool> UpdateProjStakeHolderMovAsync(tbl_ProjStakeHolderMov projStakeHolderMov)
        {
            _dbContext.Entry(projStakeHolderMov).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
            return true;
        }
        ///Created and Reviewed by : Sub Maj M Sanal Kumar
        // Reviewed Date : 31 Jul 23
        public async Task<bool> DeleteProjStakeHolderMovAsync(int psmId)
        {
            var projStakeHolderMov = await _dbContext.ProjStakeHolderMov.FindAsync(psmId);
            if (projStakeHolderMov == null)
                return false;

            _dbContext.ProjStakeHolderMov.Remove(projStakeHolderMov);
            await _dbContext.SaveChangesAsync();
            return true;
        }





        // added by Sub Maj Sanal on 21 Nov 23
        public async Task<int> ValStatusAsync(int? ProjId)
        {
            int? maxStatusId = _dbContext.ProjStakeHolderMov
    .Where(p => p.ProjId == ProjId)
    .Select(p => (int?)p.StatusActionsMappingId)
    .Max();

            int result = maxStatusId ?? 0;

            return result;
        }

        public async Task<int> AddProjStakeHolderMovAsync(tbl_ProjStakeHolderMov projmove)
        {
            _dbContext.ProjStakeHolderMov.Add(projmove);
            return await _dbContext.SaveChangesAsync();

        }



        public Task<int> CountinboxAsync(int stkhol)
        {
            throw new NotImplementedException();
        }

        public Task<int> AddProStkMovBlogAsync(Projmove psmove)
        {
            throw new NotImplementedException();
        }

        public async Task<int> GetlaststageId(int? ProjId)
        {
            int? maxStatusId = _dbContext.ProjStakeHolderMov
           .Where(p => p.ProjId == ProjId)
            .Select(p => (int?)p.StatusActionsMappingId)
           .Max();

            int result = maxStatusId ?? 0;

            return result;
        }

        public Task<int> ReturnDuplProjMovAsync(Projmove psmove)
        {
            throw new NotImplementedException();
        }

        public Task<int> RetWithObsnMovAsync(Projmove psmove)
        {
            throw new NotImplementedException();
        }

        public async Task<List<ProjLogView>> GetProjLogviewAsync(string startDate, string endDate)
        {
            List<ProjLogView> plvew;

            try
            {
                #region GetProjLogviewAsyncWithLinq

                //var result = from b in _dbContext.ProjStakeHolderMov
                //             join c in _dbContext.Projects on b.ProjId equals c.ProjId
                //             join a in _dbContext.TrnStatusActionsMapping on b.StatusActionsMappingId equals a.StatusActionsMappingId

                //             join e in _dbContext.mStatus on a.StatusId equals e.StatusId
                //             join d in _dbContext.mStages on e.StageId equals d.StagesId
                //             join f in _dbContext.mActions on a.ActionsId equals f.ActionsId
                //             join g in _dbContext.tbl_mUnitBranch on b.ToUnitId equals g.unitid
                //             join h in _dbContext.tbl_mUnitBranch on b.FromUnitId equals h.unitid
                //             join j in _dbContext.Comment on b.PsmId equals j.PsmId into commentGroup
                //             from j in commentGroup.DefaultIfEmpty()
                //             join k in _dbContext.tbl_mUnitBranch on c.StakeHolderId equals k.unitid
                //             where b.TimeStamp >= DateTime.Parse(startDate) &&
                //                   b.TimeStamp <= DateTime.Parse(endDate)
                //             orderby b.TimeStamp descending
                //             select new
                //             {
                //                 b.PsmId,
                //                 b.ProjId,
                //                 c.ProjName,
                //                 k.UnitName,
                //                 d.Stages,
                //                 e.Status,
                //                 f.Actions,
                //                 b.TimeStamp,
                //                 FwdBy = h.UnitName,
                //                 FwdTo = g.UnitName,
                //                 j.Comment,
                //                 AttDocu = string.Join(", ", _dbContext.AttHistory
                //                                            .Where(a => a.PsmId == b.PsmId)
                //                                            .Select(a => $"Desc: {a.Reamarks} : Docu: {a.ActFileName}")),
                //                 Comments = string.Join(", ", _dbContext.StkComment
                //                                            .Where(sc => sc.PsmId == b.PsmId)
                //                                            .Select(sc => $"{k.UnitName} Desc: {sc.Comments} : Docu: {sc.ActFileName}")),
                //                 b.Remarks,
                //                 b.UpdatedByUserId,
                //                 EncyId = _dataProtector.Protect(b.PsmId.ToString())
                //             };

                //// Assuming ProjLogView has the necessary properties
                //var projLogViews = result.ToList().Select(x => new ProjLogView
                //{
                //    PsmId = x.PsmId,
                //    ProjId = x.ProjId,
                //    ProjName = x.ProjName,
                //    UnitName = x.UnitName,
                //    Stages = x.Stages,
                //    Status = x.Status,
                //    Actions = x.Actions,
                //    TimeStamp = x.TimeStamp,
                //    FwdBy = x.FwdBy,
                //    FwdTo = x.FwdTo,
                //    Comment = x.Comment,
                //    AttDocu = x.AttDocu,
                //    Comments = x.Comments,
                //    AddRemarks = x.Remarks,
                //    ActionByUser = (int)x.UpdatedByUserId,
                //    EncyId = x.EncyId
                //}).ToList();

                //return projLogViews;
                #endregion

                #region GetProjLogviewAsyncWithProc
                List<ProjLogView> resultList = new List<ProjLogView>();

                using (SqlConnection conn = new SqlConnection(_dbContext.Database.GetConnectionString()))
                {
                    using (SqlCommand cmd = new SqlCommand("usp_GetProjLogView", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@StartDate", DateTime.Parse(startDate));
                        cmd.Parameters.AddWithValue("@EndDate", DateTime.Parse(endDate));

                        await conn.OpenAsync();
                        using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                ProjLogView view = new ProjLogView
                                {
                                    PsmId = Convert.ToInt32(reader["PsmId"]),
                                    ProjId = Convert.ToInt32(reader["ProjId"]),
                                    ProjName = reader["ProjName"].ToString(),
                                    UnitName = reader["UnitName"].ToString(),
                                    Stages = reader["Stages"].ToString(),
                                    Status = reader["Status"].ToString(),
                                    Actions = reader["Actions"].ToString(),
                                    TimeStamp = Convert.ToDateTime(reader["TimeStamp"]),
                                    FwdBy = reader["FwdBy"].ToString(),
                                    FwdTo = reader["FwdTo"].ToString(),
                                    Comment = reader["Comment"]?.ToString(),
                                    AttDocu = reader["AttDocu"]?.ToString(),
                                    Comments = reader["Comments"]?.ToString(),
                                    AddRemarks = reader["Remarks"]?.ToString(),
                                    ActionByUser = Convert.ToInt32(reader["UpdatedByUserId"]),
                                };

                                view.EncyId = _dataProtector.Protect(view.PsmId.ToString());

                                resultList.Add(view);
                            }
                        }
                    }
                }
                return resultList;

                #endregion
            }
            catch (Exception ex)
            {
                plvew = new List<ProjLogView>();
            }
            return plvew;

        }

        public Task<int> UpdateUndoProjectMov(int ProjectId, int PsmId)
        {
            throw new NotImplementedException();
        }


        public string GetSponsorUnitName(int StakeHolderId)
        {
            try
            {
                var unitName = _context.tbl_mUnitBranch
                                       .Where(i => i.unitid == StakeHolderId)
                                       .Select(i => i.UnitName)
                                       .FirstOrDefault();

                return unitName;
            }
            catch (Exception ex)
            {
                // Log the exception (ex) if necessary
                return null;
            }
        }


        public async Task<int> GetProjectId(string? ProjName)
        {
            //var data = from a in _dbContext.Projects where a.ProjName == ProjName
            //           select a.ProjId;
            //int data1 = Convert.ToInt32(data);
            //return data1;

            int? ProjId = _dbContext.Projects
           .Where(p => p.ProjName == ProjName)
            .Select(p => (int?)p.ProjId)
           .Max();

            int result = ProjId ?? 0;
            return result;
        }



        public async Task<int> AddNotificationCommentAsync(Notification notifications)
        {
            _dbContext.Notification.Add(notifications);
            return await _dbContext.SaveChangesAsync();

        }
    }
}
