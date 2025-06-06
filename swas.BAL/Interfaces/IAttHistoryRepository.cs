using swas.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace swas.BAL.Interfaces
{
    public interface IAttHistoryRepository:IGenericRepositoryDL<tbl_AttHistory>
    {

        ///Created and Reviewed by : Sub Maj Sanal
        ///Reviewed Date : 31 Jul 23
        ///Tested By :- 
        ///Tested Date : 
        ///Start
        Task<int> AddAttHistoryAsync(tbl_AttHistory attHistory);
        
        Task<List<tbl_AttHistory>> GetAttHistoryByIdAsync(int? psmid);

        Task<List<tbl_AttHistory>> GetAttHistDelIdAsync(int? psmid);
                

        Task<tbl_AttHistory> GetAttHistByIdAsync(int? psmid);

        Task<List<tbl_AttHistory>> GetAllAttHistoriesAsync();

        
        Task<bool> UpdateAttHistoryAsync(tbl_AttHistory attHistory);

        Task<bool> DeleteAttHistoryAsync(int attId);
    }
}
