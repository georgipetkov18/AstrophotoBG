using System.ComponentModel.DataAnnotations;

namespace AstrophotoBG.Models
{
    public class UpdatePictureViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Име на снимка")]
        public string Name { get; set; }

        [Display(Name = "Техника")]
        public string Technique { get; set; }

        [Display(Name = "Кратко описание")]
        public string Description { get; set; }
    }
}
