using Business.Abstract;
using Business.Constants;
using Business.ValidationRules.FluentValidation;
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
        [ValidationAspect(typeof(ProductValidator))]
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

                return new SucessResult(Messages.ProductAdded);
            }
            return new ErrorResult(result.Message);



        }

        public IDataResult<List<Product>> GetAll()
        {
            //return _productDal.GetAll();

            //return new DataResult<List<Product>>(_productDal.GetAll(),true,"Ürünler Listelendi");
            if (DateTime.Now.Hour == 1)
            {
                return new ErrorDataResult<List<Product>>(Messages.MaintenanceTime);
            }
            return new SuccessDataResult<List<Product>>(_productDal.GetAll(), Messages.ProductsListed);


        }

        public IDataResult<List<Product>> GetAllByCategoryId(int id)
        {
            return new SuccessDataResult<List<Product>>(_productDal.GetAll(c => c.CategoryId == id));
        }

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
        public IResult Update(Product product)
        {
            //Business da bir yöntemdir ama temiz kod Add methodundaki gibidir. 
            var categoryCheck = CheckProductCountByCategory(product.CategoryId).Success;
            var productNameCheck = CheckIfProductNameExist(product.ProductName).Success;
            var checkIfCategoryLimitExceded = CheckIfCategoryLimitExceded().Success;
            if (categoryCheck && productNameCheck && checkIfCategoryLimitExceded)
            {
                //Güncelle
            }
            throw new NotImplementedException();
        }
        private IResult CheckProductCountByCategory(int categoryId)
        {
            if (_productDal.GetAll(x => x.CategoryId == categoryId).Count() >= 10)
            {
                return new ErrorResult(Messages.ProductCountOfCategoryError);
            }
            return new SucessResult();
        }
        private IResult CheckIfProductNameExist(string productName)
        {
            if (_productDal.GetAll(x => x.ProductName == productName).Any())
            {
                return new ErrorResult(Messages.ProductNameExist);
            }
            return new SucessResult();
        }
        private IResult CheckIfCategoryLimitExceded()
        {
            var result = _categoryService.GetAll();
            if (result.Data.Count() > 15)
            {
                return new ErrorResult(Messages.CategoryLimitExceded);
            }
            return new SucessResult();
        }
    }
}
