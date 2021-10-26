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
using WebFramework.Api;
using WebFramework.Filters;

namespace Shop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiResultFilterAttribute]
    public class RoleController : ControllerBase
    {
        private readonly IRepository<Role> _roleRepository;
        public RoleController(IRepository<Role> roleRepository)
        {
            _roleRepository = roleRepository;
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "SeeRole")]
        public async Task<ApiResult<Role>> Get(Guid id, CancellationToken cancellationToken)
        {
            var product = await _roleRepository.GetByIdAsync(cancellationToken, id);
            if (product == null)
                return NotFound();

            return product;
        }
        [HttpGet]
        [Authorize(Roles = "SeeRole")]
        public async Task<ApiResult<List<Role>>> Get(CancellationToken cancellationToken)
        {
            return await _roleRepository.TableNoTracking.ToListAsync(cancellationToken);
        }
        [HttpPost]
        [Authorize(Roles = "AddRole")]
        public async Task<ApiResult<Role>> Create( Role role, CancellationToken cancellationToken)
        {
            await _roleRepository.AddAsync(role, cancellationToken);
            return role;
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "UpdateRole")]
        public async Task<ApiResult> Update(int id, Role role, CancellationToken cancellationToken)
        {
            var updatingRole = await _roleRepository.GetByIdAsync(cancellationToken, id);
            if (updatingRole == null)
                return NotFound();

            updatingRole.Name = role.Name;
            updatingRole.Description = role.Description;

            await _roleRepository.UpdateAsync(updatingRole, cancellationToken);
            return Ok();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "RemoveRole")]
        public async Task<ApiResult> Delete(int id, CancellationToken cancellationToken)
        {
            var role = await _roleRepository.GetByIdAsync(cancellationToken, id);
            if (role == null)
                return NotFound();

            await _roleRepository.DeleteAsync(role, cancellationToken);
            return Ok();
        }



    }
}