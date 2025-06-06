using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace swas.DAL.Models
{
    public class Notification
    {
        [Key]
        public int NotificationId { get; set; }

        [ForeignKey("tbl_Projects")]
        public int ProjId { get; set; }
        public int NotificationFrom { get; set; }
        public int NotificationTo { get; set; }
        public bool IsRead { get; set; }
        [Display(Name = "Date of Update")]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? ReadDateTime { get; set; }

        public int NotificationType   { get; set; }

        public bool IsDeleted  { get; set; }

    }
}
