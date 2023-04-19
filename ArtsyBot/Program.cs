using HtmlAgilityPack;
using System;

namespace ArtsyBot
{
    internal class Program
    {
        private const string Path = @"C:\sample.txt";

        public static void Main()
        {
            string baseUrl = "https://www.liveauctioneers.com/c/clocks/74/";
            string Path = "C:\\temp\\htmlfile.html";

            // Load the first page of the website
            HtmlDocument htmlDoc = LogicMethods.GetHtmlDocument(baseUrl);
            htmlDoc.Save(Path);

            // Find the total number of pages on the website
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

            // Scrape the data from each page on the website
            for (int i = 2; i <= totalPages; i++)
            {
                // Load the current page of the website
                string currentUrl = $"{baseUrl}?page={i}";
                htmlDoc = LogicMethods.GetHtmlDocument(currentUrl);
                htmlDoc.Save(Path);

                // Find the elements to scrape using XPath
                HtmlNodeCollection imageElements = htmlDoc.DocumentNode.SelectNodes("//img[@class='ImageRow__StyledImageWithFallback-sc-1s4yel2-3 jDatHc no-js-150613881_1_x']");
                HtmlNodeCollection DescriptionElements = htmlDoc.DocumentNode.SelectNodes("//div[@class='sc-iAEyYk DescriptionSection__StyledBody-sc-trkwix-2 kiRtmS gtFpYt']");
                HtmlNodeCollection urlElements = htmlDoc.DocumentNode.SelectNodes("//a[@class='no-js-150613881_1_x']");

                // Extract the values of the elements as strings
                foreach (HtmlNode imageElement in imageElements)
                {
                    string imageUrl = imageElement?.GetAttributeValue("src", "").Trim();
                    Console.WriteLine($"URL of image: {imageUrl}");
                }

                foreach (HtmlNode DescriptionElement in DescriptionElements)
                {
                    string description = DescriptionElement?.InnerText.Trim();
                    Console.WriteLine($"Value of Description: {description}");
                }

                foreach (HtmlNode urlElement in urlElements)
                {
                    string linkPageObject = urlElement?.GetAttributeValue("href", "").Trim();
                    Console.WriteLine($"Value of URL: {linkPageObject}");
                }
            }
        }

    }
}