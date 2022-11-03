using alcolikLib.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace alcolikLib.Extensions
{
    public static class QueryExtensions
    {
        public static IOrderedQueryable<TModel> Sort<TModel>(this IQueryable<TModel> query, Params p)
        {
            if (!string.IsNullOrWhiteSpace(p.Asc))
            {
                string champ = p.Asc;
                //créer l'expression lambda
                var parameter = Expression.Parameter(typeof(TModel), champ);
                var property = Expression.Property(parameter, champ/*"Name"*/);
                var o = Expression.Convert(property, typeof(object));
                var lambda = Expression.Lambda<Func<TModel, object>>(o, parameter);
                
                return query.OrderBy(lambda);
            } 
            else 
                return (IOrderedQueryable<TModel>)query;
        }
    }
}
