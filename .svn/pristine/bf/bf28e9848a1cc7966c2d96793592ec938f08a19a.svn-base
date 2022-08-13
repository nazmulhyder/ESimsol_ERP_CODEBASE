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
    #region LoanExportLC
    public class LoanExportLC : BusinessObject
    {
        public LoanExportLC()
        {
            LoanExportLCID = 0;
            LoanID = 0;
            ExportLCID = 0;
            Amount = 0;
            Remarks = string.Empty;
        }

        #region Property
        public int LoanExportLCID { get; set; }
        public int LoanID { get; set; }
        public int ExportLCID { get; set; }
        public double Amount { get; set; }
        public string Remarks { get; set; }
        public string ErrorMessage { get; set; }
        public List<LoanExportLC> LoanExportLCs { get; set; }
        public Boolean isDeletable { get; set; }
        public string ExportLCNo { get; set; }
        public string ExportLCCurrencySymbol { get; set; }
        public double ExportLCAmount { get; set; }

        #endregion

        #region Derived Property

        #endregion

        #region Functions
        public static List<LoanExportLC> Gets(int buid, long nUserID)
        {
            return LoanExportLC.Service.Gets(buid, nUserID);
        }

        public static List<LoanExportLC> Gets(string sSQL, long nUserID)
        {
            return LoanExportLC.Service.Gets(sSQL, nUserID);
        }
        public LoanExportLC Get(int id, long nUserID)
        {
            return LoanExportLC.Service.Get(id, nUserID);
        }

        public LoanExportLC Save(long nUserID)
        {
            return LoanExportLC.Service.Save(this, nUserID);
        }

        public string Delete(int id, long nUserID)
        {
            return LoanExportLC.Service.Delete(id, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static ILoanExportLCService Service
        {
            get { return (ILoanExportLCService)Services.Factory.CreateService(typeof(ILoanExportLCService)); }
        }
        #endregion
    }
    #endregion

    #region ILoan interface
    public interface ILoanExportLCService
    {
        LoanExportLC Get(int id, Int64 nUserID);

        List<LoanExportLC> Gets(int buid, Int64 nUserID);

        List<LoanExportLC> Gets(string sSQL, Int64 nUserID);
        string Delete(int id, Int64 nUserID);
        LoanExportLC Save(LoanExportLC oLoan, Int64 nUserID);



    }
    #endregion
}
