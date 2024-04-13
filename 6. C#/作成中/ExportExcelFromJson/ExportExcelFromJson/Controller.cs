

namespace ExportExcelFromJson
{
    class Controller
    {
        static void Main(string[] args)
        {
            var model = new Model();
            var sw = new System.Diagnostics.Stopwatch();
            sw.Start();
            DateTime now = DateTime.Now;
            string date = now.ToString("yyyyMMddHHmmss");
            string logFile = ".\\logs\\" + date + "_log.log";
            string outputFile = ".\\excel\\" + date + "_sitesettings.xlsx";
            model.WriteToLogFile(logFile, "ExportExcelFromJson：開始");
            try
            {
                if (model.setJson(logFile) == false)
                {
                    return;
                } else
                {
                if (model.setSiteSettings(logFile) == false)
                {
                    return;
                }
                else
                {
                model.ManipulateExcel(outputFile, logFile);
                }
                }
            }
            catch (Exception e)
            {
                model.WriteToLogFile(logFile, "「ExportExcelFromJson処理」にて例外が発生しました。");
                Console.WriteLine(e);
            }
            finally
            {
                sw.Stop();
                TimeSpan ts = sw.Elapsed;
                model.WriteToLogFile(logFile, "ExportExcelFromJson：終了");
                model.WriteToLogFile(logFile, "処理時間：" + sw.ElapsedMilliseconds + "ms");
                Console.WriteLine("処理時間：" + sw.ElapsedMilliseconds + "ms");
            }
        }
    }
}
