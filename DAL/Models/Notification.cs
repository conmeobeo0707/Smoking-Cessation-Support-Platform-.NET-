using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class Notification
    {
        public int NotificationId { get; set; }
        public string? Content { get; set; }
        public string? NotificationType { get; set; }
        public DateTime SendDate { get; set; }
        public string? Status { get; set; }
        public long UserId { get; set; }
        public int? QuitPlanId { get; set; }
        public int? AchievementBadgeId { get; set; }

        public bool IsUnread => Status?.ToUpper() == "UNREAD";
    }
}