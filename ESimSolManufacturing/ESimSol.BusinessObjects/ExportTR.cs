using System;
using System.IO;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using ICS.Core;
using ICS.Core.Framework;
using ICS.Core.Utility;

namespace ESimSol.BusinessObjects
{
    #region ExportTR
    [DataContract]
    public class ExportTR : BusinessObject
    {
        public ExportTR()
        {
            ExportTRID = 0;
            TruckReceiptNo = "";
            TruckReceiptDate = DateTime.Now;
            Carrier = "";
            TruckNo = "";
            DriverName = "";
            Activity = true;
            BUID = 0;
            ErrorMessage = "";
            TruckReceiptDateString = "";
        }
        #region Properties
        public int ExportTRID { get; set; }
        public string TruckReceiptNo { get; set; }
        public DateTime TruckReceiptDate { get; set; }
        public string Carrier { get; set; }
        public string TruckNo { get; set; }
        public string DriverName { get; set; }
        public bool Activity { get; set; }
        public int BUID { get; set; }
        public string ErrorMessage { get; set; }
        public string TruckReceiptDateString { get; set; }
        #endregion

        #region Derived Property
        public string TruckReceiptDateInString
        {
            get
            {
                return this.TruckReceiptDate.ToString("dd MMM yyyy");
            }
        }
        public string ActivityInString
        {
            get
            {
                if (this.Activity == true) return "Active";
                else if (this.Activity == false) return "Inactive";
                else return "-";
            }
        }
        #endregion

        #region Functions


        public ExportTR Get(int nId, Int64 nUserID)
        {
            return ExportTR.Service.Get(nId, nUserID);
        }
        public ExportTR Save(Int64 nUserID)
        {
            return ExportTR.Service.Save(this, nUserID);
        }
        public string Delete(int nId, Int64 nUserID)
        {
            return ExportTR.Service.Delete(nId, nUserID);
        }
        public static List<ExportTR> Gets(Int64 nUserID)
        {
            return ExportTR.Service.Gets(nUserID);
        }
        public static List<ExportTR> Gets(bool bActivity, int nBUID, Int64 nUserID)
        {
            return ExportTR.Service.Gets(bActivity, nBUID, nUserID);
        }
        public static List<ExportTR> BUWiseGets(int nBUID, Int64 nUserID)
        {
            return ExportTR.Service.BUWiseGets( nBUID, nUserID);
        }
        public static List<ExportTR> Gets(string sSQL, Int64 nUserID)
        {
            return ExportTR.Service.Gets(sSQL, nUserID);
        }

    
        #endregion

     
        #region ServiceFactory
        internal static IExportTRService Service
        {
            get { return (IExportTRService)Services.Factory.CreateService(typeof(IExportTRService)); }
        }
        #endregion
    }
    #endregion

    #region IExportTR interface
    [ServiceContract]
    public interface IExportTRService
    {
        ExportTR Get(int id, Int64 nUserID);
        List<ExportTR> Gets( Int64 nUserID);
        List<ExportTR> Gets(bool bActivity, int nBUID, Int64 nUserID);
        List<ExportTR> BUWiseGets( int nBUID, Int64 nUserID);
        List<ExportTR> Gets(string sSQL, Int64 nUserID);
        string Delete(int id, Int64 nUserID);
        ExportTR Save(ExportTR oExportTR, Int64 nUserID);
    }
    #endregion
}
