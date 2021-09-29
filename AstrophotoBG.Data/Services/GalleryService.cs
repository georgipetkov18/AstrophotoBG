using AstrophotoBG.Data.Models;
using AstrophotoBG.Data.Repository;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AstrophotoBG.Data.Services
{
    public class GalleryService : IGalleryService
    {
        private readonly IRepository repo;

        public GalleryService(IRepository repo)
        {
            this.repo = repo;
        }

        public async Task<IEnumerable<Picture>> GetPicturesAsync()
        {
            return await repo.Db.Pictures
                .Where(x => !x.IsDeleted)
                .Select(x => new Picture
                {
                    Id = x.Id,
                    Name = ReduceNameSize(x.Name),
                    Category = TranslateCategoryName(x.Category)
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<Picture>> GetPicturesAsync(int pageIndex, int pageSize, string filter)
        {
            List<Picture> pictures;
            if (filter == null)
            {
                pictures = await repo.Db.Pictures
                .OrderByDescending(x => x.CreatedOn)
                .Where(x => !x.IsDeleted)
                .Select(x => new Picture
                {
                    Id = x.Id,
                    Name = ReduceNameSize(x.Name),
                    Category = TranslateCategoryName(x.Category),
                    UserName = x.User.UserName,
                    CreatedOn = x.CreatedOn,
                    Likes = x.Likes
                })
                .Skip(pageIndex * pageSize).Take(pageSize)
                .ToListAsync();
            }

            else
            {
                pictures = await repo.Db.Pictures
                .OrderBy(x => x.CreatedOn)
                .Where(x => !x.IsDeleted)
                .Select(x => new Picture
                {
                    Id = x.Id,
                    Name = ReduceNameSize(x.Name),
                    Category = TranslateCategoryName(x.Category),
                    UserName = x.User.UserName,
                    CreatedOn = x.CreatedOn,
                    Likes = x.Likes
                })
                .Skip(pageIndex * pageSize).Take(pageSize)
                .ToListAsync();
            }

            return pictures;
        }

        public async Task<IEnumerable<Picture>> GetPicturesByCategoryNameAsync(int pageIndex, int pageSize, string categoryName, string filter)
        {
            var pictures = filter == null ? await repo.Db.Pictures
                .OrderByDescending(x => x.CreatedOn)
                .Where(x => !x.IsDeleted && x.Category.Name == categoryName)
                .Select(x => new Picture
                {
                    Id = x.Id,
                    Name = ReduceNameSize(x.Name),
                    Category = TranslateCategoryName(x.Category),
                    UserName = x.User.UserName,
                    CreatedOn = x.CreatedOn,
                    Likes = x.Likes
                })
                .Skip(pageIndex * pageSize).Take(pageSize)
                .ToListAsync() :

                await repo.Db.Pictures
                .OrderBy(x => x.CreatedOn)
                .Where(x => !x.IsDeleted && x.Category.Name == categoryName)
                .Select(x => new Picture
                {
                    Id = x.Id,
                    Name = ReduceNameSize(x.Name),
                    Category = TranslateCategoryName(x.Category),
                    UserName = x.User.UserName,
                    CreatedOn = x.CreatedOn,
                    Likes = x.Likes
                })
                .Skip(pageIndex * pageSize).Take(pageSize)
                .ToListAsync();

            return pictures;
        }

        public async Task<IEnumerable<Picture>> GetLastPicturesAsync(int count, string userId = null, int skipPictureId = -1)
        {
            var pictures = userId == null ? await repo.Db.Pictures
                .OrderByDescending(x => x.ModifiedOn)
                .Where(x => !x.IsDeleted)
                .Select(x => new Picture
                {
                    Id = x.Id,
                    Name = x.Name,
                    Category = x.Category,
                    UserName = x.User.UserName
                })
                .Take(count)
                .ToListAsync() :
                await repo.Db.Pictures
                .OrderByDescending(x => x.ModifiedOn)
                .Where(x => !x.IsDeleted && x.User.Id == userId && x.Id != skipPictureId)
                .Select(x => new Picture
                {
                    Id = x.Id,
                    Name = x.Name,
                    Category = x.Category,
                    UserName = x.User.UserName
                })
                .Take(count)
                .ToListAsync();

            return pictures;
        }

        private static string ReduceNameSize(string name)
        {
            if (name.Length > 15)
            {
                name = name.Substring(0, 12) + new string('.', 3);
            }

            return name;
        }

        private static Category TranslateCategoryName(Category category)
        {
            var name =  category.Name switch
            {
                "galaxies" => "Галактики",
                "planets" => "Планети",
                "star clusters" => "Звездни купове",
                "nebulas" => "Мъглявини",
                "sun" => "Слънце",
                "moon" => "Луна",
                "milky way" => "Млечен път",
                "stars" => "Звезди",
                "others" => "Други",
                _ => "all"
            };

            return new Category() { Name = name };
        }
    }
}
