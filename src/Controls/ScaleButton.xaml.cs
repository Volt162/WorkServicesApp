using System;
using System.Collections.Generic;
using System.Windows.Input;
using MopsterTeams.Helpers;
using CommunityToolkit.Maui.Converters;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Controls.Compatibility;
using Microsoft.Maui.Controls;
using Microsoft.Maui;

namespace MopsterTeams.Controls
{
    public partial class ScaleButton : ContentView
    {
        public ScaleButton()
        {
            InitializeComponent();
        }

        #region -- Proreties --

        public static readonly BindableProperty TextProperty =
            BindableProperty.Create(nameof(Text), typeof(string), typeof(ScaleButton), default(string));

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public static readonly BindableProperty CommandProperty =
            BindableProperty.Create(nameof(Command), typeof(ICommand), typeof(ScaleButton), default(ICommand));

        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        public static readonly BindableProperty IsUpperCaseTitleProperty =
            BindableProperty.Create(nameof(IsUpperCaseTitle), typeof(bool), typeof(ScaleButton), default(bool), propertyChanged: OnIsUpperCaseTitlePropertyChanged);

        public bool IsUpperCaseTitle
        {
            get { return (bool)GetValue(IsUpperCaseTitleProperty); }
            set { SetValue(IsUpperCaseTitleProperty, value); }
        }

        public static readonly BindableProperty SourceProperty =
            BindableProperty.Create(nameof(Source), typeof(ImageSource), typeof(ScaleButton), default(ImageSource), propertyChanged: OnSourcePropertyChanged);

        public ImageSource Source
        {
            get { return (ImageSource)GetValue(SourceProperty); }
            set { SetValue(SourceProperty, value); }
        }

        public static readonly BindableProperty IsDisableStateProperty =
            BindableProperty.Create(nameof(IsDisableState), typeof(bool), typeof(ScaleButton), default(bool), propertyChanged: OnIsDisableStatePropertyChanged);

        public bool IsDisableState
        {
            get { return (bool)GetValue(IsDisableStateProperty); }
            set { SetValue(IsDisableStateProperty, value); }
        }

        public static readonly BindableProperty TextColorProperty =
            BindableProperty.Create(nameof(TextColor), typeof(Color), typeof(ScaleButton), Colors.White);

        public Color TextColor
        {
            get { return (Color)GetValue(TextColorProperty); }
            set { SetValue(TextColorProperty, value); }
        }

        public static new readonly BindableProperty BackgroundColorProperty =
            BindableProperty.Create(nameof(BackgroundColor), typeof(Color), typeof(ScaleButton), StyleManager.GetAppResource<Color>("Primary"));

        public new Color BackgroundColor
        {
            get { return (Color)GetValue(BackgroundColorProperty); }
            set { SetValue(BackgroundColorProperty, value); }
        }

        #endregion

        #region -- Private helper --

        private static void OnSourcePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var _this = (ScaleButton)bindable;

            if (newValue != null)
            {
                _this.icon.IsVisible = true;
                _this.label.HorizontalOptions = LayoutOptions.StartAndExpand;
            }
        }

        private static void OnIsUpperCaseTitlePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var _this = (ScaleButton)bindable;

            if (newValue != null)
            {
                if ((bool)newValue)
                {
                    _this.Text = _this.Text.ToUpper();
                };
            }
        }

        private static void OnIsDisableStatePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var _this = (ScaleButton)bindable;

            if (newValue != null)
            {
                if ((bool)newValue)
                {
                    _this.InputTransparent = true;
                }
                else
                {
                    _this.InputTransparent = false;
                };
            }
        }

        #endregion
    }
}
