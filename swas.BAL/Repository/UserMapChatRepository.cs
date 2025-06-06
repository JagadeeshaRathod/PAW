using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
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
    public class UserMapChatRepository:GenericRepositoryDL<mUserMapChat>, IUserMapChatRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserMapChatRepository(ApplicationDbContext context) : base(context)
        {
            _dbContext = context;

        }

        public async Task<mUserMapChat> GetMapDetails(mUserMapChat mUserMapChat)
        {
            return await _dbContext.mUserMapChat.Where(i => i.FromUserId == mUserMapChat.FromUserId && i.ToUserId == mUserMapChat.ToUserId).SingleOrDefaultAsync();
        }
    }
}
