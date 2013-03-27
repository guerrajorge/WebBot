using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Xml;
using System.Xml.XPath;

namespace LinqToWikiBot
{
    class Program
    {
        static StringBuilder subject = new StringBuilder();
        static String response;
        static ArrayList list_subjects = new ArrayList();

        static void Main(string[] args)
        {

            //Whatever wer are looking for
            string subject = "";

            Console.WriteLine("Input a subject");
            subject = System.Console.ReadLine();

            //This is where we are going to get the information
            Uri address = getAddress(subject);

            WebClient client = new WebClient();
            printToFile(address, client);
        }

        private static void printToFile(Uri address, WebClient client)
        {

            client.Headers.Add("User-Agent", "TriviaGame");

            try
            {
                //Obtaind the string from the URI type variable
                response = client.DownloadString(address);
                //Formats it in the right way
                response = formatSubjectString(response);

                File.AppendAllText(@"C:\Users\Oer\Documents\GitHub\WebBot\LinqToWikiBot\wiki.xml", response);
                Console.Write("Process finished...");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            Console.ReadLine();
        }

        private static string formatSubjectString(string response)
        {
            /*
             * These string builders are used to format the string in the right way to be processed,
             * it is easier than to format actual string type variables. At the end the only that matters 
             * is the sb StringBuilder.
             * The regex is the object used for the regular expression, it is needed in order to get rib off
             * all the number, non characters, and garbage the string brings
             **/ 
            StringBuilder sb2 = new StringBuilder();
            StringBuilder sb = new StringBuilder();
            Regex regex = new Regex(@"\\[\w+\\]");

            //this area gets rib of all the non used parts of the strings
            int index_start = response.IndexOf("===");
            sb2.Append(response);
            sb2.Remove(0, index_start - 1);
            sb2.Replace(" ", string.Empty);
            sb2.Replace("*", string.Empty);
            
            response = sb2.ToString();


            for (int i = 0; i < response.Length; i++)
            {
                if (0 == response[i].CompareTo('['))
                {
                    if (0 == response[i + 1].CompareTo('['))
                    {
                        //Inser every value it finds between the first braket and before it get to the 
                        //close braket
                        while (response[i] != ']')
                        {
                            //starts from the i++ because the string have to brakets
                            //example: [[basketball] and since we only want the word we start for the i++ position
                            i++;
                            sb.Append(response[i]);

                        }
                        //Insert line
                        sb.AppendLine();
                    }
                }
            }

            //it takes off whatever it is on the parameneters
            sb.Replace("\r", "");
            sb.Replace("[", "");
            sb.Replace("]", "");

            /*
             * I pass the stringBuilder to an string so I could split it based on the '\n'
             * then I insert all this splis into an array of string in order to proceess it 
             **/ 
            String ss = sb.ToString();
            string[] namesArray = ss.Split('\n');
            Boolean number;
            Boolean file;
            Boolean characters;
            
            for (int i = 0; i < namesArray.Length; i++)
            {
                /*
                 * number: looks to see if there are numbers in the string
                 * file: looks to see if the string file appears in any of the strings
                 * characters: looks to see if any of the chars in the string are not chars
                 **/ 
                number = Regex.IsMatch(namesArray[i], @"\d$");
                file = Regex.IsMatch(namesArray[i], @"^File");
                characters = Regex.IsMatch(namesArray[i], @"\W|_");

                /*
                 * Logic to making sure it prints the right lines and it
                 * empties the wrong elements
                 **/ 
                if (number == false && file == false && characters == false)
                    Console.WriteLine(namesArray[i]);
                else {
                    namesArray[i] = "";
                }
            }
            

            //Console.WriteLine(sb);
            Console.WriteLine("Done");

            return response;
        }

        private static Uri getAddress(string subject)
        {
            //Variable definition
            string action = "query";
            string prop = "revisions";
            string format = "txt";
            string rvprop = "content";
            int rvlimit = 10;
            //int rvsection = 0;

            //easiest to read info form
            //Uri address = new Uri("http://en.wikipedia.org/w/api.php?action=query&prop=revisions&format=xml&rvprop=content&rvlimit=10&rvsection=0&titles=Country&titles=Country");

            Uri address = new Uri("http://en.wikipedia.org/w/api.php?" + "action=" + action + "&prop=" + prop + "&format=" +
                                            format + "&rvprop=" + rvprop + "&rvlimit=" + rvlimit + "&titles=" + subject + "&titles=" + subject);
            return address;
        }
    }
}
