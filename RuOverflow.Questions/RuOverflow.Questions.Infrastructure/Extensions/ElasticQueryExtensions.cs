using System.Linq.Expressions;
using Nest;

namespace RuOverflow.Questions.Infrastructure.Extensions
{
    public static class ElasticQueryExtensions
    {
        public static QueryContainer MatchMany<T, TValue>(this QueryContainerDescriptor<T> descriptor,
            Expression<Func<T, TValue>> objectPath, IEnumerable<string> values) where T : class
        {
            var result = new QueryContainer();
            foreach (var value in values)
            {
                result = result || descriptor.Match(s => s.Field(objectPath).Query(value));
            }

            return result;
        }
    }
}
