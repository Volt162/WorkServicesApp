using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Maui.Controls.Compatibility;
using Microsoft.Maui.Controls;
using Microsoft.Maui;

namespace MopsterTeams.Controls
{
    public partial class LoginEntry : ContentView
    {
        private CancellationTokenSource _cts;

        public LoginEntry()
        {
            InitializeComponent();
        }

        ~LoginEntry()
        {
            entry.TextChanged -= SearchBarTextChanged;
        }

        #region -- Proreties --

        public static readonly BindableProperty KeyboardProperty =
            BindableProperty.Create(nameof(Keyboard), typeof(Keyboard), typeof(LoginEntry), default(Keyboard));

        public Keyboard Keyboard
        {
            get { return (Keyboard)GetValue(KeyboardProperty); }
            set { SetValue(KeyboardProperty, value); }
        }

        public static readonly BindableProperty TextProperty =
            BindableProperty.Create(nameof(Text), typeof(string), typeof(LoginEntry), default(string), BindingMode.TwoWay);

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public static readonly BindableProperty PlaceholderProperty =
            BindableProperty.Create(nameof(Placeholder), typeof(string), typeof(LoginEntry), default(string));

        public string Placeholder
        {
            get { return (string)GetValue(PlaceholderProperty); }
            set { SetValue(PlaceholderProperty, value); }
        }

        public static readonly BindableProperty HasPasswordProperty =
            BindableProperty.Create(nameof(HasPassword), typeof(bool), typeof(LoginEntry), true, propertyChanged: OnHasPasswordPropertyChanged);

        public bool HasPassword
        {
            get { return (bool)GetValue(HasPasswordProperty); }
            set { SetValue(HasPasswordProperty, value); }
        }

        public static readonly BindableProperty UserStoppedTypingCommandProperty =
            BindableProperty.Create(nameof(Command), typeof(ICommand), typeof(ScaleButton), default(ICommand), propertyChanged: OnUserStoppedTypingCommandPropertyChanged);

        public ICommand UserStoppedTypingCommand
        {
            get { return (ICommand)GetValue(UserStoppedTypingCommandProperty); }
            set { SetValue(UserStoppedTypingCommandProperty, value); }
        }

        #endregion

        #region -- Private helper --

        private static void OnHasPasswordPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var _this = (LoginEntry)bindable;

            if (newValue != null)
            {
                if (!((bool)newValue))
                {
                    _this.entry.IsPassword = false;
                };
            }
        }

        private static void OnUserStoppedTypingCommandPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var _this = (LoginEntry)bindable;

            if (newValue != null)
            {
                _this.Subscribe();
            }
        }

        private void Subscribe()
        {
            entry.TextChanged += SearchBarTextChanged;
        }

        private async void SearchBarTextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                _cts?.Cancel();     
            }
            catch (ObjectDisposedException)    
            {
                // if previous search completed
            }

            using (_cts = new CancellationTokenSource())
            {
                try
                {
                    await Task.Delay(TimeSpan.FromMilliseconds(1000), _cts.Token);  
                    UserStoppedTypingCommand.Execute(entry.Text);
                }
                catch (TaskCanceledException)       
                {
                    // if search was canceled
                }
            }
        }

        #endregion
    }
}
