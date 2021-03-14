using Business.Abstract;
using Entities.Concrate;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class ProductsController : ControllerBase
    {
        //loosely Coupled --Gevşek bağlılık.
        IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet("GetAll")]
        public IActionResult GetAll()
        {
            try
            {
                Thread.Sleep(1000);
                var result = _productService.GetAll();
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

        [HttpGet("GetById")]
        public IActionResult GetById(int id)
        {
            try
            {
                var result = _productService.GetById(id);
                if (result.Success)
                {
                    return Ok(result);
                }
                else
                {
                    return BadRequest(result.Message);
                }
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }
        
        [HttpGet("GetByCategory")]
        public IActionResult GetByCategory(int categoryId)
        {
            try
            {
                var result = _productService.GetAllByCategoryId(categoryId);
                if (result.Success)
                {
                    return Ok(result);
                }
                else
                {
                    return BadRequest(result.Message);
                }
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }


        [HttpPost("Add")]
        public IActionResult Add(Product product)
        {
            try
            {
                var result = _productService.Add(product);
                var products = new List<Product>();
                if (result.Success)
                {
                    return Ok(result);
                }
                else
                {
                    return BadRequest(result.Message);
                }

            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

        [HttpGet("AddTransactionalTest")]
        public IActionResult AddTransactionalTest()
        {
            var product = _productService.GetById(1).Data;

            var result = _productService.AddTransactionalTest(product);
            if (result.Success)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }
    }
}
