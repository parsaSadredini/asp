using Common.Utilities;
using Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<User> getByUserAndPass(string Username, string Password, CancellationToken token)
        {
            string hashPassword = SecurityHelper.GetSha256Hash(Password);
            return await Table.Include(x => x.UserRoles).ThenInclude(x=>x.Role).Where(x => x.UserName == Username && x.PasswordHash == hashPassword).SingleOrDefaultAsync();
        }
        public async Task addUserByPassword(User model, string Password , CancellationToken token)
        {
            
            string hashPassword = SecurityHelper.GetSha256Hash(Password);
            model.PasswordHash = hashPassword;
            await base.AddAsync(model,token);
        }

        public Task UpdateLastLoginDateAsync(User user, CancellationToken cancellationToken)
        {
            user.LastDateLogin = DateTimeOffset.Now;
            return UpdateAsync(user, cancellationToken);
        }
    }
}
