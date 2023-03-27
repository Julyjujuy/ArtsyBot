using HtmlAgilityPack;
using System;

namespace ArtsyBot
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Let us scrap the whole internet!");
            string url = "https://www.example.com";

            // Call the GetHtmlDocument method to load the website's HTML document
            HtmlDocument htmlDoc = LogicMethods.GetHtmlDocument(url);

            // Use the HtmlDocument object to extract data from the website
            // Names' of the Nodes are temporary
            // foreach (HtmlNode node in htmlDoc.DocumentNode.SelectNodes("//div[@class='example-class']"))
            // {
                // Do something with the data here
                // Save the data into SQL Database
            // }

        }
    }
}