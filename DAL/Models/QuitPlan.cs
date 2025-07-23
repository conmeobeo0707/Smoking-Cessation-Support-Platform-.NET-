using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class QuitPlan
    {
        public int Id { get; set; }
        public string PlanName { get; set; }
        public string Description { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string PlanType { get; set; }
        public bool? IsFree { get; set; }
        public int? UserId { get; set; }
        public string Status { get; set; }
    }
}
