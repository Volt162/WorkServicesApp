using System;
using System.Threading.Tasks;
using MopsterTeams.Resources.Strings;
using Microsoft.Maui.Networking;

namespace MopsterTeams.Services
{
    public class InternetConnectionService : IInternetConnectionService
    {
        private readonly IDialogService _userDialogs;

        public InternetConnectionService(IDialogService userDialogs)
        {
            _userDialogs = userDialogs;
        }

        #region -- IInternetConnectionService Implementation --

        public async Task<bool> CheckInternetConnectionAsync()
        {
            var current = Connectivity.NetworkAccess;

            if (current == NetworkAccess.Internet)
            {
                return true;
            }
            else
            {
                await _userDialogs.ShowAlertAsync(Translation.Translate("LostConnection"), Translation.Translate("Warning"), Translation.Translate("Ok"));
                return false;
            }
        }

        public Task<bool> CheckInternetConnectionWithoutErrorMessageAsync()
        {
            return Task.FromResult<bool>(Connectivity.NetworkAccess == NetworkAccess.Internet);
        }

        #endregion
    }
}
