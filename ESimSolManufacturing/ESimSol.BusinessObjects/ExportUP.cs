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
    #region ExportUP
    public class ExportUP : BusinessObject
    {
        public ExportUP()
        {
            ExportUPID = 0;
            UPNo = string.Empty;
            ExportUPDate = DateTime.Today;
            UPStatus = EnumUPStatus.WaitingForUD;
            PrepareBYID = 0;
            PrepareDate = DateTime.MinValue;
            DeliveryByID = 0;
            DeliveryDate = DateTime.MinValue;
            ApproveByID = 0;
            ApproveDate = DateTime.MinValue;
            ExportUPSetupID = 0;
            BUID = 0;
            ErrorMessage = "";
            ExportUPDetails = new List<ExportUPDetail>();
        }
        #region properties
        public int ExportUPID { get; set; }
        public int BUID { get; set; }
        public string UPNo { get; set; }
        public DateTime ExportUPDate { get; set; }
        public EnumUPStatus UPStatus { get; set; }
        public int PrepareBYID { get; set; }
        public DateTime PrepareDate { get; set; }
        public int DeliveryByID { get; set; }
        public DateTime DeliveryDate { get; set; }
        public int ApproveByID { get; set; }
        public DateTime ApproveDate { get; set; }
        public int ExportUPSetupID { get; set; }
        public string ErrorMessage { get; set; }
        #endregion

        #region Dereived Property

        public string UPNoWithYear
        {
            get { return this.UPNo + "/" + this.ExportUPDate.Year.ToString().Substring(2,2);  }
        }

        public string UPStatusStr
        {
            get { return this.UPStatus.ToString(); }
        }

        public string ExportUPDateStr
        {
            get { return this.ExportUPDate.ToString("dd MMM yyyy"); }
        }

        public string PrepareDateStr
        {
            get { return (this.PrepareDate == DateTime.MinValue) ? "" : this.PrepareDate.ToString("dd MMM yyyy"); }
        }

        public string DeliveryDateStr
        {
            get { return (this.DeliveryDate == DateTime.MinValue) ? "" : this.DeliveryDate.ToString("dd MMM yyyy"); }
        }

        public string ApproveDateStr
        {
            get { return (this.ApproveDate==DateTime.MinValue)? "" : this.ApproveDate.ToString("dd MMM yyyy"); }
        }
        
        public string PrepareByName { get; set; }
        public string DeliveryByName { get; set; }
        public string ApproveByName { get; set; }

        public List<ExportUPDetail> ExportUPDetails { get; set; }
        #endregion

        #region Functions
        public static ExportUP Get(int nExportUPID, long nUserID)
        {
            return ExportUP.Service.Get(nExportUPID, nUserID);
        }
        public static List<ExportUP> Gets(string sSQL, long nUserID)
        {
            return ExportUP.Service.Gets(sSQL, nUserID);
        }
        public ExportUP IUD(long nUserID)
        {
            return ExportUP.Service.IUD(this, nUserID);
        }
        public ExportUP Approve(long nUserID)
        {
            return ExportUP.Service.Approve(this, nUserID);
        }
        public string Delete(int id, long nUserID)
        {
            return ExportUP.Service.Delete(id, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IExportUPService Service
        {
            get { return (IExportUPService)Services.Factory.CreateService(typeof(IExportUPService)); }
        }
        #endregion
    }

    #endregion

    #region IExportUPService interface

    public interface IExportUPService
    {

        ExportUP Get(int nExportUPID, Int64 nUserID);
        List<ExportUP> Gets(string sSQL, Int64 nUserID);
        ExportUP IUD(ExportUP oExportUP, Int64 nUserID);
        ExportUP Approve(ExportUP oExportUP, Int64 nUserID);
        string Delete(int id, Int64 nUserID);
    }
    #endregion
}
