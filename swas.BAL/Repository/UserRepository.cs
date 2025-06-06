using Microsoft.IdentityModel.Tokens;
using swas.DAL.Models;
using swas.BAL.Utility;
using swas.BAL.Interfaces;
using swas.DAL;
using Microsoft.EntityFrameworkCore;
using ASPNetCoreIdentityCustomFields.Data;

namespace swas.BAL
{
    ///Created and Reviewed by : Sub Maj Sanal
    ///Created Date : 14 Aug 23
    ///Tested By :- 
    ///Tested Date : 
    ///Start
    public class UserRepository : IUserRepository
    {
        public readonly ApplicationDbContext _dbContext;
       

        public UserRepository(ApplicationDbContext context)
        {
            _dbContext = context;

        }

        public bool CheckUserExist(string UserName)
        {
            return _dbContext.Users.Any(e => e.UserName == UserName);
            
        }


        //public async Task<int> DeleteUserAsync(tbl_users user)
        //{
        //    var userToDelete = await _dbContext.Users.FindAsync(user.UserName);

        //    if (userToDelete != null)
        //    {
        //        _dbContext.Users.Remove(userToDelete);
        //        await _dbContext.SaveChangesAsync();
        //        return 1; 
        //    }

        //    return 0; 
        //}
       

      
        public async Task<List<ApplicationUser>> GetAllUsersAsync()
        {
            var users = await _dbContext.Users.ToListAsync();
            return users;
        }

        //public async Task<int> Save(tbl_users Db)
        //{

        //    var userToAdd = await _dbContext.Users.FindAsync(Db.UserName);

        //    if (userToAdd != null)
        //    {
        //        _dbContext.Users.Add(userToAdd);
        //        await _dbContext.SaveChangesAsync();
        //        return 1;
        //    }

        //    return 0;
            
        //}


        public async Task<int> Add(tbl_LoginLog userlog)
        {
            //if (userlog != null)
            //{
                _dbContext.tbl_LoginLog.Add(userlog);
                await _dbContext.SaveChangesAsync();
                return 1;
            //}

            //return 0;
        }
    }
}
