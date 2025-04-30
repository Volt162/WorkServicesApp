using System;
using System.Threading.Tasks;
using MopsterTeams.Enums;
using MopsterTeams.Models;

namespace MopsterTeams.Services
{
    public interface IPushNotificationsAPIService
    {
        Task<AOResult<string>> RegisterAppForPushNotificationsAsync(string deviceId, string token, EPushNotificationPlatform platform);
        Task<AOResult<string>> UnregisterAppForPushNotificationsAsync(string deviceId);
    }
}
