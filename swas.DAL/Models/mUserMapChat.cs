﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace swas.DAL.Models
{
    public class mUserMapChat
    {
        [Key]
        public int UserMapChatId { get; set; }
        public string FromUserId { get; set; }
        public string ToUserId { get; set; }
    }
}
