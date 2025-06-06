using ASPNetCoreIdentityCustomFields.Data;
using swas.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace swas.BAL.Interfaces
{
    ///Created and Reviewed by : Sub Maj M Sanal
    ///Reviewed Date : 31 Jul 23
    ///Tested By :- 
    ///Tested Date : 
    ///Start
    public interface IUserRepository
    {
        //Task<int> Save(tbl_users Db);
        //bool CheckUserExist(string UserName);
        //Task<int> DeleteUserAsync(tbl_users Db);
        //Task<List<ApplicationUser>> GetAllUsersAsync();

        Task<int> Add(tbl_LoginLog Db);

    }

}
