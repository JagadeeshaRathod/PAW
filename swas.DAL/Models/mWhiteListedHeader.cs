using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace swas.DAL.Models
{
    public class mWhiteListedHeader
    {
        [Key]
        public int Id { get; set; }
        public string Header { get; set; }
    }

    [Keyless]
    //[NotMapped]
    public class DToWhiteListeds
    {        
        public int? Id { get; set; }       
        //public int ProjId { get; set; }
        public string? ProjName { get; set; }
        public string? HostedOn { get; set; }
        public string? Fmn { get; set; }
        public string? ContactNo { get; set; }
        public DateTime? Clearence { get; set; }

        public string? CertNo { get; set; }
        public string? Appt { get; set; }
        public DateTime? ValidUpto { get; set; }
        public string? Remarks { get; set; }
    }
}
