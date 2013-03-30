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
using LinqToWiki.Generated;

namespace LinqToWikiBot
{
    class Search_Subject_Title
    {
        static StringBuilder subject = new StringBuilder();
        static String response;
        static ArrayList list_subjects = new ArrayList();
        static string[] namesArray;

        static void Main(string[] args)
        {

            //Whatever wer are looking for
            Console.WriteLine("Input a subject");
            string subject = System.Console.ReadLine();

            //This is where we are going to get the information
            Uri address = getAddress(subject);

            WebClient client = new WebClient();
            obtainsubjects(address, client);

            SearchWord_Description word_descrition = new SearchWord_Description();

            word_descrition.obtain_information(list_subjects);
            word_descrition.Write_Console();
            
        }

        private static void obtainsubjects(Uri address, WebClient client)
        {

            client.Headers.Add("User-Agent", "TriviaGame");

            try
            {
                //Obtaind the string from the URI type variable
                response = client.DownloadString(address);
                //Formats it in the right way
                response = formatSubjectString(response);

                //File.AppendAllText(@"C:\Users\Oer\Documents\GitHub\WebBot\LinqToWikiBot\wiki.xml", namesArray.ToString());
                
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
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
            //sb2.Replace(" ", string.Empty);
            sb2.Replace("*", string.Empty);
            sb2.Replace("\n\n", "\n");
            
            response = sb2.ToString();

            //check to see if there is a new line symbol "\n" if there is, it just puts a new line
            //on the stringbuilder if there is not then it add the char to the specific string in the string builder.
            for (int i = 1; i < response.Length; i++)
            {
                if (response[i] == '/' && 0 == response[i+1].CompareTo('n'))
                    sb.AppendLine();
                else
                {
                    sb.Append(response[i]);
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
            namesArray = ss.Split('\n');
            Boolean equalsigns;
            Boolean file;
            Boolean characters;
            Boolean comma;
            Boolean list;
            Boolean tourist;
            Boolean parenthesis;
            Boolean space;
            int commaIndex = 0;

            
            for (int i = 0; i < 500 ; i++)
            {
                /*
                 * Want to remove commas because for geograpthy, the words after the commas define the states and country of the place
                 * which we do not care
                 **/ 
                comma = namesArray[i].Contains(",");
                if (comma)
                {
                    commaIndex = namesArray[i].IndexOf(",");
                    namesArray[i] = namesArray[i].Remove(commaIndex);
                }
                /*
                 * file: looks to see if the string file appears in any of the strings
                 * characters: looks to see if any of the chars in the string are not chars expect for spaces
                 * List: check to see if the string starts with the keyword "List" some strings in Wikipedia do
                 * Tourist: check to see if the string starts with the keyword "Tourist" some strings in Wikipedia do
                 * Parentheses = checks for "(" and ")"
                 * Equalsigns = checks for "==" when subject are derives from other labels, these other label are defined with "==="
                 **/
                file = Regex.IsMatch(namesArray[i], @"^File");
                list = Regex.IsMatch(namesArray[i], @"^List");
                characters = Regex.IsMatch(namesArray[i], @"[^\w\s]");
                equalsigns = namesArray[i].Contains("==");
                tourist = Regex.IsMatch(namesArray[i], @"Tourist");
                parenthesis = Regex.IsMatch(namesArray[i], @"\(\)");
                space = Regex.IsMatch(namesArray[i], @"^\s");
                
                /*
                 * Logic to making sure it prints the right lines and it
                 * empties the wrong elements
                 **/
                if (parenthesis == false && tourist == false && equalsigns == false && list == false && file == false && characters == false)
                {
                    //Some times the subject start with spaces, other times they do not, so of course ... we must check
                    if (space)
                        namesArray[i] = namesArray[i].Remove(0, 1);
                }

                else
                {
                    namesArray[i] = "";
                }

                if (namesArray[i] != "")
                    list_subjects.Add(namesArray[i]);
            }
            //File.AppendAllText(@"C:\Users\Oer\Documents\GitHub\WebBot\LinqToWikiBot\wiki.xml", namesArray.ToString());
            //System.IO.File.WriteAllLines(@"C:\Users\Oer\Documents\GitHub\WebBot\LinqToWikiBot\wiki.xml", list_subjects.ToString());
            //Console.WriteLine(sb);
            Console.WriteLine("Done obtaining and processing subjects");
            Console.WriteLine("Number of subjects: " + list_subjects.Count);

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
            
            //obtains the information specified at the given address
            Uri address = new Uri("http://en.wikipedia.org/w/api.php?" + "action=" + action + "&prop=" + prop + "&format=" +
                                            format + "&rvprop=" + rvprop + "&rvlimit=" + rvlimit + "&titles=" + subject + "&titles=" + subject);
            return address;
        }
    }
}
