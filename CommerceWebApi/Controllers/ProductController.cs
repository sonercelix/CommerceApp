using AutoMapper;
using CommerceWebApi.Entities;
using CommerceWebApi.Interfaces;
using CommerceWebApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CommerceWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProductController : ControllerBase
    {
        private IRepository<Product> repository;
        private readonly ApplicationDbContext applicationDbContext;
        private readonly IMapper mapper;

        public ProductController(IRepository<Product> _repository, IMapper mapper, ApplicationDbContext applicationDbContext)
        {
            this.repository = _repository;
            this.applicationDbContext = applicationDbContext;
            this.mapper = mapper;
        }

        // GET: api/<ProductController>
        [HttpGet]
        public IEnumerable<ProductModel> Get()
        {
            List<ProductModel> result = new List<ProductModel>();
            var spProductItems = applicationDbContext.spProductItems.FromSqlRaw("exec sp_ProductList");
            foreach (var item in spProductItems)
            {
                result.Add(mapper.Map<ProductModel>(item));
            }
            return result;
        }

        // GET api/<ProductController>/5
        [HttpGet("{id}")]
        public ActionResult<ProductModel> Get(int id)
        {
            Product result = repository.TableNoTracking.Where(p => p.Id == id).Include(c => c.Category).FirstOrDefault();

            ProductModel product = mapper.Map<ProductModel>(result);

            if (result == null)
            {
                return NotFound();
            }
            return product;
        }

        // POST api/<ProductController>
        [HttpPost]
        public IActionResult Post(ProductModel productModel)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid data.");
            Product product = mapper.Map<Product>(productModel);
            repository.Insert(product);

            return Ok();
        }

        // PUT api/<ProductController>
        [HttpPut]
        public IActionResult Put(ProductModel productModel)
        {
            Product product = repository.GetById(productModel.Id);
            if (product != null)
            {
                string tempImage = product.ImageUrl;
                mapper.Map(productModel, product);
                if (string.IsNullOrEmpty(productModel.ImageUrl))
                { 
                    product.ImageUrl = tempImage; 
                }
                repository.Update(product);
            }
            else
            {
                return NotFound();
            }

            return Ok();
        }

        // DELETE api/<ProductController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            Product product = repository.GetById(id);
            if (product != null)
            {
                repository.Delete(product);
            }
        }
    }
}