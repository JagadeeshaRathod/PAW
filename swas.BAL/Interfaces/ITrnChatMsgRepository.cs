using swas.BAL.DTO;
using swas.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace swas.BAL.Interfaces
{
    public interface ITrnChatMsgRepository : IGenericRepositoryDL<TrnChatMsg>
    {
        public Task<List<TrnChatMsg>> GetChat(int UserMapChatId, string FromUserId, string ToUserId);

        public Task<List<DTOIsChat>> GetIsChat(string ToUserId);
        public Task<List<DTOTrnChatOrderby>> GetAllUserLastchatDateFororderBy();
        public Task<List<DTOApplicationUserWithChatRead>> GetAllUsers();
    }
}
