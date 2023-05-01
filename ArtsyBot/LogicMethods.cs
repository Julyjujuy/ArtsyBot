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

            // Find the elements to scrape using XPath
            HtmlNodeCollection imageElements = htmlDoc.DocumentNode.SelectNodes("//img[@class='ImageWithHover__PrimaryImageWithFallback-sc-1b860cz-1 jYPLtE no-js-150133299_1_x']");
            HtmlNodeCollection descriptionElements = htmlDoc.DocumentNode.SelectNodes("//div[@class='sc-iAEyYk DescriptionSection__StyledBody-sc-trkwix-2 kiRtmS gtFpYt']");
            HtmlNodeCollection urlElements = htmlDoc.DocumentNode.SelectNodes("//a[@class='no-js-150613881_1_x']");

            // Extract the values of the elements as strings
            if (imageElements != null && descriptionElements != null && urlElements != null &&
                imageElements.Count == descriptionElements.Count && descriptionElements.Count == urlElements.Count)
            {
                for (int i = 0; i < imageElements.Count; i++)
                {
                    string? imageUrl = imageElements[i]?.GetAttributeValue("src", "").Trim();
                    string? description = descriptionElements[i]?.InnerText.Trim();
                    string? linkPageObject = urlElements[i]?.GetAttributeValue("href", "").Trim();

                    // Do something with the scraped data 
                    Console.WriteLine($"URL of image: {imageUrl}");
                    Console.WriteLine($"Description: {description}");
                    Console.WriteLine($"Link to page of the object: {linkPageObject}");
                }
            }
        }
    }
}

