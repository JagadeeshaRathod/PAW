using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace swas.DAL.Models
{
    public class trnWhiteListed
    {
        public int Id { get; set; }
        [ForeignKey("mHostType")]
        public int mHostTypeId { get; set; }
        public string? ProjName { get; set; }
        public string? Fmn { get; set; }
        public string? Appt { get; set; }
        public string? ContactNo { get; set; }
        public DateTime? Clearence { get; set; }
        public string? CertNo { get; set; }
        public DateTime? date { get; set; }
        public DateTime? ValidUpto { get; set; }
        public string? Remarks { get; set; }
    }
}
