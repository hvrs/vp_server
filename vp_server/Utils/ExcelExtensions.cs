using OfficeOpenXml;
using System.Reflection;
using vp_server.Models;
namespace vp_server.Utils
{
    public class ExcelGenerate
    {
        private string[] columnTrueNames = { "Название", "Стоимость", "Материал", "Вкус", "Категория", "Содержание никотина", "Крепкость продукции", "Производитель", "Количество на складе" };
        public byte[] Generate(ExcelDataModel dataModel /*DateOnly dateEnd*/)//Создание отчета
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
        public bool columnValidate(ExcelWorksheet worksheet)//Проверка соответствия заголовков
        {
            try
            {
                int allColumns = worksheet.Dimension.End.Column;
                for (int column = 1; column <= allColumns; column++)
                {
                    if (worksheet.Cells[1, column].Value.ToString() != columnTrueNames[column - 1])
                    {
                        return false;
                    }
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
      

    } 

    public static class ExcelExtension
    {
        public static IEnumerable<T> FromSheetToModel<T>(this ExcelWorksheet worksheet) where T : new()
        {
            Func<CustomAttributeData, bool> columnOnly = y => y.AttributeType == typeof(Column);
            var columns = typeof(T)
                .GetProperties()
                .Where(x => x.CustomAttributes.Any(columnOnly))
            .Select(p => new
            {
                Property = p,
                Column = p.GetCustomAttributes<Column>().First().ColumnIndex
            }).ToList();

            var rows = worksheet.Cells
                .Select(cell => cell.Start.Row)
                .Distinct()
                .OrderBy(x => x);

            var collection = rows.Skip(1)
                .Select(row =>
                {
                    var tnew = new T();
                    columns.ForEach(col =>
                    {
                        var val = worksheet.Cells[row, col.Column];
                        if (val.Value == null)
                        {
                            col.Property.SetValue(tnew, null);
                            return;
                        }
                        if (col.Property.PropertyType == typeof(Int32))
                        {
                            col.Property.SetValue(tnew, val.GetValue<int>());
                            return;
                        }
                        if (col.Property.PropertyType == typeof(float))
                        {
                            col.Property.SetValue(tnew, val.GetValue<float>());
                            return;
                        }
                        if (col.Property.PropertyType == typeof(DateTime))
                        {
                            col.Property.SetValue(tnew, val.GetValue<DateTime>());
                            return;
                        }
                        col.Property.SetValue(tnew, val.GetValue<string>());
                    });
                    return tnew;
                });
            return collection;
        }
    }
   
}
