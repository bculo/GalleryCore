using System;
using System.Collections.Generic;
using System.Linq;

namespace ApplicationCore.Helpers.Pagination
{
    public class PaginationMaker<T> : IPaginationMaker<T> where T : class
    {
        protected bool PaginationCreated { get; set; } = false;
        protected int PageSize { get; set; }
        protected int MaxPagesToShow { get; set; }
        protected IPaginationModel<T> paginationModel;

        /// <summary>
        /// Throw InvalidOperationException if FillPaginationModel method is not called first
        /// </summary>
        public virtual IPaginationModel<T> Pagination
        {
            get
            {
                if (!PaginationCreated)
                {
                    throw new InvalidOperationException("IPaginationModel<T> is empty");
                }

                return paginationModel;
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="paginationModel"></param>
        public PaginationMaker(IPaginationModel<T> paginationModel) => this.paginationModel = paginationModel;

        /// <summary>
        /// Make pagination model based on pagination result
        /// </summary>
        /// <param name="paginationResult"></param>
        /// <returns></returns>
        public virtual IPaginationModel<T> FillPaginationModel(PaginationResult<T> paginationResult, int pageButtonsToShow = 5)
        {
            PaginationOptions options = paginationResult.Options;

            SetCommonValues(paginationResult.ResultSet, options.CurrentPage, options.NumberOfItems, options.PageSize, pageButtonsToShow);

            CreatePagination();

            return paginationModel;
        }

        /// <summary>
        /// Make pagination model based on data and pagination result
        /// This method doesnt use paginationResult.ResultSet
        /// </summary>
        /// <param name="data">Data to show</param>
        /// <param name="paginationResult">Instance of pagination result</param>
        /// <returns></returns>
        public IPaginationModel<T> FillPaginationModel(IEnumerable<T> data, PaginationOptions options, int pageButtonsToShow = 5)
        {
            SetCommonValues(data, options.CurrentPage, options.NumberOfItems, options.PageSize, pageButtonsToShow);

            CreatePagination();

            return paginationModel;
        }

        /// <summary>
        /// Create pagination (Calculation)
        /// </summary>
        protected virtual void CreatePagination()
        {
            paginationModel.LastPage = (int)Math.Ceiling(Decimal.Divide(paginationModel.NumberOfItems, PageSize));

            //check if user manually changed settings
            if (paginationModel.CurrentPage < 1)
            {
                paginationModel.CurrentPage = 1;
            }
            else if (paginationModel.CurrentPage > paginationModel.LastPage)
            {
                paginationModel.StartPageShow = 1;
            }

            if (paginationModel.LastPage <= MaxPagesToShow) //total pages less than max, show all pages
            {
                paginationModel.StartPageShow = 1;
                paginationModel.EndPageShow = paginationModel.LastPage;
            }
            else //more pages than max so calculate start and end pages
            {
                var maxPagesBeforeCurrentPage = (int)Math.Floor(MaxPagesToShow / (decimal)2);
                var maxPagesAfterCurrentPage = (int)Math.Ceiling(MaxPagesToShow / (decimal)2) - 1;

                if (paginationModel.CurrentPage <= maxPagesBeforeCurrentPage) //current page near the start
                {
                    paginationModel.StartPageShow = 1;
                    paginationModel.EndPageShow = MaxPagesToShow;
                }
                else if (paginationModel.CurrentPage + maxPagesAfterCurrentPage >= paginationModel.LastPage) //current page near the end
                {
                    paginationModel.StartPageShow = paginationModel.LastPage - MaxPagesToShow + 1;
                    paginationModel.EndPageShow = paginationModel.LastPage;
                }
                else //current page somewhere in the middle
                {
                    paginationModel.StartPageShow = paginationModel.CurrentPage - maxPagesBeforeCurrentPage;
                    paginationModel.EndPageShow = paginationModel.CurrentPage + maxPagesAfterCurrentPage;
                }
            }

            paginationModel.Pages = Enumerable.Range(paginationModel.StartPageShow, paginationModel.EndPageShow);
        }

        /// <summary>
        /// Set common values for pagination
        /// </summary>
        /// <param name="data">Data to show</param>
        /// <param name="currentPage">Current page</param>
        /// <param name="totalItems">Total items to show</param>
        /// <param name="pageSize">Number of items per page</param>
        /// <param name="maxPages">Max pages to choose</param>
        protected virtual void SetCommonValues(IEnumerable<T> data, int currentPage, int totalItems, int pageSize, int maxPages)
        {
            PaginationCreated = true;
            paginationModel.ResultsSet = data;
            paginationModel.CurrentPage = currentPage;
            paginationModel.NumberOfItems = totalItems;
            MaxPagesToShow = maxPages;
            PageSize = pageSize;
        }
    }
}
