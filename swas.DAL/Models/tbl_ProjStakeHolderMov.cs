using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace swas.DAL.Models
{
	///Created and Reviewed by : Sub Maj Sanal
	///Reviewed Date : 31 Jul 23
	///Tested By :- 
	///Tested Date : 
	///Start
	public class tbl_ProjStakeHolderMov
	{
        [Key]
        public int PsmId { get; set; }

        [ForeignKey("tbl_Projects")]
        public int ProjId { get; set; }

        // public int StatusId { get; set; }

        [ForeignKey("TrnStatusActionsMapping")] 
        public int StatusActionsMappingId { get; set; }

        //[StringLength(20000)]
        //[Column(TypeName = "varchar(20000)")]
        [Display(Name = "Remarks")]
        public string? Remarks { get; set; }
        public string? UndoRemarks { get; set; }

        public DateTime? TimeStamp { get; set; }

        public int ToUnitId { get; set; }

        public int FromUnitId { get; set; }

        public bool IsDeleted { get; set; }

        public bool IsActive { get; set; }
        public bool IsRead { get; set; }
        public bool? IsPullBack { get; set; }

        [Display(Name = "Edit/Delete By")]
        public int? EditDeleteBy { get; set; }
        [Display(Name = "Edit/Delete Date")]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? EditDeleteDate { get; set; }

        public int? UpdatedByUserId { get; set; }
        [Display(Name = "Date of Update")]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? DateTimeOfUpdate { get; set; }

        public bool IsComplete { get; set; }
        public bool IsComment { get; set; }

        public string? UserDetails { get; set; }
        public string? CertNo { get; set; }


    }


}
