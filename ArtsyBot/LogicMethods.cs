﻿using System;
using HtmlAgilityPack;
using System.Net;
using System.Xml.Serialization;
using System.Text;
using System.Diagnostics;

namespace ArtsyBot
{
    public class LogicMethods
    {
        public static bool ScrapeWebsite(string url, List<AuctionItem> items, int delaySeconds)
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

                    Thread.Sleep(delaySeconds * 1000);
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

                    //check if load suceesful
                    if (innerHtmlDoc == null)
                    {
                        //load failed / limit hit
                        Thread.Sleep(60 * 1000);
                        innerHtmlDoc = web.Load(pageUrl);
                        if (innerHtmlDoc == null)
                            Debugger.Break();

                    }

                    // Extract the Estimate text again in case it is encased
                    var estimateNode = innerHtmlDoc.DocumentNode.SelectSingleNode("//div[@class='ItemBiddingEstimate__StyledEstimateAmounts-sc-e0x4f0-4 eRGEFe']");

                    //check if load suceesful
                    if(estimateNode == null)
                    {
                        //load failed / limit hit
                        Thread.Sleep(60 * 1000);
                        innerHtmlDoc = web.Load(pageUrl);
                        estimateNode = innerHtmlDoc.DocumentNode.SelectSingleNode("//div[@class='ItemBiddingEstimate__StyledEstimateAmounts-sc-e0x4f0-4 eRGEFe']");
                        if (estimateNode == null)
                            Debugger.Break();//2 tries still boo TODO: implement handling

                    }

                    var estimatedPrice = estimateNode?.InnerText;

                    //sometimes unhappy with estimatedPrice
                    if(estimatedPrice == null || estimatedPrice.Contains("something"))
                    {
                        ;
                    }

                    // Extract the description section
                    var spanElement = innerHtmlDoc.DocumentNode.SelectSingleNode("//span[@data-testid='body-primary' and contains(@class, 'sc-vjKnw kanvka DescriptionSection__StyledBody-sc-trkwix-2 gtFpYt')]");
                    string spanText = spanElement?.InnerText ?? string.Empty;

                    // Extract the name from AuctioneerInfo
                    var auctioneerNameNode = innerHtmlDoc.DocumentNode.SelectSingleNode("//span[@data-testid='itemPageSellerName' and @class='sc-hLseeU AuctioneerInfo__SellerNameText-sc-1erc2m8-5 jnbWxy chuhQu']");


                    if (auctioneerNameNode == null)
                    {
                        
                        Thread.Sleep(60 * 1000);
                        innerHtmlDoc = web.Load(pageUrl);
                        auctioneerNameNode = innerHtmlDoc.DocumentNode.SelectSingleNode("//span[@data-testid='itemPageSellerName' and @class='sc-hLseeU AuctioneerInfo__SellerNameText-sc-1erc2m8-5 jnbWxy chuhQu']");
                        if (auctioneerNameNode == null)
                            Debugger.Break();

                    }

                    var auctioneerName = auctioneerNameNode?.InnerText;

                    // Extract the time left before the Auction
                    var dateNode = innerHtmlDoc.DocumentNode.SelectSingleNode("//div[contains(@class, 'lt1g861EjAKl0fG1gZvV') and contains(@class, 'BiddingCountdown__StyledCountdown-sc-et36hu-0')]");
                    var auctionTimeLeft = dateNode?.InnerText;

                    //// Extract the starting price
                    var priceNode = htmlDoc.DocumentNode.SelectSingleNode("//h1[@data-testid='h1']/span[@class='FormattedCurrency__StyledFormattedCurrency-sc-1ugrxi1-0 kRoxAz']");

                    if (priceNode == null)
                    {
                        
                        Thread.Sleep(60 * 1000);
                        innerHtmlDoc = web.Load(pageUrl);
                        priceNode = innerHtmlDoc.DocumentNode.SelectSingleNode("//h1[@data-testid='h1']/span[@class='FormattedCurrency__StyledFormattedCurrency-sc-1ugrxi1-0 kRoxAz']");
                        if (priceNode == null)
                            Debugger.Break();

                    }



                    string startingPrice = priceNode?.InnerText ?? string.Empty;


                    // Increment the counter
                    counter++;

                    // Create a new AuctionItem object
                    AuctionItem item = new AuctionItem
                    {
                        ImageUrl = imageUrl,
                        Description = description,
                        PageUrl = pageUrl,
                        EstimatedPrice = estimatedPrice,
                        LongDescription = spanText,
                        AuctioneerName = auctioneerName,
                        AuctionTimeLeft = auctionTimeLeft,
                        StartingPrice = startingPrice,

                    };
                    // Add the item to the list
                    auctionItems.Add(item);
                  
                    // Output the results
                    Console.WriteLine($"Image URL: {imageUrl}");
                    Console.WriteLine($"Description: {description}");
                    Console.WriteLine($"Page URL: {pageUrl}");
                    Console.WriteLine($"Estimate: {estimatedPrice}");
                    Console.WriteLine($"Description Section: {spanText}");
                    Console.WriteLine($"AuctioneerName: {auctioneerName}");
                    Console.WriteLine($"Time left before Auction: {auctionTimeLeft}");
                    Console.WriteLine($"Starting Price: {startingPrice}");
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

        public static class Logger
        {
            private static string logFilePath = "log.txt";
            private static string reportFilePath = "report.txt";

            public static void Log(string message)
            {
                string logEntry = $"[{DateTime.Now}] {message}";

                // Append the log entry to the log file
                File.AppendAllText(logFilePath, logEntry + Environment.NewLine);

                // Print the log entry to the console
                Console.WriteLine(logEntry);
            }

            public static void GenerateReport(List<AuctionItem> auctionItems, int successfulExtractions, int failedExtractions)
            {
                // Generate a report summary
                StringBuilder reportSummary = new StringBuilder();
                reportSummary.AppendLine("Scraping Report");
                reportSummary.AppendLine($"Total Items: {auctionItems.Count}");
                reportSummary.AppendLine($"Successful Extractions: {successfulExtractions}");
                reportSummary.AppendLine($"Failed Extractions: {failedExtractions}");

                // Generate a report details for each auction item
                StringBuilder reportDetails = new StringBuilder();
                reportDetails.AppendLine("Scraped Items:");
                foreach (var item in auctionItems)
                {
                    reportDetails.AppendLine($"- {item.Description}");
                    reportDetails.AppendLine($"  Image URL: {item.ImageUrl}");
                    reportDetails.AppendLine($"  Page URL: {item.PageUrl}");
                    reportDetails.AppendLine($"  Estimated Price: {item.EstimatedPrice}");
                    reportDetails.AppendLine($"  Description Section: {item.LongDescription}");
                    reportDetails.AppendLine($"  Description Section: {item.AuctioneerName}");
                    reportDetails.AppendLine();
                }

                // Generate the final report
                StringBuilder report = new StringBuilder();
                report.Append(reportSummary);
                report.AppendLine();
                report.Append(reportDetails);

                // Save the report to the report file
                File.WriteAllText(reportFilePath, report.ToString());

                // Print the report to the console
                Console.WriteLine(report);
            }
        }
        public static void SaveReportToFile(string report, string filePath)
        {
            File.WriteAllText(filePath, report);
            Console.WriteLine($"Report saved to {filePath}");
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


