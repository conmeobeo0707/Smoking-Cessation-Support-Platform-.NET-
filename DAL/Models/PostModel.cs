using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class PostModel
    {
        public int PostId { get; set; }
        public string? Title { get; set; }
        public string? Content { get; set; }
        public string? Status { get; set; }

        public int UserId { get; set; }          
        public string? UserName { get; set; }    

        public DateTime PostDate { get; set; }   
    }
}



