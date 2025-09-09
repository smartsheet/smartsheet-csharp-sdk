namespace Smartsheet.Api.Models
{
    public class UserPlan
    {
        public string PlanId { get; set; }
        public string SeatType { get; set; }
        public string SeatTypeLastChangedAt { get; set; }
        public bool IsInternal { get; set; }
    }
}