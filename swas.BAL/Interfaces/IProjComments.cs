using swas.BAL.DTO;
using swas.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace swas.BAL.Interfaces
{
    public interface IProjComments
    {
        Task<List<DTOProComments>> GetAllStkForComment(int UnitId);
        Task<DTOProComments> GetCommentStatus(int UnitId);
    }
}
