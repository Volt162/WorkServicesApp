using System;

namespace MopsterTeams.Helpers
{
    public static class StyleManager
    {
        public static T GetAppResource<T>(string key)
        {
            return (T)App.Current.Resources[key];
        }
    }
}
