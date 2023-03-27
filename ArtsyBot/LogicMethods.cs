using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.XPath;


namespace ArtsyBot
{
    internal class LogicMethods
    {
        //takes the url and transform the page into a HtmlDocument
        public static HtmlDocument GetHtmlDocument(string url)
        {
            HtmlWeb web = new HtmlWeb();
            return web.Load(url);
        }
        public static void ScrapeWebsite(string url)
        {
            // Load the website's HTML document
            HtmlDocument htmlDoc = GetHtmlDocument(url);

            // Find the elements to scrape using XPath
            // Element's names are temporary
            HtmlNode imageElement = htmlDoc.DocumentNode.SelectSingleNode("//img[@class='my-image']");
            HtmlNode DescriptionElement = htmlDoc.DocumentNode.SelectSingleNode("//span[@class='y']");
            HtmlNode urlElement = htmlDoc.DocumentNode.SelectSingleNode("//a[@href='/z']");

            // Extract the values of the elements as strings
            string imageUrl = imageElement?.GetAttributeValue("src", "").Trim();
            string yValue = DescriptionElement?.GetAttributeValue("description", "").Trim();
            string zValue = urlElement?.GetAttributeValue("href", "").Trim();

            // Do something with the scraped data 
            Console.WriteLine($"URL of image: {imageUrl}");
            Console.WriteLine($"Value of Y: {yValue}");
            Console.WriteLine($"Value of Z: {zValue}");
        }


    }
}