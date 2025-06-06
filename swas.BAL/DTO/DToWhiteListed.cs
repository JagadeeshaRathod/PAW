using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace swas.BAL.DTO
{
    public class DToWhiteListed
    {
        public int? Id { get; set; }
        //public int? HeaderID { get; set; }
        //public string? ProjName { get; set; }
        //public string? Appt { get; set; }
        //public string? Fmn { get; set; }
        //public string? ContactNo { get; set; }
        //public string? Clearence { get; set; }
        //public string? CertNo { get; set; }
        //public string? date { get; set; }
        //public string? ValidUpto { get; set; }
        //public string? Remarks { get; set; }
        //public string? Header { get; set; }

        public int ProjId { get; set; }
        public string? ProjName { get; set; }
        public string? HostedOn { get; set; }
        public string? Sponser { get; set; }
        public string? ContactNo { get; set; }
        public string? ACGClearence { get; set; }

        public string? CertNo { get; set; }
        public string? ValidUpto { get; set; }
        public string? Remarks { get; set; }
    }
}
