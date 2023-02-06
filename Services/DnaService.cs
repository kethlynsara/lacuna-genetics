using System.Text;

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

        public static string encodeString(JobDTO job) 
        {
            string binString = getBinaryString(job.strand);
            string hexString = getHexString(binString);
            byte[] bytes = Encoding.BigEndianUnicode.GetBytes(hexString);            
            string base64String = Convert.ToBase64String(bytes);
            return base64String;
        }

        private static string getBinaryString(string strand)
        {
            string binString = "";        
            for (int i = 0; i < strand.Length; i++)
            {
                if (strand[i].Equals('A')) binString += "00";
                else if (strand[i].Equals('C')) binString += "01";
                else if (strand[i].Equals('G')) binString += "10";
                else if (strand[i].Equals('T')) binString += "11";
            }
            Console.WriteLine("bin string: " + binString);            
            return binString;
        }

        private static string getHexString(string binString)
        {
            int rest = binString.Length % 4;
            binString = binString.PadLeft(rest, '0');
            string hexString = "";

            for(int i = 0; i <= binString.Length - 4; i +=4)
            {
                hexString += string.Format("{0:X}", Convert.ToByte(binString.Substring(i, 4), 2));
            }
            return hexString;
        }

        public static CheckGeneResponseDTO checkGene(JobDTO job) 
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