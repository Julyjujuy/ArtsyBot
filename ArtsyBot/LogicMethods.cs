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

        // Find the total number of pages on the website
        public static int GetTotalPages(HtmlDocument htmlDoc)
        {
            int totalPages = 0;
            HtmlNode pageNode = htmlDoc.DocumentNode.SelectSingleNode("//a[@title='Last Page']");
            if (pageNode != null)
            {
                string hrefValue = pageNode.GetAttributeValue("href", "");
                if (!string.IsNullOrEmpty(hrefValue))
                {
                    int startIndex = hrefValue.LastIndexOf("page=") + "page=".Length;
                    int endIndex = hrefValue.LastIndexOf("&", StringComparison.InvariantCulture);
                    if (startIndex >= 0 && endIndex >= 0 && endIndex > startIndex)
                    {
                        int.TryParse(hrefValue.Substring(startIndex, endIndex - startIndex), out totalPages);
                    }
                }
            }

            return totalPages;
        }
        public static void ScrapeWebsite(string url)
        {
            // Load the website's HTML document
            HtmlDocument htmlDoc = GetHtmlDocument(url);

            var imageContainers = htmlDoc.DocumentNode.SelectNodes("//div[contains(@class, 'ImageWithHover')]");
            if (imageContainers != null)
            {
                foreach (var container in imageContainers)
                {
                    // Get the image URL
                    var imageUrl = container.SelectSingleNode(".//img")?.GetAttributeValue("src", "");

                    // Get the description
                    var description = container.SelectSingleNode(".//img")?.GetAttributeValue("alt", "");

                    // Get the page URL
                    var pageUrl = container.ParentNode.GetAttributeValue("href", "");

                    // Output the results
                    Console.WriteLine($"Image URL: {imageUrl}");
                    Console.WriteLine($"Description: {description}");
                    Console.WriteLine($"Page URL: {pageUrl}");
                    Console.WriteLine();
                }
            }
            else
            {
                Console.WriteLine("No image containers found.");
            }

        }

    }
}

