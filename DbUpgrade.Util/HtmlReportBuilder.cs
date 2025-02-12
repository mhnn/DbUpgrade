using DbUp.Engine;
using DbUp.Helpers;

namespace DbUpgrade.Util
{
    public static class HtmlReportBuilder
    {
        /// <summary>
        /// 生成报告
        /// </summary>
        /// <param name="engine">升级引擎</param>
        /// <param name="fileName">文件名</param>
        /// <param name="version">版本号</param>
        public static void Build(UpgradeEngine? engine, string fileName, string version)
        {
            if (!Directory.Exists(AppContext.BaseDirectory + @"\htmlReports"))
            {
                Directory.CreateDirectory(AppContext.BaseDirectory + @"\htmlReports");
            }
            var filePath = AppContext.BaseDirectory + @"\htmlReports\" + $"{fileName}.html";
            engine.GenerateUpgradeHtmlReport(filePath, version, fileName);
            HtmlProcessor(filePath);
        }

        /// <summary>
        /// 原报告的js文件都是引入的 cdn，没有网络或不开代理会无法正常显示，此处将其替换为静态文件
        /// </summary>
        /// <param name="filePath">文件路径</param>
        private static void HtmlProcessor(string filePath)
        {
            try
            {
                var assetsPath = AppContext.BaseDirectory + @"\static";
                // 读取HTML文件内容
                string htmlContent = File.ReadAllText(filePath);

                // 替换指定字符串
                htmlContent = RegexDeclare.Link().Replace(htmlContent, "");
                var bootstrapCss = File.ReadAllText(assetsPath + @"\bootstrap.min.css");
                htmlContent = RegexDeclare.Css().Replace(htmlContent, $"<style>\n{bootstrapCss}\n</style>");

                var jquery = File.ReadAllText(assetsPath + @"\jquery-3.3.1.slim.min.js");
                htmlContent = RegexDeclare.Jquery().Replace(htmlContent, $"<script>{jquery}</script>");

                //popper.min.js
                var popper = File.ReadAllText(assetsPath + @"\popper.min.js");
                htmlContent = RegexDeclare.Popper().Replace(htmlContent, $"<script>\n{popper}\n</script>");
                //bootstrap.min.js
                var bootstrapJs = File.ReadAllText(assetsPath + @"\bootstrap.min.js");
                htmlContent = RegexDeclare.Bootstrap().Replace(htmlContent, $"<script>\n{bootstrapJs}\n</script>");
                //run_prettify.js
                var runPrettify = File.ReadAllText(assetsPath + @"\run_prettify.js");
                htmlContent = RegexDeclare.RunPrettify().Replace(htmlContent, $"<script>\n{runPrettify}\n</script>");
                //lang-sql.js
                var langSql = File.ReadAllText(assetsPath + @"\lang-sql.js");
                htmlContent = RegexDeclare.LangSql().Replace(htmlContent, $"<script>\n{langSql}\n</script>");

                // 将更新后的内容写回到文件
                File.WriteAllText(filePath, htmlContent);

                Console.WriteLine("生成报告成功!请至根目录查看");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"升级报告生成出现错误: {ex.Message}");
            }
        }
    }
}
