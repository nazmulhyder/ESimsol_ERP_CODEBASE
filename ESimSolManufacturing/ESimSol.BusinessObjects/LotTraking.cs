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
    #region LotTraking
    public class LotTraking : BusinessObject
    {
        #region  Constructor
        public LotTraking()
        {
            DateTime = DateTime.Now;
            EndDate = DateTime.Now;
           ProductID = 0;
           OpeningQty = 0;
           Qty = 0;
           OutQty = 0;
           ClosingQty = 0;
           TriggerParentID = 0;
           TriggerParentType =0;
           LotID = 0;
           InOutType = 0;
           LotNo = "";
           WorkingUnitID = 0;
           LotNo = "";
           ContractorName = "";
           OrderNo = "";
           WUName = "";
           MUnit = "";
           ProductName = "";
           MUnitID = 0;
           TriggerNo = "";
           ProductCode = "";
           ReceiveQty = 0;
        }
        #endregion

       #region Properties
       
       public int ProductID { get; set; }
       public int LotID { get; set; }
       public double OpeningQty { get; set; }
       public double Qty { get; set; }
       public double UnitPrice { get; set; }
       public double OutQty { get; set; }
       public double ClosingQty { get; set; }
       public int MUnitID { get; set; }
       public int InOutType { get; set; }
       public int WorkingUnitID { get; set; }
       public int TriggerParentID { get; set; }
       public int TriggerParentType { get; set; }
       #region Derived Properties
       public string WUName { get; set; }
       public string LotNo { get; set; }
       public string TriggerNo { get; set; }
       public string ProductName { get; set; }
       public string ProductCode { get; set; }
       public string ContractorName { get; set; }
       public string OrderNo { get; set; }
       public string LCNo { get; set; }
       public double ReceiveQty { get; set; }
       public string MUnit { get; set; }
       public int DateType { get; set; }
       public DateTime DateTime { get; set; }
       public DateTime EndDate { get; set; }
       public string DateTimeSt
       {
           get
           {
               return this.DateTime.ToString("dd MMM yyyy");
           }
       }
       public string EndDateSt
       {
           get
           {
               return this.EndDate.ToString("dd MMM yyyy");
           }
       }
       public string InOutTypeSt
       {
           get
           {
               return ((EnumInOutType)this.InOutType).ToString();
           }
       }
     
       public string TriggerParentTypeSt
       {
           get
           {
               return EnumObject.jGet((EnumTriggerParentsType)this.TriggerParentType);
           }
       }

       #endregion
       #endregion

        #region Functions
     
    
       public static List<LotTraking> Gets_Lot(int nBUID,string sLotIDs, long nUserID)
       {
           return LotTraking.Service.Gets_Lot(nBUID,sLotIDs, nUserID);
       }
      
        #endregion
        #region [NonDB Functions]

        #endregion
        #region ServiceFactory
    
        internal static ILotTrakingService Service
        {
            get { return (ILotTrakingService)Services.Factory.CreateService(typeof(ILotTrakingService)); }
        }
        #endregion
    }
    #endregion
 
    
    #region ILotTraking interface
     public interface ILotTrakingService
     {

     
         List<LotTraking> Gets_Lot(int nBUID,string sLotIDs, Int64 nUserID);
         
     }
    #endregion
}