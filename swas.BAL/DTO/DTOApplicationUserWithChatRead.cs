using ASPNetCoreIdentityCustomFields.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace swas.BAL.DTO
{
	public class DTOApplicationUserWithChatRead : ApplicationUser
	{
		public string FromUserID { get; set; }
		public int Total { get; set; }
		public DateTime CreatedOn { get; set; }
		public string? sorton { get; set; }
        public string? RankName { get; set; }
    }
}
