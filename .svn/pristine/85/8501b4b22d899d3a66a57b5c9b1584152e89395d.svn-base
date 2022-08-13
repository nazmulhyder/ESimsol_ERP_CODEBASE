using System;
using System.IO;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using ICS.Core.Framework;
using ICS.Core.Utility;
using System.Data;

namespace ESimSol.BusinessObjects
{
    #region InventoryTransaction
     
    public class InventoryTransaction : BusinessObject
    {
        #region  Constructor
        public InventoryTransaction()
        {
           DateTime = DateTime.Now;
           ITransactionID = 0;
           ProductID = 0;
           Qty = 0;
           CurrentBalance = 0;
           UnitPrice = 0;
           UserID = 0;
           TriggerParentID = 0;
           TriggerParentType = (int)EnumTriggerParentsType.None;
           LotID = 0;
           LotNo = "";
           InOutType = (int)EnumInOutType.None;
           
           Description = "";
           ErrorMessage = "";
         
        }
        #endregion

       #region Properties
       
       public int ITransactionID { get; set; }
       public DateTime DateTime { get; set; }
       public int ProductID { get; set; }
       public int LotID { get; set; }
       public double Qty { get; set; }
       public double CurrentBalance { get; set; }
       public double UnitPrice { get; set; }
       public int CurrencyID { get; set; }
       public int MUnitID { get; set; }
       public int InOutType { get; set; }
       public string Description { get; set; }
       public int WorkingUnitID { get; set; }
       public int TriggerParentID { get; set; }
       public int TriggerParentType { get; set; }
       public int UserID { get; set; }

       #region Derived Properties
       public double Amount { get; set; }
       public string ErrorMessage { get; set; }
       public int OperationUnitID { get; set; }
       public int SL { get; set; }
       public string LocationName { get; set; }
       public string OperationUnitName { get; set; }
       public string LotNo { get; set; }
       public string ProductName { get; set; }
       public string ProductCode { get; set; }
       public string LCNo { get; set; }
       public string MUnitName { get; set; }
       public string Currency { get; set; }
       public int ProductCategoryID { get; set; }
       public DateTime EndDate { get; set; }
       public string StartDateSt
       {
           get
           {
               return this.DateTime.ToString("dd MMM yyyy 08:00:00");
           }
       }
       public string EndDateSt
       {
           get
           {
               return this.EndDate.ToString("dd MMM yyyy 08:00:00");
           }
       }

       #region  property for NonDB
   
       public string WorkingUnitName
        {
            get { return this.LocationName+"["+this.OperationUnitName+"]"; }
        }
      
       public string InOutName
       {
            get { return ((EnumInOutType)this.InOutType).ToString(); }
       }
       public string TriggerParentTypeSt
       {
           get { return ((EnumTriggerParentsType)this.TriggerParentType).ToString(); }
       }
        
       public string ProductNameCode { get; set; }
       public string AmountSt
       {
           get
           {
               return this.Currency + " " + Global.MillionFormat(this.Amount);
           }
       }

       #region ClousingBalance
       private double _nClousingBalance = 0;
       public double ClousingBalance
       {
           get;
           set;
       }
        #endregion
       #endregion

       
     


       #endregion
       #endregion

        #region Functions

     
        public static List<InventoryTransaction> Gets(string sSQL, long nUserID)
        {
            return InventoryTransaction.Service.Gets(sSQL, nUserID);
        }
        public static List<InventoryTransaction> Gets(int nTriggerParentID, int eEnumTriggerParentsType, int eInOutType, int nProductID,string sStoreIDs, long nUserID)
        {
            return InventoryTransaction.Service.Gets(nTriggerParentID, eEnumTriggerParentsType, eInOutType, nProductID, sStoreIDs, nUserID);
        }
        public static List<InventoryTransaction> GetsBY(int nEnumTriggerParentsType, int eInOutType, DateTime dTranDate, DateTime dTranDateTo, string sWorkingUnitIDs, long nUserID)
        {
            return InventoryTransaction.Service.GetsBY(nEnumTriggerParentsType, eInOutType, dTranDate, dTranDateTo, sWorkingUnitIDs, nUserID);
        }
        public static List<InventoryTransaction> GetsByLotID(int nProductID, string sLotIDs, long nUserID)
        {
            return InventoryTransaction.Service.GetsByLotID(nProductID, sLotIDs, nUserID);
        }
        public static List<InventoryTransaction> GetsStockByLotID(int nLotID, long nUserID)
        {
            return InventoryTransaction.Service.GetsStockByLotID(nLotID, nUserID);
        }
        public static List<InventoryTransaction> GetsByInvoice(int nTriggerParentID, int eTriggerParentType,int nProductID, int nInoutType, long nUserID)
        {
            return InventoryTransaction.Service.GetsByInvoice(nTriggerParentID,  eTriggerParentType, nProductID,  nInoutType, nUserID);
        }

        
        #endregion
        #region [NonDB Functions]

      
        public static double TotalQty(List<InventoryTransaction> oITs)
        {
            double nTotalWeight = 0;
            foreach (InventoryTransaction oItem in  oITs)
            {
                nTotalWeight = nTotalWeight + oItem.Qty;
            }
            return nTotalWeight;
        }
     
     
        public double TotalWeightKG()
        {
            return Global.GetKG(this.TotalWeightKG(), 2);
        }
        #endregion
        #region ServiceFactory
        internal static IInventoryTransactionService Service
        {
            get { return (IInventoryTransactionService)Services.Factory.CreateService(typeof(IInventoryTransactionService)); }
        }
        #endregion
    }
    #endregion
    
    #region IInventoryTransaction interface
     public interface IInventoryTransactionService
     {

         List<InventoryTransaction> GetsByInvoice(int nTriggerParentID, int eTriggerParentType, int nProductID, int nInoutType, Int64 nUserID);
         List<InventoryTransaction> GetsBY(int nEnumTriggerParentsType, int eInOutType, DateTime dTranDate, DateTime dTranDateTo, string sWorkingUnitIDs, Int64 nUserID);
         List<InventoryTransaction> Gets(int nTriggerParentID, int eEnumTriggerParentsType, int eInOutType, int nProductID, string sStoreIDs, Int64 nUserID);
         List<InventoryTransaction> GetsByLotID(int nProductID, string sLotIDs, Int64 nUserID);
         List<InventoryTransaction> Gets(string sSQL, Int64 nUserID);
         List<InventoryTransaction> GetsStockByLotID(int nLotID, Int64 nUserID);
     }
    #endregion
}