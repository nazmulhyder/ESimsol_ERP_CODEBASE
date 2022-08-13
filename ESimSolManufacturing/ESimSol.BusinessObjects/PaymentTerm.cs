using System;
using System.IO;
using ICS.Core;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using ICS.Core.Framework;
using ICS.Core.Utility;

namespace ESimSol.BusinessObjects
{
    #region PaymentTerm    
    public class PaymentTerm : BusinessObject
    {
        public PaymentTerm()
        {
            PaymentTermID = 0;
            TermText = "";
            DateText = "";
            Days = 0;
            BUID = 0;
            ErrorMessage = "";
            PaymentTerms = new List<PaymentTerm>();
        }

        #region Properties        
        public int PaymentTermID { get; set; }
        public int Percentage { get; set; }
        public int BUID { get; set; }
        public string TermText { get; set; }
        public EnumDayApplyType DayApplyType { get; set; }
        public int DayApplyTypeint { get; set; }
        public int Days { get; set; }
        public EnumDisplayPart DateDisplayPart { get; set; }
        public int DateDisplayPartint { get; set; }
        public string DateText { get; set; }
        public EnumPaymentTermType PaymentTermType { get; set; }
        public int PaymentTermTypeInt { get; set; }
        public string EndNote { get; set; }
        public string ErrorMessage { get; set; }   
        public List<PaymentTerm> PaymentTerms { get; set; }        
     

        #region Derived Property
        private string sFullTerm="";
        public string FullTerm
        {
            get
            {
                if( this.Percentage>0 )
                {
                    sFullTerm =sFullTerm+""+this.Percentage + "% ";
                }
                if (!String.IsNullOrEmpty(this.TermText))
                {
                    sFullTerm =sFullTerm+ this.TermText+" ";
                }
                if (this.DayApplyType != EnumDayApplyType.None)
                {
                    sFullTerm =sFullTerm+""+this.DayApplyType.ToString()+" ";
                }
                if (this.Days > 0)
                {
                    sFullTerm = sFullTerm + "" + this.Days.ToString() + " ";
                }
                if (this.DateDisplayPart != EnumDisplayPart.None)
                {
                    sFullTerm = sFullTerm + EnumObject.jGet(this.DateDisplayPart) + " ";
                }
                if (!String.IsNullOrEmpty(this.DateText))
                {
                    sFullTerm = sFullTerm + this.DateText + " ";
                }
                if (this.PaymentTermType != EnumPaymentTermType.None)
                {
                    sFullTerm = sFullTerm + EnumObject.jGet(this.PaymentTermType) + " ";
                }
                if (!String.IsNullOrEmpty(this.EndNote))
                {
                    sFullTerm = sFullTerm + this.EndNote + " ";
                }

                return sFullTerm;
            }
        }
   
        #endregion

        #endregion
        
        #region Functions
        public static List<PaymentTerm> Gets(Int64 nUserID)
        {
            return PaymentTerm.Service.Gets(nUserID);
        }

        public static List<PaymentTerm> GetsByBU(int nBUID, Int64 nUserID)
        {
            return PaymentTerm.Service.GetsByBU(nBUID, nUserID);
        }
        public static List<PaymentTerm> Gets(string sSQL,Int64 nUserID)
        {
            return PaymentTerm.Service.Gets(sSQL,nUserID);
        }
        public PaymentTerm Get(int id, Int64 nUserID)
        {
            return PaymentTerm.Service.Get(id, nUserID);
        }
        public PaymentTerm Save(Int64 nUserID)
        {
            return PaymentTerm.Service.Save(this, nUserID);
        }
        public string Delete(int id, Int64 nUserID)
        {
            return PaymentTerm.Service.Delete(id, nUserID);
        }

        #endregion

        #region ServiceFactory
        internal static IPaymentTermService Service
        {
            get { return (IPaymentTermService)Services.Factory.CreateService(typeof(IPaymentTermService)); }
        }
        #endregion
    }
    #endregion


    #region IPaymentTerms interface
    public interface IPaymentTermService
    {
        PaymentTerm Get(int id, Int64 nUserID);
        List<PaymentTerm> Gets(Int64 nUserID);
        List<PaymentTerm> GetsByBU(int nBUID, Int64 nUserID);
        List<PaymentTerm> Gets(string sSQL,Int64 nUserID);
        string Delete(int id, Int64 nUserID);
        PaymentTerm Save(PaymentTerm oPaymentTerm, Int64 nUserID);
    }
    #endregion
}