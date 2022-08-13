using System;
using System.IO;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using ICS.Core.Framework;
using ICS.Core.Utility;
namespace ESimSol.BusinessObjects
{
    #region TransferableLot
    public class TransferableLot : BusinessObject
    {
        #region  Constructor
        public TransferableLot()
        {
            TransferableLotID = 0;
            LotNo = "";
            Qty = 0;
            Product = "";
         
        }
        #endregion

        #region Properties  
       
        public int TransferableLotID { get; set; }
        public int LotID { get; set; }
        public int WorkingUnitID { get; set; }
        public double Qty { get; set; }
        public string LotNo { get; set; }
        public string Product { get; set; }
        public string WUName { get; set; }
        public string OrderType { get; set; }
        public string ErrorMessage { get; set; }
       public int WorkingUnitID_Recd { get; set; }
        #endregion

        #region Functions
       public static List<TransferableLot> Gets( long nUserID)
       {
           return TransferableLot.Service.Gets(nUserID);
       }
       public static List<TransferableLot> SendToRequsition(List<TransferableLot> oItems, long nUserID)
       {
           return TransferableLot.Service.SendToRequsition(oItems, nUserID);
       }
       public string Delete( long nUserID)
       {
           return TransferableLot.Service.Delete(this, nUserID);
       }
       public string TransferToStore(long nUserID)
       {
           return TransferableLot.Service.TransferToStore(this, nUserID);
       }
       public string LotAdjustment(long nUserID)
       {
           return TransferableLot.Service.LotAdjustment(this, nUserID);
       }
    
        
        #endregion

        #region Non DB Function

        #endregion
        #region ServiceFactory

     
        internal static ITransferableLotService Service
        {
            get { return (ITransferableLotService)Services.Factory.CreateService(typeof(ITransferableLotService)); }
        }
        #endregion
    }
    #endregion

    
    #region ITransferableLot interface
    public interface ITransferableLotService
    {
        List<TransferableLot> Gets( Int64 nUserID);
        List<TransferableLot> SendToRequsition(List<TransferableLot> oTransferableLot, long nUserID);
         string Delete(TransferableLot oTransferableLot, Int64 nUserID);
         string TransferToStore(TransferableLot oTransferableLot, Int64 nUserID);
         string LotAdjustment(TransferableLot oTransferableLot, Int64 nUserID);
    }
    
    #endregion

}