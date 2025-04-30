using System;
using MopsterTeams.Enums;
using MopsterTeams.Models;
using Microsoft.Maui.Controls.Compatibility;
using Microsoft.Maui.Controls;
using Microsoft.Maui;

namespace MopsterTeams.Controls
{
    public class CalendarCellSelector : DataTemplateSelector
    {
        public DataTemplate CalendarCell { get; set; }
        public DataTemplate CalendarCellWithInfo { get; set; }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            ECalendarCellType type = ((CalendarCellModel)item).CalendarCellType;
            bool isInfoCell = type == ECalendarCellType.SalesAppointment || type == ECalendarCellType.ServiceAppointment;

            DataTemplate cell = isInfoCell && !string.IsNullOrWhiteSpace(((CalendarCellModel)item).Info) 
                ? CalendarCellWithInfo : CalendarCell;

            return cell;
        }
    }
}