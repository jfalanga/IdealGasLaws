using System;
using System.IO;

namespace IdealGasLaws
{
    
    class Program
    {
        static string fileName = "C:\\Users\\BookG\\Downloads\\MolecularWeightsGasesAndVapors.csv";
            //Where the file is on my computer
            //MIGHT WANT TO CHANGE THIS ONE WHEN YOU RUN IT!!!
        public static void Main(string[] args)
        {
            /*Name: Jonathan Falanga
             * 
            E-Mail: jfalanga@cnm.edu
            File Name: (MolecularWeightsGasesAndVapors.csv)
            Date Started: May 30th '21 */
            DisplayHeader();
            
            string[] gases=new string[100];
            double[] weights=new double[100];
            //100 entries, since I'm not entirely sure how many entries the .csv file contains
            //...& I'm thinking that's why I have a "counting" variable:
            int counting = new int();
            GetMolecularWeights(ref gases, ref weights, out counting);
            Console.WriteLine();
            DisplayGasNames(gases, counting);
            bool keepGoing = true;
            
            do
            {
                Console.WriteLine("Which of these gases do you want to talk about? (Caps/Lowercase Letters aren't necessary)");
                string thisGas = Console.ReadLine();
                bool isValid = new bool();
                double thisWeight= GetMolecularWeightFromName(thisGas,  gases, weights, counting,ref isValid);
                if (!isValid)
                {
                    Console.WriteLine("That's not an exact match to any of the gases in my database. Do you want to try again? (Y/N)");
                    bool yesNo = YesOrNo();
                    if (yesNo)
                    {
                        continue;
                    } else
                    {
                        return;
                    }

                }
                IdealGass myGas = new IdealGass();
                myGas.SetMolecularWeight(thisWeight);
                Console.WriteLine("What is the volume of the gas? (In cubic meters?)");
                string volumeString;
                double volume=0.0;
                bool tBool = false;
                while (!tBool)
                {
                    try
                    {
                        volumeString = Console.ReadLine();
                        volume = Double.Parse(volumeString);
                    }
                    catch (FormatException)
                    {
                        continue;
                    }
                    catch (OverflowException)
                    {
                        Console.WriteLine("That number was too big!");
                        continue;
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Something happened:");
                        Console.WriteLine(e.Message);
                        Console.WriteLine(e.GetType());
                        Console.WriteLine("Do you want to continue and try again?");
                        if (!YesOrNo())
                        {
                            break;
                        }
                        else
                        {
                            continue;
                        }
                    }
                }
                myGas.SetVolume(volume);                                                    Console.WriteLine("And, what is the mass of the gas, in this case (in grams)?");
                string massString;
                double mass;
                do
                {
                    massString = Console.ReadLine();
                } while (!Double.TryParse(massString, out mass));
                myGas.SetMass(mass);
                Console.WriteLine("Now, tell me what the temperature of this gas is?");
                string stringTemp;
                double temperature;
                do
                {
                    stringTemp = Console.ReadLine();
                } while (!Double.TryParse(stringTemp, out temperature));
                myGas.SetTemperature(temperature);
                double pres = myGas.GetPressure();
                DisplayPressure(pres);
                Console.WriteLine("\nDo you want to try that for a different gas? (Y/N)");
                keepGoing = YesOrNo();
            } while (keepGoing);

        }

        public static double CelsiusToKelvin(double temp)
        {
            return temp+ 273.15;
        }

        public static void DisplayHeader()
        {
            Console.WriteLine("Name: Jonathan Falanga\nE-Mail: jfalanga@cnm.edu\nFile Name: "+fileName);
            Console.WriteLine("Date Started: May 30th '21");
        }

        public static bool YesOrNo()       //Knew I'd end up doing this thing twice, so...
        {
            do
            {
                string yOrN = Console.ReadLine();
                yOrN = yOrN.ToUpper();
                switch (yOrN)
                {
                    case "Y":
                        return true;
                    case "N":
                        return false;
                    default:
                        Console.WriteLine("Please right Y or N!");
                        break;
                }

            } while (2 == 2);
            
        }

        public static void GetMolecularWeights(ref string[] gasNames, ref double[] molecularWeights, out int count)
        {
            
            string[] lines = File.ReadAllLines(fileName);
            if (lines.Length >101)
            {
                throw new Exception("I can only read up to 100 entries in the .csv value; and this file exceeds that number");
            }

            int nameOrWeight = 1;   //a simple counter to tell if this is the name of the gas
                                    //or the Mole. Weight: 1=Name, & 2=Weight. 
            int upto2 = 0;      //If there is more than 1 entry per "line", I have a counter to see if
                                //we have all the information for this sort of Molecular Weight thing.
                                //...Which is only 2 parts!
            count = 0;
            foreach(string line in lines)   //Each line of the file- considering a "line"
                                            //to be where there is text, ending w/ a carraige return
            {
                
                
                string[] words = line.Split(",");   //"words" are all the phrases- names
                                                    //or gases- in the file/array
                if (count == 0) ///because this appears to be the title "line" of this document!
                {
                    count = 1;  //since this is the 1st entry
                    
                    switch(words[0])
                    {
                        case "Molecular Weight":        //In case this is file is the reverse of the present one!
                            nameOrWeight = 2;
                            break;
                        case "Gas or Vapor":
                            break;
                        default:
                            //An overly-simple exception for this file! I'd have to make
                            //this more complicated if I wanted to make this serious!
                            throw new Exception("I don't understand the file: " + fileName + "\nDoesn't contain a good head line for the speadsheet!");

                    }
                    continue;
                    
                }      

                foreach (string word in words)
                {
                    
                    switch(nameOrWeight)
                    {
                        case 1:
                            upto2++;    
                            nameOrWeight = 2;
                            gasNames[count - 1] = word;
                            break;
                        case 2:
                            nameOrWeight = 1;
                            upto2++;
                            try
                            {
                                molecularWeights[count-1] = Double.Parse(word);
                            } catch (FormatException)
                            {
                                throw new Exception ("I don't understand the file: " + fileName + "\nHas a non-Double value in the Molecular Weights Column for entry: "+count);
                            }
                            
                            break;
                    }
                    if (upto2 == 2)
                    {
                        //just in case we're dealing w/ a different document,
                        // wherein there might be multiple gases/mole weights on the same line
                        // we'd need the count after we reach the limit of this kind of data
                        count++; 
                        upto2 = 0;
                    }
                }
                upto2 = upto2;
            }
            count--;    //since the 1st line is the header, we need
                        //to subtract one to give the actual number of entries.
        }
        public static void DisplayGasNames(string[] gasNames, int countGases)
        {
            int thirdOfCountGas = ((int)countGases / 3)+1;  //+1 in case we're dealing with a non-multiple of 3
            bool OutOfNames = false;
            for (int ix=0;ix< thirdOfCountGas; ix++)
            {

                if (ix == 33)   //that is, if we're dealing w/ exactly 100 entries.
                {
                    OutOfNames= Display3OrLess(gasNames[99]);   
                } else
                {
                    int xIx = ix * 3;
                    OutOfNames= Display3OrLess(gasNames[xIx], gasNames[xIx + 1], gasNames[xIx + 2]);
                }
                if (ix == thirdOfCountGas-1 && OutOfNames)
                {
                    Console.WriteLine();        //Just so there's sure to be 1, and ONLY empty line,
                                                //after the list of gases. (With a multiple of 3, there
                                                //will be an empty line anyway)
                                
                }
            }
        }

        public static bool Display3OrLess(string A="", string B=" ",string C="")
        {
            Console.WriteLine("{0,-20}{1,-20}{2}", A, B, C);
            if (A==null&&B== null && C== null)
            {
                return false;   //This'll be triggered if we're dealing with a multiple of 3
            }
            return true;
            IdealGass jew = new IdealGass();
        }

        private static double GetMolecularWeightFromName(string gasName, string[] gasNames, double[] molecularWeights, int countGases, ref bool valid)
        {
            valid = true;       //I'm adding this in case the user entered a "name" not on the list
            int counter = 0;

            gasName = gasName.ToLower();        //can be THIS nice to the user!
            foreach (string name in gasNames)
            {
                
                if (counter != countGases)      

                {
                    string tName = name.ToLower();  //This avoids an Exception- can't go
                                                    //modifying things from foreach- & name is from foreach!
                    if (tName == gasName)
                    {
                        return molecularWeights[counter];
                    }
                }
                else    //Thus, is the end of the list of gases,
                        //and the user has entered an improper name!
                {
                    valid = false;
                    return 0.0;
                }
                counter++;
            }

            return 400.0;       //Silly compiler thinks I there might be a chance it
                                //wouldn't return anything from this method w/out this!
        }
        /*
        public static double Pressure(double mass, double vol, double temp, double molecularWeight)
        {
            double n = NumberOfMoles(mass, molecularWeight);
            double r = 8.3145;
            temp = CelsiusToKelvin(temp);

            return ((n * r * temp) / vol);
        }
        */

        public static double NumberOfMoles(double mass, double moleWeight)
        {
            return mass / moleWeight;
        }

        

        private static void DisplayPressure(double pres)
        {
            Console.WriteLine("The Pressure in Pascals would be: " + pres);
            Console.WriteLine("The Pressure in PSI would be: " + PaToPSI(pres));
        }

        public static double PaToPSI(double pascals)
        {
            return 0.0001450377 * pascals;
        }
        //End of methods- here for navigation purposes
    }
}
