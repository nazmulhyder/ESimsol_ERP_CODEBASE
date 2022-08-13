using System;
using System.IO;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using System.Drawing;
using ICS.Core;
using ICS.Core.Framework;
using ICS.Core.Utility;
namespace ESimSol.BusinessObjects
{
    #region ExportUPDetail
    public class ExportUPDetail : BusinessObject
    {
        public ExportUPDetail()
        {
           
            ExportUPDetailID = 0;
            ExportUPID = 0;
            ExportUDID = 0;
            ExportLCID = 0;
            DateofUDReceive = DateTime.MinValue;
            ContractPersonalID = 0;
            Note = "";
            UPNo = "";
            ErrorMessage = "";
            ExportUP = null;
            UDReceiveDate = DateTime.MinValue;
            YetToUPAmount = 0;
        }
        #region properties
        public int ExportUPDetailID { get; set; }
        public int ExportUPID { get; set; }
        public int ExportUDID { get; set; }
        public int ExportLCID { get; set; }
        
        public DateTime DateofUDReceive { get; set; }
        public int ContractPersonalID { get; set; }
        public string Note { get; set; }
        public string UPNo { get; set; }
        public string ErrorMessage { get; set; }

        #endregion
        #region derivedproperties
        public double YetToUPAmount { get; set; }
        public string ExportUDNo { get; set; }
        public string ApplicantName { get; set; }
        public string ExportLCNo { get; set; }
        public DateTime LCOpeningDate { get; set; }
        public double Qty  { get; set; }
        public double Amount { get; set; }
        public DateTime UDReceiveDate { get; set; }

        public string DateofUDReceiveStr
        {
            get { return (this.DateofUDReceive == DateTime.MinValue) ? "" : this.DateofUDReceive.ToString("dd MMM yyyy"); }
        }

        public string LCOpeningDateStr
        {
            get { return (this.LCOpeningDate == DateTime.MinValue) ? "" : this.LCOpeningDate.ToString("dd MMM yyyy"); }
        }
        public string UDReceiveDateStr
        {
            get { return (this.UDReceiveDate == DateTime.MinValue) ? "" : this.UDReceiveDate.ToString("dd MMM yyyy"); }
        }


        public ExportUP ExportUP { get; set; }
        public int ANo { set; get; }
        #endregion

        #region Functions
        public static ExportUPDetail Get(int nExportUPDetailID, long nUserID)
        {
            return ExportUPDetail.Service.Get(nExportUPDetailID, nUserID);
        }
        public static List<ExportUPDetail> Gets(string sSQL, long nUserID)
        {
            return ExportUPDetail.Service.Gets(sSQL, nUserID);
        }
        public ExportUPDetail IUD(int nDBOperation, long nUserID)
        {
            return ExportUPDetail.Service.IUD(this, nDBOperation, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IExportUPDetailService Service
        {
            get { return (IExportUPDetailService)Services.Factory.CreateService(typeof(IExportUPDetailService)); }
        }
        #endregion
    }

    #endregion

    #region IExportUPDetailService interface

    public interface IExportUPDetailService
    {

        ExportUPDetail Get(int nExportUPDetailID, Int64 nUserID);
        List<ExportUPDetail> Gets(string sSQL, Int64 nUserID);
        ExportUPDetail IUD(ExportUPDetail oExportUPDetail, int nDBOperation, Int64 nUserID);
    }
    #endregion
}
