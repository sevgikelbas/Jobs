using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HasarOnlineDosyaDurumSorgulamaWeb.Models
{
    public class FileQueryModel
    {
        [Required(ErrorMessage = "Dosya numarası giriniz.")]
        public string FileNumber { get; set; }
        [Required(ErrorMessage = "Plaka numarası giriniz.")]
        public string RegNumber { get; set; }
        [Required(ErrorMessage = "Kimlik numarası giriniz.")]
        public string IdentNumber { get; set; }
        [Required(ErrorMessage = "Mağdur numarası giriniz.")]
        public string SuffererNumber { get; set; }
    }
}

