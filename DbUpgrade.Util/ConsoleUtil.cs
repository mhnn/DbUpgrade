namespace DbUpgrade.Util
{
    public static class ConsoleUtil
    {
        /// <summary>
        /// 控制台输出文本，带颜色
        /// </summary>
        /// <param name="text">文本</param>
        /// <param name="consoleColor">颜色</param>
        public static void WriteLine(string text, ConsoleColor consoleColor)
        {
            Console.ForegroundColor = consoleColor;
            Console.WriteLine(text);
            Console.ResetColor();
        }

        /// <summary>
        /// 控制台输出报错，带颜色
        /// </summary>
        /// <param name="text">文本</param>
        /// <param name="consoleColor">颜色</param>
        public static void Error(string text, ConsoleColor? consoleColor = null)
        {
            if (consoleColor.HasValue)
            {
                Console.ForegroundColor = consoleColor.Value;
            }
            Console.WriteLine(text);
            if (consoleColor.HasValue)
            {
                Console.ResetColor();
            }
            Console.WriteLine("按任意键退出...");
            Console.ReadKey();
        }
    }
}
