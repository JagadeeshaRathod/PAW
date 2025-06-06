using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace swas.BAL.DTO
{
    public class DTOUnderProcessProj
    {
        public int ProjId { get; set; }

        public string ProjName { get; set; }
        public string FromUnitName { get; set; }
        public string ToUnitName { get; set; }

        public string Sponser { get; set; }
        public string StatusName { get; set; }
        public string ActionName { get; set; }

        public string StageName { get; set; }
        public int FromUnitId { get; set; }
        public int ToUnitId { get; set; }
        public int StatusId { get; set; }
        public int ActionId { get; set; }
        public string UserDetails { get; set; }
        public DateTime? DateTimeOfUpdate { get; set; }

        public string UnitName { get; set; }
        public string FromUnitUserDetail { get; set; }
        public string Remark { get; set; }



    }
}
