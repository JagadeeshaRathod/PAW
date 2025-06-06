using ASPNetCoreIdentityCustomFields.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting.Internal;
using swas.DAL;
using swas.BAL;
using swas.BAL.Interfaces;
using swas.Exceptions;
using swas.UI.Controllers;
using swas.BAL.Repository;
using swas.BAL.Utility;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.FileProviders;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using BAL;
using swas.BAL.DTO;
using swas.DAL.Logger;
using swas.BAL.Helpers;

var builder = WebApplication.CreateBuilder(args);





//var builder = WebApplication.CreateBuilder(new WebApplicationOptions
//{
//    Args = args,
//    ApplicationName = typeof(Program).Assembly.FullName,
//    ContentRootPath = Directory.GetCurrentDirectory(),
//    EnvironmentName = Environments.Staging,
//    WebRootPath = "customwwwroot"
//});


/// Add services to the container.
///
///Developer :- Sub Maj M Sanal Kumar 
///Created On :  29 Jul 23


var connectionString = builder.Configuration.GetConnectionString("DB");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

//builder.Services.AddScoped<HandlerSessionMW>();
//builder.Services.AddTransient<ExHandMW>();
builder.Services.AddSingleton<IAuthorizationHandler, CustomAuthorizationHandler>();
builder.Services.AddScoped<AccountController, AccountController>();
builder.Services.AddScoped<HomeController, HomeController>();
builder.Services.AddScoped<CommentController, CommentController>();
builder.Services.AddScoped<ActionsController, ActionsController>();
builder.Services.AddScoped<StagesController, StagesController>();
builder.Services.AddScoped<AttHistoryController, AttHistoryController>();
builder.Services.AddScoped<ProjectsController, ProjectsController>();
builder.Services.AddScoped<StatusController, StatusController>();
builder.Services.AddScoped<DdlController, DdlController>();
builder.Services.AddScoped<ProjStakeHolderMovController, ProjStakeHolderMovController>();
builder.Services.AddScoped<StakeHolderController, StakeHolderController>();
builder.Services.AddScoped<UnitDtlsController, UnitDtlsController>();
builder.Services.AddScoped<IActionsRepository, ActionsRepository>();
builder.Services.AddScoped<IAttHistoryRepository, AttHistoryRepository>();
builder.Services.AddScoped<IChartService, ChartService>();
builder.Services.AddScoped<ICommentRepository, CommentRepository>();
builder.Services.AddScoped<IDdlRepository, DdlRepository>();
builder.Services.AddScoped<ISoftwareTypeRepository, SoftwareTypeRepository>();
builder.Services.AddScoped<IProjectsRepository, ProjectsRepository>();
builder.Services.AddScoped<IProjStakeHolderMovRepository, ProjStakeHolderMovRepository>();
builder.Services.AddScoped<IStagesRepository, StagesRepository>();
builder.Services.AddScoped<IStakeHolderRepository, StakeHolderRepository>();
builder.Services.AddScoped<IStatusRepository, StatusRepository>();
builder.Services.AddScoped<IUnitRepository, UnitRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IRankRepository, RankRepository>();
builder.Services.AddScoped<IAttHistComment, AttHistCommentRepository>();
builder.Services.AddScoped<IProjComments, ProjComments>();
builder.Services.AddScoped<IStkStatusRepository, StkStatusRepository>();
builder.Services.AddScoped<IStkCommentRepository, StkCommentRepository>();
builder.Services.AddScoped<IUnitStatusMapping, UnitStatusMapping>();
builder.Services.AddScoped<IStatusActionsMapping, StatusActionsMapping>();
builder.Services.AddScoped<IChatRepository, ChatRepository>();
builder.Services.AddScoped<IUserMapChatRepository, UserMapChatRepository>();
builder.Services.AddScoped<ITrnChatMsgRepository, TrnChatMsgRepository>();
builder.Services.AddScoped<INotificationRepository, NotificationRepository>();
builder.Services.AddScoped<IActionExceptionRepository, ActionExceptionRepository>();


builder.Services.AddAntiforgery(o => o.SuppressXFrameOptionsHeader = true);


builder.Services.AddIdentity<ASPNetCoreIdentityCustomFields.Data.ApplicationUser, IdentityRole>(options =>
{
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
    options.Lockout.MaxFailedAccessAttempts = 3;
    options.User.RequireUniqueEmail = false;
    options.SignIn.RequireConfirmedAccount = true;
    options.SignIn.RequireConfirmedEmail = false;
    options.Lockout.AllowedForNewUsers = true;

})
    .AddDefaultUI()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();


builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 100 * 1024 * 1024;
});

//builder.Services.AddRazorPages();
builder.Services.AddRazorPages(options =>
{
    options.Conventions.AuthorizePage("/Identity/Account/Register");
    options.Conventions.ConfigureFilter(new IgnoreAntiforgeryTokenAttribute());

});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Admin",
        builder => builder.RequireRole("Dte", "Dte"));
    options.AddPolicy("Unit",
       builder => builder.RequireRole("Unit", "Unit"));
    options.AddPolicy("StakeHolders",
      builder => builder.RequireRole("Unit", "Dte"));
});

builder.Services.AddControllersWithViews();

builder.Services.Configure<SecurityStampValidatorOptions>(options => options.ValidationInterval = TimeSpan.FromSeconds(30));

builder.Services.AddDataProtection();
builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(15);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;

});

//builder.Services.Configure<FormOptions>(options =>
//{

//    options.MultipartBodyLengthLimit = 100 * 1024 * 1024;
//});


builder.Services.Configure<CookiePolicyOptions>(options =>
{


    options.CheckConsentNeeded = context => true;
});

builder.Services.AddSignalR();
builder.Services.AddHttpContextAccessor();

// Configure logging to use the custom database logger provider
builder.Logging.ClearProviders();
builder.Logging.AddProvider(new DbLoggerProvider(builder.Services.BuildServiceProvider()));

var app = builder.Build();
//app.UseMiddleware<MoveFileMiddleware>();

app.UseCookiePolicy(
new CookiePolicyOptions
{
    Secure = CookieSecurePolicy.Always,
    HttpOnly = HttpOnlyPolicy.Always,
    MinimumSameSitePolicy = SameSiteMode.None

});


app.UseHttpsRedirection();

app.UseStaticFiles();

//app.UseMiddleware<ExHandMW>();
//app.UseExceptionHandler("/Home/Error");

app.UseRouting();
app.UseSession();
app.UseAuthentication();
app.UseAuthorization();

//app.UseEndpoints(endpoints =>
//{
//    endpoints.MapDefaultControllerRoute().RequireAuthorization();
//    endpoints.MapRazorPages();
//});

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");

    endpoints.MapRazorPages();
});
app.UseEndpoints(endpoints =>
{
    endpoints.MapHub<ChatHub>("/chatHub");
});

app.MapRazorPages();
app.Run();

