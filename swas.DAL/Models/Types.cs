using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace swas.DAL.Models
{
    ///Created and Reviewed by : Sub Maj Sanal
    ///Reviewed Date : 07 Aug 23
    ///Tested By :- 
    ///Tested Date : 
    ///Start
    public class Types
    {
        [Key]
        [Column("Id")]
        public int Id { get; set; }

        [Required]
        [Column("Name", TypeName = "varchar(200)")]
        public string Name { get; set; }

        public int IsActive { get; set; }

    }
    //AH Added by Sub Maj M Sanal Kumar on 07 Aug 23
    [NotMapped]
    [AllowAnonymous]
    public class AddlTask
    {
        [NotMapped]
        [Column("Id")]
        public string Id { get; set; }

        [NotMapped]
        [Column("Name", TypeName = "varchar(200)")]
        public string Name { get; set; }
        [NotMapped]
        public int IsActive { get; set; }
        [NotMapped]
        [Column("Unit", TypeName = "varchar(250)")]
        public string Unitname { get; set; }

    }
}
