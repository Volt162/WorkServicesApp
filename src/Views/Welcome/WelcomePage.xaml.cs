using System;
using System.Collections.Generic;
using System.Diagnostics;
using MopsterTeams.Helpers;
using MopsterTeams.Views.Base;
using Microsoft.Maui.Controls.PlatformConfiguration.iOSSpecific;
using Microsoft.Maui.Controls.Compatibility;
using Microsoft.Maui.Controls;
using Microsoft.Maui;
using Microsoft.Maui.Storage;

namespace MopsterTeams.Views
{
    public partial class WelcomePage : ContentPageBase
    {
        public WelcomePage()
        {
            InitializeComponent();

            if (Device.RuntimePlatform == Device.iOS)
            {
                this.Content = mainGrid;
            }

            mainGrid.SizeChanged += MainGridSizeChanged;
        }

        private void MainGridSizeChanged(object sender, EventArgs e)
        {
            var safeAreaInset = On<Microsoft.Maui.Controls.PlatformConfiguration.iOS>().SafeAreaInsets();
            mainGrid.SizeChanged -= MainGridSizeChanged;
        }
    }
}
