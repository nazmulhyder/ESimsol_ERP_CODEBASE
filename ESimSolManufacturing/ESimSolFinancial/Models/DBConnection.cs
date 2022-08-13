using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Web.Mvc;
using System.Web.Security;

namespace ESimSolChemical.Models
{
    public class DBConnection
    {
        [Required]
        [StringLength(50, MinimumLength = 0, ErrorMessage = "Maximum length is 50.")]
        [Display(Name = "Project Name")]
        [RegularExpression(@"(\S)+", ErrorMessage = "White space is not allowed")]
        [ScaffoldColumn(false)]
        public string ProjectName { get; set; }
    }
}