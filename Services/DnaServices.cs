namespace LacunaGenetics.Service
{
    
    public class DnaService {

        public static string decodeToString(Job job)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(job.strand);
            Console.WriteLine(System.Text.Encoding.UTF8.GetString(base64EncodedBytes));
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }

        public static string encodeString(Job job) 
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(job.strand);
            Console.WriteLine(System.Text.Encoding.UTF8.GetString(plainTextBytes));
            return System.Convert.ToBase64String(plainTextBytes);
        }

        public static bool checkGene(Job job) 
        {


            return true;
        }
    }
}