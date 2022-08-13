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
    #region ImportLCReqByExBillDetail
    public class ImportLCReqByExBillDetail : BusinessObject
    {
        public ImportLCReqByExBillDetail()
        {
            ImportLCReqByExBillDetailID = 0;
            ImportLCReqByExBillID = 0;
            ExportBillID = 0;
            ExportLCID = 0;
            ExportLCNo = "";
            LCOpeningDate = DateTime.Now;
            LDBCNo = "";
            Amount = 0;
            MaturityDate = DateTime.Now;
            ErrorMessage = "";
        }

        #region Property
        public int ImportLCReqByExBillDetailID { get; set; }
        public int ImportLCReqByExBillID { get; set; }
        public int ExportBillID { get; set; }
        public string ErrorMessage { get; set; }

        #endregion

        #region Derived Property
        public int ExportLCID { get; set; }
        public string ExportLCNo { get; set; }
        public DateTime LCOpeningDate { get; set; }
        public string LDBCNo { get; set; }
        public double Amount { get; set; }
        public DateTime MaturityDate { get; set; }
        public string LCOpeningDateInString
        {
            get
            {
                return LCOpeningDate.ToString("dd MMM yyyy");
            }
        }
        public string MaturityDateInString
        {
            get
            {
                return MaturityDate.ToString("dd MMM yyyy");
            }
        }
        #endregion

        #region Functions
        public static List<ImportLCReqByExBillDetail> Gets(int nImportLCReqByExBillID, long nUserID)
        {
            return ImportLCReqByExBillDetail.Service.Gets(nImportLCReqByExBillID, nUserID);
        }
        public static List<ImportLCReqByExBillDetail> Gets(string sSQL, long nUserID)
        {
            return ImportLCReqByExBillDetail.Service.Gets(sSQL, nUserID);
        }
        public ImportLCReqByExBillDetail Get(int id, long nUserID)
        {
            return ImportLCReqByExBillDetail.Service.Get(id, nUserID);
        }

        #endregion

        #region ServiceFactory
        internal static IImportLCReqByExBillDetailService Service
        {
            get { return (IImportLCReqByExBillDetailService)Services.Factory.CreateService(typeof(IImportLCReqByExBillDetailService)); }
        }
        #endregion

        public List<ImportLCReqByExBillDetail> ImportLCReqByExBillDetails { get; set; }
    }
    #endregion

    #region IImportLCReqByExBillDetail interface
    public interface IImportLCReqByExBillDetailService
    {
        ImportLCReqByExBillDetail Get(int id, Int64 nUserID);
        List<ImportLCReqByExBillDetail> Gets(int nImportLCReqByExBillID, Int64 nUserID);
        List<ImportLCReqByExBillDetail> Gets(string sSQL, Int64 nUserID);

    }
    #endregion
}
