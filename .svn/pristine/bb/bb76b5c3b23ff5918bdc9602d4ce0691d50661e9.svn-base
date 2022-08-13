using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using ESimSol.BusinessObjects;
using System.Drawing;
using System.Runtime.Serialization;
using System.ServiceModel;
using ICS.Core;
using ICS.Core.Framework;
using ICS.Core.Utility;


namespace ESimSol.BusinessObjects
{
    #region DUPScheduleDetail
    
    public class DUPScheduleDetail : BusinessObject
    {
        #region  Constructor
        public DUPScheduleDetail()
        {
            DUPScheduleDetailID = 0;
            DUPScheduleID = 0;
            Qty = 0;
            Remarks = "";
            DBUserID = 0;
            ErrorMessage = "";
            OrderNo = "";
            ProductName = "";
            BuyerRef = "";
            BuyerName="";
            ColorName="";
            Qty_PSD = 0;
            PSBatchNo = "";
            RSState = EnumRSState.Initialized;
            UsesWeight = "";
            DODID = 0;
            PTUID = 0;
            DyeingOrderID = 0;
            LabDipDetailID = 0;
            StartTime = DateTime.Now;
            EndTime = DateTime.Now;
            ScheduleNo = "";
            OrderQty = 0;
            Qty_RS = 0;
            DeliveryDate = DateTime.MinValue;
            MUnit = "";
            IsRequistion = true;
            ApproveLotNo = "";
            HankorConeST = "";
            ContractorID = 0;
            WorkingUnitID_Lot = 0;
            RouteSheetID = 0;
            SLNo = 0;
            SLMax = 0;
            MachineID = 0;
            OrderLast = "";
            ColorNo = "";
            PantonNo = "";
        }
        #endregion

        #region Properties
        public string ScheduleNo { get; set; }
        public string BuyerRef { get; set; }
        public int DUPScheduleDetailID { get; set; }
        public int DUPScheduleID { get; set; }
        public DateTime DeliveryDate { get; set; }
        public int DODID { get; set; }
        public int PTUID { get; set; }
        public double Qty { get; set; }
        public string PSBatchNo { get; set; }
        public string Remarks { get; set; }
        public int DBUserID { get; set; }
        public int RouteSheetDOID { get; set; }
        public double BagCount { get; set; }
        public double QtyPerBag { get; set; }
        public short HankorCone { get; set; }
        public bool IsRequistion { get; set; }
        public int WorkingUnitID_Lot { get; set; }
        public string OrderLast { get; set; }
        public int MachineID { get; set; }
        public string DyeLoadNote { get; set; }
        public string DyeUnloadNote { get; set; }
        public string ErrorMessage { get; set; }

        #region derived Properties
        public int RouteSheetID { get; set; }
        public int DyeingOrderID { get; set; }
        public string LocationName { get; set; }
        public string MachineName { get; set; }
        public string MachineNo { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string OrderTypest { get; set; }
        public int OrderType { get; set; }
        public int ProductID { get; set; }
        public int LabDipDetailID { get; set; }
        public int SLNo { get; set; }
        public int SLMax { get; set; }
        public int ContractorID { get; set; }
        public string ContractorName { get; set; }
        public string ColorNo { get; set; }
        public string OrderNo { get; set; }
        public string ProductName { get; set; }
        public string BuyerName { get; set; }
        public string ColorName { get; set; }
        public string PantonNo { get; set; }
        public double Qty_PSD { get; set; }
        public double OrderQty { get; set; }
        public double Qty_RS { get; set; }
        public double Qty_Pro { get; set; }
   
        public string RouteSheetNo { get; set; }
        public string MUnit { get; set; }
        //public string RedyingForRSNo { get; set; }
     
        public string ApproveLotNo { get; set; }
        public bool IsInHouse { get; set; }
        public string HankorConeST { get; set; }
        public string Params { get; set; }
        public string StartTimeInST
        {
            get
            {
                return StartTime.ToString("dd MMM yyyy HH:mm");
            }
        }

        public string EndTimeInST
        {
            get
            {
                return EndTime.ToString("dd MMM yyyy HH:mm");
            }
        }

        public string EndTimeTS
        {
            get
            {
                return this.EndTime.ToString("HH:mm");

            }
        }

        public string StartDateSt
        {
            get
            {
                return StartTime.ToString("dd MMM yyyy");
            }
        }
        public string DeliveryDateSt
        {
            get
            {
                if (this.DeliveryDate==DateTime.MinValue) return "";
                else return DeliveryDate.ToString("dd MMM yyyy");
            }
        }
        public string EndDateSt
        {
            get
            {
                return EndTime.ToString("dd MMM yyyy");
            }
        }
    
        public string BatchCardNo
        {
            get
            {
                if (this.RouteSheetNo != "") { return "B# " + this.RouteSheetNo; }
                else { return ""; }
            }
        }

        
        public EnumRSState RSState { get; set; }

        public string RSStateSt
        {
            get
            {
                return this.RSState.ToString();
            }
        }
        private double _nYetToRS = 0;
        public double YetToRS
        {
            get
            {
                _nYetToRS = Math.Round((this.OrderQty - this.Qty_Pro), 2);
                return _nYetToRS;
            }
        }
        #region Production Report
        
      
        
        public string UsesWeight { get; set; }
      
        #endregion

      
        
        #endregion
        #region Functions
        public static List<DUPScheduleDetail> Gets(int nId, long nUserID)
        {
            return DUPScheduleDetail.Service.Gets(nId, nUserID);
        }
        public static List<DUPScheduleDetail> Gets(string sPSIDs,long nUserID)
        {
            return DUPScheduleDetail.Service.Gets(sPSIDs, nUserID);
        }
        public static List<DUPScheduleDetail> GetsSqL(string sSQL, long nUserID)
        {
            return DUPScheduleDetail.Service.GetsSqL(sSQL, nUserID);
        }
        public string Delete(long nUserID)
        {
            return DUPScheduleDetail.Service.Delete(this, nUserID);
        }
        public static List<DUPScheduleDetail> Swap(List<DUPSchedule> oDUPSchedule, long nUserID)
        {
            return DUPScheduleDetail.Service.Swap(oDUPSchedule, nUserID);
        }
        public List<DUPScheduleDetail> Update_Requisition(DUPScheduleDetail oDUPScheduleDetail, int nUserID)
        {
            return DUPScheduleDetail.Service.Update_Requisition(oDUPScheduleDetail, nUserID);
        }
        public static List<DUPScheduleDetail> Gets_RS(string sSQL, long nUserID)
        {
            return DUPScheduleDetail.Service.Gets_RS(sSQL, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IDUPScheduleDetailService Service
        {
            get { return (IDUPScheduleDetailService)Services.Factory.CreateService(typeof(IDUPScheduleDetailService)); }
        }
        #endregion

    }
        #endregion

        #region IDUPScheduleDetail interface
    public interface IDUPScheduleDetailService
    {
        List<DUPScheduleDetail> Gets(int id, Int64 nUserID);
        List<DUPScheduleDetail> Gets(string sPSIDs,Int64 nUserID);
        List<DUPScheduleDetail> GetsSqL(string sSQL, Int64 nUserID);
        string Delete(DUPScheduleDetail oDUPScheduleDetail, long nUserID);
        List<DUPScheduleDetail> Swap(List<DUPSchedule> oDUPSchedules, long nUserID);
        List<DUPScheduleDetail> Update_Requisition(DUPScheduleDetail oDUPScheduleDetail, long nUserID);
        List<DUPScheduleDetail> Gets_RS(string sSQL, Int64 nUserID);
    }
    #endregion

    #endregion
}
