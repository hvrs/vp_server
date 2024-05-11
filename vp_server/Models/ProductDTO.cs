
namespace vp_server.Models
{
    public class ProductDTO
    {
        public int Id { get; set; }
        public string NameProduct { get; set; } = "";
        public byte[]? Photo { get; set; }
        public string Category { get; set; } = "";
        public string Manufacturer { get; set; } = "";
        public string? Nicotine { get; set; }
        public string? Strength { get; set; }
        public double Cost { get; set; }
    }
    public class ProductExcelDTO
    {
        [Column(1)]
        public string Title { get; set; } = "";
        [Column(2)]
        public double Cost { get; set; }
        [Column(3)]
        public string Material { get; set; } = "";
        [Column(4)]
        public string? Taste { get; set; }
        [Column(5)]
        public string Category { get; set; } = "";
        [Column(6)]
        public string? Nicotine { get; set; }
        [Column(7)]
        public string? Strength { get; set; }
        [Column(8)]
        public string Manufacturer { get; set; } = "";
        [Column(9)]
        public int Count { get; set; } = 0;
    }

    [AttributeUsage(AttributeTargets.All)]
    public class Column : System.Attribute
    {
        public int ColumnIndex { get; set; }
        public Column(int column)
        {
            ColumnIndex = column;
        }
    }

    public class ProductToB
    {
        public int ProductId { get; set; }
    }

    public class DTOProductAndQuantity
    {
        public ProductDTO? product { get; set; }
        public int? quantityInBusket { get; set; }
    }

    public class idProductsInBasketAndSum
    {
        public List<ProductAndQuantity> productQ { get; set;}
        public double Sum { get; set; }
        public idProductsInBasketAndSum()
        {
            productQ = new List<ProductAndQuantity>();
        }
    }

    public class ProductAndQuantity
    {
        public int ProductID { get; set; }
        public int Quantity { get; set;}
    }
   
}
