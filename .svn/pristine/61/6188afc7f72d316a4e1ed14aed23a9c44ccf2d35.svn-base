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
    public class FNProductionBatchTransfer :BusinessObject
    {
         public FNProductionBatchTransfer()
        {
            FNPBTransferID=0;
            FNPBTransferNo=string.Empty;
            FNTreatment=EnumFNTreatment.None;
            TransferNote="";
            TransferBy=0;
            TransferDate=DateTime.MinValue;
            ReceiveBy=0;
            ReceiveDate=DateTime.MinValue;
            Params = string.Empty;
            ErrorMessage = string.Empty;
            FNProductionBatchTransferDetails = new List<FNProductionBatchTransferDetail>();
        }
        #region FNProductionBatchTransfer
        public int FNPBTransferID{get; set;}
        public string FNPBTransferNo{get; set;}
        public EnumFNTreatment FNTreatment{get; set;}
        public string TransferNote{get; set;}
        public int TransferBy{get; set;}
        public DateTime TransferDate{get; set;}
        public int ReceiveBy{get; set;}
        public DateTime ReceiveDate { get; set; }
        public string Params { get; set; }
        public string ErrorMessage { get; set; }
        #endregion
        #region dreiverd Properties 
      
        public string TransferDateStr { get { return (this.TransferDate == DateTime.MinValue) ? "" : this.ReceiveDate.ToString("dd MMM yyyy"); } }
        public string ReceiveDateStr { get { return (this.ReceiveDate == DateTime.MinValue) ? "" : this.ReceiveDate.ToString("dd MMM yyyy"); } }

        public string FNTreatmentStr { get { return this.FNTreatment.ToString(); } }
        public string TransferByName {get; set;}
        public string RecvByName { get; set; }
        public List<FNProductionBatchTransferDetail> FNProductionBatchTransferDetails { get; set; }
        #endregion
        #region Functions   
        public FNProductionBatchTransfer IUD(int nDBOperation, long nUserID)
        {
            return FNProductionBatchTransfer.Service.IUD(this, nDBOperation, nUserID);
        }

        public static FNProductionBatchTransfer Get(int nFNPBTID, long nUserID)
        {
            return FNProductionBatchTransfer.Service.Get(nFNPBTID, nUserID);
        }
        public static List<FNProductionBatchTransfer> Gets(string sSQL, long nUserID)
        {
            return FNProductionBatchTransfer.Service.Gets(sSQL, nUserID);
        }

        #endregion

        #region ServiceFactory
        internal static IFNProductionBatchTransferService Service
        {
            get { return (IFNProductionBatchTransferService)Services.Factory.CreateService(typeof(IFNProductionBatchTransferService)); }
        }

        #endregion
        
    }
    #region  IFNProductionBatchTransfer interface
    public interface IFNProductionBatchTransferService
    {

        FNProductionBatchTransfer Get(int nFNPBTID, Int64 nUserID);
        List<FNProductionBatchTransfer> Gets(string sSQL, Int64 nUserID);
        FNProductionBatchTransfer IUD(FNProductionBatchTransfer oFNProductionBatchTransfer, int nDBOperation, Int64 nUserID);

    }
    #endregion
}
