using System.Text.RegularExpressions;

namespace LacunaGenetics
{
     public class LoginDTO
     {
        
        public string username { get; set; }
        public string password { get; set; }

        public bool validateData() 
        {
            Regex usernameRegex = new Regex(@"(^[A-Za-z0-9]{4,32}$)");
            if (!usernameRegex.IsMatch(this.username)) return false;

            Regex passwordRegex = new Regex(@"(^.{8,}$)");
            if (!passwordRegex.IsMatch(this.password)) return false;
            
            return true;
        }
    }
}