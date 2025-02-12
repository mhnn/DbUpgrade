using DbUp.Engine;
using DbUpgrade.Models;
using Microsoft.Extensions.Options;
using System.Text;

namespace DbUpgrade.Util
{
    /// <summary>
    /// 脚本执行前的预处理
    /// 本类读取脚本中的`--@h `特殊声明，脚本将根据特殊声明后的医院序号执行多次
    /// </summary>
    public partial class HospitalIDProcessor : IScriptPreprocessor
    {
        public IOptions<AppConfig> AppConfig = null!;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="contents">脚本内容</param>
        /// <returns></returns>
        public string Process(string contents)
        {
            // 未声明医院序号变量，不处理
            if (!RegexDeclare.HospitalDeclare().IsMatch(contents))
            {
                return contents;
            }
            
            // 只有外网才需要执行多次
            if (AppConfig.Value.HospitalID == "0")
            {
                ConsoleUtil.WriteLine("匹配到多医院声明，脚本将多次执行！", ConsoleColor.DarkYellow);
                // 故需要用到`--@h`特殊声明
                var matchHospitalIDs = RegexDeclare.MatchHospitalVariables().Match(contents);
                // 没有匹配到变量替换，返回
                if (!matchHospitalIDs.Success)
                {
                    contents += ";\r\nRAISERROR (N'没有找到--@h声明，请检查脚本', 15, 1);";
                    return contents;
                }
                var scriptBuilder = new StringBuilder();
                // 解析出要执行的医院
                var hospitalIDs = matchHospitalIDs.Value.Split([',', '，']);
                if (hospitalIDs == null || hospitalIDs.Length == 0 || !hospitalIDs.All(hospitalID => int.TryParse(hospitalID, out _)))
                {
                    contents += ";\r\nRAISERROR (N'--@h声明参数有误，请检查脚本', 15, 1);";
                    return contents;
                }
                foreach (var hospitalID in hospitalIDs!)
                {
                    var script = RegexDeclare.HospitalDeclare().Replace(contents, $"DECLARE @h VARCHAR = '{hospitalID}';", 1);
                    // TODO：mysql不支持go语句，需要判断当前数据库是什么类型，再决定加不加go语句
                    scriptBuilder.AppendLine(script + "\r\ngo");
                }
                return scriptBuilder.ToString();
            }
            else
            {
                ConsoleUtil.WriteLine("匹配到医院变量声明，对变量进行赋值", ConsoleColor.DarkYellow);
                // 线上不需要执行多次，直接替换变量
                var script = RegexDeclare.HospitalDeclare().Replace(contents, $"DECLARE @h VARCHAR = '{AppConfig.Value.HospitalID}';", 1);
                return script;
            }
        }
    }
}
