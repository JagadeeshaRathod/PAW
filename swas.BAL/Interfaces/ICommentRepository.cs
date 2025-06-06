using swas.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace swas.BAL.Interfaces
{
    ///Created and Reviewed by : Sub Maj Sanal
    ///Reviewed Date : 31 Jul 23
    ///Tested By :- 
    ///Tested Date : 
    ///Start
    public interface ICommentRepository
    {
        Task<int> AddCommentAsync(tbl_Comment comment);
        Task<tbl_Comment> GetCommentByIdAsync(int commentId);
        Task<List<tbl_Comment>> GetAllCommentsAsync();
        Task<bool> UpdateCommentAsync(tbl_Comment comment);
        Task<bool> DeleteCommentAsync(int commentId);
        Task<tbl_Comment> GetCommentByPsmIdAsync(int? PsmId);

        Task<int> CountCommentAsync(int stkhol);

        //Task<List<Notification>> GetNotificationAsync(int ProjId);
        Task<List<Notification>> GetNotificationInbox (int ProjId);
        //Task<int> GetNotificationCommentCount();
        Task<int> GetNotificationInboxCount();
        //
    }

}
