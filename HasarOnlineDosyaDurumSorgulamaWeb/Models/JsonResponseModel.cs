using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HasarOnlineDosyaDurumSorgulamaWeb.Models
{
    public class JsonResponseModel
    {
        public string FileStatus { get; set; }
        public string PaymentStatus { get; set; }
        public decimal PaymentAmount { get; set; }
        public string PaymentDate { get; set; }
    }
}