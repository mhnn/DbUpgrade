using DbUp.Engine;
using System.Text.RegularExpressions;

namespace DbUpgrade.Util
{
    /// <summary>
    /// 
    /// </summary>
    public partial class ReplaceLineEndingsProcessor : IScriptPreprocessor
    {
        /// <summary>
        /// 读取到的文件的行尾符可能是非标准化的，需要替换
        /// </summary>
        /// <param name="contents">脚本内容</param>
        /// <returns></returns>
        public string Process(string contents)
        {
            // 使用ReplaceLineEndings，替换为对应平台的标准行尾符；（非Unix平台为\r\n，Unix平台为\n）
            var newContents = contents.ReplaceLineEndings();
            return newContents;
        }
    }
}
