using alcolikLib.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace alcolikLib.Extensions
{
    public static class QueryExtensions
    {
        public static IQueryable<TModel> filter<TModel>(this IQueryable<TModel> query, Params p, Dictionary<string, string> arrayProperties)
        {
            //arrayProperties = arrayProperties ?? throw new ArgumentNullException(nameof(arrayProperties));
            BinaryExpression binaryExpression = null;
            var parameter = Expression.Parameter(typeof(TModel), "x");
            foreach (var item in arrayProperties)
            {
                if (!string.IsNullOrWhiteSpace(item.Key) && !string.IsNullOrWhiteSpace(item.Value))
                {
                    var key = item.Key;
                    var value = item.Value;
                    var c = Expression.Constant(value);
                    var property = Expression.Property(parameter, key /*"Name"*/);
                    var o = Expression.Convert(property, typeof(object));
                    var lambda = Expression.Equal(o, c);
                    if(binaryExpression == null)
                    {
                        binaryExpression = lambda;
                    } 
                    else 
                        binaryExpression = Expression.And(binaryExpression, lambda);
                    // var equal = Expression.Equal(olamba,value);
                    //return await _context.Set<TModel>().Where(x => x.Name.Contains(param.Name)).ToListAsync();

                    Console.WriteLine("hey");
                }
            }
            if(binaryExpression != null)
            {
                return query.Where(Expression.Lambda<Func<TModel, bool>>(binaryExpression, parameter));
            } 
            else 
                return (IQueryable<TModel>)query;
        }

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
            else if (!string.IsNullOrWhiteSpace(p.Desc))
            {
                string champ = p.Desc;
                //créer l'expression lambda
                var parameter = Expression.Parameter(typeof(TModel), champ);
                var property = Expression.Property(parameter, champ /* "Name" */);
                var o = Expression.Convert(property, typeof(object));
                var lambda = Expression.Lambda<Func<TModel, object>>(o, parameter);

                return query.OrderByDescending(lambda);
            }
            else 
                return (IOrderedQueryable<TModel>)query;
        }

        public static IOrderedQueryable<TModel> Pagination<TModel>(this IQueryable<TModel> query, int start, int end)
        {
            return (IOrderedQueryable<TModel>)query.Skip(start).Take((end - start) + 1);
            //return query.OrderBy(x => x.Name);
        }
    }
}
