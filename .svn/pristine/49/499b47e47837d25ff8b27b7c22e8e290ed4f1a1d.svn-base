using System;
using System.IO;
using ICS.Core;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using ICS.Core.Framework;
using ICS.Core.Utility;


namespace ESimSol.BusinessObjects
{
    #region RMClosingStock
    
    public class RMClosingStock : BusinessObject
    {
        public RMClosingStock()
        {
            RMClosingStockID = 0;
            SLNo="";
            BUID = 0;
            BUName = "";
            AccountingSessionID = 0;
            AccountingSessionName = "";
            RMAccountHeadID = 0; 
            RMAccountHeadName = "";
            ClosingStockValue=0;
            StockDate=DateTime.Now;
            ApprovedBy = 0;
            ApprovedByName = "";
            Remarks="";
            RMClosingStockDetails = new List<RMClosingStockDetail>();
        }

        #region Properties
        public int   RMClosingStockID { get; set; }     
        public string SLNo { get; set; }
        public int BUID { get; set; }
        public string BUName { get; set; }
        public int AccountingSessionID { get; set; }
        public string AccountingSessionName { get; set; }
        public int RMAccountHeadID { get; set; }
        public string RMAccountHeadName { get; set; }     
        public double ClosingStockValue { get; set; }     
        public DateTime StockDate { get; set; }     
        public int ApprovedBy { get; set; }
        public string ApprovedByName { get; set; }
        public string Remarks { get; set; }   
        public string ErrorMessage { get; set; }        
      
        #endregion

        #region Derived Property      
        public List<RMClosingStockDetail> RMClosingStockDetails { get; set; }
        public string StockDateST 
        {
            get { return this.StockDate.ToString("dd MMM yyyy"); }
        }
        public string ClosingStockValueST
        {
            get { return Global.MillionFormat(this.ClosingStockValue); }
        }
        #endregion

        #region Functions
        public static List<RMClosingStock> Gets(long nUserID)
        {
            return RMClosingStock.Service.Gets( nUserID);
        }
     
        public static List<RMClosingStock> GetsByName(string sName,  long nUserID)
        {
            return RMClosingStock.Service.GetsByName(sName,  nUserID);
        }

    
        public RMClosingStock Get(int id, long nUserID)
        {
            return RMClosingStock.Service.Get(id, nUserID);
        }
    
        public RMClosingStock Save(long nUserID)
        {
            return RMClosingStock.Service.Save(this, nUserID);
        }
        public RMClosingStock Approve(long nUserID)
        {
            return RMClosingStock.Service.Approve(this, nUserID);
        }
        public RMClosingStock UndoApprove(long nUserID)
        {
            return RMClosingStock.Service.UndoApprove(this, nUserID);
        }
        public static List<RMClosingStock> Gets(string sSQL, long nUserID)
        {
            return RMClosingStock.Service.Gets(sSQL, nUserID);
        }
        public string Delete(int id, long nUserID)
        {
            return RMClosingStock.Service.Delete(id, nUserID);
        }

     
   
        #endregion

        #region ServiceFactory
        internal static IRMClosingStockService Service
        {
            get { return (IRMClosingStockService)Services.Factory.CreateService(typeof(IRMClosingStockService)); }
        }

        #endregion
    }
    #endregion

    #region IRMClosingStock interface
     
    public interface IRMClosingStockService
    {
         
        RMClosingStock Get(int id, Int64 nUserID);
         
        List<RMClosingStock> Gets(Int64 nUserID);
     
        List<RMClosingStock> Gets(string sSQL, Int64 nUserID);
         
        string Delete(int id, Int64 nUserID);

        RMClosingStock Approve(RMClosingStock oRMClosingStock, Int64 nUserID);
        RMClosingStock UndoApprove(RMClosingStock oRMClosingStock, Int64 nUserID);
        RMClosingStock Save(RMClosingStock oRMClosingStock, Int64 nUserID);

        List<RMClosingStock> GetsByName(string sName,  Int64 nUserID);
         
        
    }
    #endregion
}