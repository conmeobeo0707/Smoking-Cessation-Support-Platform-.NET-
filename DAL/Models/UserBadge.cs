using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class UserBadge
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int BadgeId { get; set; }
        public DateTime? AchievedDate { get; set; }
        public bool? IsShared { get; set; }
    }
}
