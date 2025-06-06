using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace swas.DAL.Models
{
    public class tbl_LoginLog
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int? UserId { get; set; }
        public int? unitid { get; set; }
        public string IP { get; set; }
        public DateTime logindate { get; set; }

        public string userName { get; set; }
        public bool? IsActive { get; set; }
        public int Updatedby { get; set; }
        public DateTime UpdatedOn { get; set; }

    }
}
