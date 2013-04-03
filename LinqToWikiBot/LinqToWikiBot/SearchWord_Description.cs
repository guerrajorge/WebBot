using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LinqToWikipedia;
using System.IO;

namespace LinqToWikiBot
{
    

    class SearchWord_Description
    {
        static WikipediaContext datacontext = new WikipediaContext();
        static Dictionary<int, information_structure> dictionary = new Dictionary<int, information_structure>();
        static information_structure info_struc;
        static int rn;
        static Random random_gen;



        internal void Write_Console()
        {
            StringBuilder sb = new StringBuilder();

            foreach (KeyValuePair<int, information_structure> value in dictionary)
            {
                sb.AppendLine(value.Value.question + "?");
                sb.AppendLine(value.Value.answer + "    " + value.Value.wrong_answers[0]
                    + "    " + value.Value.wrong_answers[1] + "    " + value.Value.wrong_answers[2]);
                sb.AppendLine();
            }

            using (StreamWriter outfile = new StreamWriter(@"C:\Users\Oer\Documents\GitHub\WebBot\LinqToWikiBot\Categories\wiki.xml"))
            {
                outfile.Write(sb.ToString());
            }
             

            Console.WriteLine();
            Console.WriteLine("Process Finished...");
            Console.ReadLine();
        }

        private static void formatString(StringBuilder sb2)
        {
            String info = sb2.ToString();
            int index_is = 0;
            int index_are = 0;
            int point = 0;
            StringBuilder sb3 = new StringBuilder();

            //self explanatory, need to find the index
            //so we can know where the string actually begin and remove
            //any information before these verbs
            index_is = info.IndexOf("is");
            index_are = info.IndexOf("are");
            point = info.LastIndexOf(".");

            /*if any of these index are less than zero, it means that
             * the IndexOf function did not find any string as "is" or "are"
             * and therefore set them up to "-1". I then give a value of 200 so 
             * either index becomes greater than the other one. 
             * */
            if (index_are < 0)
                index_are = 200;
            if (index_is < 0)
                index_is = 200;

            /*
             * Now I can check if any of those tokens are in the string and 
             * format the string correctly
             **/
            if (index_is > 0 && index_is < index_are)
            {
                sb2.Remove(0, index_is - 1);

            }

            else if (index_are > 0 && index_are < index_is)
            {
                sb2.Remove(0, index_are - 1);
            }

            //removes point in the description
            if (sb2.Length != 0) { sb2.Length -= 2; }

            /*
             * Inserts the interrogative pronouns based on the category
             **/
            sb2.Insert(0, "What");

        }

        internal void obtain_information(System.Collections.ArrayList list_subjects)
        {
            StringBuilder sb2 = new StringBuilder();


            for (int i = 0; i < list_subjects.Count; i++)
            {

                /*
                 * creates object: opensearch, which uses:
                 * subject = whatever it is being search in Wikipedia
                 * take(1) = limits the search to one option (the first one and that is it!)
                 * OpenSearch = the action in the wiki sandbox
                 **/
                var opensearch = (
                from wikipedia in datacontext.OpenSearch
                where wikipedia.Keyword == list_subjects[i].ToString()
                select wikipedia).Take(1);

                info_struc.answer = list_subjects[i].ToString();

                foreach (WikipediaOpenSearchResult result in opensearch)
                {
                    Console.WriteLine(i + " Processing subject: " + info_struc.answer.ToString());
                    /*
                     * add the descriptions of the searched subject to the StringBuilder sb2 and replaces
                     * the subject with ""(empty string") so the subject name is not shown in the question
                     **/
                    sb2.Append(result.Description).Replace(list_subjects[i].ToString(), "");
                    /*
                     * Make sure the string if formatted correctly and adds the interrogative pronous
                     * where needed
                     **/
                    formatString(sb2);
                    info_struc.question = sb2.ToString();
                    info_struc.wrong_answers = fill_wrong_answers(list_subjects);
                    //add the name and question to the dictionary list
                    dictionary.Add(i, info_struc);
                    //clears sb2 becuase if not then everytime we run dictionary.add ... whatever information is
                    //in sb2 gets added to the second string, which we dont want DUH!
                    sb2.Clear();
                }
            }
        }

        private string[] fill_wrong_answers(System.Collections.ArrayList list_subjects)
        {
            int number_subjects = list_subjects.Count;
            String[] cuatro_answers = new String[3];
            random_gen = new Random(number_subjects);

            for (int j = 0; j < 3; j++)
            {
                rn = GetRandomNumber(0,number_subjects);
                cuatro_answers[j] = list_subjects[rn].ToString();
            }

            return cuatro_answers;
        }

        //Function to get random number
        private static readonly Random getrandom = new Random();
        private static readonly object syncLock = new object();
        public static int GetRandomNumber(int min, int max)
         {
           lock(syncLock)
                    return getrandom .Next(min, max);
         }
    }
    
    /*
     * Structure of the information obtain from Wikipedia
     * and how it is going to be passed to the database
     **/ 
    struct information_structure
    {
        public String category;
        public String question;
        public String answer;
        public String[] wrong_answers;
    };

}
