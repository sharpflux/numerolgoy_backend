using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OmsSolution.Entities
{
    public class Products
    {
    }
    public class TableDataModel
    {
        public string ProductsPricingId { get; set; }
        public string ProductPricingName { get; set; }
        public List<ProductModel> Products { get; set; }
    }

    public class ProductModel
    {
        public int id { get; set; }
        public string price { get; set; }
    }
}
