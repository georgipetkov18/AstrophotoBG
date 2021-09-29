using System;
using System.ComponentModel.DataAnnotations;

namespace AstrophotoBG.Data.Models
{
    public abstract class BaseModel<T> : IEntity
    {
        [Key]
        public T Id { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime ModifiedOn { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }
    }
}
