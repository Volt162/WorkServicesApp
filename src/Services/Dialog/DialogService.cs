using System.Threading.Tasks;
using Microsoft.Maui.Controls.Compatibility;
using Microsoft.Maui.Controls;
using Microsoft.Maui;

namespace MopsterTeams.Services
{
    public class DialogService : IDialogService
    {
        public async Task ShowAlertAsync(string message, string title, string buttonLabel)
        {
            await App.Current.MainPage.DisplayAlert(title, message, buttonLabel);
        }

        public Task<string> ShowActionSheetAsync(string title, string cancel, string description, params string[] buttons)
        {
            return App.Current.MainPage.DisplayActionSheet(title, cancel, description, buttons);
        }
    }
}
