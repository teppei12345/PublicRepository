using Excel = Microsoft.Office.Interop.Excel;
using System;
using io = System.IO;
using System.Text;
using Microsoft.Office.Interop.Excel;
using Microsoft.VisualBasic;
using Newtonsoft.Json.Linq;
using System.Reflection;
using System.Text.Json.Nodes;
using Microsoft.Office.Interop.Excel;
using Newtonsoft.Json.Linq;
using System.Data.Common;
using System.Linq;
using System.Text.Json.Nodes;
using Newtonsoft.Json;

namespace ExportExcelFromJson
{
    internal class Model
    {
        Dictionary<string, string> logType = new Dictionary<string, string> { { "INFO", "INFO " }, { "ERROR", "ERROR" }, { "DEBUG", "DEBUG" } };
        string jsonString;
        string inputFile = ".\\json\\multipleProcess.json";
        JObject jsonObject;
        JToken sites;
        JObject siteSettings;
        JToken Columns;
        string[] sitesettings;
        Dictionary<string, JArray> columnInfoDictionaly = new Dictionary<string, JArray>();
        string[] siteSettingsKeys = { "Columns", "Processes" };
        public bool setJson (string logFile)
        {
            WriteToLogFile(logFile, "jsonファイルの読み取り：開始");
            WriteToLogFile(logFile, "jsonファイルパス：" + inputFile);
            jsonString = File.ReadAllText(inputFile);
            WriteToLogFile(logFile, "jsonファイルの読み取り：終了");
            WriteToLogFile(logFile, "jsonオブジェクトの生成処理：開始");
            jsonObject = JObject.Parse(jsonString);
            WriteToLogFile(logFile, "jsonオブジェクトの生成処理：終了");
            WriteToLogFile(logFile, "サイト設定取得処理：開始");
            sites = jsonObject["Sites"];
            if (sites == null)
            {
                WriteToLogFile(logFile, "サイトが存在しないため関数処理の中断");
                return false;
            } else
            {
                return true;
            }
        }
        public bool setSiteSettings (string logFile)
        {
            siteSettings = (JObject)sites[0]["SiteSettings"];
            if (siteSettings == null)
            {
                WriteToLogFile(logFile, "サイト設定が存在しないため関数処理の中断");
                return false;
            } else
            {
                return true;
            }
        }
        public void ManipulateExcel(string ExportFile, string logFile)
        {
            WriteToLogFile(logFile, "Excelアクセス処理：開始");
            Application excelApp = new Application(); ;
            Workbooks excelBooks = excelApp.Workbooks; ;
            Workbook excelBook = null;
            Sheets sheets = null;
            Worksheet sheet = null;
            excelApp.Visible = false;
            excelApp.DisplayAlerts = false;
            try
            {
                WriteToLogFile(logFile, "Excel操作処理：開始");
                if (!File.Exists(ExportFile))
                {
                    WriteToLogFile(logFile, "既存ブックが存在しない場合");
                    excelBook = excelBooks.Add();
                    foreach (var sitesetting in siteSettings)
                        {
                        if (sitesetting.Value is JArray)
                        {
                            columnInfoDictionaly[sitesetting.Key] = (JArray)sitesetting.Value;
                        }
                    }
                    foreach (var siteSettingsKey in siteSettingsKeys)
                    {
                        Console.WriteLine("siteSettingsKey：" + siteSettingsKey);
                        Console.WriteLine(columnInfoDictionaly[siteSettingsKey]);
                        Console.WriteLine(columnInfoDictionaly[siteSettingsKey].GetType());
                        foreach (JArray siteSettingsValue in columnInfoDictionaly.Values)
                        {
                            Console.WriteLine(siteSettingsValue.GetType());
                            Console.WriteLine(siteSettingsValue[0]);            
                        }
                    }
                    string path =  Path.GetFullPath(ExportFile);
                    excelBook.SaveAs(path);
                    WriteToLogFile(logFile, ExportFile);
                }
                else
                {
                    WriteToLogFile(logFile, "既存ブックが存在する場合");
                    excelBook = excelBooks.Open(Path.GetFullPath(ExportFile));
                    sheets = excelBook.Worksheets;
                    sheet = sheets[1];
                    foreach (var sitesetting in siteSettings)
                    {
                        Console.WriteLine(sitesetting);
                    }
                    excelBook.Save();
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(sheets);
                    WriteToLogFile(logFile, ExportFile);
                }
            }
            catch (Exception e)
            {
                WriteToLogFile(logFile, "例外が発生しました。", logType["ERROR"]);
                Console.WriteLine(e);
            }
            finally
            {
                WriteToLogFile(logFile, "Excel操作処理：終了");
                System.Runtime.InteropServices.Marshal.ReleaseComObject(sheet);
                excelBook.Close();
                System.Runtime.InteropServices.Marshal.ReleaseComObject(excelBook);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(excelBooks);
                excelApp.DisplayAlerts = true;
                excelApp.Quit();
                System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);
                WriteToLogFile(logFile, "Excelアクセス処理：終了");
            }
        }

        public void WriteToLogFile(string File, string text, string logType = "DEBUG")
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
