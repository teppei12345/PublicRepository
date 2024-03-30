using Excel = Microsoft.Office.Interop.Excel;


namespace ExportExcelFromJson
{
    internal class Model
    {
        public void Manipulate (string File, string status, int line, int colum)
        {
            Excel.Application excelApp = new Excel.Application();
            Excel.Workbooks excelBooks = excelApp.Workbooks;
            Excel.Workbook excelBook = excelBooks.Add();
            Excel.Worksheet sheet = (Excel.Worksheet)excelApp.Worksheets["sheet1"];
            try
            {
                Console.WriteLine("Excel操作処理：開始");
                excelApp.Visible = false;
                excelApp.DisplayAlerts = false;
                sheet.Cells[1, 1] = status;
                excelBook.SaveAs(File);
                Console.WriteLine(File);
            }
            catch (Exception e)
            {
                Console.WriteLine("例外が発生しました。");
                Console.WriteLine(e);
            }
            finally
            {
                Console.WriteLine("Excel操作処理：終了");
                excelApp.Quit();
                System.Runtime.InteropServices.Marshal.ReleaseComObject(sheet);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(excelBook);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);
                Console.WriteLine("Excelアクセス処理：終了");
            }
        }

        public void WriteToConsole (string message)
        {
            Console.WriteLine(message);
        }
    }
}
