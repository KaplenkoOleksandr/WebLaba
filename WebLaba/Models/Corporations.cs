using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;
using WebLaba.Models;
using System.Collections;

namespace WebLaba.Models
{
    public class Corporations
    {
        public Corporations()
        {
            Teams = new HashSet<Teams>();
        }

        [Key]
        [Required]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        [DisplayName("Emblem Name")]
        public string Emblem { get; set; }

        public virtual ICollection<Teams> Teams { get; set; }

        [NotMapped]
        [Required]
        [DisplayName("Upload File")]

        public IFormFile ImageFile { get; set; }
    }
}
