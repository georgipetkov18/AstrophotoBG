using AstrophotoBG.Data.Models;
using System.Threading.Tasks;

namespace AstrophotoBG.Data.Services
{
    public interface IUserService
    {
        Task<ApplicationUser> GetUserAsync(string userName);
        Task DeleteUserAsync(string userName);
        Task<ApplicationUser> ExternalRegisterAsync(string providerKey, string providerName, string fullName, string email);
        Task<bool> IsRegistered(string email);
    }
}
