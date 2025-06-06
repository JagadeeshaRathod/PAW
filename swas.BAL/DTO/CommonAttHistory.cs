using swas.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace swas.BAL.DTO
{
    public class CommonAttHistory
    {
        public tbl_AttHistoryDTO Tbl_AttHistoryDTO { get; set; }

        public List<ProjHistory> ProjHistories { get; set; }

       
    }
}
