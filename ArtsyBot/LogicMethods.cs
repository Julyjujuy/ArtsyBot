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
       public static void ScrapeWebsite(string url)
        {
            // Load the website's HTML document
            HtmlDocument htmlDoc = GetHtmlDocument(url);

            // Find the elements to scrape using XPath
            HtmlNode imageElement = htmlDoc.DocumentNode.SelectSingleNode("//img[@class='ImageRow__StyledImageWithFallback-sc-1s4yel2-3 jDatHc no-js-150613881_1_x']");
            HtmlNode descriptionElement = htmlDoc.DocumentNode.SelectSingleNode("//div[@class='sc-iAEyYk DescriptionSection__StyledBody-sc-trkwix-2 kiRtmS gtFpYt']");
            HtmlNode urlElement = htmlDoc.DocumentNode.SelectSingleNode("//a[@class='no-js-150613881_1_x']");

            // Extract the values of the elements as strings
            string imageUrl = imageElement?.GetAttributeValue("src", "").Trim();
            string description = descriptionElement?.InnerText.Trim();
            string linkPageObject = urlElement?.GetAttributeValue("href", "").Trim();

            // Do something with the scraped data 
            Console.WriteLine($"URL of image: {imageUrl}");
            Console.WriteLine($"Description: {description}");
            Console.WriteLine($"Link to page of the object: {linkPageObject}");
        }

    }
}