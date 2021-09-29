using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace AstrophotoBG.Data.Repository
{
    public class Repository : IRepository
    {
        public ApplicationDbContext Db { get; set; }

        public Repository(ApplicationDbContext Db)
        {
            this.Db = Db;
        }

        public void Migrate()
        {
            this.Db.Database.Migrate();
        }

        public async Task SaveDbChangesAsync()
        {
            await this.Db.SaveChangesAsync();
        }
    }
}
