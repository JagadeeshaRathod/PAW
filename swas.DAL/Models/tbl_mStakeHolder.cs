using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace swas.DAL.Models
{

    ///Created and Reviewed by : Sub Maj Sanal
    ///Reviewed Date : 31 Jul 23
    ///Tested By :- 
    ///Tested Date : 
    ///Start
    public class tbl_mStakeHolder
    {
        [Key]
        [Display(Name = "Stake Holder")]
        public int StakeHolderId { get; set; }

        [Required]
        [StringLength(50)]
        [Column(TypeName = "varchar(50)")]
        [Display(Name = "Stake Holder Name")]
        public string StakeHolder { get; set; }

        public bool IsDeleted { get; set; }

        public bool IsActive { get; set; }

        [Display(Name = "Edit/Delete By")]
        public int EditDeleteBy { get; set; }
        [Display(Name = "Edit/Delete Date")]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? EditDeleteDate { get; set; }

        public int UpdatedByUserId { get; set; }
        [Display(Name = "Date of Update")]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? DateTimeOfUpdate { get; set; }

    }
}
