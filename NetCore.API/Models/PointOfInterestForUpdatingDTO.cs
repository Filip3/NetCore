using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NetCore.API.Models
{
    public class PointOfInterestForUpdatingDTO
    {
        [Required(ErrorMessage = "No Name provided.")]
        [MaxLength(50)]
        public string Name { get; set; }
        [MaxLength(150)]
        public string Description { get; set; }
    }
}
