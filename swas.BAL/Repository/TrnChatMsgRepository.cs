using Microsoft.AspNetCore.Http;
using swas.BAL.Interfaces;
using swas.DAL.Models;
using swas.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using swas.BAL.DTO;
using static Grpc.Core.Metadata;
using System.Collections;
using System.Security.Cryptography;
using swas.BAL.Helpers;

namespace swas.BAL.Repository
{
    public class TrnChatMsgRepository : GenericRepositoryDL<TrnChatMsg>, ITrnChatMsgRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public TrnChatMsgRepository(ApplicationDbContext context) : base(context)
        {
            _dbContext = context;

        }

        public async Task<List<TrnChatMsg>> GetChat(int UserMapChatId,string FromUserId, string ToUserId)
        {

            List<TrnChatMsg> trnChatMsgs = new List<TrnChatMsg>();  
            var ret = await _context.mUserMapChat.Where(i => i.FromUserId == FromUserId && i.ToUserId==ToUserId).SingleOrDefaultAsync();
			if(ret!=null)
			{
				var friends = _dbContext.TrnChatMsg.Where(f => f.UserMapChatId==ret.UserMapChatId).ToList();
				friends.ForEach(a => a.IsRead = true);
			    _dbContext.SaveChanges();
			}
			
			var chat = await (from msg in _context.TrnChatMsg

							  where msg.UserMapChatId == UserMapChatId

							 select new TrnChatMsg
                             {
								ChatId = msg.ChatId,
								UserMapChatId = msg.UserMapChatId,
								Msg = msg.Msg,
								CreatedOn = msg.CreatedOn,
								type = 1,
								IsRead= msg.IsRead
							 }).ToListAsync();

			//var chat = await _context.TrnChatMsg.Where(i => i.UserMapChatId == UserMapChatId).ToListAsync();
			//var chat1 = await _context.TrnChatMsg.Where(i => i.UserMapChatId == ret.UserMapChatId).ToListAsync();
			if(ret!=null)
			{
				var chat1 = await (from msg in _context.TrnChatMsg

								   where msg.UserMapChatId == ret.UserMapChatId

								   select new TrnChatMsg
								   {
									   ChatId= msg.ChatId,
									   UserMapChatId = msg.UserMapChatId,
									   Msg = msg.Msg,
									   CreatedOn = msg.CreatedOn,
									   type = 2,
									   IsRead = msg.IsRead
								   }).ToListAsync();

				chat.AddRange(chat1);
			}
			//var retww = chat.OrderByDescending(i => i.CreatedOn);
			return chat.OrderBy(i => i.CreatedOn).ToList();
		}

		public async Task<List<DTOIsChat>> GetIsChat(string ToUserId)
		{
			var ret = await (from map in _context.mUserMapChat
							 join chat in _context.TrnChatMsg on map.UserMapChatId equals chat.UserMapChatId
							 where chat.IsRead == false && map.ToUserId == ToUserId
							 group map by new { map.FromUserId, chat.CreatedOn } into g
							 select new DTOIsChat
							 {
								 FromUserID = g.Key.FromUserId,
								 CreatedOn = g.Key.CreatedOn,
								 Total = g.Count()
							 }).ToListAsync();
            var data = ret.OrderByDescending(i => i.CreatedOn).ToList();
			return data;
		}
		public async Task<List<DTOTrnChatOrderby>> GetAllUserLastchatDateFororderBy()
		{
			var ret =await (from map in _context.mUserMapChat
							join trnchat in _context.TrnChatMsg on map.UserMapChatId equals trnchat.UserMapChatId
                            let datetime = (from map1 in _context.mUserMapChat
                                            join trnchat1 in _context.TrnChatMsg on map1.UserMapChatId equals trnchat1.UserMapChatId
											where map1.ToUserId== map.ToUserId
                                            orderby trnchat1.CreatedOn descending
                                            select trnchat1.CreatedOn).FirstOrDefault()
                            group map by new
                            {
                                map.ToUserId,
                                datetime

                            } into gr
                             select new DTOTrnChatOrderby
                             {
                                 ToUserId = gr.Key.ToUserId,
								 CreatedOn = gr.Key.datetime,               
                             }).ToListAsync();
            return ret;
        }
        //public async Task<List<DTOApplicationUserWithChatRead>> GetAllUsers()
        //{
        //	try
        //	{
        //              var ret = await (from users in _context.Users
        //                                   //join mapchat in _context.mUserMapChat on users.Id equals mapchat.ToUserId
        //                               let datetime = (from map1 in _context.mUserMapChat
        //                                               join trnchat1 in _context.TrnChatMsg on map1.UserMapChatId equals trnchat1.UserMapChatId
        //                                               orderby trnchat1.CreatedOn descending
        //                                               where users.Id == map1.ToUserId
        //                                               select trnchat1.CreatedOn).FirstOrDefault()

        //                               select new DTOApplicationUserWithChatRead
        //                               {
        //                                   Id = users.Id,
        //                                   Rank = users.Rank,
        //                                   Offr_Name = users.Offr_Name,
        //                                   UserName = users.UserName,

        //                                   CreatedDate = datetime
        //                               }).ToListAsync();
        //              return ret;

        //          }
        //          catch (Exception ex	)
        //	{

        //		throw ex;
        //	}			
        //}
        public async Task<List<DTOApplicationUserWithChatRead>> GetAllUsers()
        {
            var commonHelper = new CommonHelper(_context);
            try
            {
                var ret = await (from users in _context.Users
                                 let datetime = (from map1 in _context.mUserMapChat
                                                 join trnchat1 in _context.TrnChatMsg on map1.UserMapChatId equals trnchat1.UserMapChatId
                                                 where (users.Id == map1.ToUserId ) || (users.Id == map1.FromUserId )
                                                 orderby trnchat1.ChatId descending
                                                 select (DateTime?)trnchat1.CreatedOn).FirstOrDefault()

                                 select new DTOApplicationUserWithChatRead
                                 {
                                     
                                     Id = users.Id,
                                     //Rank = users.Rank 
                                     RankName = commonHelper.GetRankByRankId(users.Rank),
                                     Offr_Name = users.Offr_Name,
                                     UserName = users.UserName,
                                     CreatedDate = datetime 
                                 }).ToListAsync();

                return ret.OrderByDescending(i => i.CreatedDate).ToList();
            }
            catch (Exception)
            {
                return new List<DTOApplicationUserWithChatRead>();
            }
        }
    }
}
