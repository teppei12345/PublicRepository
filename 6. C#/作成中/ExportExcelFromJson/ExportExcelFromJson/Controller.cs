using Newtonsoft.Json.Linq;

namespace ExportExcelFromJson
{
    class Controller
    {
        static void Main(string[] args)
        {
            var sw = new System.Diagnostics.Stopwatch();
            sw.Start();
            var modelWriteToLogFile = new Model();
            Model modelManipulateExcel = new Model();
            Dictionary<string, string> logType = new Dictionary<string, string> { { "INFO", "INFO " }, { "ERROR", "ERROR" } };
            string jsonString;
            JObject jsonObject;
            int columnCount = 1;
            Dictionary<string, string> statusCollection = new Dictionary<string, string>();
            DateTime dt = DateTime.Now;
            string now = dt.ToString("yyyyMMddHHmmss");
            string logFile = "C:\\Implem\\個人フォルダ_PublicRepository\\PublicRepository\\6. C#\\作成中\\ExportExcelFromJson\\ExportExcelFromJson\\bin\\Debug\\net8.0\\logs\\" + now + "_log.log";
            string inputFile = "C:\\Implem\\個人フォルダ_PublicRepository\\PublicRepository\\6. C#\\作成中\\ExportExcelFromJson\\ExportExcelFromJson\\bin\\Debug\\net8.0\\json\\multipleProcess.json";
            string outputFile = "C:\\Implem\\個人フォルダ_PublicRepository\\PublicRepository\\6. C#\\作成中\\ExportExcelFromJson\\ExportExcelFromJson\\bin\\Debug\\net8.0\\excel\\" + now + "_sitesettings.xlsx";
            try
            {
                modelWriteToLogFile.WriteToLogFile(logFile, "ExportExcelFromJson：開始", logType["INFO"]);
                modelWriteToLogFile.WriteToLogFile(logFile, "jsonファイルの読み取り：開始", logType["INFO"]);
                modelWriteToLogFile.WriteToLogFile(logFile, "jsonファイルパス：" + inputFile, logType["INFO"]);
                jsonString = File.ReadAllText(inputFile);
                modelWriteToLogFile.WriteToLogFile(logFile, "jsonファイルの読み取り：終了", logType["INFO"]);
                modelWriteToLogFile.WriteToLogFile(logFile, "jsonオブジェクトの生成処理：開始", logType["INFO"]);
                jsonObject = JObject.Parse(jsonString);
                modelWriteToLogFile.WriteToLogFile(logFile, "jsonオブジェクトの生成処理：終了", logType["INFO"]);
                modelWriteToLogFile.WriteToLogFile(logFile, "ステータスの取得処理：開始", logType["INFO"]);
                var columns = jsonObject["Sites"][0]["SiteSettings"]["Columns"];
                if (columns == null)
                {
                    modelWriteToLogFile.WriteToLogFile(logFile, "columns == null", logType["INFO"]);
                }
                else
                {
                    foreach (var column in columns)
                    {
                        string columnName = (string)column["ColumnName"];
                        if (columnName != "Status") continue;
                        string choicesText = (string)column["ChoicesText"];
                        if (choicesText == null) continue;
                        string[] choicesTextArray = choicesText.Split("\n");
                        statusCollection["0"] = "未設定";
                        statusCollection["-1"] = "全て";
                        foreach (var choiceText in choicesTextArray)
                        {
                            string[] choiceTextArray = choiceText.Split(",");
                            statusCollection[choiceTextArray[0]] = choiceTextArray[1];
                        }
                        int i = 1;
                        foreach (var status in statusCollection)
                        {
                            switch (status.Key)
                            {
                                case "0":
                                case "-1":
                                    break;
                                default:
                                    columnCount = i * 2;
                                    modelManipulateExcel.ManipulateExcel(outputFile, status.Value, 1, columnCount, logFile, logType["INFO"]);
                                    i++;
                                    break;
                            }
                        }
                    }
                }
                modelWriteToLogFile.WriteToLogFile(logFile, "ステータスの取得処理：終了", logType["INFO"]);
                modelWriteToLogFile.WriteToLogFile(logFile, "プロセスの取得処理：開始", logType["INFO"]);
                var processes = jsonObject["Sites"][0]["SiteSettings"]["Processes"];
                if (processes == null)
                {
                    modelWriteToLogFile.WriteToLogFile(logFile, "processes == null", logType["INFO"]);
                }
                else
                {
                    foreach (JObject process in processes)
                    {
                        modelWriteToLogFile.WriteToLogFile(logFile, "プロセス設定の読み込み：開始", logType["INFO"]);
                        try
                        {
                            if ((string)process["CurrentStatus"] == null)
                            {
                                modelWriteToLogFile.WriteToLogFile(logFile, "(string)process[\"CurrentStatus\"] == null", logType["INFO"]);
                            }
                            else
                            {
                                modelWriteToLogFile.WriteToLogFile(logFile, "現在の状況の比較処理：開始", logType["INFO"]);
                                modelManipulateExcel.ManipulateExcel(outputFile, (string)process["Name"], ((int)process["Id"] + 1), 1, logFile, logType["INFO"]);
                                for (var i = 2; i <= columnCount; i++)
                                {
                                    modelManipulateExcel.ManipulateExcel(outputFile, "→", ((int)process["Id"] + 1), i, logFile, logType["INFO"]);
                                }
                                foreach (var status in statusCollection.Select((Entry, Index) => new { Entry, Index }))
                                {
                                    switch (status.Entry.Key)
                                    {
                                        case "0":
                                        case "-1":
                                            break;
                                        default:
                                            if (status.Entry.Key == (string)process["CurrentStatus"])
                                            {
                                                modelManipulateExcel.ManipulateExcel(outputFile, "●", ((int)process["Id"] + 1), (status.Index * 2), logFile, logType["INFO"]);
                                            }
                                            break;
                                    }
                                }
                                modelWriteToLogFile.WriteToLogFile(logFile, "現在の状況の比較処理：終了", logType["INFO"]);
                                modelWriteToLogFile.WriteToLogFile(logFile, "変更後の状況の比較処理：開始", logType["INFO"]);
                                foreach (var status in statusCollection.Select((Entry, Index) => new { Entry, Index }))
                                {
                                    switch (status.Entry.Key)
                                    {
                                        case "0":
                                        case "-1":
                                            break;
                                        default:
                                            if (status.Entry.Key == (string)process["ChangedStatus"])
                                            {
                                                modelManipulateExcel.ManipulateExcel(outputFile, "●", ((int)process["Id"] + 1), (status.Index * 2), logFile, logType["INFO"]);
                                            }
                                            break;
                                    }
                                }
                                modelWriteToLogFile.WriteToLogFile(logFile, "変更後の状況の比較処理：終了", logType["INFO"]);
                                modelWriteToLogFile.WriteToLogFile(logFile, "プロセスの取得処理：終了", logType["INFO"]);
                            }
                        }
                        catch (Exception e)
                        {
                            modelWriteToLogFile.WriteToLogFile(logFile, "例外が発生しました。", logType["ERROR"]);
                            Console.WriteLine(e);
                        }
                        finally
                        {
                        }
                    }
                    modelWriteToLogFile.WriteToLogFile(logFile, "プロセス設定の読み込み：終了", logType["INFO"]);
                }
            }
            catch (Exception e)
            {
                modelWriteToLogFile.WriteToLogFile(logFile, "例外が発生しました。", logType["ERROR"]);
                Console.WriteLine(e);
            }
            finally
            {
                sw.Stop();
                TimeSpan ts = sw.Elapsed;
                Console.WriteLine("処理時間：" + sw.ElapsedMilliseconds + "ms");
                modelWriteToLogFile.WriteToLogFile(logFile, "ExportExcelFromJson：終了", logType["INFO"]);
                modelWriteToLogFile.WriteToLogFile(logFile, "処理時間：" + sw.ElapsedMilliseconds + "ms", logType["INFO"]);
            }
        }
    }
}
