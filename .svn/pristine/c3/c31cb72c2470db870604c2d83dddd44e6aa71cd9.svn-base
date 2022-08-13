using System;
using System.IO;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using System.Data;
using ICS.Core;
using ICS.Core.Framework;
using ICS.Core.Utility;

namespace ESimSol.BusinessObjects
{
    #region DUDeliverySummary
    public class DUDeliverySummary : BusinessObject
    {
        #region  Constructor
        public DUDeliverySummary()
        {
            RSNo = "";
            Buyer = "";
            ManagedQty = 0;
            Product = "";
            FactN = "";
            ErrorMessage = "";
            ChallanDate = "";
            ChallanID = 0;
            Recycle = 0;
        }
        #endregion

        #region Properties   
        public string RSNo { get; set; }
        public int RSID { get; set; }
        public int ChallanID { get; set; }
        public string ChallanNo { get; set; }
        public string RSDate { get; set; }
        public string ChallanDate { get; set; }
        public string OrderNo { get; set; }
        public string Buyer{get; set;}
        public string Location { get; set; }
        public string FactN { get; set; }
        public string DeliverTo { get; set; }
        public string Product { get; set; }
        public double RSQty { get; set; }
        public double TotalDel { get; set; }
        public double RawYarnIssue { get; set; }
        public double InSubFinish { get; set; }
        public double InFinishing { get; set; }
        public double FreshDyedYarn { get; set; }
        public double ManagedQty { get; set; }
        public double Recycle { get; set; }//
        public double Wastage { get; set; }
        public double Gain { get; set; }
        public double Loss { get; set; }
        public double DeliveredQty { get; set; }
        public double InWastageRec { get; set; }
        public double InRecycleRec { get; set; }
        public int OrderType { get; set; }
        public int ReportType { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string ErrorMessage { get; set; }

        #region Report Type=2
        public int OrderID { get; set; }
       public double Managed { get; set; }
       public double UnManage { get; set; }

        #endregion

        #endregion

        #region Functions

        public static List<DUDeliverySummary> Gets(DateTime dStartDate, DateTime dEndDate, int nOrderType, int nReportType, long nUserID)
        {
            return DUDeliverySummary.Service.Gets(dStartDate, dEndDate, nOrderType, nReportType, nUserID);
        }
       
        #endregion

        #region Non DB Function

        #endregion
        #region ServiceFactory
        internal static IDUDeliverySummaryService Service
        {
            get { return (IDUDeliverySummaryService)Services.Factory.CreateService(typeof(IDUDeliverySummaryService)); }
        }
        #endregion
    }
    #endregion

    
    #region IDUDeliverySummary interface
    public interface IDUDeliverySummaryService
    {
        List<DUDeliverySummary> Gets(DateTime dStartDate, DateTime dEndDate, int nOrderType, int nReportType,Int64 nUserID);
    }
    
    #endregion

}