using System;
using Microsoft.Maui.Controls.Compatibility;
using Microsoft.Maui.Controls;
using Microsoft.Maui;
using Microsoft.Maui.Graphics;

namespace MopsterTeams.Extensions
{
    public static class VisualElementExtensions
	{
		public static ViewPositionInfoModel GetPositionOnScreenAndSize(this VisualElement view)
		{
			double screenCoordinateX = view.X;
			double screenCoordinateY = view.Y;
			Rect viewBounds = view.Bounds;
			Rect screenBounds = new Rect();

			if (view.Parent.GetType() != typeof(App))
			{
				VisualElement parent = (VisualElement)view.Parent;

				while (parent != null)
				{
					screenCoordinateX += parent.X;
					screenCoordinateY += parent.Y;
					screenBounds = parent.Bounds;

					if (parent.Parent?.GetType() == typeof(App))
						parent = null;
					else
                    {
                        try
                        {
							parent = (VisualElement)parent.Parent;
                        }
                        catch (Exception ex)
                        {
							parent = null;
                        }
						
                    }
						
				}
			}

			return new ViewPositionInfoModel()
			{
				X = screenCoordinateX,
				Y = screenCoordinateY,
				ViewBounds = viewBounds,
				ScreenBounds = screenBounds
			};
		}
	}

    public class ViewPositionInfoModel
	{
		public double X { get; set; }
		public double Y { get; set; }
		public Rect ViewBounds { get; set; }
		public Rect ScreenBounds { get; set; }
	}
}