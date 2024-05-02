
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
}
