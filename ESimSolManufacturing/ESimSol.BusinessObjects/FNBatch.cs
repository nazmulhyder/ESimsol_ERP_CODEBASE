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
    #region FUProcess
    public class FNBatch : BusinessObject
    {
        public FNBatch()
        {
            FNBatchID = 0;
            BatchNo = string.Empty;
            FNExOID = 0;
            Qty = 0;
            FNBatchStatus = EnumFNBatchStatus.InFloor;
            GLM = 0;
            FNPPID = 0;
            IssueDate = DateTime.Today;
            ExpectedDeliveryDate = DateTime.Today;
            OutQty = 0;
            GreyGSM = 0;
            QCQty = 0;
            FabricNo = "";
            ErrorMessage = "";
            PrepareByName = "";
            Params = "";
            IsFullTransfer = false;
            FNBatchTransferHistoryID = 0;
            SourceBatchID = 0;
            SourceBatchNo = "";
            DestinationBatchID = 0;
            DestinationBatchNo = "";
            UserID = 0;
            UserName = "";
            TransferTime = DateTime.MinValue;
            FNBatchCards = new List<FNBatchCard>();
            FNProductionBatchs = new List<FNProductionBatch>();
        }

        #region Properties
        public int FNBatchID { get; set; }
        public string BatchNo { get; set; }
        public int FNExOID { get; set; }
        public double Qty { get; set; }
        public EnumFNBatchStatus FNBatchStatus { get; set; }
        public double GLM { get; set; }
        public int FNPPID { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime ExpectedDeliveryDate { get; set; }
        public double GreyGSM { get; set; }
        public double QCQty { get; set; }
        public string PrepareByName { get; set; }
        public string FabricNo { get; set; }
        public string SCNoFull { get; set; }
        public string ErrorMessage { get; set; }
        public string Note { get; set; }
        public string Params { get; set; }
        #endregion

        #region Derived Property
        public bool IsFullTransfer { get; set; }
        public string ExpectedDeliveryDateStr { get { return this.ExpectedDeliveryDate.ToString("dd MMM yyyy"); } }
        public string IssueDateStr { get { return this.IssueDate.ToString("dd MMM yyyy"); } }
        public string FNBatchStatusStr { get { return this.FNBatchStatus.ToString(); } }
        public string FNExONo { get; set; }
        public double GSM { get; set; }
        public double ExeQty { get; set; }
        public string CountName { get; set; }
        public int BuyerID { get; set; }
        public string BuyerName { get; set; }
        public string Construction { get; set; }
        public double GreyWidth { get; set; }
        public string FinishWidth { get; set; }
        public int FinishType { get; set; }
        public string FinishTypeName { get; set; }
        public double OutQty { get; set; }
        public string MUnit { get; set; }
        public string FabricWeaveName { get; set; }
        public List<FNBatchCard> FNBatchCards { get; set; }
        public List<FNProductionBatch> FNProductionBatchs { get; set; }
        public double YetToOutQty
        {
            get
            {
                return this.Qty - this.OutQty;
            }
           
        }

        public double QtyInMtr
        {
            get
            {
                return Global.GetMeter(this.Qty, 2);
            }

        }

        public double OutQtyInMtr
        {
            get
            {
                return Global.GetMeter(this.OutQty, 2);
            }

        }

        public double QCQtyInMtr
        {
            get
            {
                return Global.GetMeter(this.QCQty, 2);
            }

        }
        

        #endregion

        #region Batch Transfer History Property
        public int FNBatchTransferHistoryID { get; set; }
        public int SourceBatchID { get; set; }
        public string SourceBatchNo { get; set; }
        public int DestinationBatchID { get; set; }
        public string DestinationBatchNo { get; set; }
        public int UserID { get; set; }
        public string UserName { get; set; }
        public DateTime TransferTime { get; set; }
        public string TransferTimeSt
        {
            get
            {
                if (TransferTime == DateTime.MinValue) return "";
                return TransferTime.ToString("dd MMM yyyy");
            }
        }
        #endregion

        #region Functions

        public static FNBatch Get(int nFNBatchID, long nUserID)
        {
            return FNBatch.Service.Get(nFNBatchID, nUserID);
        }
        public static List<FNBatch> Gets(string sSQL, long nUserID)
        {
            return FNBatch.Service.Gets(sSQL, nUserID);
        }
        public FNBatch IUD(int nDBOperation, long nUserID)
        {
            return FNBatch.Service.IUD(this, nDBOperation, nUserID);
        }
        public FNBatch SaveNote(string sSql, long nUserID)
        {
            return FNBatch.Service.SaveNote(sSql, nUserID);
        }
        public FNBatch TransferFNBatchCard(long nUserID)
        {
            return FNBatch.Service.TransferFNBatchCard(this, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IFNBatchService Service
        {
            get { return (IFNBatchService)Services.Factory.CreateService(typeof(IFNBatchService)); }
        }

        #endregion

    }
    #endregion

    #region IFUProcess interface

    public interface IFNBatchService
    {
        FNBatch SaveNote(string sSQL, Int64 nUserID);
        FNBatch Get(int nFNBatchID, Int64 nUserID);
        List<FNBatch> Gets(string sSQL, Int64 nUserID);
        FNBatch IUD(FNBatch oFUProcess, int nDBOperation, Int64 nUserID);
        FNBatch TransferFNBatchCard(FNBatch oFNBatch, long nUserID);
    }
    #endregion
}
