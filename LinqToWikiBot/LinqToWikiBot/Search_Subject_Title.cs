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
using LinqToWikipedia;

namespace LinqToWikiBot
{
    class Search_Subject_Title
    {
        static StringBuilder subject = new StringBuilder();
        static String response;
        static ArrayList list_subjects = new ArrayList();
        static string[] namesArray;
        static SearchWord_Description word_description = new SearchWord_Description();
        static Boolean equalsigns;
        static Boolean file;
        static Boolean characters;
        static Boolean comma;
        static Boolean category_label;
        static Boolean list;
        static Boolean tourist;
        static Boolean parenthesis;
        static Boolean space;
        static string str;

        static void Main(string[] args)
        {
            //Whatever wer are looking for
            Console.WriteLine("Input a subject");
            string subject = System.Console.ReadLine();

            if (subject.StartsWith("Category"))
                category(subject);

            else if (subject.StartsWith("Continents"))
                continentsfunc();
            else if (!subject.StartsWith("Category"))
                not_category(subject);

            list_subjects.Sort();
            Console.WriteLine("Done obtaining and processing subjects");
            Console.WriteLine("Number of subjects: " + list_subjects.Count);

            word_description.obtain_information(list_subjects);
            word_description.Write_Console();
                
        }

        private static void continentsfunc()
        {
            String[] continents = new String[] { "Africa", "Antarctica", "Asia", "Europe", "North America", "South America", "Central America" };

            foreach (string str in continents)
                list_subjects.Add(str);

        }

        private static void category(string subject)
        {
            var wiki = new Wiki("TheNameOfMyBot/1.0 (http://website, myemail@site)", "en.wikipedia.org");

            var pages = (from cm in wiki.Query.categorymembers()
                         where cm.title == subject
                         select cm.title)
            .ToEnumerable();

            Write(pages);
        }

        private static void Write<T>(IEnumerable<T> results)
        {
            var array = results.ToArray();

            foreach (var result in array)
               verifystring(result.ToString());

        }


        private static void not_category(string subject)
        {
            //This is where we are going to get the information
            Uri address = getAddress(subject);

            WebClient client = new WebClient();
            obtainsubjects(address, client);
        }

        private static void obtainsubjects(Uri address, WebClient client)
        {
            client.Headers.Add("User-Agent", "TriviaGame");
            try
            {
                //Obtaind the string from the URI type variable
                response = client.DownloadString(address);
                //Formats it in the right way
                formatSubjectString(response);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message + " " + e.Data + " " + e.TargetSite);
            }
        }


        private static void formatSubjectString(string response)
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
            int index_start = response.IndexOf("==");
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
            sb.Replace("\r", string.Empty);
            sb.Replace("[", string.Empty);
            sb.Replace("]", string.Empty);
            sb.Replace("''", string.Empty);

            /*
             * I pass the stringBuilder to an string so I could split it based on the '\n'
             * then I insert all this splis into an array of string in order to proceess it 
             **/ 
            String ss = sb.ToString();
            namesArray = ss.Split('\n');
            

            for (int i = 0; i < 500 ; i++)
                verifystring(namesArray[i]);

        }

        private static void verifystring(string str)
        {

            int commaIndex = 0;

            /* Want to remove commas because for geography, the words after the commas define the states and country of the place
             * which we do not care
             **/

            comma = str.Contains(",");
            if (comma)
            {
                commaIndex = str.IndexOf(",");
                str = str.Remove(commaIndex);
            }

            category_label = str.Contains("Category:");

            if (category_label)
            {
                commaIndex = str.IndexOf("Category:");
                str = str.Remove(commaIndex);
            }

            /*
                 * file: looks to see if the string file appears in any of the strings
                 * characters: looks to see if any of the chars in the string are not chars expect for spaces
                 * List: check to see if the string starts with the keyword "List" some strings in Wikipedia do
                 * Tourist: check to see if the string starts with the keyword "Tourist" some strings in Wikipedia do
                 * Parentheses = checks for "(" and ")"
                 * Equalsigns = checks for "==" when subject are derives from other labels, these other label are defined with "==="
                 **/
            file = Regex.IsMatch(str, @"^File");
            list = Regex.IsMatch(str, @"^List");
            characters = Regex.IsMatch(str, @"[^\w\s]");
            equalsigns = str.Contains("==");
            tourist = Regex.IsMatch(str, @"Tourist");
            parenthesis = Regex.IsMatch(str, @"\(\)");
            space = Regex.IsMatch(str, @"^\s");

            /*
             * Logic to making sure it prints the right lines and it
             * empties the wrong elements
             **/
            if (parenthesis == false && tourist == false && equalsigns == false && list == false && file == false && characters == false)
            {
                //Some times the subject start with spaces, other times they do not, so of course ... we must check
                if (space)
                    str = str.Remove(0, 1);
            }

            else
                str = "";

            if (str != "")
                list_subjects.Add(str);
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
