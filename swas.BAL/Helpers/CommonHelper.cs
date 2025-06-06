using ASPNetCoreIdentityCustomFields.Data;
using swas.BAL.DTO;
using swas.BAL.Utility;
using swas.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace swas.BAL.Helpers
{
    public class CommonHelper
    {

        private readonly ApplicationDbContext _dbContext;
        public CommonHelper(ApplicationDbContext context)
        {
            _dbContext = context;
        }


        public string UserRankDetail(ApplicationUser User)
        {
            try
            {
                var rank = Convert.ToInt32(User.Rank);
                var rankName = _dbContext.mRank
                    .Where(x => x.Id == rank)
                    .Select(x => x.RankName)
                    .FirstOrDefault();

                return rankName?.Trim() ?? string.Empty;
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }

        public string GetRankByRankId(int? Rank)
        {
            try
            {
                var rank = Convert.ToInt32(Rank);
                var rankName = _dbContext.mRank
                    .Where(x => x.Id == rank)
                    .Select(x => x.RankName)
                    .FirstOrDefault();

                return rankName?.Trim() ?? string.Empty;
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }
        
    }
}
