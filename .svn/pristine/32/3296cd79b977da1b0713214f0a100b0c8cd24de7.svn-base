using System;
using System.IO;
using System.ComponentModel.DataAnnotations;
using ICS.Core;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using ICS.Core.Framework;
using ICS.Core.Utility;
using ESimSol.BusinessObjects.ReportingObject;



namespace ESimSol.BusinessObjects
{
    #region OrderRecap
    
    public class OrderRecap : BusinessObject
    {
        public OrderRecap()
        {
            OrderRecapID = 0;            
            SLNo = "";
            OrderRecapNo = "";
            OrderRecapStatus = EnumOrderRecapStatus.Initialized;
            BusinessSessionID = 0;
            SessionName = "";
            TechnicalSheetID = 0;
            ProductID = 0;
            CollectionNo = "";
            BuyerID = 0;
            BuyerContactPersonID = 0;
            MerchandiserID = 0;
            Incoterms = EnumIncoterms.None;
            TransportType = EnumTransportType.None;
            BoardDate = DateTime.Now;
            FactoryShipmentDate = DateTime.Now;
            AgentID = 0;
            FabricID = 0;
            Description = "";
            OrderDate = DateTime.Today;
            ShipmentDate = DateTime.Today;
            CurrencyID = 0;
            ProductionFactoryID = 0;
            FactoryName = "";
            ApproveBy = 0;
            ApproveDate = DateTime.Now;
            RecognizeImage = null;
            UnitPrice = 0;
            GG = "";
            Count = "";
            ONSQty = 0;
            SpecialFinish = "";
            Weight = "";
            CurrencySymbol = "";
            ProductCategoryID = 0;
            MerchandiserName = "";
            CurrencyName = "";
            BrandName = "";
            OrderType = EnumOrderType.None;
            OrderTypeInInt = 0;
            MeasurementUnitID = 0;
            UnitPrice = 0;
            TotalQuantity = 0;
            Amount = 0;
            CMValue = 0;
            IsActive = true;
            PIAttachQty = 0;
            Dept = 0;
            YetToShipmentQty = 0;
            AlreadyShipmentQty = 0;
            TSType = EnumTSType.Sweater;
            DeliveryTerm = "";
            PaymentTerm= "";
            RequiredSample= "";
            PackingInstruction = "";
            Assortment = "";
            ErrorMessage = "";
            ColorSizeRatios = new List<ColorSizeRatio>();
            ProductionFactoryName = "";
            FactoryAddress = "";
            YarnRequired = 0;
            LCValue = 0;
            DBDate = DateTime.Now;
            LocalYarnSupplierID = 0;
            ImportYarnSupplierID = 0;
            CommercialRemarks = "";
            LocalYarnSupplierName = "";
            ImportYarnSupplierName = "";
            OrderRecapLogID = 0;
            OrderRecapYarns = new List<OrderRecapYarn>();
            AssortmentType = EnumAssortmentType.Select_AssortmentType;
            AssortmentTypeInt = -1;
            IsShippedOut = false;
            FabricUnitType = EnumUniteType.None;
            UnitName = "";
            ClassName = "";
            SubClassName = "";
            Wash = "";
            FabCode = "";
            CartonQty = 0;
            QtyPerCarton = 0;
            KnittingPattern = 0;
            ORPackingPolicyCount = 0;
            BUID = 0;
            BUShortName = "";
            Param = "";
            YetToScheduleQty = 0;
            YetToInvoicety = 0;
            MachineQty = 0;
            ShipmentCTNQty = 0;
            IsORExistInPlan = false;
            bIsDependsOnShipment = false;
            PAMID = 0;
            PAMNo = "";
            ProductionPlanDate = DateTime.MinValue;
            TAPIssueDate = DateTime.MinValue;
            ImageComments = new List<ImageComment>();
            BarCodeNo = "";
            ORAssortments = new List<ORAssortment>();
            ORBarCodes = new List<ORBarCode>();
            RecapBillOfMaterials = new List<RecapBillOfMaterial>();
            BarCodeColorSizeRatios = new List<ColorSizeRatio>();
            Brands = new List<Brand>();
            StyleDepartments = new List<StyleDepartment>();
            ApprovalRequest = new ApprovalRequest();
            ReviseRequest = new ReviseRequest();
            ShipmentSchedules = new List<ShipmentSchedule>();
            CostSheets = new List<CostSheet>();
            TAP = new TAP();
            GUProductionOrders = new List<GUProductionOrder>();
            ProductionExecutionPlanDetails = new List<ProductionExecutionPlanDetail>();
            MasterLCDetail = new MasterLCDetail();
        }

        #region Properties
         
        public int OrderRecapID { get; set; }
        public int OrderRecapLogID { get; set; }
        public int BUID { get; set; }
         
        public string SLNo { get; set; }
        public string FabCode { get; set; }
         
        public string OrderRecapNo { get; set; }
        public string BrandName { get; set; }
        public double YetToInvoicety { get; set; }
        public EnumOrderRecapStatus OrderRecapStatus { get; set; }
        public double QtyPerCarton { get; set; }
        public int BusinessSessionID { get; set; }
        public EnumAssortmentType AssortmentType { get; set; }
        public int AssortmentTypeInt { get; set; }
        public bool IsShippedOut { get; set; } 
        public string SessionName { get; set; }
         
        public EnumOrderType OrderType { get; set; }
         
        public int OrderTypeInInt { get; set; }
         
        public int TechnicalSheetID { get; set; }
         
        public int ProductID { get; set; }
         
        public string CollectionNo { get; set; }
         
        public string CurrencyName { get; set; }
         
        public string CurrencySymbol { get; set; }
        public double YetToScheduleQty { get; set; }

        public DateTime FactoryShipmentDate { get; set; }
         
        public byte[] RecognizeImage { get; set; }
         
        public int BuyerID { get; set; }
         
        public int BuyerContactPersonID { get; set; }
         
        public int MerchandiserID { get; set; }
         
        public EnumIncoterms Incoterms { get; set; }
         
        public EnumTransportType TransportType { get; set; }
         
        public DateTime BoardDate { get; set; }
        public string Wash { get; set; }
        public string Description { get; set; }
        public string BUShortName { get; set; }
        public string ErrorMessage { get; set; }
         
        public int AgentID { get; set; }
         
        public int FabricID { get; set; }
         
        public DateTime OrderDate { get; set; }
         
        public DateTime ShipmentDate { get; set; }
        public bool IsORExistInPlan { get; set; }
        public int CurrencyID { get; set; }
         
        public int ProductionFactoryID { get; set; }
         
        public string FactoryName { get; set; }
         
        public int ApproveBy { get; set; }
         
        public DateTime ApproveDate { get; set; }
         
        public double CartonQty { get; set; }
        public double ShipmentCTNQty { get; set; }
         
        public double Amount { get; set; }
        public int ORPackingPolicyCount { get; set; }
        public double UnitPrice { get; set; }
         
        public double ONSQty { get; set; }
         
        public double TotalQuantity { get; set; }
         
        public string PinCode { get; set; }
         
        public string BarCodeNo { get; set; }
         
        public string GG { get; set; }
         
        public string Count { get; set; }
         
        public string SpecialFinish { get; set; }
         
        public string Weight { get; set; }
         
        public int KnittingPattern { get; set; }
        public int MachineQty { get; set; }
         
        public bool IsActive { get; set; }
         
        public double CMValue { get; set; }
         
        public double PIAttachQty { get; set; }
         
        public int Dept { get; set; }

        public EnumTSType TSType { get; set; }
         
        public string DeliveryTerm { get; set; }
         
        public string PaymentTerm { get; set; }
         
        public string RequiredSample { get; set; }
         
        public string PackingInstruction { get; set; }
         
        public string Assortment { get; set; }
         
        public DateTime DBDate { get; set; }
         
        public string FactoryAddress { get; set; }
         
        public int LocalYarnSupplierID { get; set; }
         
        public int ImportYarnSupplierID { get; set; }
         
        public string CommercialRemarks { get; set; }
        public int PAMID  { get; set; }
        public string PAMNo { get; set; }
        public string LocalYarnSupplierName { get; set; }
        public DateTime  ProductionPlanDate { get; set; }
        public DateTime  TAPIssueDate { get; set; }
        public EnumUniteType FabricUnitType { get; set; }
        public string ClassName { get; set; }
        public string SubClassName { get; set; }

        public string KnittingPatternName { get; set; }
        public string ImportYarnSupplierName { get; set; }

        public string IsORExistInPlanSt
        {
            get
            {
                if(this.IsORExistInPlan)
                {
                    return "Yes";
                }
                else
                {
                    return "No";
                }
            }
        }
        public string ProductionPlanDateInString
        {
            get
            {
              if(this.ProductionPlanDate==DateTime.MinValue)
              {
                  return "-";
              }
              else
              {
                  return this.ProductionPlanDate.ToString("dd MMM yyyy");
              }
                
            }
        }

        public string CMValueSt
        {
            get
            {
                return Global.MillionFormat(this.CMValue);
            }
        }
        public string TAPIssueDateInString
        {
            get
            {
                if (this.TAPIssueDate == DateTime.MinValue)
                {
                    return "-";
                }
                else
                {
                    return this.TAPIssueDate.ToString("dd MMM yyyy");
                }
            }
        }
        public string IsActiveInString
        {
            get
            {
                if (this.IsActive)
                {
                    return "Active";
                }
                else
                {
                    return "In-Active";
                }                
            }
        }
        public string IsShippedOutSt
        {
            get
            {
                if (this.IsShippedOut)
                {
                    return "Yes";
                }
                else
                {
                    return "No";
                }
            }
        }

        public string TransportTypeInString
        {
            get
            {
                return TransportType.ToString();
            }
        }
        public string IncotermsInString
        {
            get
            {
                return Incoterms.ToString();
            }
        }
        public string OrderDateInString
        {
            get
            {
                return this.OrderDate.ToString("dd MMM yyyy");
            }
        }
        public string ShipmentDateInString
        {
            get
            {
                return ShipmentDate.ToString("dd MMM yyyy");
            }
        }

        public string BoardDateInString
        {
            get
            {
                return BoardDate.ToString("dd MMM yyyy");
            }
        }
        public string OrderDateForViewInString
        {
            get
            {
                return this.OrderDate.ToString("dd MMM yyyy");
            }
        }
        public string ShipmentDateForViewInString
        {
            get
            {
                return this.ShipmentDate.ToString("dd MMM yyyy");
            }
        }

        public double YetToONS
        {
            get
            {
                double nONSQty = 0;
                if (this.ONSQty > 0)
                {
                    nONSQty = (this.TotalQuantity - this.ONSQty);
                    if (nONSQty < 0) { nONSQty = 0; }
                }
                else
                {
                    nONSQty = this.TotalQuantity;
                }
                return nONSQty;
            }
        }

        public double YetToAttachQty
        {
            get
            {
                if (this.TotalQuantity > this.PIAttachQty)
                {
                    return this.TotalQuantity - this.PIAttachQty;
                }
                else
                {
                    return 0;
                }
            }
        }
        public string NumberOfPackingPolicyCountInString
        {
            get
            {
                return this.OrderRecapID + "~" + this.ORPackingPolicyCount+"~'"+this.OrderRecapNo+"'";
            }
        }
        public string DeptName{get;set;}

        public string AmountInString
        {
            get
            {
                return Global.MillionFormat(this.Amount);
            }
        }
        public string UnitPriceInString
        {
            get
            {
                return Global.MillionFormat(this.UnitPrice);
            }
        }
        public string TotalQuantityInString
        {
            get
            {
                return Global.MillionFormatActualDigit(this.TotalQuantity);
            }
        }

        #endregion

        #region derived property
        public double YetToQcQty { get; set; }
        public double AlreadyQCQty { get; set; }
        public bool bIsDependsOnShipment { get; set; }
        public string Param { get; set; }
        public double YetToShipmentQty { get; set; }
        public double AlreadyShipmentQty { get; set; }
        public BusinessUnit BusinessUnit { get; set; }
        public TAP TAP { get; set; }
        public List<Brand> Brands { get; set; }
        public List<GUProductionOrder> GUProductionOrders { get; set; }
        public List<ProductionExecutionPlanDetail> ProductionExecutionPlanDetails { get; set; }
        public MasterLCDetail MasterLCDetail { get; set; }
             
        public EnumActionType ActionType { get; set; }
        public List<CostSheet> CostSheets { get; set; }
        public int ActionTypeInInt { get; set; }
        public int OrderRecapStatusInInt { get; set; }
        public ApprovalRequest ApprovalRequest { get; set; }
        public ReviseRequest ReviseRequest { get; set; }
        public List<ShipmentSchedule> ShipmentSchedules { get; set; }
        public List<ImageComment> ImageComments { get; set; }
        public Contractor Contractor { get; set; }
        public List<Employee> Employees { get; set; }
        public List<BarCodeComment> BarCodeComments { get; set; }
        public List<OrderRecap> OrderRecapList { get; set; }
        public List<MaterialType> MaterialTypes { get; set; }
        public List<Currency> CurrencyList { get; set; }
        public List<ColorSizeRatio> ColorSizeRatios { get; set; }
        public List<ColorSizeRatio> AssortmentColorSizeRatios { get; set; }
        public List<ColorSizeRatio> BarCodeColorSizeRatios { get; set; }         
        public List<RecapBillOfMaterial> RecapBillOfMaterials { get; set; }
        public List<StyleDepartment> StyleDepartments { get; set; }
        public List<ORAssortment> ORAssortments { get; set; }

        public List<ORBarCode> ORBarCodes { get; set; }
        public List<TechnicalSheetSize> TechnicalSheetSizes { get; set; }
        public List<MeasurementUnit> Units { get; set; }

        public List<RawMaterialSourcing> RawMaterialSourcings { get; set; }
        public List<OrderRecapDetail> OrderRecapDetails { get; set; }

        public List<BusinessSession> BusinessSessions { get; set; }
         
        public List<ContactPersonnel> ContactPersonnelList { get; set; }
        
         
        public OrderRecapDetail OrderRecapDetail { get; set; }
         
        public List<OrderRecapYarn> OrderRecapYarns { get; set; }
         
        public List<SizeCategory> SizeCategories { get; set; }
         
        public List<ColorCategory> ColorCategories { get; set; }
         
        public TechnicalSheet TechnicalSheet { get; set; }
         
        public TechnicalSheetImage TechnicalSheetFrontImage { get; set; }
         
        public TechnicalSheetImage TechnicalSheetBackImage { get; set; }
        public System.Drawing.Image StyleCoverImage { get; set; }
         
        public EnumDepartment Department { get; set; }
         
        public List<SampleRequirement> SampleRequirements { get; set; }
        //public List<LabDipOrderDetail> LabDipOrderDetails { get; set; }
        public List<DyeingOrderDetail> DyeingOrderDetails { get; set; }
        public List<WorkOrderDetail> WorkOrderDetails { get; set; }
        //public List<PackageBreakdown> PackageBreakdowns { get; set; }
        public List<SampleType> SampleTypes { get; set; }
        public MasterLC MasterLC { get; set; }
        public List<DyeingOrder> DyeingOrders { get; set; }
        public List<WorkOrder> WorkOrders { get; set; }
        public List<ImportLC> ImportLCs { get; set; }
        public List<ImportPI> ImportPIs { get; set; }
         
        public List<BillOfMaterial> BillOfMaterials { get; set; }

        public string AssortmentTypeInString 
        {
            get
            {
                return this.AssortmentType.ToString();
            }
        }
         
        public Company Company { get; set; }
         
        public ClientOperationSetting ClientOperationSetting { get; set; }
         
        public string StyleNo { get; set; }
         
        public string BuyerName { get; set; }
         
        public string MerchandiserName { get; set; }
         
        public string ColorRange { get; set; }
         
        public string SizeRange { get; set; }
         
        public string FabricName { get; set; }
         
        public string BuyerContactPerson { get; set; }
         
        public string ProductName { get; set; }
         
        public string AgentName { get; set; }
         
        public string ApproveByName { get; set; }
         
        public string ProductionFactoryName { get; set; }
         
        public string StyleDescription { get; set; }
         
        public double YarnRequired { get; set; }
         
        public double LCValue { get; set; }
        

        public string Approved
        {
            get
            {
                if (ApproveBy != 0)
                {
                    return ("Approved");
                }
                else
                {
                    return ("Not Approved");
                }
            }
        }

        public string OrderRecapWithStyleNo
        {
            get
            {
                return this.OrderRecapNo + "(" + this.StyleNo + ")";
            }
        }
        public string OrderRecapStatusInString
        {
            get
            {
                return OrderRecapStatus.ToString();
            }
        }

        public string FactoryShipmentDateInString
        {
            get
            {
                return this.FactoryShipmentDate.ToString("dd MMM yyyy");
            }
            
        }
        public string OrderRecapWithIDInString
        {
            get
            {
                //REfID ~RefNo~RefType //don't delete use in Merchanidsing Report and deshboard
                return this.OrderRecapID+"~"+this.OrderRecapNo+"~2";
            }
        }
        public int ProductCategoryID { get; set; }
         
        public int MeasurementUnitID { get; set; }

        public string UnitName { get; set; }

        #endregion

        #region Functions

        public OrderRecap Get(int id, long nUserID)
        {
            return OrderRecap.Service.Get(id, nUserID);
        }
        public OrderRecap GetByLog(int id, long nUserID)
        {
            return OrderRecap.Service.GetByLog(id, nUserID);
        }
        public OrderRecap UpdateCMValue(int id, double nCMValue, long nUserID)
        {
            return OrderRecap.Service.UpdateCMValue(id, nCMValue, nUserID);

        }
        public OrderRecap UpdateInfo( long nUserID)
        {
            return OrderRecap.Service.UpdateInfo(this, nUserID);
        }
        public static List<OrderRecap> Gets(long nUserID)
        {
            return OrderRecap.Service.Gets(nUserID);
        }
        public static List<OrderRecap> GetsByBUWithOrderType(int nBUIID,string sOType,  long nUserID)
        {
            return OrderRecap.Service.GetsByBUWithOrderType(nBUIID, sOType, nUserID);
        }
        
        public OrderRecap Save(long nUserID)
        {
            return OrderRecap.Service.Save(this, nUserID);

        }
   
        public OrderRecap AcceptRevise (long nUserID)
        {
            return OrderRecap.Service.AcceptRevise(this, nUserID);
        }
        public OrderRecap ActiveInActive(long nUserID)
        {
            return OrderRecap.Service.ActiveInActive(this, nUserID);
        }
        public OrderRecap ShippedUnShipped(long nUserID)
        {
            return OrderRecap.Service.ShippedUnShipped(this, nUserID);
        }
  
        public string Delete(int id, long nUserID)
        {
            return OrderRecap.Service.Delete(id, nUserID);
        }

        public static List<OrderRecap> Gets(string sSQL, long nUserID)
        {
            return OrderRecap.Service.Gets(sSQL, nUserID);
        }
      

        public static string PickNewColor(List<TechnicalSheetColor> oTechnicalSheetColors, long nUserID)
        {
            return OrderRecap.Service.PickNewColor(oTechnicalSheetColors, nUserID);

        }

        public OrderRecap Gets_byTechnicalSheet(int nTechnicalSheetID, int nOrderTypeId, long nUserID)
        {
            return OrderRecap.Service.Gets_byTechnicalSheet(nTechnicalSheetID,nOrderTypeId, nUserID);
        }
             
        public OrderRecap ChangeStatus( long nUserID)
        {
            return OrderRecap.Service.ChangeStatus(this, nUserID);
        }

        public OrderRecap ApprovedOrderRecap(long nUserID)
        {
            return OrderRecap.Service.ApprovedOrderRecap(this, nUserID);

        }

        #endregion

        #region ServiceFactory
 
        internal static IOrderRecapService Service
        {
            get { return (IOrderRecapService)Services.Factory.CreateService(typeof(IOrderRecapService)); }
        }

        #endregion
    }
    #endregion


    #region IOrderRecapService interface
     
    public interface IOrderRecapService
    {
         
        OrderRecap Get(int id, Int64 nUserID);
         
        OrderRecap GetByLog(int id, Int64 nUserID);
         
        OrderRecap UpdateCMValue(int id, double nCMValue, Int64 nUserID);
         
        OrderRecap UpdateInfo(OrderRecap oOrderRecap, Int64 nUserID);
         
        string PickNewColor(List<TechnicalSheetColor> oTechnicalSheetColors, Int64 nUserID);
         
        List<OrderRecap> Gets(Int64 nUserID);

        List<OrderRecap> GetsByBUWithOrderType(int nBUID,string sOType,  Int64 nUserID);        
         
        List<OrderRecap> Gets(string sSQL, Int64 nUserID);
         
        string Delete(int id, Int64 nUserID);
         
        OrderRecap Save(OrderRecap oOrderRecap, Int64 nUserID);
    
        OrderRecap AcceptRevise(OrderRecap oOrderRecap, Int64 nUserID);
        OrderRecap ActiveInActive(OrderRecap oOrderRecap, Int64 nUserID);
        OrderRecap ShippedUnShipped(OrderRecap oOrderRecap, Int64 nUserID);    
         
        OrderRecap Gets_byTechnicalSheet(int nTechnicalSheetID, int nOrderTypeId, Int64 nUserID);     
         
        OrderRecap ChangeStatus(OrderRecap oOrderRecap, Int64 nUserID);
         
        OrderRecap ApprovedOrderRecap(OrderRecap oOrderRecap, Int64 nUserID);
    }
    #endregion   
}
