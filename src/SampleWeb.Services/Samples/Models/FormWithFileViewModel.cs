using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace SampleWeb.Services.Samples.Models
{
    public class FormWithFileViewModel
    {
        [Required]
        [StringLength(25)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [StringLength(25)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Display(Name="Profile Photo")]
        public IFormFile ProfilePhoto { get; set; }
    }
}