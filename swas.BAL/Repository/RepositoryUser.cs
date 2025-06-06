using ASPNetCoreIdentityCustomFields.Data;
using Microsoft.AspNetCore.Identity;
using swas.DAL.Models;
using swas.BAL.Utility;
using swas.DAL;
using Microsoft.EntityFrameworkCore;

namespace swas.BAL
{
    ///Created and Reviewed by : Sub Maj Sanal
    ///Reviewed Date : 30 Jul 23
    ///Tested By :- 
    ///Tested Date : 
    ///Start
    public class RepositoryUser
    {
        public readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public RepositoryUser(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;

        }

        ///Created and Reviewed by : Sub Maj Sanal
        ///Reviewed Date : 30 Jul 23
        ///Tested By :- 
        ///Tested Date : 
        ///Start
        //public async Task<int> Save(Users Db)
        //{

        //    Db.Password = Utility.Security.GetHashString(Db.Password);
        //    Db.IsActive = 1;



        //    if (!CheckUserExist(Db.UserName, Db.Id))
        //    {

        //        _context.Add(Db);
        //        _context.SaveChanges();


        //        return Convert.ToInt32(EnumHelper.SaveData.Save);
        //    }

        //    else
        //    {
        //        //_context.Update(Db);
        //        //await _context.SaveChangesAsync();
        //        return Convert.ToInt32(EnumHelper.SaveData.Duplicate);
        //    }



        //}
        /////Created and Reviewed by : Sub Maj Sanal
        /////Reviewed Date : 30 Jul 23
        /////Tested By :- 
        /////Tested Date : 
        /////Start
        //public int ChnagePassword(Users Db)
        //{
        //    var query =
        //    from ord in _context.tbl_users
        //    where ord.Id == Db.Id
        //    select ord;

        //    // Execute the query, and change the column values
        //    // you want to change.
        //    foreach (Users ord in query)
        //    {

        //        ord.Password = Db.Password;
        //        // Insert any additional changes to column values.
        //    }

        //    // Submit the changes to the database.
        //    try
        //    {
        //        _context.SaveChanges();
        //        return 1;
        //    }
        //    catch (Exception e)
        //    {
        //        return 2;
        //        Console.WriteLine(e);
        //        // Provide for exceptions.
        //    }
        //}
        /////Created and Reviewed by : Sub Maj Sanal
        /////Reviewed Date : 30 Jul 23
        /////Tested By :- 
        /////Tested Date : 
        /////Start
        //public bool CheckUserExist(string UserName, int Id)
        //{

        //    return _context.tbl_users.Any(e => e.UserName == UserName && e.Id != Id && e.IsActive == 1);

        //}
        /////Created and Reviewed by : Sub Maj Sanal
        /////Reviewed Date : 30 Jul 23
        /////Tested By :- 
        /////Tested Date : 
        /////Start
        //public Users GetById(int Id)
        //{
        //    var AllData = _context.tbl_users.Where(i => i.Id == Id).Single();


        //    return AllData;
        //}
        /////Created and Reviewed by : Sub Maj Sanal
        /////Reviewed Date : 30 Jul 23
        /////Tested By :- 
        /////Tested Date : 
        /////Start
        //public Users GetByIdAll(string Id)
        //{
        //    var AllData = _context.tbl_users.Where(i => i.UserName == Id).Single();


        //    return AllData;
        //}

        /////Created and Reviewed by : Sub Maj Sanal
        /////Reviewed Date : 30 Jul 23
        /////Tested By :- 
        /////Tested Date : 
        /////Start
        //public List<Users> GetByIdAll(int ComponentId)
        //{
        //    var AllData = _context.tbl_users.Where(i => i.IsActive == 1 && i.Id != 1).ToList();


        //    return AllData;
        //}
        //public List<Role> GetAllRoles()
        //{
        //    // 
        //    var AllData = _context.tbl_Types.Where(i => i.IsActive == 1).ToList();
        //    AllData.Insert(0, new Role { Id = 0, Name = "--Select--" });
        //    return AllData;
        //}
        /////Created and Reviewed by : Sub Maj Sanal
        /////Reviewed Date : 30 Jul 23
        /////Tested By :- 
        /////Tested Date : 
        /////Start
        //public List<UnitDtl> GetAllUnit()
        //{
        //    // ViewBag.AllUnitData
        //    var AllData = _context.tbl_mUnitBranch.ToList();
        //    AllData.Insert(0, new UnitDtl { Id = 0, UnitName = "--Select--" });
        //    return AllData;
        //}


        /////Created and Reviewed by : Sub Maj Sanal
        /////Reviewed Date : 30 Jul 23
        /////Tested By :- 
        /////Tested Date : 
        /////Start

        //public async Task<int> del(Users Db)
        //{
        //    var query =
        //    from ord in _context.tbl_users
        //    where ord.Id == Db.Id
        //    select ord;

        //    // Execute the query, and change the column values
        //    // you want to change.
        //    foreach (Users ord in query)
        //    {

        //        ord.IsActive = 0;
        //        // Insert any additional changes to column values.
        //    }

        //    // Submit the changes to the database.
        //    try
        //    {
        //        _context.SaveChanges();
        //        return Convert.ToInt32(EnumHelper.SaveData.Delete); ;
        //    }
        //    catch (Exception e)
        //    {
        //        return 2;
        //        Console.WriteLine(e);
        //        // Provide for exceptions.
        //    }
        //}

        ///Created and Reviewed by : Sub Maj Sanal
        ///Reviewed Date : 30 Jul 23
        ///Tested By :- 
        ///Tested Date : 
        ///Start

        public List<AddlTask> GetAdlTaskFromUsers(string actualUser)
        {
            
          
            var usersWithCustomField = _userManager.Users.Where(u => u.domain_iam == actualUser).ToList();

            List<AddlTask> unitlist = new List<AddlTask>();
            foreach (var item in usersWithCustomField)
            {
                AddlTask dt = new AddlTask();
                dt.Name = item.UserName;
                dt.Id = item.UserName;
                
                unitlist.Add(dt);
            }

            AddlTask dtS = new AddlTask();
            dtS.Name = actualUser;
            dtS.Id = actualUser;

            unitlist.Add(dtS);

            unitlist.Insert(0, new AddlTask { Id = "", Name = "--Select--" });
           
            return unitlist;
        }

        public int GetRoleId(string rolename)
        {

            Types rle = new Types();
            rle = _context.tbl_Type.FirstOrDefault(a => a.Name == rolename);
            int roleid = rle.Id;
            return roleid;


        }

        public async Task<UnitDtl> GetUnitDtl(int unitid)
        {

            UnitDtl logdet = new UnitDtl();
            logdet = await _context.tbl_mUnitBranch.FirstOrDefaultAsync(a => a.unitid== unitid);

            return logdet;

        }

    }
}
