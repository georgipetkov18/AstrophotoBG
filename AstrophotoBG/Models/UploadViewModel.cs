using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;

namespace AstrophotoBG.Models
{
    public class UploadViewModel
    {
        [Required]
        [Display(Name = "Име на снимка *")]
        public string Name { get; set; }

        [Display(Name = "Техника")]
        public string Technique { get; set; }

        [Display(Name = "Кратко описание")]
        public string Description { get; set; }

        [Display(Name = "Дата на заснемане")]
        public DateTime? Date { get; set; }

        [Display(Name = "Категория")]
        public string Category { get; set; }

        [Required]
        public IFormFile Data { get; set; }

        public string UserId { get; set; }
    }
}
