using AstrophotoBG.Data.Models;
using AstrophotoBG.Data.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace AstrophotoBG.Data.Services
{
    public class PictureService : IPictureService
    {
        private readonly IRepository repo;
        private readonly UserManager<ApplicationUser> userManager;

        public PictureService(IRepository repo, UserManager<ApplicationUser> userManager)
        {
            this.repo = repo;
            this.userManager = userManager;
        }
        public async Task<byte[]> GetPictureDataByIdAsync(int id)
        {
            var picture = await repo.Db.Pictures
                .FirstOrDefaultAsync(x => x.Id == id);

            return picture.PictureData;
        }

        public async Task<Picture> GetPictureByIdAsync(int id)
        {
            return await repo.Db.Pictures
                .Include(x => x.User)
                .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
        }

        public async Task<int> IncrementLikesAsync(int pictureId, string userId)
        {
            var picture = await this.GetPictureByIdAsync(pictureId);
            var pictureUser = picture.User;
            pictureUser.Raiting++;
            var likes = ++picture.Likes;
            await this.repo.Db.UserPicture.AddAsync(new UserPicture()
            {
                PictureId = picture.Id,
                Picture = picture,
                UserId = userId,
                User = await this.userManager.FindByIdAsync(userId),
            });
            await this.repo.SaveDbChangesAsync();
            return likes;
        }

        public async Task<int> ReduceLikesAsync(int pictureId, string userId)
        {
            var picture = await this.GetPictureByIdAsync(pictureId);
            var pictureUser = picture.User;
            pictureUser.Raiting--;
            var likes = --picture.Likes;
            this.repo.Db.UserPicture
                .Remove(await this.repo.Db.UserPicture
                .SingleOrDefaultAsync(x => x.PictureId == pictureId && x.UserId == userId));

            await this.repo.SaveDbChangesAsync();
            return likes;
        }

        public async Task<bool> IsLiked(int pictureId, string userId)
        {
            return await this.repo.Db.UserPicture.AnyAsync(x => x.PictureId == pictureId && x.UserId == userId);
        }

        public async Task<int> CountPictures(string userId)
        {
            return await this.repo.Db.Pictures.CountAsync(p => p.User.Id == userId);
        }

        public async Task UpdatePicture(Picture picture)
        {
            var dbPicture = await this.repo.Db.Pictures.FirstOrDefaultAsync(p => p.Id == picture.Id && !p.IsDeleted);
            dbPicture.Name = picture.Name;
            dbPicture.Technique = picture.Technique;
            dbPicture.Description = picture.Description;
            await this.repo.SaveDbChangesAsync();
        }

        public async Task DeletePicture(Picture picture)
        {
            var dbPicture = await this.repo.Db.Pictures.FirstOrDefaultAsync(p => p.Id == picture.Id);
            dbPicture.IsDeleted = true;
            dbPicture.DeletedOn = DateTime.UtcNow;
            await this.repo.SaveDbChangesAsync();
        }
    }
}
