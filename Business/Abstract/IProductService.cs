﻿using Core.Utilities.Results;
using Entities.Concrate;
using Entities.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Abstract
{
   public interface IProductService
    {
        List<Product> GetAll();
        List<Product> GetAllByCategoryId( int id);
        List<Product> GetAllByUnitPrice(decimal min, decimal max);
        List<ProductDetailDto> GetProductDetails();
        Product GetById( int id);
        IResult Add(Product product);
    }
}
