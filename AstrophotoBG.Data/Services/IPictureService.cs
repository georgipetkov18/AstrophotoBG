using AstrophotoBG.Data.Models;
using System.Threading.Tasks;

namespace AstrophotoBG.Data.Services
{
    public interface IPictureService
    {
        Task<Picture> GetPictureByIdAsync(int id);

        Task<byte[]> GetPictureDataByIdAsync(int id);

        Task<int> IncrementLikesAsync(int pictureId, string userId);

        Task<int> ReduceLikesAsync(int pictureId, string userId);

        Task<bool> IsLiked(int pictureId, string userId);

        Task<int> CountPictures(string userId);

        Task UpdatePicture(Picture picture);

        Task DeletePicture(Picture picture);
    }
}
