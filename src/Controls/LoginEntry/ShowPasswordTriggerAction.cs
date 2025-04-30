using System;
using System.ComponentModel;
using System.Threading.Tasks;
using Microsoft.Maui.Controls.Compatibility;
using Microsoft.Maui.Controls;
using Microsoft.Maui;

namespace MopsterTeams.Controls
{
    public class ShowPasswordTriggerAction : TriggerAction<ImageButton>, INotifyPropertyChanged
    {
        public string ShowIcon { get; set; }
        public string HideIcon { get; set; }

        bool _hidePassword = true;

        public ShowPasswordTriggerAction()
        {
            NewMethod();
        }

        private async void NewMethod()
        {
            if (Device.RuntimePlatform == Device.Android)
            {
                await Task.Delay(300);
                HidePassword = false;
                await Task.Delay(300);
                HidePassword = true;
            }
        }

        public bool HidePassword
        {
            set
            {
                if (_hidePassword != value)
                {
                    _hidePassword = value;

                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(HidePassword)));
                }
            }
            get => _hidePassword;
        }

        protected override void Invoke(ImageButton sender)
        {
            HidePassword = !HidePassword;
            sender.Source = HidePassword ? ShowIcon : HideIcon;
        }

        public event PropertyChangedEventHandler PropertyChanged;

    }
}
