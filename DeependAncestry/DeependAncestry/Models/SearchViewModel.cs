﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DeependAncestry.Models
{
    public class SearchViewModel
    {
        [Required]
        public string Name { get; set; }
        public bool Male { get; set; }
        public bool Female { get; set; }
        public SearchDirection Direction { get; set; }

        public IList<PersonSearchResult> Result { get; set; }
    }

    public enum SearchDirection
    {
        Ancestors,
        Descendants
    }
}