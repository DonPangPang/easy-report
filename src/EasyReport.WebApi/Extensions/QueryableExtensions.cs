
using EasyReport.Infrastructure.Dto;
using EasyReport.Infrastructure.GeneralModel;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace EasyReport.WebApi.Extensions;

public static class QueryableExtensions
{
    public static IQueryable<T> ApplyOrdering<T>(this IQueryable<T> query, IOrderQueryParameter parameter)
    {
        if (string.IsNullOrWhiteSpace(parameter.OrderBy))
        {
            return query;
        }

        var orderParams = parameter.OrderBy.Split(',');
        var propertyInfos = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
        var orderQueryBuilder = new StringBuilder();

        foreach (var param in orderParams)
        {
            if (string.IsNullOrWhiteSpace(param))
            {
                continue;
            }

            var propertyFromQueryName = param.Split(" ")[0];
            var objectProperty = propertyInfos.FirstOrDefault(pi =>
                               pi.Name.Equals(propertyFromQueryName, StringComparison.InvariantCultureIgnoreCase));

            if (objectProperty == null)
            {
                continue;
            }

            var direction = param.EndsWith(" desc") ? "descending" : "ascending";

            orderQueryBuilder.Append($"{objectProperty.Name} {direction}, ");
        }

        var orderQuery = orderQueryBuilder.ToString().TrimEnd(',', ' ');

        if (string.IsNullOrWhiteSpace(orderQuery))
        {
            return query;
        }

        return query.OrderBy(orderQuery);
    }

    public static async Task<PagedList<T>> ToListAsync<T>(this IQueryable<T> query, IQueryParameter parameter)
    {

        if (parameter is IOrderQueryParameter orderQueryParameter)
        {
            query = query.ApplyOrdering(orderQueryParameter);
        }

        if (parameter is IPagedQueryParameter pagingQueryParameter)
        {
            return await PagedList<T>.CreateAsync(query, pagingQueryParameter.PageIndex, pagingQueryParameter.PageSize);
        }
        else
        {
            return await PagedList<T>.CreateAsync(query, 1, int.MaxValue);
        }
    }

    public static IQueryable<T> WhereIf<T>(this IQueryable<T> query, bool condition,
        Expression<Func<T, bool>> predicate)
    {
        return condition ? query.Where(predicate) : query;
    }
}