using System;
using System.Globalization;
using MopsterTeams.Enums;
using Microsoft.Maui.Controls.Compatibility;
using Microsoft.Maui.Controls;
using Microsoft.Maui;

namespace MopsterTeams.Converters
{
    public class ButtonStateToObjectConverter : IValueConverter
    {
        public object DefaultState { get; set; }
        public object SecondState { get; set; }
        public object DisableState { get; set; }
        public object CheckInDisableState { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            object res = null;

            switch ((EButtonState)value)
            {
                case EButtonState.DefaultState:
                    res = DefaultState;
                    break;
                case EButtonState.SecondState:
                    res = SecondState;
                    break;
                case EButtonState.CheckInDisableState:
                    res = CheckInDisableState;
                    break;
                default:
                    res = DisableState;
                    break;
            }

            return res;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
