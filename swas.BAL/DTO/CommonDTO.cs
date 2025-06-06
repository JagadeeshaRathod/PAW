using swas.DAL.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using System.Runtime.InteropServices;

namespace swas.BAL.DTO
{
    public class CommonDTO
    {
        public List<tbl_Projects>? Projects { get; set; }
        public tbl_Projects? ProjEdit { get; set; }
        public List<tbl_AttHistory>? Atthistory { get; set; }
        public tbl_AttHistory? AttHisAdd { get; set; }

        public tbl_ProjStakeHolderMov? ProjMov { get; set; }

        public List<ProjHistory>? ProjHistory { get; set; }

        public List<NextActionsDto>? NextActionDetl { get; set; }

        public CommonDTO()
        {

            ProjMov = new tbl_ProjStakeHolderMov();
        }

        public bool? Submitcde { get; set; } = false;

        public int? DataProjId { get; set; }
        public string? ProjCode { get; set; }

    }


    public class ChartServiceResponse
    {
        public string? JsonData { get; set; }

    }
    public class MailBox
    {
        public List<DTOProjectsFwd> InBox { get; set; }
        public List<DTOProjectsFwd> SendItems { get; set; }
        public List<tbl_Projects> CompletedItems { get; set; }

        public List<tbl_Projects> Draft { get; set; }

    }

    public class DTOViews
    {

        public string ProjName { get; set; }
        public string Comment { get; set; }
        public string Actions { get; set; }
        public string Stages { get; set; }
        public string StakeHolder { get; set; }
        public string Status { get; set; }
        public string CurrentStakeHolder { get; set; }
        public string FromStakeHolder { get; set; }
        public string ToStakeHolder { get; set; }
        public string AttachDocuments { get; set; }
        public string AddRemarks { get; set; }
        public DateTime TimeStamp { get; set; }
        public string EditDeleteBy { get; set; }
        public DateTime EditDeleteDate { get; set; }
        public string UpdatedByUserId { get; set; }
        public DateTime DateTimeOfUpdate { get; set; }
        public DateTime TostackholderDt { get; set; }



    }
    [AllowAnonymous]
    public class Login
    {
        public string? UserName { get; set; }
        public string? Unit { get; set; }
        public string? Role { get; set; }
        public int? Comdid { get; set; }
        public int? Corpsid { get; set; }
        public string? Iamuserid { get; set; }
        public string? Appontment { get; set; }
        public string? ActualUserName { get; set; }
        public int? unitid { get; set; }
        public int? totmsgin { get; set; }
        public int? tocommentin { get; set; }
        public int? typeid { get; set; }
        public int UserIntId { get; set; }
        public string? IcNo { get; set; }
        public string? Offr_Name { get; set; }
        public string? Rank { get; set; }
        public string? IpAddress { get; set; }
        public int cla { get; set; }
    }
    public class DefaultValueID
    {
        public int StageID { get; set; } = 0;
        public int StatusId { get; set; } = 0;
        public int ActionId { get; set; } = 0;

    }




    public class ProjHistory
    {

        public int ProjId { get; set; }
        public int? PsmId { get; set; }
        public string? ProjName { get; set; }
        public string? Stages { get; set; }
        public string? Status { get; set; }
        public string? Comment { get; set; }
        public string? FromStakeHolder { get; set; }
        public string? ToStakeHolder { get; set; }
        public string? CurrentStakeHolder { get; set; }
        public string? InitiatedBy { get; set; }
        public string? TimeStamp { get; set; }
        public string? InitialRemarks { get; set; }
        public string? Attachments { get; set; }


        public string UpdatedByUserId { get; set; }

        public string DocumentDesc { get; set; }
        public string AttPath { get; set; }

        public DateTime? AttTimeStamp { get; set; }
        public int AttId { get; set; }

        public DateTime? ActionDt { get; set; }
        public int? ActionCde { get; set; }

        public string? AppDesc { get; set; }

        public string? HostedOn { get; set; }

        public List<tbl_AttHistory> Atthistory { get; set; }

        public List<tbl_Projects> ProjectDetl { get; set; }


        public ProjHistory()
        {
            Atthistory = new List<tbl_AttHistory> { new tbl_AttHistory { Reamarks = "" } };
            ProjectDetl = new List<tbl_Projects> { new tbl_Projects { InitialRemark = "" } };

        }

        public int? AttCnt { get; set; }
        public string? Remarks { get; set; }

        public string? ActFileName { get; set; }
        public string? ActionName { get; set; }


    }



    public class Projmove
    {
        public tbl_ProjStakeHolderMov ProjMov { get; set; }
        public List<tbl_AttHistory> Atthistory { get; set; }

        public tbl_AttHistory AttHistory { get; set; }
        public Projmove()
        {
            ProjMov = new tbl_ProjStakeHolderMov();
            Atthistory = new List<tbl_AttHistory> { new tbl_AttHistory { Reamarks = "" } };
        }

        public bool? Submitcde { get; set; } = false;

        public int? DataProjId { get; set; }
        public string? ProjCode { get; set; }
        public int? PsmId { get; set; }


    }

    public class ProjIDRes
    {
        public string ProjWdOne { get; set; }
        public string ProjWdTwo { get; set; }
        public string PorjPin { get; set; }

    }

    public class UnitValidate
    {
        [StringLength(200)]

        [Display(Name = "Regd User Name ")]
        public string? UserId { get; set; }

    }





    public class AttHistoryindep
    {
        public List<tbl_AttHistory> AttDocuHistory { get; set; }
        public AttHistoryindep()
        {

            AttDocuHistory = new List<tbl_AttHistory> { new tbl_AttHistory { Reamarks = "" } };
        }


    }

    public class Actiondto
    {
        public int? ActionsId { get; set; }
        public string? Status { get; set; }
        public string? Stages { get; set; }
        public string? Actions { get; set; }
        public int? TimeLimit { get; set; }
        public int? ActionSeq { get; set; }
        public string? UnitName { get; set; }
        public DateTime? EditDeleteDate { get; set; }
        public string? Statuss { get; set; }

    }


    public class TimeExceeds
    {

        public int? ProjId { get; set; }
        public int? psmid { get; set; }
        public string? ProjName { get; set; }
        public string? helwith { get; set; }
        public string? fwdby { get; set; }
        public string? Actions { get; set; }
        public string? Comment { get; set; }
        public DateTime? EditDeleteDate { get; set; }
        public int? TimeLimit { get; set; }
        public int? dayss { get; set; }
        public int? exceeds { get; set; }
        public string? StrDate { get; set; }


    }

    public class TimeExceedsAlerts
    {

        public int? unitid { get; set; }
        public string? unitname { get; set; }
        public string? color { get; set; }
        public string? forecolor { get; set; }
    }

    public class MyRequestModel
    {
        public int DtaProjID { get; set; }
        public string AttPath { get; set; }
        public int PsmId { get; set; }
        public string Projpin { get; set; }
        public string ActFileName { get; set; }
        public string AttDocuDesc { get; set; }
    }



    public class Users
    {
        [Key]
        [Column("Id")]
        public int Id { get; set; }

        [Required]
        [Column("UserName", TypeName = "varchar(200)")]
        [StringLength(20)]
        public string? UserName { get; set; }


        [Required]
        [Column("NormalizedUserName", TypeName = "varchar(200)")]
        [StringLength(20)]
        public string? NormalizedUserName { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string? Password { get; set; }
        [NotMapped]
        [Required(ErrorMessage = "Confirm Password is required")]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Compare("Password")]
        public string? ConfirmPassword { get; set; }
        [Required]
        [Column("RoleId")]
        public int RoleId { get; set; }

        [Column("CmdId")]
        public int CmdId { get; set; }
        [Column("CorpsId")]
        public int CorpsId { get; set; }

        [Column("UnitId")]
        public int UnitId { get; set; }

        [Required]
        [Column("updatedBy", TypeName = "varchar(200)")]
        public int UpdatedBy { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        [Column("updateddt")]
        public DateTime UpdatedDate { get; set; }

        public int IsActive { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? RankId { get; set; }

        public bool? Flag { get; set; }

        [NotMapped]

        public string? RoleName { get; set; }
        [NotMapped]
        public string? UnitName { get; set; }

        [NotMapped]
        public string? CommandName { get; set; }
        [NotMapped]
        public string? CorpsName { get; set; }
        [NotMapped]


        [Display(Name = "Existing Regn Id of the unit(If already Regd)")]
        public string? ExRegnId { get; set; }


    }
    public class InputModel
    {
        public string Id { get; set; }


        [Display(Name = "UserName")]
        public string? UserName { get; set; }


        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string? Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string? ConfirmPassword { get; set; }

        [Required(ErrorMessage = "RoleName is required.")]
        [Display(Name = "RoleName")]
        public string? RoleName { get; set; }

        [Required(ErrorMessage = "OfficerName is required.")]
        //[RegularExpression(@"^[a-zA-Z/S]*$", ErrorMessage = "OfficerName should contain only letters.")]
        [Display(Name = "OfficerName")]
        public string? OfficerName { get; set; }

        [Display(Name = "IAM User ID")]
        public string? domain_iam { get; set; }

        [Display(Name = "Description IAM")]
        public string? description_iam { get; set; }

        [Display(Name = "Role IAM")]
        public string? RoleName_IAM { get; set; }

        [Display(Name = "Unit")]
        public int unitId { get; set; }

        //[RegularExpression(@"^[a-zA-Z0-9\s]*$", ErrorMessage = "Appointment should contain only letters and numbers.")]
        [Display(Name = "Appointment")]
        public string? appointment { get; set; }


        [Required(ErrorMessage = "Rank is required.")]
        //[RegularExpression(@"^[a-zA-Z]*$", ErrorMessage = "Rank should contain only letters.")]
        [Display(Name = "Rank")]
        public int? RankId { get; set; }


        public string RankName { get; set; }

        //[RegularExpression(@"^\d{0,5}$", ErrorMessage = "Tele_Army should be a maximum of 10-digit number.")]
        [Display(Name = "Tele No (Army)")]
        public string? Tele_Army { get; set; }

    }


    public class EncryModel
    {
        public string EncryItem { get; set; }

    }



    public class ProjectDetailsDTO
    {
        public int? StageId { get; set; }
        public int? StatusId { get; set; }
        public int? ActionId { get; set; }
        public int? NextActionId { get; set; }
        public int? NextStatus { get; set; }
        public int? NextStage { get; set; }
        public int? StkHoldID { get; set; }

        public string? ActionName { get; set; }

    }

    public class CommentBy_StakeHolder
    {
        public int ProjId { get; set; }

        public string? stakeholder { get; set; }
        public DateTime? DateTimeOfUpdate { get; set; }
        public string? Comment { get; set; }
        public string? Action { get; set; }

    }

    public class NextActionsDto
    {
        public int ActionsId { get; set; }
        public int StatusId { get; set; }
        public string Stages { get; set; }
        public string Actions { get; set; }
        public string NextAction { get; set; }
    }

    public class ActionsSeq
    {
        public string Stages { get; set; }
        public string Status { get; set; }
        public string Actions { get; set; }
        public string UnitName { get; set; }
        public string ActionDesc { get; set; }
    }

    public class ProjLogView
    {
        public int PsmId { get; set; }
        public int ProjId { get; set; }
        public string ProjName { get; set; }
        public string UnitName { get; set; }
        public string Stages { get; set; }
        public string Status { get; set; }
        public string Actions { get; set; }
        public DateTime? TimeStamp { get; set; }
        public string FwdBy { get; set; }
        public string FwdTo { get; set; }
        public string Comment { get; set; }
        public string AttDocu { get; set; }
        public string Comments { get; set; }
        public string AddRemarks { get; set; }
        public int ActionByUser { get; set; }
        public string EncyId { get; set; }
    }
    public class IdObj
    {
        public int stakeHolderId { get; set; }
        public int psmId { get; set; }

        public int projId { get; set; }

    }

    public class SearObj
    {
        public string[] searchStakename { get; set; }
        public string TimeStampFrom { get; set; }

        public string TimeStampTo { get; set; }

    }

    //public class SearRes
    //{
    //    public string SearchText { get; set; }
    //    public string TimeStampFrom { get; set; }

    //    public string TimeStampTo { get; set; }

    //}
    public class HitCounterDaily
    {
        public int Today { get; set; }
        public int CurrentMonth { get; set; }
        public int Total { get; set; }
    }

    public class StatusStageDTO
    {
        public int StatusId { get; set; }
        public int StageId { get; set; }
        public string Status { get; set; }
        public string Stages { get; set; }
    }


    public class ActionExceptionDTO
    {
        public int UnitStatusMappingId { get; set; }
        public string UnitName  { get; set; }
        public string Stage { get; set; }
        public string SubStage  { get; set; }
        public string Action  { get; set; }

        public int StageId { get; set; }
        public int SubStageId  { get; set; }
        public int ActionId { get; set; }
        public int UnitId { get; set; }

    }

    public class NotificationContent
    {
        public string Title { get; set; }
        public string Html { get; set; }
    }



}


