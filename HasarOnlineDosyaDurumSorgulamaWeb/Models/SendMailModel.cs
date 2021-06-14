using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HasarOnlineDosyaDurumSorgulamaWeb.Models
{
    public class SendMailModel
    {
        
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
        public int NoticeId { get; set; }
        public string FileNo { get; set; }
    }
}