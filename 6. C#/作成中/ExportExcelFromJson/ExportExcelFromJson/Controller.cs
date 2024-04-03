using Microsoft.Office.Interop.Excel;
using Newtonsoft.Json.Linq;
using System.Data.Common;
using System.Linq;
using System.Text.Json.Nodes;

namespace ExportExcelFromJson
{
    class Controller
    {
        static void Main(string[] args)
        {
            var sw = new System.Diagnostics.Stopwatch();
            sw.Start();
            var model = new Model();
            Dictionary<string, string> logType = new Dictionary<string, string> { { "INFO", "INFO " }, { "ERROR", "ERROR" }, { "DEBUG", "DEBUG" } };
            string[,] ExportTextArray;
            string jsonString;
            string columnName;
            string statusChoicesText;
            string[] statusChoicesTextArray = [];
            int statusChoicesTextNum = 0;
            int processNum = 0;
            JToken siteSettings;
            JToken columnSettings;
            JToken processes;
            JObject jsonObject;
            int columnCount = 0;
            Dictionary<string, string> statusCollection = new Dictionary<string, string>();
            DateTime dt = DateTime.Now;
            string now = dt.ToString("yyyyMMddHHmmss");
            string logFile = "C:\\Implem\\個人フォルダ_PublicRepository\\PublicRepository\\6. C#\\作成中\\ExportExcelFromJson\\ExportExcelFromJson\\bin\\Debug\\net8.0\\logs\\" + now + "_log.log";
            string inputFile = "C:\\Implem\\個人フォルダ_PublicRepository\\PublicRepository\\6. C#\\作成中\\ExportExcelFromJson\\ExportExcelFromJson\\bin\\Debug\\net8.0\\json\\multipleProcess.json";
            string outputFile = "C:\\Implem\\個人フォルダ_PublicRepository\\PublicRepository\\6. C#\\作成中\\ExportExcelFromJson\\ExportExcelFromJson\\bin\\Debug\\net8.0\\excel\\" + now + "_processsettings.xlsx";
            model.WriteToLogFile(logFile, "ExportExcelFromJson：開始", logType["DEBUG"]);
            try
            {
                model.WriteToLogFile(logFile, "jsonファイルの読み取り：開始", logType["DEBUG"]);
                model.WriteToLogFile(logFile, "jsonファイルパス：" + inputFile, logType["DEBUG"]);
                jsonString = File.ReadAllText(inputFile);
                model.WriteToLogFile(logFile, "JSON文字列：" + jsonString, logType["INFO"]);
                model.WriteToLogFile(logFile, "jsonファイルの読み取り：終了", logType["DEBUG"]);
                model.WriteToLogFile(logFile, "jsonオブジェクトの生成処理：開始", logType["DEBUG"]);
                jsonObject = JObject.Parse(jsonString);
                model.WriteToLogFile(logFile, "JSONオブジェクト：" + jsonObject, logType["INFO"]);
                model.WriteToLogFile(logFile, "jsonオブジェクトの生成処理：終了", logType["DEBUG"]);
                model.WriteToLogFile(logFile, "サイト設定取得処理：開始", logType["DEBUG"]);
                siteSettings = jsonObject["Sites"][0]["SiteSettings"];
                if (siteSettings == null)
                {
                    model.WriteToLogFile(logFile, "サイト設定が存在しないため関数処理の中断", logType["DEBUG"]);
                    return;
                }
                else
                {
                    columnSettings = siteSettings["Columns"];
                    if (columnSettings == null)
                    {
                        model.WriteToLogFile(logFile, "カラム設定が存在しないため処理の中断", logType["DEBUG"]);
                        return;
                    }
                    model.WriteToLogFile(logFile, "ステータスの取得処理：開始", logType["DEBUG"]);
                    foreach (var columns in columnSettings)
                    {
                        columnName = (string)columns["ColumnName"];
                        if (columnName != "Status") continue;
                        statusChoicesText = (string)columns["ChoicesText"];
                        if (statusChoicesText == null) continue;
                        statusChoicesTextArray = statusChoicesText.Split("\n");
                        statusChoicesTextNum = (statusChoicesTextArray.Length * 2);
                    }
                    model.WriteToLogFile(logFile, "ステータス数：" + statusChoicesTextNum, logType["INFO"]);
                    statusCollection["0"] = "未設定";
                    statusCollection["-1"] = "全て";
                    foreach (var choiceText in statusChoicesTextArray)
                    {
                        string[] choiceTextArray = choiceText.Split(",");
                        statusCollection[choiceTextArray[0]] = choiceTextArray[1];
                    }
                    model.WriteToLogFile(logFile, "ステータスの取得処理：終了", logType["DEBUG"]);
                    model.WriteToLogFile(logFile, "プロセスの取得処理：開始", logType["DEBUG"]);
                    processes = siteSettings["Processes"];
                    if (processes == null)
                    {
                        model.WriteToLogFile(logFile, "プロセス設定が存在しないため処理の中断", logType["DEBUG"]);
                        return;
                    }
                    processNum = processes.Count();
                    model.WriteToLogFile(logFile, "プロセス数：" + processNum, logType["INFO"]);

                    model.WriteToLogFile(logFile, "プロセスの取得処理：終了", logType["DEBUG"]);
                }
                model.WriteToLogFile(logFile, "サイト設定取得処理：終了", logType["DEBUG"]);
                model.WriteToLogFile(logFile, "メモリ作成処理：開始", logType["DEBUG"]);
                ExportTextArray = new string[(processNum + 1), statusChoicesTextNum];
                ExportTextArray[0, 0] = "";
                model.WriteToLogFile(logFile, "メモリ作成処理：終了", logType["DEBUG"]);
                model.WriteToLogFile(logFile, "メモリ設定処理：開始", logType["DEBUG"]);
                model.WriteToLogFile(logFile, "メモリ領域へのStatus(Column)設定処理：開始", logType["DEBUG"]);
                string[] statusArray = new string[(statusChoicesTextNum)];
                foreach (var status in statusCollection)
                {
                    switch (status.Key)
                    {
                        case "0":
                        case "-1":
                            break;
                        default:
                            statusArray[columnCount] = "";
                            columnCount++;
                            statusArray[columnCount] = status.Value;
                            columnCount++;
                            break;
                    }
                }
                int index = 0;
                foreach (var status in statusArray)
                {
                    ExportTextArray[0, index] = status;
                    index++;
                }
                model.WriteToLogFile(logFile, "メモリ領域へのStatus(Column)設定処理：終了", logType["DEBUG"]);
                model.WriteToLogFile(logFile, "メモリ領域へのProcess(line)設定処理：開始", logType["DEBUG"]);
                foreach (JObject process in processes)
                {
                    string[] processArray = new string[(statusChoicesTextNum)];
                    if (process["CurrentStatus"] == null)
                    {
                        model.WriteToLogFile(logFile, "現在の状況が設定されていないため処理の中断", logType["DEBUG"]);
                        return;
                    }
                    else if (process["CurrentStatus"] != null)
                    {
                        model.WriteToLogFile(logFile, "「現在の状況」の設定処理：開始", logType["DEBUG"]);
                        processArray[0] = (string)process["Name"];
                        columnCount = 1;
                        var currentStatusFlag = false;
                        foreach (var status in statusCollection.Select((Entry, Index) => new { Entry, Index }))
                        {
                            if (currentStatusFlag)
                            {
                                break;
                            }
                            switch (status.Entry.Key)
                            {
                                case "0":
                                case "-1":
                                    break;
                                default:
                                    if (columnCount % 2 == 1)
                                    {
                                        if (status.Entry.Key == (string)process["CurrentStatus"])
                                        {
                                            processArray[columnCount] = "●";
                                            currentStatusFlag = true;
                                        }
                                        else
                                        {
                                            processArray[columnCount] = "";
                                        }
                                    }
                                    columnCount++;
                                    if (columnCount % 2 == 0)
                                    {
                                        processArray[columnCount] = "";
                                        columnCount++;
                                    }
                                    break;
                            }
                        }
                        model.WriteToLogFile(logFile, "「現在の状況」の設定処理：終了", logType["DEBUG"]);
                    }
                    if (process["ChangedStatus"] == null)
                    {
                        model.WriteToLogFile(logFile, "変更後の状況が設定されていないため処理の中断", logType["DEBUG"]);
                        return;
                    }
                    else if (process["ChangedStatus"] != null)
                    {
                        model.WriteToLogFile(logFile, "「変更後の状況」の設定処理：開始", logType["DEBUG"]);
                        var changedStatusFlag = false;
                        var flag = false;
                        columnCount = 1;
                        foreach (var status in statusCollection.Select((Entry, Index) => new { Entry, Index }))
                        {
                            if (changedStatusFlag)
                            {
                                break;
                            }
                            switch (status.Entry.Key)
                            {
                                case "0":
                                case "-1":
                                    break;
                                default:
                                    if (status.Entry.Key == (string)process["CurrentStatus"])
                                    {
                                        flag = true;
                                        columnCount++;
                                        processArray[columnCount] = "→";
                                        columnCount++;
                                        break;
                                    }
                                    if (flag == false)
                                    {
                                        columnCount = columnCount + 2;
                                        break;
                                    }
                                    else
                                    {
                                        if (status.Entry.Key == (string)process["ChangedStatus"])
                                        {
                                            processArray[columnCount] = "●";
                                            changedStatusFlag = true;
                                        }
                                        else
                                        {
                                            processArray[columnCount] = "→";
                                            columnCount++;
                                            processArray[columnCount] = "→";
                                            columnCount++;
                                        }
                                    }
                                    break;
                            }
                        }
                        model.WriteToLogFile(logFile, "「変更後の状況」の設定処理：終了", logType["DEBUG"]);
                    }
                    index = 0;
                    foreach (var status in processArray)
                    {
                        ExportTextArray[(int)process["Id"], index] = status;
                        index++;
                    }
                }
                model.WriteToLogFile(logFile, "メモリ領域へのProcess(line)設定処理：終了", logType["DEBUG"]);
                model.WriteToLogFile(logFile, "メモリ設定処理：終了", logType["DEBUG"]);
                model.WriteToLogFile(logFile, "Excel出力処理：開始", logType["DEBUG"]);
                model.ManipulateExcel(outputFile, ExportTextArray, logFile, logType["DEBUG"]);
                model.WriteToLogFile(logFile, "Excel出力処理：終了", logType["DEBUG"]);
            }
            catch (Exception e)
            {
                model.WriteToLogFile(logFile, "「ExportExcelFromJson処理」にて例外が発生しました。", logType["ERROR"]);
                Console.WriteLine(e);
            }
            finally
            {
                sw.Stop();
                TimeSpan ts = sw.Elapsed;
                model.WriteToLogFile(logFile, "ExportExcelFromJson：終了", logType["DEBUG"]);
                model.WriteToLogFile(logFile, "処理時間：" + sw.ElapsedMilliseconds + "ms", logType["DEBUG"]);
                Console.WriteLine("処理時間：" + sw.ElapsedMilliseconds + "ms");
            }
        }
    }
}
