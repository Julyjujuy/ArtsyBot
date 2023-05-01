using HtmlAgilityPack;



namespace ArtsyBot
{
    public class LogicMethods
    {
        //takes the url and transform the page into a HtmlDocument
        public static HtmlDocument GetHtmlDocument(string url)
        {
            HtmlWeb web = new HtmlWeb();
            return web.Load(url);
        }
        public static int GetTotalPages(HtmlDocument htmlDoc)
        {
            return htmlDoc.DocumentNode.SelectNodes("//a[starts-with(@href, 'http')]").Count;
        }
        public static void ScrapeWebsite(string url)
        {
            // Load the website's HTML document
            HtmlDocument htmlDoc = GetHtmlDocument(url);

            var articleContainers = htmlDoc.DocumentNode.SelectNodes("//article[contains(@class, 'CategorySearchCard__StyledCard-sc-1o7izf2-0')]");
            if (articleContainers != null)
            {
                foreach (var container in articleContainers)
                {
                    // Get the image URL
                    var imageUrl = container.SelectSingleNode(".//img")?.GetAttributeValue("src", "");

                    // Get the description
                    var description = container.SelectSingleNode(".//img")?.GetAttributeValue("alt", "");

                    // Get the page URL
                    var pageUrl = container.SelectSingleNode(".//a[@class='sc-hsiEis cgsAkx ImageRow__ContainerLink-sc-1s4yel2-0 dlJysj CategorySearchCard__PlacedItemCardImage-sc-1o7izf2-3 iSZMAW']")?.GetAttributeValue("href", "");

                    //var pageUrl = container.SelectSingleNode(".//a[@class='h4 item-title']")?.GetAttributeValue("href", "");

                    // Output the results
                    Console.WriteLine($"Image URL: {imageUrl}");
                    Console.WriteLine($"Description: {description}");
                    Console.WriteLine($"Page URL: {pageUrl}");
                    Console.WriteLine();
                }
            }
            else
            {
                Console.WriteLine("No article containers found.");
            }
        }

    }
}

