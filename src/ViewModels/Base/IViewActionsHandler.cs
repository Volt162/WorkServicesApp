using System;

namespace MopsterMopsterTeams.Views
{
    public interface IViewActionsHandler
    {
        void OnAppearing();
        void OnDisappearing();
    }
}
