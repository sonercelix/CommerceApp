using AutoMapper;
using CommerceWebApi.Entities;
using CommerceWebApi.Interfaces;
using CommerceWebApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CommerceWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CategoryController : ControllerBase
    {
        private IRepository<Category> repository;
        private readonly IMapper mapper;

        public CategoryController(IRepository<Category> repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        // GET: api/<CategoryController>
        [HttpGet]
        public IEnumerable<CategoryModel> Get()
        {
            return repository.TableNoTracking.Select(s => new CategoryModel()
            {
                Id = s.Id,
                Name = s.Name,
                ParentId = s.ParentId,
                ParentCategoryName = s.Parent != null ? s.Parent.Name : string.Empty
            }).OrderBy(p=>p.ParentCategoryName).ThenBy(c=>c.Name).ToList();
        }

        // GET api/<CategoryController>/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            Category category = repository.GetById(id);
            if (category == null)
            {
                return NotFound();
            }

            CategoryModel categoryModel = mapper.Map<CategoryModel>(category);
            return Ok(categoryModel);
        }

        // POST api/<CategoryController>
        [HttpPost]
        public IActionResult Post(CategoryModel categoryModel)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid data.");

            repository.Insert(new Category
            {
                Name = categoryModel.Name,
                ParentId = categoryModel.ParentId
            });

            return Ok();
        }

        // PUT api/<CategoryController>/5
        [HttpPut]
        public IActionResult Put(CategoryModel categoryModel)
        {
            Category category = repository.GetById(categoryModel.Id);
            if (category != null)
            {
                category.Name = categoryModel.Name;
                category.ParentId = categoryModel.ParentId;
                repository.Update(category);
            }
            else
            {
                return NotFound();
            }

            return Ok();
        }

        // DELETE api/<CategoryController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            if (id <= 0)
                return BadRequest("Not a valid category id");

            Category category = repository.GetById(id);
            if (category != null)
            {
                repository.Delete(category);
            }
            else
            {
                return NotFound();
            }

            return Ok();
        }
    }
}