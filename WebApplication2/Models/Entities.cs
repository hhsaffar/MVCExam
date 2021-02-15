using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication2.Models
{

    public class Category
    {

    }


    public class Product
    {
        public int ProductId { get; set; }

        public string ProductName { get; set; }
        public string Category { get; set; }
        public decimal Price { get; set; }

        public List<ProductImage> Images { get; set; }

    }


    public class Cart
    {
        public int CartId { get; set; }

        public List<OrderLine> Orders { get; set; }
    }

    public class OrderLine
    {
        public int OrderLineId { get; set; }
        public int Count { get; set; }
        public Product OrderLineItem { get; set; }
    }

    public class ProductImage
    {
        public int ProductImageId { get; set; }
        public string Location { get; set; }

        public int ProductId { get; set; }

        public Product Product { get; set; }
    }

}