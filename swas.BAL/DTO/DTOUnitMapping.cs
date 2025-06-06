using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace swas.BAL.DTO
{
    public class DTOUnitMapping
    {
        public int StatusActionsMappingId { get; set; }
        public int UnitStatusMappingId { get; set; }
        public int Id { get; set; }
        public int UnitId { get; set; }
        public string? UnitName { get; set; }
        public string? StagesName { get; set; }
        public int StagesId { get; set; }
        public string? SubStagesName { get; set; }
        public int SubStagesId { get; set;}
        public int ActionsId { get; set; }
        public string? ActionsName { get; set; }

    }
}
