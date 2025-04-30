using System;
using System.Globalization;
using System.Net.Security;
using MopsterTeams.Helpers;
using Microsoft.Maui.Controls.Compatibility;
using Microsoft.Maui.Controls;
using Microsoft.Maui;

namespace MopsterTeams.Converters
{
    public class StrongPasswordConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var val = (int)value;
            var level = int.Parse(((BoxView)parameter).StyleId);
            return val >= level ? StyleManager.GetAppResource<Color>("Primary") : StyleManager.GetAppResource<Color>("Grey_3");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
