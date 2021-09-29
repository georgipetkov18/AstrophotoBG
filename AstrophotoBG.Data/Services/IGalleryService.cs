using AstrophotoBG.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AstrophotoBG.Data.Services
{
    public interface IGalleryService
    {
        Task<IEnumerable<Picture>> GetPicturesAsync();

        Task<IEnumerable<Picture>> GetPicturesAsync(int blockNumber, int blockSize, string filter);

        Task<IEnumerable<Picture>> GetPicturesByCategoryNameAsync(int pageIndex, int pageSize, string categoryName, string filter);

        Task<IEnumerable<Picture>> GetLastPicturesAsync(int count, string userId = null, int skipPictureId = -1);
    }
}
