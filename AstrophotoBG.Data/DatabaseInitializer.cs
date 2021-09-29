using AstrophotoBG.Data.Enumerations;
using AstrophotoBG.Data.Models;
using AstrophotoBG.Data.Repository;
using Microsoft.AspNetCore.Identity;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AstrophotoBG.Data
{
    public class DatabaseInitializer
    {
        public async static Task Initialize(IRepository repo)
        {
            repo.Db.Database.EnsureCreated();
            if (!repo.Db.Categories.Any())
            {
                var categories = Enum.GetValues(typeof(CategoryList)).Cast<CategoryList>().ToList();
                for (int i = 0; i < categories.Count(); i++)
                {
                    await repo.Db.Categories.AddAsync(new Category
                    {
                        Name = categories[i].ToDescriptionString(),
                        CreatedOn = DateTime.UtcNow,
                        ModifiedOn = DateTime.UtcNow,
                        IsDeleted = false
                    });
                }
                await repo.SaveDbChangesAsync();
            }

            if (!repo.Db.Roles.Any())
            {
                await repo.Db.Roles.AddAsync(new IdentityRole
                {
                    Name = Roles.Administrator.ToString(),
                    NormalizedName = Roles.Administrator.ToString().ToUpper(),
                    ConcurrencyStamp = Guid.NewGuid().ToString()
                });
                await repo.SaveDbChangesAsync();
            }
        }
    }
}
