using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class QuitPlanBasicModel
    {
        public int PlanId { get; set; }
        public string Title { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime ExpectedEndDate { get; set; }
        public string Status { get; set; }

        public int UserId { get; set; }         // ID người dùng tạo kế hoạch
        public string UserName { get; set; }    // Tên người dùng (nếu API trả về)
        public int? CoachId { get; set; }
    }
}
