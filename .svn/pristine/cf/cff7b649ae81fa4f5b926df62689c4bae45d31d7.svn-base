using System;
using System.IO;
using ICS.Core;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using ICS.Core.Framework;
using ICS.Core.Utility;
using ESimSol.BusinessObjects.ReportingObject;

namespace ESimSol.BusinessObjects
{


    #region CostSheetHistory
    
    public class CostSheetHistory : BusinessObject
    {
        public CostSheetHistory()
        {
            CostSheetHistoryID = 0;
            CostSheetID = 0;
            PreviousStatus = EnumCostSheetStatus.None;
            CurrentStatus = EnumCostSheetStatus.None;
            OperationBy = 0;
            Note = "";
            OperateByName = "";
            FileNo = "";
            OperationDateTime = DateTime.Now;
            ErrorMessage = "";
        }

        #region Properties

         
        public int CostSheetHistoryID { get; set; }
         
        public int CostSheetID { get; set; }

        public EnumCostSheetStatus PreviousStatus { get; set; }

        public EnumCostSheetStatus CurrentStatus { get; set; }
         
        public int OperationBy { get; set; }
         
        public string Note { get; set; }
         
        public string OperateByName { get; set; }
         
        public string FileNo { get; set; }

         
        public DateTime OperationDateTime { get; set; }
         
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property

        public string PreviousStatusInString
        {
            get
            {
                return this.PreviousStatus.ToString();
            }
        }
        public string CurrentStatusInString
        {
            get
            {
                return this.CurrentStatus.ToString();
            }
        }

        public string OperationDateTimeInString
        {
            get
            {
                return this.OperationDateTime.ToString("dd MMM yyyy hh:mm:ss tt");
            }
        }




        #endregion

        #region Functions

        public static List<CostSheetHistory> Gets(int nCostSheetID, long nUserID)
        {
            return CostSheetHistory.Service.Gets(nCostSheetID, nUserID);
        }
        public static List<CostSheetHistory> Gets(string sSQL, long nUserID)
        {
            return CostSheetHistory.Service.Gets(sSQL, nUserID);
        }
        public CostSheetHistory Get(int id, long nUserID)
        {
            return CostSheetHistory.Service.Get(id, nUserID);
        }

        public CostSheetHistory Save(long nUserID)
        {           
            return CostSheetHistory.Service.Save(this, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static ICostSheetHistoryService Service
        {
            get { return (ICostSheetHistoryService)Services.Factory.CreateService(typeof(ICostSheetHistoryService)); }
        }

        #endregion
    }
    #endregion

    #region ICostSheetHistory interface
     
    public interface ICostSheetHistoryService
    {
         
        CostSheetHistory Get(int id, Int64 nUserID);
         
        List<CostSheetHistory> Gets(int ProfromaInvoiceID, Int64 nUserID);
         
        List<CostSheetHistory> Gets(string sSQL, Int64 nUserID);
         
        CostSheetHistory Save(CostSheetHistory oCostSheetHistory, Int64 nUserID);


    }
    #endregion
    
   
}
