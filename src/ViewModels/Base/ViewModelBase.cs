using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MopsterMopsterTeams.Views;
using MopsterTeams.Services;
using Microsoft.Maui.Controls.Compatibility;
using Microsoft.Maui.Controls;
using Microsoft.Maui;

namespace MopsterTeams.ViewModels.Base
{
    public abstract class ViewModelBase : BindableBase, IQueryAttributable, IViewActionsHandler
    {
        protected readonly IDialogService DialogService;
        protected readonly INavigationService NavigationService;
        protected readonly IGearLoadingIndicatorService GearLoadingIndicatorService;

        private bool _isInitialized;
        public bool IsInitialized
        {
            get => _isInitialized;

            set
            {
                _isInitialized = value;
                RaisePropertyChanged(nameof(IsInitialized));
            }
        }

        private bool _multipleInitialization;
        public bool MultipleInitialization
        {
            get => _multipleInitialization;

            set
            {
                _multipleInitialization = value;
                RaisePropertyChanged(nameof(MultipleInitialization));
            }
        }

        private bool _isBusy;
        public bool IsBusy
        {
            get => _isBusy;

            set
            {
                _isBusy = value;
                UpdateLoadingIndicator();
                RaisePropertyChanged(nameof(IsBusy));
            }
        }

        private void UpdateLoadingIndicator()
        {
            if (IsBusy)
            {
                GearLoadingIndicatorService.ShowGearLoading();
            }
            else
            {
                GearLoadingIndicatorService.HideGearLoading();
            }
        }

        public ViewModelBase()
        {
            DialogService = ViewModelLocator.Resolve<IDialogService>();
            NavigationService = ViewModelLocator.Resolve<INavigationService>();
            GearLoadingIndicatorService = ViewModelLocator.Resolve<IGearLoadingIndicatorService>();

            var settingsService = ViewModelLocator.Resolve<ISettingsService>();
        }

        public virtual Task InitializeAsync (IDictionary<string, string> query)
        {
            return Task.FromResult (false);
        }

        public virtual Task InitializeAsync(IDictionary<string, object> query)
        {
            return Task.FromResult(false);
        }

        #region -- IViewActionsHandler implementation --

        public virtual void OnAppearing()
        {
        }

        public virtual void OnDisappearing()
        {
        }

        #endregion

        #region -- IQueryAttributable implementation --

        public async void ApplyQueryAttributes (IDictionary<string, string> query)
        {
            if(!IsInitialized)
            {
                IsInitialized = true;
                await InitializeAsync (query);
            }
        }

        public async void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (!IsInitialized)
            {
                IsInitialized = true;
                await InitializeAsync(query);
            }
        }

        #endregion
    }
}