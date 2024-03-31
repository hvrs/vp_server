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
    }
    public class ProductExcelDTO
    {
        public string Title { get; set; } = "";
        public double? Cost { get; set; }
        public string? Material { get; set; }
        public string? Taste { get; set; }
        public string Category { get; set; } = "";
        public string Manufacturer { get; set; } = "";
        public string? Nicotine { get; set; }
        public string? Strength { get; set; }
        public int count { get; set; } = 0;
    }
}
