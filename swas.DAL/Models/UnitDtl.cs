using Microsoft.AspNetCore.Authorization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace swas.DAL.Models
{
    ///Created and Reviewed by : Sub Maj Sanal
    ///Reviewed Date : 31 Jul 23
    ///Tested By :- 
    ///Tested Date : 
    ///Start
    [AllowAnonymous]
    public class UnitDtl
    {
        [Key]
        [Column("unitid")]
        public int unitid { get; set; }

        [Required]
        [Column("unitname", TypeName = "varchar(200)")]
        [Display(Name = "Unit Name")]
        [StringLength(100)]
        public string UnitName { get; set; }


        [Column("comdid")]
        [Display(Name = "Command")]
        public int? Command { get; set; }

        [Required]
        [Column("unitSusNo", TypeName = "varchar(15)")]
        [Display(Name = "SUS No")]
        [StringLength(15)]
        public string UnitSusNo { get; set; }

        //[Required]
        //[Column("area_loc", TypeName = "varchar(200)")]
        //public string? Loc { get; set; }

        [Required]
        [Display(Name = "Type (Unit/Fmn)")]

        public int? TypeId { get; set; }

        [Display(Name = "Corps")]
        [Required(AllowEmptyStrings = true)]
        public int? CorpsId { get; set; }


        [Required]
        [Column("status")]
        public bool Status { get; set; }

        public bool? commentreqdid { get; set; }

        [Required]
        [Column("updatedby")]
        [Display(Name = "Updated By")]
        public string? UpdatedBy { get; set; }

        [Required]
        [DataType(DataType.Date)]

        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        [Column("updateddt")]
        [Display(Name = "Updated Dt")]
        public DateTime UpdatedDate { get; set; }


        [NotMapped]
        public string? ComdAbbreviation { get; set; }

        [NotMapped]
        public string? Corps { get; set; }
        [NotMapped]
        public string? Types { get; set; }

        [NotMapped]
        public string? EnctyptID { get; set; }
        [NotMapped]
        public int? Updatecde { get; set; }
        [NotMapped]
        public string? Remarks { get; set; }
        [NotMapped]
        public int? InitialStatus { get; set; }


    }
}