using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace swas.DAL.Models
{
	///Created and Reviewed by : Sub Maj Sanal
	///Reviewed Date : 31 Jul 23
	///Tested By :- 
	///Tested Date : 
	///Start
	public class tbl_Projects
	{ 
		[Key]
		public int ProjId { get; set; }
         

       
		[Column(TypeName = "varchar(200)")]
		[Display(Name = "Project Name")]
      
        public string ProjName { get; set; }

		[ForeignKey("tbl_mStakeHolder")]
        [Display(Name = "Stake Holder")]

        public string Sponsor { get; set; }

        [ForeignKey("tbl_mUnitBranch")]
        public int StakeHolderId { get; set; }

        [ForeignKey("tbl_ProjStakeHolderMov")]
        public int CurrentPslmId { get; set; }

		[DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]

        [Display(Name = "Initiated On")]
        public DateTime? InitiatedDate { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Completed On")]
        public DateTime? CompletionDate { get; set; }

		
		[Column(TypeName = "varchar(200)")]

        [Display(Name = "Whitelisted")]
        public string IsWhitelisted { get; set; }
		
		[Column(TypeName = "varchar(200)")]
		[Display(Name = "Initial Remarks")]
   
        public string InitialRemark { get; set; }
		public bool IsDeleted { get; set; }
		public bool IsActive  { get; set; }

		[Display(Name = "Edit/Delete By")]
		public int? EditDeleteBy { get; set; }
		[Display(Name = "Edit/Delete Date")]
		[DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? EditDeleteDate { get; set; }
        [Display(Name = "Updated by")]
        public int? UpdatedByUserId { get; set; }
		[Display(Name = "Date of Update")]
		[DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
		public DateTime? DateTimeOfUpdate { get; set; }

        
        [Display(Name = "Aim & Scope")]
    
        public string? AimScope { get; set; }
		[Display(Name = "IT infra reqd")]
		
		public string? HQandITinfraReqd { get; set; }
		[Display(Name = "Hosted On (ADN/Internet)")]
		
 
        public string? Hostedon { get; set; }
        
        [Display(Name = "Brief details of content of the proposed SW appl")]
		public string? ContentofSWApp { get; set; }
      
        [Display(Name = "Brief justification")]
		public string? ReqmtJustification { get; set; }
       
        [Display(Name = "Usability of proposed appl by others arms/services/org/est ")]
		public string? UsabilityofProposedAppln { get; set; }
      
        [Display(Name = "Details of user base")]
		public string? DetlsofUserBase { get; set; }

		[Display(Name = "Envisage cost of entire proj incl license fees and maint (Rupees)")]
       
        public string EnvisagedCost { get; set; }

		[Display(Name = "Brief details of proposed network and bandwidth reqmts")]
		
		public string? NWBandWidthReqmt { get; set; }
       
        [Display(Name = "Projected dt of completion incl broad timelines")]
		public string? MajTimeLines { get; set; }
       
        [Display(Name = "Brief details of SW platform and tech stack proposed for devp of  appl incl op sys dependencies (if any)")]
		public string? TechStackProposed { get; set; }
       
        [Display(Name = "Brief details of proposed data security measures incl backup of data")]
		public string? DataSecurity_backup { get; set; }
        
        [Display(Name = "Type of Software")]
		public string? TypeofSW { get; set; }


		[Display(Name = "Being devp in house or through Outsourced")]
		public string? BeingDevpInhouse { get; set; }

       
        [Display(Name = "Endorsement by Head of Dept")]
		public string? EndorsmentbyHeadof { get; set; }
        public string? ProjCode { get; set; }
		[Display(Name = "Application Type")]
        [ForeignKey("mAppType")]
      
        public int Apptype { get; set; }


        public bool IsProcess { get; set; }

        [Display(Name = "Deployment Mode")]
       
        public string? Deplytype { get; set; }
		
        public bool IsSubmited { get; set; }

        [Display(Name = "Hosted On")]
        [ForeignKey("mHostType")]
        public int HostTypeID { get; set; }

        

        //16/07/24

        [Display(Name = "Brief details of OS & Sys software reqmts")]
        
        public string? Detlsof_OS { get; set; }

        [Display(Name = "Brief details of proposed DB Engine to be Used in the Appl.")]
       
        public string? ProposedDB_Engine { get; set; }


        [Display(Name = "Detls of Sw Architecture and COTS Sw proposed to be utilised")]
       
        public string? DetlsofSw_Architecture { get; set; }


        [Display(Name = "Detls of proposed architecture")]
      
        public string? DetlsofProposed_Architecture { get; set; }

        [Display(Name = "Brief details of proposed utilisation of Public Key Infra(PKI) and Iden and Access Mgt(IAM)")]
        
        public string? DetlsPki_IAM { get; set; }


        [Display(Name = "Technology dependencies(if any)")]
        
        public string? Technology_dependencies { get; set; }


        [Display(Name = "Database reqmts")]
        
        public string? Database_reqmts { get; set; }

        [Display(Name = "Enhancement/upgradation(incl patch mgt/Sw updt procedure and mechanism")]
       
        public string? Enhancement_upgradation { get; set; }

        [Display(Name = "Details of licensing(if any)")]
        
        public string? Details_licensing { get; set; }
       

        [NotMapped]
        public int CurrentStakeHolderId { get; set; } 

		[NotMapped]
		public string? StakeHolder { get; set; }
        [NotMapped]
        public string? FwdtoUser { get; set; }
        [NotMapped]
        [Display(Name = "Fwd On")]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? FwdtoDate { get; set; }

        [NotMapped]
        public string? RecdFmUser { get; set; }

        [NotMapped]
        public string? FwdBy { get; set; }


     
        [NotMapped]
        public string? Status { get; set; }
		[NotMapped]
		public string? Comments { get; set; }

		[NotMapped]
		public string? UserID { get; set; }

		[NotMapped]
		public string? UploadedFile { get; set; }
		[NotMapped]
		public string? RegdUserID { get; set; }
        [NotMapped]
        public int? TotalDays { get; set; }
        [NotMapped]
        public string? AdRemarks { get; set; }
        [NotMapped]
        public string? Stages { get; set; }
        [NotMapped]
        public string? Action { get; set; }
        [NotMapped]
        public DateTime? ActionDt { get; set; }
        [NotMapped]
        public int? ActionCde { get; set; }

        [NotMapped]
        public int? AttCnt { get; set; }
        [NotMapped]
        public string? EncyID { get; set; }
		[NotMapped]
		public string? ActFileName { get; set; }
        //public List<UnitDtl>? DynamicColumns { get; set; } = new List<UnitDtl>();

        [NotMapped]
        public int? ActionId { get; set; }
        [NotMapped]
        public string? EncyPsmID { get; set; }
		[NotMapped]
		public string? BlogComment { get; set; }
        [NotMapped]
        public int? PsmIds { get; set; }
        [NotMapped]
        public int StageId { get; set; }

        
    }


}
