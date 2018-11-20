﻿using Sorgu.Lib.BaseType;
using Sorgu.Lib.Entity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Sorgu.Lib.Extensions;

namespace Sorgu.Lib.Repository
{
    public class QueryRepository
    {
        public static Result<List<ResponseModel>> QueryFiles(string FNumber, string RegNumber, string IdentNumber)
        {
            FNumber = FNumber.ToNormalize();
            RegNumber = RegNumber.ToNormalize();
            IdentNumber = IdentNumber.ToNormalize();

            int validationCount = 0;
            if (!string.IsNullOrEmpty(FNumber))
            {
                validationCount++;
            }
            if (!string.IsNullOrEmpty(RegNumber))
            {
                validationCount++;
            }
            if (!string.IsNullOrEmpty(IdentNumber))
            {
                validationCount++;
            }

            if ((string.IsNullOrEmpty(FNumber) && string.IsNullOrEmpty(RegNumber) && string.IsNullOrEmpty(IdentNumber)) || (validationCount<2))
            {
                return new Result<List<ResponseModel>>
                {
                    Success = false,
                    Message = "Dosyaya durumunuzu sorgulayabilmek için en az iki bilgi girmelisiniz",
                    Response = new List<ResponseModel>()
                };
            }

            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["SorguCS"].ConnectionString))
            {
                List<ResponseModel> data = null;
                if (ConfigurationManager.AppSettings["Test"] == "1")
                {
                    data = new List<ResponseModel>();
                    ResponseModel rm = new ResponseModel()
                    {
                        ClaimCauseName = "Çarpışma",
                        ClaimCityDistrict = "İstanbul - Bakırköy",
                        ClaimDate = "01.05.2017",
                        EstimatedDamageAmount = 2254,
                        ExpertAssignDate = "02.05.2017",
                        ExpertName = "Ekspertiz A.Ş",
                        ExpertPhone = "0219 272 36 85",
                        ExpertReportSendDate = "-",
                        FileNumber = "2017101993849384",
                        FileStateName = "Kesin Rapor Bekleniyor",
                        HasarDosyaID = 3,
                        HasarIhbarID = 1,
                        InformDate = "02.05.2017",
                        InformDocumentTypeName = "Beyan",
                        InsuredBrandName = "Audi",
                        InsuredModelName = "A3",
                        InsuredModelYear = "2014",
                        InsuredName = "Sigortalı Test 1",
                        InsuredRegistrationNumber = "34 ABC 000",
                        PolicyBeginDate = "01.01.2017",
                        PolicyEndDate = "31.12.2017",
                        PolicyNumber = "Pol99281",
                        PolicyTypeName = "Trafik",
                        PreReportSendDate = "04.05.2017",
                        RepairShopAddress = "Mithatpaşa cd. Dereboyu sok. No:23 Levent/İstanbul",
                        RepairShopContracted = "Anlaşmasız",
                        RepairShopName = "Oto Kardeş Tic. Ltd. Şti.",
                        RepairShopPhone = "0212 654 87 97",
                        SuffererBrandName = "Peugeot",
                        SuffererModelName = "3008",
                        SuffererModelYear = "2016",
                        SuffererName = "Mağdur Test 1",
                        SuffererRegistrationNumber = "48 ET 333",
                        PaymentDecisionMainState = "?",
                        PaymentDecisionSubState = "?"
                    };
                    data.Add(rm);
                }
                else
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append("Select top 1 d.HasarDosyaID, d.DosyaNo as FileNumber, d.PoliceNo as PolicyNumber, convert(varchar(25), d.PoliceBaslangicTarihi, 105) as PolicyBeginDate, convert(varchar(25), d.PoliceBitisTarihi, 105) as PolicyEndDate, convert(varchar(25), d.HasarTarihi, 105) as ClaimDate, i.IhbarDurumID,");
                    sb.Append(" d.SigortaliAdi +' '+ d.SigortaliSoyadi as InsuredName, d.SigortaliAracPlakaNo as InsuredRegistrationNumber, d.SigortaliAracModelYili as InsuredModelYear,");

                    sb.Append(" i.HasarIhbarID, Convert(varchar(25), i.IhbarTarih, 105) as InformDate, tt.Ad as InformDocumentTypeName, i.MuallakHasar as EstimatedDamageAmount,");
                    sb.Append(" idt.DurumValue as FileStateName, i.AracSahibiAdi + ' ' + i.AracSahibiSoyadi as SuffererName, i.AracPlakaNo as SuffererRegistrationNumber, i.AracModelYili as SuffererModelYear,");
                    sb.Append(" i.ServisAdi as RepairShopName, i.ServisTelefon1 as RepairShopPhone, i.ServisAdres as RepairShopAddress, Case ISNULL(i.ServisID, 0) When 0 Then 'Anlaşmasız' Else 'Anlaşmalı' End as RepairShopContracted, Convert(varchar(25), i.EkspertizTalepTarihi, 105) as ExpertAssignDate, ");

                    sb.Append(" od.OdemeOnayi, od.OdemeDurumID, od.OdemeAltDurumID, od.DurumAdi as PaymentDecisionMainState, od.AltDurumAdi as PaymentDecisionSubState,od.PaymentTargetName,od.PaymentTargetLastName,od.PaymentTargetTypeName,");

                    sb.Append(" ob.ActorCode, ob.PaymentNo, ob.PaymentAmount, ob.PaymentDate, ob.PState, ob.PDescription,");

                    sb.Append(" ef.FirmaAdi as ExpertName, ef.Telefon1 as ExpertPhone,");
                    sb.Append(" brans.BransAdi as PolicyTypeName,");
                    sb.Append(" hasned.HasarNedeniAdi as ClaimCauseName,");
                    sb.Append(" ISNULL(hcity.Adi, '') +' '+ ISNULL(hdistrict.Adi, '') as ClaimCityDistrict,");
                    sb.Append(" ms.MarkaAdi as InsuredBrandName,");
                    sb.Append(" mos.MarkaTipAdi as InsuredModelName,");
                    sb.Append(" mm.MarkaAdi as SuffererBrandName,");
                    sb.Append(" mom.MarkaTipAdi as SuffererModelName,");
                    sb.Append(" Case When erO.GonderimTarihi is null Then '-' Else Convert(varchar(25), erO.GonderimTarihi, 105) End as PreReportSendDate,");
                    sb.Append(" Case When erK.GonderimTarihi is null Then '-' Else Convert(varchar(25), erK.GonderimTarihi, 105) End as ExpertReportSendDate");

                    sb.Append(" from TblHasarDosya as d");
                    sb.Append(" inner join TblHasarIhbar as i on d.HasarDosyaID=i.HasarDosyaID");
                    sb.Append(" inner join TblBrans as brans on brans.BransID=d.BransID");
                    sb.Append(" inner join TblHasarNedeni as hasned on hasned.HasarNedeniID=d.HasarNedeniID");
                    sb.Append(" left outer join TblSehir as hcity on hcity.SehirID=d.HasarSehirID");
                    sb.Append(" left outer join TblIlce as hdistrict on hdistrict.IlceID=d.HasarIlceID");
                    sb.AppendFormat(" left outer join {0}.TblAracMarka as ms on ms.MarkaID=d.SigortaliAracMarkaID", ConfigurationManager.AppSettings["CommonDBTablePath"]);
                    sb.AppendFormat(" left outer join {0}.TblAracMarkaTipi as mos on mos.AracMarkaTipiID=d.SigortaliAracMarkaTipiID", ConfigurationManager.AppSettings["CommonDBTablePath"]);
                    sb.AppendFormat(" left outer join {0}.TblAracMarka as mm on mm.MarkaID=i.AracMarkaID", ConfigurationManager.AppSettings["CommonDBTablePath"]);
                    sb.AppendFormat(" left outer join {0}.TblAracMarkaTipi as mom on mom.AracMarkaTipiID=i.AracMarkaTipiID", ConfigurationManager.AppSettings["CommonDBTablePath"]);
                    sb.AppendFormat(" left outer join {0}.TblEksperFirma as ef on ef.EksperFirmaID=i.EksperFirmaID", ConfigurationManager.AppSettings["CommonDBTablePath"]);
                    sb.Append(" left outer join TblEksperRapor as erO on erO.IhbarID=i.HasarIhbarID and erO.RaporTipi=1 ");
                    sb.Append(" left outer join TblEksperRapor as erK on erK.IhbarID=i.HasarIhbarID and erK.RaporTipi=2 ");
                    sb.Append(" left outer join TblTutanakTipi as tt on tt.Kodu=i.TutanakTipi");
                    sb.Append(" left outer join TblIhbarDurumTipi as idt on idt.DurumKey=i.IhbarDurumID");
                    sb.Append(" outer apply (Select Top 1 od.OdemeOnayi, od.OdemeDurumID, od.OdemeAltDurumID, dur1.Aciklama as DurumAdi, dur2.Aciklama as AltDurumAdi, od.AlacakliAdi as PaymentTargetName, od.AlacakliSoyad as PaymentTargetLastName, at.Adi as PaymentTargetTypeName, od.AlacakliTipi from TblOdemeDetaylari as od inner join TblOdemeDurumlari as dur1 on od.OdemeDurumID=dur1.ID left outer join TblOdemeAltDurumlari as dur2 on dur2.ID=od.OdemeAltDurumID left outer join TblAlacakliTipi as at on at.ID=od.AlacakliTipi where od.HasarIhbarID=i.HasarIhbarID order by od.ID) as od");
                    sb.Append(" outer apply (Select Top 1 ob.AktorKodu as ActorCode, ob.TediyeNo as PaymentNo, ob.OdenenTutar as PaymentAmount, ob.OdemeTarihi as PaymentDate, ob.OdemeDurumu as PState, ob.OdemeAciklama as PDescription from TblOdemeBildirim as ob where ob.HasarIhbarID=i.HasarIhbarID and ob.OdemeTipi=od.AlacakliTipi order by ob.ID desc) as ob");
                    sb.Append(" where d.Durum=1 and i.Durum=1");
                    if (!string.IsNullOrEmpty(FNumber))
                    {
                        sb.AppendFormat(" and Replace(d.DosyaNo, ' ', '')='{0}'", FNumber);
                    }
                    if (!string.IsNullOrEmpty(RegNumber))
                    {
                        sb.AppendFormat(" and (Replace(d.SigortaliAracPlakaNo, ' ', '')='{0}' or Replace(i.AracPlakaNo, ' ', '')='{0}')", RegNumber);
                    }
                    if (!string.IsNullOrEmpty(IdentNumber))
                    {
                        sb.AppendFormat(" and (Replace(d.SigortaliKimlikNo, ' ', '')='{0}' or Replace(i.AracSahibiKimlikNo, ' ', '')='{0}')", IdentNumber);
                    }
                    sb.Append(" order by i.HasarIhbarID");

                    string query = sb.ToString();
                    data = cn.Query<ResponseModel>(query).ToList();
                }

                //Eksik döküman listesi için burada for açacağız.
                foreach(var item in data)
                {
                    item.MissingDocumentList = QueryRepository.GetEksikEvrakList(item.HasarIhbarID);
                }

                return new Result<List<ResponseModel>>
                {
                    Success = true,
                    Response = data
                };
            }
        }
        public static List<EksikEvrakModel> GetEksikEvrakList(int IhbarID)
        {
            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["SorguCS"].ConnectionString))
            {
                List<EksikEvrakModel> data = null;
                if (ConfigurationManager.AppSettings["Test"] == "1")
                {
                    data = new List<EksikEvrakModel>();
                    data.Add(new EksikEvrakModel
                    {
                        Aciklama = "Açıklama test ",
                        Email = "info@sigortali.com",
                        EvrakAdi = "Evrak İsmi",
                        Tarih = DateTime.Now,
                        KapanisTarihi = DateTime.Now,
                        AdiSoyadi = "Test Kullanıcı"
                    });
                }
                else
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append("SELECT  et.ID, et.EvrakID, et.KapanisTarihi, e.EvrakAdi, et.Tarih FROM TblEksikEvrak et INNER JOIN TblEvrak e ON e.EvrakID = et.EvrakID WHERE et.HasarIhbarID = " + IhbarID);

                    string query = sb.ToString();
                    data = cn.Query<EksikEvrakModel>(query).ToList();
                }

                if (data == null)
                {
                    data = new List<EksikEvrakModel>();
                }

                return data;
            }
        }
    }
}
