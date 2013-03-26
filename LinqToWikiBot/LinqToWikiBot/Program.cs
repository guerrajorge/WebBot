using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml;
using System.Xml.XPath;

namespace LinqToWikiBot
{
    class Program
    {
        static StringBuilder subject = new StringBuilder();

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
                string response = client.DownloadString(address);
                formatSubjectString(response);

                File.AppendAllText(@"C:\Users\Oer\Documents\Visual Studio 2012\Projects\LinqToWikiBot\wiki.xml", response);
                Console.Write("Process finished...");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            Console.ReadLine();
        }

        private static void formatSubjectString(string response)
        {
            StringBuilder sb = new StringBuilder();
            StringBuilder sb2 = new StringBuilder();
            sb2.Append(response);
            sb2.Replace("\n", string.Empty);
            sb2.Replace(" ", string.Empty);
            response = sb2.ToString();
            Boolean flag = false;

            foreach (char c in response)
            {
                if (0 == c.CompareTo('['))
                {
                    if (flag)
                    {
                    }
                    flag = true;
                }
            }
            for (int i=0; i<response.Length;i++)
            {
                if (0 == response[i].CompareTo('['))
                {
                    if (0 == response[i + 1].CompareTo('['))
                    {
                        while(response[i] != ']')
                        {
                            i++;
                            sb.Append(response[i]);
                            
                        }
                        sb.AppendLine();
                    }
                }
            }
            Console.WriteLine("Done");
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
