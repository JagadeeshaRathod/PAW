using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace swas.DAL.Models
{


    public class StkComment
    {

        [Key]
        public int StkCommentId { get; set; }

        [ForeignKey("tbl_ProjStakeHolderMov")]
        public int? PsmId { get; set; }

        [ForeignKey("tbl_Projects")]
        public int? ProjId { get; set; }

        [ForeignKey("tbl_mUnitBranch")]
        public int? StakeHolderId { get; set; }
        [ForeignKey("tbl_mActions")]
        public int? ActionsId { get; set; }
        [StringLength(1500)]
        public string? Comments { get; set; }

        public int? StkStatusId { get; set; }
        public bool? IsDeleted { get; set; }
        public bool? IsActive { get; set; }
        public int? EditDeleteBy { get; set; }
        public DateTime? EditDeleteDate { get; set; }
        public int? UpdatedByUserId { get; set; }
        public DateTime? DateTimeOfUpdate { get; set; }
        public string? UserDetails { get; set; }
        [NotMapped]
        public string? Reamarks { get; set; }

        public string? ActFileName { get; set; }
        public string? Attpath { get; set; }
        public string? AttDesc { get; set; }



        [NotMapped]
        public tbl_AttHistory? AttHisAdd { get; set; }



        public StkComment()
        {
            ProjDetl = new List<tbl_Projects>();
        }
        [NotMapped]
        public List<tbl_Projects>? ProjDetl { get; set; }
    }
}
