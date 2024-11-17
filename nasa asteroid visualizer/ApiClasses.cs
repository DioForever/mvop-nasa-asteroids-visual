using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Threading.Tasks;

namespace nasa_asteroid_visualizer
{
    public class ApiClasses
    {
        public class AsteroidsData
        {
            [JsonPropertyName("links")]
            public Links Links { get; set; }

            [JsonPropertyName("element_count")]
            public int ElementCount { get; set; }

            [JsonPropertyName("near_earth_objects")]
            public Dictionary<string, List<Asteroid>> NearEarthObjects { get; set; }

        }

        public class Links
        {
            [JsonPropertyName("next")]
            public string Next { get; set; }

            [JsonPropertyName("prev")]
            public string Prev { get; set; }

            [JsonPropertyName("self")]
            public string Self { get; set; }
        }

        public class Asteroid
        {
            [JsonPropertyName("links")]
            public Links Links { get; set; }

            [JsonPropertyName("id")]
            public string Id { get; set; }

            [JsonPropertyName("neo_reference_id")]
            public string NeoReferenceId { get; set; }

            [JsonPropertyName("name")]
            public string Name { get; set; }

            [JsonPropertyName("nasa_jpl_url")]
            public string NasaJplUrl { get; set; }

            [JsonPropertyName("absolute_magnitude_h")]
            public double AbsoluteMagnitudeH { get; set; }

            [JsonPropertyName("estimated_diameter")]
            public EstimatedDiameter EstimatedDiameter { get; set; }

            [JsonPropertyName("is_potentially_hazardous_asteroid")]
            public bool IsPotentiallyHazardousAsteroid { get; set; }

            [JsonPropertyName("close_approach_data")]
            public List<CloseApproachData> CloseApproachData { get; set; }

            [JsonPropertyName("is_sentry_object")]
            public bool IsSentryObject { get; set; }
        }

        public class EstimatedDiameter
        {
            [JsonPropertyName("kilometers")]
            public DiameterRange Kilometers { get; set; }

            [JsonPropertyName("meters")]
            public DiameterRange Meters { get; set; }

            [JsonPropertyName("miles")]
            public DiameterRange Miles { get; set; }

            [JsonPropertyName("feet")]
            public DiameterRange Feet { get; set; }
        }

        public class DiameterRange
        {
            [JsonPropertyName("estimated_diameter_min")]
            public double EstimatedDiameterMin { get; set; }

            [JsonPropertyName("estimated_diameter_max")]
            public double EstimatedDiameterMax { get; set; }
        }

        public class CloseApproachData
        {
            [JsonPropertyName("close_approach_date")]
            public string CloseApproachDate { get; set; }

            [JsonPropertyName("close_approach_date_full")]
            public string CloseApproachDateFull { get; set; }

            [JsonPropertyName("epoch_date_close_approach")]
            public long EpochDateCloseApproach { get; set; }

            [JsonPropertyName("relative_velocity")]
            public RelativeVelocity RelativeVelocity { get; set; }

            [JsonPropertyName("miss_distance")]
            public MissDistance MissDistance { get; set; }

            [JsonPropertyName("orbiting_body")]
            public string OrbitingBody { get; set; }
        }

        public class RelativeVelocity
        {
            [JsonPropertyName("kilometers_per_second")]
            public string KilometersPerSecond { get; set; }

            [JsonPropertyName("kilometers_per_hour")]
            public string KilometersPerHour { get; set; }

            [JsonPropertyName("miles_per_hour")]
            public string MilesPerHour { get; set; }
        }

        public class MissDistance
        {
            [JsonPropertyName("astronomical")]
            public string Astronomical { get; set; }

            [JsonPropertyName("lunar")]
            public string Lunar { get; set; }

            [JsonPropertyName("kilometers")]
            public string Kilometers { get; set; }

            [JsonPropertyName("miles")]
            public string Miles { get; set; }
        }

        public class AsteroidAdditioanlData
        {
            [JsonPropertyName("orbital_data")]
            public OrbitalData OrbitalData { get; set; }
        }

        public class OrbitalData
        {
            [JsonPropertyName("orbit_id")]
            public string OrbitId { get; set; }

            [JsonPropertyName("orbit_determination_date")]
            public string OrbitDeterminationDate { get; set; }

            [JsonPropertyName("first_observation_date")]
            public string FirstObservationDate { get; set; }

            [JsonPropertyName("last_observation_date")]
            public string LastObservationDate { get; set; }

            [JsonPropertyName("data_arc_in_days")]
            public int DataArcInDays { get; set; }

            [JsonPropertyName("observations_used")]
            public int ObservationsUsed { get; set; }

            [JsonPropertyName("orbit_uncertainty")]
            public string OrbitUncertainty { get; set; }

            [JsonPropertyName("minimum_orbit_intersection")]
            public string MinimumOrbitIntersection { get; set; }

            [JsonPropertyName("jupiter_tisserand_invariant")]
            public string JupiterTisserandInvariant { get; set; }

            [JsonPropertyName("epoch_osculation")]
            public string EpochOsculation { get; set; }

            [JsonPropertyName("eccentricity")]
            public string Eccentricity { get; set; }

            [JsonPropertyName("semi_major_axis")]
            public string SemiMajorAxis { get; set; }

            [JsonPropertyName("inclination")]
            public string Inclination { get; set; }

            [JsonPropertyName("ascending_node_longitude")]
            public string AscendingNodeLongitude { get; set; }

            [JsonPropertyName("orbital_period")]
            public string OrbitalPeriod { get; set; }

            [JsonPropertyName("perihelion_distance")]
            public string PerihelionDistance { get; set; }

            [JsonPropertyName("perihelion_argument")]
            public string PerihelionArgument { get; set; }

            [JsonPropertyName("aphelion_distance")]
            public string AphelionDistance { get; set; }

            [JsonPropertyName("perihelion_time")]
            public string PerihelionTime { get; set; }

            [JsonPropertyName("mean_anomaly")]
            public string MeanAnomaly { get; set; }

            [JsonPropertyName("mean_motion")]
            public string MeanMotion { get; set; }

            [JsonPropertyName("equinox")]
            public string Equinox { get; set; }

            [JsonPropertyName("orbit_class")]
            public OrbitClass OrbitClass { get; set; }
        }

        public class OrbitClass
        {
            [JsonPropertyName("orbit_class_type")]
            public string OrbitClassType { get; set; }

            [JsonPropertyName("orbit_class_description")]
            public string OrbitClassDescription { get; set; }

            [JsonPropertyName("orbit_class_range")]
            public string OrbitClassRange { get; set; }
        }
    }


}
