using Microsoft.Maui.Graphics;
using Microsoft.Maui.Graphics.Text;
using System;
using static SolarSystem;
using nasa_asteroid_visualizer;
using System.Security.Cryptography.X509Certificates;

namespace nasa_asteroid_visualizer
{
    public class SolarSystemDrawable : IDrawable
    {
        public static SolarSystemDrawable Instance { get; } = new SolarSystemDrawable();

        private float offsetX = 0; // Focus point X (center of the solar system)
        private float offsetY = 0; // Focus point Y (center of the solar system)
        private float zoom = 1;   // Zoom level

        private float _centerX;
        private float _centerY;

        private const float MinZoom = 0.05f;  // Minimum zoom level
        private const float MaxZoom = 1000000f;  // Maximum zoom level

        private const float AUtoKm = 149597871;
        private const float AsteroidSizeModifier = 1000000;

        public ICanvas Canvas;
        public GraphicsView SolarSystemView;
        
        public delegate void AlertDelegate(string message, string title = "Alert");
        public AlertDelegate Alert;

        // Filters
        public static float Km3Min = 0;
        public static bool HazardousOnly = false;
        public static DateTime Date { get; set; } = DateTime.Now;

        public Dictionary<string, (float x, float y)> planetsDrawnPositions = new Dictionary<string, (float, float)>();
        public List<AsteroidProperties> asteroidsToDraw = new List<AsteroidProperties>();
        public static int asteroidLimit = 5;

        public struct AsteroidProperties
        {
            public double x;
            public double y;
            public double size;
            public bool isHazardous;
            public string name;
            public string orbitingBody;
        }

        public void SetDate(DateTime date)
        {
            if (Date.Year == date.Year && Date.Month == date.Month && Date.Day == date.Day) return;
            Date = date;
        }

        public void SetKm3Min(float limit) => Km3Min = limit;
        public void SetHazardousOnly(bool filter) => HazardousOnly = filter;

        public async void Draw(ICanvas canvas, RectF dirtyRect)
        {
            Canvas = canvas;

            // Center of the canvas
            float centerX = dirtyRect.Center.X;
            float centerY = dirtyRect.Center.Y;

            _centerX = centerX;
            _centerY = centerY;

            SolarSystem solarSystem = new SolarSystem();
            solarSystem.Calculate(Date, centerX, centerY);

            foreach (PlanetToDraw planet in solarSystem.PlanetsToDraw)
            {
                DrawOrbitAndPlanet(canvas, planet, centerX, centerY);
                DrawAsteroids(canvas, centerX, centerY);
            }

            canvas.FillColor = Colors.Yellow;
            float sunRadius = 5 * zoom;
            canvas.FillCircle(centerX + offsetX * zoom, centerY + offsetY * zoom, sunRadius);
            SolarSystemView?.Invalidate();

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
                ellipseX,
                ellipseY,  
                (float)(2 * semiMajorAxisPixels),                 
                (float)(2 * semiMinorAxisPixels));          

            canvas.FillColor = GetColorByPlanetName(Planet.Name);
            canvas.FillCircle(planetX, planetY, planetSize);

            // Add text under the planet
            float textX = planetX; 
            float textY = planetY + planetSize / 2; 
            canvas.FontSize = Math.Clamp(8 * planetSize / zoom, 1, 20); 
            canvas.FontColor = Colors.White; 
            canvas.DrawString(
                Planet.Name,   
                textX,      
                textY,       
                HorizontalAlignment.Center);

            planetsDrawnPositions[Planet.Name] = ((float)planet.X, (float)planet.Y);
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

        public async void CalculateAsteroids(ApiClasses.AsteroidsData asteroidsData, DateTime date)
        {
            asteroidsToDraw = [];
            int limit = asteroidLimit;

            foreach (var asteroid in asteroidsData.NearEarthObjects)
            {
                foreach (var asteroidData in asteroid.Value)
                {
                    limit--;
                    if(limit < 0) return;

                    var asteroidProperties = await CalculateAsteroidRelativePosition(asteroidData, date, scaledAU);
                    var estimatedSize =
                        (asteroidData.EstimatedDiameter.Kilometers.EstimatedDiameterMax
                        + asteroidData.EstimatedDiameter.Kilometers.EstimatedDiameterMin) / 2;
                    var size = estimatedSize;
                    var name = asteroidData.Name;

                    if (asteroidProperties.orbitingBody == "") continue; // Some error happened and we want to skip it

                    // Find the closest approach data for the specified date
                    if (asteroidData.CloseApproachData.Count == 0)
                        throw new InvalidOperationException("No close approach data available for the target date.");
                    var approachData = asteroidData.CloseApproachData[0];

                    AsteroidProperties AsteroidProperties = new AsteroidProperties
                    {
                        x = asteroidProperties.x,
                        y = asteroidProperties.y,
                        size = size,
                        isHazardous = asteroidData.IsPotentiallyHazardousAsteroid,
                        orbitingBody = approachData.OrbitingBody,
                        name = name
                    };
                    asteroidsToDraw.Add(AsteroidProperties);
                }
            }
        }

        public async void DrawAsteroid(AsteroidProperties asteroid, float centerX, float centerY)
        {
            float x = (float)asteroid.x;
            float y = (float)asteroid.y;
            float size = (float)asteroid.size / AUtoKm * zoom * AsteroidSizeModifier;
            bool isHazardous = asteroid.isHazardous;
            string name = asteroid.name;

            float orbitingBodyX = planetsDrawnPositions[asteroid.orbitingBody].x;
            float orbitingBodyY = planetsDrawnPositions[asteroid.orbitingBody].y;

            // Shift the asteroid to the orbiting body
            x += orbitingBodyX;
            y += orbitingBodyY;

            float planetX = ((float)orbitingBodyX - centerX + offsetX) * zoom + centerX;
            float planetY = ((float)orbitingBodyY - centerY + offsetY) * zoom + centerY;

            float asteroidX = (x - centerX + offsetX) * zoom + centerX;
            float asteroidY = (y - centerY + offsetY) * zoom + centerY;

            // Make a line from planet to asteroid and extend by the planet's size
            float deltaX = asteroidX - planetX;
            float deltaY = asteroidY - planetY;
            float distance = MathF.Sqrt(deltaX * deltaX + deltaY * deltaY);
            float lineExtension = (float)PlanetSizes[asteroid.orbitingBody] * 200 * zoom;

            // Calculate the new asteroid position at the end of the extended line
            float scaleFactor = (distance + lineExtension) / distance;
            asteroidX = planetX + deltaX * scaleFactor;
            asteroidY = planetY + deltaY * scaleFactor;

            x = asteroidX;
            y = asteroidY;

            // Draw asteroid
            if (isHazardous) Instance.Canvas.FillColor = Colors.Red;
            else Instance.Canvas.FillColor = Colors.White;

            Instance.Canvas.FillCircle(x, y, size);

            // Make a tad smaller circle in the middle
            Instance.Canvas.FillColor = Colors.Gray;
            Instance.Canvas.FillCircle(x + size / 10, y + size / 10, size - 2*size/10);

            // Add text under the asteroid
            float textX = x;
            float textY = y + size / 2;
            Instance.Canvas.FontSize = Math.Clamp(8 * size / Instance.zoom, 8, 20); 
            Instance.Canvas.FontColor = Colors.White; 
            Instance.Canvas.DrawString(
                name,      
                textX,    
                textY,       
                HorizontalAlignment.Center);
        }

        public void DrawAsteroids(ICanvas canvas, float centerX, float centerY)
        {
            int limit = asteroidLimit;
            foreach (AsteroidProperties asteroid in Instance.asteroidsToDraw)
            {
                if (asteroid.size * AUtoKm < Km3Min) continue;
                if (HazardousOnly && !asteroid.isHazardous) continue;
                if (limit < 0) return;
                limit--;
                DrawAsteroid(asteroid, centerX, centerY);
            }
        }
    }
}
