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
    #region CommercialInvoiceHistory
    
    public class CommercialInvoiceHistory : BusinessObject
    {
        public CommercialInvoiceHistory()
        {
            CommercialInvoiceHistoryID = 0;
            CommercialInvoiceID = 0;
            PreviousStatus = EnumCommercialInvoiceStatus.Initialized;
            CurrentStatus = EnumCommercialInvoiceStatus.Initialized;
            Note = "";
            OperateBy = 0;
            OperateByName = "";
            InVoiceNo = "";
            OperationDateTime = DateTime.Now;
            ErrorMessage = "";
        }

        #region Properties
         
        public int CommercialInvoiceHistoryID { get; set; }
         
        public int CommercialInvoiceID { get; set; }
         
        public EnumCommercialInvoiceStatus PreviousStatus { get; set; }
         
        public EnumCommercialInvoiceStatus CurrentStatus { get; set; }
         
        public string Note { get; set; }
         
        public int OperateBy { get; set; }
         
        public string OperateByName { get; set; }
         
        public string InVoiceNo { get; set; }
         
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
        public static List<CommercialInvoiceHistory> Gets(int ProformaInvoiceID, long nUserID)
        {
            return CommercialInvoiceHistory.Service.Gets(ProformaInvoiceID, nUserID);
        }
        public static List<CommercialInvoiceHistory> Gets(string sSQL, long nUserID)
        {
            return CommercialInvoiceHistory.Service.Gets(sSQL, nUserID);
        }
        public CommercialInvoiceHistory Get(int CommercialInvoiceHistoryID, long nUserID)
        {
            return CommercialInvoiceHistory.Service.Get(CommercialInvoiceHistoryID, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static ICommercialInvoiceHistoryService Service
        {
            get { return (ICommercialInvoiceHistoryService)Services.Factory.CreateService(typeof(ICommercialInvoiceHistoryService)); }
        }
        #endregion
    }
    #endregion

    #region ICommercialInvoiceHistory interface
     
    public interface ICommercialInvoiceHistoryService
    {
         
        CommercialInvoiceHistory Get(int CommercialInvoiceHistoryID, Int64 nUserID);
         
        List<CommercialInvoiceHistory> Gets(int ProformaInvoiceID, Int64 nUserID);
         
        List<CommercialInvoiceHistory> Gets(string sSQL, Int64 nUserID);
    }
    #endregion
}
