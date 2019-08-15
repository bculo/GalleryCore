using ApplicationCore.Interfaces;
using System;

namespace ApplicationCore.Helpers.Pagination
{
    public class PaginationChecker : IPaginationChecker
    {
        private int Top { get; set; }
        private int Down { get; set; }

        public int CheckPageLimits(int currentPage, int numberOfInstances, int pageSize, int lowestLimit = 1)
        {
            return CheckLimits(currentPage, numberOfInstances, pageSize, lowestLimit);
        }

        public int CheckPageLimits(int currentPage, int numberOfInstances, IPaginationService service, int lowestLimit = 1)
        {
            return CheckLimits(currentPage, numberOfInstances, service.PageSize, lowestLimit);
        }

        protected int CheckLimits(int currentPage, int numberOfInstances, int pageSize, int lowestLimit = 1)
        {
            Top = (int)Math.Ceiling(numberOfInstances / (decimal)pageSize);
            Down = lowestLimit;

            if (Top < currentPage)
            {
                currentPage = (Top == 0) ? 1 : Top;
            }
            else if (currentPage < lowestLimit)
            {
                currentPage = lowestLimit;
            }

            return currentPage;
        }
    }
}
