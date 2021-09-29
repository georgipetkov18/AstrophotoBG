using AstrophotoBG.Data.Models;
using System.Threading.Tasks;

namespace AstrophotoBG.Data.Services
{
    public interface ICategoryService
    {
        Task<Category> GetCategoryByNameAsync(string name);
    }
}
