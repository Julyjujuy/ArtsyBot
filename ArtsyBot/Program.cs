using HtmlAgilityPack;
using System;
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
                LogicMethods.ScrapeWebsite(restBaseUrl);

                // Pause for a moment
                Thread.Sleep(1000);
            }

            // Scrape data from the first page
            LogicMethods.ScrapeWebsite(baseUrl);

            Console.WriteLine("Done scraping!");
            Console.ReadLine();
        }
    }
}