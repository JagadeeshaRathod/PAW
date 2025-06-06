using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace swas.DAL.Models
{
    public class mRank
    {

        [Key]
        
        public int Id { get; set; }
        public string? RankName { get; set; }
    }
}
