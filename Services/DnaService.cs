using System;
using System.Text;

namespace LacunaGenetics.Service
{
    
    public class DnaService {

        public static string decodeToString(Job job)
        {
            byte[] base64EncodedBytes = Convert.FromBase64String(job.strandEncoded);
            string result = System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
            Console.WriteLine("new new -> " + result);
            // Console.WriteLine("DECODED: " + System.Convert.FromBase64String(job.strandEncoded));
            // Console.WriteLine("DECODED: " + BitConverter.ToString(base64EncodedBytes));
            // //Encoding.UTF8.GetString(base64EncodedBytes)
            // //ASCIIEncoding.ASCII.GetString(base64EncodedBytes)
            // var hexaString = BitConverter.ToString(base64EncodedBytes);
            // string[] hexaSplit = hexaString.Split("-");
            // string decodedString = "";
            // foreach (string hex in hexaSplit)
            // {
            //     int value = Convert.ToInt32(hex, 16);
            //     string stringValue = Char.ConvertFromUtf32(value);
            //     //char charValue = (char)value;
            //     decodedString += stringValue;
            // }

            Console.WriteLine("new string: " + "decodedString");

            return result;
        }

        public static string encodeString(Job job) 
        {
            var plainTextBytes = Encoding.UTF8.GetBytes(job.strand);
            Console.WriteLine("ENCODED: " + System.Text.Encoding.UTF8.GetBytes(job.strand));
            Console.WriteLine("ENCODED: " + Convert.ToBase64String(plainTextBytes));
            return Convert.ToBase64String(plainTextBytes);
        }

        public static CheckGeneResponseDTO checkGene(Job job) 
        {
            string templateStrand = findTemplateStrand(job.strandEncoded);
            string geneEncoded = job.geneEncoded;
            int templateStrandLength = templateStrand.Length;
            int geneEncodedLength = geneEncoded.Length;
            int halfGeneLength = geneEncodedLength / 2;
            CheckGeneResponseDTO obj = new CheckGeneResponseDTO();
            obj.isActivated = false;

            int i = 0;
            while (halfGeneLength < (geneEncodedLength - 1) && !obj.isActivated)
            {
                string halfGene =  geneEncoded.Substring(i, (halfGeneLength));			
			    int j = 0;
                while (halfGeneLength < (templateStrandLength - j) && !obj.isActivated)
			    {
                    string halfStrand = templateStrand.Substring(j, halfGeneLength);
                    Console.WriteLine("halfStrand: " + halfStrand);
                    Console.WriteLine("");
                    if (halfStrand == halfGene) 
                    {
                        obj.isActivated = true;
                    }
                    j++;
			    }			
			    i++;		
            }            
            return obj;
        }

        private static string findTemplateStrand(string strand)
        {
            string sub = strand.Substring(0,3);
            int qtd = strand.Length;
            Console.WriteLine("strand template SUB: " + sub);
        
            if (sub != "CAT")
            {
                string newStrand = "";
                for (int i = 0; i < qtd; i++)
                {
                    if (strand[i] == 'C') newStrand += "G";	
                    else if (strand[i] == 'A') newStrand += "T";
                    else if (strand[i] == 'T') newStrand += "A";
                    else if (strand[i] == 'G') newStrand += "C";
                }
                Console.WriteLine("new strand: " + newStrand);
                return newStrand;
            } 

            return strand;
        }
    }
}