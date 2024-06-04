using OfficeOpenXml;
using System.Reflection;
using OfficeOpenXml.Style;
using vp_server.Models;
namespace vp_server.Utils
{
    public class ExcelGenerate
    {
        private string[] columnTrueNames = { "Название", "Стоимость", "Материал", "Вкус", "Категория", "Содержание никотина", "Крепкость продукции", "Производитель", "Количество на складе" };
        private string[] columnTrueNamesR = { "Название продукта", "Количество", "Дата пополнения"};
        private Dictionary<int, string> mounths = new Dictionary<int, string>()
        {
            {1,"Января" },
            {2,"Февраля" },
            {3,"Марта" },
            {4,"Апреля" },
            {5,"Мая" },
            {6,"Июня" },
            {7,"Июля" },
            {8,"Августа" },
            {9,"Сентября" },
            {10,"Октября" },
            {11,"Ноября" },
            {12,"Декабря" },
        };
        public byte[] Generate(ExcelDataModel dataModel /*DateOnly dateEnd*/)//Создание отчета
        {
            var package = new ExcelPackage();
            var sheet = package.Workbook.Worksheets.Add("Отчет");          
            sheet.Cells["A1"].Value = "Общее число продаж:";
            sheet.Cells["A2"].Value = "Сумма продаж:";
            sheet.Cells["A3"].Value = $"Остаток продукции на конец промежутка:";
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

        public byte[] GenerateReceipt(ReceiptDataModel DataModel)
        {
            var package = new ExcelPackage();
            var sheet = package.Workbook.Worksheets.Add("Товарный чек");
            //1 строка
            sheet.Cells["C1:E1"].Merge = true;
            sheet.Cells["C1"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            sheet.Cells["C1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            sheet.Cells["C1:E1"].Value = $"Товарный чек №{DataModel.transaction.Id}";
            //2 строка
            sheet.Cells["A2"].Value = $"Организация:";
            sheet.Cells["A2"].AutoFitColumns();
            sheet.Cells["B2"].Value = DataModel.NameCompany;//Название организации
            sheet.Cells["H2"].Value = "От"; 
            sheet.Cells["I2"].Value = $"{DataModel.transaction.Date.Day}"; 
            sheet.Cells["J2"].Value = $"{mounths[DataModel.transaction.Date.Month]}"; 
            sheet.Cells["K2"].Value = $"{DataModel.transaction.Date.Year} года";
            sheet.Cells["I2:K2"].AutoFitColumns();
            sheet.Cells["H2"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
            sheet.Cells["I2:K2"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            //4 строка
            sheet.Cells["A4:D4"].Merge = true;
            sheet.Cells["A4"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            sheet.Cells["A4"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            sheet.Cells["A4:D4"].Value = "Наименование товара";
            sheet.Cells["E4"].Value = "Артикул";//ID товара в системе
            sheet.Cells["F4"].Value = "Кол-во";
            sheet.Cells["G4"].Value = "Цена";
            sheet.Cells["H4"].Value = "Сумма";
            sheet.Cells["A4:H4"].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            sheet.Cells["A4:H4"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            sheet.Cells["A4:H4"].Style.Border.Right.Style = ExcelBorderStyle.Thin;
            //Некоторый цикл
            int row = 5; //Строка
            int col = 1; //Столбек
            if (DataModel.productsInTransaction != null)
            {
                foreach (var item in DataModel.productsInTransaction)
                {
                    sheet.Cells[row, col, row, 4].Merge = true;
                    sheet.Cells[row, col, row, 4].Value = item.Name;
                    sheet.Cells[row, 5].Value = item.Id;
                    sheet.Cells[row, 6].Value = item.Quality;
                    sheet.Cells[row, 7].Value = item.Cost;
                    sheet.Cells[row, 8].Value = item.Cost * item.Quality;
                    row++;
                }
                sheet.Cells[5, 1, row-1, 8].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                sheet.Cells[5, 1, row-1, 8].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                sheet.Cells[5, 1, row-1, 8].Style.Border.Right.Style = ExcelBorderStyle.Thin;
            }
            //Конец некоторого цикла           

            //Заключительная строка
            sheet.Cells[row, 1].Value = "Итого:";
            sheet.Cells[row, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
            sheet.Cells[row, 2].Value =$"{DataModel.transaction.Sum} руб.";
            sheet.Cells[row, 2].AutoFitColumns();
            sheet.Cells[row, 1, row, 2].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            sheet.Cells[row, 1, row, 2].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            sheet.Cells[row, 1, row, 2].Style.Border.Right.Style = ExcelBorderStyle.Thin;

            sheet.Cells[row, 4].Value = "Подпись:";           
            sheet.Cells[row, 5, row, 8].Merge = true;
            sheet.Cells[row, 5, row, 8].Value = "____________________________________";

            sheet.Protection.IsProtected = true;
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
        public bool columnValidateR(ExcelWorksheet worksheet)//Проверка соответствия заголовков
        {
            try
            {
                int allColumns = worksheet.Dimension.End.Column;
                for (int column = 1; column <= allColumns; column++)
                {
                    if (worksheet.Cells[1, column].Value.ToString() != columnTrueNamesR[column - 1])
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
