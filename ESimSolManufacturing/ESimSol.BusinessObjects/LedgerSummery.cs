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
    #region LedgerSummery
    public class LedgerSummery : BusinessObject
    {
        public LedgerSummery()
        {
            AccountHeadID = 0;
            CurrencyID = 0;
            IsDebit = false;
            DrAmount = 0;
            CrAmount = 0;
            AccountHeadCode = "";
            AccountHeadName = "";
            BUID = 0;
            StartDate = DateTime.Now;
            EndDate = DateTime.Now;
            IsApproved = false;
            ErrorMessage = "";
        }

        #region Property
        public int AccountHeadID { get; set; }
        public int CurrencyID { get; set; }
        public bool IsDebit { get; set; }
        public double DrAmount { get; set; }
        public double CrAmount { get; set; }
        public string AccountHeadCode { get; set; }
        public string AccountHeadName { get; set; }

        public int BUID { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsApproved { get; set; }
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property

        public List<Currency> Currencies { get; set; }
        public string BusinessUnitIDs { get; set; }
        public string DrAmountInString
        {
            get
            {
                if (DrAmount <= 0)
                    return "";
                else
                    return DrAmount.ToString("###,0.00");
            }
        }
        public string CrAmountInString
        {
            get
            {
                if (CrAmount <= 0)
                    return "";
                else
                    return CrAmount.ToString("###,0.00");
            }
        }
        public string StartDateInString
        {
            get
            {
                return StartDate.ToString("dd MMM yyyy");
            }
        }
        public string EndDateInString
        {
            get
            {
                return EndDate.ToString("dd MMM yyyy");
            }
        }
        //public string RequestTypeInString
        //{
        //    get
        //    {
        //        return EnumObject.jGet(this.RequestType);
        //    }
        //}
        //public int RequestTypeInt
        //{
        //    get
        //    {
        //        return (int)RequestType;
        //    }
        //}

        #endregion

        #region Functions

        public static List<LedgerSummery> Gets(LedgerSummery oLedgerSummery, long nUserID)
        {
            return LedgerSummery.Service.Gets(oLedgerSummery, nUserID);
        }
        
        
        #endregion

        #region ServiceFactory
        internal static ILedgerSummeryService Service
        {
            get { return (ILedgerSummeryService)Services.Factory.CreateService(typeof(ILedgerSummeryService)); }
        }
        #endregion
    }
    #endregion

    #region ILedgerSummery interface
    public interface ILedgerSummeryService
    {
        List<LedgerSummery> Gets(LedgerSummery oLedgerSummery, Int64 nUserID);
        

    }
    #endregion
}
