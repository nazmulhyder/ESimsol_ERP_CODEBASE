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
    #region FabricDeliveryChallan

    public class FabricDeliveryChallan : BusinessObject
    {
        #region  Constructor
        public FabricDeliveryChallan()
        {
            FDCID=0;
            ChallanNo = "";
            FDOID=0;
            DeliveryPoint="";
            VehicleNo="";
            DriverName="";
            Note="";
            ApproveBy=0;
            ApproveDate=DateTime.MinValue;
            DisburseBy=0;
            DisburseDate=DateTime.Now;
            IssueDate = DateTime.Now;
            FDODate = DateTime.Now;
            WorkingUnitID = 0;
            ErrorMessage = "";
            FDCDetails = new List<FabricDeliveryChallanDetail>();
            Params = "";
            FEOQty = 0;
            Qty = 0;
            WorkingUnitName = "";
            PreparedByName = "";
            DeliveryMan = "";
            GatePassNo = "";
            CPDeliveryMan = "";
            IsSample = false;
            IsBill = false;
            FDOType = EnumDOType.None;
        }
        #endregion

        #region Properties
        public int FDCID { get; set; }
        public string ChallanNo { get; set; }
        public int FDOID { get; set; }
        public string DeliveryPoint { get; set; }
        public string VehicleNo { get; set; }
        public string DriverName { get; set; }
        public string Note { get; set; }
        public int ApproveBy { get; set; }
        public DateTime ApproveDate { get; set; }
        public int DisburseBy { get; set; }
        public DateTime DisburseDate { get; set; }
        public DateTime IssueDate { get; set; }
        public int WorkingUnitID { get; set; }
        public int VehicleTypeID { get; set; }
        public string DriverMobile { get; set; }
        public string VehicleTypeName { get; set; }
        public string DeliveryToAddress { get; set; }
        public bool IsSample { get; set; }
        public string Params { get; set; }
        public string CPDeliveryMan { get; set; }
        public EnumDOType FDOType { get; set; }
        public string ErrorMessage { get; set; }
        #endregion

        #region Derive Properties
        public bool IsBill { get; set; }
        public int ContractorID { get; set; }
        public string PreparedByName { get; set; }
        public string ApproveByName { get; set; }
        public string DisburseByName { get; set; }
        public string BuyerCPName { get; set; }
        public string MKTPerson { get; set; }
        public string FabricDONo { get; set; }
        public string DeliveryToName { get; set; }
        public string BuyerName { get; set; }
        public string WorkingUnitName { get; set; }
        public string FEONo { get; set; }
        public bool IsInHouse { get; set; }
        public double FEOQty { get; set; }
        public double Qty { get; set; }
        public DateTime FDODate { get; set; }
        public string DeliveryMan { get; set; }
        public string GatePassNo { get; set; }
        public List<FabricDeliveryChallanDetail> FDCDetails { get; set; }
        public List<FabricDeliveryChallanDetail> FDCDetails_Adj { get; set; }
        //FEOQty
        public string OrderNo { get { if (this.FEONo!="") { if (this.IsInHouse)return "" + this.FEONo; else return "" + this.FEONo; } else return ""; } }
        public string ApproveDateInStr { get { return (this.ApproveDate != DateTime.MinValue) ? this.ApproveDate.ToString("dd MMM yyyy") : ""; } }
        public string DisburseDateInStr { get { return (this.DisburseBy > 0) ? this.DisburseDate.ToString("dd MMM yyyy") : ""; } }
        public string IssueDateSt { get { return this.IssueDate.ToString("dd MMM yyyy"); } }
        public string FDODateSt { get { return this.FDODate.ToString("dd MMM yyyy"); } }
        public string FDOTypeSt { get { return EnumObject.jGet(this.FDOType); } }
      
        #endregion

        #region Functions
        public static FabricDeliveryChallan Get(int nFDCID, long nUserID)
        {
            return FabricDeliveryChallan.Service.Get(nFDCID, nUserID);
        }
        public static List<FabricDeliveryChallan> Gets(string sSQL, long nUserID)
        {
            return FabricDeliveryChallan.Service.Gets(sSQL, nUserID);
        }
        public FabricDeliveryChallan IUD(int nDBOperation, long nUserID)
        {
            return FabricDeliveryChallan.Service.IUD(this, nDBOperation, nUserID);
        }
        public FabricDeliveryChallan Approve(long nUserID)
        {
            return FabricDeliveryChallan.Service.Approve(this, nUserID);
        }
        public FabricDeliveryChallan UndoApprove(long nUserID)
        {
            return FabricDeliveryChallan.Service.UndoApprove(this, nUserID);
        }
        public static FabricDeliveryChallan FDCDisburse(FabricDeliveryChallan oFDC, long nUserID)
        {
            return FabricDeliveryChallan.Service.FDCDisburse(oFDC, nUserID);
        }
        public string Delete(int nUserID)
        {
            return FabricDeliveryChallan.Service.Delete(this, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IFabricDeliveryChallanService Service
        {
            get { return (IFabricDeliveryChallanService)Services.Factory.CreateService(typeof(IFabricDeliveryChallanService)); }
        }
        #endregion
    }
    #endregion


    #region IFabricDeliveryChallan interface
    public interface IFabricDeliveryChallanService
    {
        FabricDeliveryChallan Get(int nFDCID, long nUserID);
        List<FabricDeliveryChallan> Gets(string sSQL, long nUserID);
        FabricDeliveryChallan IUD(FabricDeliveryChallan oFabricDeliveryChallan, int nDBOperation, long nUserID);
        FabricDeliveryChallan Approve(FabricDeliveryChallan oFabricDeliveryChallan, long nUserID);
        FabricDeliveryChallan UndoApprove(FabricDeliveryChallan oFabricDeliveryChallan, long nUserID);
        FabricDeliveryChallan FDCDisburse(FabricDeliveryChallan oFDC, long nUserID);
        string Delete(FabricDeliveryChallan oFabricDeliveryChallan, Int64 nUserID);
    }
    #endregion
}