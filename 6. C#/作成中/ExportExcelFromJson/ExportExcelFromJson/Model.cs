﻿using Excel = Microsoft.Office.Interop.Excel;
using System;
using io = System.IO;
using System.Text;
using Microsoft.Office.Interop.Excel;


namespace ExportExcelFromJson
{
    internal class Model
    {
        public void ManipulateExcel(string ExportFile, string[,] text, string logFile, string logType)
        {
            WriteToLogFile(logFile, "Excelアクセス処理：開始", logType);
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
                    for (var line = 0; line < text.GetLength(0); line++)
                    {
                        for (var column = 0; column < text.GetLength(1); column++)
                        {
                            sheet.Cells[line + 1, column + 1] = text[line, column];
                        }
                    }
                    excelBook.SaveAs(ExportFile);
                    WriteToLogFile(logFile, ExportFile, logType);
                }
                else
                {
                    WriteToLogFile(logFile, "既存ブックが存在する場合", logType);
                    excelBook = excelBooks.Open(Path.GetFullPath(ExportFile));
                    sheets = excelBook.Worksheets;
                    sheet = sheets[1];
                    for (var line = 0; line < text.GetLength(0); line++)
                    {
                        for (var column = 0; column < text.GetLength(1); column++)
                        {
                            sheet.Cells[line + 1, column + 1] = text[line, column];
                        }
                    }
                    excelBook.Save();
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(sheets);
                    WriteToLogFile(logFile, ExportFile, logType);
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
                System.Runtime.InteropServices.Marshal.ReleaseComObject(sheet);
                excelBook.Close();
                System.Runtime.InteropServices.Marshal.ReleaseComObject(excelBook);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(excelBooks);
                excelApp.DisplayAlerts = true;
                excelApp.Quit();
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
