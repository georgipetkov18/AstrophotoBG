using AstrophotoBG.Data.Models;
using System;
using System.Collections.Generic;

namespace AstrophotoBG.Models
{
    public class UserViewModel
    {
        public string UserName { get; set; }
        public string Location { get; set; }
        public string Description { get; set; }
        public int Rating { get; set; }
        public int PicturesCount { get; set; }
        public DateTime CreatedOn { get; set; }
        public ICollection<Picture> RecentPictures { get; set; }
    }
}
