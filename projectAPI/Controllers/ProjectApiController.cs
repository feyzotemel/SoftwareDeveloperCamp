using Business.Concrate;
using DataAccess.Concrate.EntityFramework;
using Entities.Concrate;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace project.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProjectApiController : ControllerBase
    {

        [HttpGet]
        public ActionResult Get()
        {
            try
            {
                return ProductTest();
               // ProductManager productManager = new ProductManager(new EfProductDal());
               // var product = new Product()
               // {
               //     CategoryId = 2,
               //     ProductId = 5,
               //     ProductName = "çorap",
               //     UnitPrice = 15,
               //     UnitsInStock = 5
               // };
               //var result =  productManager.Add(product);
               // if (result.Success)
               // {
               //     return Ok(result.Message);

               // }
               // else
               // {
               //     return BadRequest(result.Message);
               // }

               // return Ok("Ekleme işlemi Başarılı");
               // //return CategoryTest();

            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }

        }

        private ActionResult CategoryTest()
        {
            ///Burda hata olursa Videoda anlatılmayan birşey var. 
            ///MyFinalProject\DataAccess\Concrate\EntityFramework\EfCategoryDal.cs içindeki kısımları boşalt. 
            ///class ın aşağıdaki gibi olduğuna dikkat et
            ///public class EfCategoryDal : EfEntityRepositoryBase<Category, FinalProjectContext>, ICategoryDal
            CategoryManager categoryManager = new CategoryManager(new EfCategoryDal());
            List<Category> allCategories = categoryManager.GetAll();

            return Ok(allCategories);
        }

        private ActionResult ProductTest()
        {
            ProductManager productManager = new ProductManager(new EfProductDal());
            var result = productManager.GetProductDetails();
            if (result.Success)
            {
                return Ok(result.Data);

            }
            else
            {
                return BadRequest(result.Message);
            }
        }
    }
}
