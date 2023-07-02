using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpressKuryer.Infrastructure.Services.Map
{
    public  class FindCoordinate
    {
        public static async Task<string> FindCor(string add)
        {
            string address = add;
          
            string apiKey = "AIzaSyAxW1MC2fjxukQ6DGg0Z3ahWMfDK7eKEig"; // Google Haritalar API anahtarınızı buraya ekleyin
            HttpClient client = new HttpClient();

            string url = $"https://maps.googleapis.com/maps/api/geocode/json?address={Uri.EscapeDataString(address)}&key={apiKey}&region=az";

            HttpResponseMessage response = await client.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                string json = await response.Content.ReadAsStringAsync();

                // JSON yanıtını analiz etmek için kullanılacak bir JSON ayrıştırma kütüphanesi kullanabilirsiniz.
                // Bu örnekte, Newtonsoft.Json kütüphanesini kullanıyoruz.
                dynamic result;
                try
                {
                    result = Newtonsoft.Json.JsonConvert.DeserializeObject(json);
                }
                catch (Exception ex)
                {
                    return ex + "Sehvdi ";
                }
                string status = result.status;

                if (status == "OK")
                {
                    string formattedAddress = result.results[0].formatted_address;
                    double latitude = result.results[0].geometry.location.lat;
                    double longitude = result.results[0].geometry.location.lng;
                    return latitude.ToString() + ", " + longitude.ToString();
                }
                else
                {
                    return status;
                }


            }
        
            return "No";
        }
    }
}
