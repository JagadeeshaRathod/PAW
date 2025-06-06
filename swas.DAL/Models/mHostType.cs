using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace swas.DAL.Models
{
    public class mHostType
    {
        [Key]
        public int HostTypeID { get; set; }
        public string? HostingDesc { get; set; }
    
    }


}
