using System;
using MopsterTeams.ViewModels.AppointmentDetails;
using Microsoft.Maui.Controls.Compatibility;
using Microsoft.Maui.Controls;
using Microsoft.Maui;

namespace MopsterTeams.Views.CarouselViews
{
    public class TabViewsDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate InformationTabView { get; set; }
        public DataTemplate ServicesTabView { get; set; }

        public TabViewsDataTemplateSelector()
        {
            InformationTabView = new DataTemplate(typeof(InformationTabView));
            ServicesTabView = new DataTemplate(typeof(ServicesTabView));
        }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            var position = ((AppointmentViewModel)item).TabId;

            if(position == "1")
                return InformationTabView;

            return ServicesTabView;
        }
    }
}
