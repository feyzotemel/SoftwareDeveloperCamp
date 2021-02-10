using Core.DataAccess.EntityFramework;
using DataAccess.Abstract;
using Entities.Concrate;
using Entities.DTOs;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace DataAccess.Concrate.EntityFramework
{
    public class EfProductDal : EfEntityRepositoryBase<Product, FinalProjectContext>, IProductDal
    {
        public List<ProductDetailDto> GetProductDetails()
        {
            using (FinalProjectContext context = new FinalProjectContext())
            {
                var result = from p in context.Products
                             join c in context.Categories
                             on p.CategoryId equals c.CategoryId
                             select new ProductDetailDto
                             {
                                 CategoryName = c.CategoryName,
                                 ProductId = p.ProductId,
                                 ProductName = p.ProductName,
                                 UnitsInStock = p.UnitsInStock

                             };
                return result.ToList();
            }


        }
    }
}
