// See https://aka.ms/new-console-template for more information
using System.Text.RegularExpressions;
using LacunaGenetics.Controller;

namespace LacunaGenetics
{
    class Program
    {
        public static async Task Main()
         {
            Console.WriteLine("Hello! Enter your username, email and password to register:");
            UserDTO newUser = new UserDTO()
            {
                username =  Console.ReadLine(),
                email = Console.ReadLine(),
                password = Console.ReadLine()

            };

            await UserController.Create(newUser);

            Console.WriteLine("Enter your username and password to login and request a job:");
            LoginDTO user = new LoginDTO()
            {
                username = Console.ReadLine(),
                password = Console.ReadLine()

            };
            UserController.Login(user);

           await DnaController.GetJob(user);
        
        } 

    }
}
