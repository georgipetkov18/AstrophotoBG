using AstrophotoBG.Data.Models;
using AstrophotoBG.Data.Repository;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace AstrophotoBG.Data.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IRepository repo;

        public CategoryService(IRepository repo)
        {
            this.repo = repo;
        }

        public async Task<Category> GetCategoryByNameAsync(string name)
        {
            return await this.repo.Db.Categories
                .FirstOrDefaultAsync(x => x.Name == name.ToLower() && !x.IsDeleted);
        }
    }

}
