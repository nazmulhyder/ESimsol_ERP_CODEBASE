using System;
using ICS.Core.Framework;
using ICS.Core.Utility;
using ICS.Core;
using ESimSol.BusinessObjects;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.ServiceModel;
using ESimSol.BusinessObjects.ReportingObject;
namespace ESimSol.BusinessObjects
{
    #region Payment
    [DataContract]
    public class Payment : BusinessObject
    {
        public Payment()
        {           
            PaymentID = 0;
            ContractorID = 0;
            ContactPersonnelID = 0;
            MRNo = "";
            PaymentMode = EnumPaymentMethod.None;
            Amount = 0;
            //PaymentStatus = // Paid = false and Received = true
            ApproveBy = 0;
            PaymentDocID = 0;
            Note = "";
            EncashmentDate = DateTime.Now;
            MRDate = DateTime.Now;
            ContractorName = "";
            ContactPerson = "";
            ApprovedByName = "";
            BankID = 0;
            BankName = "";
            DocNo = "";
            AccountNo = "";            
            DocAmount = 0;
            ConsumedAmount = 0;
            DocDate = DateTime.Now;
            ContractorName = "";
            PrepareByName = "";
            BankAccountID_Deposit = 0;
            PaymentType = EnumPaymentReceiveType.Cash;
            PaymentDetails = new List<PaymentDetail>();
            BUID = 0;
        }

        #region Properties      
        public int PaymentID { get; set; }
        public int ContractorID { get; set; }
        public int ContactPersonnelID { get; set; }
        public string MRNo { get; set; }
        public DateTime MRDate { get; set; }
        public EnumPaymentMethod PaymentMode { get; set; }
        public double Amount { get; set; }
        public double Discount { get; set; }
        public EnumPaymentStatus PaymentStatus { get; set; }
        public int ApproveBy { get; set; }
        public int PaymentDocID { get; set; }
        public string Note { get; set; }
        public DateTime EncashmentDate { get; set; }
        public string ContactPerson { get; set; }
        public string ApprovedByName { get; set; }
        public int BankID { get; set; }
        public string BankName { get; set; }
        public string DocNo { get; set; }
        public string AccountNo { get; set; }        
        public double DocAmount { get; set; }
        public double ConsumedAmount { get; set; }
        public DateTime DocDate { get; set; }
        public string ContractorName { get; set; }
        public EnumPaymentReceiveType PaymentType { get; set; } 
        public int PaymentTypeInInt { get; set; }
        public int BUID { get; set; }
        public int PaymentModeInt { get; set; }
        public string Currency { get; set; }
        public string CurrencyBC { get; set; }
        public int CurrencyID { get; set; }
        public int BankAccountID_Deposit { get; set; }
        public double CRate { get; set; }
        public string ErrorMessage { get; set; }
        public string Parm { get; set; }
        public bool IsWillVoucherEffect { get; set; }
        #endregion

        #region Derive Property
        public string IsWillVoucherEffectSt
        {
            get
            {
                if (this.IsWillVoucherEffect)
                {
                    return "Yes";
                }
                else
                {
                    return "No";
                }
            }
        }
        public string PrepareByName { get; set; }
        public List<PaymentDetail> PaymentDetails { get; set; }
        public int PaymentStatusInInt { get; set; }
        public string PaymentStatusSt
        {
            get
            {
                return PaymentStatus.ToString();
            }
        }
        public string PaymentTypeSt
        {
            get
            {
                return EnumObject.jGet((EnumPaymentReceiveType)this.PaymentType);
            }
        }
        public string PaymentModeSt
        {
            get
            {
                return EnumObject.jGet((EnumPaymentMethod)this.PaymentMode);
            }
        }
        public string EncashmentDateSt
        {
            get
            {
                if (this.EncashmentDate == DateTime.MinValue)
                {
                    return "-";
                }
                else
                {
                    return EncashmentDate.ToString("dd MMM yyyy");
                }
            }
        }
        public string MRDateSt
        {
            get
            {
                return this.MRDate.ToString("dd MMM yyyy");
            }
        }

        public string DocDateSt
        {
            get
            {
                return DocDate.ToString("dd MMM yyyy");
            }
        }
        
     
        private string sTakaInWord = "";
        public string TakaInWord
        {
            get
            {
                if (this.CurrencyID == 1)
                {
                    sTakaInWord = Global.TakaWords(this.Amount);
                }
                else
                {
                    sTakaInWord = Global.DollarWords(this.Amount);
                }
                return sTakaInWord;
            }
        }
        public string AmountInSt
        {
            get
            {
                return this.Currency + " " + Global.MillionFormat(this.Amount);
            }
        }        
        public string AmountInBC
        {
            get
            {
                return this.CurrencyBC + " " + Global.MillionFormat(this.Amount * this.CRate);
            }
        }
        #endregion

        #region Functions
        public Payment Get(int id, long nUserID)
        {
            return Payment.Service.Get(id, nUserID);
        }
        public Payment GetBY(int id, long nUserID)
        {
            return Payment.Service.GetBY(id, nUserID);
        }
        public static List<Payment> Gets(long nUserID)
        {
            return Payment.Service.Gets(nUserID);
        }

        public static List<Payment> Gets(string sSQL, long nUserID)
        {
            return Payment.Service.Gets(sSQL, nUserID);
        }
        public static List<Payment> Gets(EnumPaymentType ePaymentType, long nUserID)
        {
            return Payment.Service.Gets(ePaymentType, nUserID);
        }
        public string Delete(long nUserID)
        {
            return Payment.Service.Delete(this, nUserID);
        }
        public Payment Save(long nUserID)
        {
            return Payment.Service.Save(this, nUserID);
        }
        public Payment Payment_UndoApprove(long nUserID)
        {
            return Payment.Service.Payment_UndoApprove(this, nUserID);
        }
        public Payment Payment_Approve(long nUserID)
        {
            return Payment.Service.Payment_Approve(this, nUserID);
        }
        public Payment UpdateVoucherEffect(long nUserID)
        {
            return Payment.Service.UpdateVoucherEffect(this, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IPaymentService Service
        {
            get { return (IPaymentService)Services.Factory.CreateService(typeof(IPaymentService)); }
        }
    
        #endregion
    }
    #endregion


    #region IPayment interface
    public interface IPaymentService
    {
        Payment Get(int id, long nUserID);
        Payment GetBY(int nDebitNoteID, long nUserID);
        List<Payment> Gets(Int64 nUserID);
        List<Payment> Gets(string sSQL,Int64 nUserID);
        List<Payment> Gets(EnumPaymentType ePaymentType, Int64 nUserID);
        string Delete(Payment oPayment, Int64 nUserID);
        Payment Save(Payment oPayment, Int64 nUserID);
        Payment Payment_Approve(Payment oPayment, Int64 nUserID);
        Payment Payment_UndoApprove(Payment oPayment, Int64 nUserID);
        Payment UpdateVoucherEffect(Payment oPayment, Int64 nUserID);   
    }
    #endregion
   
}
