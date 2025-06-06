using swas.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace swas.BAL.Interfaces
{
    public interface IChatRepository
    {
        public Task<mUserMapChat> GetChatMapId(string FromUserId, string ToUserId);
        public Task<List<TrnChatMsg>> GetChatbyUserMapChat(int UserMapChatId);
    }
}
