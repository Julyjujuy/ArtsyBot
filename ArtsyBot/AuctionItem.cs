namespace ArtsyBot
{
    [Serializable]
    public class AuctionItem
    {
        public string ImageUrl { get; set; }
        public string Description { get; set; }
        public string PageUrl { get; set; }
        public string EstimatedPrice { get; set; }
        public string LongDescription { get; set; }
        //TODO: get ALL THE THINGS

        //f.e. when it was scraped timestamp

        //f.e. scrape source
    }
}