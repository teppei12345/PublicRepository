using Excel = Microsoft.Office.Interop.Excel;
using System;
using System.IO;
using System.Text;


namespace ExportExcelFromJson
{
    internal class Model
    {
        public void ManipulateExcel (string ExportFile, string status, int line, int column, string logFile, string logType, int statusNum)
        {
            Console.WriteLine(column);
            WriteToLogFile(logFile, "Excelアクセス処理：開始", logType);
            Excel.Application excelApp = new Excel.Application();
            Excel.Workbooks excelBooks = excelApp.Workbooks;
            Excel.Workbook excelBook = excelBooks.Add();
            Excel.Worksheet sheet = (Excel.Worksheet)excelApp.Worksheets["sheet1"];
            try
            {
            WriteToLogFile(logFile, "Excel操作処理：開始", logType);
                if (File.Exists(ExportFile))
                {
                    WriteToLogFile(logFile, "既存ブックが存在する場合", logType);
                    excelApp.Visible = false;
                    excelApp.DisplayAlerts = false;
                    sheet.Cells[line, column] = status;
                    excelBook.Save();
                }
                else
                {
                    WriteToLogFile(logFile, "既存ブックが存在しない場合", logType);
                    excelApp.Visible = false;
                    excelApp.DisplayAlerts = false;
                    sheet.Cells[line, column] = status;
                    excelBook.SaveAs(ExportFile);
                    WriteToLogFile(logFile, ExportFile , logType);
                }
            }
            catch (Exception e)
            {
                WriteToLogFile(logFile, "例外が発生しました。", "ERROR");
                Console.WriteLine(e);
            }
            finally
            {
                if (column == statusNum)
                {
                WriteToLogFile(logFile, "Excel操作処理：終了", logType);
                excelApp.Quit();
                System.Runtime.InteropServices.Marshal.ReleaseComObject(sheet);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(excelBook);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);
                WriteToLogFile(logFile, "Excelアクセス処理：終了", logType);
                }
            }
        }

        public void WriteToLogFile (string File, string message, string logType)
        {
            message = logType + "【" + DateTime.Now + "】" +   message;
            Encoding enc = Encoding.GetEncoding("utf-8");
            using (StreamWriter writer = new StreamWriter(File, true, enc))
            {
                writer.WriteLine(message);
            }
        }
    }
}
