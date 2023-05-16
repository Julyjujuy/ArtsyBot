using HtmlAgilityPack;
using System.Net;

namespace ArtsyBot
{
    public class LogicMethods
    {
        public static void ScrapeWebsite(string url)
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

            // Find all the article containers on the page
            var articleContainers = htmlDoc.DocumentNode.SelectNodes("//article[contains(@class, 'CategorySearchCard__StyledCard-sc-1o7izf2-0')]");
            if (articleContainers != null)
            {
                int counter = 0; // Initialize the counter variable
                foreach (var container in articleContainers)
                {
                    // Get the image URL
                    var imageUrl = container.SelectSingleNode(".//img[1]")?.GetAttributeValue("src", "");

                    // Get the description
                    var description = container.SelectSingleNode(".//img[1]")?.GetAttributeValue("alt", "");

                    // Get the page URL
                    var pageUrlElement = container.SelectSingleNode(".//a[@class='sc-hsiEis cgsAkx ImageRow__ContainerLink-sc-1s4yel2-0 dlJysj CategorySearchCard__PlacedItemCardImage-sc-1o7izf2-3 iSZMAW']");
                    var pageUrl = pageUrlElement?.GetAttributeValue("href", "");

                    // Concatenate the prefix to the page URL
                    pageUrl = "https://www.liveauctioneers.com" + pageUrl;

                    // Navigate to the page URL and load the inner HTML document
                    HtmlDocument innerHtmlDoc = web.Load(pageUrl);
                    //todo: check if HtmlWeb saves coockies => if yes try to delete after request if no: try to save cookies between request
           
                    // Extract the Estimate text
                    var estimateNode = innerHtmlDoc.DocumentNode.SelectSingleNode("//div[@class='ItemBiddingEstimate__StyledEstimateAmounts-sc-e0x4f0-4 eRGEFe']");
                    var estimatedPrice = estimateNode?.InnerText;

                    // Extract the description section
                    var descriptionSectionNode = innerHtmlDoc.DocumentNode.SelectSingleNode("//span[@data-testid='body-primary' and contains(@class, 'sc-bjMMwb iLlJHW DescriptionSection__StyledBody-sc-trkwix-2 gtFpYt')]");
                    var descriptionSection = descriptionSectionNode?.InnerText;

                    // Extract the image URL from AuctioneerInfo
                    var auctioneerImageNode = innerHtmlDoc.DocumentNode.SelectSingleNode("//img[contains(@class, 'AuctioneerInfo__StyledImageWithFallback-sc-1erc2m8-2 eoKPJL no-js-jasper52_large')]");
                    var auctioneerImageUrl = auctioneerImageNode?.GetAttributeValue("src", "");

                    // Increment the counter
                    counter++;
                    Thread.Sleep(60*1000);

                    // Output the results
                    Console.WriteLine($"Image URL: {imageUrl}");
                    Console.WriteLine($"Description: {description}");
                    Console.WriteLine($"Page URL: {pageUrl}");
                    Console.WriteLine($"Estimate: {estimatedPrice}");
                    Console.WriteLine($"Description Section: {descriptionSection}");
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
        }

    }
}


