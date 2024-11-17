using System;
using System.Globalization;
using Microsoft.Maui.Graphics;
using nasa_asteroid_visualizer;
using static nasa_asteroid_visualizer.ApiClasses;
using static nasa_asteroid_visualizer.SolarSystemDrawable;

public class SolarSystem
{
    public static double scaledAU = 50;
    public struct Planet
    {
        public string Name;
        public double SemiMajorAxis; // in AU
        public double Eccentricity;
        public double Inclination; // in degrees
        public double LongitudeOfAscendingNode; // in degrees
        public double ArgumentOfPeriapsis; // in degrees6
        public double MeanLongitudeAtEpoch; // in degrees
        public double OrbitalPeriod; // in days
        public double MeanAnomalyAtEpoch; // in degrees
    }

    public struct PlanetToDraw
    {
        public Planet Planet;
        public double X;
        public double Y;
        public double PlanetSize;
    }

    public static Planet[] planets =
    {
        new Planet { Name = "Mercury", SemiMajorAxis = 0.39, Eccentricity = 0.205, Inclination = 7.0,
                    LongitudeOfAscendingNode = 48.33, ArgumentOfPeriapsis = 77.45, MeanLongitudeAtEpoch = 252.25,
                    OrbitalPeriod = 87.97, MeanAnomalyAtEpoch = 174.796 },
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

    public List<PlanetToDraw> PlanetsToDraw = new List<PlanetToDraw>(); 


    public static Dictionary<string, double> PlanetSizes = new Dictionary<string, double>
    {
        { "Mercury", 0.0035 },
        { "Venus", 0.0087 },
        { "Earth", 0.0092 },
        { "Mars", 0.0049 },
        { "Jupiter", 0.1005 },
        { "Saturn", 0.0837 },
        { "Uranus", 0.0365 },
        { "Neptune", 0.0354 }
    };

    public void Calculate(DateTime date, double centerX, double centerY)
    {
        // Define scale factor
        CalculateSolarSystem(date, centerX, centerY);
    }

    private void CalculateSolarSystem(DateTime date, double centerX, double centerY)
    {
        // Add orbits and planets
        foreach (var planet in planets)
        {
            var position = CalculatePlanetPosition(planet, date, centerX, centerY, scaledAU);

            PlanetsToDraw.Add(new PlanetToDraw { Planet = planet, X = position.X, Y = position.Y, PlanetSize = PlanetSizes[planet.Name] });
        }
    }

    public static Color GetColorByPlanetName(string planet)
    {
        switch (planet)
        {
            case "Mercury":
                return Color.FromArgb("44636D");
            case "Venus":
                return Color.FromArgb("7B2700");
            case "Earth":
                return Color.FromArgb("00B4E8");
            case "Mars":
                return Color.FromArgb("A42502");
            case "Jupiter":
                return Color.FromArgb("9A8374");
            case "Saturn":
                return Color.FromArgb("C29373");
            case "Uranus":
                return Color.FromArgb("94D1D5");
            case "Neptune":
                return Color.FromArgb("115488");
            default:
                return Color.FromArgb("E1E900"); // Sun is default
        }
    }

    public static List<(double x, double y)> FindEllipseLineIntersections(
        double ellipseX, double ellipseY, double ellipseWidth, double ellipseHeight,
        double lineStartX, double lineStartY, double lineEndX, double lineEndY)
    {
        // Center and radius of the ellipse
        double h = ellipseX + ellipseWidth / 2.0;
        double k = ellipseY + ellipseHeight / 2.0;
        double a = ellipseWidth / 2.0;
        double b = ellipseHeight / 2.0;

        // Line parameters
        double dx = lineEndX - lineStartX;
        double dy = lineEndY - lineStartY;

        double A = (dx * dx) / (a * a) + (dy * dy) / (b * b);
        double B = 2 * (dx * (lineStartX - h) / (a * a) + dy * (lineStartY - k) / (b * b));
        double C = ((lineStartX - h) * (lineStartX - h)) / (a * a) +
                   ((lineStartY - k) * (lineStartY - k)) / (b * b) - 1;

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


    public static (double X, double Y) CalculatePlanetPosition(Planet planet, DateTime date, double centerX, double centerY, double scaledAU)
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

        int planetSize = (int)(20 * PlanetSizes[planet.Name]);

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


        return (xFinal, yFinal);
    }

    public static async Task<(double x, double y, string orbitingBody)> CalculateAsteroidRelativePosition(
    Asteroid asteroid, DateTime targetDate, double scaledAU)
    {
        try
        {
            // Find the closest approach data for the specified date
            var approachData = asteroid.CloseApproachData
                .FirstOrDefault(data => DateTime.Parse(data.CloseApproachDate) == targetDate);

            if (approachData == null)
            {
                if(asteroid.CloseApproachData.Count == 0)
                {
                    SolarSystemDrawable.Instance.Alert("No close approach data available for the target date.", "API Error");
                    return (0, 0, "");
                }
                approachData = asteroid.CloseApproachData[0];
            }

            // Retrieve additional orbital data
            var result = await DataFetcher.GetAdditionalAsteroidData(asteroid.Id);
            if (!result.success)
            {
                SolarSystemDrawable.Instance.Alert(result.errorText, result.errorTitle);
                return (0,0,"");
                //throw new InvalidOperationException($"Failed to retrieve additional data for asteroid {asteroid.Name}: {result.errorText}");
            }
            var orbitalData = result.asteroid.OrbitalData;

            // BUGGGG

            // Days since J2000
            double daysSinceJ2000 = (targetDate - new DateTime(2000, 1, 1, 12, 0, 0)).TotalDays;

            // Mean anomaly (M = M0 + n * t)
            double meanMotion = 360.0 / double.Parse(orbitalData.OrbitalPeriod, CultureInfo.InvariantCulture); // degrees per day
            double meanAnomaly = double.Parse(orbitalData.MeanAnomaly, CultureInfo.InvariantCulture) + meanMotion * daysSinceJ2000;
            meanAnomaly = meanAnomaly % 360; // Normalize to 0-360 degrees
            meanAnomaly = meanAnomaly * Math.PI / 180; // Convert to radians

            // Solve Kepler's equation for Eccentric Anomaly using a first-order approximation
            double eccentricity = double.Parse(orbitalData.Eccentricity, CultureInfo.InvariantCulture);
            double eccentricAnomaly = meanAnomaly + eccentricity * Math.Sin(meanAnomaly);

            // True anomaly (θ)
            double trueAnomaly = 2 * Math.Atan2(
                Math.Sqrt(1 + eccentricity) * Math.Sin(eccentricAnomaly / 2),
                Math.Sqrt(1 - eccentricity) * Math.Cos(eccentricAnomaly / 2)
            );

            // Distance from the orbiting body (r)
            double semiMajorAxis = double.Parse(orbitalData.SemiMajorAxis, CultureInfo.InvariantCulture); // AU
            double r = semiMajorAxis * (1 - eccentricity * Math.Cos(eccentricAnomaly));

            // Position in orbital plane
            double xOrbital = r * Math.Cos(trueAnomaly);
            double yOrbital = r * Math.Sin(trueAnomaly);

            // Transform to ecliptic coordinates
            double argPeriRad = double.Parse(orbitalData.PerihelionArgument, CultureInfo.InvariantCulture) * Math.PI / 180;
            double inclinationRad = double.Parse(orbitalData.Inclination, CultureInfo.InvariantCulture) * Math.PI / 180;
            double longitudeOfAscendingNodeRad = double.Parse(orbitalData.AscendingNodeLongitude, CultureInfo.InvariantCulture) * Math.PI / 180;

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

            // Scale position for display
            double asteroidX = (xFinal);
            double asteroidY = (yFinal);

            return (asteroidX, asteroidY, approachData.OrbitingBody);

        }catch(Exception e)
        {
            throw new Exception("Failed to calculate asteroid position", e);
        }
    }
}
