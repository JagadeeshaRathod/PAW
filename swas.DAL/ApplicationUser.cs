
using Microsoft.AspNetCore.Identity;

///
///Developer Name :- Sub Maj M Sanal Kumar
///Purpose :-
///Authority & Reference :- 
///Kind Of Request :- 
///Version :- 
///Dated :- 29/07/2023
///Remarks :- 
///
namespace ASPNetCoreIdentityCustomFields.Data
{
    public class ApplicationUser : IdentityUser
    {
        public int UserIntId { get; set; }
        public string? RoleName { get; set; }
        public string? domain_iam { get; set; }
        public string? description_iam { get; set; }
        public string? RoleName_IAM { get; set; }
        public int unitid { get; set; }
        public string? appointment { get; set; }
        public string? Icno { get; set; }
        public int? Rank { get; set; }
        public string? Offr_Name { get; set; }
        public string? Tele_Army { get; set; }

        public bool? Flag { get; set; }
        public DateTime? CreatedDate { get; set; }

        //public int skipImdtFmn { get; set; } = 0;
    }
}