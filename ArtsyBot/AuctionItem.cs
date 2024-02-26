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


        //timestamp
    }
}



//if (auctioneerNameNode == null)
//{

//    Thread.Sleep(60 * 1000);
//    innerHtmlDoc = web.Load(pageUrl);
//    auctioneerNameNode = innerHtmlDoc.DocumentNode.SelectSingleNode("//span[@data-testid='itemPageSellerName' and @class='sc-hLseeU AuctioneerInfo__SellerNameText-sc-1erc2m8-5 jnbWxy chuhQu']");
//    if (auctioneerNameNode == null)
//        Debugger.Break();
//    Thread.Sleep(30 * 1000);
//    innerHtmlDoc = web.Load(pageUrl);
//    auctioneerNameNode = innerHtmlDoc.DocumentNode.SelectSingleNode("//span[@data-testid='itemPageSellerName' and @class='sc-hLseeU AuctioneerInfo__SellerNameText-sc-1erc2m8-5 jnbWxy chuhQu']");
//    if (auctioneerNameNode == null)
//        Debugger.Break();
//    Thread.Sleep(60 * 1000);
//    innerHtmlDoc = web.Load(pageUrl);
//    auctioneerNameNode = innerHtmlDoc.DocumentNode.SelectSingleNode("//span[@data-testid='itemPageSellerName' and @class='sc-hLseeU AuctioneerInfo__SellerNameText-sc-1erc2m8-5 jnbWxy chuhQu']");
//    if (auctioneerNameNode == null)
//        Debugger.Break();
//}

//      var auctioneerName = auctioneerNameNode?.InnerText;

// Extract the time left before the Auction
//var dateNode = innerHtmlDoc.DocumentNode.SelectSingleNode("//div[contains(@class, 'lt1g861EjAKl0fG1gZvV') and contains(@class, 'BiddingCountdown__StyledCountdown-sc-et36hu-0')]");
//var auctionTimeLeft = dateNode?.InnerText;




//while (priceNode == null)
//{
//    Thread.Sleep(60 * 1000);
//    innerHtmlDoc = web.Load(pageUrl);
//    priceNode = innerHtmlDoc.DocumentNode.SelectSingleNode("//h1[@data-testid='h1']/span[@class='FormattedCurrency__StyledFormattedCurrency-sc-1ugrxi1-0 kRoxAz']");

//    if (priceNode == null)
//        Debugger.Break();
//}


//if (priceNode == null)
//{

//    Thread.Sleep(60 * 1000);
//    innerHtmlDoc = web.Load(pageUrl);
//    priceNode = innerHtmlDoc.DocumentNode.SelectSingleNode("//h1[@data-testid='h1']/span[@class='FormattedCurrency__StyledFormattedCurrency-sc-1ugrxi1-0 kRoxAz']");
//    if (priceNode == null)
//        Debugger.Break();

//}




//if (auctioneerNameNode == null)
//{

//    Thread.Sleep(60 * 1000);
//    innerHtmlDoc = web.Load(pageUrl);
//    auctioneerNameNode = innerHtmlDoc.DocumentNode.SelectSingleNode("//span[@data-testid='itemPageSellerName' and @class='sc-hLseeU AuctioneerInfo__SellerNameText-sc-1erc2m8-5 jnbWxy chuhQu']");
//    if (auctioneerNameNode == null)
//        Debugger.Break();
//    Thread.Sleep(30 * 1000);
//    innerHtmlDoc = web.Load(pageUrl);
//    auctioneerNameNode = innerHtmlDoc.DocumentNode.SelectSingleNode("//span[@data-testid='itemPageSellerName' and @class='sc-hLseeU AuctioneerInfo__SellerNameText-sc-1erc2m8-5 jnbWxy chuhQu']");
//    if (auctioneerNameNode == null)
//        Debugger.Break();
//    Thread.Sleep(60 * 1000);
//    innerHtmlDoc = web.Load(pageUrl);
//    auctioneerNameNode = innerHtmlDoc.DocumentNode.SelectSingleNode("//span[@data-testid='itemPageSellerName' and @class='sc-hLseeU AuctioneerInfo__SellerNameText-sc-1erc2m8-5 jnbWxy chuhQu']");
//    if (auctioneerNameNode == null)
//        Debugger.Break();
//}

//      var auctioneerName = auctioneerNameNode?.InnerText;

// Extract the time left before the Auction
//var dateNode = innerHtmlDoc.DocumentNode.SelectSingleNode("//div[contains(@class, 'lt1g861EjAKl0fG1gZvV') and contains(@class, 'BiddingCountdown__StyledCountdown-sc-et36hu-0')]");
//var auctionTimeLeft = dateNode?.InnerText;




//while (priceNode == null)
//{
//    Thread.Sleep(60 * 1000);
//    innerHtmlDoc = web.Load(pageUrl);
//    priceNode = innerHtmlDoc.DocumentNode.SelectSingleNode("//h1[@data-testid='h1']/span[@class='FormattedCurrency__StyledFormattedCurrency-sc-1ugrxi1-0 kRoxAz']");

//    if (priceNode == null)
//        Debugger.Break();
//}


//if (priceNode == null)
//{

//    Thread.Sleep(60 * 1000);
//    innerHtmlDoc = web.Load(pageUrl);
//    priceNode = innerHtmlDoc.DocumentNode.SelectSingleNode("//h1[@data-testid='h1']/span[@class='FormattedCurrency__StyledFormattedCurrency-sc-1ugrxi1-0 kRoxAz']");
//    if (priceNode == null)
//        Debugger.Break();

//}
