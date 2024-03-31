using Excel = Microsoft.Office.Interop.Excel;
using System;
using io = System.IO;
using System.Text;


namespace ExportExcelFromJson
{
    internal class Model
    {
        public void ManipulateExcel(string ExportFile, string text, int line, int column, string logFile, string logType)
        {
            WriteToLogFile(logFile, "Excelアクセス処理：開始", logType);
            WriteToLogFile(logFile, "Cells[" + line + ", " + column + "] = " + text, logType);
            Excel.Application excelApp = new Excel.Application(); ;
            Excel.Workbooks excelBooks = excelApp.Workbooks; ;
            Excel.Workbook excelBook = null;
            Excel.Sheets sheets = null;
            Excel.Worksheet sheet = null;
            excelApp.Visible = false;
            excelApp.DisplayAlerts = false;
            try
            {
                WriteToLogFile(logFile, "Excel操作処理：開始", logType);
                if (!File.Exists(ExportFile))
                {
                    WriteToLogFile(logFile, "既存ブックが存在しない場合", logType);
                    excelBook = excelBooks.Add();
                    sheet = (Excel.Worksheet)excelApp.Worksheets["sheet1"];
                    sheet.Cells[line, column] = text;
                    excelBook.SaveAs(ExportFile);
                    WriteToLogFile(logFile, ExportFile, logType);
                }
                else
                {
                    WriteToLogFile(logFile, "既存ブックが存在する場合", logType);
                    excelBook = excelBooks.Open(Path.GetFullPath(ExportFile));
                    sheets = excelBook.Worksheets;
                    sheet = sheets[1];
                    sheet.Cells[line, column] = text;
                    excelBook.Save();
                }
            }
            catch (Exception e)
            {
                WriteToLogFile(logFile, "例外が発生しました。", "ERROR");
                Console.WriteLine(e);
            }
            finally
            {
                WriteToLogFile(logFile, "Excel操作処理：終了", logType);
                excelApp.Quit();
                System.Runtime.InteropServices.Marshal.ReleaseComObject(sheet);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(excelBook);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(excelBooks);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);
                WriteToLogFile(logFile, "Excelアクセス処理：終了", logType);
            }
        }

        public void WriteToLogFile(string File, string text, string logType)
        {
            string message = logType + "【" + DateTime.Now + "】" + text;
            Encoding enc = Encoding.GetEncoding("utf-8");
            try
            {
                if (io.File.Exists(File))
                {
                    using (StreamWriter writer = new StreamWriter(File, true, enc))
                    {
                        writer.WriteLine(message);
                    }
                }
                else
                {
                    using (var fileStream = io.File.Create(File))
                    {
                        //書き込み
                        byte[] info = new UTF8Encoding().GetBytes(message + "\n");
                        fileStream.Write(info, 0, info.Length);
                    }
                }
            }
            catch (Exception e)
            {
                message = "ERROR  【" + DateTime.Now + "】例外が発生しました。";
                using (StreamWriter writer = new StreamWriter(File, true, enc))
                {
                    writer.WriteLine(message);
                }
                Console.WriteLine(e);
            }
            finally
            {

            }
        }
    }
}
