using Newtonsoft.Json.Linq;

namespace ExportExcelFromJson
{
    class Controller
    {
        static void Main(string[] args)
        {
            var modelWriteToLogFile = new Model();
            Dictionary<string, string> logType = new Dictionary<string, string> { { "INFO", "INFO   " },{ "WARNING", "WARNING"},{"ERROR", "WRROR  "} };
            string jsonString;
            JObject jsonObject;
            Dictionary<string, string> statusCollection = new Dictionary<string, string>();
            string logFile = "C:\\Implem\\個人フォルダ_PublicRepository\\PublicRepository\\6. C#\\作成中\\ExportExcelFromJson\\ExportExcelFromJson\\log.txt";
            string inputFile = "C:\\Implem\\個人フォルダ_PublicRepository\\PublicRepository\\6. C#\\作成中\\ExportExcelFromJson\\ExportExcelFromJson\\multipleProcess.json";
            string outputFile = "C:\\Implem\\個人フォルダ_PublicRepository\\PublicRepository\\6. C#\\作成中\\ExportExcelFromJson\\ExportExcelFromJson\\sample.xlsx";
            try
            {
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
                foreach(var column in columns)
                {
                    string columnName = (string)column["ColumnName"];
                    if (columnName != "Status") continue;
                    string choicesText = (string)column["ChoicesText"];
                    if (choicesText == null) continue; 
                    var choicesTextArray = choicesText.Split("\n");
                        var statusNum = choicesTextArray.Length;
                        Console.WriteLine("ステータス数：" + statusNum);
                        statusCollection["0"] = "未設定";
                        statusCollection["-1"] = "全て";
                        int i = 1;
                        foreach (var choiceText in choicesTextArray)
                        {
                            var choiceTextArray = choiceText.Split(",");
                            statusCollection[choiceTextArray[0]] = choiceTextArray[1];
                            var modelManipulateExcel = new Model();
                            modelManipulateExcel.ManipulateExcel(outputFile, choiceTextArray[1], 1, i, logFile, logType["INFO"], statusNum);
                            i++;
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
                foreach(JObject process in processes)
                {
                    modelWriteToLogFile.WriteToLogFile(logFile, "プロセス設定の読み込み：開始", logType["INFO"]);
                    try
                    {
                        modelWriteToLogFile.WriteToLogFile(logFile, "Id:" + process["Id"], logType["INFO"]);
                        modelWriteToLogFile.WriteToLogFile(logFile, "Name:" + process["Name"], logType["INFO"]);
                        modelWriteToLogFile.WriteToLogFile(logFile, "DisplayName:" + process["DisplayName"], logType["INFO"]);
                        modelWriteToLogFile.WriteToLogFile(logFile, "CurrentStatus" + process["CurrentStatus"], logType["INFO"]);
                        modelWriteToLogFile.WriteToLogFile(logFile, "ChangedStatus" + process["ChangedStatus"], logType["INFO"]);
                            modelWriteToLogFile.WriteToLogFile(logFile, "現在の状況の比較処理：開始", logType["INFO"]);
                            foreach (var status in statusCollection)
                            {
                                if (status.Key == (string)process["CurrentStatus"]) 
                                {
                                    modelWriteToLogFile.WriteToLogFile(logFile, "現在の状況:" + status.Value, logType["INFO"]);
                                    modelWriteToLogFile.WriteToLogFile(logFile, "Excelアクセス処理：開始", logType["INFO"]);
                                    
                                    break;
                                }
                            }
                            modelWriteToLogFile.WriteToLogFile(logFile, "現在の状況の比較処理：終了", logType["INFO"]);
                            modelWriteToLogFile.WriteToLogFile(logFile, "変更後の状況の比較処理：開始", logType["INFO"]);
                            foreach (var status in statusCollection)
                            {
                                if (status.Key == (string)process["ChangedStatus"])
                                {
                                    modelWriteToLogFile.WriteToLogFile(logFile, "変更後の状況:" + status.Value, logType["INFO"]);
                                    break;
                                }
                            }
                            modelWriteToLogFile.WriteToLogFile(logFile, "変更後の状況の比較処理：終了", logType["INFO"]);
                　　　　　　modelWriteToLogFile.WriteToLogFile(logFile, "プロセスの取得処理：終了", logType["INFO"]);
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
        }
    }
}
