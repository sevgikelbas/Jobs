using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sorgu.Lib.Entity
{
    public class EksikEvrakModel
    {
        public string EvrakAdi { get; set; }
        public DateTime Tarih { get; set; }
        public DateTime? KapanisTarihi { get; set; }
        public string Email { get; set; }
        public string Aciklama { get; set; }
        public string AdiSoyadi { get; set; }
    }
}
