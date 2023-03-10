using System.Text;
using Newtonsoft.Json;

namespace LacunaGenetics.Controller
{
    public class UserController 
    {

        public static async Task Create(UserDTO obj)
         {

            if (obj.validateData())
            {
                using(var client =  new HttpClient())
                {
                    var endpoint = new Uri("https://gene.lacuna.cc/api/users/create");

                    var json = JsonConvert.SerializeObject(obj);
                    var payload = new StringContent(json, Encoding.UTF8, "application/json");
                    var response = client.PostAsync(endpoint, payload).Result.Content.ReadAsStringAsync().Result;

                    var jsonDeserialized = JsonConvert.DeserializeObject<ApiResponseDTO>(response);
                    Console.WriteLine($"{jsonDeserialized.code}! {jsonDeserialized.message}");
                }
            } else
            {
                 Console.WriteLine("Oops, something went wrong. Check your data!");
            }
        }
    
        public static string Login(LoginDTO user) 
        {
            if (user.validateData())
            {
                using(var client =  new HttpClient())
                {
                    var endpoint = new Uri("https://gene.lacuna.cc/api/users/login");

                    var json = JsonConvert.SerializeObject(user);
                    var payload = new StringContent(json, Encoding.UTF8, "application/json");
                    var response = client.PostAsync(endpoint, payload).Result.Content.ReadAsStringAsync().Result;

                    var jsonDeserialized = JsonConvert.DeserializeObject<LoginResponseDTO>(response);
                    Console.WriteLine($"{jsonDeserialized.code}! {jsonDeserialized.message}");

                    string token = jsonDeserialized.accessToken;
                    if (token != null) 
                    {
                        Console.WriteLine("Here's your access token: " + token);
                        return token;
                    } else 
                        Console.WriteLine("Something went wrong! Bad request!");
                        return "Something went wrong! Bad request!";
                }
            } else
            {
                 Console.WriteLine("Something went wrong. Check your data!");
                 return "Something went wrong. Check your data!";
            }
        }        
    }
    
}