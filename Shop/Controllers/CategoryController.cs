using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Common.Exceptions;
using Data.Repositories;
using Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebFramework.Api;
using WebFramework.Filters;

namespace Shop.Controllers
{
    [Route("api/[controller]")]
    [ApiResultFilterAttribute]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly IRepository<Category> _categoryRepository;
        public CategoryController(IRepository<Category> categoryRepository)
        {
            this._categoryRepository = categoryRepository;
        }
        [HttpPost]
        [Authorize(Roles = "AddCategory")]
        public async Task<ApiResult<Category>> Create(Category category,CancellationToken cancellationToken)
        {
            await _categoryRepository.AddAsync(category, cancellationToken);
            return category;
        }

        [HttpGet("{id:int}")]
        [Authorize(Roles = "SeeCategory")]
        public async Task<ApiResult<Category>> Get( int id, CancellationToken cancellationToken)
        {
            
             var category = await _categoryRepository.GetByIdAsync( cancellationToken, id);
            if (category == null)
                //return NotFound();
                throw new NotFoundException();

            return category;
        }

        [HttpGet]
        [Authorize(Roles = "SeeCategory")]
        public async Task<ApiResult<List<Category>>> Get(CancellationToken cancellationToken)
        {
            var categories = await _categoryRepository.TableNoTracking.ToListAsync(cancellationToken);
            return categories;
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "UpdateCategory")]
        public async Task<ApiResult> Update(int id  , Category category , CancellationToken cancellationToken)
        {
            var updatingCategory = await _categoryRepository.GetByIdAsync(cancellationToken, id);
            if (updatingCategory == null)
                return NotFound();

            updatingCategory.CategoryID = category.CategoryID;
            updatingCategory.Title = category.Title;

            await _categoryRepository.UpdateAsync(updatingCategory, cancellationToken);
            return Ok();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "RemoveCategory")]
        public async Task<ApiResult> Delete(int id,CancellationToken cancellationToken)
        {
            var category = await _categoryRepository.GetByIdAsync(cancellationToken, id);
            if (category == null)
                return NotFound();

            await _categoryRepository.DeleteAsync(category,cancellationToken);
            return Ok();
        }
    }
}