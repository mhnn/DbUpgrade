using DbUp.Builder;
using DbUp.Engine;
using DbUp.Support;
using DbUpgrade.Models;

namespace DbUpgrade.Util
{
    public static class DbUpExtension
    {
        /// <summary>
        /// 设置数据库类型
        /// </summary>
        /// <param name="sd"></param>
        /// <param name="dbType">数据库类型</param>
        /// <param name="dbConn">连接字符串</param>
        /// <returns></returns>
        public static UpgradeEngineBuilder SetDb(this SupportedDatabases sd, string? dbType, string dbConn)
        {
            return dbType == DbType.MySql.ToString() ? sd.MySqlDatabase(dbConn) : sd.SqlDatabase(dbConn);
        }

        /// <summary>
        /// 如果是有版控的数据库，则更新版本号
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="hasVersionDbs">具有版控的数据库</param>
        /// <param name="dbIdentifier">数据库标识</param>
        /// <param name="nextVersion">新版本号</param>
        /// <returns></returns>
        public static UpgradeEngineBuilder WithVersionScript(this UpgradeEngineBuilder builder, string[] hasVersionDbs, string dbIdentifier, string nextVersion, Models.Environment? environment)
        {
            // dev环境/版本号为空/数据库没有字典表，不执行版本号更新
            if (environment == Models.Environment.Development || string.IsNullOrEmpty(nextVersion) || !hasVersionDbs.Contains(dbIdentifier))
            {
                return builder;
            }
            var contents = dbIdentifier switch
            {
                "medical" => $"update HospitalList set SystemVersion='{nextVersion}',KnowledgeBaseVersion='{nextVersion}';",
                _ => $"update HospitalList set SystemVersion='{nextVersion}';"
            };
            if (contents is not null)
            {
                builder.WithScript($"{nextVersion}.sql", contents, new SqlScriptOptions
                {
                    ScriptType = ScriptType.RunOnce,
                    RunGroupOrder = 9999
                });
            }
            return builder;
        }
    }
}
