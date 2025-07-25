using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class MemberPackage
    {
        public int MemberPackageId { get; set; }
        public string PackageName { get; set; }
        public decimal Price { get; set; }
        public int Duration { get; set; } // đơn vị là tháng, nếu có thể thêm chú thích
        public string FeaturesDescription { get; set; }
    }
}
