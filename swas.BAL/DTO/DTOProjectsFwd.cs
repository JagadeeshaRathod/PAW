using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace swas.BAL.DTO
{
    [Keyless]
    public class DTOProjectsFwd
    {
        public int ProjId { get; set; }
        public int PsmIds { get; set; }
        public string? ProjName { get; set; }
        public int StakeHolderId { get; set; }
        public string? StakeHolder { get; set; }
        public string? Remarks { get; set; }
        public string? Status { get; set; }
        public int FromUnitId { get; set; }
        public string? FromUnitName { get; set; }
        public int ToUnitId { get; set; }
        public string? ToUnitName { get; set; }
        public string? Action { get; set; }
        public int StageId { get; set; }
        public int StatusId { get; set; }
        public int ActionId  { get; set; }
        public int TotalDays { get; set; }
        public string? EncyID { get; set; }
        public string? EncyPsmID { get; set; }
        public bool IsProcess { get; set; } 
        public bool IsRead { get; set; }            
        public int undopsmId { get; set; }
        public  bool IsComplete { get; set; }
        public DateTime? TimeStamp { get; set; }

        public string? Stage { get; set; }
        public string? Sponsor { get; set; }


        public string UnitName { get; set; }
        public string FromUnitUserDetail { get; set; }

        public int StkStatusId { get; set; }
        public DateTime? DateTimeOfUpdate { get; set; }

        public bool IsCommentRead { get; set; }

		public int? AttCnt { get; set; }

        public bool IsComment { get; set; }
        public bool? IsPullBack { get; set; }

        public bool PullbackAction { get; set; }

        //public int Date_type { get; set; }
        //public bool AdminApprovalStatus { get; set; }
        //public bool UserRequest { get; set; }
        //public int? RequestUnitId { get; set; }

        public int Date_type { get; set; }
        public bool AdminApprovalStatus { get; set; }
        public bool UserRequest { get; set; }
        public int? RequestUnitId { get; set; }

    }
}
