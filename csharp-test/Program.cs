using System;
using System.Drawing;
using System.Drawing.Imaging;

class SolarSystem
{
    struct Planet
    {
        public string Name;
        public double SemiMajorAxis; // in AU
        public double Eccentricity;
        public double Inclination; // in degrees
        public double LongitudeOfAscendingNode; // in degrees
        public double ArgumentOfPeriapsis; // in degrees
        public double MeanLongitudeAtEpoch; // in degrees

        // Add these properties:
        public double OrbitalPeriod; // in days
        public double MeanAnomalyAtEpoch; // in degrees
    }

    static Planet[] planets = new Planet[]
    {
        new Planet { Name = "Mercury", SemiMajorAxis = 0.39, Eccentricity = 0.205, Inclination = 7.0,
                    LongitudeOfAscendingNode = 48.33, ArgumentOfPeriapsis = 77.45, MeanLongitudeAtEpoch = 252.25,
                    OrbitalPeriod = 87.97, MeanAnomalyAtEpoch = 174.796 }, // Example values for MeanAnomalyAtEpoch
        new Planet { Name = "Venus", SemiMajorAxis = 0.72, Eccentricity = 0.007, Inclination = 3.39,
                    LongitudeOfAscendingNode = 76.68, ArgumentOfPeriapsis = 131.53, MeanLongitudeAtEpoch = 181.98,
                    OrbitalPeriod = 224.70, MeanAnomalyAtEpoch = 50.115 },
        new Planet { Name = "Earth", SemiMajorAxis = 1.00, Eccentricity = 0.017, Inclination = 0.0,
                    LongitudeOfAscendingNode = 0.0, ArgumentOfPeriapsis = 102.94, MeanLongitudeAtEpoch = 100.46,
                    OrbitalPeriod = 365.25, MeanAnomalyAtEpoch = 358.617 },
        new Planet { Name = "Mars", SemiMajorAxis = 1.52, Eccentricity = 0.094, Inclination = 1.85,
                    LongitudeOfAscendingNode = 49.58, ArgumentOfPeriapsis = 336.04, MeanLongitudeAtEpoch = 355.45,
                    OrbitalPeriod = 686.98, MeanAnomalyAtEpoch = 19.373 },
        new Planet { Name = "Jupiter", SemiMajorAxis = 5.20, Eccentricity = 0.049, Inclination = 1.3,
                    LongitudeOfAscendingNode = 100.46, ArgumentOfPeriapsis = 14.73, MeanLongitudeAtEpoch = 34.40,
                    OrbitalPeriod = 4332.59, MeanAnomalyAtEpoch = 20.020 },
        new Planet { Name = "Saturn", SemiMajorAxis = 9.58, Eccentricity = 0.056, Inclination = 2.49,
                    LongitudeOfAscendingNode = 113.72, ArgumentOfPeriapsis = 92.43, MeanLongitudeAtEpoch = 49.94,
                    OrbitalPeriod = 10759.22, MeanAnomalyAtEpoch = 317.020 },
        new Planet { Name = "Uranus", SemiMajorAxis = 19.22, Eccentricity = 0.046, Inclination = 0.77,
                    LongitudeOfAscendingNode = 74.00, ArgumentOfPeriapsis = 170.96, MeanLongitudeAtEpoch = 313.23,
                    OrbitalPeriod = 30688.5, MeanAnomalyAtEpoch = 142.238 },
        new Planet { Name = "Neptune", SemiMajorAxis = 30.05, Eccentricity = 0.009, Inclination = 1.77,
                    LongitudeOfAscendingNode = 131.79, ArgumentOfPeriapsis = 44.97, MeanLongitudeAtEpoch = 304.88,
                    OrbitalPeriod = 60182.0, MeanAnomalyAtEpoch = 256.228 }
    };


    static Dictionary<string, double> PlanetSizes = new Dictionary<string, double>
    {
        { "Mercury", 0.0035 },
        { "Venus", 0.0087 },
        { "Earth", 0.0092 },
        { "Mars", 0.0049 },
        { "Jupiter", 0.1005 },
        { "Saturn", 0.0837 },
        { "Uranus", 0.0365 },
        { "Neptune", 0.0354 },
        { "Sun", 1}
    };

    static void Main(string[] args)
    {
        // Define scale factor
        double scale = 10.0; // 1.0 = normal scale, >1 increases resolution and sizes
        int baseWidth = 1920, baseHeight = 1080;
        int width = (int)(baseWidth * scale);
        int height = (int)(baseHeight * scale);

        DateTime date = DateTime.Now; // Replace with argument or user input for custom date

        Bitmap bmp = new Bitmap(width, height);
        using (Graphics g = Graphics.FromImage(bmp))
        {
            // Set background
            g.Clear(Color.Black);

            // Draw orbits and planets
            DrawSolarSystem(g, width, height, date, scale);

            // Save the image
            bmp.Save("solar_system_scaled.png", ImageFormat.Png);
            Console.WriteLine($"Solar system image saved as solar_system_scaled.png at scale {scale}");
        }

        // Example: Fetch planet position by name
        string planetName = "Mars";
        var position = GetPlanetPositionByName(planetName, date, width / 2, height / 2, 50 * scale, scale);
        Console.WriteLine($"{planetName} position on {date:yyyy-MM-dd}: X={position.X}, Y={position.Y}");
    }

    static void DrawSolarSystem(Graphics g, int width, int height, DateTime date, double scale)
    {
        int centerX = width / 2;
        int centerY = height / 2;
        double scaledAU = 50 * scale; // Scale for AU to pixels

        // Draw orbits and planets
        foreach (var planet in planets)
        {
            var position = CalculatePlanetPosition(planet, date, centerX, centerY, scaledAU, scale);

            // Convert the orbital position (in AU) to screen coordinates
            int planetX = (int)position.X;
            int planetY = (int)position.Y;

            int orbitRadius = (int)(planet.SemiMajorAxis * scaledAU);

            int planetSize = (int)(200 * scale * PlanetSizes[planet.Name]);


            double semiMajorAxisPixels = planet.SemiMajorAxis * scaledAU;
            double semiMinorAxisPixels = semiMajorAxisPixels * Math.Sqrt(1 - planet.Eccentricity * planet.Eccentricity);

            // Calculate the focus offset for the ellipse (this is the distance from the center to the focus)
            double focusOffset = semiMajorAxisPixels * planet.Eccentricity;
            var color = GetColorByPlanetName(planet.Name);

            var Pen = new Pen(color, 2);
            var PenWhite = new Pen(Color.White, 2);
            g.DrawEllipse(PenWhite,
                (float)(centerX - semiMajorAxisPixels + focusOffset),  // X position of top-left corner
                (float)(centerY - semiMinorAxisPixels),               // Y position of top-left corner
                (float)(2 * semiMajorAxisPixels),                     // Width of the ellipse
                (float)(2 * semiMinorAxisPixels));                    // Height of the ellipse

            // Draw the planet as a filled circle
            var Brush = new SolidBrush(color);

            g.FillEllipse(Brush, planetX - planetSize / 2, planetY - planetSize / 2, planetSize, planetSize);
            g.DrawString(planet.Name, new Font("Arial", 12), Brushes.White, planetX - planetSize / 2 + 10, planetY - planetSize / 2 + 10);
        }

        // Draw the Sun (scaled size)
        int sunSize = (int)(20 * scale);
        g.FillEllipse(Brushes.Yellow, centerX - sunSize / 2, centerY - sunSize / 2, sunSize, sunSize);
    }

    public static Color GetColorByPlanetName(string planet)
    {
        switch (planet)
        {
            case "Mercury":
                return Color.Orange;
            case "Venus":
                return Color.OrangeRed;
            case "Earth":
                return Color.Blue;
            case "Mars":
                return Color.Red;
            case "Jupiter":
                return Color.Brown;
            case "Saturn":
                return Color.LightGoldenrodYellow;
            case "Uranus":
                return Color.LightSkyBlue;
            case "Neptune":
                return Color.DarkBlue;
            default:
                return Color.Black;
        }
    }

    public static List<(double x, double y)> FindEllipseLineIntersections(
        double ellipseX, double ellipseY, double ellipseWidth, double ellipseHeight,
        double lineStartX, double lineStartY, double lineEndX, double lineEndY)
    {
        // Center and radii of the ellipse
        double h = ellipseX + ellipseWidth / 2.0;
        double k = ellipseY + ellipseHeight / 2.0;
        double a = ellipseWidth / 2.0;
        double b = ellipseHeight / 2.0;

        // Line parameters
        double dx = lineEndX - lineStartX;
        double dy = lineEndY - lineStartY;

        // Parametric line equation: x = x1 + t*dx, y = y1 + t*dy
        double A = (dx * dx) / (a * a) + (dy * dy) / (b * b);
        double B = 2 * (dx * (lineStartX - h) / (a * a) + dy * (lineStartY - k) / (b * b));
        double C = ((lineStartX - h) * (lineStartX - h)) / (a * a) +
                   ((lineStartY - k) * (lineStartY - k)) / (b * b) - 1;

        // Solve the quadratic equation A*t^2 + B*t + C = 0
        double discriminant = B * B - 4 * A * C;
        List<(double x, double y)> intersections = new List<(double x, double y)>();

        if (discriminant >= 0)
        {
            // Compute the two solutions for t
            double t1 = (-B - Math.Sqrt(discriminant)) / (2 * A);
            double t2 = (-B + Math.Sqrt(discriminant)) / (2 * A);

            // Check if solutions are within the segment [0, 1]
            if (t1 >= 0 && t1 <= 1)
            {
                double ix1 = lineStartX + t1 * dx;
                double iy1 = lineStartY + t1 * dy;
                intersections.Add((ix1, iy1));
            }

            if (t2 >= 0 && t2 <= 1)
            {
                double ix2 = lineStartX + t2 * dx;
                double iy2 = lineStartY + t2 * dy;
                intersections.Add((ix2, iy2));
            }
        }

        return intersections;
    }

    static (double x, double y) OrbitLineIntersection(double centerX, double centerY, double planetX, double planetY, double semiMajorAxis, double semiMinorAxis)
    {
        // Direction vector from the planet to the center of the ellipse
        double dx = planetX - centerX;
        double dy = planetY - centerY;

        double a = semiMajorAxis;
        double b = semiMinorAxis;

        double A = (dx * dx) / (a * a) + (dy * dy) / (b * b);
        double B = 2 * (dx * (centerX - planetX)) / (a * a) + 2 * (dy * (centerY - planetY)) / (b * b);
        double C = ((planetX * planetX) / (a * a)) + ((planetY * planetY) / (b * b)) - 1;

        // Calculate the discriminant to check for real intersections
        double discriminant = B * B - 4 * A * C;

        // If the discriminant is negative, there are no real intersections
        if (discriminant < 0)
        {
            return (double.NaN, double.NaN);
        }

        // Find the values of t1 and t2
        double t1 = (-B + Math.Sqrt(discriminant)) / (2 * A);

        double x1 = planetX + t1 * dx;
        double y1 = planetY + t1 * dy;

        return (x1, y1);
    }


    static (double X, double Y) CalculatePlanetPosition(Planet planet, DateTime date, double centerX, double centerY, double scaledAU, double scale)
    {
        double daysSinceJ2000 = (date - new DateTime(2000, 1, 1, 12, 0, 0)).TotalDays;

        // Mean anomaly (M = M0 + n * t)
        double meanMotion = 360.0 / planet.OrbitalPeriod; // degrees per day
        double meanAnomaly = planet.MeanAnomalyAtEpoch + meanMotion * daysSinceJ2000;
        meanAnomaly = meanAnomaly % 360; // Normalize to 0-360 degrees
        meanAnomaly = meanAnomaly * Math.PI / 180; // Convert to radians

        // Solve Kepler's Equation for Eccentric Anomaly
        double eccentricAnomaly = meanAnomaly;
        double delta;
        do
        {
            double newEccentricAnomaly = meanAnomaly + planet.Eccentricity * Math.Sin(eccentricAnomaly);
            delta = Math.Abs(newEccentricAnomaly - eccentricAnomaly);
            eccentricAnomaly = newEccentricAnomaly;
        } while (delta > 1e-6);

        // True anomaly (θ)
        double trueAnomaly = 2 * Math.Atan2(
            Math.Sqrt(1 + planet.Eccentricity) * Math.Sin(eccentricAnomaly / 2),
            Math.Sqrt(1 - planet.Eccentricity) * Math.Cos(eccentricAnomaly / 2)
        );

        // Distance from the Sun (r)
        double r = planet.SemiMajorAxis * (1 - planet.Eccentricity * Math.Cos(eccentricAnomaly));

        // Position in orbital plane
        double xOrbital = r * Math.Cos(trueAnomaly);
        double yOrbital = r * Math.Sin(trueAnomaly);

        // Transform to ecliptic coordinates
        double argPeriRad = planet.ArgumentOfPeriapsis * Math.PI / 180;
        double inclinationRad = planet.Inclination * Math.PI / 180;
        double longitudeOfAscendingNodeRad = planet.LongitudeOfAscendingNode * Math.PI / 180;

        // Apply argument of periapsis
        double x1 = xOrbital * Math.Cos(argPeriRad) - yOrbital * Math.Sin(argPeriRad);
        double y1 = xOrbital * Math.Sin(argPeriRad) + yOrbital * Math.Cos(argPeriRad);

        // Apply inclination
        double z1 = y1 * Math.Sin(inclinationRad);
        y1 = y1 * Math.Cos(inclinationRad);

        // Apply longitude of ascending node
        double xFinal = x1 * Math.Cos(longitudeOfAscendingNodeRad) - y1 * Math.Sin(longitudeOfAscendingNodeRad);
        double yFinal = x1 * Math.Sin(longitudeOfAscendingNodeRad) + y1 * Math.Cos(longitudeOfAscendingNodeRad);
        double zFinal = z1;

        // Convert the orbital position (in AU) to screen coordinates
        double planetX = centerX + (int)(xFinal * scaledAU);
        double planetY = centerY + (int)(yFinal * scaledAU);

        int orbitRadius = (int)(planet.SemiMajorAxis * scaledAU);

        int planetSize = (int)(200 * scale * PlanetSizes[planet.Name]);

        double semiMajorAxisPixels = planet.SemiMajorAxis * scaledAU;
        double semiMinorAxisPixels = semiMajorAxisPixels * Math.Sqrt(1 - planet.Eccentricity * planet.Eccentricity);

        // Calculate the focus offset for the ellipse (this is the distance from the center to the focus)
        double focusOffset = semiMajorAxisPixels * planet.Eccentricity;

        var intersections = FindEllipseLineIntersections(
            (centerX - semiMajorAxisPixels + focusOffset),
            (centerY - semiMinorAxisPixels),
            2 * semiMajorAxisPixels,
            2 * semiMinorAxisPixels,
            centerX,
            centerY,
            centerX + (int)(xFinal * scaledAU * 4),
            centerY + (int)(yFinal * scaledAU * 4));

        if (intersections.Count > 0)
        {
            xFinal = intersections[0].x;
            yFinal = intersections[0].y;
        }

        System.Console.WriteLine($"{planet.Name} position on {date:yyyy-MM-dd}: X={xFinal}, Y={yFinal}");

        return (xFinal, yFinal);
    }

    static (double X, double Y) GetPlanetPositionByName(string name, DateTime date, double centerX, double centerY, double scaledAU, double scale)
    {
        foreach (var planet in planets)
        {
            if (planet.Name.Equals(name, StringComparison.OrdinalIgnoreCase))
            {
                return CalculatePlanetPosition(planet, date, centerX, centerY, scaledAU, scale);
            }
        }
        throw new ArgumentException($"Planet '{name}' not found.");
    }
}
