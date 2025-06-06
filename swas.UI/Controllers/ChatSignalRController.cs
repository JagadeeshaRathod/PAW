using ASPNetCoreIdentityCustomFields.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using swas.BAL.DTO;
using swas.BAL.Helpers;
using swas.BAL.Interfaces;
using swas.BAL.Utility;
using swas.DAL.Models;
using System.Security.Claims;

namespace swas.UI.Controllers
{
    public class ChatSignalRController : Controller
    {

        private readonly IUserMapChatRepository _userMapChatRepository;
        private readonly ITrnChatMsgRepository _trnChatMsg;
        private readonly IHubContext<ChatHub> _hubContext;
        //private readonly UserManager<ApplicationUser> _userManager;
        //private readonly IHttpContextAccessor _httpContextAccessor;
        public ChatSignalRController(
            UserManager<ApplicationUser> userManager,
            IUserMapChatRepository userMapChatRepository,
            IHttpContextAccessor httpContextAccessor,
            ITrnChatMsgRepository trnChatMsg,
            IHubContext<ChatHub> hubContext
            )
        {

            //_userManager = userManager;
            _userMapChatRepository = userMapChatRepository;
            //_httpContextAccessor = httpContextAccessor;
            _trnChatMsg = trnChatMsg;
            _hubContext = hubContext;
        }
        //public async Task<IActionResult> Index()
        //{
        //    return View();
        //}

        //public async Task<IActionResult> GetAllUsers(int Id)
        //{
        //    try
        //    {
        //        //var userdet = await _userManager.Users.ToListAsync();
        //        string id = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
        //        //var userdet = await _userManager.Users.ToListAsync();
        //        var userdet = await _trnChatMsg.GetAllUsers();
        //        var ischat = await _trnChatMsg.GetIsChat(id);
        //        //  var orderby = await _trnChatMsg.GetAllUserLastchatDateFororderBy();
        //        List<DTOApplicationUserWithChatRead> lstuser = new List<DTOApplicationUserWithChatRead>();
        //        if (userdet.Count > 0)
        //        {
        //            foreach (var user in userdet)
        //            {
        //                if (id != user.Id)
        //                {
        //                    DTOApplicationUserWithChatRead db = new DTOApplicationUserWithChatRead();
        //                    db.Id = user.Id;
        //                    db.Rank = user.Rank;
        //                    db.Offr_Name = user.Offr_Name;
        //                    db.UserName = user.UserName;
        //                    if (ischat != null && ischat.Count > 0)
        //                    {
        //                        DTOIsChat dTOIsChat = new DTOIsChat();
        //                        dTOIsChat = ischat.Where(i => i.FromUserID == user.Id).FirstOrDefault();
        //                        if (dTOIsChat != null)
        //                        {
        //                            db.Total = dTOIsChat.Total;
        //                            db.CreatedOn = dTOIsChat.CreatedOn;
        //                        }
        //                    }
        //                    db.CreatedDate = user.CreatedDate;
        //                    lstuser.Add(db);
        //                }
        //            }
        //        }
        //        return Json(lstuser.OrderByDescending(i => i.Total).OrderByDescending(i => i.CreatedOn));
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(nmum.Exception);
        //    }
        //}

        //public async Task<IActionResult> GetAllUsers(int id)
        //{
        //    var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
        //    var users = await _trnChatMsg.GetAllUsers();
        //    var isChats = await _trnChatMsg.GetIsChat(userId);

        //    var userWithChatData = users
        //        .Where(user => userId != user.Id)
        //        .Select(user =>
        //        {
        //            var chat = isChats.FirstOrDefault(c => c.FromUserID == user.Id);
        //            return new DTOApplicationUserWithChatRead
        //            {
        //                Id = user.Id,
        //                Rank = user.Rank,
        //                Offr_Name = user.Offr_Name,
        //                UserName = user.UserName,
        //                CreatedDate = user.CreatedDate,
        //                Total = chat?.Total ?? 0,
        //                CreatedOn = chat?.CreatedOn ?? DateTime.Now
        //            };
        //        })
        //        .OrderByDescending(u => u.Total)
        //        .ThenByDescending(u => u.CreatedOn)
        //        .ToList();

        //    return Json(userWithChatData);
        //}
        //public async Task<IActionResult> SaveUserMapChat(mUserMapChat mUserMapChat)
        //{
        //    var id = this.User.FindFirstValue(ClaimTypes.NameIdentifier).ToLower();
        //    mUserMapChat.FromUserId = id;
        //    mUserMapChat.ToUserId = mUserMapChat.ToUserId.ToLower();
             
        //    var mapDetails = await _userMapChatRepository.GetMapDetails(mUserMapChat);
        //    var response = mapDetails ?? await _userMapChatRepository.AddWithReturn(mUserMapChat);

        //    return Json(response);
        //}

        //public async Task<IActionResult> SaveChat(TrnChatMsg trnChatMsg)
        //{
        //    trnChatMsg.ChatId = 0;
        //    trnChatMsg.CreatedOn = DateTime.Now;
        //    trnChatMsg.IsRead = false;

        //    var savedChat = await _trnChatMsg.AddWithReturn(trnChatMsg);

        //    // Broadcast message via SignalR
        //    await _hubContext.Clients.All.SendAsync("ReceiveMessage", trnChatMsg.UserMapChatId, trnChatMsg.Msg);

        //    return Json(savedChat);
        //}

        //public async Task<IActionResult> GetUserMapChat(int userMapChatId, string fromUserId)
        //{
        //    var toUserId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
        //    var chatHistory = await _trnChatMsg.GetChat(userMapChatId, fromUserId, toUserId);

        //    return Json(chatHistory);
        //}
    }
}
