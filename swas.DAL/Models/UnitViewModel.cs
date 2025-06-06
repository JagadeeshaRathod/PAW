using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace swas.DAL.Models
{
    public class UnitViewModel
    {
        public List<UnitDtl> UnitDetails { get; set; }
        public List<DTOUnitMapping> UnitMappings { get; set; }

        public class DTOUnitMapping
        {
            public int StatusActionsMappingId { get; set; }
            public int UnitStatusMappingId { get; set; }
            public int Id { get; set; }
            public int UnitId { get; set; }
            public string? Unit { get; set; }
            public string? StagesName { get; set; }
            public int Stages { get; set; }
            public string? SubStagesName { get; set; }
            public int SubStages { get; set; }
            public int Actions { get; set; }
            public string? ActionsName { get; set; }

        }
    }
}
