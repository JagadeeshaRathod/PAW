using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace swas.BAL.DTO
{
    public class DTODashboard 
    {
        public List<DTODashboardCount> DTODashboardCountlst { get; set; }
        public List<DTODashboardHeader> DTODashboardHeaderlst { get; set; }
        public List<DTODashboardAction> DTODashboardActionlst { get; set; }
        public List<DTOApprovedCount> DTOApprovedCountlst { get; set; }
        public List<DTODashboardCount> DTODashboardCountlstForAction { get; set; }
    }
    public class DTODashboardCount
    {
        public int StatusId { get; set; }
        public string? Stages { get; set; }
        public int StagesId { get; set; }
        public int ActionId { get; set; }
        public string? Status { get; set; }
        public int Tot { get; set; }
        public bool IsComplete { get; set; }
       
    }
    public class DTODashboardHeader
    {
        public int StageId { get; set; }
        public int StatusId { get; set; }
        public string? Stages { get; set; }
        public string? Status { get; set; }
        public string? Icons { get; set; }
        public int Statseq { get; set; }
    }
    public class DTODashboardAction
    {
      
        public int StatusId { get; set; }
        public int ActionId { get; set; }
        public string? Action { get; set; }
    }
    public class DTOApprovedCount
    {
        public int ProjId { get; set; }
        public int StatusActionsMappingId { get; set; }
        public int StatusId { get; set; }
        public string Status { get; set; }
        public int Total { get; set; }
    }
}
