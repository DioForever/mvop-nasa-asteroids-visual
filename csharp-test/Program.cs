using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System;
using System.Drawing;
using System.Drawing.Imaging;  // For ImageFormat
using System.Threading.Tasks;


class AsteroidVisualization
{
    // API Base URL
    private const string NasaApiUrl = "https://api.nasa.gov/neo/rest/v1/feed";
    private const string ApiKey = "dhnOj4fBIIAejzFf82vnwiMSeL88HJUolrZMXBr9";

    static async Task Main(string[] args)
    {
        string date = args.Length > 0 ? args[0] : "2015-09-07"; // Default date: 2015-09-07
        await GetAsteroidsAndVisualize(date);
    }

    public static async Task GetAsteroidsAndVisualize(string date)
    {
        // Step 1: Get list of asteroids for the specified date
        // var asteroids = await GetAsteroids(date);
        // if (asteroids.Count == 0)
        // {
        //     Console.WriteLine("No asteroids found for the given date.");
        //     return;
        // }

        // Step 1.5: Make a plot of the asteroids in the 2D plane
        // Step 1.6: Visualize asteroids using C# Graphics
        var asteroidData = new List<AsteroidData>();
        using (var bitmap = new Bitmap(8000, 8000))
        {
            using (var graphics = Graphics.FromImage(bitmap))
            {
            graphics.Clear(Color.White);

            // Draw the solar system
            SolarSystem.DrawSun(graphics, bitmap.Width, bitmap.Height);
            SolarSystem.DrawPlanets(graphics, DateTime.Parse(date), bitmap.Width, bitmap.Height);

            // foreach (var asteroid in asteroids)
            // {
            //     var data = await GetAsteroidDetails(asteroid.Id);
            //     var relativeLocation = AsteroidData.GetRelativeLocation(data);
            //     double diameter = data.EstimatedDiameter?.Kilometers.Diameter ?? 0;

            //     // Draw the asteroid as a circle, relative to the center of the image
            //     float x = 4000 + (float)relativeLocation.x * 100;
            //     float y = 4000 + (float)relativeLocation.y * 100;
            //     float radius = (float)diameter * 100;
            //     Color color = relativeLocation.isDangerous ? Color.Red : Color.Blue;
            //     System.Console.WriteLine($"Drawing asteroid {data.Name} at x={x}, y={y}, radius={radius}, color={color}");

            //     // Get location of the orbiting body (e.g., Earth)
            //     var orbPosition = SolarSystem.GetPlanetPosition(relativeLocation.orbitingBody, SolarSystem.DatetoJulianDate(DateTime.Parse(date)));
            //     float orbX = Math.Clamp(4000 + (float)orbPosition.x, 0, 8000);
            //     float orbY = Math.Clamp(4000 + (float)orbPosition.y, 0, 8000);
            //     System.Console.WriteLine($"Orbiting body {relativeLocation.orbitingBody} at x={orbX}, y={orbY}");

            //     // Draw orbiting body
            //     graphics.FillEllipse(new SolidBrush(Color.Green), orbX - 5, orbY - 5, 1000, 1000);

            //     graphics.FillEllipse(new SolidBrush(color), x + orbX - radius, y + orbY - radius, 2 * radius, 2 * radius);

                
            //     // Console.WriteLine($"Relative location of asteroid {data.Name}: x={relativeLocation.x}, y={relativeLocation.y}");
            //     asteroidData.Add(data);
            // }

            graphics.FillEllipse(new SolidBrush(Color.Green), 400, 400, 10, 10);  // Earth at the center
            }

            // Save the bitmap to a file
            bitmap.Save("asteroids.png", System.Drawing.Imaging.ImageFormat.Png);
        }

    }

    public static async Task<List<Asteroid>> GetAsteroids(string date)
    {
        using var client = new HttpClient();
        string url = $"{NasaApiUrl}?start_date={date}&end_date={date}&api_key={ApiKey}";

        var response = await client.GetStringAsync(url);
        var feed = JsonConvert.DeserializeObject<NasaAsteroidFeed>(response);

        // Write the data to a JavaScript file
        using (var writer = new StreamWriter("asteroids.js"))
        {
            writer.WriteLine("const asteroids = [");
            foreach (var asteroid in feed.NearEarthObjects[date])
            {
                writer.WriteLine($"  {{ id: '{asteroid.Id}', name: '{asteroid.Name}', closeApproachDate: '{asteroid.CloseApproachData[0].CloseApproachDate}', missDistance: {asteroid.CloseApproachData[0].MissDistance.Astronomical} }}");
            }
            writer.WriteLine("];");
        }

        return feed.NearEarthObjects.ContainsKey(date) ? feed.NearEarthObjects[date] : new List<Asteroid>();
    }

    public static async Task<AsteroidData> GetAsteroidDetails(string asteroidId)
    {
        Console.WriteLine($"Fetching data for asteroid {asteroidId}...");
        // Console.ReadLine();
        using var client = new HttpClient();
        string url = $"https://api.nasa.gov/neo/rest/v1/neo/{asteroidId}?api_key={ApiKey}";

        var response = await client.GetStringAsync(url);
        var data = JsonConvert.DeserializeObject<AsteroidData>(response);
        Console.WriteLine(data.ToString());

        return data;
    }
}

// Models for deserializing JSON responses

public class NasaAsteroidFeed
{
    [JsonProperty("near_earth_objects")]
    public Dictionary<string, List<Asteroid>> NearEarthObjects { get; set; }
}

public class Asteroid
{
    [JsonProperty("id")]
    public string Id { get; set; }
    [JsonProperty("name")]
    public string Name { get; set; }
    [JsonProperty("close_approach_data")]
    public List<CloseApproachData> CloseApproachData { get; set; }
}

public class AsteroidVisualData
{

}

public class AsteroidData
{
    [JsonProperty("id")]
    public string Id { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("close_approach_data")]
    public List<CloseApproachData> CloseApproachData { get; set; }

    // Changed: OrbitalData now holds the orbital elements
    [JsonProperty("orbital_data")]
    public OrbitalData OrbitalData { get; set; }

    [JsonProperty("estimated_diameter")]
    public EstimatedDiameter EstimatedDiameter { get; set; }

    public override string ToString()
    {
        return $"Asteroid: {Name}, Close Approach Date: {CloseApproachData[0].CloseApproachDate}, Orbiting body {CloseApproachData[0].OrbitingBody} " +
               $"Semi-Major Axis: {OrbitalData?.SemiMajorAxis} AU, Eccentricity: {OrbitalData?.Eccentricity}, Inclination: {OrbitalData?.Inclination}°";
    }

public static AsteroidRelativeLocation GetRelativeLocation(AsteroidData data)
{
    // Assuming data.OrbitalData contains the orbital elements
    var orbitalData = data.OrbitalData;
    if (orbitalData == null)
    {
        throw new ArgumentNullException(nameof(orbitalData), "Orbital data cannot be null.");
    }

    // Semi-major axis (in AU)
    double semiMajorAxis = orbitalData.SemiMajorAxis;

    // Eccentricity
    double eccentricity = orbitalData.Eccentricity;

    // True anomaly (set to a specific value for this example; you may want to calculate or pass it)
    double trueAnomaly = Math.PI;  // For example, we're assuming the asteroid is at aphelion (farthest point).

    // Orbital radius at this position using the correct formula
    double orbitalRadius = (semiMajorAxis * (1 - Math.Pow(eccentricity, 2))) /
                           (1 + eccentricity * Math.Cos(trueAnomaly));

    // Calculate the x and y positions in the 2D plane
    double x = orbitalRadius * Math.Cos(trueAnomaly);
    double y = orbitalRadius * Math.Sin(trueAnomaly);

    // Create the AsteroidRelativeLocation object
    var relativeLocation = new AsteroidRelativeLocation
    {
        x = x,
        y = y,
        orbitingBody = "Earth", // Assuming the asteroid orbits Earth (change as necessary)
        diameter = data.EstimatedDiameter?.Kilometers.Diameter ?? 0, // Assuming EstimatedDiameter is provided
        isDangerous = data.EstimatedDiameter?.Kilometers.Diameter > 140 // Dangerous if diameter > 140 meters
    };

    return relativeLocation;
}


}

public class AsteroidRelativeLocation
{
    public double x { get; set; }
    public double y { get; set; }
    public string orbitingBody { get; set; }
    public double diameter { get; set; }
    public bool isDangerous { get; set; }
}

public class OrbitalData
{
    [JsonProperty("semi_major_axis")]
    public double SemiMajorAxis { get; set; }

    [JsonProperty("eccentricity")]
    public double Eccentricity { get; set; }

    [JsonProperty("inclination")]
    public double Inclination { get; set; }

    [JsonProperty("ascending_node_longitude")]
    public double AscendingNodeLongitude { get; set; }

    [JsonProperty("orbital_period")]
    public double OrbitalPeriod { get; set; }

    [JsonProperty("perihelion_distance")]
    public double PerihelionDistance { get; set; }

    [JsonProperty("perihelion_argument")]
    public double PerihelionArgument { get; set; }

    [JsonProperty("mean_anomaly")]
    public double MeanAnomaly { get; set; }

    [JsonProperty("epoch_osculation")]
    public double EpochOsculation { get; set; }

    public override string ToString()
    {
        return $"Semi-Major Axis: {SemiMajorAxis} AU, Eccentricity: {Eccentricity}, Inclination: {Inclination}°";
    }
}

public class EstimatedDiameter
{
    [JsonProperty("kilometers")]
    public DiameterData Kilometers { get; set; }
}

public class DiameterData
{
    [JsonProperty("estimated_diameter_min")]
    public double EstimatedDiameterMin { get; set; }
    [JsonProperty("estimated_diameter_max")]
    public double EstimatedDiameterMax { get; set; }
    public double Diameter => (EstimatedDiameterMin + EstimatedDiameterMax) / 2;
}


public class CloseApproachData
{
    [JsonProperty("close_approach_date")]
    public string CloseApproachDate { get; set; }
    [JsonProperty("miss_distance")]
    public MissDistance MissDistance { get; set; }
    [JsonProperty("orbiting_body")]
    public string OrbitingBody { get; set; }
}

public class MissDistance
{
    [JsonProperty("astronomical")]
    public string Astronomical { get; set; }
}



public class SolarSystem
{
    // Constants for each planet's orbital parameters (simplified for demonstration)
    private const double AU = 149597870.7; // Astronomical Unit in kilometers

    public static double DatetoJulianDate(DateTime date)
    {
        int year = date.Year;
        int month = date.Month;
        int day = date.Day;

        if (month <= 2)
        {
            year -= 1;
            month += 12;
        }

        int A = year / 100;
        int B = 2 - A + A / 4;

        double JD = Math.Floor(365.25 * (year + 4716)) + Math.Floor(30.6001 * (month + 1)) + day + B - 1524.5;
        return JD;
    }

    public static (double x, double y, double z) GetPlanetPosition(string planetName, double julianDate)
    {
        switch (planetName.ToLower())
        {
            case "mercury": return GetPlanetPosition(julianDate, 0.387, 0.205, 88, 7.0, 48.3, 29.1);
            case "venus": return GetPlanetPosition(julianDate, 0.723, 0.0067, 224.7, 3.4, 76.7, 54.9);
            case "earth": return GetPlanetPosition(julianDate, 1.0, 0.0167, 365.25, 0.0, -11.3, 114.2);
            case "mars": return GetPlanetPosition(julianDate, 1.524, 0.0934, 687, 1.85, 49.6, 286.5);
            case "jupiter": return GetPlanetPosition(julianDate, 5.203, 0.0489, 4331, 1.3, 100.5, 275.1);
            case "saturn": return GetPlanetPosition(julianDate, 9.537, 0.0565, 10747, 2.5, 113.7, 336.0);
            case "uranus": return GetPlanetPosition(julianDate, 19.191, 0.0463, 30589, 0.77, 74.0, 96.8);
            case "neptune": return GetPlanetPosition(julianDate, 30.068, 0.0097, 59800, 1.77, 131.8, 265.6);
            default: throw new ArgumentException("Invalid planet name");
        }
    }

    private static (double x, double y, double z) GetPlanetPosition(
        double julianDate, double semiMajorAxis, double eccentricity,
        double orbitalPeriod, double inclination, double longAscNode,
        double argPerihelion)
    {
        // Calculate mean anomaly
        double meanAnomaly = CalculateMeanAnomaly(julianDate, orbitalPeriod);

        // Estimate eccentric anomaly using iterative approximation (Newton-Raphson)
        double eccentricAnomaly = CalculateEccentricAnomaly(meanAnomaly, eccentricity);

        // Calculate the true anomaly
        double trueAnomaly = 2 * Math.Atan2(
            Math.Sqrt(1 + eccentricity) * Math.Sin(eccentricAnomaly / 2),
            Math.Sqrt(1 - eccentricity) * Math.Cos(eccentricAnomaly / 2)
        );

        // Calculate the distance from the Sun
        double distance = semiMajorAxis * (1 - eccentricity * Math.Cos(eccentricAnomaly)) * AU;

        // Convert to Cartesian coordinates in orbital plane
        double xOrbital = distance * Math.Cos(trueAnomaly);
        double yOrbital = distance * Math.Sin(trueAnomaly);

        // Adjust for inclination and orbital orientation
        double xEcliptic = (xOrbital * (Math.Cos(argPerihelion * Math.PI / 180) * Math.Cos(longAscNode * Math.PI / 180) -
                            Math.Sin(argPerihelion * Math.PI / 180) * Math.Sin(longAscNode * Math.PI / 180) * Math.Cos(inclination * Math.PI / 180))) -
                           (yOrbital * (Math.Sin(argPerihelion * Math.PI / 180) * Math.Cos(longAscNode * Math.PI / 180) +
                            Math.Cos(argPerihelion * Math.PI / 180) * Math.Sin(longAscNode * Math.PI / 180) * Math.Cos(inclination * Math.PI / 180)));

        double yEcliptic = (xOrbital * (Math.Cos(argPerihelion * Math.PI / 180) * Math.Sin(longAscNode * Math.PI / 180) +
                            Math.Sin(argPerihelion * Math.PI / 180) * Math.Cos(longAscNode * Math.PI / 180) * Math.Cos(inclination * Math.PI / 180))) +
                           (yOrbital * (Math.Cos(argPerihelion * Math.PI / 180) * Math.Cos(longAscNode * Math.PI / 180) -
                            Math.Sin(argPerihelion * Math.PI / 180) * Math.Sin(longAscNode * Math.PI / 180) * Math.Cos(inclination * Math.PI / 180)));

        double zEcliptic = xOrbital * (Math.Sin(argPerihelion * Math.PI / 180) * Math.Sin(inclination * Math.PI / 180)) +
                           yOrbital * (Math.Cos(argPerihelion * Math.PI / 180) * Math.Sin(inclination * Math.PI / 180));

        return (xEcliptic, yEcliptic, zEcliptic);
    }

    private static double CalculateMeanAnomaly(double julianDate, double orbitalPeriod)
    {
        double meanMotion = 2 * Math.PI / orbitalPeriod;
        double epochDate = 2451545.0; // J2000 Epoch
        return meanMotion * (julianDate - epochDate);
    }

    private static double CalculateEccentricAnomaly(double meanAnomaly, double eccentricity)
    {
        double E = meanAnomaly;
        for (int i = 0; i < 10; i++) // Iterate for convergence
        {
            E = E - (E - eccentricity * Math.Sin(E) - meanAnomaly) / (1 - eccentricity * Math.Cos(E));
        }
        return E;
    }

    public static float Scale = 10f;

    public static void DrawSun(Graphics graphics, int width, int height)
    {
        var size = GetPlanetSizeByPlanet("Sun") * Scale / 20;
        graphics.FillEllipse(new SolidBrush(Color.Yellow), width/2 - size/2, height/2 - size/2, size, size); // 20x20 px sun
    }

    public static void DrawPlanets(Graphics graphics, DateTime date, int bitmapWidth, int bitmapHeight)
    {
        // Centralize planets in the bitmap (400, 400 for 800x800)
        float centerX = bitmapWidth / 2f;
        float centerY = bitmapHeight / 2f;
        
        // Set a scaling factor so that Neptune's orbit (about 30 AU from the Sun) fits within the bitmap
        double maxDistanceAU = 30; // Neptune's max distance ~30 AU
        double scaleFactor = (bitmapWidth / 2f) / (maxDistanceAU * SolarSystem.AU); // Scale AU to pixels

        foreach (var planet in new string[] { "Mercury", "Venus", "Earth", "Mars", "Jupiter", "Saturn", "Uranus", "Neptune" })
        {
            var position = SolarSystem.GetPlanetPosition(planet, SolarSystem.DatetoJulianDate(date));
            var color = GetColorByPlanet(planet);
            var size = GetPlanetSizeByPlanet(planet) * Scale;
            System.Console.WriteLine($"Planet: {planet}, Position: {position}, Color: {color}, Size: {size}");

            // Scale position to fit bitmap and center
            float x = centerX + (float)(position.x * scaleFactor);
            float y = centerY + (float)(position.y * scaleFactor);

            // Draw planet
            graphics.FillEllipse(new SolidBrush(color), x - size/2, y - size/2, size, size); // 10x10 px planet size
        }
    }

    public static Color GetColorByPlanet(string planet)
    {
        switch (planet.ToLower())
        {
            case "mercury": return Color.Gray;
            case "venus": return Color.Orange;
            case "earth": return Color.Blue;
            case "mars": return Color.Red;
            case "jupiter": return Color.Orange;
            case "saturn": return Color.Yellow;
            case "uranus": return Color.LightBlue;
            case "neptune": return Color.Blue;
            default: return Color.White;
        }
    }

    public static float GetPlanetSizeByPlanet(string planet)
    {
        switch (planet.ToLower())
        {
            case "mercury": return 0.38f;
            case "venus": return 0.95f;
            case "earth": return 1.0f;
            case "mars": return 0.53f;
            case "jupiter": return 11.2f;
            case "saturn": return 9.45f;
            case "uranus": return 4.01f;
            case "neptune": return 3.88f;
            case "sun": return 109.0f;
            default: return 1.0f;
        }
    }
}