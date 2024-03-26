namespace vp_server.Models
{
    public class ExcelDataModel
    {
        public int totalSales {  get; set; }//Общее число продаж
        public float totalSum { get; set; }//Сумма продаж
        public int? productRest { get; set; }//Остаток продукции
        public IEnumerable<restProduct>? restProducts { get; set; }//Список оставшейся продукции
        public int? productReceipt { get; set; }//Поступившая за выбранный период продукция
        public IEnumerable<receiptProduct> receiptProducts { get; set; }//список поступившей продукции 
    }
    public class restProduct
    {
        public string productName { get; set; } = "";
        public int? remains { get; set; }//остаток
        public float cost { get; set; }
    }
    public class receiptProduct
    {
        public string productName { get; set; } = "";
        public float cost { get; set; }//Стоимость за штуку
        public int quantity { get; set; }//количество поступившей продукции
        public DateOnly dateReceipt { get; set; }//дата поступления

    }
    
}
