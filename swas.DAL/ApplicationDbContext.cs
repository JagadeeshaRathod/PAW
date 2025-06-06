
using ASPNetCoreIdentityCustomFields.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using swas.DAL.Logger;
using swas.DAL.Mapper;
using swas.DAL.Models;

namespace swas.DAL
{

    ///Created and Reviewed by : Sub Maj Sanal
    ///Reviewed Date : 29 Jul 23
    ///Tested By :- 
    ///Tested Date : 
    ///Start
    [AllowAnonymous]
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<tbl_LoginLog> tbl_LoginLog { get; set; }
        //public DbSet<SoftwareType> SoftwareTypes { get; set; }
        public DbSet<Types> tbl_Type { get; set; }
        public DbSet<UnitDtl> tbl_mUnitBranch { get; set; }
        public DbSet<tbl_AttHistory> AttHistory { get; set; }
        public DbSet<tbl_mStatus> mStatus { get; set; }
        public DbSet<tbl_Projects> Projects { get; set; }
        public DbSet<tbl_mStakeHolder> mStakeHolder { get; set; }
        public DbSet<tbl_mStages> mStages { get; set; }
        public DbSet<tbl_mActions> mActions { get; set; }
        public DbSet<tbl_Comment> Comment { get; set; }
        public DbSet<tbl_ProjStakeHolderMov> ProjStakeHolderMov { get; set; }
        public DbSet<mCommand> mCommand { get; set; }
        public DbSet<mCorps> mCorps { get; set; }
        //public DbSet<tbl_users> Users { get; set; }
        public DbSet<mAppType> mAppType { get; set; }
        public DbSet<mHostType> mHostType { get; set; }
        public DbSet<tbl_mUnitBranch> mUnitBranch { get; set; } 

        public DbSet<mRank> mRank { get; set; }
        //public DbSet<Proj_initialstatus> PorjIniStat { get; set; }
        //public DbSet<tbl_viewActionsum> Viewaction { get; set; }
        //public DbSet<tbl_viewStageSummary> StageSummary { get; set; }
        public DbSet<ChartModel> ChartModel { get; set; }
        public DbSet<ChartModelS> ChartModelS { get; set; }
        public DbSet<StkComment> StkComment { get; set; }
        public DbSet<Notification> Notification { get; set; }
        public DbSet<StkStatus> StkStatus { get; set; }
        //public DbSet<mActionmapping> mActionmapping { get; set; }
        //public DbSet<Resultss> Resultstr { get; set; }
        //public DbSet<AttHistComment> AttHistComments { get; set; }
        public DbSet<mWhiteListedHeader> mWhiteListedHeader { get; set; }
        public DbSet<trnWhiteListed> trnWhiteListed { get; set; }
        public DbSet<TrnUnitStatusMapping> TrnUnitStatusMapping { get; set; }
        public DbSet<TrnStatusActionsMapping> TrnStatusActionsMapping { get; set; }

        public DbSet<mUserMapChat> mUserMapChat { get; set; }
        public DbSet<TrnChatMsg> TrnChatMsg { get; set; }
        public DbSet<mCalendar> mCalendar { get; set; }

        public DbSet<LogEntry> Errors { get; set; }
        public DbSet<AddNewProject> AddNewProjects   { get; set; }

        public DbSet<DToWhiteListeds> WhiteListedProjects { get; set; }
        //public DbSet<DTOProjectsFwd> WhiteListedProjects { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<Resultss>().HasNoKey();

            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ApplicationUser>()
                .Property(e => e.RoleName)
                .HasColumnType("nvarchar(max)");

            modelBuilder.Entity<ApplicationUser>()
              .Property(e => e.RoleName_IAM)
              .HasColumnType("nvarchar(max)");

            modelBuilder.Entity<ApplicationUser>()
                .Property(e => e.description_iam)
                .HasColumnType("nvarchar(max)");


            modelBuilder.Entity<ApplicationUser>()
                .Property(e => e.domain_iam)
                .HasColumnType("nvarchar(max)");


            modelBuilder.Entity<ApplicationUser>()
              .Property(e => e.unitid)
              .HasColumnType("int");

            modelBuilder.Entity<ApplicationUser>()
        .Property(e => e.appointment)
        .HasColumnType("nvarchar(max)");

            modelBuilder.Entity<AddNewProject>().HasNoKey(); // Mark as keyless entity

        }

    }


}
