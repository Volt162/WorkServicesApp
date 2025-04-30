using System;
using System.Globalization;
using MopsterTeams.Helpers;
using Microsoft.Maui.Controls.Compatibility;
using Microsoft.Maui.Controls;
using Microsoft.Maui;

namespace MopsterTeams.Converters
{
    public class DateTimeToTimeRecordsStrFormatConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string str = string.Empty;

            var date = (DateTime)value;
            if (date.Year == 9999)
            {
                str = " -           ";
            }
            else
            {
               str = date.ToString(" - HH:mm");
            }
            
            return str;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
