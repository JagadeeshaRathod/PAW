using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace swas.DAL.Models
{
	///Created and Reviewed by : Sub Maj Sanal
	///Reviewed Date : 31 Jul 23
	///Tested By :- 
	///Tested Date : 
	///Start
	public class tbl_mStages
	{
		[Key]
		public int StagesId { get; set; }
		
		[StringLength(200)]
		[Column(TypeName = "varchar(200)")]
		[Display(Name = "Stage")]
		public string Stages { get; set; }
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
		public bool? InitiaalID { get; set; } = false;
		public bool? FininshID { get; set; } = false;
	}


}
