using swas.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace swas.BAL.Interfaces
{
    public interface IUserMapChatRepository : IGenericRepositoryDL<mUserMapChat>
    {
        public Task<mUserMapChat> GetMapDetails(mUserMapChat mUserMapChat);
    }
}
