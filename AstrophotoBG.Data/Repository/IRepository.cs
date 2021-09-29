using System.Threading.Tasks;

namespace AstrophotoBG.Data.Repository
{
    public interface IRepository
    {
        ApplicationDbContext Db { get; }

        void Migrate();

        Task SaveDbChangesAsync();
    }
}
