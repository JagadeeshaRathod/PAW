using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace swas.DAL.Models
{
    /// <summary>
    /// Created and Reviewed by: [Your Reviewer Name]
    /// Reviewed Date: [Review Date]
    /// Tested By: 
    /// Tested Date: 
    /// </summary>
    public class tbl_mUnitBranch
    {
        [Key]
        [Display(Name = "Unit ID")]
        public int unitid { get; set; }

        [Required]
        [StringLength(200)]
        [Display(Name = "Unit Name")]
        public string? unitname { get; set; }

        [Display(Name = "Company ID")]
        public int comdid { get; set; }

        [Required]
        [StringLength(200)]
        [Display(Name = "Unit SUS Number")]
        public string unitSusNo { get; set; }

        [Required]
        [StringLength(200)]
        [Display(Name = "Area Location")]
        public string area_loc { get; set; }

        [Display(Name = "Type ID")]
        [ForeignKey("Types")]
        public int TypeId { get; set; }

        [Display(Name = "Corps ID")]
        public int CorpsId { get; set; }

        [Display(Name = "Status")]
        public bool status { get; set; }

        [Required]
        [StringLength(200)]
        [Display(Name = "Updated By")]
        public string updatedby { get; set; }

        [Display(Name = "Updated Date")]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy HH:mm:ss}", ApplyFormatInEditMode = true)]
        public DateTime UpdatedDate { get; set; }

        [Display(Name = "Comment Required")]
        public bool? CommentRequired { get; set; }
    }
}
