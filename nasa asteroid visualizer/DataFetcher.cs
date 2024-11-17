using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using static nasa_asteroid_visualizer.ApiClasses;
using static nasa_asteroid_visualizer.APIKeyHandler;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace nasa_asteroid_visualizer
{
    public class DataFetcher
    {
        public static string GetAsteroidsAddress(DateTime date) => $"https://api.nasa.gov/neo/rest/v1/feed?start_date={date.ToString("yyyy-MM-dd")}&{date.ToString("yyyy-MM-dd")}=END_DATE&api_key={APIKeyHandler.Get()}";
        public static string GetAsteroidAddress(string id) => $"https://api.nasa.gov/neo/rest/v1/neo/{id}?api_key={APIKeyHandler.Get()}";

        public class Result
        {
            public bool success = true;
            public string errorTitle;
            public string errorText;
            public AsteroidsData asteroidsData;
        }

        public class ResultAsteroid
        {
            public bool success = true;
            public string errorTitle;
            public string errorText;
            public AsteroidAdditioanlData asteroid;
        }

        public static async Task<Result> GetAsteroids(DateTime date)
        {
            using HttpClient client = new HttpClient();

            // Fetch the response
            HttpResponseMessage response;
            var result = new Result();
            try
            {
                response = await client.GetAsync(GetAsteroidsAddress(date));
            }catch(Exception e)
            {
                if(e.Message == "Connection failure")
                {
                    result.success = false;
                    result.errorTitle = "Connection failure";
                    result.errorText = "Please check your internet connection";
                    return result;
                }

                result.success = false;
                result.errorTitle = "Error fetching asteroids";
                result.errorText = e.Message;

                return result;
            }


            if (!response.IsSuccessStatusCode)
            {
                result.errorTitle = "Error fetching data";
                result.errorText = response.ReasonPhrase;
                result.success = false;
                return result;
            }

            try
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var asteroidsData = JsonSerializer.Deserialize<AsteroidsData>(responseContent, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (asteroidsData == null)
                {
                    result.success = false;
                }
                result.asteroidsData = asteroidsData ?? new AsteroidsData();
            }
            catch (Exception ex)
            {
                result.success = false;
                result.errorText = ex.Message;
                result.errorTitle = $"Error parsing response: {ex.Message}";
                result.asteroidsData = new AsteroidsData();
            }

            return result;
        }

        public static async Task<ResultAsteroid> GetAdditionalAsteroidData(string id)
        {
            using HttpClient client = new HttpClient();

            // Fetch the response
            HttpResponseMessage response;
            var result = new ResultAsteroid();
            try
            {
                response = await client.GetAsync(GetAsteroidAddress(id));
                if(response.ReasonPhrase == "Too many requests.")
                {
                    await Task.Delay(1000);
                    response = await client.GetAsync(GetAsteroidAddress(id));
                }
            }
            catch (Exception e)
            {
                if (e.Message == "Connection failure")
                {
                    result.success = false;
                    result.errorTitle = "Connection failure";
                    result.errorText = "Please check your internet connection";
                    return result;
                }

                result.success = false;
                result.errorTitle = "Error fetching asteroids";
                result.errorText = e.Message;

                return result;
            }


            if (!response.IsSuccessStatusCode)
            {
                result.success = false;
                result.errorTitle = "Error fetching data";
                result.errorText = response.ReasonPhrase ?? "-*-*-*-*-";
                return result;
            }

            try
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var asteroidsData = JsonSerializer.Deserialize<AsteroidAdditioanlData>(responseContent, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (asteroidsData == null)
                {
                    result.success = false;
                }
                result.asteroid = asteroidsData ?? new AsteroidAdditioanlData();
            }
            catch (Exception ex)
            {
                result.success = false;
                result.errorText = $"Error parsing response: {ex.Message}";
                result.asteroid = new AsteroidAdditioanlData();
            }

            return result;
        }
    }
}
