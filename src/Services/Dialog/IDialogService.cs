using System.Threading.Tasks;

namespace MopsterTeams.Services
{
    public interface IDialogService
    {
        Task ShowAlertAsync(string message, string title, string buttonLabel);
        Task<string> ShowActionSheetAsync(string title, string cancel, string description, params string[] buttons);
    }
}
