using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HasarOnlineDosyaDurumSorgulamaWeb.Models
{
    public class DocumentInstallModel
    {
        public long HasarDosyaID { get; set; }
        public long HasarIhbarID { get; set; }
        public long EksikEvrakID { get; set; }
        public long EvrakID { get; set; }
    }
}