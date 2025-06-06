using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace swas.DAL.Models
{
    public class TrnStatusActionsMapping
    {
        [Key]
        public int StatusActionsMappingId { get; set; }
        
        [ForeignKey("tbl_mStatus")]
        public int StatusId { get; set; }

        [ForeignKey("tbl_mActions")]
        public int ActionsId { get; set; }
        public bool IsActive { get; set; }
    }
}
