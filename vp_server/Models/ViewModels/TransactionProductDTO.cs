namespace vp_server.Models.ViewModels
{
    public class TransactionProductDTO//Для передачи данных в представление с информацией о продуктах, входящих в транзакицю
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public double Cost { get; set; }
        public int Quality { get; set; }
    }
}
