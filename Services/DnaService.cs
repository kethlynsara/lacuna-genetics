using System;
using System.Text;
using System.Collections;
using System.Net;
using System.Linq;

namespace LacunaGenetics.Service
{
    
    public class DnaService {

        public static DecodedStrandResponseDTO decodeToString(string strandEncoded)
        {
            byte[] bytesArray = Convert.FromBase64String(strandEncoded);
            string hexString = BitConverter.ToString(bytesArray);
            string hexStringTrim = hexString.Replace("-", "");
            String binString = hexStringTrim
                                .Aggregate(new StringBuilder(),(builder, c) => builder
                                .Append(Convert.ToString(Convert.ToInt32(c.ToString(), 16), 2)
                                .PadLeft(4, '0')))
                                .ToString();
            if (binString.Length % 2 != 0) 
            {
                string aux = "0";
                aux += binString;
                binString = aux;
            }
            string nucleobases = findNucleobases(binString);
            Console.WriteLine("DECODED: " + nucleobases);
            DecodedStrandResponseDTO obj = new DecodedStrandResponseDTO();
            obj.strand = nucleobases;
            return obj;
        }
        private static string findNucleobases(string binString)
        {
            string nucleobases = "";

            for (int i = 0; i < binString.Length; i+=2)
            {
                string aux = $"{binString[i]}{binString[i+1]}";
                if (aux.Equals("00")) nucleobases += "A";
                else if (aux.Equals("01")) nucleobases += "C";
                else if (aux.Equals("10")) nucleobases += "G";
                else if (aux.Equals("11")) nucleobases += "T";
            }

            return nucleobases;
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
            DecodedStrandResponseDTO strandEncoded = decodeToString(job.strandEncoded); 
            string templateStrand = findTemplateStrand(strandEncoded.strand);
            DecodedStrandResponseDTO geneEncoded = decodeToString(job.geneEncoded);
            string geneDecoded = geneEncoded.strand;
            int templateStrandLength = templateStrand.Length;
            int geneEncodedLength = geneDecoded.Length;
            int halfGeneLength = geneEncodedLength / 2 - 1;
            CheckGeneResponseDTO obj = new CheckGeneResponseDTO();
            obj.isActivated = false;

            int i = 0;
            while (halfGeneLength <= (geneEncodedLength - halfGeneLength) && !obj.isActivated)
            {
                string halfGene =  geneDecoded.Substring(i, halfGeneLength);			
			    int j = 0;
                int peaceStrandLength = halfGeneLength;
                while (peaceStrandLength <= (templateStrandLength - peaceStrandLength) && !obj.isActivated)
			    {
                    string halfStrand = templateStrand.Substring(j, peaceStrandLength);
                    if (halfStrand == halfGene) 
                    {
                        obj.isActivated = true;
                    }
                    j++;
                    peaceStrandLength++;
			    }			
			    i++;
                halfGeneLength++;		
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