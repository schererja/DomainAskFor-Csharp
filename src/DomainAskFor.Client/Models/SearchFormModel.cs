using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DomainAskFor.Client.Models
{
    public class SearchFormModel
    {
        public string Prefix { get; set; }
        [Required]
        public string Word { get; set; }
        public string Suffix { get; set; }
        [Required]
        public TLDModel TLD { get; set; }

    }
    
}
