using System;
using System.Globalization;
using MopsterTeams.Enums;
using MopsterTeams.Helpers;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Controls.Compatibility;
using Microsoft.Maui.Controls;
using Microsoft.Maui;

namespace MopsterTeams.Converters
{
    public class CalendarCellTypeToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var type = (ECalendarCellType)value;

            Color color = Colors.White;

            switch (type)
            {
                case ECalendarCellType.ServiceAppointment:
                case ECalendarCellType.SalesAppointment:
                    color = StyleManager.GetAppResource<Color>("Green");
                    break;
                case ECalendarCellType.BlockedTime:
                    color = StyleManager.GetAppResource<Color>("Grey_6");
                    break;
                case ECalendarCellType.Holiday:
                    color = StyleManager.GetAppResource<Color>("Blue");
                    break;
                case ECalendarCellType.Vacation:
                    color = StyleManager.GetAppResource<Color>("Orange");
                    break;
                case ECalendarCellType.SickLeave:
                    color = StyleManager.GetAppResource<Color>("Violet");
                    break;
                default:
                    throw new Exception("Unknown ECalendarCellType!");
            }

            return color;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
