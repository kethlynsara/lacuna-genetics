using System.Text;
using Newtonsoft.Json;

namespace LacunaGenetics.Controller
{
    public class UserController {
        //private readonly UserService userService;

        public static async Task Create(User obj) {
            if (obj.validateData())
            {
                using(var client =  new HttpClient())
                {
                    var endpoint = new Uri("https://gene.lacuna.cc/api/users/create");

                    var json = JsonConvert.SerializeObject(obj);
                    var payload = new StringContent(json, Encoding.UTF8, "application/json");
                    var response = client.PostAsync(endpoint, payload).Result.Content.ReadAsStringAsync().Result;

                    var jsonDeserialized = JsonConvert.DeserializeObject<Response>(response);
                    Console.WriteLine($"{jsonDeserialized.code}! {jsonDeserialized.message}");
                }
            } else
            {
                 Console.WriteLine("Oops, something went wrong. Check your data!");
            }
        }
    

    
    }
    
}