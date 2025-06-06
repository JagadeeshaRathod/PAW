using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace swas.DAL.Models
{
    public class TrnChatMsg
    {
        [Key]
        public int ChatId { get; set; }

        [ForeignKey("mUserMapChat")]
        public int UserMapChatId { get; set; }
        public string Msg { get; set; } = string.Empty;
        public DateTime CreatedOn { get; set; }
        [NotMapped]
        public int type { get; set; }

        public bool IsRead { get; set; }
    }
}
