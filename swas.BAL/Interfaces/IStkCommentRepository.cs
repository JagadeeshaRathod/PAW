using swas.BAL.DTO;
using swas.BAL.Repository;
using swas.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace swas.BAL.Interfaces
{
    public interface IStkCommentRepository : IGenericRepositoryDL<StkComment>
    {
        Task<List<DTOProComments>> GetAllCommentBypsmId_UnitId(StkComment Data);

        Task<int> GetCommentStatusByPsmiId(int psmid);
        int IsAllowForCommentByStkStatusId(int stkStatusId);


    }
}
