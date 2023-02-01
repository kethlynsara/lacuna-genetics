using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;

namespace LacunaGenetics.Controller
{
    public class DnaController 
    {

        public static async Task<JobResponse> GetJob([Bind(Exclude = "email")] User user) 
        {
            var token = DnaController.getToken(user);
            if ("token" != "false")
            {                
                const string url = "https://gene.lacuna.cc/api";
                HttpClient httpClient = new HttpClient();

                using (var request = new HttpRequestMessage(HttpMethod.Get, $"{url}/dna/jobs"))
                {
                    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    var response = await httpClient.SendAsync(request);

                    response.EnsureSuccessStatusCode();
                    Console.WriteLine("--------------------");
                    Console.WriteLine(response.Content.ReadAsStringAsync());

                    var stringResponse = await response.Content.ReadAsStringAsync();
                    var jsonDeserialized = JsonConvert.DeserializeObject<JobResponse>(stringResponse);
                    Console.WriteLine($"job ---------------------------------");
                    Console.WriteLine($"{jsonDeserialized.code}! {jsonDeserialized.message}");
                    Console.WriteLine($"{jsonDeserialized.job}! {jsonDeserialized.job.type}");

                    return jsonDeserialized;
                }
            } 

        }
    
        private static string getToken(User user) 
        {
            var token = UserController.Login(user);
            string response = token.Substring(0,20);
            Console.WriteLine("response: " + response + response.Length);

            if (response == "Something went wrong")
            {
                return "false";
            }

            return token;
        }
    
    }
}