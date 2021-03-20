using Business.Abstract;
using Business.BusinessAspects.Autofac;
using Business.Constants;
using Business.ValidationRules.FluentValidation;
using Core.Aspects.Autofac.Caching;
using Core.Aspects.Autofac.Performance;
using Core.Aspects.Autofac.Transaction;
using Core.Aspects.Autofac.Validation;
using Core.CrossCuttingConcerns.Validation;
using Core.Utilities.Business;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrate;
using Entities.DTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Business.Concrate
{
    public class ProductManager : IProductService
    {
        IProductDal _productDal;
        //**Yıldızlı uyarı** Bir entity manager kendisi haricindeki başka dalı enjekte edemez sadece servisi enjekte edebilir..
        //ICategoryDal _categoryDal;
        ICategoryService _categoryService;
        //public ProductManager(IProductDal productDal, ICategoryDal categoryDal)
        public ProductManager(IProductDal productDal, ICategoryService categoryService)
        {
            _productDal = productDal;
            //_categoryDal = categoryDal;
            _categoryService = categoryService;

        }
        //[SecuredOperation("product.add,admin")]
        [ValidationAspect(typeof(ProductValidator))]
        [CacheRemoveAspect("IProductService.Get")]
        public IResult Add(Product product)
        {
            IResult result = BusinessRules.Run(CheckProductCountByCategory(product.CategoryId),
                                                   CheckIfProductNameExist(product.ProductName),
                                                   CheckIfCategoryLimitExceded());

            //var categoryCheck = CheckProductCountByCategory(product.CategoryId).Success;
            //var productNameCheck = CheckIfProductNameExist(product.ProductName).Success;
            //if (categoryCheck && productNameCheck)
            if (result == null)
            {

                _productDal.Add(product);

                return new SuccessResult(Messages.ProductAdded);
            }
            return new ErrorResult(result.Message);



        }

        [CacheAspect]
        [PerformanceAspect(5)]

        public IDataResult<List<Product>> GetAll()
        {
            //return _productDal.GetAll();

            //return new DataResult<List<Product>>(_productDal.GetAll(),true,"Ürünler Listelendi");
            if (DateTime.Now.Hour == 15)
            {
                return new ErrorDataResult<List<Product>>(Messages.MaintenanceTime);
            }
            return new SuccessDataResult<List<Product>>(_productDal.GetAll(), Messages.ProductsListed);


        }

        public IDataResult<List<Product>> GetAllByCategoryId(int id)
        {
            return new SuccessDataResult<List<Product>>(_productDal.GetAll(c => c.CategoryId == id));
        }

        [CacheAspect]
        public IDataResult<Product> GetById(int id)
        {
            return new SuccessDataResult<Product>(_productDal.Get(p => p.ProductId == id));
        }
        public IDataResult<List<Product>> GetAllByUnitPrice(decimal min, decimal max)
        {
            return new SuccessDataResult<List<Product>>(_productDal.GetAll(c => c.UnitPrice >= min && c.UnitPrice <= max));
        }

        public IDataResult<List<ProductDetailDto>> GetProductDetails()
        {
            if (DateTime.Now.Hour == 23)
            {
                return new ErrorDataResult<List<ProductDetailDto>>(Messages.MaintenanceTime);
            }

            return new SuccessDataResult<List<ProductDetailDto>>(_productDal.GetProductDetails(), Messages.ProductsListed);
        }
        [ValidationAspect(typeof(ProductValidator))]
        [CacheRemoveAspect("IProductService.Get")]
        public IResult Update(Product product)
        {
            //Business da bir yöntemdir ama temiz kod Add methodundaki gibidir. 
            var categoryCheck = CheckProductCountByCategory(product.CategoryId).Success;
            var productNameCheck = CheckIfProductNameExist(product.ProductName).Success;
            var checkIfCategoryLimitExceded = CheckIfCategoryLimitExceded().Success;
            if (categoryCheck && productNameCheck && checkIfCategoryLimitExceded)
            {
                //Güncelle
                _productDal.Update(product);
                return new SuccessResult(Messages.ProductUpdated);
            }
            return new SuccessResult(Messages.ProductNotUpdated);

        }
        private IResult CheckProductCountByCategory(int categoryId)
        {
            if (_productDal.GetAll(x => x.CategoryId == categoryId).Count() >= 10)
            {
                return new ErrorResult(Messages.ProductCountOfCategoryError);
            }
            return new SuccessResult();
        }
        private IResult CheckIfProductNameExist(string productName)
        {
            if (_productDal.GetAll(x => x.ProductName == productName).Any())
            {
                return new ErrorResult(Messages.ProductNameExist);
            }
            return new SuccessResult();
        }
        private IResult CheckIfCategoryLimitExceded()
        {
            var result = _categoryService.GetAll();
            if (result.Data.Count() > 15)
            {
                return new ErrorResult(Messages.CategoryLimitExceded);
            }
            return new SuccessResult();
        }

        [TransactionScopeAspect]
        public IResult AddTransactionalTest(Product product)
        {
            var firstProduct = _productDal.Get(x=> x.ProductId == product.ProductId);
            firstProduct.ProductName = "Güncellenen " + firstProduct.ProductName;
           var result = Update(firstProduct);
            throw new NotImplementedException();

            firstProduct.ProductName = "Güncellenen2 " + firstProduct.ProductName;
            result = Update(firstProduct);
            return result;

        }
    }
}
