using HtmlAgilityPack;
using System;

namespace ArtsyBot
{
    internal class Program
    {

        public static void Main()
        {
            string baseUrl = "https://www.liveauctioneers.com/c/clocks/74/";

            // Load the first page of the website
            HtmlDocument htmlDoc = LogicMethods.GetHtmlDocument(baseUrl);

            // Find the total number of pages on the website
            int totalPages = LogicMethods.GetTotalPages(htmlDoc);
            Console.WriteLine(totalPages.ToString());

            // Scrape data from all pages of the website
            for (int page = 1; page <= totalPages; page++)
            {
                string url = $"{baseUrl}?page={page}";
                LogicMethods.ScrapeWebsite(url);
            }


            // Scrape the first page
            LogicMethods.ScrapeWebsite(baseUrl);

            // Scrape the remaining pages
            for (int page = 2; page <= totalPages; page++)
            {
                string pageUrl = $"{baseUrl}?page={page}";
                LogicMethods.ScrapeWebsite(pageUrl);
            }
        }
    }
}
