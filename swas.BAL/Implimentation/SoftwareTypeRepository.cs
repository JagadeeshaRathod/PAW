using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using swas.DAL;
using swas.BAL.Interfaces;

namespace swas.BAL
{
    public class SoftwareTypeRepository : ISoftwareTypeRepository
    {
        private readonly ApplicationDbContext _context;

        public SoftwareTypeRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        //public async Task<IEnumerable<SoftwareType>> GetAll()
        //{
        //    var Software = await _context.SoftwareTypes.ToListAsync();
        //    return Software;
        //    //throw new NotImplementedException();
        //}

        //public Task<SoftwareType> GetById(int id)
        //{
        //    throw new NotImplementedException();
        //}
        //public  Task Add(SoftwareType model)
        //{

        //    throw new NotImplementedException();
        //}

        //public Task Update(SoftwareType model)
        //{
        //    throw new NotImplementedException();
        //}
        public Task Delete(int id)
        {
            throw new NotImplementedException();
        }

      
    }
}
