using OfficeOpenXml;
using vp_server.Models;
namespace vp_server.Utils
{
    public class ExcelGenerator
    {
        public byte[] Generate(ExcelDataModel dataModel /*DateOnly dateEnd*/)
        {
            var package = new ExcelPackage();
            var sheet = package.Workbook.Worksheets.Add("Отчет");          
            sheet.Cells["A1"].Value = "Общее число продаж:";
            sheet.Cells["A2"].Value = "Сумма продаж:";
            sheet.Cells["A3"].Value = $"Остаток продукции на ";
            sheet.Cells["A4"].Value = $"Поступление продукции:";

            sheet.Cells["B1"].Value = dataModel.totalSales.ToString();
            sheet.Cells["B2"].Value = dataModel.totalSum.ToString();
            sheet.Cells["B3"].Value = dataModel.productRest.ToString();
            sheet.Cells["B4"].Value = dataModel.productReceipt.ToString();

            sheet.Cells["A7"].Value = "Список оставшейся продукции:";
            sheet.Cells["B7"].Value = "Название";
            sheet.Cells["C7"].Value = "Остаток";
            sheet.Cells["D7"].Value = "Стоимость за штуку";
            int row = 8;//строка
            int col = 2;
            if (dataModel.restProducts != null)
            {
                foreach (var product in dataModel.restProducts)
                {
                    sheet.Cells[row, col].Value = product.productName;
                    sheet.Cells[row, col+1].Value = product.remains.ToString();
                    sheet.Cells[row, col+2].Value = product.cost.ToString();
                    row++;
                }
            }
            else { sheet.Cells[row, 2].Value = "Продукция отсутствует"; }
            row++;
            sheet.Cells[row, 1].Value = "Список поступившей продукции:";
            sheet.Cells[row, 2].Value = "Название";
            sheet.Cells[row, 3].Value = "Стоимость за штуку";
            sheet.Cells[row, 4].Value = "Количество поступления";
            sheet.Cells[row, 5].Value = "Дата поступления";
            row++;
            if (dataModel.receiptProducts != null)
            {
                foreach (var product in dataModel.receiptProducts)
                {
                    sheet.Cells[row, col].Value = product.productName;
                    sheet.Cells[row, col+1].Value = product.cost.ToString();
                    sheet.Cells[row, col+2].Value = product.quantity.ToString();
                    sheet.Cells[row, col+3].Value = product.dateReceipt.ToString();
                    row++;
                }
            }
            else
            {
                sheet.Cells[row, col].Value = "Продукция отсутствует";
            }
            sheet.Cells[1, 1, row, col + 3].AutoFitColumns();
            return package.GetAsByteArray();
        }
    }
}
