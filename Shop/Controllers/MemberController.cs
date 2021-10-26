using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Common.Utilities;
using Data;
using Data.Repositories;
using Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shop.Model;
using WebFramework.Api;
using WebFramework.Filters;

namespace Shop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiResultFilterAttribute]
    public class MemberController : ControllerBase
    {
        IRepository<Member> _memberRepository;
        IRepository<Category> _categoryRepository;
        public MemberController(IRepository<Member> memberRepository , IRepository<Category> categoryRepository )
        {
            _memberRepository = memberRepository;
            _categoryRepository = categoryRepository;
        }

        [HttpPost("[Action]")]
        public async Task<ApiResult<Member>> SigUp(MemberViewModel memberViewModel , CancellationToken cancellationToken)
        {
            
            var exists = await _memberRepository.TableNoTracking.AnyAsync(x => x.UserName == memberViewModel.UserName);
            if (exists)
                return BadRequest("نام کاربری تکراریست");

            string hashPassword = SecurityHelper.GetSha256Hash(memberViewModel.Password);
            var member = new Member { PasswordHash = hashPassword, UserName = memberViewModel.UserName };
            await _memberRepository.AddAsync(member, cancellationToken);
            return member;
        }

        [HttpPost("[Action]")]
        public async Task<ApiResult<Member>> SigIn(MemberViewModel memberViewModel, CancellationToken cancellationToken)
        {
            string hashPassword = SecurityHelper.GetSha256Hash(memberViewModel.Password);

            var member = await _memberRepository.TableNoTracking.Where(x => x.UserName == memberViewModel.UserName && x.PasswordHash == hashPassword).FirstOrDefaultAsync();
            return member;
        }

        [HttpGet("[action]")]
        public async Task<List<Category>> GetGetegories(CancellationToken cancellationToken)
        {
            return await _categoryRepository.TableNoTracking.ToListAsync(cancellationToken);
        }

        [HttpGet("[action]")]
        public List<Product> GetProductByCategoryId(int id ,CancellationToken cancellationToken)
        {
            return _categoryRepository.TableNoTracking.Include(x=>x.Products).FirstOrDefaultAsync(x => x.ID == id).Result.Products.ToList();

            //return _categoryRepository.TableNoTracking.Where(x => x.ID == id).Include(x => x.Products).FirstOrDefault().Products.ToList();
        }
    
    }
}