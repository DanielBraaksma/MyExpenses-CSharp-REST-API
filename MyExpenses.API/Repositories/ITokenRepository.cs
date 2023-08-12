using Microsoft.AspNetCore.Identity;

namespace MyExpenses.API.Repositories
{
    public interface ITokenRepository
    {
        string CreateJWTToken(IdentityUser user, List<string> roles);
    }
}
