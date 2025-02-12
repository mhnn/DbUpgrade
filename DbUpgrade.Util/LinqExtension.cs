using System.Linq.Expressions;

namespace DbUpgrade.Util
{
    public static class LinqExtension
    {
        /// <summary>
        /// Linq-Where扩展，第一个参数为True时，才会拼接第二个条件
        /// Example：list.Where(m => !string.IsNullOrEmpty(AssessListGroupID), m => m.AssessListGroupID == assessListGroupID)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="condition">拼接条件</param>
        /// <param name="predicate">查询条件</param>
        /// <returns></returns>
        public static IEnumerable<T> IfWhere<T>(this IEnumerable<T> query,
            bool condition, Func<T, bool> predicate)
        {
            return condition ? query.Where(predicate) : query;
        }
        public static IQueryable<T> IfWhere<T>(this IQueryable<T> query,
            bool condition, Expression<Func<T, bool>> predicate)
        {
            return condition ? query.Where(predicate) : query;
        }
    }
}
