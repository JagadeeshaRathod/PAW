
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace swas.DAL.Models
{
    public class mCommand
    {
        [Key]
        public int comdid { get; set; }

        [Required]
        [MaxLength(50)]
        public string? Command_Name { get; set; }

        public bool? IsDeleted { get; set; }

        public bool? IsActive { get; set; }

        public int? EditDeleteBy { get; set; }

        public DateTime? EditDeleteDate { get; set; }

        public int? UpdatedByUserId { get; set; }

        public DateTime? DateTimeOfUpdate { get; set; }
    }
}
