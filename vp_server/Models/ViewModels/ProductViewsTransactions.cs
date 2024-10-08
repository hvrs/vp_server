﻿namespace vp_server.Models.ViewModels
{
    public class ProductViewsTransactions
    {
        public Product Product { get; set; }
        public int? Count { get; set; }    
    }
    public class ProductViews
    {
        public string datetime { get; set; }
        public int countViews { get; set; } = 0;
        public int countBuyProduct { get; set; } = 0;
    }

}