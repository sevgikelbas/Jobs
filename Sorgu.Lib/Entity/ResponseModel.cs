using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sorgu.Lib.Entity
{
    public class ResponseModel
    {
        public int HasarDosyaID { get; set; }
        public int HasarIhbarID { get; set; }
        public string FileNumber { get; set; }
        public string PolicyTypeName { get; set; }
        public string PolicyNumber { get; set; }
        public string PolicyBeginDate { get; set; }
        public string PolicyEndDate { get; set; }
        public string ClaimDate { get; set; }
        public string InformDate { get; set; }
        public string InformDocumentTypeName { get; set; }
        public string ClaimCauseName { get; set; }
        public decimal EstimatedDamageAmount { get; set; }
        public string ClaimCityDistrict { get; set; }
        public int IhbarDurumID { get; set; }
        public string FileStateName { get; set; }
        
        public string InsuredName { get; set; }
        public string InsuredRegistrationNumber { get; set; }
        public string InsuredBrandName { get; set; }
        public string InsuredModelName { get; set; }
        public string InsuredModelYear { get; set; }
        
        public string SuffererName { get; set; }
        public string SuffererRegistrationNumber { get; set; }
        public string SuffererBrandName { get; set; }
        public string SuffererModelName { get; set; }
        public string SuffererModelYear { get; set; }

        public string RepairShopName { get; set; }
        public string RepairShopPhone { get; set; }
        public string RepairShopAddress { get; set; }
        public string RepairShopContracted { get; set; }

        public string ExpertName { get; set; }
        public string ExpertAssignDate { get; set; }
        public string ExpertPhone { get; set; }

        public string PreReportSendDate { get; set; }
        public string ExpertReportSendDate { get; set; }

        public string PaymentDecisionMainState { get; set; }
        public string PaymentDecisionSubState { get; set; }

        string _targetName = string.Empty, _targetLastname = string.Empty;
        public string PaymentTargetName
        {
            get
            {
                if (!string.IsNullOrEmpty(_targetName) && _targetName.Length > 1)
                {
                    return _targetName.Substring(0, 1) + "***";
                }
                else
                {
                    return "***";
                }
            }
            set { _targetName = value; }
        }
        public string PaymentTargetLastName {
            get
            {
                if (!string.IsNullOrEmpty(_targetLastname) && _targetLastname.Length > 1)
                {
                    return _targetLastname.Substring(0, 1) + "***";
                }
                else
                {
                    return "***";
                }
            }
            set { _targetLastname = value; }
        }
        public string ActorCode { get; set; }
        public string PaymentNo { get; set; }
        public decimal? PaymentAmount { get; set; }
        public DateTime? PaymentDate { get; set; }
        public string PState { get; set; }
        public string PDescription { get; set; }
        public string PaymentTargetTypeName { get; set; }

        public List<EksikEvrakModel> MissingDocumentList { get; set; }
    }
}
