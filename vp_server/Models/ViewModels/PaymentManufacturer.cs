namespace vp_server.Models.ViewModels
{
    public class PaymentManufacturer
    {
        public PaymentDetail? PaymentDetail { get; set; }
    }

    public class ManufacturerM
    {
        public Manufacturer manufacturer { get; set; }
        public List<Manufacturer> manufacturerList { get; set; }
    }



}
