using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

using System.Threading.Tasks;

namespace swas.DAL.Models
{
    public class StkStatus
    {
        [Key]
        public int StkStatusId { get; set; }
        [StringLength(100)]
        public string Status { get; set; }
    }

    //public class Resultss
    //{
    //    public string? Result { get; set; }
    //}
}
