using vp_server.Models;
namespace vp_server.Models.ViewModels
{
    public class ProductViewsTransactions
    {
        public Product Product { get; set; }
        public IEnumerable<ProductViews> Views { get; set; }
        public IEnumerable<ProductTransaction> productTransactions { get; set; }
    }
    public class ProductTransaction
    {
        public int Id { get; set; }
        public int Quantitly {  get; set; }
        public DateTime date {  get; set; }
    }
    public class ProductViews
    {
        public int? Id { get; set; }
        public DateOnly? date { get; set; }
        public TimeOnly? time { get; set; }
    }
}