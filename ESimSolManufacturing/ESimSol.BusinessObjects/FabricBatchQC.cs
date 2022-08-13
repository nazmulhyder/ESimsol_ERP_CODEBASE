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
    #region FabricBatchQC
    public class FabricBatchQC : BusinessObject
    {
        public FabricBatchQC()
        {
            FBQCID = 0;
            FBID = 0;
            TotalLength = 0;
            QCStartDateTime = new DateTime(1900,1,1);
            QCEndDateTime= new DateTime(1900,1,1);
            QCInCharge  = 0;
            BatchNo = "";
            FabricBatchQty = 0;
            FabricBatchStatus = EnumFabricBatchState.Initialize;
            BuyerName = "";
            Construction = "";
            IsInHouse = true;
            OrderType = EnumOrderType.None;
            FEONo = "";
            FabricType = "";// EnumFinishType.None;
            CW = "";
            PINo = "";
            InChargeName = "";
            ErrorMessage = "";
            FabricSalesContractDetails = new List<FabricSalesContractDetail>();
            FabricBatchQCDetails = new List<FabricBatchQCDetail>();
            FabricBatchQCFaults = new List<FabricBatchQCFault>();
            FEOID = 0;
            CountNotRecDetail = 0;
            GreyWidth = 0;
            FabricWeaveName = string.Empty;
            ProcessTypeName = string.Empty;
            FabricBatchQCs = new List<FabricBatchQC>();
            FabricBatchLoomID = 0;
            FEOSID = 0;
            DispoNo = "";
            InsQty = 0;
            FBGradeID = 0;
        }

        #region Properties
        public int FabricBatchLoomID { get; set; }
        public int FBQCID { get; set; }
        public int FBID { get; set; }
        public string BatchNo { get; set; }
        public int QCInCharge { get; set; }
        public string InChargeName { get; set; }
        public double FabricBatchQty { get; set; }
        public EnumFabricBatchState FabricBatchStatus { get; set; }
        public int StatusInInt { get; set; }
        public int FEOID { get; set; }
        public string FEONo { get; set; }
        public string BuyerName { get; set; }
        public double TotalLength { get; set; }
        public string FabricType { get; set; }
        public string Construction { get; set; }
        public string ErrorMessage { get; set; }
        public string Params { get; set; }
        public FabricSalesContractDetail FabricSalesContractDetail { get; set; }
        public List<FabricSalesContractDetail> FabricSalesContractDetails { get; set; }
        public List<FabricBatchQC> FabricBatchQCs { get; set; }
        public DateTime QCStartDateTime { get; set; }
        public DateTime QCEndDateTime { get; set; }
        public bool IsInHouse { get; set; }
        public EnumOrderType OrderType { get; set; }
        public string CW { get; set; } //
        public string PINo { get; set; }
        public int CountNotRecDetail { get; set; }
        public double GreyWidth { get; set; }
        public string ProcessTypeName { get; set; }

        //for searching
        public string DispoNo { get; set; }
        public int FBGradeID { get; set; }
        #endregion

        #region Derive Property
        public double LoomQty { get; set; }
        public double InsQty { get; set; }
        public int FMID { get; set; }
        public string FabricMachineName { get; set; }
        public int FEOSID { get; set; }
        public string FabricWeaveName { get; set; }
        public bool IsYarn { get; set; }
        public List<FabricBatchQCDetail> FabricBatchQCDetails { get; set; }
        public List<FabricBatchQCFault> FabricBatchQCFaults { get; set; }
        public double LoomQtyInMtr
        {
            get
            {
                return Global.GetMeter(this.LoomQty, 2);
            }
        }
        public double InsQtyInMtr
        {
            get
            {
                return Global.GetMeter(this.InsQty, 2);
            }
        }
        public double FabricBatchQtyInMtr
        {
            get
            {
                return Global.GetMeter(this.FabricBatchQty, 2);
            }
        }
        public string BatchNoWithFEONo
        {
            get
            {
                return this.BatchNo + " [" + this.OrderNo + "]";
            }
        }
        public string FabricTypeInString
        {
            get
            {
                return this.FabricType.ToString();
            }
        }
        public string OrderNo
        {
            get
            {
                    return  this.FEONo;
            }
        }
     
        public string StatusSt
        {
            get
            {
                return FabricBatchStateObj.GetEnumFabricBatchStateObjs(this.FabricBatchStatus);
               // return "Please cheack BO";
            }
        }
        public string QtySt
        {
            get
            {
                return Global.MillionFormat(this.FabricBatchQty);
            }
        }
        public string QCStartDateTimeInString
        {
            get
            {
                return this.QCStartDateTime.ToString("dd MMM yyyy HH:mm");
            }
        }

        public string QCEndDateTimeInString
        {
            get
            {
                return this.QCEndDateTime.ToString("dd MMM yyyy HH:mm");
            }
        }
        #endregion

        #region Functions
        public static List<FabricBatchQC> Gets(long nUserID)
        {
            return FabricBatchQC.Service.Gets(nUserID);
        }
        public static List<FabricBatchQC> Gets(string sSQL, long nUserID)
        {
            return FabricBatchQC.Service.Gets(sSQL, nUserID);
        }
        public FabricBatchQC Get(int nId, long nUserID)
        {
            return FabricBatchQC.Service.Get(nId, nUserID);
        }
        public FabricBatchQC GetByBatch(int nId, long nUserID)//id = FBID, state = FBSTATE
        {
            return FabricBatchQC.Service.GetByBatch(nId,  nUserID);
        }
        public FabricBatchQC GetByProduction(int nFabricBatchLoomID, long nUserID)//id = FabricBatchLoomID
        {
            return FabricBatchQC.Service.GetByProduction(nFabricBatchLoomID, nUserID);
        }
        public FabricBatchQC QCDone(long nUserID)
        {
            return FabricBatchQC.Service.QCDone(this, nUserID);
        }

        public string Delete(int nId, long nUserID)
        {
            return FabricBatchQC.Service.Delete(nId, nUserID);
        }

        #endregion

        #region ServiceFactory
        internal static IFabricBatchQCService Service
        {
            get { return (IFabricBatchQCService)Services.Factory.CreateService(typeof(IFabricBatchQCService)); }
        }
        #endregion
    }
    #endregion

    #region IFabricBatchQC interface
    public interface IFabricBatchQCService
    {
        List<FabricBatchQC> Gets(long nUserID);
        List<FabricBatchQC> Gets(string sSQL, long nUserID);
        FabricBatchQC Get(int id, long nUserID);
        FabricBatchQC GetByBatch(int id,  long nUserID);
        FabricBatchQC GetByProduction(int nFabricBatchLoomID, long nUserID);

        FabricBatchQC QCDone(FabricBatchQC oFabricBatchQC, long nUserID);
        string Delete(int id, long nUserID);
    }
    #endregion
}
