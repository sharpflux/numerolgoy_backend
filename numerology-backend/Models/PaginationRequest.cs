using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OmsSolution.Models
{
    public class PaginationRequest
    {
        [Required]
        public string StartIndex { get; set; }

        [Required]
        public string PageSize { get; set; }

        [Required]
        public string SearchBy { get; set; }
        [Required]
        public string SearchCriteria { get; set; }

        public string SortColumn { get; set; }

        public string SortOrder { get; set; }
        public string ReducedDayTotal { get; set; }
    }

    public class OrdersFilterRequest
    {
        [Required]
        public string StartIndex { get; set; }

        [Required]
        public string PageSize { get; set; }

        [Required]
        public string SearchBy { get; set; }
        [Required]
        public string SearchCriteria { get; set; }

        public string SortColumn { get; set; }

        public string SortOrder { get; set; }


        public string? fromDate { get; set; }
        public string? toDate { get; set; }
        public string? owner_company_id { get; set; }
        public string? statusid { get; set; }
        public string? orderno { get; set; }

        public   bool IsCustomerAccess { get; set; }


    }
}
