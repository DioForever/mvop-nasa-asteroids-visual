import requests
import matplotlib.pyplot as plt
import numpy as np
import matplotlib
matplotlib.use('Agg')  # Use a non-interactive backend

# Constants
API_KEY = "DEMO_KEY"  # Replace with your actual API key for production use
BASE_URL = "https://api.nasa.gov/neo/rest/v1/feed"
START_DATE = "2015-09-07"
END_DATE = "2015-09-08"

# Approximate distances from the Sun in AU for planets and their sizes in km
PLANETS = {
    "Sun": {"distance": 0, "diameter_km": 1392700, "color": 'yellow'},
    "Mercury": {"distance": 0.39, "diameter_km": 48800, "color": 'grey'},
    "Venus": {"distance": 0.72, "diameter_km": 121040, "color": 'orange'},
    "Earth": {"distance": 1.0, "diameter_km": 127420, "color": 'blue'},
    "Mars": {"distance": 1.52, "diameter_km": 67790, "color": 'red'},
    "Jupiter": {"distance": 5.2, "diameter_km": 139820, "color": 'brown'},
    "Saturn": {"distance": 9.58, "diameter_km": 1164600, "color": 'goldenrod'},
    "Uranus": {"distance": 19.22, "diameter_km": 507240, "color": 'lightblue'},
    "Neptune": {"distance": 30.05, "diameter_km": 492440, "color": 'purple'}
}

# Scaling factor for diameters to markersize (tuned for visibility)
SCALE_FACTOR = 0.00003  # Scales diameters to reasonable marker sizes in the plot

# Fetch asteroid data from NASA's NEO API
def fetch_asteroids(start_date, end_date):
    url = f"{BASE_URL}?start_date={start_date}&end_date={end_date}&api_key={API_KEY}"
    response = requests.get(url)
    data = response.json()
    return data

# Extract and prepare asteroid data
def prepare_asteroid_data(neo_data):
    asteroids = []
    for date in neo_data["near_earth_objects"]:
        for asteroid in neo_data["near_earth_objects"][date]:
            approach_data = asteroid["close_approach_data"][0]  # Use first close approach data
            if approach_data["orbiting_body"] == "Earth":  # Filter to Earth-approaching asteroids
                dist_au = float(approach_data["miss_distance"]["astronomical"])
                relative_velocity_kms = float(approach_data["relative_velocity"]["kilometers_per_second"])
                # Assign a small marker size for asteroids based on relative distance for visualization
                asteroid_diameter_km = float(asteroid["estimated_diameter"]["kilometers"]["estimated_diameter_max"])
                marker_size = max(2, asteroid_diameter_km * SCALE_FACTOR * 10)  # Ensure a minimum visibility size
                asteroids.append({
                    "name": asteroid["name"],
                    "distance_au": dist_au,
                    "velocity_kms": relative_velocity_kms,
                    "marker_size": marker_size
                })
    return asteroids

# Plot asteroids and planets in 2D space, with planets only if they fall within view
def plot_asteroids_and_planets(asteroids):
    # Determine the max distance from Earth to set plot limits
    max_distance_au = max(asteroid["distance_au"] for asteroid in asteroids) if asteroids else 0
    plot_limit = max(2.0, max_distance_au * 1.5)  # Extra 50% for padding

    plt.figure(figsize=(10, 10))
    plt.title("Top-Down View of Solar System with Asteroids Relative to Earth")
    plt.xlabel("Distance AU")
    plt.ylabel("Distance AU")
    
    # Plot Sun at the center
    plt.plot(0, 0, 'o', markersize=PLANETS["Sun"]["diameter_km"] * SCALE_FACTOR, color=PLANETS["Sun"]["color"], label="Sun")

    # Plot planets if within the current view, scaling size based on diameter
    for planet, data in PLANETS.items():
        distance = data["distance"]
        if distance <= plot_limit:
            angle = np.random.uniform(0, 2 * np.pi)  # Randomize angle for visualization
            x, y = distance * np.cos(angle), distance * np.sin(angle)
            plt.plot(x, y, 'o', markersize=data["diameter_km"] * SCALE_FACTOR, color=data["color"], label=planet)
            # Draw orbit path for planets
            orbit_theta = np.linspace(0, 2 * np.pi, 100)
            orbit_x = distance * np.cos(orbit_theta)
            orbit_y = distance * np.sin(orbit_theta)
            plt.plot(orbit_x, orbit_y, 'gray', linestyle='--', linewidth=0.5)

    # Plot each asteroid relative to Earth's position
    for asteroid in asteroids:
        # Asteroids will be placed in a random orbit around the Earth
        angle = np.random.uniform(0, 2 * np.pi)
        x = asteroid["distance_au"] * np.cos(angle)  # Asteroid distance from Earth
        y = asteroid["distance_au"] * np.sin(angle)
        plt.plot(x, y, 'ro', markersize=asteroid["marker_size"], label=asteroid["name"])

        # Check for overlapping asteroids and add glow if necessary
        overlapping = False
        for other in asteroids:
            if other != asteroid and abs(asteroid["distance_au"] - other["distance_au"]) < 0.01:
                overlapping = True
                break
        if overlapping:
            plt.plot(x, y, 'yo', markersize=asteroid["marker_size"] * 1.5, alpha=0.3)  # Add yellow glow around overlapping asteroids

    plt.legend(loc="upper right")
    plt.grid(True)
    plt.xlim(-plot_limit, plot_limit)
    plt.ylim(-plot_limit, plot_limit)
    plt.savefig("asteroids_and_planets_plot.png")

    plt.show()

# Main function to run the entire process
def main():
    # Fetch asteroid data
    neo_data = fetch_asteroids(START_DATE, END_DATE)
    
    # Prepare and filter data
    asteroids = prepare_asteroid_data(neo_data)
    
    # Plot asteroids and planets
    plot_asteroids_and_planets(asteroids)

# Run the main function
main()
