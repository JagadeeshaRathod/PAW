using swas.BAL.DTO;
using swas.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace swas.BAL.Interfaces
{
    public interface IActionExceptionRepository  : IGenericRepositoryDL<TrnUnitStatusMapping>
    {
        Task<List<ActionExceptionDTO>> GetUnitStatusMapping();

        Task<bool> CheckProjectExists(int UnitId , int ActionId );
    }
}
