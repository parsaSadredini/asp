using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Common.Exceptions;
using Common.Utilities;
using Data.Repositories;
using Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shop.Model;
using Sservices;
using WebFramework.Api;
using WebFramework.Filters;

namespace Shop.Controllers
{
    [Route("api/[controller]")]
    [ApiResultFilterAttribute]
    [AllowAnonymous]
    [ApiController]
    public class UserController : ControllerBase
    {
     
        private readonly IUserRepository userRepository;
        IJwtService _jwtService;
        public UserController(IUserRepository userRepository,IJwtService jwtService)
        {
            this.userRepository = userRepository;
            this._jwtService = jwtService;
        }

        [HttpGet]
        [Authorize(Roles = "SeeUser")]
        public async Task<ApiResult<List<User>>> Get(CancellationToken cancellationToken)
        {
            var users = await userRepository.TableNoTracking.ToListAsync(cancellationToken);
            if (users == null)
                return NotFound();
            return users;    
        }

        [HttpGet("{id:int}")]
        [Authorize(Roles = "SeeUser")]
        public async Task<ApiResult<User>> Get(int id, CancellationToken cancellationToken)
        {
            var user = await userRepository.TableNoTracking.Include(x=>x.UserRoles).FirstOrDefaultAsync(x=>x.ID==id);
            if (user == null)
                return NotFound();
            return user;
        }

        [HttpPost]
        [Authorize(Roles = "AddUser")]
        public async Task<ApiResult<User>> Create(UserViewModel userDto, CancellationToken cancellationToken)
        {
            var exists = await userRepository.TableNoTracking.AnyAsync(p => p.UserName == userDto.UserName);
            if (exists)
                return BadRequest("نام کاربری تکراری است");

            var user = new User
            {
                Age = userDto.Age,
                Fullname = userDto.FullName,
                Gender = userDto.Gender,
                UserName = userDto.UserName
            };
            await userRepository.addUserByPassword(user, userDto.Password, cancellationToken);
            user.UserRoles = userDto.UserRoles.Select(x=> new UserRoles { RoleId = x.RoleId , UserId = user.ID}).ToList();
            await userRepository.UpdateAsync(user,cancellationToken);
            return user;
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "UpdateUser")]
        public async Task<ApiResult> Update(int id, UpdateUserViewModel user, CancellationToken cancellationToken)
        {
            var updateUser = await userRepository.Table.Include(x => x.UserRoles).FirstOrDefaultAsync(x => x.ID == id);
            updateUser.UserName = user.UserName;
            updateUser.Fullname = user.FullName;
            updateUser.UserRoles.Clear();
            updateUser.UserRoles = user.UserRoles;
     
            await userRepository.UpdateAsync(updateUser, cancellationToken);

            return Ok();
        }
        [Authorize(Roles = "RemoveUser")]
        [HttpDelete("{id}")]
        public async Task<ApiResult> Delete(int id, CancellationToken cancellationToken)
        {
            var user = await userRepository.GetByIdAsync(cancellationToken, id);
            await userRepository.DeleteAsync(user, cancellationToken);

            return Ok();
        }

        
        [HttpGet("[action]")]
        public async Task<ApiResult<UserLoginViewModel>> Login(string userName ,  string password,CancellationToken cancelationToken) 
        {

            var user = await userRepository.getByUserAndPass(userName, password, cancelationToken);
            if(user==null)
                throw new NotFoundException();

            string token = _jwtService.Generate(user);
            return new UserLoginViewModel
            {
                token = token,
                UserRoles = user.UserRoles
            };
        }
    }
}