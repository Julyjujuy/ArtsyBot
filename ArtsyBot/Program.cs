using HtmlAgilityPack;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;

namespace ArtsyBot
{
    internal class Program
    {
        public static void Main()
        {
            string baseUrl = "https://www.liveauctioneers.com/c/clocks/74/";

            // Loop from 2 to 100, constructing and scraping each page
            for (int i = 2; i <= 100; i++)
            {
                // Construct the new URL for this page
                string restBaseUrl = $"{baseUrl}?page={i}";

                // Scrape data from this page
                bool sucess = LogicMethods.ScrapeWebsite(restBaseUrl);

                if (!sucess) 
                {
                    Console.WriteLine("Faild at Reuest no:" + i); 
                }

                // Pause function with a random time between 1000 and 5000
                Thread.Sleep(new Random().Next(1000, 5000));
            }

            // Scrape data from the first page
            LogicMethods.ScrapeWebsite(baseUrl);

            Console.WriteLine("Done scraping!");

            Console.ReadLine();
        }
    }
}