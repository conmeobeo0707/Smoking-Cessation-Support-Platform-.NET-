namespace DAL.Models
{
    public class RatingRequest
    {
        public int RatingValue { get; set; }
        public string FeedbackText { get; set; }
        public long MemberId { get; set; }
        public long PlanId { get; set; }
    }

}