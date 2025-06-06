using Microsoft.AspNetCore.DataProtection;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using swas.BAL.DTO;
using swas.BAL.Interfaces;
using swas.DAL;
using swas.DAL.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace swas.BAL
{
    public class ProjComments : IProjComments
    {
        private readonly ApplicationDbContext _context;
        private readonly IDataProtector _dataProtector;
        public ProjComments(ApplicationDbContext applicationDbContext, IDataProtectionProvider dataProtector)
        {
            _context = applicationDbContext;
            _dataProtector = dataProtector.CreateProtector("swas.UI.Controllers.ProjectsController"); 
        }
        public async Task<List<DTOProComments>> GetAllStkForComment(int UnitId)
         {
            #region GetAllStkForCommentWithLinq

            //var queryforstackholderself = from a in _context.Projects
            //                              join b in _context.ProjStakeHolderMov on a.ProjId equals b.ProjId

            //                              where b.IsComment == true
            //                              && a.StakeHolderId == b.ToUnitId
            //                              group b by new { b.ToUnitId, a.ProjId } into grp
            //                              where grp.Count() > 1
            //                              select new DTOForStackHolderCout
            //                              {
            //                                  ProjId = grp.Key.ProjId,
            //                                  UnitId = grp.Key.ToUnitId
            //                              };


            //var maxpsmid = await (from a in _context.ProjStakeHolderMov
            //                      join b in queryforstackholderself on a.ProjId equals b.ProjId
            //                      where a.IsComment == true
            //                      select new DTOForStackHolderCout
            //                      {
            //                          ProjId = b.ProjId,
            //                          PsmId = a.PsmId,
            //                      }).ToListAsync();

            //var lastpsmiid = maxpsmid.GroupBy(x => x.ProjId).Select(x => x.OrderByDescending(a => a.PsmId).FirstOrDefault()).ToList();



            //List<DTOProComments> lst = new List<DTOProComments>();
            //var queryes = await (from proj in _context.Projects
            //                     join mov in _context.ProjStakeHolderMov on proj.ProjId equals mov.ProjId
            //                     //join com in _context.StkComment on proj.ProjId equals com.ProjId
            //                     join stakeholder in _context.tbl_mUnitBranch on proj.StakeHolderId equals stakeholder.unitid
            //                     let StkStatusId =
            //                 (from cr1 in _context.StkComment
            //                  join Stdkst in _context.StkStatus on cr1.StkStatusId equals Stdkst.StkStatusId
            //                  where cr1.StakeHolderId == mov.ToUnitId && cr1.PsmId == mov.PsmId
            //                  orderby cr1.StkCommentId descending
            //                  select cr1.StkStatusId
            //                 ).FirstOrDefault()/*.Count()*/
            //                     where mov.ToUnitId == UnitId && mov.IsComment == true //&& mov.StatusId==5

            //                     select new DTOProComments
            //                     {
            //                         ProjectName = proj.ProjName,
            //                         Stakeholder = stakeholder.UnitName,
            //                         Status = "",
            //                         StkStatusId = Convert.ToInt32(StkStatusId),
            //                         //StkStatusId = comment.StkStatusId,
            //                         ProjId = proj.ProjId,
            //                         PsmId = mov.PsmId,
            //                         EncyID = _dataProtector.Protect(proj.ProjId.ToString()),
            //                         //TimeStamp = _context.StkComment.Where(i => i.StkStatusId == StkStatusId).Select(i => i.DateTimeOfUpdate).SingleOrDefault()?? mov.TimeStamp,
            //                         TimeStamp = mov.TimeStamp,
            //                         UnitId = mov.ToUnitId,
            //                         IsComment = mov.IsRead,
            //                         //StkCommentId = com.StkCommentId
            //                          AdminApprovalStatus = _context.DateApproval.Any(da => da.ProjId == proj.ProjId && da.UnitId == mov.ToUnitId && da.DDGIT_approval == true)

            //                     }
            //                     // Group by both ProjId and ToUnitId  /////old  by new { proj.ProjId, mov.ToUnitId } into g 
            //                     ).ToListAsync();

            ////var data = lst.OrderByDescending(x => x.StkCommentId).ToList();

            //lst.AddRange(queryes);

            //lst.RemoveAll(item => lastpsmiid.Any(item2 => item.PsmId == item2.PsmId));

            //return lst.OrderByDescending(i => i.TimeStamp).ToList();
            ////return data;

            #endregion

            #region GetAllStkForCommentWithProc

            List<DTOProComments> results = new List<DTOProComments>();

            using (SqlConnection conn = new SqlConnection(_context.Database.GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand("usp_GetAllStkForComment", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@UnitId", UnitId);

                    await conn.OpenAsync();
                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            DTOProComments item = new DTOProComments
                            {
                                ProjId = Convert.ToInt32(reader["ProjId"]),
                                PsmId = Convert.ToInt32(reader["PsmId"]),
                                ProjectName = reader["ProjectName"].ToString(),
                                Stakeholder = reader["Stakeholder"].ToString(),
                                Status = reader["Status"].ToString(),
                                StkStatusId = Convert.ToInt32(reader["StkStatusId"]),
                                UnitId = Convert.ToInt32(reader["UnitId"]),
                                TimeStamp = Convert.ToDateTime(reader["TimeStamp"]),
                                IsComment = Convert.ToBoolean(reader["IsComment"]),
                                AdminApprovalStatus = Convert.ToBoolean(reader["AdminApprovalStatus"])
                            };

                            item.EncyID = _dataProtector.Protect(item.ProjId.ToString());
                            results.Add(item);
                        }
                    }
                }
            }
            return results.OrderByDescending(i => i.TimeStamp).ToList();

            #endregion
        }



        public async Task<DTOProComments> GetCommentStatus(int UnitId)
        {
            var tempFileName = await (from comment in _context.StkComment
                                      join status in _context.StkStatus on comment.StkStatusId equals status.StkStatusId into statusGroup
                                      from status in statusGroup.DefaultIfEmpty()
                                      where comment.StakeHolderId == UnitId
                                      select new DTOProComments
                                      {
                                          Status = status.Status,
                                          StkStatusId = comment.StkStatusId,
                                      }).OrderByDescending(c => c.StkCommentId).SingleOrDefaultAsync();
            // Where(comment.StakeHolderId == UnitId)
            //.OrderByDescending(x => x.StkCommentId)
            //.Take(1)
            //.Select(x => x.StkStatusId)
            //.ToList()
            //.FirstOrDefault();
            return null;
        }
    }
}
