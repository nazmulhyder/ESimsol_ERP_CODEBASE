using System;
using System.IO;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using System.Drawing;
using ICS.Core;
using ICS.Core.Framework;
using ICS.Core.Utility;
namespace ESimSol.BusinessObjects
{
    public class FNProductionBatchTransferDetail:BusinessObject
    {
        public FNProductionBatchTransferDetail()
        {
         
            FNPBTransferDetailID =0;
            FNPBTransferID=0;
            FNPBatchID=0;
            Params = string.Empty;
            ErrorMessage = string.Empty;
      
        }
        #region FNProductionBatchTransferDetail
        public int FNPBTransferDetailID{get; set;}
        public int FNPBTransferID{get; set;}
        public int FNPBatchID{get; set;}
        public string Params { get; set; }
        public string ErrorMessage { get; set; }
        #endregion
        #region dreiverd Properties
        public FNProductionBatchTransfer TempFNProductionBatchTransfer { get; set; }
        public List<FNProductionBatchTransferDetail> FNProductionBatchTransferDetails { get; set; }
        public string BatchNo{ get; set; }
        public double StartQty{ get; set; }
        public double EndQty{ get; set; }
        public string  ShiftName{ get; set; }
        public double StartWidth{ get; set; }
        public double EndWidth { get; set; }
        #endregion

        #region Functions
        public FNProductionBatchTransferDetail IUD(int nDBOperation, long nUserID)
        {
            return FNProductionBatchTransferDetail.Service.IUD(this, nDBOperation, nUserID);
        }

        public static FNProductionBatchTransferDetail Get(int nFNPBTDID, long nUserID)
        {
            return FNProductionBatchTransferDetail.Service.Get(nFNPBTDID, nUserID);
        }
        public static List<FNProductionBatchTransferDetail> Gets(string sSQL, long nUserID)
        {
            return FNProductionBatchTransferDetail.Service.Gets(sSQL, nUserID);
        }
      

        #endregion

        #region ServiceFactory
        internal static IFNProductionBatchTransferDetailService Service
        {
            get { return (IFNProductionBatchTransferDetailService)Services.Factory.CreateService(typeof(IFNProductionBatchTransferDetailService)); }
        }

        #endregion
    }
    #region  IFNProductionBatchTransferDetailService interface
    public interface IFNProductionBatchTransferDetailService
    {

        FNProductionBatchTransferDetail Get(int nFNPBTDID, Int64 nUserID);
        List<FNProductionBatchTransferDetail> Gets(string sSQL, Int64 nUserID);
        FNProductionBatchTransferDetail IUD(FNProductionBatchTransferDetail oFNProductionBatchTransferDetail, int nDBOperation, Int64 nUserID);

    }
    #endregion
}
