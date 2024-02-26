using System;
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
                return false;
            }

            // Create a list to hold the scraped items
            List<AuctionItem> auctionItems = new List<AuctionItem>();

            // Find all the section elements representing item containers
            var itemContainers = htmlDoc.DocumentNode.SelectNodes("//section[@class='CategorySearchCard__CategorySearchCardGrid-sc-1o7izf2-2 gxjpRt']");

            if (itemContainers != null)
            {
                int counter = 0; // Initialize the counter variable

                foreach (var container in itemContainers)
                {
                    Thread.Sleep(delaySeconds * 1000);



                    // Get the image URL
                    var imageUrl = container.SelectSingleNode("//section[@class=\"ImageWithHover__SecondaryImageContainer-sc-1b860cz-2 iQeHhf\"]//a/img/@src")?.ToString();

                    // Get the description
                    Thread.Sleep(5000);
                    var description = container.SelectSingleNode("//span[@class='sc-hKFymg iDetkt']")?.InnerText;



                    // Get the page URL
                    var pageUrlElement = container.SelectSingleNode(".//a[@class='sc-dPyGX fbA-dDA ImageRow__ContainerLink-sc-1s4yel2-0 bkYgH CardImage__ItemCardImageGrid-sc-1fhtv7y-1 dNYGTn']");
                    var pageUrl = pageUrlElement?.GetAttributeValue("href", "");

                    // Concatenate the prefix to the page URL
                    pageUrl = "https://www.liveauctioneers.com" + pageUrl;

                    // Get the estimated price (working)
                    var estimatedPriceNode = container.SelectSingleNode(".//span [@class=\"FormattedCurrency__StyledFormattedCurrency-sc-1ugrxi1-0 cZCaob\"]"); 
                    var estimatedPrice = estimatedPriceNode?.InnerText;

               
                    // Get the auctioneer name (working)                           
                    var auctioneerNameNode = container.SelectSingleNode(".//section[@class='CardInfo__CardInfoSection-sc-1bvw270-0 jHQfzK']/a[@class='sc-kEqXeH fgHGD CardInfo__StyledAuctionHouseName-sc-1bvw270-2 kGlvMC']/span[@data-testid='body-secondary' and @class='sc-hKFymg AOFIB']");
                    var auctioneerName = auctioneerNameNode?.InnerText;

                    // Get the auction time left
                    var auctionTimeLeftNode = container.SelectSingleNode("//span[@data-testid=\"body-secondary\" and @class=\"sc-hKFymg hQgMeF\"]//span[@data-testid=\"date-span\" and @class=\"sc-hKFymg hQgMeF\"]//span[@data-testid=\"body-secondary\" and @class=\"sc-hKFymg fmGzJe CatalogDate__CountdownSpan-sc-132deab-0 bxNvnk\"]");
                    var auctionTimeLeft = auctionTimeLeftNode?.InnerText;

                    //// Extract the starting price
                    var priceNode = htmlDoc.DocumentNode.SelectSingleNode("//h1[@data-testid='h1']/span[@class='FormattedCurrency__StyledFormattedCurrency-sc-1ugrxi1-0 kRoxAz']");
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
                        //           LongDescription = spanText,
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
                    //Console.WriteLine($"Description Section: {spanText}");
                    Console.WriteLine($"AuctioneerName: {auctioneerName}");
                    //         Console.WriteLine($"Time left before Auction: {auctionTimeLeft}");
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
            //Serialize(auctionItems, filePath);
            SerializeToCsv(auctionItems, filePath);


            // Save the XML to a file
            //Deserialize(filePath);
            DeserializeFromCsv(filePath);
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


        public static string EscapeCsvValue(string value)
        {
            if (value.Contains(",") || value.Contains("\n") || value.Contains("\r") || value.Contains("\"") || value.Contains("'"))
            {
                return "\"" + value.Replace("\"", "\"\"") + "\""; // Escape quotes with double quotes
            }
            return value;
        }

        public static void SetPropertyValue(object obj, string propertyName, object value)
        {
            var property = obj.GetType().GetProperty(propertyName);
            if (property != null)
            {
                // Attempt type conversion if necessary
                if (property.PropertyType != value.GetType())
                {
                    value = Convert.ChangeType(value, property.PropertyType);
                }
                property.SetValue(obj, value);
            }
        }





        public static void SerializeToCsv(List<AuctionItem> auctionItems, string path)
        {
            using (StreamWriter writer = new StreamWriter(path))
            {
                // Write header row
                var properties = typeof(AuctionItem).GetProperties();
                writer.WriteLine(string.Join(",", properties.Select(p => p.Name)));

                // Write item data
                foreach (var item in auctionItems)
                {
                    var values = properties.Select(p => EscapeCsvValue(p.GetValue(item)?.ToString()));
                    writer.WriteLine(string.Join(",", values));
                }
            }
        }



        public static List<AuctionItem> DeserializeFromCsv(string path)
        {
            List<AuctionItem> auctionItems = new List<AuctionItem>();
            using (StreamReader reader = new StreamReader(path))
            {
                // Read header row
                var properties = reader.ReadLine().Split(',');

                // Read item data
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    var values = line.Split(',');
                    var item = new AuctionItem();
                    for (int i = 0; i < properties.Length; i++)
                    {
                        SetPropertyValue(item, properties[i], values[i]);
                    }
                    auctionItems.Add(item);
                }
            }
            return auctionItems;
        }




    }
}


