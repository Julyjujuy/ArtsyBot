using System;
using HtmlAgilityPack;
using System.Net;
using CsvHelper;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Xml;
using System.Threading;
using System.Xml.Serialization;


namespace ArtsyBot
{
    public class LogicMethods
    {
        public static bool ScrapeWebsite(string url)
        {
            const string filePath = @"C:\Temp\auction_items.xml";

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
            Serialize(auctionItems, filePath);

            // Save the XML to a file
            Deserialize(filePath);

            return true;
        }



        public static void Serialize(List<AuctionItem> auctionItems, string path)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<AuctionItem>));
            using (FileStream file = File.Create(path))
            {
                serializer.Serialize(file, auctionItems);
            }
        }
        public static List<AuctionItem> Deserialize(string path)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<AuctionItem>));
            using (FileStream file = File.OpenRead(path))
            {
                return (List<AuctionItem>)serializer.Deserialize(file);
            }
        }


    }
}


