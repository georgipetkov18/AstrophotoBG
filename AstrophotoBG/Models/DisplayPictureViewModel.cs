using AstrophotoBG.Data.Models;
using System;
using System.Collections.Generic;

namespace AstrophotoBG.Models
{
    public class DisplayPictureViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public ApplicationUser User { get; set; }

        public byte[] PictureData { get; set; }

        public string Technique { get; set; }

        public DateTime? Date{ get; set; }

        public string Description { get; set; }

        public int Likes { get; set; }

        public bool IsLikedByCurrentUser { get; set; }

        public ICollection<Picture> RecentPictures { get; set; }
    }
}
