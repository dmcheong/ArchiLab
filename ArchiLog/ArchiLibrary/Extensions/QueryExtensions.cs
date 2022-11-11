using System;
using System.Linq.Expressions;
using ArchiLibrary.Models;
using Microsoft.EntityFrameworkCore;

namespace ArchiLibrary.Extensions
{
    public static class QueryExtensions
    {
        public static IOrderedQueryable<TModel> SortAsc<TModel>(this IQueryable<TModel> query, Params p)
        {
            if (!string.IsNullOrWhiteSpace(p.Asc))
            {
                string champ = p.Asc;

                //créer lambda
                var parameter = Expression.Parameter(typeof(TModel), "x");
                var property = Expression.Property(parameter, champ/*"Name"*/);

                var o = Expression.Convert(property, typeof(object));
                var lambda = Expression.Lambda<Func<TModel, object>>(o, parameter);

                //utilisation lambda
                return query.OrderBy(lambda);
                //return query.OrderBy(x => x.Name);
            }
            else
                return (IOrderedQueryable<TModel>)query;

        }



        public static IOrderedQueryable<TModel> SortDsc<TModel>(this IQueryable<TModel> query, Params p)
        {
            if (!string.IsNullOrWhiteSpace(p.Desc))
            {
                string champ = p.Desc;

                //créer lambda
                var parameter = Expression.Parameter(typeof(TModel), "x");
                var property = Expression.Property(parameter, champ/*"Name"*/);

                var o = Expression.Convert(property, typeof(object));
                var lambda = Expression.Lambda<Func<TModel, object>>(o, parameter);

                //utilisation lambda
                return query.OrderByDescending(lambda);
                //return query.OrderBy(x => x.Name);
            }
            else
                return (IOrderedQueryable<TModel>)query;

        }



        public static IQueryable<TModel> PicByRange<TModel>(this IQueryable<TModel> query, Params p)
        {
            if (!string.IsNullOrWhiteSpace(p.Range))
            {
                string champ = p.Range;
                int start, end;
                //decoupage au niveau du tiret pour differencier le start et le end
                string[] subs = champ.Split('-');
                start = int.Parse(subs[0]);
                end = int.Parse(subs[1]);
                query = query.Skip(start).Take(end);
            }
            return query;

        }



        //public static IQueryable<TModel> GetByFields<TModel>(this IQueryable<TModel> query, Params p)
        //{
        //    if (!string.IsNullOrWhiteSpace(p.Fields))
        //    {
        //        string champ = p.Fields;
        //        string [] Fields = champ.Split(',');


       

        //        query = query.Select(x => x.Fields[0]);
        //    }
        //    return query;

        //}






        //    public static IQueryable<TModel> GetByType<TModel>(this IQueryable<TModel> query, Params p)
        //    {
        //        if (!string.IsNullOrWhiteSpace(p.Type))
        //        {
        //            string champ = p.Type;

        //            //créer lambda
        //            var parameter = Expression.Parameter(typeof(TModel), "x");
        //            var property = Expression.Property(parameter, champ/*"Name"*/);

        //            var o = Expression.Convert(property, typeof(object));
        //            var lambda = Expression.Lambda<Func<TModel, object>>(o, parameter);

        //            //utilisation lambda
        //            return query.Where(x => x.C == champ);
        //            //return query.OrderBy(x => x.Name);
        //        }
        //        else
        //            return (IOrderedQueryable<TModel>)query;

        //    }

    }

}