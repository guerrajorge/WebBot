﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LinqToWikipedia;
using System.IO;
using System.Reflection;

namespace LinqToWikiBot
{

    class SearchWord_Description
    {
        static WikipediaContext datacontext = new WikipediaContext();
        static Dictionary<int, information_structure> dictionary = new Dictionary<int, information_structure>();
        static information_structure info_struc;
        static int rn;
        static Random random_gen;
        static string category_subject;
        static string[] subject_print = new string[2];

        //use for debugging purposes
        internal void sendsubject(string subject)
        {
            if (subject.Contains("Category"))
                subject_print = subject.Split(':');
            else subject_print[1] = subject;
        }
        //this is used for debugging purposes as well
        internal void Write_Console()
        {
            //Printing space on the Console for the presentation
            Console.WriteLine();
            var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            StringBuilder path_app = new StringBuilder();
            path_app.Append(path.ToString());
            path_app.Replace(@"\WebBot\LinqToWikiBot\LinqToWikiBot\bin\Debug", @"\Trivia-Game\TriviaGame\bin\");
            path_app.Append(subject_print[1] + "_" + category_subject + ".xml");
            path_app.Replace(" ", string.Empty);

            StringBuilder sb = new StringBuilder();

            foreach (KeyValuePair<int, information_structure> value in dictionary)
            {
                if (value.Value.question.Contains("Who is") || value.Value.question.Contains("What is") ||
                    value.Value.question.Contains("Who are") || value.Value.question.Contains("What are"))
                {
                    sb.AppendLine(value.Value.question + "?");
                    sb.AppendLine(value.Value.answer + "    " + value.Value.wrong_answers[0]
                        + "    " + value.Value.wrong_answers[1] + "    " + value.Value.wrong_answers[2]);
                    sb.AppendLine();
                }
            }

            //Printing on the Console for the presentation
            Console.WriteLine(sb.ToString());
            sb.Clear();
            //using (StreamWriter outfile = new StreamWriter(path_app.ToString()))
            //{
            //    outfile.Write(sb.ToString());
            //}
             

            Console.WriteLine();
            Console.WriteLine("Process Finished...");
            //Console.ReadLine();

            dictionary.Clear();
        }

        private static void formatString(StringBuilder sb2)
        {
            String info = sb2.ToString();
            int index_is = 0;
            int index_are = 0;
            int point = 0;
            StringBuilder sb3 = new StringBuilder();
            int indexarechanger = 0;

            //self explanatory, need to find the index
            //so we can know where the string actually begin and remove
            //any information before these verbs
            index_is = info.IndexOf("is");
            index_are = info.IndexOf("are");
            point = info.LastIndexOf(".");

            if (info.Contains("are a"))
            {
                sb2.Replace("are a", "is a");
                info = info.Replace("are a", "is a");
                index_is = info.IndexOf("is");
                index_are = -1;
            }

            if (info.Contains("are an"))
            {
                sb2.Replace("are an", "is an");
                info = info.Replace("are a", "is a");
                index_is = info.IndexOf("is");
                index_are = -1;
            }

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
                indexarechanger = info.IndexOf("are");
                sb2.Remove(0, index_are - 1);
            }

            //removes point in the description
            if (sb2.Length != 0 && sb2.Length != 1) 
                sb2.Length -= 2; 

            /*
             * Inserts the interrogative pronouns based on the category
             **/
            if (!category_subject.Contains("People")) 
            sb2.Insert(0, "What");
            else
            sb2.Insert(0, "Who");
            

        }

        //Use only for the presentation!!!
        internal void Obtain_information_singlesubject(string subject_passed)
        {
            StringBuilder sb2 = new StringBuilder();


                /*
                 * creates object: opensearch, which uses:
                 * subject = whatever it is being search in Wikipedia
                 * take(1) = limits the search to one option (the first one and that is it!)
                 * OpenSearch = the action in the wiki sandbox
                 **/
                var opensearch = (
                from wikipedia in datacontext.OpenSearch
                where wikipedia.Keyword == subject_passed
                select wikipedia).Take(1);

                info_struc.answer = subject_passed;

                foreach (WikipediaOpenSearchResult result in opensearch)
                {
                    /*
                     * add the descriptions of the searched subject to the StringBuilder sb2 and replaces
                     * the subject with ""(empty string") so the subject name is not shown in the question
                     **/
                    sb2.Append(result.Description).Replace(subject_passed, "");
                    /*
                     * Make sure the string if formatted correctly and adds the interrogative pronous
                     * where needed
                     **/
                    formatString_singlesubect(sb2);
                    info_struc.question = sb2.ToString();
                    //add the name and question to the dictionary list
                    dictionary.Add(0, info_struc);
                    //clears sb2 becuase if not then everytime we run dictionary.add ... whatever information is
                    //in sb2 gets added to the second string, which we dont want DUH!
                    sb2.Clear();
                }

                Write_Console_singlesubject();
                dictionary.Clear();
            
        }

        private void formatString_singlesubect(StringBuilder sb2)
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

            else if (index_are > 0 && index_are < index_is && index_are < 30)
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

        //only for presentation reasons!
        private static void Write_Console_singlesubject()
        {
            Console.WriteLine();

            foreach (KeyValuePair<int, information_structure> value in dictionary)
            {
                Console.WriteLine(value.Value.question + "?");
                Console.WriteLine(value.Value.answer);
                Console.WriteLine();
            }

            Console.WriteLine();
            Console.WriteLine("Process Finished...");
            //Console.ReadLine();
        }


        internal void obtain_information(System.Collections.ArrayList list_subjects, string Category)
        {
            StringBuilder sb2 = new StringBuilder();
            category_subject = Category;
            int j = 0;

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
                    Console.WriteLine(j + " Processing subject: " + info_struc.answer.ToString());
                    //use to mark the right number of printing elements
                    j++;
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
                    info_struc.category = Category;
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
