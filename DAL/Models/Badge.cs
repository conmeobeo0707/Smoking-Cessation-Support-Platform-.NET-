using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class Badge
    {
        public int BadgeId { get; set; }
        public string BadgeName { get; set; }
        public string Description { get; set; }
        public string Criteria { get; set; }
        public string BadgeType { get; set; }
    }
}
