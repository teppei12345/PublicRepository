using Newtonsoft.Json.Linq;

namespace ExportExcelFromJson
{
    class Program
    {
        static void Main(string[] args)
        {
            string jsonString = "";
            JObject jsonObject;
            Dictionary<string, string> statusCollection = new Dictionary<string, string>();
            string inputFile = "C:\\Implem\\個人フォルダ_PublicRepository\\PublicRepository\\6. C#\\作成中\\ExportExcelFromJson\\ExportExcelFromJson\\multipleProcess.json";
            string outputFile = "C:\\Implem\\個人フォルダ_PublicRepository\\PublicRepository\\6. C#\\作成中\\ExportExcelFromJson\\ExportExcelFromJson\\sample.xlsx";
            try
            {
                Console.WriteLine("jsonファイルの読み取り：開始");
                Console.WriteLine("jsonファイルパス：" + inputFile);
                jsonString = File.ReadAllText(inputFile);
                Console.WriteLine("jsonファイルの読み取り：終了");
                Console.WriteLine("jsonオブジェクトの生成処理：開始");
                jsonObject = JObject.Parse(jsonString);
                Console.WriteLine("jsonオブジェクトの生成処理：終了");
                Console.WriteLine("ステータスの取得処理：開始");
                var columns = jsonObject["Sites"][0]["SiteSettings"]["Columns"];
                if (columns == null)
                {
                    Console.WriteLine("columns == null");
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
                        statusCollection["0"] = "未設定";
                        statusCollection["-1"] = "全て";
                        foreach (var choiceText in choicesTextArray)
                        {
                            var choiceTextArray = choiceText.Split(",");
                            statusCollection[choiceTextArray[0]] = choiceTextArray[1];
                        }
                    }
                }
                Console.WriteLine("ステータスの取得処理：終了");
                Console.WriteLine("プロセスの取得処理：開始");
                var processes = jsonObject["Sites"][0]["SiteSettings"]["Processes"];
                if (processes == null)
                {
                    Console.WriteLine("processes == null");
                }
                else
                {
                foreach(JObject process in processes)
                {
                    Console.WriteLine("プロセス設定の読み込み：開始");
                    try
                    {
                        Console.WriteLine("Id:" + process["Id"]);
                        Console.WriteLine("Name:" + process["Name"]);
                        Console.WriteLine("DisplayName:" + process["DisplayName"]);
                        Console.WriteLine("CurrentStatus" + process["CurrentStatus"]);
                        Console.WriteLine("ChangedStatus" + process["ChangedStatus"]);
                            Console.WriteLine("現在の状況の比較処理：開始");
                            foreach (var status in statusCollection)
                            {
                                if (status.Key == (string)process["CurrentStatus"]) 
                                {
                                    Console.WriteLine("現在の状況:" + status.Value);
                                    break;
                                }
                            }
                            Console.WriteLine("現在の状況の比較処理：終了");
                            Console.WriteLine("変更後の状況の比較処理：開始");
                            foreach (var status in statusCollection)
                            {
                                if (status.Key == (string)process["ChangedStatus"])
                                {
                                    Console.WriteLine("変更後の状況:" + status.Value);
                                    break;
                                }
                            }
                            Console.WriteLine("変更後の状況の比較処理：終了");
                　　　　　　Console.WriteLine("プロセスの取得処理：終了");
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("例外が発生しました。");
                        Console.WriteLine(e);
                    }
                    finally
                    {
                    }
                }
                Console.WriteLine("プロセス設定の読み込み：終了");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("例外が発生しました。");
                Console.WriteLine(e);
            }
            /*
            Excel.Application excelApp = new Excel.Application();
            Excel.Workbooks excelBooks = excelApp.Workbooks;
            Excel.Workbook excelBook = excelBooks.Add();
            Excel.Worksheet sheet = (Excel.Worksheet)excelApp.Worksheets["sheet1"];
            try
            {
                excelApp.Visible = false;
                sheet.Cells[1, 1] = jsonString;
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
            */
        }
    }
}
