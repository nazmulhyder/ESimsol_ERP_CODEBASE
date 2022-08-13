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
    #region ImportLCReqByExBill
    public class ImportLCReqByExBill : BusinessObject
    {
        public ImportLCReqByExBill()
        {
            ImportLCReqByExBillID = 0;
            ImportLCID = 0;
            AmendmentNo = 0;
            BankAccountID = 0;
            Amount = 0;
            MarginLien = 0;
            MarginCash = 0;
            MarginSCF = 0;
            MarginLienP = 0;
            MarginCashP = 0;
            MarginSCFP = 0;
            Enclosed = "";
            Note = "";
            IssueDate = DateTime.Now;
            ErrorMessage = "";
            ImportLCReqByExBillDetails = new List<ImportLCReqByExBillDetail>();
            ImportLC = new ImportLC();
        }

        #region Property
        public int ImportLCReqByExBillID { get; set; }
        public int ImportLCID { get; set; }
        public int AmendmentNo { get; set; }
        public int BankAccountID { get; set; }
        public double Amount { get; set; }
        public double MarginLien { get; set; }
        public double MarginCash { get; set; }
        public double MarginSCF { get; set; }
        public double MarginLienP { get; set; }
        public double MarginCashP { get; set; }
        public double MarginSCFP { get; set; }
        public string Enclosed { get; set; }
        public string Note { get; set; }
        public DateTime IssueDate { get; set; }
        public string ErrorMessage { get; set; }
        #endregion 

        #region Derived Property
        public ImportLC ImportLC { get; set; }
        public string IssueDateInString
        {
            get
            {
                return IssueDate.ToString("dd MMM yyyy");
            }
        }
        public List<ImportLCReqByExBillDetail> ImportLCReqByExBillDetails { get; set; }
        #endregion

        #region Functions
        public static List<ImportLCReqByExBill> Gets(long nUserID)
        {
            return ImportLCReqByExBill.Service.Gets(nUserID);
        }
        public static List<ImportLCReqByExBill> Gets(string sSQL, long nUserID)
        {
            return ImportLCReqByExBill.Service.Gets(sSQL, nUserID);
        }
        public ImportLCReqByExBill Get(int id, long nUserID)
        {
            return ImportLCReqByExBill.Service.Get(id, nUserID);
        }
        public ImportLCReqByExBill GetByLC(int id, long nUserID)
        {
            return ImportLCReqByExBill.Service.GetByLC(id, nUserID);
        }
        public ImportLCReqByExBill Save(long nUserID)
        {
            return ImportLCReqByExBill.Service.Save(this, nUserID);
        }
        public string Delete(int id, long nUserID)
        {
            return ImportLCReqByExBill.Service.Delete(id, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IImportLCReqByExBillService Service
        {
            get { return (IImportLCReqByExBillService)Services.Factory.CreateService(typeof(IImportLCReqByExBillService)); }
        }
        #endregion
    }
    #endregion

    #region IImportLCReqByExBill interface
    public interface IImportLCReqByExBillService
    {
        ImportLCReqByExBill Get(int id, Int64 nUserID);
        ImportLCReqByExBill GetByLC(int id, Int64 nUserID);
        List<ImportLCReqByExBill> Gets(Int64 nUserID);
        List<ImportLCReqByExBill> Gets(string sSQL, Int64 nUserID);
        string Delete(int id, Int64 nUserID);
        ImportLCReqByExBill Save(ImportLCReqByExBill oImportLCReqByExBill, Int64 nUserID);


    }
    #endregion
}
