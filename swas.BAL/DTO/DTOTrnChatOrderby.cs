using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace swas.BAL.DTO
{
    public class DTOTrnChatOrderby
    {
        public DateTime CreatedOn { get; set; }
        public string ToUserId { get; set; }
    }
}
