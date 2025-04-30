using System;
using System.Collections.Generic;
using MopsterTeams.Enums;
using Microsoft.Maui.Controls.Compatibility;
using Microsoft.Maui.Controls;
using Microsoft.Maui;

namespace MopsterTeams.Controls
{
    public partial class StrongPasswordControl : ContentView
    {
        public StrongPasswordControl()
        {
            InitializeComponent();
        }

        public static readonly BindableProperty StrongPasswordProperty =
            BindableProperty.Create(nameof(StrongPassword), typeof(EStrongPassword), typeof(StrongPasswordControl), EStrongPassword.None);

        public EStrongPassword StrongPassword
        {
            get { return (EStrongPassword)GetValue(StrongPasswordProperty); }
            set { SetValue(StrongPasswordProperty, value); }
        }
    }
}
