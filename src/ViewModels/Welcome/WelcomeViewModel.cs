using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using MopsterTeams.Enums;
using MopsterTeams.Helpers;
using MopsterTeams.Models;
using MopsterTeams.Resources.Strings;
using MopsterTeams.Services;
using MopsterTeams.ViewModels.Base;
using MopsterTeams.Views;
using static Microsoft.Maui.ApplicationModel.Permissions;
using Microsoft.Maui.Controls.Compatibility;
using Microsoft.Maui.Controls;
using Microsoft.Maui;
using Microsoft.Maui.ApplicationModel;
using FFImageLoading.Helpers;
using ZXing.Net.Maui;

namespace MopsterTeams.ViewModels
{
    public class WelcomeViewModel : ViewModelBase
    {
        private readonly IAccountService _accountService;

        public WelcomeViewModel(IAccountService accountService)
        {
            _accountService = accountService;

            var e = DependencyService.Get<IEnvironment>();
            if (App.Current.RequestedTheme == AppTheme.Dark)
            {
                e?.UpdateStatusBarColor(StyleManager.GetAppResource<Color>("Primary_2"), false);
            }
            else
            {
                e?.UpdateStatusBarColor(StyleManager.GetAppResource<Color>("Primary_2"), false);
            }

            OnBarcodeScannedCommand = new Command<BarcodeDetectionEventArgs>(OnBarcodeScanned);
        }

        #region -- Public properties --

        private EButtonState _CheckInState = EButtonState.DefaultState;
        public EButtonState CheckInState
        {
            get => _CheckInState;
            set => SetProperty(ref _CheckInState, value);
        }

        bool _isProcessing = false;

        private bool _isAnalyzing = true;
        public bool IsAnalyzing
        {
            get { return _isAnalyzing; }
            set { SetProperty(ref _isAnalyzing, value); }
        }

        private bool _isScanning = true;
        public bool IsScanning
        {
            get { return _isScanning; }
            set { SetProperty(ref _isScanning, value); }
        }

        private string _BaseUrl;
        public string BaseUrl
        {
            get { return _BaseUrl; }
            set { SetProperty(ref _BaseUrl, value); CheckUrl(); }
        }

        private ZXing.Result _result;
        public ZXing.Result Result
        {
            get { return _result; }
            set { SetProperty(ref _result, value); }
        }

        public ICommand OnBarcodeScannedCommand { get; set; }

        private ICommand _ScanCommand;
        public ICommand ScanCommand
        {
            get { return _ScanCommand ?? (_ScanCommand = SingleExecutionCommand.FromFunc(OnScanCommand)); }
        }

        private ICommand _GoToLoginCommand;
        public ICommand GoToLoginCommand
        {
            get { return _GoToLoginCommand ?? (_GoToLoginCommand = SingleExecutionCommand.FromFunc(OnGoToLoginCommand)); }
        }

        private ICommand _GoBackCommand;
        public ICommand GoBackCommand
        {
            get { return _GoBackCommand ?? (_GoBackCommand = new Command(OnGoBackCommand)); }
        }

        #endregion

        #region -- Overrides --

        public override void OnAppearing()
        {
            base.OnAppearing();
        }

        public override Task InitializeAsync (IDictionary<string, string> query)
        {
            if (query.ContainsKey(nameof(BaseUrl)))
            {
                BaseUrl = Uri.UnescapeDataString(query[nameof(BaseUrl)]);
            }

            return Task.FromResult<bool>(true);
        }

        #endregion


        #region -- Private helpers --

        private void OnGoBackCommand()
        {
            if (Device.RuntimePlatform == Device.iOS)
            {
                NavigationService.GoBackAsync();
            }
            else
            {
                Shell.Current.GoToAsync("..", false);
            }
        }

        private void CheckUrl()
        {
            string pattern = URL_PATTERN;
            Regex reg = new Regex(pattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);
            bool result = reg.IsMatch(BaseUrl);

            if (result)
            {
                CheckInState = EButtonState.DisableState;
            }
            else
            {
                CheckInState = EButtonState.DefaultState;
            }
        }

        private async Task OnGoToLoginCommand()
        {

            AOResult versRes = null;

            IsBusy = true;
            await Task.Run(async () => { versRes = await _accountService.GetVersion(BaseUrl.Trim().ToLower()).ConfigureAwait(false); });
            IsBusy = false;

            if (versRes.IsSuccess)
            {
                await NavigationService.ResetNavStackAndNavigateToAsync($"{nameof(LoginPage)}");
            }
            else
            {
                await DialogService.ShowAlertAsync(Translation.Translate("InvalidPortalAddress"), Translation.Translate("Warning"), Translation.Translate("Ok"));
            }

        }

        private async Task OnScanCommand()
        {
            IsScanning = true;
            IsAnalyzing = true;

            var status = await CheckAndRequestPermissionAsync(new Permissions.Camera());
            if (status == PermissionStatus.Granted)
            {
                await Application.Current.MainPage.Navigation.PushModalAsync(new ScannerPage() { BindingContext = this }, true);
            }
            else
            {
                await DialogService.ShowAlertAsync(Translation.Translate("PermissionsDenied"), Translation.Translate("UnableTakePhoto"), Translation.Translate("Ok"));
                AppInfo.ShowSettingsUI();
            }
        }

        private async Task<PermissionStatus> CheckAndRequestPermissionAsync<T>(T permission) where T : BasePermission
        {
            var status = await permission.CheckStatusAsync();
            if (status != PermissionStatus.Granted)
            {
                status = await permission.RequestAsync();
            }

            return status;
        }

        private void OnBarcodeScanned(BarcodeDetectionEventArgs arg)
        {
            if (!_isProcessing)
            {
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    _isProcessing = true;
                    IsAnalyzing = false;
                    IsScanning = false;

                    OnGoBackCommand();

                    BaseUrl = arg.Results.First().Value;

                    _isProcessing = false;
                });
            }
        }

        #endregion
    }
}