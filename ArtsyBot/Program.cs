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

            

        }
    }
}