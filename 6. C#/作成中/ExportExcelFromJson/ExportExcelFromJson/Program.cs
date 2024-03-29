using Excel = Microsoft.Office.Interop.Excel;

namespace ExportExcelFromJson
{
    class Program
    {
        static void Main(string[] args)
        {
            string jsonData = "";
            string inputFile = "C:\\Implem\\個人フォルダ_VSCode用\\1. サンプルコード\\7. C#\\作成中\\ExportExcelFromJson\\ExportExcelFromJson\\sample.json";
            string outputFile = "C:\\Implem\\個人フォルダ_VSCode用\\1. サンプルコード\\7. C#\\作成中\\ExportExcelFromJson\\ExportExcelFromJson\\sample.xlsx";
            try
            {
                Console.WriteLine("jsonファイルの読み取り：開始");
                Console.WriteLine("jsonファイルパス：" + inputFile);
                jsonData = File.ReadAllText(inputFile);
                Console.WriteLine("jsonファイルの読み取り：終了");
                Console.WriteLine("jsonのファイル出力：開始");
                Console.WriteLine("出力内容：");
                Console.WriteLine(jsonData);
                Console.WriteLine("出力ファイルパス：" + outputFile);
            }
            catch (Exception e)
            {
                Console.WriteLine("例外が発生しました。");
                Console.WriteLine(e);
            }
            Excel.Application excelApp = new Excel.Application();
            Excel.Workbooks excelBooks = excelApp.Workbooks;
            Excel.Workbook excelBook = excelBooks.Add();
            Excel.Worksheet sheet = (Excel.Worksheet)excelApp.Worksheets["sheet1"];
            try
            {
                excelApp.Visible = false;
                sheet.Cells[1, 1] = jsonData;
                excelBook.SaveAs(outputFile);
                Console.WriteLine(outputFile);
            }
            catch (Exception e)
            {
                Console.WriteLine("例外が発生しました。");
                Console.WriteLine(e);
            }
            finally
            {
                excelApp.Quit();
                System.Runtime.InteropServices.Marshal.ReleaseComObject(sheet);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(excelBook);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);
            }
        }
    }
}
