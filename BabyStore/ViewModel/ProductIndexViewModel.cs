using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using BabyStore.Models;
using PagedList;

namespace BabyStore.ViewModel
{
    public class ProductIndexViewModel
    {

        public IPagedList<Product> Products { get; set; }
        //public IQueryable<Product> Products { get; set; }
        public string Search { get; set; }
        public IEnumerable<CategoryWithCounts> CategsWithCount { get; set; }
        public string Category { get; set; }
        public string SortBy { get; set; }
        public Dictionary<string, string> Sorts { get; set; }

        public IEnumerable<SelectListItem> CategFilterItems {
            get
            {
                var allCateg = CategsWithCount.Select(cc => new SelectListItem
                {
                    Value = cc.CategoryName,
                    Text = cc.CategNameWithCount
                }
                );
                return allCateg;
            }
        }
    }

    public class CategoryWithCounts
    {
        public int ProductCount { get; set; }
        public string CategoryName { get; set; }
        public string CategNameWithCount {
            get
            {
                return CategoryName + " (" + ProductCount.ToString() + ")";
            }            
        }
    }
}