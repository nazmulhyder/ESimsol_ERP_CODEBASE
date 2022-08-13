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

    #region ProformaInvoiceHistory
    
    public class ProformaInvoiceHistory : BusinessObject
    {
        public ProformaInvoiceHistory()
        {
            ProformaInvoiceHistoryID = 0;
            ProformaInvoiceID = 0;
            PreviousStatus = EnumPIStatus.Initialized;
            CurrentStatus = EnumPIStatus.Initialized;
            OperateBy = 0;
            Note = "";
            OperateByName = "";
            PINo = "";
            OperationDateTime = DateTime.Now;
            ErrorMessage = "";
        }

        #region Properties

         
        public int ProformaInvoiceHistoryID { get; set; }
         
        public int ProformaInvoiceID { get; set; }
         
        public EnumPIStatus PreviousStatus { get; set; }
         
        public EnumPIStatus CurrentStatus { get; set; }
         
        public int OperateBy { get; set; }
         
        public string Note { get; set; }
         
        public string OperateByName { get; set; }
         
        public string PINo { get; set; }
        
         
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

        public static List<ProformaInvoiceHistory> Gets(int ProfromaInvoiceID, long nUserID)
        {

            return ProformaInvoiceHistory.Service.Gets(ProfromaInvoiceID, nUserID);
        }
        public static List<ProformaInvoiceHistory> Gets(string sSQL, long nUserID)
        {
            
            return ProformaInvoiceHistory.Service.Gets(sSQL, nUserID);
        }
        public ProformaInvoiceHistory Get(int id, long nUserID)
        {
            return ProformaInvoiceHistory.Service.Get(id, nUserID);
        }

        public ProformaInvoiceHistory Save(long nUserID)
        {
             return ProformaInvoiceHistory.Service.Save(this, nUserID);
        }
        #endregion

        #region ServiceFactory
 
        internal static IProformaInvoiceHistoryService Service
        {
            get { return (IProformaInvoiceHistoryService)Services.Factory.CreateService(typeof(IProformaInvoiceHistoryService)); }
        }

        #endregion
    }
    #endregion

    #region IProformaInvoiceHistory interface
     
    public interface IProformaInvoiceHistoryService
    {
         
        ProformaInvoiceHistory Get(int id, Int64 nUserID);
         
        List<ProformaInvoiceHistory> Gets(int ProfromaInvoiceID, Int64 nUserID);
         
        List<ProformaInvoiceHistory> Gets(string sSQL, Int64 nUserID);
         
        ProformaInvoiceHistory Save(ProformaInvoiceHistory oProformaInvoiceHistory, Int64 nUserID);


    }
    #endregion
    

}
