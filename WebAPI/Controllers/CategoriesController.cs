using Business.Abstract;
using Microsoft.AspNetCore.Mvc;
using System;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
       private ICategoryService _categoryService;
        //loosely Coupled --Gevşek bağlılık.

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }



        [HttpGet("GetAll")]
        public IActionResult GetAll()
        {
            try
            {
                var result = _categoryService.GetAll();
                if (result.Success)
                {
                    return Ok(result);
                }
                else
                {
                    return BadRequest(result);
                }
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

    }
}
