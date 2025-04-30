using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using MopsterTeams.Enums;
using MopsterTeams.Helpers;
using MopsterTeams.Models;
using MopsterTeams.Resources.Strings;
using MopsterTeams.Services;
using MopsterTeams.ViewModels.AppointmentDetails;
using MopsterTeams.ViewModels.Base;
using MopsterTeams.Views;
using WinofficePrimeMobile.ViewModels;
using Microsoft.Maui.Controls.Compatibility;
using Microsoft.Maui.Controls;
using Microsoft.Maui;
using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.ApplicationModel.Communication;
using Microsoft.Maui.Storage;

namespace MopsterTeams.ViewModels
{
    public class AppointmentDetailViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IAppointmentService _appointmentService;
        private readonly ISettingsService _settingsService;

        private bool _isSales;

        public AppointmentDetailViewModel(
            INavigationService navigationService,
            IAppointmentService appointmentService,
            ISettingsService settingsService,
            )
        {
            _navigationService = navigationService;
            _appointmentService = appointmentService;
            _settingsService = settingsService;

        }

        #region -- Public properties --

        private bool _IsInitialize = true;
        public bool IsInitialize
        {
            get => _IsInitialize;
            set => SetProperty(ref _IsInitialize, value);
        }
        
        public ECalendarCellType CellType { get; set; }

        private string _Title;
        public string Title
        {
            get => _Title;
            set => SetProperty(ref _Title, value);
        }

        private EButtonState _CheckInState = EButtonState.DefaultState;
        public EButtonState CheckInState
        {
            get => _CheckInState;
            set => SetProperty(ref _CheckInState, value);
        }

        private string _itemId;
        public string ItemId
        {
            get => _itemId;
            set => SetProperty(ref _itemId, value);
        }

        private string _SecondTabTitle = Translation.Translate("Description");
        public string SecondTabTitle
        {
            get => _SecondTabTitle;
            set => SetProperty(ref _SecondTabTitle, value);
        }

        private IList<AppointmentViewModel> _CarouselTabs;
        public IList<AppointmentViewModel> CarouselTabs
        {
            get => _CarouselTabs;
            set => SetProperty(ref _CarouselTabs, value);
        }

        private int _PositionSelected = 0;
        public int PositionSelected
        {
            get => _PositionSelected;
            set => SetProperty(ref _PositionSelected, value);
        }

        private ICommand _GoBackCommand;
        public ICommand GoBackCommand
        {
            get { return _GoBackCommand ?? (_GoBackCommand = new Command(OnGoBackCommand)); }
        }

        private ICommand _CheckInCommand;
        public ICommand CheckInCommand
        {
            get { return _CheckInCommand ?? (_CheckInCommand = new Command(OnCheckInCommand)); }
        }

        public ICommand SelectItemCommand { get => new Command<string>((param) => PositionSelected = int.Parse(param)); }

        private ICommand _FileReportCommand;
        public ICommand FileReportCommand
        {
            get { return _FileReportCommand ?? (_FileReportCommand = new Command(OnFileReportCommandCommand)); }
        }

        private ICommand _SettingsCommand;
        public ICommand SettingsCommand
        {
            get { return _SettingsCommand ?? (_SettingsCommand = new Command(OnSettingsCommand)); }
        }

        #endregion

        #region -- Overrides --

        public override async Task InitializeAsync(IDictionary<string, object> query)
        {
            if (query.ContainsKey(nameof(Title)))
            {
                ItemId = Uri.UnescapeDataString(query["Id"].ToString()!);
                Title = Uri.UnescapeDataString(query[nameof(Title)].ToString()!);
                string str = Uri.UnescapeDataString(query[nameof(ECalendarCellType)].ToString()!);
                CellType = (ECalendarCellType)Enum.Parse(typeof(ECalendarCellType), str);
            }

            IsBusy = true;
            if (Device.RuntimePlatform == Device.Android)
            {
                Task task = SetTabBarIsVisible();
                Task update = UpdateCarouselTabsAsync();
                await Task.WhenAll(task, update);
            }
            else
            {
                await UpdateCarouselTabsAsync();
            }

            if (CarouselTabs != null && CarouselTabs.Count > 0)
            {

                if (string.IsNullOrEmpty(Title))
                {
                    Title = CarouselTabs[0]?.Address;
                }

                IsInitialize = !IsBusy;

                if (query.ContainsKey(ENotificationAction.OpenFacilityAddress.ToString()))
                {
                    CarouselTabs[0]?.OpenMapCommand.Execute(null);
                }
                else if (query.ContainsKey(ENotificationAction.CheckIn.ToString()))
                {
                    if (CheckInState == EButtonState.DefaultState)
                    {
                        await SetCheckInAsync();
                    }
                    else if (CheckInState == EButtonState.SecondState)
                    {
                        _settingsService.CurrentWorkLogAppointmentId = ItemId;
                        var mainPage = (AppShell)App.Current.MainPage;
                        mainPage.SetCurrentTab(1);
                        Preferences.Set("isNeedAppointmentTimer", true);
                    }
                }
                else if (query.ContainsKey(ENotificationAction.CheckOut.ToString()))
                {
                    if (CheckInState == EButtonState.SecondState)
                    {
                        await SetCheckOutAsync();
                    }
                }
            }

            IsBusy = false;
        }

        #endregion

        #region -- Private helpers --

        private async Task SetTabBarIsVisible()
        {
            await Task.Delay(300);
            var page = App.Current.MainPage.Navigation.NavigationStack.Last() as AppointmentDetailPage;
            if (page != null)
            {
                Shell.SetTabBarIsVisible(page, false);
            }
        }

        private void UpdateCheckState(Appointment appoint)
        {
            if (appoint.CheckInDate == null && appoint.CheckOutDate == null && appoint.StartDate.Date != DateTime.Now.Date)
            {
                CheckInState = EButtonState.CheckInDisableState;
            }
            else if (appoint.CheckInDate == null && appoint.CheckOutDate == null)
            {
                CheckInState = EButtonState.DefaultState;
            }
            else if (appoint.CheckInDate != null && appoint.CheckOutDate == null)
            {
                CheckInState = EButtonState.SecondState;
            }
            else if(appoint.CheckInDate != null && appoint.CheckOutDate != null)
            {
                CheckInState = EButtonState.DisableState;
            }
        }

        private static AppointmentViewModel SetInfoTabData(Appointment appoint)
        {
            AppointmentViewModel infoTabViewModel = new AppointmentViewModel() { TabId = "1" };
            infoTabViewModel.Id = appoint.Id;
            infoTabViewModel.Address = appoint.Address;
            infoTabViewModel.Customer = appoint.Customer;
            infoTabViewModel.StartDate = appoint.StartDate;
            infoTabViewModel.EndDate = appoint.EndDate;
            infoTabViewModel.Date = appoint.StartDate.ToString("dd.MM.yyyy");
            infoTabViewModel.StartTime = appoint.StartDate.ToString("HH:mm");
            infoTabViewModel.EndTime = appoint.EndDate.ToString("HH:mm");;
            infoTabViewModel.MobilePhoneNumber = appoint.MobilePhoneNumber;
            infoTabViewModel.Email = appoint.Email;
            return infoTabViewModel;
        }

        private IList<AttachmentViewModel> GetOtherAttachments(IList<Attachment> attachments)
        {
            var otherAttachments = attachments.Where(x => !x.MimeType.Contains("image"));
            IList<AttachmentViewModel> otherAttachmentsVm = new List<AttachmentViewModel>();

            foreach (var item in otherAttachments)
            {
                otherAttachmentsVm.Add(new AttachmentViewModel()
                {
                    Id = item.Id,
                    Name = item.Name,
                    SelectCommand = new Command(OnTapOtherTypeOfAttachmentCommand)
                });
            }
            return otherAttachmentsVm;
        }

        private async Task SetCheckOutAsync()
        {
            if (_isSales)
            {
                IsBusy = true;
                var res = await _appointmentService.SalesAppointmentCheckOutAsync(CarouselTabs.First().Id);
                IsBusy = false;

                if (res.IsSuccess)
                {
                    CheckInState = EButtonState.DisableState;
                }
                else
                {
                    await DialogService.ShowAlertAsync(res.Message, "Sorry", Translation.Translate("Ok"));
                }
            }
            else
            {
                IsBusy = true;
                var res = await _appointmentService.ServiceAppointmentCheckOutAsync(CarouselTabs.First().Id);
                IsBusy = false;

                if (res.IsSuccess)
                {
                    CheckInState = EButtonState.DisableState;
                }
                else
                {
                    await DialogService.ShowAlertAsync(res.Message, "Sorry", Translation.Translate("Ok"));
                }
            }
        }

        private async Task SetCheckInAsync()
        {
            if (_isSales)
            {
                IsBusy = true;
                var res = await _appointmentService.SalesAppointmentCheckInAsync(CarouselTabs.First().Id);
                IsBusy = false;

                if (res.IsSuccess)
                {
                    CheckInState = EButtonState.SecondState;
                }
                else
                {
                    await DialogService.ShowAlertAsync(res.Message, "Sorry", Translation.Translate("Ok"));
                }
            }
            else
            {
                IsBusy = true;
                var res = await _appointmentService.ServiceAppointmentCheckInAsync(CarouselTabs.First().Id);
                IsBusy = false;

                if (res.IsSuccess)
                {
                    CheckInState = EButtonState.SecondState;
                }
                else
                {
                    await DialogService.ShowAlertAsync(res.Message, "Sorry", Translation.Translate("Ok"));
                }
            }
        }

        #endregion
    }
}