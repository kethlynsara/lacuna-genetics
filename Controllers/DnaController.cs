using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using LacunaGenetics.Service;

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
                    Console.WriteLine($"{jsonDeserialized.job.type}! - {jsonDeserialized.job.strand}");

                    Console.WriteLine("job result: " + jsonDeserialized.job);

                    checkJob(jsonDeserialized.job);

                    return jsonDeserialized;
                }
            } 
        }

        private static void checkJob(Job job) 
        {
            if (job.type == "DecodeStrand") postDecodeToString(job);
            else if (job.type == "EncodeStrand") postEncodeString(job);
            else if (job.type == "CheckGene") postCheckGene(job);
        }

        public static  void postDecodeToString(Job job) {
            string decodedString = DnaService.decodeToString(job);
            Console.WriteLine("dedoded string: " + decodedString);
        } 

        public static void postEncodeString(Job job) {
            string encodeString  =  DnaService.encodeString(job);
            Console.WriteLine("endoded string: " + encodeString);
        }

        public static void postCheckGene(Job job) {
            bool isActive  =  DnaService.checkGene(job);
            Console.WriteLine("check gene");
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