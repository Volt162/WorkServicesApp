using System;
using MopsterTeams.Models;

namespace MopsterTeams.Services
{
    public interface IDeviceInstallationService
    {
        string Token { get; set; }
        bool NotificationsSupported { get; }
        string GetDeviceId();
        DeviceInstallation GetDeviceInstallation();
        bool RegisteredForNotifications();
    }
}
