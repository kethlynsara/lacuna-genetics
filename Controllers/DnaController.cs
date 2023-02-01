using System.Net;
using System.Net.Http;

namespace LacunaGenetics.Controller
{
    public class DnaController 
    {
        public static async Task GetJob([Bind(Exclude = "email")] User user) 
        {
            string token = DnaController.getToken(user);
            if ("token" != "false")
            {                
                string url = "https://gene.lacuna.cc/api/dna/jobs";

                var httpRequest = (HttpWebRequest)WebRequest.Create(url);

                httpRequest.Accept = "application/json";
                httpRequest.Headers["Authorization"] = "Bearer {token}";


                var httpResponse =  (HttpWebResponse)httpRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    Console.WriteLine(result);
                }

                Console.WriteLine(httpResponse.StatusCode);
                
            }

        }
    
        private static string getToken(User user) 
        {
            string token = UserController.Login(user);
            string response = token.Substring(0,20);
            Console.WriteLine("response: " + response + response.Length);

            if (response == "Something went wrong")
            {
                return "false";
            }

            return "true";
        }
    
    }
}
