using System;
using HtmlAgilityPack;
using System.Net;
using CsvHelper;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Xml;
using System.Threading;


namespace ArtsyBot
{
    public class LogicMethods
    {
      
        public static bool ScrapeWebsite(string url)
        {
            // Load the website's HTML document
            HtmlWeb web = new HtmlWeb();
            HtmlDocument htmlDoc = web.Load(url);
            var lastStatusCode = HttpStatusCode.OK;

            web.PostResponse = (request, response) =>
            {
                if (response != null)
                {
                    lastStatusCode = response.StatusCode;
                }
            };

            if (lastStatusCode != HttpStatusCode.OK) 
            {
                ;
                return false;
                // Reason: the page request failed

            }

            // Create a list to hold the scraped items
            List<AuctionItem> auctionItems = new List<AuctionItem>();

            // Find all the article containers on the page
            var articleContainers = htmlDoc.DocumentNode.SelectNodes("//article[contains(@class, 'CategorySearchCard__StyledCard-sc-1o7izf2-0')]");
            if (articleContainers != null)
            {
                int counter = 0; // Initialize the counter variable
                foreach (var container in articleContainers)
                {
                    Thread.Sleep(60*1000);
                    // Get the image URL from main Page
                    var imageUrl = container.SelectSingleNode(".//img[1]")?.GetAttributeValue("src", "");

                    // Get the general description
                    var description = container.SelectSingleNode(".//img[1]")?.GetAttributeValue("alt", "");

                    // Get the page URL
                    var pageUrlElement = container.SelectSingleNode(".//a");
                    var pageUrl = pageUrlElement?.GetAttributeValue("href", "");

                    // Concatenate the prefix to the page URL
                    pageUrl = "https://www.liveauctioneers.com" + pageUrl;

                    // Navigate to the page URL and load the inner HTML document
                    HtmlDocument innerHtmlDoc = web.Load(pageUrl);

                    //todo: check if HtmlWeb saves coockies => it doesnt. Tried to save cookies in a cookie container and it was always empty
           
                    // Extract the Estimate text
                    var estimateNode = innerHtmlDoc.DocumentNode.SelectSingleNode("//div[@class='ItemBiddingEstimate__StyledEstimateAmounts-sc-e0x4f0-4 eRGEFe']");
                    var estimatedPrice = estimateNode?.InnerText;

                    // Extract the description section
                    var spanElement = innerHtmlDoc.DocumentNode.SelectSingleNode("//span[@data-testid='body-primary' and contains(@class, 'sc-vjKnw kanvka DescriptionSection__StyledBody-sc-trkwix-2 gtFpYt')]");
                    string spanText = spanElement?.InnerText ?? string.Empty;

                    // Extract the image URL from AuctioneerInfo
                    var auctioneerImageNode = innerHtmlDoc.DocumentNode.SelectSingleNode("//img[contains(@class, 'AuctioneerInfo__StyledImageWithFallback-sc-1erc2m8-2 eoKPJL no-js-jasper52_large')]");
                    var auctioneerImageUrl = auctioneerImageNode?.GetAttributeValue("src", "");

                    // Increment the counter
                    counter++;

                    // Create a new AuctionItem object
                    AuctionItem item = new AuctionItem
                    {
                        ImageUrl = imageUrl,
                        Description = description,
                        PageUrl = pageUrl,
                        EstimatedPrice = estimatedPrice,
                        LongDescription = spanText
                    };
                    // Add the item to the list
                    auctionItems.Add(item);
                  

                    // Output the results
                    Console.WriteLine($"Image URL: {imageUrl}");
                    Console.WriteLine($"Description: {description}");
                    Console.WriteLine($"Page URL: {pageUrl}");
                    Console.WriteLine($"Estimate: {estimatedPrice}");
                    Console.WriteLine($"Description Section: {spanText}");
                    Console.WriteLine($"AuctioneerImageUrl: {auctioneerImageUrl}");
                    Console.WriteLine();
                }

                // Output the total number of elements scraped
                Console.WriteLine($"Scraped {counter} elements.");
            }
            else
            {
                Console.WriteLine("No article containers found.");
            }
            // Convert the scraped items to XML
            string xml = ConvertToXml(auctionItems);

            // Save the XML to a file
            string filePath = @"C:\Temp\auction_items.xml";
            SaveXmlToFile(xml, filePath);

            return true;
        }



        private static string ConvertToXml(List<AuctionItem> auctionItems)
        {
            XmlDocument xmlDoc = new XmlDocument();

            // create the root element
            XmlElement rootElement = xmlDoc.CreateElement("AuctionItems");
            xmlDoc.AppendChild(rootElement);

            // create child elements for each item
            foreach (var item in auctionItems)
            {
                XmlElement itemElement = xmlDoc.CreateElement("AuctionItem");

                XmlElement imageUrlElement = xmlDoc.CreateElement("ImageUrl");
                imageUrlElement.InnerText = item.ImageUrl;
                itemElement.AppendChild(imageUrlElement);

                XmlElement descriptionElement = xmlDoc.CreateElement("Description");
                descriptionElement.InnerText = item.Description;
                itemElement.AppendChild(descriptionElement);

                XmlElement pageUrlElement = xmlDoc.CreateElement("PageUrl");
                pageUrlElement.InnerText = item.PageUrl;
                itemElement.AppendChild(pageUrlElement);

                XmlElement estimatedPriceElement = xmlDoc.CreateElement("EstimatedPrice");
                estimatedPriceElement.InnerText = item.EstimatedPrice;
                itemElement.AppendChild(estimatedPriceElement);

                XmlElement spanTextElement = xmlDoc.CreateElement("LongDescription");
                spanTextElement.InnerText = item.LongDescription;
                itemElement.AppendChild(spanTextElement);

                rootElement.AppendChild(itemElement);
            }

            return xmlDoc.OuterXml;
        }
        private static void SaveXmlToFile(string xml, string filePath)
        {
            // Save the XML 
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xml);
            xmlDoc.Save(filePath);
        }

    }
}


