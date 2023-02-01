// See https://aka.ms/new-console-template for more information
using System.Text.RegularExpressions;
using LacunaGenetics.Controller;

namespace LacunaGenetics
{
    class Program
    {
        public static async Task Main()
         {
            Console.WriteLine("Enter your username, email and password to register:");
            User newUser = new User()
            {
                username =  Console.ReadLine(),
                email = Console.ReadLine(),
                password = Console.ReadLine()

            };

            await UserController.Create(newUser);

            UserController.Login(newUser);
        } 
    }
}
