using System;
namespace MopsterTeams.Services
{
    public interface INotificationActionService
    {
        void TriggerAction(string action, string id, string itemtype);
        void TriggerRegistrationRefresh();
    }
}
