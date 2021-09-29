using AstrophotoBG.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AstrophotoBG.Models
{
    public class UserPicturesViewModel
    {
        public string UserName { get; set; }
        public ICollection<Picture> Pictures { get; set; }
    }
}
