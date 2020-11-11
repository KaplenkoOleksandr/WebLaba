using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;
using Microsoft.AspNetCore.Http;

namespace WebLaba.Models
{
    public class Teams
    {
        public Teams()
        {
            Players = new HashSet<Players>();
        }

        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Icon { get; set; }
        [Required]
        public int CorporationId { get; set; }

        [NotMapped]
        [Required]
        [DisplayName("Upload File")]
        public IFormFile ImageFile { get; set; }

        public virtual Corporations Corporation { get; set; }
        public virtual ICollection<Players> Players { get; set; }
    }
}

