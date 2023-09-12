using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RealTimeChatApi.DataAccessLayer.Models
{
    public class Message
    {
        [Key]
        public int messageId { get; set; }
        public string senderId { get; set; }
        public string receiverId { get; set; }
        public string content { get; set; }
        public DateTime timestamp { get; set; }

        [ForeignKey("senderId")]
        public virtual AppUser sender { get; set; }
        [ForeignKey("receiverId")]
        public virtual AppUser receiver { get; set; }
    }
}
