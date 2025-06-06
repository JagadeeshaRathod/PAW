using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace swas.DAL.Models
{
    public class mAppType
    {
        [Key]
        public int Apptype { get; set; }
        public string AppDesc { get; set; }
    
        
    }
}
