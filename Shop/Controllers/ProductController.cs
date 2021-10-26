using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Data.Repositories;
using Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shop.Model;
using Sservices.JwtServices;
using WebFramework.Api;
using WebFramework.Filters;

namespace Shop.Controllers
{
    [Route("api/[controller]")]
    [ApiResultFilterAttribute]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IRepository<Product> _productRepository;
        private readonly IPictureServices _pictureServices;
        public ProductController(IRepository<Product> productRepository , IPictureServices pictureServices)
        {
            _productRepository = productRepository;
            _pictureServices = pictureServices;
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "SeeProduct")]
        public async Task<ApiResult<Product>> Get(Guid id, CancellationToken cancellationToken)
        {
            var product = await _productRepository.GetByIdAsync(cancellationToken, id);
            if (product == null)
                return NotFound();

            return product;
        }
        [HttpGet]
        [Authorize(Roles = "SeeProduct")]
        public async Task<ApiResult<List<Product>>> Get(CancellationToken cancellationToken)
        {
            return await _productRepository.TableNoTracking.ToListAsync(cancellationToken);
        }
        [HttpPost]
        [Authorize(Roles = "AddProduct")]
        public async Task<ApiResult<Product>> Create([FromForm] ProductViewModel productViewModel ,CancellationToken cancellationToken)
        {
            var file = Request.Form.Files.GetFile("file");
            string imageUrl = _pictureServices.uploadFile(file);
      
            var product = new Product
            {
                CategoryID = productViewModel.CategoryID,
                OperatorID = productViewModel.OperatorID,
                price = productViewModel.price,
                name = productViewModel.name,
                imageUrl = imageUrl
            };

            await _productRepository.AddAsync(product, cancellationToken);
            return product;
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "UpdateProduct")]
        public async Task<ApiResult> Update(Guid id, [FromForm] ProductViewModel product, CancellationToken cancellationToken)
        {
            var updatingProduct = await _productRepository.GetByIdAsync(cancellationToken, id);
            if (updatingProduct == null)
                return NotFound();

            updatingProduct.CategoryID = product.CategoryID;
            updatingProduct.name = product.name;
            updatingProduct.OperatorID = product.OperatorID;
            updatingProduct.price = product.price;

            var file = Request.Form.Files.GetFile("file");
            if (file != null) { 
                string imageUrl = _pictureServices.uploadFile(file);
                updatingProduct.imageUrl = imageUrl;
            }
            await _productRepository.UpdateAsync(updatingProduct, cancellationToken);
            return Ok();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "RemoveProduct")]
        public async Task<ApiResult> Delete(Guid id, CancellationToken cancellationToken)
        {
            var product = await _productRepository.GetByIdAsync(cancellationToken, id);
            if (product == null)
                return NotFound();

            await _productRepository.DeleteAsync(product, cancellationToken);
            return Ok();
        }

    }
}