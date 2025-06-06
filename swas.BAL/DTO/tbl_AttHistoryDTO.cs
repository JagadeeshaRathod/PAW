using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace swas.BAL.DTO
{
	///Created and Reviewed by : Sub Maj Sanal
	///Reviewed Date : 31 Jul 23
	///Tested By :- 
	///Tested Date : 
	///Start
	public class tbl_AttHistoryDTO
	{
		[Key]
		public int AttId { get; set; }

		[ForeignKey("tbl_ProjStakeHolderMov")]
		public int PsmId { get; set; }
		[StringLength(200)]
		[Column(TypeName = "varchar(200)")]
		[Display(Name = "Attach Path")]
		public string AttPath { get; set; }
		public int ActionId { get; set; }
		public DateTime? TimeStamp { get; set; }
		public bool IsDeleted { get; set; }
		public bool IsActive { get; set; }
		[Display(Name = "Edit/Delete By")]
		public string EditDeleteBy { get; set; }
		[Display(Name = "Edit/Delete Date")]
		[DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
		public DateTime? EditDeleteDate { get; set; }

		public string UpdatedByUserId { get; set; }
		[Display(Name = "Date of Update")]
		[DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
		public DateTime? DateTimeOfUpdate { get; set; }


		public string Message { get; set; }
		public bool IsSuccess { get; set; }
		public bool IsResponse { get; set; }

		[Required(ErrorMessage = "Please enter file name")]
		public string FileName { get; set; }
		[Required(ErrorMessage = "Please select file")]
		public IFormFile File { get; set; }

	

	}


}
