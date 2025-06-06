using Microsoft.EntityFrameworkCore;
using swas.BAL.DTO;
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
    public class UnitStatusMapping :GenericRepositoryDL<TrnUnitStatusMapping>, IUnitStatusMapping
    {
        private readonly ApplicationDbContext _dbContext;
        public UnitStatusMapping(ApplicationDbContext context) : base(context)
        {
            _dbContext = context;
        }

        public async Task<List<TrnUnitStatusMapping>> GetAllAsyn()
        {

            
          var unitstatus= await _dbContext.TrnUnitStatusMapping.ToListAsync();

            return unitstatus;

        }

       

      

        public async Task<TrnUnitStatusMapping> GetByUnitAndStatusAsync(int UnitStatusMappingId,int unitid, int statusid)
        {
            //if (UnitStatusMappingId > 0)
            //{
            //    return await _context.TrnUnitStatusMapping
            //   .FirstOrDefaultAsync(usm => usm.UnitId == unitid && usm.StatusId == statusid && usm.UnitStatusMappingId == UnitStatusMappingId);
            //}
            //else
            //{
            //    return await _context.TrnUnitStatusMapping
            //                   .FirstOrDefaultAsync(usm => usm.UnitId == unitid && usm.StatusId == statusid );
            //}
            return null;
            
        }
    }
}
