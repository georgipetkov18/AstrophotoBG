using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace AstrophotoBG.Data.Models
{
    public class ApplicationUser : IdentityUser, IEntity
    {
        public ApplicationUser()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        public DateTime CreatedOn { get; set; }

        public DateTime ModifiedOn { get; set; }

        public DateTime UsernameModifiedOn { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }

        public string Location { get; set; }

        public string ShortDescription { get; set; }

        public int Raiting { get; set; }

        public List<UserPicture> UserPictures { get; set; } = new List<UserPicture>();
    }
}
