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
    #region ExportUDDetail
    public class ExportUDDetail : BusinessObject
    {
        public ExportUDDetail()
        {
            ExportUDDetailID = 0;
            ExportUDID = 0;
            ExportPIID = 0;
            PINo = "";
            ErrorMessage = "";
        }
        #region Property
        public int ExportUDDetailID { get; set; }
        public int ExportUDID { get; set; }
        public int ExportPIID { get; set; }
        public DateTime LCDate { get; set; }
        public DateTime LCReceiveDate { get; set; }
        public DateTime IssueDate { get; set; }
        public double Amount { get; set; }
        public double Qty { get; set; }
        public int ANo { get; set; }
        public string PINo { get; set; }
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property
        public string LCDateInString
        {
            get
            {
                if (LCDate == DateTime.MinValue) return "";
                return LCDate.ToString("dd MMM yyyy");
            }
        }
        public string LCReceiveDateInString
        {
            get
            {
                if (LCReceiveDate == DateTime.MinValue) return "";
                return LCReceiveDate.ToString("dd MMM yyyy");
            }
        }
        public string IssueDateInString
        {
            get
            {
                if (IssueDate == DateTime.MinValue) return "";
                return IssueDate.ToString("dd MMM yyyy");
            }
        }
        #endregion 


        #region Functions
        public static List<ExportUDDetail> Gets(int nExportUDID, long nUserID)
        {
            return ExportUDDetail.Service.Gets(nExportUDID, nUserID);
        }
        public static List<ExportUDDetail> Gets(string sSQL, long nUserID)
        {
            return ExportUDDetail.Service.Gets(sSQL, nUserID);
        }
        public ExportUDDetail Get(int id, long nUserID)
        {
            return ExportUDDetail.Service.Get(id, nUserID);
        }

        #endregion

        #region ServiceFactory
        internal static IExportUDDetailService Service
        {
            get { return (IExportUDDetailService)Services.Factory.CreateService(typeof(IExportUDDetailService)); }
        }
        #endregion

        public List<ExportUDDetail> ExportUDDetails { get; set; }
    }
    #endregion

    #region IExportUDDetail interface
    public interface IExportUDDetailService
    {
        ExportUDDetail Get(int id, Int64 nUserID);
        List<ExportUDDetail> Gets(int nExportUDID, Int64 nUserID);
        List<ExportUDDetail> Gets(string sSQL, Int64 nUserID);

    }
    #endregion
}
