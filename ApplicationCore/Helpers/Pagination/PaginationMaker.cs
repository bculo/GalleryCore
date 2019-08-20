using System;
using System.Collections.Generic;
using System.Linq;

namespace ApplicationCore.Helpers.Pagination
{
    public class PaginationMaker : IPaginationMaker
    {
        public int CheckPageLimits(int currentPage, int numberOfInstances, int pageSize, int lowestLimit = 1)
        {
            int maxPage = (int)Math.Ceiling(numberOfInstances / (decimal)pageSize);

            if (maxPage < currentPage)
            {
                currentPage = (maxPage == 0) ? 1 : maxPage;
            }
            else if (currentPage < lowestLimit)
            {
                currentPage = lowestLimit;
            }

            return currentPage;
        }

        public PaginationModel<DataType> PreparePaginationModel<DataType>(
            IEnumerable<DataType> data, 
            PaginationOptions opt) where DataType : class
        {
            //Create generic pagination model
            var pagModel = new PaginationModel<DataType>
            {
                Data = data,
                CurrentPage = opt.CurrentPage,
                TotalItems = opt.NumberOfItems,
                PageSize = opt.PageSize,
            };

            pagModel.TotalPages = (int)Math.Ceiling((decimal)pagModel.TotalItems / pagModel.PageSize);
            
            if(pagModel.TotalPages <= opt.ButtonToShow)
            {
                //case [ 1, 2, 3, 4, 5, 6, 7 ]
                //we have less then opt.ButtonToShow = 10, so we need to show buttons for all pages
                pagModel.StartPage = 1;
                pagModel.EndPage = pagModel.TotalPages;
            }
            else //case [ 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 ]
            {
                //show button for pages before currentpage
                var maxPagesBeforeCurrentPage = (int)Math.Floor((decimal)opt.ButtonToShow / 2);
                //show button for pages after currentpage
                var maxPagesAfterCurrentPage = (int)Math.Ceiling((decimal)opt.ButtonToShow / 2) - 1;

                // current page is 3
                // so maxPagesBeforeCurrentPage is ~ 5
                // result of 3 - 5 is -2 and we dont have a page with index -2, so we need to set start page on index = 1
                if (pagModel.CurrentPage <= maxPagesAfterCurrentPage)
                {
                    pagModel.StartPage = 1;
                    pagModel.EndPage = opt.ButtonToShow;
                }
                else if (pagModel.CurrentPage + maxPagesAfterCurrentPage >= pagModel.TotalPages) // current page near the end
                {
                    pagModel.StartPage = pagModel.TotalPages - opt.ButtonToShow + 1;
                    pagModel.EndPage = pagModel.TotalPages;
                }
                else // current page somewhere in the middle
                {
                    pagModel.StartPage = pagModel.CurrentPage - maxPagesBeforeCurrentPage;
                    pagModel.EndPage = pagModel.CurrentPage + maxPagesAfterCurrentPage;
                }
            }

            pagModel.Pages = Enumerable.Range(pagModel.StartPage, pagModel.EndPage);

            return pagModel;
        }
    }
}
