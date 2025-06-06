using swas.BAL.DTO;
using swas.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace swas.BAL.Interfaces
{
    public interface IUnitStatusMapping :IGenericRepositoryDL<TrnUnitStatusMapping>
    {

        Task<List<TrnUnitStatusMapping>> GetAllAsyn();

        Task<TrnUnitStatusMapping> GetByUnitAndStatusAsync(int UnitStatusMappingId,int unitid, int statusid);
    }
}
