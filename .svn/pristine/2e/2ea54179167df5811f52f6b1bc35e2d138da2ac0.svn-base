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
    public class LoanProductRate : BusinessObject
    {
        public LoanProductRate()
		{
            LoanProductRateID = 0;
            ProductID =0;
            ProductCode = "";
            BUID = 0;
            UnitPrice =0;
            CurrencyID = 0;
            MUnitID = 0;
            Remarks = "";
            ProductName = "";
            UnitName = "";
            Note = "";
            LastUpdateByName = "";
            DBServerDateTime = DateTime.Now;
            LastUpdateDateTime = DateTime.Now;
            LoanProductRateList = new List<LoanProductRate>();
            ErrorMessage = "";
		}
        #region Property
        public int LoanProductRateID { get; set; }
        public int ProductID { get; set; }
        public string ProductCode { get; set; }
        public int BUID { get; set; }
        public double UnitPrice { get; set; }
        public int CurrencyID { get; set; }
        public int MUnitID { get; set; }
        public string Remarks { get; set; }
        public string Note { get; set; }
        public string ProductName { get; set; }
        public string UnitName { get; set; }
        public DateTime DBServerDateTime { get; set; }
        public DateTime LastUpdateDateTime { get; set; }
        public string LastUpdateByName { get; set; }
        public string ErrorMessage { get; set; }
        #endregion 

        
        #region Derived Property
        public List<LoanProductRate> LoanProductRateList { get; set; }
        public string ProcessDateInString
        {
            get
            {
                return DBServerDateTime.ToString("dd MMM yyyy");
            }
        }
        public string LastUpdateDateInString
        {
            get
            {
                return DBServerDateTime.ToString("dd MMM yyyy");
            }
        }

        #endregion 

        #region Functions
        public LoanProductRate Save(long nUserID)
        {
            return LoanProductRate.Service.Save(this, nUserID);
        }
        public static List<LoanProductRate> Gets(string sSQL, long nUserID)
        {
            return LoanProductRate.Service.Gets(sSQL, nUserID);
        }
        public LoanProductRate Get(int id, long nUserID)
        {
            return LoanProductRate.Service.Get(id, nUserID);
        }

        public string Delete(int id, long nUserID)
        {
            return LoanProductRate.Service.Delete(id, nUserID);
        }

        #endregion

        #region ServiceFactory
        internal static ILoanProductRateService Service
        {
            get { return (ILoanProductRateService)Services.Factory.CreateService(typeof(ILoanProductRateService)); }
        }
        #endregion
    }

    #region ILoanProductRate interface
    public interface ILoanProductRateService
    {
        LoanProductRate Save(LoanProductRate oLoanProductRate, Int64 nUserID);
        List<LoanProductRate> Gets(string sSQL, Int64 nUserID);
        string Delete(int id, long nUserID);
        LoanProductRate Get(int id, long nUserID);

    }
    #endregion
}
