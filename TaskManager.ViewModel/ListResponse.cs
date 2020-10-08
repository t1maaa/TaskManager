using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace TaskManager.ViewModel
{
    public class ListResponse<TItem> where TItem : class
    {
        public ICollection<TItem> Items { get; set; }
        public string Sort { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; } = 10;
        public int TotalItemsCount { get; set; }
        public int PagesCount
        {
            get
            {
                if (TotalItemsCount > 0)
                {
                   // int pagesCount = TotalItemsCount / PageSize;
                    return (TotalItemsCount % PageSize != 0)&& (TotalItemsCount > PageSize) 
                    ? TotalItemsCount / PageSize + 1
                    : TotalItemsCount / PageSize;
                }

                return 0;
            }
        }
    }
}
