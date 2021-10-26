using Entities;

namespace Sservices
{
    public interface IJwtService
    {
        string Generate(User user);
    }
}