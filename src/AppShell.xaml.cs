using System.Threading.Tasks;
using MopsterTeams.Services;
using MopsterTeams.ViewModels.Base;
using MopsterTeams.Views;
using Microsoft.Maui.Controls.Compatibility;
using Microsoft.Maui.Controls;
using Microsoft.Maui;

namespace MopsterTeams
{
    public partial class AppShell : Microsoft.Maui.Controls.Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));
            Routing.RegisterRoute(nameof(NewPasswordPage), typeof(NewPasswordPage));
            Routing.RegisterRoute(nameof(AppointmentDetailPage), typeof(AppointmentDetailPage));
            Routing.RegisterRoute(nameof(FileReportPage), typeof(FileReportPage));
            Routing.RegisterRoute(nameof(ScannerPage), typeof(ScannerPage));

            ViewModelLocator.Init();
            
            AppNavigation();
        }

        public void SetCurrentTab(int tabIndex)
        {
            if (tabIndex == 0)
            {
                Current.CurrentItem = calendar;
            }
            else if (tabIndex == 1)
            {
                Current.CurrentItem = timeRecords;
            }
            else
            {
                Current.CurrentItem = profile;
            }
        }

        private async void AppNavigation()
        {
            var accountService = DependencyService.Get<IAccountService>();
            var settingsService = DependencyService.Get<ISettingsService>();

            if (string.IsNullOrEmpty(settingsService.BaseUrl))
            {
                await GoToAsync($"//{nameof(WelcomePage)}", false);
            }
            else
            {
                if (accountService.CanDoAutoLogin())
                {
                    var delayTask = Task.Delay(500);
                    var aoTask = accountService.AutoLoginAsync().ContinueWith((aoResult) =>
                    {
                        if (App.Resolve<IGearLoadingIndicatorService>().IsRunning)
                        {
                            App.Resolve<IGearLoadingIndicatorService>().HideGearLoading();
                        } 

                        if (aoResult.Result.IsSuccess)
                        {
                            MainThread.InvokeOnMainThreadAsync(async() =>
                            {
                                await GoToAsync($"//Main/{nameof(CalendarPage)}", false);
                            });
                        }
                        else
                        {
                            MainThread.InvokeOnMainThreadAsync(async() =>
                            {
                                await GoToAsync($"//{nameof(LoginPage)}", false);
                            });
                        }
                    });

                    await Task.WhenAny(delayTask, aoTask);

                    if (!aoTask.IsCompleted)
                    {
                        App.Resolve<IGearLoadingIndicatorService>().ShowGearLoading();
                    }
                }
                else
                {
                    MainThread.InvokeOnMainThreadAsync(async() =>
                    {
                        await GoToAsync($"//{nameof(LoginPage)}", false);
                    });
                }
            }
        }
    }
}
