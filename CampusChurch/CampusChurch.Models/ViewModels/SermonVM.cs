using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CampusChurch.Models.ViewModels
{
    public class SermonVM
    {
        public Sermon Sermon {  get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem> SeriesList { get; set; }
        public IEnumerable<string> ImageList { get; set; } = new List<string>();
    }
}
