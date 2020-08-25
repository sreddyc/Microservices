using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace Application.Helpers
{
      public class PagedList<T> : List<T>
    {

        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }

        public PagedList(List<T> items, int count, int pageNumber, int pageSize)
        {
            TotalCount = count;
            PageSize = pageSize;
            CurrentPage = pageNumber;
            TotalPages = (int)Math.Ceiling((double) count / (double)pageSize);
            this.AddRange(items);
        }

        public static PagedList<T> CreateAsync(IQueryable<T> source, 
            int pageNumber, int pageSize)
        {
            var count = source.Count();
            var items = source.Skip((pageNumber) * pageSize).Take(pageSize).ToList();
            return new PagedList<T>(items, count, pageNumber, pageSize);
        }

         public static async Task<PagedList<T>> CreateAsync(IFindFluent<T,T> source, 
            int pageNumber, int pageSize)
        {
          //  var filtList = await source.ToListAsync();
         //   var count = filtList.Count;
            //var count = source.Count();
            //var items = source.Skip((pageNumber) * pageSize).Take(pageSize).ToList();
              var items = await source.Skip((pageNumber) * pageSize)
                                    .Limit(pageSize).ToListAsync();
            return new PagedList<T>(items, items.Count, pageNumber, pageSize);
        }
       
           
        
    }
}