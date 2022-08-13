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
    #region ExportUD
    public class ExportUD : BusinessObject
    {
        public ExportUD()
        {
            ExportUDID = 0;
            ExportLCID = 0;
            ANo = 0;
            Amount = 0;
            UDNo = "";
            UDReceiveDate = DateTime.Now;
            ReceiveBYID = 0;
            ReceiveFrom = "";
            ContractNo = "";
            Note = "";
            AUDNo = "";
            ADate = DateTime.MinValue;
            ErrorMessage = "";
            ExportUDDetails = new List<ExportUDDetail>();
            YetToUPAmount = 0;
        }

        #region Property
        public int ExportUDID { get; set; }
        public int ExportLCID { get; set; }
        public int ANo { get; set; }
        public double Amount { get; set; }
        public double YetToUPAmount { get; set; }
        public string UDNo { get; set; }
        public DateTime UDReceiveDate { get; set; }
        public int ReceiveBYID { get; set; }
        public string ReceiveFrom { get; set; }
        public string ContractNo { get; set; }
        public string Note { get; set; }
        public string ExportLCNo { get; set; }
        public string ApplicantName { get; set; }
        public DateTime LCOpeningDate { get; set; }
        public string AUDNo { get; set; }
        public Nullable<DateTime> ADate { get; set; }
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property
        public int BUID { get; set; }
        public List<ExportUDDetail> ExportUDDetails { get; set; }
        public string UDReceiveDateInString
        {
            get
            {
                return UDReceiveDate.ToString("dd MMM yyyy");
            }
        }
        public string LCOpeningDateInString
        {
            get
            {
                return LCOpeningDate.ToString("dd MMM yyyy");
            }
        }
        public string ADateInString
        {
            get
            {
                if (ADate == DateTime.MinValue) return "";
                return ADate.Value.ToString("dd MMM yyyy");
            }
        }
        #endregion 


        #region Functions
        public static List<ExportUD> Gets(long nUserID)
        {
            return ExportUD.Service.Gets(nUserID);
        }
        public static List<ExportUD> Gets(string sSQL, long nUserID)
        {
            return ExportUD.Service.Gets(sSQL, nUserID);
        }
        public ExportUD Get(int id, long nUserID)
        {
            return ExportUD.Service.Get(id, nUserID);
        }
        public ExportUD Save(long nUserID)
        {
            return ExportUD.Service.Save(this, nUserID);
        }
        public string Delete(int id, long nUserID)
        {
            return ExportUD.Service.Delete(id, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IExportUDService Service
        {
            get { return (IExportUDService)Services.Factory.CreateService(typeof(IExportUDService)); }
        }
        #endregion
    }
    #endregion

    #region IExportUD interface
    public interface IExportUDService
    {
        ExportUD Get(int id, Int64 nUserID);
        List<ExportUD> Gets(Int64 nUserID);
        List<ExportUD> Gets(string sSQL, Int64 nUserID);
        string Delete(int id, Int64 nUserID);
        ExportUD Save(ExportUD oExportUD, Int64 nUserID);


    }
    #endregion
}
