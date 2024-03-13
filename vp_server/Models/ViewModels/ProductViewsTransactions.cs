namespace vp_server.Models.ViewModels
{
    public class ProductViewsTransactions
    {
        public Product Product { get; set; }
        public int? Count { get; set; }    
    }
    public class ProductViews
    {
        public DateTime datetime { get; set; }
        public int count { get; set; } = 0;
    }

}