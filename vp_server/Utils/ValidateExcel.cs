using OfficeOpenXml;
namespace vp_server.Utils
{
    public class ValidateExcel
    {
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
        private string[] columnTrueNames = { "Название", "Стоимость", "Материал", "Вкус", "Категория", "Содержание никотина", "Крепкость продукции", "Производитель", "Количество на складе" };   
    }
}
