using System.Collections.Generic;

namespace Smartsheet.Api.Models
{
    public class UserPlansResponse
    {
        public List<UserPlan> Data { get; set; }
        public string LastKey { get; set; }
    }
}