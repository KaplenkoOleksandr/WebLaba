using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace WebLaba.Models
{
    public class Players
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Nickname { get; set; }
        [Required]
        public string Photo { get; set; }
        [Required]
        public int TeamId { get; set; }
        [NotMapped]
        [Required]
        [DisplayName("Upload File")]
        public IFormFile ImageFile { get; set; }
        public virtual Teams Team { get; set; }
    }
}

