using System;
using ICS.Core.Framework;
using ICS.Core.Utility;
using ICS.Core;
using ESimSol.BusinessObjects;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.ServiceModel;
using ESimSol.BusinessObjects.ReportingObject;

namespace ESimSol.BusinessObjects
{
    #region FSCFollowUp
    public class FSCFollowUp : BusinessObject
    {
        public FSCFollowUp()
        {
            SCNoFull = "";
            SCDate = DateTime.Now;
            ExeNo = "";
            ExeDate = DateTime.Now;
            Qty_PO = 0;
            Qty_Dispo = 0;
            BuyerName = "";
            ContractorName = "";
            MKTPersonName = "";
            ProcessType = 0;
            ProcessTypeName = "";
            Construction = "";
            FinishTypeName = "";
            FinishDesign = "";
            FabricWeave = 0;
            FabricWeaveName = "";
            DateDUPSchedule = DateTime.Now;
            QtyReqDyed = 0;
            QtyDUPSchedule = 0;
            DateLotAssign = DateTime.Now;
            QtyLotAssign = 0;
            DateIssueDUReq = DateTime.Now;
            QtyDUReq = 0;
            DateReceiveDUReq = DateTime.Now;
            QtySoftWinding = 0;
            DateRSInFloor = DateTime.Now;
            DateBatchload = DateTime.Now;
            QtyDyeMachine = 0;
            QtyHydro = 0;
            QtyDryer = 0;
            QtyApproval = 0;
            DateHWRecd = DateTime.Now;
            QtyFreshDye = 0;
            DateBeamTr = DateTime.Now;
            QtyBeamTr = 0;
            DateWarpingStart = DateTime.Now;
            QtyWarping = 0;
            DateWarpingEnd = DateTime.Now;
            DateSizingStart = DateTime.Now;
            QtySizing = 0;
            DateSizingEnd = DateTime.Now;
            DateDrawingStart = DateTime.Now;
            QtyDrawing = 0;
            DateDrawingEnd = DateTime.Now;
            DateLoomStart = DateTime.Now;
            QtyLoom = 0;
            DateLoomEnd = DateTime.Now;
            QtyGreyIns = 0;
            DatePretreatment = DateTime.Now;
            QtyPretreatment = 0;
            DateFnDyeing = DateTime.Now;
            QtyFnDyeing = 0;
            DateFinishing = DateTime.Now;
            QtyFinishing = 0;
            DateFNInsRecd = DateTime.Now;
            QtyFNIns = 0;
            DateFNInsDC = DateTime.Now;
            QtyDO = 0;
            IsPrint = true;
            ContractorID = 0;
            BCPID = 0;
            MKTPersonID = 0;
            FabricID = 0;
            MUID = 0;
            ProductName = "";
            FabricNo = "";
            ColorInfo = "";
            FabricWidth = "";
            BuyerReference = "";
            StyleNo = "";
            PINo = "";
            LCNo = "";
            ProductID = 0;
            FinishType = 0;
            BuyerID = 0;
            FSCID = 0;
            FSCDetailID = 0;
            DyeingOrderID = 0;
            MUnit = "";
            FabricDesignID = 0;
            FabricDesign = "";
            QtyGreyRecd = 0;
            QtyFnBatch = 0;
            QtyFnInspRecd = 0;
            QtyDC = 0;
            QtyRC = 0;
            QtyStoreRecd = 0;
            StockInHand = 0;
            ErrorMessage = "";
            DateDryerUnLoad = DateTime.Now;
        }

        #region Property
        public string SCNoFull { get; set; }
        public DateTime SCDate { get; set; }
        public string ExeNo { get; set; }
        public DateTime ExeDate { get; set; }
        public double Qty_PO { get; set; }
        public double Qty_Dispo { get; set; }
        public string BuyerName { get; set; }
        public string ContractorName { get; set; }
        public string MKTPersonName { get; set; }
        public int ProcessType { get; set; }
        public string ProcessTypeName { get; set; }
        public string Construction { get; set; }
        public string FinishTypeName { get; set; }
        public string FinishDesign { get; set; }
        public int FabricWeave { get; set; }
        public string FabricWeaveName { get; set; }
        public DateTime DateDUPSchedule { get; set; }
        public double QtyReqDyed { get; set; }
        public double QtyDUPSchedule { get; set; }
        public DateTime DateLotAssign { get; set; }
        public double QtyLotAssign { get; set; }
        public DateTime DateIssueDUReq { get; set; }
        public double QtyDUReq { get; set; }
        public DateTime DateReceiveDUReq { get; set; }
        public double QtySoftWinding { get; set; }
        public DateTime DateRSInFloor { get; set; }
        public DateTime DateBatchload { get; set; }
        public double QtyDyeMachine { get; set; }
        public double QtyHydro { get; set; }
        public double QtyDryer { get; set; }
        public double QtyApproval { get; set; }
        public DateTime DateHWRecd { get; set; }
        public double QtyFreshDye { get; set; }
        public DateTime DateBeamTr { get; set; }
        public double QtyBeamTr { get; set; }
        public DateTime DateWarpingStart { get; set; }
        public double QtyWarping { get; set; }
        public DateTime DateWarpingEnd { get; set; }
        public DateTime DateSizingStart { get; set; }
        public double QtySizing { get; set; }
        public DateTime DateSizingEnd { get; set; }
        public DateTime DateDrawingStart { get; set; }
        public double QtyDrawing { get; set; }
        public DateTime DateDrawingEnd { get; set; }
        public DateTime DateLoomStart { get; set; }
        public double QtyLoom { get; set; }
        public DateTime DateLoomEnd { get; set; }
        public double QtyGreyIns { get; set; }
        public DateTime DatePretreatment { get; set; }
        public double QtyPretreatment { get; set; }
        public DateTime DateFnDyeing { get; set; }
        public double QtyFnDyeing { get; set; }
        public DateTime DateFinishing { get; set; }
        public double QtyFinishing { get; set; }
        public DateTime DateFNInsRecd { get; set; }
        public double QtyFNIns { get; set; }
        public DateTime DateFNInsDC { get; set; }
        public double QtyDO { get; set; }
        public bool IsPrint { get; set; }
        public int ContractorID { get; set; }
        public int BCPID { get; set; }
        public int MKTPersonID { get; set; }
        public int FabricID { get; set; }
        public int MUID { get; set; }
        public string ProductName { get; set; }
        public string FabricNo { get; set; }
        public string ColorInfo { get; set; }
        public string FabricWidth { get; set; }
        public string BuyerReference { get; set; }
        public string StyleNo { get; set; }
        public string PINo { get; set; }
        public string LCNo { get; set; }
        public int ProductID { get; set; }
        public int FinishType { get; set; }
        public int BuyerID { get; set; }
        public int FSCID { get; set; }
        public int FSCDetailID { get; set; }
        public int DyeingOrderID { get; set; }
        public string MUnit { get; set; }
        public int FabricDesignID { get; set; }
        public string FabricDesign { get; set; }
        public double QtyGreyRecd { get; set; }
        public double QtyFnBatch { get; set; }
        public double QtyFnInspRecd { get; set; }
        public double QtyDC { get; set; }
        public double QtyRC { get; set; }
        public double QtyStoreRecd { get; set; }
        public double StockInHand { get; set; }
        public string ErrorMessage { get; set; }
        public DateTime DateDryerUnLoad { get; set; }
        public DateTime DateBatchUnload { get; set; }
        public DateTime DateDCOut { get; set; }
        public DateTime DateHydroLoad { get; set; }
        public DateTime DateApproval { get; set; }
        #endregion

        #region Derived Property
        public string DateBatchUnloadInString
        {
            get
            {
                if (DateBatchUnload == DateTime.MinValue) return "";
                return DateBatchUnload.ToString("dd MMM yyyy");
            }
        }
        public string DateDCOutInString
        {
            get
            {
                if (DateDCOut == DateTime.MinValue) return "";
                return DateDCOut.ToString("dd MMM yyyy");
            }
        }
        public string DateHydroLoadInString
        {
            get
            {
                if (DateHydroLoad == DateTime.MinValue) return "";
                return DateHydroLoad.ToString("dd MMM yyyy");
            }
        }
        public string DateApprovalInString
        {
            get
            {
                if (DateApproval == DateTime.MinValue) return "";
                return DateApproval.ToString("dd MMM yyyy");
            }
        }

        public string DateDryerUnLoadInString
        {
            get
            {
                if (DateDryerUnLoad == DateTime.MinValue) return "";
                return DateDryerUnLoad.ToString("dd MMM yyyy");
            }
        }
        public string SCDateInString
        {
            get
            {
                if (SCDate == DateTime.MinValue) return "";
                return SCDate.ToString("dd MMM yyyy");
            }
        }
        public string ExeDateInString
        {
            get
            {
                if (ExeDate == DateTime.MinValue) return "";
                return ExeDate.ToString("dd MMM yyyy");
            }
        }
        public string DateDUPScheduleInString
        {
            get
            {
                if (DateDUPSchedule == DateTime.MinValue) return "";
                return DateDUPSchedule.ToString("dd MMM yyyy");
            }
        }
        public string DateLotAssignInString
        {
            get
            {
                if (DateLotAssign == DateTime.MinValue) return "";
                return DateLotAssign.ToString("dd MMM yyyy");
            }
        }
        public string DateIssueDUReqInString
        {
            get
            {
                if (DateIssueDUReq == DateTime.MinValue) return "";
                return DateIssueDUReq.ToString("dd MMM yyyy");
            }
        }
        public string DateReceiveDUReqInString
        {
            get
            {
                if (DateReceiveDUReq == DateTime.MinValue) return "";
                return DateReceiveDUReq.ToString("dd MMM yyyy");
            }
        }
        public string DateRSInFloorInString
        {
            get
            {
                if (DateRSInFloor == DateTime.MinValue) return "";
                return DateRSInFloor.ToString("dd MMM yyyy");
            }
        }
        public string DateBatchloadInString
        {
            get
            {
                if (DateBatchload == DateTime.MinValue) return "";
                return DateBatchload.ToString("dd MMM yyyy");
            }
        }
        public string DateHWRecdInString
        {
            get
            {
                if (DateHWRecd == DateTime.MinValue) return "";
                return DateHWRecd.ToString("dd MMM yyyy");
            }
        }
        public string DateBeamTrInString
        {
            get
            {
                if (DateBeamTr == DateTime.MinValue) return "";
                return DateBeamTr.ToString("dd MMM yyyy");
            }
        }
        public string DateWarpingStartInString
        {
            get
            {
                if (DateWarpingStart == DateTime.MinValue) return "";
                return DateWarpingStart.ToString("dd MMM yyyy");
            }
        }
        public string DateWarpingEndInString
        {
            get
            {
                if (DateWarpingEnd == DateTime.MinValue) return "";
                return DateWarpingEnd.ToString("dd MMM yyyy");
            }
        }
        public string DateSizingStartInString
        {
            get
            {
                if (DateSizingStart == DateTime.MinValue) return "";
                return DateSizingStart.ToString("dd MMM yyyy");
            }
        }
        public string DateSizingEndInString
        {
            get
            {
                if (DateSizingEnd == DateTime.MinValue) return "";
                return DateSizingEnd.ToString("dd MMM yyyy");
            }
        }
        public string DateDrawingStartInString
        {
            get
            {
                if (DateDrawingStart == DateTime.MinValue) return "";
                return DateDrawingStart.ToString("dd MMM yyyy");
            }
        }
        public string DateDrawingEndInString
        {
            get
            {
                if (DateDrawingEnd == DateTime.MinValue) return "";
                return DateDrawingEnd.ToString("dd MMM yyyy");
            }
        }
        public string DateLoomStartInString
        {
            get
            {
                if (DateLoomStart == DateTime.MinValue) return "";
                return DateLoomStart.ToString("dd MMM yyyy");
            }
        }
        public string DateLoomEndInString
        {
            get
            {
                if (DateLoomEnd == DateTime.MinValue) return "";
                return DateLoomEnd.ToString("dd MMM yyyy");
            }
        }
        public string DatePretreatmentInString
        {
            get
            {
                if (DatePretreatment == DateTime.MinValue) return "";
                return DatePretreatment.ToString("dd MMM yyyy");
            }
        }
        public string DateFnDyeingInString
        {
            get
            {
                if (DateFnDyeing == DateTime.MinValue) return "";
                return DateFnDyeing.ToString("dd MMM yyyy");
            }
        }
        public string DateFinishingInString
        {
            get
            {
                if (DateFinishing == DateTime.MinValue) return "";
                return DateFinishing.ToString("dd MMM yyyy");
            }
        }
        public string DateFNInsRecdInString
        {
            get
            {
                if (DateFNInsRecd == DateTime.MinValue) return "";
                return DateFNInsRecd.ToString("dd MMM yyyy");
            }
        }
        public string DateFNInsDCInString
        {
            get
            {
                if (DateFNInsDC == DateTime.MinValue) return "";
                return DateFNInsDC.ToString("dd MMM yyyy");
            }
        }
        #endregion

        #region Functions
        public static List<FSCFollowUp> Gets(long nUserID)
        {
            return FSCFollowUp.Service.Gets(nUserID);
        }
        public static List<FSCFollowUp> Gets(string sSQL, long nUserID)
        {
            return FSCFollowUp.Service.Gets(sSQL, nUserID);
        }
        public FSCFollowUp Get(int id, long nUserID)
        {
            return FSCFollowUp.Service.Get(id, nUserID);
        }
        
        #endregion

        #region ServiceFactory
        internal static IFSCFollowUpService Service
        {
            get { return (IFSCFollowUpService)Services.Factory.CreateService(typeof(IFSCFollowUpService)); }
        }
        #endregion
    }
    #endregion

    #region IFSCFollowUp interface
    public interface IFSCFollowUpService
    {
        FSCFollowUp Get(int id, Int64 nUserID);
        List<FSCFollowUp> Gets(Int64 nUserID);
        List<FSCFollowUp> Gets(string sSQL, Int64 nUserID);
        
    }
    #endregion
}







