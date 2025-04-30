using MopsterMopsterTeams.Views;
using MopsterTeams.Services;
using MopsterTeams.ViewModels.Base;
using Microsoft.Maui.Controls.PlatformConfiguration;
using Microsoft.Maui.Controls.PlatformConfiguration.iOSSpecific;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Controls.Compatibility;
using Microsoft.Maui.Controls;
using Microsoft.Maui;

namespace MopsterTeams.Views.Base
{
    public abstract class ContentPageBase : ContentPage
    {
        private bool isLoadingIndicatorRunning = false;
        private readonly IGearLoadingIndicatorService _gearLoadingIndicatorService;

        public ContentPageBase()
        {
            Microsoft.Maui.Controls.NavigationPage.SetBackButtonTitle (this, string.Empty);
            ViewModelLocator.SetAutoWireViewModel(this, true);

            _gearLoadingIndicatorService = DependencyService.Get<IGearLoadingIndicatorService>();
            
            BackgroundColor = Colors.White;
            Shell.SetNavBarIsVisible(this, false);
            Shell.SetFlyoutBehavior(this, FlyoutBehavior.Disabled);
        }

        #region -- Overrides --

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            if (BindingContext is ViewModelBase vmb)
            {
                if (vmb.MultipleInitialization)
                {
                    await vmb.InitializeAsync(default(IDictionary<string, string>));
                }
            }

            var actionsHandler = BindingContext as IViewActionsHandler;

            if (actionsHandler != null && !isLoadingIndicatorRunning)
            {
                actionsHandler.OnAppearing();
            }

            isLoadingIndicatorRunning = false;
        }

        protected override void OnDisappearing()
        {
            isLoadingIndicatorRunning = _gearLoadingIndicatorService.IsRunning;
            base.OnDisappearing();

            var actionsHandler = BindingContext as IViewActionsHandler;
            if (actionsHandler != null)
            {
                actionsHandler.OnDisappearing();
            }
        }

        protected override bool OnBackButtonPressed()
        {
            DependencyService.Get<Services.INavigationService>().GoBackAsync();
            return true;
        }

        #endregion
    }
}
