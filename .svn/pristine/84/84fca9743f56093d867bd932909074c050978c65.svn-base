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

    #region MasterLCHistory
    
    public class MasterLCHistory : BusinessObject
    {
        public MasterLCHistory()
        {
            MasterLCHistoryID = 0;
            MasterLCID = 0;
            PreviousStatus = EnumLCStatus.None;
            CurrentStatus = EnumLCStatus.None;
            OperateBy = 0;
            Note = "";
            OperateByName = "";
            MasterLCNo = "";
            OperationDateTime = DateTime.Now;
            ErrorMessage = "";
        }

        #region Properties

         
        public int MasterLCHistoryID { get; set; }
         
        public int MasterLCID { get; set; }
         
        public EnumLCStatus PreviousStatus { get; set; }
         
        public EnumLCStatus CurrentStatus { get; set; }
         
        public int OperateBy { get; set; }
         
        public string Note { get; set; }
         
        public string OperateByName { get; set; }
         
        public string MasterLCNo { get; set; }

         
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

        public static List<MasterLCHistory> Gets(int ProfromaInvoiceID, long nUserID)
        {
            return MasterLCHistory.Service.Gets(ProfromaInvoiceID, nUserID);
        }
        public static List<MasterLCHistory> Gets(string sSQL, long nUserID)
        {
            return MasterLCHistory.Service.Gets(sSQL, nUserID);
        }
        public MasterLCHistory Get(int id, long nUserID)
        {
            return MasterLCHistory.Service.Get(id, nUserID);
        }

        public MasterLCHistory Save(long nUserID)
        {           
            return MasterLCHistory.Service.Save(this, nUserID);
        }
        #endregion

        #region ServiceFactory

        internal static IMasterLCHistoryService Service
        {
            get { return (IMasterLCHistoryService)Services.Factory.CreateService(typeof(IMasterLCHistoryService)); }
        }

        #endregion
    }
    #endregion

    #region IMasterLCHistory interface
     
    public interface IMasterLCHistoryService
    {
         
        MasterLCHistory Get(int id, Int64 nUserID);
         
        List<MasterLCHistory> Gets(int ProfromaInvoiceID, Int64 nUserID);
         
        List<MasterLCHistory> Gets(string sSQL, Int64 nUserID);
         
        MasterLCHistory Save(MasterLCHistory oMasterLCHistory, Int64 nUserID);


    }
    #endregion
    
    
}
