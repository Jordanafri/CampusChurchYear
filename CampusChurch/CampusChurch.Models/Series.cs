using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CampusChurch.Models
{
    public class Series
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        [DisplayName("Series Name")]
        public string Name { get; set; }

        [DisplayName("Start Date")]
        public DateTime StartDate { get; set; }

        public int DisplayOrder { get; set; }

        [MaxLength(500)]
        [DisplayName("Description")]
        public string? Description { get; set; }
    }
}
