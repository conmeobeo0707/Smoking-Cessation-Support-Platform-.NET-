using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class QuitPlan
    {
        [JsonPropertyName("planId")]
        public int Id { get; set; }

        [JsonPropertyName("title")]
        public string PlanName { get; set; }

        [JsonPropertyName("startDate")]
        public DateTime? StartDate { get; set; }

        [JsonPropertyName("expectedEndDate")]
        public DateTime? EndDate { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonPropertyName("reason")]
        public string Reason { get; set; }

        [JsonPropertyName("stagesDescription")]
        public string StagesDescription { get; set; }

        [JsonPropertyName("customNotes")]
        public string CustomNotes { get; set; }

        [JsonPropertyName("userId")]
        public int UserId { get; set; }

        [JsonPropertyName("coachId")]
        public int? CoachId { get; set; }

        [JsonPropertyName("recommendedPackageId")]
        public int? RecommendedPackageId { get; set; }

    }
}
