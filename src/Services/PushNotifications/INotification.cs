using System;
namespace MopsterTeams.Services
{
    public interface INotification
    {
        void CreateNotification(string title, string message, string messageAction, string id);
    }
}
