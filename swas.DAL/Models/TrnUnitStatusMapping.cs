using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace swas.DAL.Models
{
    public class TrnUnitStatusMapping
    {
        [Key]
        public int UnitStatusMappingId { get; set; }

        [ForeignKey("tbl_mUnitBranch")]
        public int UnitId { get; set; }
        
        [ForeignKey("TrnStatusActionsMapping")]
        public int StatusActionsMappingId { get; set; }
    }
}
