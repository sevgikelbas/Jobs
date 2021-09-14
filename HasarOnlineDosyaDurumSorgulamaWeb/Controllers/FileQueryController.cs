using HasarOnlineDosyaDurumSorgulamaWeb.Models;
using Sorgu.Lib.BaseType;
using Sorgu.Lib.Entity;
using Sorgu.Lib.Extensions;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
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
        public ActionResult Index(FileQueryModel fileQueryModel)
        {
            Result<List<ResponseModel>> result = new Result<List<ResponseModel>>();
            if (ModelState.IsValid)
            {
                return RedirectToAction("Detail", fileQueryModel);
            }
            else
            {
                return View();
            }
        }

        public ActionResult Detail(FileQueryModel fileQueryModel)
        {
            Result<List<ResponseModel>> result = new Result<List<ResponseModel>>();

            result = Sorgu.Lib.Repository.QueryRepository.QueryFiles(fileQueryModel.FileNumber, fileQueryModel.RegNumber, fileQueryModel.IdentNumber, fileQueryModel.SuffererNumber);
            if (result.Success)
            {
                return View(result);
            }
            else
            {
                //return new JsonResult
                //{
                //    Data = result,
                //    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                //};
                ModelState.AddModelError("err", "Dosya bulunamadı");
                return RedirectToAction("Index");
            }
        }
        public JsonResult GetDetail(string FileNumber, string RegNumber, string IdentNumber, string SuffererNumber)
        {
            var result = Sorgu.Lib.Repository.QueryRepository.QueryFiles(FileNumber, RegNumber, IdentNumber, SuffererNumber);
            return new JsonResult
            {
                Data = result,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        // Dosya Numarasına göre mağdur listeleme.
        [HttpGet]
        public JsonResult GetVictimListByFileNumber(string FileNumber)
        {
            var result = Sorgu.Lib.Repository.QueryRepository.GetFileVictims(FileNumber);
            return new JsonResult
            {
                Data = result,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        [HttpPost]
        public JsonResult SendMail(SendMailModel sendMailModel)
        {
            Sorgu.Lib.BaseType.Result<bool> result = new Sorgu.Lib.BaseType.Result<bool>();

            try
            {
                if (string.IsNullOrEmpty(sendMailModel.Name))
                    throw new Exception("Ad soyad alanı doldurulmalıdır.");
                if (string.IsNullOrEmpty(sendMailModel.Email))
                    throw new Exception("Email alanı doldurulmalıdır.");
                if (string.IsNullOrEmpty(sendMailModel.Phone))
                    throw new Exception("Telefon alanı doldurulmalıdır.");
                if (string.IsNullOrEmpty(sendMailModel.Subject))
                    throw new Exception("Konu alanı doldurulmalıdır.");
                if (string.IsNullOrEmpty(sendMailModel.Message))
                    throw new Exception("Mesaj alanı doldurulmalıdır.");

                List<string> lstEMail = new List<string>();
                List<string> lstCcEmail = new List<string>();
                List<string> lstBCcEmail = new List<string>();

                var fileResponsible = Sorgu.Lib.Repository.QueryRepository.GetFileResponsible(sendMailModel.NoticeId);


                lstEMail.Add(fileResponsible.Email);
                string subject = string.Format("{0} numaralı dosya için bir mesajınız var", sendMailModel.FileNo);
                string body = string.Format("İsim: {0} <br /> Email: {1} <br /> Telefon: {2} <br /> Konu: {3} <br /> Mesaj: {4}", sendMailModel.Name, sendMailModel.Email, sendMailModel.Phone, sendMailModel.Subject, sendMailModel.Message);

                bool mailRes = CommonExtensions.SendMail(lstEMail, lstCcEmail, subject, body, lstBCcEmail);
                if (mailRes)
                {
                    result.Response = true;
                    result.Message = "Mail gönderilmiştir.";
                }
                else
                {
                    result.Response = false;
                    result.Message = "İşlem başarısız";
                }


            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                result.Response = false;
            }
            return new JsonResult
            {
                Data = result,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public ActionResult DocumentInstall(long EksikEvrakID)
        {
            var eksikEvrak = Sorgu.Lib.Repository.QueryRepository.GetEksikEvrak(EksikEvrakID);
            return View(eksikEvrak);
        }

        [HttpPost]
        public JsonResult DocumentInstall(FormCollection collection)
        {
            HttpPostedFileBase file = Request.Files["flEksikEvrak"];
            bool success = false;
            if (file != null && file.ContentLength > 0)
            {
                long HasarDosyaID = Convert.ToInt64(Request.Form["HasarDosyaID"]);
                long HasarIhbarID = Convert.ToInt64(Request.Form["HasarIhbarID"]);
                long EksikEvrakID = Convert.ToInt64(Request.Form["EksikEvrakID"]);
                long EvrakID = Convert.ToInt64(Request.Form["EvrakID"]);
                long SigortaFirmaID = Convert.ToInt64(ConfigurationManager.AppSettings["SigortaFirmaID"]);

                if (CheckFileType(file.FileName))
                {
                    string path = ConfigurationManager.AppSettings["EksikEvrakPath"] + ConfigurationManager.AppSettings["SigortaFirmaID"] + "\\" + HasarDosyaID + "\\" + HasarIhbarID + "\\" + "EVRAK" + "\\";
                    Guid guid = Guid.NewGuid();
                    string extension = Path.GetExtension(file.FileName);
                    string fileName = guid + extension;

                    string documentPath = path + fileName;

                    string documentUrl = ConfigurationManager.AppSettings["EksikEvrakUrl"] + ConfigurationManager.AppSettings["SigortaFirmaID"] + "/" + HasarDosyaID + "/" + HasarIhbarID + "/" + "EVRAK" + "/" + fileName;

                    file.SaveAs(documentPath);

                    Sorgu.Lib.Repository.QueryRepository.EksikEvrakUpdate(EksikEvrakID, DateTime.Now);
                    Sorgu.Lib.Repository.QueryRepository.EksikEvrakResimInsert(SigortaFirmaID, EvrakID, HasarIhbarID, HasarDosyaID, fileName, documentUrl, 1);

                    Sorgu.Lib.Repository.QueryRepository.FileStatusUpdate(HasarIhbarID, 7);                 

                    success = true;
                }
                else
                {
                    success = false;
                }           

                //ToDo:Mail gönder

            }
            else
            {
                success = false;
            }

            //var result = Sorgu.Lib.Repository.QueryRepository.QueryFiles(FileNumber, RegNumber, IdentNumber);


            return Json(new { success = success }, JsonRequestBehavior.AllowGet);
        }

        bool CheckFileType(string fileName)
        {
            string ext = Path.GetExtension(fileName);
            switch (ext.ToLower())
            {
                case ".jpg":
                    return true;
                case ".jpeg":
                    return true;
                case ".png":
                    return true;
                case ".pdf":
                    return true;
                default:
                    return false;
            }
        }
    }
}
