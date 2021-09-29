using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace AstrophotoBG.Data.Models
{
    public class Picture : BaseModel<int>
    {
        public string Name { get; set; }

        public byte[] PictureData { get; set; }

        public virtual ApplicationUser User { get; set; }
        [NotMapped]
        public string UserName { get; set; }

        public virtual Category Category { get; set; }

        public string Technique { get; set; }

        public DateTime? Date { get; set; }

        public string Description { get; set; }

        public int Likes { get; set; }

        public List<UserPicture> UserPictures { get; set; } = new List<UserPicture>();
    }
}
