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
    #region ExportPISignatory
    public class ExportPISignatory : BusinessObject
    {
        public ExportPISignatory()
        {
            ExportPISignatoryID = 0;
            ExportPIID = 0;
            SLNo = 0;
            ReviseNo = 0;
            RequestTo = 0;
            ApproveBy = 0;
            ApproveDate = DateTime.Now;
            IsApprove = false;
            ErrorMessage = "";
            ApprovalHeadID = 0;
            Note = "";
        }

        #region Property
        public int ExportPISignatoryID { get; set; }
        public int ExportPIID { get; set; }
        public int SLNo { get; set; }
        public int ReviseNo { get; set; }
        public int RequestTo { get; set; }
        public int ApprovalHeadID { get; set; }
        public int ApproveBy { get; set; }
        public string Note { get; set; }
        public DateTime ApproveDate { get; set; }
        public bool IsApprove { get; set; }
        public string ErrorMessage { get; set; }
        public string HeadName { get; set; }
        #endregion

        #region Derived Property
        public string Name_Request { get; set; }
        public string ApproveDateSt
        {
            get
            {
                if (this.ApproveDate == DateTime.MinValue) return "";
                return ApproveDate.ToString("dd MMM yyyy");
            }
        }
        public string Status
        {
            get
            {
                if (this.ApproveBy <= 0) return " Wating For Approve";
                return "Approved";
            }
        }
        #endregion

        #region Functions
        public static List<ExportPISignatory> Gets(int nExportPIID, long nUserID)
        {
            return ExportPISignatory.Service.Gets(nExportPIID, nUserID);
        }
        public static List<ExportPISignatory> Gets(string sSQL, long nUserID)
        {
            return ExportPISignatory.Service.Gets(sSQL, nUserID);
        }
        public ExportPISignatory Get(int id, long nUserID)
        {
            return ExportPISignatory.Service.Get(id, nUserID);
        }

        public static List<ExportPISignatory> SaveAll(List<ExportPISignatory> oExportPISignatorys, string sExportPISignatoryID, long nUserID)
        {
            return ExportPISignatory.Service.SaveAll(oExportPISignatorys, sExportPISignatoryID, nUserID);
        }
        public ExportPISignatory Save(Int64 nUserID)
        {
            return ExportPISignatory.Service.Save(this, nUserID);
        }
        public string Delete(long nUserID)
        {
            return ExportPISignatory.Service.Delete(this, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IExportPISignatoryService Service
        {
            get { return (IExportPISignatoryService)Services.Factory.CreateService(typeof(IExportPISignatoryService)); }
        }
        #endregion
    }
    #endregion

    #region IExportPISignatory interface
    public interface IExportPISignatoryService
    {
        ExportPISignatory Get(int id, Int64 nUserID);
        List<ExportPISignatory> Gets(int nExportPIID, Int64 nUserID);
        List<ExportPISignatory> Gets(string sSQL, Int64 nUserID);
        string Delete(ExportPISignatory oExportPISignatory, Int64 nUserID);
        ExportPISignatory Save(ExportPISignatory oExportPISignatory, Int64 nUserID);
        List<ExportPISignatory> SaveAll(List<ExportPISignatory> oExportPISignatorys, string sExportPISignatoryID, Int64 nUserID);
    }
    #endregion
}
