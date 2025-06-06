using Microsoft.EntityFrameworkCore;
using swas.BAL.Interfaces;
using swas.DAL;
using swas.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace swas.BAL.Repository
{

    ///Created and Reviewed by : Sub Maj Sanal
    ///Reviewed Date : 31 Jul 23
    ///Tested By :- 
    ///Tested Date : 
    ///Start
    public class AttHistoryRepository : GenericRepositoryDL<tbl_AttHistory>, IAttHistoryRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public AttHistoryRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<int> AddAttHistoryAsync(tbl_AttHistory attHistory)
        {
            if (attHistory.Reamarks == null)
            {
                attHistory.Reamarks = "Att File";
            }
            _dbContext.AttHistory.Add(attHistory);
            await _dbContext.SaveChangesAsync();
            return attHistory.AttId;
        }

        ///Created and Reviewed by : Sub Maj Sanal
        ///Reviewed Date : 31 Jul 23
        public async Task<List<tbl_AttHistory>> GetAttHistoryByIdAsync(int? psmid)
        {
            List<tbl_AttHistory> tblhis = new List<tbl_AttHistory>();
            tblhis = await _dbContext.AttHistory.Where(a => a.PsmId == psmid).ToListAsync();
            return tblhis;
        }

        public async Task<List<tbl_AttHistory>> GetAttHistDelIdAsync(int? psmid)
        {
            List<tbl_AttHistory> tblhis = new List<tbl_AttHistory>();
            tblhis = await _dbContext.AttHistory.Where(a => a.AttId == psmid).ToListAsync();
            return tblhis;
        }

        public async Task<tbl_AttHistory> GetAttHistByIdAsync(int? psmid)
        {
            tbl_AttHistory atthis = await _dbContext.AttHistory.Where(a => a.PsmId == psmid).FirstOrDefaultAsync();
            return atthis;
        }

        ///Created and Reviewed by : Sub Maj Sanal
        ///Reviewed Date : 31 Jul 23
        public async Task<List<tbl_AttHistory>> GetAllAttHistoriesAsync()
        {
            return await _dbContext.AttHistory.ToListAsync();
        }
        ///Created and Reviewed by : Sub Maj Sanal
        ///Reviewed Date : 31 Jul 23
        public async Task<bool> UpdateAttHistoryAsync(tbl_AttHistory attHistory)
        {
            _dbContext.Entry(attHistory).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAttHistoryAsync(int attId)
        {
            try
            {
                var attHistory = await _dbContext.AttHistory.FindAsync(attId);
                if (attHistory == null)
                    return false;
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Uploads", attHistory.AttPath);
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
                _dbContext.AttHistory.Remove(attHistory);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }                      
        }
    }
}
