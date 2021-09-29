using AstrophotoBG.Data.Models;
using AstrophotoBG.Data.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AstrophotoBG.Data.Services
{
    public class UserService : IUserService
    {
        private readonly IRepository repo;

        public UserService(IRepository repo)
        {
            this.repo = repo;
        }

        public async Task DeleteUserAsync(string userName)
        {
            var user = await this.repo.Db.Users.FirstOrDefaultAsync(u => u.UserName == userName);
            user.IsDeleted = true;
            user.DeletedOn = DateTime.UtcNow;
            var userPictures = await this.repo.Db.Pictures.Where(p => p.User.UserName == userName).ToListAsync();
            foreach (var picture in userPictures)
            {
                picture.IsDeleted = true;
                picture.DeletedOn = DateTime.UtcNow;
            }
            var likedPictures = await this.repo.Db.UserPicture.Where(x => x.UserId == user.Id).ToListAsync();
            this.repo.Db.UserPicture.RemoveRange(likedPictures);
            await this.repo.SaveDbChangesAsync();
        }

        public async Task<ApplicationUser> ExternalRegisterAsync(string providerKey, string providerName, string fullName, string email)
        {
            var user = new ApplicationUser
            {
                UserName = fullName,
                Email = email,
                CreatedOn = DateTime.UtcNow,
                ModifiedOn = DateTime.UtcNow,
                UsernameModifiedOn = DateTime.UtcNow
            };

            var externalLogin = new IdentityUserLogin<string>
            {
                ProviderKey = providerKey,
                LoginProvider = providerName,
                ProviderDisplayName = providerName,
                UserId = user.Id,
            };

            await this.repo.Db.Users.AddAsync(user);
            await this.repo.Db.UserLogins.AddAsync(externalLogin);
            await this.repo.SaveDbChangesAsync();
            return user;
        }

        public async Task<ApplicationUser> GetUserAsync(string userName)
        {
            var user = await this.repo.Db.Users
                .SingleOrDefaultAsync(u => u.UserName == userName && !u.IsDeleted);

            return user;
        }

        public async Task<bool> IsRegistered(string email)
        {
            return await this.repo.Db.Users.AnyAsync(u => u.Email == email);
        }
    }
}
