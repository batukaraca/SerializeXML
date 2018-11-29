using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Linq;
using System.Net;
using System.Xml.Serialization;
using System.Xml;
using System.Collections.Generic;

namespace ConsoleApp1
{

    public class WordCounter
    {


        public static void Main()
        {

            List<Word> Word = new List<Word>();

            string remoteUri = "http://www.gutenberg.org/files/2701/";

            string inFileName = "2701-0.txt", myStringWebResource = null;

            WebClient myWebClient = new WebClient();

            myStringWebResource = remoteUri + inFileName;
            Console.WriteLine("Downloading File \"{0}\" from \"{1}\" .......\n\n", inFileName, myStringWebResource);

            myWebClient.DownloadFile(myStringWebResource, inFileName);


            StreamReader sr = new StreamReader(inFileName);
            string text = System.IO.File.ReadAllText(inFileName);
            Regex reg_exp = new Regex("[^a-zA-Z0-9]");
            text = reg_exp.Replace(text, " ");
            string[] words = text.Split(new char[] {
            ' '
        }, StringSplitOptions.RemoveEmptyEntries);
            var word_query = (from string word in words orderby word select word).Distinct();
            string[] result = word_query.ToArray();
            int counter = 0;
            string delim = " ,.";
            string[] fields = null;
            string line = null;
            while (!sr.EndOfStream)
            {
                line = sr.ReadLine();
                line.Trim();
                fields = line.Split(delim.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                counter += fields.Length;

                foreach (string word in result)
                {
                    int count = 0;
                    int i = 0;
                    while ((i = text.IndexOf(word, i)) != -1)
                    {
                        i += word.Length;
                        count++;
                    }
                    Word.Add(new Word(word, count));
                    Serialize(Word);


                }
            }
            sr.Close();


        }

        public class Word
        {
            [XmlAttribute(AttributeName = "text")]

            public string Words { get; set; }

            [XmlAttribute(AttributeName = "count")]

            public int Count { get; set; }


            public Word(string words, int count)
            {
                this.Words = words;
                this.Count = count;


            }
            public Word() { }
        }
        static void Serialize(List<Word> Deneme)
        {
            FileStream fs = new FileStream("Word.xml", FileMode.Create);
            XmlSerializer xs = new XmlSerializer(typeof(List<Word>));
            xs.Serialize(fs, Deneme);
            fs.Close();
        }
    }
}