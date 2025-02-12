using System.Text.RegularExpressions;

namespace DbUpgrade.Util
{
    public partial class RegexDeclare
    {
        /// <summary>
        /// 医院序号匹配
        /// </summary>
        /// <returns></returns>
        [GeneratedRegex(@"(?<=--\s*@h\s+)[0-9,，]+", RegexOptions.IgnoreCase)]
        public static partial Regex MatchHospitalVariables();
        /// <summary>
        /// 变量声明匹配
        /// </summary>
        /// <remarks>
        /// declare或set开头，出现0~1次；匹配@h；匹配varchar，varchar后可跟宽度，0~1次；匹配=''，字符串中可包含任意字符，0~1次；以;结尾，；可选
        /// </remarks>
        /// <returns></returns>
        [GeneratedRegex(@"declare\s+@h\s+(varchar(\(\w*\))?)?\s*(=\s*'\S*')?;?", RegexOptions.IgnoreCase)]
        public static partial Regex HospitalDeclare();

        /// <summary>
        /// 匹配压缩包文件名中的版本号声明
        /// </summary>
        /// <returns></returns>
        [GeneratedRegex(@"^V\d+\.\d+\.\d+(\.\d+)?")]
        public static partial Regex Version();

        #region html报告，文件静态引入，以解决CORS问题
        /// <summary>
        /// jquery文件引入匹配
        /// </summary>
        /// <returns></returns>
        [GeneratedRegex(@"^<script .*jquery-3\.3\.1\.slim\.min\.js.*></script>$")]
        public static partial Regex Jquery();

        /// <summary>
        /// css文件引入匹配
        /// </summary>
        /// <returns></returns>
        [GeneratedRegex(@"<link .*>")]
        public static partial Regex Link();

        /// <summary>
        /// style标签匹配
        /// </summary>
        /// <returns></returns>
        [GeneratedRegex(@"<style>[\s\S]*</style>", RegexOptions.Multiline)]
        public static partial Regex Css();

        /// <summary>
        /// popper文件引入匹配
        /// </summary>
        /// <returns></returns>
        [GeneratedRegex(@"<script .*popper\.min\.js.*></script>")]
        public static partial Regex Popper();

        /// <summary>
        /// bootstrap文件引入匹配
        /// </summary>
        /// <returns></returns>
        [GeneratedRegex(@"<script .*bootstrap\.min\.js.*></script>")]
        public static partial Regex Bootstrap();

        /// <summary>
        /// run_prettify文件引入匹配
        /// </summary>
        /// <returns></returns>
        [GeneratedRegex(@"<script .*run_prettify\.js.*></script>")]
        public static partial Regex RunPrettify();

        /// <summary>
        /// lang-sql文件引入匹配
        /// </summary>
        /// <returns></returns>
        [GeneratedRegex(@"<script .*lang-sql\.js.*></script>")]
        public static partial Regex LangSql();
        #endregion
    }
}
