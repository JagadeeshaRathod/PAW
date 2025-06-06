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
    public class StkStatusRepository : GenericRepositoryDL<StkStatus>, IStkStatusRepository
    {
        public StkStatusRepository(ApplicationDbContext context) : base(context)
        {

        }

      
    }
}
