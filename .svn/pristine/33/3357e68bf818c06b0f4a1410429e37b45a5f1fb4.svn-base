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
    #region RMClosingStockDetail
    [DataContract]
    public class RMClosingStockDetail : BusinessObject
    {
        private object obj = null;
        #region  Constructor
        public RMClosingStockDetail()
        {
            
            RMClosingStockDetailID = 0;
            RMClosingStockID = 0;
            RMAccountHeadID = 0;
            Amount = 0;
            AccountCode ="";
            AccountHeadName = "";
            ParentHeadName = "";
        }
        #endregion

        #region Properties
        public int RMClosingStockDetailID { get; set; }
        public int RMClosingStockID { get; set; }
        public int RMAccountHeadID { get; set; }
        public double Amount { get; set; }
        public string AccountCode { get; set; }
        public string AccountHeadName { get; set; }
        public string ParentHeadName { get; set; }
    
        #endregion
      

        #region Derived PRoperty

        public RMClosingStock RMClosingStock { get; set; }
        public List<RMClosingStockDetail> RMClosingStockDetails { get; set; }
   
 
        #endregion

        #region Functions
        public RMClosingStockDetail Get(int nId, Int64 nUserID)
        {
            return RMClosingStockDetail.Service.Get(nId, nUserID);
        }
      
        public string Delete(Int64 nUserID)
        {
            return RMClosingStockDetail.Service.Delete(this, nUserID);
        }
     
        #region  Collection Functions

        public static List<RMClosingStockDetail> Gets(int nLCBillID, Int64 nUserID)
        {
            return RMClosingStockDetail.Service.Gets(nLCBillID, nUserID);
        }
      
        public static List<RMClosingStockDetail> GetsBySQL(string sSQL, Int64 nUserID)
        {
            return RMClosingStockDetail.Service.GetsBySQL(sSQL, nUserID);
        }
       

     
        #endregion
        #endregion
        #region ServiceFactory

        internal static IRMClosingStockDetailService Service
        {
            get { return (IRMClosingStockDetailService)Services.Factory.CreateService(typeof(IRMClosingStockDetailService)); }
        }

        #endregion

    }
    #endregion

    #region IRMClosingStockDetail interface
    [ServiceContract]
    public interface IRMClosingStockDetailService
    {
        RMClosingStockDetail Get(int id, Int64 nUserID);
        List<RMClosingStockDetail> Gets(int nLCBillID, Int64 nUserID);
        List<RMClosingStockDetail> GetsBySQL(string sSQL, Int64 nUserID);
        string Delete(RMClosingStockDetail oRMClosingStockDetail, Int64 nUserID);

    }
    #endregion
}
