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

    public class FNBatchQC : BusinessObject
    {
        public FNBatchQC()
        {
            FNBatchQCID = 0;
            FNBatchID = 0;
            Qty = 0;
            StartTime = DateTime.Today;
            EndTime = DateTime.Today;
            QCInchargeID = 0;
            Color="";
            Composition = "";
		    FabricWeaveName="";
		    StyleNo="";
		    ExeQty=0;
            ErrorMessage = "";
            Params = "";
            ActualWidth = 0;
            FNExOID = 0;
            ConstructionPI = "";
            QCInchargeName = "";
            BuyerID = 0;
            FNBatchQCDetails = new List<FNBatchQCDetail>();
        }

        #region Properties

        public int FNBatchQCID { get; set; }
        public int FNBatchID { get; set; }
        public double Qty { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int QCInchargeID { get; set; }
        public double ActualWidth { get; set; }
        public int FNExOID { get; set; }
        public List<FNBatchQCDetail> FNBatchQCDetails { get; set; }
        public string ErrorMessage { get; set; }
        public string Params { get; set; }

        #endregion

        #region Derived Property
        public int BUID { get; set; }
        public int BuyerID { get; set; }
        public string FNBatchNo { get; set; }
        public string QCInchargeName { get; set; }
        public string FNExONo { get; set; }
        public double BatchQty { get; set; }
        public double OutQty { get; set; }
        public string CountName { get; set; }
        public string BuyerName { get; set; }
        public string Construction { get; set; }
        public string ConstructionPI { get; set; }
        public string FinishWidth { get; set; }
        public string FinishTypeName { get; set; }
        public EnumFNBatchStatus FNBatchStatus { get; set; }
        public int CountYetNotRecv { get; set; }
        public string MUnit { get; set; }

        public string Color { get; set; }
        public string Composition { get; set; }
        public string FabricWeaveName { get; set; }
        public string StyleNo { get; set; }
        public string BuyerRef { get; set; }
        public double ExeQty { get; set; }
        

        public string StartTimeStr { get { return this.StartTime.ToString("dd MMM yyyy"); } }
        public string EndTimeStr { get { return this.EndTime.ToString("dd MMM yyyy"); } }
        public string FNBatchStatusStr { get { return this.FNBatchStatus.ToString(); } }

        public double QtyInMtr { get { return Global.GetMeter(this.Qty, 2); } }
        public double OutQtyInMtr { get { return Global.GetMeter(this.OutQty, 2); } }
        public double BatchQtyInMtr { get { return Global.GetMeter(this.BatchQty, 2); } }

        
        #endregion

        #region Functions

        public static FNBatchQC Get(int nFNBatchQCID, long nUserID)
        {
            return FNBatchQC.Service.Get(nFNBatchQCID, nUserID);
        }
        public static List<FNBatchQC> Gets(string sSQL, long nUserID)
        {
            return FNBatchQC.Service.Gets(sSQL, nUserID);
        }
        public FNBatchQC IUD(int nDBOperation, long nUserID)
        {
            return FNBatchQC.Service.IUD(this, nDBOperation, nUserID);
        }

        public FNBatchQC FNBatchQCDone(long nUserID)
        {
            return FNBatchQC.Service.FNBatchQCDone(this, nUserID);
        }

        public FNBatchQC SaveProcess(int nFNBatchID, string sRollNo, int nRollCountStart, int nQCLotItem, long nUserID)
        {
            return FNBatchQC.Service.SaveProcess(nFNBatchID, sRollNo, nRollCountStart, nQCLotItem, nUserID);
        }

        #endregion

        #region ServiceFactory
        internal static IFNBatchQCService Service
        {
            get { return (IFNBatchQCService)Services.Factory.CreateService(typeof(IFNBatchQCService)); }
        }

        #endregion
    }
    #endregion

    #region IFUProcess interface

    public interface IFNBatchQCService
    {

        FNBatchQC Get(int nFNBatchQCID, Int64 nUserID);
        List<FNBatchQC> Gets(string sSQL, Int64 nUserID);
        FNBatchQC IUD(FNBatchQC oFUProcess, int nDBOperation, Int64 nUserID);
        FNBatchQC FNBatchQCDone(FNBatchQC oFUProcess, Int64 nUserID);
        FNBatchQC SaveProcess(int nFNBatchID , string sRollNo, int nRollCountStart , int nQCLotItem,  Int64 nUserID);
    }
    #endregion
}
