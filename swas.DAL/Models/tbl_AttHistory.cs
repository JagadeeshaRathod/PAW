using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace swas.DAL.Models
{
	///Created and Reviewed by : Sub Maj Sanal
	///Reviewed Date : 31 Jul 23
	///Tested By :- 
	///Tested Date : 
	///Start 
	public class tbl_AttHistory
	{
		[Key]
		public int AttId { get; set; }

		[ForeignKey("tbl_ProjStakeHolderMov")]
		public int PsmId { get; set; }
		[StringLength(200)]
		[Column(TypeName = "varchar(200)")]
		[Display(Name = "Attach Path")]
		public string? AttPath { get; set; }
		// Check with database all the value is 0 that is not possible to make foregin relation with mAction table.
		public int? ActionId { get; set; }
		public DateTime? TimeStamp { get; set; }
		public bool? IsDeleted { get; set; }
		public bool? IsActive { get; set; }
		[Display(Name = "Edit/Delete By")]
		public int? EditDeleteBy { get; set; }
		[Display(Name = "Edit/Delete Date")]
		[DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
		public DateTime? EditDeleteDate { get; set; }

		public int? UpdatedByUserId { get; set; }
		[Display(Name = "Date of Update")]
		[DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
		public DateTime? DateTimeOfUpdate { get; set; }

        [StringLength(250)]
        public string? Reamarks { get; set; }
		public string? ActFileName { get; set; }

        //public int CommentId { get; set; }


    }


}
