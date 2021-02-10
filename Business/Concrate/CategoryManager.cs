using Business.Abstract;
using DataAccess.Abstract;
using Entities.Concrate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Business.Concrate
{
    public class CategoryManager : ICategoryService
    {
        ICategoryDal _categoryDal;

        public CategoryManager(ICategoryDal categoryDal)
        {
            _categoryDal = categoryDal;
        }

        //İş Kodları
        public List<Category> GetAll()
        {
            return _categoryDal.GetAll();
        }       

        public Category GetById(int categoryId)
        {
            return _categoryDal.Get(c=> c.CategoryId == categoryId);
        }
    }
}
