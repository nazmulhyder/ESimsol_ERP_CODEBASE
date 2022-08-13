using System;
using System.IO;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using ICS.Core.Framework;
using ICS.Core.Utility;

namespace ESimSol.BusinessObjects
{
    #region FabricSalesContractDetail
    public class FabricSalesContractDetail : BusinessObject
    {
        public FabricSalesContractDetail()
        {
            FabricSalesContractDetailID = 0;
            FabricSalesContractID = 0;
            FabricID = 0;
            ProductID = 0;
            Qty = 0.00;
            MUnitID = 0;
            UnitPrice = 0.00;
            Amount = 0;
            ProductCode = "";
            ProductName = "";
            ErrorMessage = "";
            PINo = "";
            MUName = "";
            Currency = "";
            Note = "";
            ColorInfo = "";
            StyleNo = "";
            FabricNo = "";
            Code = "";
            FabricWidth = "";
            Construction = "";
            ProcessType = 0;
            FabricWeave = 0;
            FinishType = 0;
            ProcessTypeName = "";
            FabricWeaveName = "";
            FinishTypeName = "";
            HLReference = "";
            DesignPattern = "";
            FabricDesignName = "";
            FabricDesignID = 0;
            BuyerName = "";
            ExeNo = "";
            IsProduction = true;
            ExportPIID = 0;
            ExportPIDetailID = 0;
            Qty_PI = 0;
            ConstructionPI = "";
            OptionNo = string.Empty;
            FNLabdipDetailID = 0;
            ShadeID = 0;
            LDNo = "";
            Weight = "";
            Shrinkage = "";
            Status = EnumPOState.None;
            SCNo = "";
             //FNExecutionOrderProcessList  = new List<FNExecutionOrderProcess>();
            SLNo = 0;
            SCNoFull = "";
            ShadeCount = 0;
            PantonNo = "";
            SCDate = DateTime.MinValue;
            OrderName = "";
            ExeNoFull = "";
            IsBWash = false;
            SCDetailType = EnumSCDetailType.None; // Geranal
            YarnType = "";
            FabricSCHistorys = new List<FabricSCHistory>();
        }

        #region Properties
        public int FabricSalesContractDetailID { get; set; }
        public int FabricSalesContractID { get; set; }
        public int FabricID { get; set; }
        public int ProductID { get; set; }
        public double Qty { get; set; }
        public double Qty_PI { get; set; }
        public int MUnitID { get; set; }
        public double UnitPrice { get; set; }
        public double Amount { get; set; }
        public string OrderName { get; set; }
        public string ExeNoFull { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string PINo { get; set; }
        public string MUName { get; set; }
        public string Currency { get; set; }
        public string ErrorMessage { get; set; }
        public int FabricDesignID { get; set; }
        public string FabricDesignName { get; set; }
        public bool IsProduction { get; set; }
        public bool IsBWash { get; set; }
        public int ShadeCount { get; set; }
        public string Code { get; set; }
        public string PantonNo { get; set; }
        public string Weight { get; set; }
        public string Shrinkage { get; set; }
        public int SLNo { get; set; }
        public string SCNoFull { get; set; }
        public string BuyerName { get; set; }
        public DateTime SCDate { get; set; }
        public string YarnType { get; set; }
        
        #endregion

        #region Derived Property
        public string ContractorName { get; set; }
        public bool IsWithMail { get; set; }
        public string Note { get; set; }
        public string StyleNo { get; set; }
        public string Size { get; set; }
        public string ColorInfo { get; set; }
        public string FabricNo { get; set; }
        public string Construction { get; set; }
        public string ConstructionPI { get; set; }
        public string FabricWidth { get; set; }
        public int ProcessType { get; set; }
        public int FabricWeave { get; set; }
        public int ExportPIID { get; set; }
        public int ExportPIDetailID { get; set; }
        public int FNLabdipDetailID { get; set; }
        public int ShadeID { get; set; }
        public EnumPOState Status { get; set; }
        public int SCDetailTypeInt { get; set; }
        public EnumSCDetailType SCDetailType { get; set; }
        public int FinishType { get; set; }
        public string ProcessTypeName { get; set; }
        public string FabricWeaveName { get; set; }
        public List<FNExecutionOrderProcess> FNExecutionOrderProcessList { get; set; }
        public string FinishTypeName { get; set; }
        public string BuyerReference { get; set; }
        public string HLReference { get; set; }
        public string DesignPattern { get; set; }
        public string ExeNo { get; set; }
        public string LDNo { get; set; }
        public string OptionNo { get; set; }
        public string SCNo { get; set; }
        //public string DispoNo { get; set; }
        public double OrderQty { get; set; }
        //public double DispoQty { get; set; }
        public double RawFabricRcvQty { get; set; }
        public double PlannedQty { get; set; }
        public double BatchQty { get; set; }
        public double DeliveredQty { get; set; }
        public double Balance { get; set; }
        public string LabDipNo { get; set; }
        public EnumPOState PreviousStatus { get; set; }
        public List<FabricSCHistory> FabricSCHistorys { get; set; }
        public string ProductNameCode
        {
            get
            {
                return this.ProductName + "[" + this.ProductCode + "]";
            }
        }
        public string AmountSt
        {
            get
            {
                return  this.Currency+""+ Global.MillionFormat(this.Amount);
            }
        }
        public string StatusSt
        {
            get
            {
                return EnumObject.jGet((EnumPOState)this.Status);
            }
        }
        public string IsProductionSt
        {
            get
            {
                if (this.IsProduction)
                    return "Production";
                else return "Only Sale";
            }
        }
        public string SCDateSt
        {
            get
            {
                if(SCDate == DateTime.MinValue)
                {
                    return "-";
                }
                else
                {
                    return this.SCDate.ToString("dd MMM yyyy");
                }
            }
        }
      
        private double _nQtyTwo;
        public double QtyTwo
        {
            get
            {
              
                    _nQtyTwo = Math.Round((this.Qty/1.0936132983),5); //0.45359237001003542909395360718511
                
                return _nQtyTwo;
            }
        }
        private double _nUnitPriceTwo;
        public double UnitPriceTwo
        {
            get
            {
               
                    // 2-Yard To Meter
                    _nUnitPriceTwo = Math.Round((UnitPrice * 1.0936132983), 8); //1.0936133
                        return _nUnitPriceTwo;
            }
        }
        private string _sMUNameTwo;
        public string MUNameTwo
        {
            get
            {
             
                if (this.MUnitID == 17)
                {
                    /// 2-Yard To Meter
                    _sMUNameTwo = "MTR";
                }
                else
                {
                    /// 2-Yard To Meter
                    _sMUNameTwo = "Kg";
                }
                return _sMUNameTwo;
            }
        }
        public string OrderQtyInStr
        {
            get
            {
                if (this.OrderQty > 0)
                {
                    return Global.MillionFormat(this.OrderQty) + "(Y)";
                }
                else
                {
                    return "-";
                }
            }

        }
        public string RawFabricRcvQtyInStr
        {
            get
            {
                if (this.RawFabricRcvQty > 0)
                {
                    return Global.MillionFormat(this.RawFabricRcvQty);
                }
                else
                {
                    return "-";
                }
            }

        }
        public string PlannedQtyInStr
        {
            get
            {
                if (this.PlannedQty > 0)
                {
                    return Global.MillionFormat(this.PlannedQty);
                }
                else
                {
                    return "-";
                }
            }
        }

        public string BatchQtyInStr
        {
            get
            {
                if (this.BatchQty > 0)
                {
                    return Global.MillionFormat(this.BatchQty);
                }
                else
                {
                    return "-";
                }
            }

        }
        public string DeliveredQtyInStr
        {
            get
            {
                if (this.DeliveredQty > 0)
                {
                    return Global.MillionFormat(this.DeliveredQty);
                }
                else
                {
                    return "-";
                }
            }

        }

        public string BalanceInStr
        {
            get
            {
                if (this.Balance > 0)
                {
                    return Global.MillionFormat(this.Balance);
                }
                else
                {
                    return "-";
                }
            }

        }
       
       
         
        #endregion

        #region Functions
        public static List<FabricSalesContractDetail> Gets(int nFabricSalesContractID, Int64 nUserID)
        {
            return FabricSalesContractDetail.Service.Gets(nFabricSalesContractID, nUserID);
        }
     
        public static List<FabricSalesContractDetail> Gets(Int64 nUserID)
        {
            return FabricSalesContractDetail.Service.Gets(nUserID);
        }

        public static List<FabricSalesContractDetail> GetsLog(int nFabricSalesContractLogID, Int64 nUserID)
        {
            return FabricSalesContractDetail.Service.GetsLog(nFabricSalesContractLogID, nUserID);
        }
        public static List<FabricSalesContractDetail> GetsReport(string sSQL, long nUserID)
        {
            return FabricSalesContractDetail.Service.GetsReport(sSQL, nUserID);
        }
        public static List<FabricSalesContractDetail> Gets(string sSQL, Int64 nUserID)
        {
            return FabricSalesContractDetail.Service.Gets(sSQL, nUserID);
        }
        public FabricSalesContractDetail SaveProcess(FabricSalesContractDetail oFabricSalesContractDetail, long nUserID)
        {
            return FabricSalesContractDetail.Service.SaveProcess(oFabricSalesContractDetail, nUserID);
        }
        public FabricSalesContractDetail Get(int id, Int64 nUserID)
        {
            return FabricSalesContractDetail.Service.Get(id, nUserID);
        }
        public static List<FabricSalesContractDetail> Save_UpdateDispoNo(List<FabricSalesContractDetail> oFabricSalesContractDetails, int nUserID)
        {
            return FabricSalesContractDetail.Service.Save_UpdateDispoNo(oFabricSalesContractDetails, nUserID);
        }
        public static List<FabricSalesContractDetail> SetHandLoomNo(List<FabricSalesContractDetail> oFabricSalesContractDetails, int nUserID)
        {
            return FabricSalesContractDetail.Service.SetHandLoomNo(oFabricSalesContractDetails, nUserID);
        }
        public static List<FabricSalesContractDetail> SetFabricExcNo(FabricSalesContract oFabricSalesContract, int nUserID)
        {
            return FabricSalesContractDetail.Service.SetFabricExcNo(oFabricSalesContract, nUserID);
        }
        public  FabricSalesContractDetail SaveLDNo(FabricSalesContractDetail oFabricSalesContractDetail, int nUserID)
        {
            return FabricSalesContractDetail.Service.SaveLDNo(oFabricSalesContractDetail, nUserID);
        }
        public FabricSalesContractDetail OrderHold(Int64 nUserID)
        {
            return FabricSalesContractDetail.Service.OrderHold(this, nUserID);
        }
        public FabricSalesContractDetail UpdateStatus(long nUserID)
        {
            return FabricSalesContractDetail.Service.UpdateStatus(this, nUserID);
        }

        #endregion

        #region conversion
      

      
        #endregion

        #region ServiceFactory
        internal static IFabricSalesContractDetailService Service
        {
            get { return (IFabricSalesContractDetailService)Services.Factory.CreateService(typeof(IFabricSalesContractDetailService)); }
        }
        #endregion
    }
    #endregion

    #region IFabricSalesContractDetail interface
    public interface IFabricSalesContractDetailService
    {
        FabricSalesContractDetail Get(int id, Int64 nUserID);
        List<FabricSalesContractDetail> Gets(int nFabricSalesContractID, Int64 nUserID);
        List<FabricSalesContractDetail> Gets(Int64 nUserID);
        List<FabricSalesContractDetail> Gets(string sSQL, Int64 nUserID);
        List<FabricSalesContractDetail> GetsReport(string sSQL, Int64 nUserID);
        List<FabricSalesContractDetail> GetsLog(int nFabricSalesContractLogID, Int64 nUserID);
        FabricSalesContractDetail SaveProcess(FabricSalesContractDetail oFabricSalesContractDetail, Int64 nUserID);
        List<FabricSalesContractDetail> Save_UpdateDispoNo(List<FabricSalesContractDetail> oFabricSalesContractDetails, Int64 nUserID);
       List<FabricSalesContractDetail> SetHandLoomNo(List<FabricSalesContractDetail> oFabricSalesContractDetails, Int64 nUserID);
        List<FabricSalesContractDetail> SetFabricExcNo(FabricSalesContract oFabricSalesContract, Int64 nUserID);
        FabricSalesContractDetail OrderHold(FabricSalesContractDetail oFabricSalesContractDetail, Int64 nUserID);
        FabricSalesContractDetail SaveLDNo(FabricSalesContractDetail oFabricSalesContractDetail, Int64 nUserID);
        FabricSalesContractDetail UpdateStatus(FabricSalesContractDetail oFabricSalesContractDetail, Int64 nUserID);
        
    }
    #endregion
}
