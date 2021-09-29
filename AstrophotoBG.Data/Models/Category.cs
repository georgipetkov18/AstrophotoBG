using System.Collections.Generic;

namespace AstrophotoBG.Data.Models
{
    public class Category : BaseModel<int>
    {
        public Category()
        {
            this.Pictures = new HashSet<Picture>();
        }

        public string Name { get; set; }

        public virtual ICollection<Picture> Pictures { get; set; }
    }
}
