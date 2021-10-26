using Entities;
using System.Threading;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public interface IUserRepository:IRepository<User>
    {
        Task<User> getByUserAndPass(string Username, string Password, CancellationToken token);
        Task addUserByPassword(User model, string Password, CancellationToken token);
        Task UpdateLastLoginDateAsync(User user, CancellationToken cancellationToken);

    }
}