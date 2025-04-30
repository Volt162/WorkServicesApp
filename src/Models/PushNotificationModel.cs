using System;
using MopsterTeams.Enums;

namespace MopsterTeams.Models
{
    public class PushNotificationModel
    {
        public ENotificationAction ActionType { get; set; }
        public string ActionId { get; set; }
        public string Parameter { get; set; }
    }
}
