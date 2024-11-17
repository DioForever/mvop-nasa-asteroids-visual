using Microsoft.Maui.Controls;
using System;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace nasa_asteroid_visualizer
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();


            SolarSystemDrawable.Instance.SetDate(DateTime.Now);
            DatePicker.Date = DateTime.Now;
            SolarSystemView.Drawable = SolarSystemDrawable.Instance;
            SolarSystemDrawable.Instance.Alert = Alert;
            SolarSystemDrawable.Instance.SolarSystemView = SolarSystemView;
            LoadApiKey();
        }

        public void LoadApiKey()
        {
            APIKeyHandler.LoadAPIKey();
            ApiKeyEntry.Text = APIKeyHandler.APIKEY;
        }

        public void Alert(string message, string title = "Alert")
        {
            DisplayAlert(title, message, "OK");
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

        private async void LoadAsteroids(object sender, EventArgs e)
        {
            DisplayBtn.IsEnabled = false;
            var originalText = DisplayBtn.Text;
            DisplayBtn.Text = "IN PROGRESS...";
            DateTime date = DatePicker.Date;
            SolarSystemDrawable.Instance.SetDate(date);
            var result = await DataFetcher.GetAsteroids(date);
            if (!result.success)
            {
                SolarSystemDrawable.Instance.Alert(result.errorText, result.errorTitle);
                return;
            }
            var astData = result.asteroidsData;

            SolarSystemDrawable.Instance.CalculateAsteroids(astData, date);

            DisplayBtn.Text = originalText;
            DisplayBtn.IsEnabled = true;

        }

        private void HazardousCheckChanged(object sender, CheckedChangedEventArgs e)
        {
            SolarSystemDrawable.Instance.SetHazardousOnly(e.Value);
        }

        private void OnNumericEntryTextChanged(object sender, TextChangedEventArgs e)
        {
            string newText = e.NewTextValue;
            float limit = 0;
            if (!string.IsNullOrEmpty(newText) && !float.TryParse(newText, out limit))
            {
                ((Entry)sender).Text = e.OldTextValue;
                return;
            }

            // Its valid
            SolarSystemDrawable.Instance.SetKm3Min(limit);
        }

        private void ApiKeySaveClicked(object sender, EventArgs e)
        {
            // Logic for the button click
            Button button = (Button)sender;

            APIKeyHandler.SaveAPIKey(ApiKeyEntry.Text);
        }

    }
}
