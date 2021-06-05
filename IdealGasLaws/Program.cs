using System;
using System.IO;

namespace IdealGasLaws
{
    class Program
    {
        static void Main(string[] args)
        {
            /*Name: Jonathan Falanga
            E-Mail: jfalanga@cnm.edu
            File Name: (unknown)
            Date Started: May 30th '21 */
            DisplayHeader();
            
            string[] gases=new string[100];
            double[] weights=new double[100];
            int counting = new int();
            GetMolecularWeights(ref gases, ref weights, out counting);

        }

        static void DisplayHeader()
        {
            Console.WriteLine("Name: Jonathan Falanga\nE-Mail: jfalanga@cnm.edu\nFile Name: (unknown)");
            Console.WriteLine("Date Started: May 30th '21");
        }

        static void GetMolecularWeights(ref string[] gasNames, ref double[] molecularWeights, out int count)
        {
            string fileName = "C:\\Users\\BookG\\Downloads\\MolecularWeightsGasesAndVapors.csv";
            string[] lines = File.ReadAllLines(fileName);

            int nameOrWeight = 1;
            count = 0;
            foreach(string line in lines)
            {
                

                string[] words = line.Split(",");
                if (count == 0) ///because this appears to be the title of this document!
                {
                    count = 1;
                    
                    switch(words[0])
                    {
                        case "Molecular Weight":
                            nameOrWeight = 2;
                            break;
                        case "Gas or Vapor":
                            break;
                        default:
                            throw new Exception("I don't understand the file: "+ fileName+"\nDoesn't contain a good head of the speadsheet!");
                            //break;

                    }
                    continue;
                    
                }      

                foreach (string word in words)
                {
                    
                    switch(nameOrWeight)
                    {
                        case 1:
                            count++; //just in case we're dealing w/ a different document,
                                     // wherein there might be gases/mole. weights on the same line!
                            nameOrWeight = 2;
                            gasNames[count - 1] = word;
                            break;
                        case 2:
                            try
                            {
                                molecularWeights[count-1] = Double.Parse(word);
                            } catch (FormatException)
                            {
                                throw new Exception ("I don't understand the file: " + fileName + "\nHas a non-Double value in the Molecular Weights Column!");
                            }
                            break;
                    }
                }
                
            }
            
        }
    }
}
