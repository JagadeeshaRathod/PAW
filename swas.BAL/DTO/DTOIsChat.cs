using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace swas.BAL.DTO
{
	public class DTOIsChat
	{
        public string FromUserID { get; set; }
        public int Total { get; set; }
        //public bool IsRead { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
