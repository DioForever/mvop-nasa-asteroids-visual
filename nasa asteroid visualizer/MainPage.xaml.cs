using Microsoft.Maui.Controls;
using System;

namespace nasa_asteroid_visualizer
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();


            SolarSystemView.Drawable = SolarSystemDrawable.Instance;
        }

        private void OnPanUpdated(object sender, PanUpdatedEventArgs e)
        {
            if (e.StatusType == GestureStatus.Running)
            {
                float deltaX = (float)-e.TotalX / 10; 
                float deltaY = (float)-e.TotalY / 10;

                SolarSystemDrawable.Instance.MoveFocus(deltaX, deltaY);
                SolarSystemView.Invalidate();  // This initiates the redraw
            }
        }

        private void OnPinchUpdated(object sender, PinchGestureUpdatedEventArgs e)
        {
            if (e.Status == GestureStatus.Running)
            {
                float newScale = (float)e.Scale;
                SolarSystemDrawable.Instance.ChangeZoom(newScale);
                SolarSystemView.Invalidate();  // This initiates the redraw
            }
        }
    }
}
