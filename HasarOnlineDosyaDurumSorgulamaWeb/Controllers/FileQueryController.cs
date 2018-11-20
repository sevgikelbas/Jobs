using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HasarOnlineDosyaDurumSorgulamaWeb.Controllers
{
    public class FileQueryController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Detail(string FileNumber, string RegNumber, string IdentNumber)
        {
            var result = Sorgu.Lib.Repository.QueryRepository.QueryFiles(FileNumber, RegNumber, IdentNumber);
            return View(result);
        }
        //[HttpGet]
        //public ActionResult Detail()
        //{
        //    return View();
        //}
    }
}
