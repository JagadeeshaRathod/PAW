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
    public class ChatRepository: IChatRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public ChatRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<mUserMapChat> GetChatMapId(string FromUserId, string ToUserId)
        {

            var ret = await _dbContext.mUserMapChat.Where(i => i.FromUserId == FromUserId && i.ToUserId == ToUserId).SingleOrDefaultAsync();
            return ret;

        }
        public async Task<List<TrnChatMsg>> GetChatbyUserMapChat(int UserMapChatId)
        {
           
            var ret = await _dbContext.TrnChatMsg.Where(i=>i.UserMapChatId==UserMapChatId).ToListAsync();
            return ret;
        }

    }
}
