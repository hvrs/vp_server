namespace vp_server.Models
{
    public class TransactionProductDTO//Для передачи данных в представление с информацией о продуктах, входящих в транзакицю
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public float Cost { get; set; }
        public int Quality { get; set; }
    }
}
