using System;
using System.IO;
using ICS.Core;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using ICS.Core.Framework;
using ICS.Core.Utility;
using System.Data;

namespace ESimSol.BusinessObjects
{
    #region GUProductionOrder

    public class GUProductionOrder : BusinessObject
    {
        public GUProductionOrder()
        {
            GUProductionOrderID = 0;
            GUProductionOrderNo = "";
            OrderRecapID = 0;
            BuyerID = 0;
            ProductID = 0;
            FabricID = 0;
            GG = "";
            Count = "";
            ProductionUnitID = 0;
            MerchandiserID = 0;
            OrderStatus = EnumGUProductionOrderStatus.None;
            OrderDate = DateTime.Today;
            ShipmentDate = DateTime.Today;
            Note = "";
            OrderRecapNo = "";
            TechnicalSheetID = 0;
            ODSNo = "";
            ProductionFactoryName = "";
            ProductionFactoryPhone = "";
            BuyerName = "";
            BuyerPhone = "";
            GarmentsProductName = "";
            GarmentsProductCode = "";
            FabricProductCode = "";
            FabricProductName = "";
            FactoryContactPersonName = "";
            FactoryContactPersonPhone = "";
            MerchandiserName = "";
            MerchandiserContactNo = "";
            StyleNo = "";
            ApprovedBy = "";
            TotalQty = 0;
            ODSDetailQty = 0;
            GUProductionOrderHistoryID = 0;
            ActionType = EnumOPOActionType.None;
            ErrorMessage = "";
            GUProductionOrderDetails = new List<GUProductionOrderDetail>();
            StartDate = DateTime.Now;
            ProductionOperationNote = "";
            OperationDate = DateTime.Now;
            ExecutionFactoryID = 0;
            OperationType = 0;
            UnitName = "";
            RecapQty = 0;
            YetToPoductionQty = 0;
            ToleranceInPercent = 0;
            WindingStatus = EnumWindingStatus.Initialize;
            WindingStatusInInt = 0;
            KnittingPattern = 0;
            KnittingPatternName = "";
            StyleDescription = "";
            BUID = 0;
            FactoryShipmentDate = DateTime.Now;
            ShipmentScheduleID = 0;
            InputDate = DateTime.Now;
            SSQty = 0;
            FBUID = 0;
            UnitID = 0;
            ProductionUnitName = "";
            CutOffDate = DateTime.Now;
            CutOffType = EnumCutOffType.None;
            GUProductionProcedures = new List<GUProductionProcedure>();
            ProductionSteps = new List<ProductionStep>();
            PlanAnalysises = new List<PlanAnalysis>();
            OrderRecap = new OrderRecap();
            TechnicalSheetSizes = new List<TechnicalSheetSize>();
            ColorSizeRatios = new List<ColorSizeRatio>();
            PLineConfigures = new List<PLineConfigure>();
        }

        #region Properties

        public int GUProductionOrderID { get; set; }

        public string GUProductionOrderNo { get; set; }

        public int OrderRecapID { get; set; }

        public int BuyerID { get; set; }

        public int ProductID { get; set; }

        public int FabricID { get; set; }

        public int GUProductionOrderHistoryID { get; set; }
        public int FBUID { get; set; }
        public string GG { get; set; }

        public string Count { get; set; }

        public int ProductionUnitID { get; set; }

        public int MerchandiserID { get; set; }

        public double ToleranceInPercent { get; set; }

        public EnumWindingStatus WindingStatus { get; set; }

        public int WindingStatusInInt { get; set; }

        public int KnittingPattern { get; set; }

        public string StyleDescription { get; set; }
        public int BUID { get; set; }
        public EnumGUProductionOrderStatus OrderStatus { get; set; }

        public EnumOPOActionType ActionType { get; set; }

        public double YetToPoductionQty { get; set; }


        public DateTime OrderDate { get; set; }

        public DateTime ShipmentDate { get; set; }

        public string Note { get; set; }

        public string OrderRecapNo { get; set; }

        public int TechnicalSheetID { get; set; }

        public string ODSNo { get; set; }

        public string ProductionFactoryName { get; set; }

        public string ProductionFactoryPhone { get; set; }

        public string BuyerName { get; set; }

        public string BuyerPhone { get; set; }

        public string GarmentsProductName { get; set; }

        public string GarmentsProductCode { get; set; }

        public string FabricProductCode { get; set; }

        public string FabricProductName { get; set; }

        public string FactoryContactPersonName { get; set; }

        public string FactoryContactPersonPhone { get; set; }

        public string MerchandiserName { get; set; }

        public string MerchandiserContactNo { get; set; }

        public string StyleNo { get; set; }

        public string ApprovedBy { get; set; }

        public double TotalQty { get; set; }

        public double ODSDetailQty { get; set; }

        public double RecapQty { get; set; }
        public string KnittingPatternName { get; set; }
        public DateTime FactoryShipmentDate { get; set; }
        public int ShipmentScheduleID { get; set; }
        public DateTime InputDate { get; set; }
        public double SSQty { get; set; }
        public DateTime CutOffDate { get; set; }
        public EnumCutOffType CutOffType { get; set; }
        public string ProductionUnitName { get; set; }
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property
        public OrderRecap OrderRecap { get; set; }
        public TechnicalSheetImage TechnicalSheetImage { get; set; }
        public List<GUProductionOrder> GUProductionOrders = new List<GUProductionOrder>();
        public List<PlanAnalysis> PlanAnalysises = new List<PlanAnalysis>();
        public List<TechnicalSheetSize> TechnicalSheetSizes { get; set; }
        public List<ColorSizeRatio> ColorSizeRatios { get; set; }
        public List<PLineConfigure> PLineConfigures { get; set; }
        public int nOrderStatus { get; set; }
        public string ActionTypeExtra { get; set; }
        public string PinCode { get; set; }
        public string RecapWithStyleNo
        {
            get
            {
                if (!string.IsNullOrEmpty(this.OrderRecapNo))
                {
                    return this.OrderRecapNo + "(" + this.StyleNo + ")";
                }
                else
                {
                    return "";
                }

            }
        }
        public string CutOffTypeSt
        {
            get
            {
                return EnumObject.jGet((this.CutOffType));
            }
        }
        public string CutOffDateSt
        {
            get
            {
                return this.CutOffDate.ToString("dd MMM yyyy");
            }
        }
        public string FactoryShipmentDateSt
        {
            get
            {
                return this.FactoryShipmentDate.ToString("dd MMM yyyy");
            }
        }
        public string InputDateSt
        {
            get
            {
                return this.InputDate.ToString("dd MMM yyyy");
            }
        }
        public string OrderStatusInString
        {
            get
            {
                return OrderStatus.ToString();
            }
        }
        public string OrderDateInString
        {
            get
            {
                return OrderDate.ToString("dd MMM yyyy");
            }
        }
        public string OperationDateInString
        {
            get
            {
                return OperationDate.ToString("dd MMM yyyyy");
            }
        }
        public string ShipmentDateInString
        {
            get
            {
                return ShipmentDate.ToString("dd MMM yyyy");
            }
        }
        public string TotalQtySt
        {
            get
            {
                return Global.MillionFormatActualDigit(this.TotalQty);
            }
        }
        public string ProductionOrderNoWithIDInString
        {
            get
            {
                //REfID ~RefNo~RefType //don't delete use in Merchanidsing  deshboard
                return this.GUProductionOrderID + "~" + this.GUProductionOrderNo + "~5";
            }
        }
        public List<GUProductionOrder> PODList { get; set; }

        public List<GUProductionTracingUnit> GUProductionTracingUnits { get; set; }
        public List<PTUTransection> PTUTransections { get; set; }

        public Company Company { get; set; }

        public List<GUProductionOrderDetail> GUProductionOrderDetails { get; set; }

        public List<Employee> EmployeeList { get; set; }

        public string UnitName { get; set; }

        public int UnitID { get; set; }

        public DateTime OperationDate { get; set; }

        public DataTable ProductionSummeryColumnTable { get; set; }

        public DataTable ProductionSummeryValueTable { get; set; }

        public List<GUProductionProcedure> GUProductionProcedures { get; set; }
        public List<ProductionStep> ProductionSteps { get; set; }
        public DateTime StartDate { get; set; }
        public string ProductionOperationNote { get; set; }
        public int ExecutionFactoryID { get; set; }
        public int OperationType { get; set; }

        #endregion

        #region Functions

        public static List<GUProductionOrder> Gets(long nUserID)
        {
            return GUProductionOrder.Service.Gets(nUserID);
        }

        public static List<GUProductionOrder> Gets_bySalorderID(int nOrderRecapID, long nUserID)
        {
            return GUProductionOrder.Service.Gets_bySalorderID(nOrderRecapID, nUserID);
        }

        public static List<GUProductionOrder> Gets_byPOIDs(string sIDs, long nUserID)
        {
            return GUProductionOrder.Service.Gets_byPOIDs(sIDs, nUserID);
        }
        public static List<GUProductionOrder> Gets(string sSQL, long nUserID)
        {
            return GUProductionOrder.Service.Gets(sSQL, nUserID);
        }
        public GUProductionOrder UpdateToleranceWithStatus(long nUserID)
        {
            return GUProductionOrder.Service.UpdateToleranceWithStatus(this, nUserID);
        }
        public GUProductionOrder Get(int id, long nUserID)
        {
            return GUProductionOrder.Service.Get(id, nUserID);
        }
        public GUProductionOrder ProductionProgresReport(string sRecapIDs, long nUserID)
        {
            return GUProductionOrder.Service.ProductionProgresReport(sRecapIDs, nUserID);
        }

        public GUProductionOrder GetbyGUProductionOrderNo(string GUProductionOrderno, long nUserID)
        {
            return GUProductionOrder.Service.GetbyGUProductionOrderNo(GUProductionOrderno, nUserID);
        }

        public GUProductionOrder Save(long nUserID)
        {

            return GUProductionOrder.Service.Save(this, nUserID);
        }

        public GUProductionOrder SendToProducton(int nGUProductionOrderID, long nUserID)
        {
            return GUProductionOrder.Service.SendToProducton(nGUProductionOrderID, nUserID);
        }

        public GUProductionOrder ChangeStatus(long nUserID)
        {

            return GUProductionOrder.Service.ChangeStatus(this, nUserID);
        }

        public string Delete(int id, long nUserID)
        {
            return GUProductionOrder.Service.Delete(id, nUserID);
        }

        #endregion

        #region ServiceFactory
        internal static IGUProductionOrderService Service
        {
            get { return (IGUProductionOrderService)Services.Factory.CreateService(typeof(IGUProductionOrderService)); }
        }


        #endregion
    }
    #endregion

    #region IGUProductionOrder interface

    public interface IGUProductionOrderService
    {

        GUProductionOrder Get(int id, Int64 nUserID);

        GUProductionOrder ProductionProgresReport(string sRecapIDs, Int64 nUserID);

        GUProductionOrder GetbyGUProductionOrderNo(string GUProductionOrderno, Int64 nUserId);

        List<GUProductionOrder> Gets(Int64 nUserID);

        List<GUProductionOrder> Gets(string sSQL, Int64 nUserID);

        GUProductionOrder ChangeStatus(GUProductionOrder oGUProductionOrder, Int64 nUserID);

        string Delete(int id, Int64 nUserID);

        GUProductionOrder Save(GUProductionOrder oGUProductionOrder, Int64 nUserID);

        GUProductionOrder UpdateToleranceWithStatus(GUProductionOrder oGUProductionOrder, Int64 nUserID);


        GUProductionOrder SendToProducton(int nGUProductionOrderID, Int64 nUserID);

        List<GUProductionOrder> Gets_bySalorderID(int nOrderRecapID, Int64 nUserID);

        List<GUProductionOrder> Gets_byPOIDs(string sIDs, Int64 nUserID);
    }
    #endregion
}
