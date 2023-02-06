using System.Text;
using Newtonsoft.Json;
using LacunaGenetics.Service;
using System.Net.Http.Headers;

namespace LacunaGenetics.Controller
{
    public class DnaController 
    {
        private const string url = "https://gene.lacuna.cc/api/dna/jobs";

        public static async Task<JobResponseDTO> GetJob(LoginDTO user) 
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

                    var stringResponse = await response.Content.ReadAsStringAsync();
                    var jsonDeserialized = JsonConvert.DeserializeObject<JobResponseDTO>(stringResponse);
                    Console.WriteLine($"job ---------------------------------");
                    Console.WriteLine($"{jsonDeserialized.code}! {jsonDeserialized.message}");

                    checkJob(jsonDeserialized.job, token);

                    return jsonDeserialized;
                }
            } 
        }        

        public static async void postDecodeToString(JobDTO job, string token) 
        {
            DecodedStrandResponseDTO decodedString = DnaService.decodeToString(job.strandEncoded);            
            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                
                var json = JsonConvert.SerializeObject(decodedString);
                var payload = new StringContent(json, Encoding.UTF8, "application/json");
                var response = httpClient.PostAsync( $"{url}/{job.id}/decode", payload).Result.Content.ReadAsStringAsync().Result;
                Console.WriteLine("response: " + response);
                var jsonDeserialized = JsonConvert.DeserializeObject<ApiResponseDTO>(response);
                Console.WriteLine($"DECODED API RESPONSE:  {jsonDeserialized.code}! {jsonDeserialized.message}");
            }
        } 

        public static void postEncodeString(JobDTO job, string token) {
            string encodedString  =  DnaService.encodeString(job);
            Console.WriteLine("encoded string: " + encodedString);

            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                
                var body = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("strandEncoded", encodedString),
                };

                var json = JsonConvert.SerializeObject(body);
                var payload = new StringContent(json, Encoding.UTF8, "application/json");
                var response = httpClient.PostAsync( $"{url}/{job.id}/encode", payload).Result.Content.ReadAsStringAsync().Result;
                Console.WriteLine("response: " + response);
                var jsonDeserialized = JsonConvert.DeserializeObject<ApiResponseDTO>(response);
                Console.WriteLine($" ENCODED API RESPONSE:  {jsonDeserialized.code}! {jsonDeserialized.message}");
            }
        }

        public static void postCheckGene(JobDTO job, string token) {
            CheckGeneResponseDTO isActivatedObj  =  DnaService.checkGene(job);
            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                
                var json = JsonConvert.SerializeObject(isActivatedObj);
                var payload = new StringContent(json, Encoding.UTF8, "application/json");
                var response = httpClient.PostAsync( $"{url}/{job.id}/gene", payload).Result.Content.ReadAsStringAsync().Result;
                Console.WriteLine("response: " + response);
                var jsonDeserialized = JsonConvert.DeserializeObject<ApiResponseDTO>(response);
                Console.WriteLine($"check gene API RESPONSE:  {jsonDeserialized.code}! {jsonDeserialized.message}");
            }
        }
        
        private static string getToken(LoginDTO user) 
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
    
        private static void checkJob(JobDTO job, string token) 
        {
            if (job.type == "DecodeStrand") postDecodeToString(job, token);
            else if (job.type == "EncodeStrand") postEncodeString(job, token);
            else if (job.type == "CheckGene") postCheckGene(job, token);
        }
    
    }
}