using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class Rating
    {
        public int Id { get; set; }
        public int QuitPlanId { get; set; }
        public int UserId { get; set; }
        public int Rate { get; set; }
        public string Comment { get; set; }
        public DateTime? CreatedAt { get; set; }
    }
}