using System.Text.Json.Serialization;

public class UserBadge
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int BadgeId { get; set; }

    [JsonPropertyName("dateAchieved")]
    public DateTime? AchievedDate { get; set; }

    [JsonPropertyName("shared")]
    public bool? IsShared { get; set; }

    [JsonPropertyName("badgeName")]
    public string BadgeName { get; set; }

    [JsonPropertyName("badgeDescription")]
    public string Description { get; set; }
}