using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CampusChurch.Models
{
    public class Sermon
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        [DisplayName("Sermon Title")]
        public string Title { get; set; }

        
        [MaxLength(500)]
        public string Description { get; set; }

        [Required]
        public DateTime Date { get; set; }

        public int? SeriesId { get; set; } // Nullable
        [ForeignKey("SeriesId")]
        [ValidateNever]
        public Series Series { get; set; }
        [ValidateNever]
        [Required]
        public string FilePath { get; set; } // Added FilePath for storing the audio file path
        public string ImagePath { get; set; }
    }
}
