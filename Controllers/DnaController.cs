using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;
using LacunaGenetics.Service;

namespace LacunaGenetics.Controller
{
    public class DnaController 
    {
        private const string url = "https://gene.lacuna.cc/api/dna/jobs";

        public static async Task<JobResponse> GetJob([Bind(Exclude = "email")] User user) 
        {
            var token = DnaController.getToken(user);
            if ("token" != "false")
            {                
                HttpClient httpClient = new HttpClient();

                using (var request = new HttpRequestMessage(HttpMethod.Get, url))
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
                    Console.WriteLine($"{jsonDeserialized.job.type}! - {jsonDeserialized.job.strandEncoded}");
                    Console.WriteLine($"{jsonDeserialized.job.type}! - {jsonDeserialized.job.geneEncoded}");

                    checkJob(jsonDeserialized.job, token);

                    return jsonDeserialized;
                }
            } 
        }        

        public static async void postDecodeToString(Job job, string token) 
        {
            Console.WriteLine("encoded string: " + job.strandEncoded);
            // string decodedString = DnaService.decodeToString(job);
            // Console.WriteLine("decoded string: " + decodedString);
            
            // using (HttpClient httpClient = new HttpClient())
            // {
            //     Console.WriteLine("entrou no using");
            //     httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                
            //     var body = new List<KeyValuePair<string, string>>
            //     {
            //         new KeyValuePair<string, string>("strand", decodedString),
            //     };

            //     var json = JsonConvert.SerializeObject(body);
            //     var payload = new StringContent(json, Encoding.UTF8, "application/json");
            //     var response = httpClient.PostAsync( $"{url}/{job.id}/decode", payload).Result.Content.ReadAsStringAsync().Result;
            //     Console.WriteLine("response: " + response);
            //     var jsonDeserialized = JsonConvert.DeserializeObject<ApiResponseDTO>(response);
            //     Console.WriteLine($" DECODED API RESPONSE:  {jsonDeserialized.code}! {jsonDeserialized.message}");
            //     Console.WriteLine("cade a resposta?????");
            //     Console.WriteLine("id: " + job.id);
            //     Console.WriteLine("jsondes: " +jsonDeserialized);
            // }
        } 

        public static void postEncodeString(Job job) {
            string encodeString  =  DnaService.encodeString(job);
            Console.WriteLine("encoded string: " + encodeString);
        }

        public static void postCheckGene(Job job) {
            CheckGeneResponseDTO isActivatedObj  =  DnaService.checkGene(job);
            Console.WriteLine("check gene is activated? " + isActivatedObj.isActivated);
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
    
        private static void checkJob(Job job, string token) 
        {
            if (job.type == "DecodeStrand") postDecodeToString(job, token);
            else if (job.type == "EncodeStrand") postEncodeString(job);
            else if (job.type == "CheckGene") postCheckGene(job);
        }
    
    }
}