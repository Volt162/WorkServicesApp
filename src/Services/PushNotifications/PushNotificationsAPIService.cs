using System;
using System.Threading.Tasks;
using MopsterTeams.Enums;
using MopsterTeams.Models;
using Newtonsoft.Json;
using Microsoft.Maui.ApplicationModel;

namespace MopsterTeams.Services
{
    public class PushNotificationsAPIService : BaseServiceWithAuthorization, IPushNotificationsAPIService
    {
        public PushNotificationsAPIService(IRestService rest,
             ISettingsService settingsManager,
             IInternetConnectionService internetConnectionService) : 
            base(rest, settingsManager, internetConnectionService)
         {

         }

        public async Task<AOResult<string>> RegisterAppForPushNotificationsAsync(string deviceId, string token, EPushNotificationPlatform platform)
        {
            var res = new AOResult<string>();

            var model = new RegisterPushNotificationModel()
            {
                DeviceId = deviceId,
                Token = token,
                Platform = platform
            };

            try
            {
                var serverResponse = await PostAsync<string>($"{SettingsService.BaseUrl}/api/Employee/RegisterEmployeeAppForPushNotifications", model, null);

                if (serverResponse != null)
                {
                    res.SetSuccess(serverResponse);
                }
                else
                {
                    res.SetFailure();
                }
            }
            catch (Exception ex)
            {
                res.SetError("RegisterAppForPushNotificationsAsync_Exception", ex.Message, ex);
            }

            return res;
        }

        public async Task<AOResult<string>> UnregisterAppForPushNotificationsAsync(string deviceId)
        {
            var res = new AOResult<string>();

            var model = new DeviceInfoForPushNotificationModel()
            {
                DeviceId = deviceId,
            };

            try
            {
                var serverResponse = await PostAsync<string>($"{SettingsService.BaseUrl}/api/Employee/UnregisterEmployeeAppForPushNotifications", model, null);

                if (serverResponse != null)
                {
                    res.SetSuccess(serverResponse);
                }
                else
                {
                    res.SetFailure();
                }
            }
            catch (Exception ex)
            {
                res.SetError("UnregisterAppForPushNotificationsAsync_Exception", ex.Message, ex);
            }

            return res;
        }
    }

    public class DeviceInfoForPushNotificationModel
    {
        [JsonProperty("deviceId")]
        public string DeviceId { get; set; }
    }

    public class RegisterPushNotificationModel : DeviceInfoForPushNotificationModel
    {
        [JsonProperty("token")]
        public string Token { get; set; }

        [JsonProperty("platform")]
        public EPushNotificationPlatform Platform { get; set; }
        
    }
}
