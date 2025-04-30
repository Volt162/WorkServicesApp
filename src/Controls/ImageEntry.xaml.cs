using System;
using System.Collections.Generic;
using System.Windows.Input;
using MopsterTeams.Helpers;
using Microsoft.Maui.Controls.Compatibility;
using Microsoft.Maui.Controls;
using Microsoft.Maui;

namespace MopsterTeams.Controls
{
    public partial class ImageEntry : ContentView
    {

        public ImageEntry()
        {
            InitializeComponent();
        }

        #region -- Proreties --

        public bool IsNeedOpacityimage { get; set; }

        public static readonly BindableProperty KeyboardProperty =
            BindableProperty.Create(nameof(Keyboard), typeof(Keyboard), typeof(ImageEntry), default(Keyboard));


        public Keyboard Keyboard
        {
            get { return (Keyboard)GetValue(KeyboardProperty); }
            set { SetValue(KeyboardProperty, value); }
        }

        public static readonly BindableProperty TextProperty =
            BindableProperty.Create(nameof(Text), typeof(string), typeof(ImageEntry), default(string), BindingMode.TwoWay);

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public static readonly BindableProperty IsEntryEnabledProperty =
            BindableProperty.Create(nameof(IsEntryEnabled), typeof(bool), typeof(ImageEntry), true);

        public bool IsEntryEnabled
        {
            get { return (bool)GetValue(IsEntryEnabledProperty); }
            set { SetValue(IsEntryEnabledProperty, value); }
        }

        public static readonly BindableProperty PlaceholderProperty =
            BindableProperty.Create(nameof(Placeholder), typeof(string), typeof(ImageEntry), default(string));

        public string Placeholder
        {
            get { return (string)GetValue(PlaceholderProperty); }
            set { SetValue(PlaceholderProperty, value); }
        }

        public static readonly BindableProperty SourceProperty =
            BindableProperty.Create(nameof(Source), typeof(ImageSource), typeof(ImageEntry), default(ImageSource), propertyChanged: OnSourcePropertyChanged);

        public ImageSource Source
        {
            get { return (ImageSource)GetValue(SourceProperty); }
            set { SetValue(SourceProperty, value); }
        }

        public static readonly BindableProperty ImageButtonCommandProperty =
            BindableProperty.Create(nameof(ImageButtonCommand), typeof(ICommand), typeof(ImageEntry), default(ICommand));

        public ICommand ImageButtonCommand
        {
            get { return (ICommand)GetValue(ImageButtonCommandProperty); }
            set { SetValue(ImageButtonCommandProperty, value); }
        }

        public static readonly BindableProperty FrameBorderColorProperty =
            BindableProperty.Create(nameof(FrameBorderColor), typeof(Color), typeof(ImageEntry), StyleManager.GetAppResource<Color>("Grey_6"));

        public Color FrameBorderColor
        {
            get { return (Color)GetValue(FrameBorderColorProperty); }
            set { SetValue(FrameBorderColorProperty, value); }
        }

        public static readonly BindableProperty FrameBackgroundColorProperty =
            BindableProperty.Create(nameof(FrameBackgroundColor), typeof(Color), typeof(ImageEntry), StyleManager.GetAppResource<Color>("Grey_5"));

        public Color FrameBackgroundColor
        {
            get { return (Color)GetValue(FrameBackgroundColorProperty); }
            set { SetValue(FrameBackgroundColorProperty, value); }
        }

        public static readonly BindableProperty PlaceholderTextColorProperty =
            BindableProperty.Create(nameof(PlaceholderTextColor), typeof(Color), typeof(ImageEntry), StyleManager.GetAppResource<Color>("Grey_4"));

        public Color PlaceholderTextColor
        {
            get { return (Color)GetValue(PlaceholderTextColorProperty); }
            set { SetValue(PlaceholderTextColorProperty, value); }
        }

        #endregion

        #region -- Private helper --

        private static void OnSourcePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var _this = (ImageEntry)bindable;

            if (newValue != null)
            {
                _this.image.IsVisible = true;
                if (_this.IsNeedOpacityimage)
                {
                    _this.image.Opacity = 0.3;
                }
            }
        }

        #endregion
    }
}
