using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using swas.DAL;
using swas.DAL.Models;

namespace swas.BAL.Interfaces
{
    public interface IStakeHolderRepository
    {
        Task<int> AddStakeHolderAsync(tbl_mStakeHolder stakeHolder);
        Task<tbl_mStakeHolder> GetStakeHolderByIdAsync(int stakeHolderId);
        Task<List<tbl_mStakeHolder>> GetAllStakeHoldersAsync();
        Task<bool> UpdateStakeHolderAsync(tbl_mStakeHolder stakeHolder);
        Task<bool> DeleteStakeHolderAsync(int stakeHolderId);
    }

}
