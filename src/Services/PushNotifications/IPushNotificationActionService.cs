using System;
using System.Threading.Tasks;
using MopsterTeams.Models;

namespace MopsterTeams.Services
{
    public interface IPushNotificationActionService : INotificationActionService
    {
        event EventHandler<PushNotificationModel> ActionTriggered;
        event Action RegistrationRefreshTriggered;
        int? GetInvocationList();
        //Task<NotificationCallParamModel> GetNotificationCallParamAsync(string phoneNumber);
    }
}
