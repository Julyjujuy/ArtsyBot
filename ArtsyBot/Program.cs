

namespace ArtsyBot
{
    internal class Program
    {
        public static void Main()
        {
            string baseUrl = "https://www.liveauctioneers.com/c/clocks/74/";

            // Create a list to hold the scraped items
            List<AuctionItem> auctionItems = new List<AuctionItem>();

            // Variables to keep track of successful and failed extractions
            int successfulExtractions = 0;
            int failedExtractions = 0;

            // Loop from 2 to 100, constructing and scraping each page
            for (int i = 2; i <= 100; i++)
            {
                // Construct the new URL for this page
                string restBaseUrl = $"{baseUrl}?page={i}";

                // Scrape data from this page
                bool success = LogicMethods.ScrapeWebsite(restBaseUrl, auctionItems);

                if (success)
                {
                    successfulExtractions++;
                    LogicMethods.Logger.Log($"Successfully scraped page {i}");
                }
                else
                {
                    failedExtractions++;
                    LogicMethods.Logger.Log($"Failed to scrape page {i}");
                }

                // Pause function with a random time between 1000 and 5000
                Thread.Sleep(new Random().Next(1000, 5000));
            }

            // Scrape data from the first page
            bool firstPageSuccess = LogicMethods.ScrapeWebsite(baseUrl, auctionItems);

            if (firstPageSuccess)
            {
                successfulExtractions++;
                Console.WriteLine("Successfully scraped the first page");
            }
            else
            {
                failedExtractions++;
                Console.WriteLine("Failed to scrape the first page");
            }

            // Generate the report
            string report = $"Scraping Summary:\n\nSuccessful Extractions: {successfulExtractions}\nFailed Extractions: {failedExtractions}";

            // Save the report to a file
            string reportFilePath = @"C:\Temp\scraping_report.txt";
            LogicMethods.SaveReportToFile(report, reportFilePath);

            Console.WriteLine("Done scraping!");

            Console.ReadLine();
        }
    }
}