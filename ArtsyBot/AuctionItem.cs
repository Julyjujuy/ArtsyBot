using System.ComponentModel.DataAnnotations;

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
        public string AuctioneerName { get; set; }

        public string AuctionTimeLeft { get; set; }

        public string StartingPrice { get; set; }
        public string EstimatedPriceFailSafe { get; set; }

        //timestamp
        //
    }
}