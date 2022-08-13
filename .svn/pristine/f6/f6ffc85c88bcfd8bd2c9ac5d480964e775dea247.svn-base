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
    #region ExportPIHistory

    public class ExportPIHistory : BusinessObject
    {
        public ExportPIHistory()
        {
            ExportPIHistoryID = 0;
            ExportPIID = 0;
            PreviousStatus = EnumPIStatus.Initialized;            
            CurrentStatus = EnumPIStatus.Initialized;            
            Note = "";
            OperateBy = 0;
            OperateByName = "";
            DBServerDateTime = DateTime.Today;
            PINo = "";
            ErrorMessage = "";
        }

        #region Properties
        public int ExportPIHistoryID { get; set; }
        public int ExportPIID { get; set; }
        public string PINo { get; set; }
        public EnumPIStatus PreviousStatus { get; set; }        
        public EnumPIStatus CurrentStatus { get; set; }        
        public string Note { get; set; }
        public int OperateBy { get; set; }
        public string OperateByName { get; set; }
        public DateTime DBServerDateTime { get; set; }  
        public string ErrorMessage { get; set; }
        #endregion

        #region Derive Property
        public string DBServerDateTimeInString
        {
            get
            {
                return this.DBServerDateTime.ToString("dd MMM yyyy hh:mm tt");
            }
        }
        public string PreviousStatusInString
        {
            get
            {
                return EnumObject.jGet(this.PreviousStatus);
            }
        }
        public string CurrentStatusInString
        {
            get
            {
                return EnumObject.jGet(this.CurrentStatus);
            }
        }
        #endregion

        #region Functions
        public static List<ExportPIHistory> Gets(long nUserID)
        {
            return ExportPIHistory.Service.Gets(nUserID);
        }
        public static List<ExportPIHistory> GetsByExportId(int nExportPIID, long nUserID)
        {
            return ExportPIHistory.Service.GetsByExportId(nExportPIID, nUserID);
        }
        public ExportPIHistory Get(int nId, long nUserID)
        {
            return ExportPIHistory.Service.Get(nId, nUserID);
        }
        public ExportPIHistory Save(long nUserID)
        {
            return ExportPIHistory.Service.Save(this, nUserID);
        }
        public string Delete(int nId, long nUserID)
        {
            return ExportPIHistory.Service.Delete(nId, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IExportPIHistoryService Service
        {
            get { return (IExportPIHistoryService)Services.Factory.CreateService(typeof(IExportPIHistoryService)); }
        }
        #endregion
    }
    #endregion

    #region IExportPIHistory interface

    public interface IExportPIHistoryService
    {
        ExportPIHistory Get(int id, long nUserID);
        List<ExportPIHistory> Gets(long nUserID);
        List<ExportPIHistory> GetsByExportId(int nExportPIID, long nUserID);
        string Delete(int id, long nUserID);
        ExportPIHistory Save(ExportPIHistory oExportPIHistory, long nUserID);
    }
    #endregion
}
