using DataAccess.Abstract;
using Entities.Concrate;
using Entities.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace DataAccess.Concrate.InMemory
{
    public class InMemoryProductDal : IProductDal
    {
        List<Product> _products;
        public InMemoryProductDal()
        {
            _products = new List<Product>();

            for (int i = 0; i < 15; i++)
            {
                var product = new Product()
                {
                    CategoryId = i,
                    ProductId = i * 2,
                    ProductName = "telefon" + i,
                    UnitPrice = 10 * i,
                    UnitsInStock = ((5 * i) - 2)

                };
                _products.Add(product);

            }
        }
        public void Add(Product entity)
        {
            _products.Add(entity);
        }

        public void Delete(Product entity)
        {
            var willDelete = _products.Where(x => x.ProductId == entity.ProductId).SingleOrDefault();

            _products.Remove(willDelete);

        }

        public Product Get(Expression<Func<Product, bool>> filter)
        {
            throw new NotImplementedException();
        }

        public List<Product> GetAll()
        {
            return _products;
        }

        public List<Product> GetAll(Expression<Func<Product, bool>> filter = null)
        {
            throw new NotImplementedException();
        }

        public List<ProductDetailDto> GetProductDetails()
        {
            throw new NotImplementedException();
        }

        public void Update(Product entity)
        {
            var willDelete = _products.Where(x => x.ProductId == entity.ProductId).SingleOrDefault();
            willDelete.ProductName = entity.ProductName+"Guncellendi";


        }
    }
}
