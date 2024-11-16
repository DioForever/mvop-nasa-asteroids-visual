using Microsoft.Maui.Graphics;
using Microsoft.Maui.Graphics.Text;
using System;
using static SolarSystem;

namespace nasa_asteroid_visualizer
{
    public class SolarSystemDrawable : IDrawable
    {
        public static SolarSystemDrawable Instance { get; } = new SolarSystemDrawable();

        private float offsetX = 0; // Focus point X (center of the solar system)
        private float offsetY = 0; // Focus point Y (center of the solar system)
        private float zoom = 1;   // Zoom level

        private const float MinZoom = 0.05f;  // Minimum zoom level
        private const float MaxZoom = 100f;  // Maximum zoom level


        public void Draw(ICanvas canvas, RectF dirtyRect)
        {

            // Center of the canvas
            float centerX = dirtyRect.Center.X;
            float centerY = dirtyRect.Center.Y;

            DateTime date = DateTime.Now;

            SolarSystem solarSystem = new SolarSystem();
            solarSystem.Calculate(date, centerX, centerY);

            foreach (PlanetToDraw planet in solarSystem.PlanetsToDraw)
            {
                DrawOrbitAndPlanet(canvas, planet, centerX, centerY);
            }

            canvas.FillColor = Colors.Yellow;
            float sunRadius = 5 * zoom;
            canvas.FillCircle(centerX + offsetX * zoom, centerY + offsetY * zoom, sunRadius);

        }

        private void DrawOrbitAndPlanet(ICanvas canvas, PlanetToDraw planet, float centerX, float centerY)
        {
            canvas.StrokeColor = Colors.White;
            canvas.StrokeSize = 1;

            var Planet = planet.Planet;

            float planetX = ((float)planet.X - centerX + offsetX) * zoom + centerX;
            float planetY = ((float)planet.Y - centerY + offsetY) * zoom + centerY;

            float planetSize = 200 * (float)PlanetSizes[Planet.Name] * zoom;

            double semiMajorAxisPixels = Planet.SemiMajorAxis * scaledAU * zoom;
            double semiMinorAxisPixels = semiMajorAxisPixels * Math.Sqrt(1 - Planet.Eccentricity * Planet.Eccentricity);

            double focusOffset = semiMajorAxisPixels * Planet.Eccentricity;
            var color = GetColorByPlanetName(Planet.Name);

            float ellipseX = (float)(centerX + (offsetX * zoom) - semiMajorAxisPixels + focusOffset);
            float ellipseY = (float)(centerY + (offsetY * zoom) - semiMinorAxisPixels);

            canvas.DrawEllipse(
                ellipseX,  // X position of top-left corner
                ellipseY,               // Y position of top-left corner
                (float)(2 * semiMajorAxisPixels),                     // Width of the ellipse
                (float)(2 * semiMinorAxisPixels));                    // Height of the ellipse

            canvas.FillColor = GetColorByPlanetName(Planet.Name);
            canvas.FillCircle(planetX - planetSize / 2, planetY - planetSize / 2, planetSize);

            // Add text under the planet
            float textX = planetX; // Center the text horizontally with the planet
            float textY = planetY + planetSize / 2; // Position below the planet
            canvas.FontSize = Math.Clamp(8 * planetSize / zoom, 1, 20); // Adjust font size based on zoom
            canvas.FontColor = Colors.White; // Text color
            canvas.DrawString(
                Planet.Name,              // Text to display
                textX,                    // X-coordinate
                textY,                    // Y-coordinate
                HorizontalAlignment.Center);
        }

        public void MoveFocus(float dx, float dy)
        {
            offsetX += dx / zoom;
            offsetY += dy / zoom;
        }

        public void ChangeZoom(float deltaZoom)
        {
            zoom = Math.Clamp(zoom * deltaZoom, MinZoom, MaxZoom);
        }
    }
}
