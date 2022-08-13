using System;
using System.IO;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using System.Data;
using ICS.Core.Framework;
using ICS.Core.Utility;


namespace ESimSol.BusinessObjects
{
    #region PIReport

    public class DispoProduction : BusinessObject
    {
        public DispoProduction()
        {
            FabricSalesContractDetailID = 0;
            FabricSalesContractID = 0;
            FEOSID = 0;
            PaymentType = EnumPIPaymentType.None;
            CurrentStatus = EnumFabricPOStatus.Initialized;
            SCDate = DateTime.Now;
            ContractorID = 0;
            BuyerID = 0;
            MktAccountID = 0;
            CurrencyID = 2;
            LightSourceID = 0;
            LightSourceIDTwo = 0;
            Amount = 0;
            IsInHouse = true;
            LCTermID = 8;
            Note = "";
            ApprovedDate = DateTime.MinValue;
            BUID = 0;
            ContractorName = "";
            BuyerName = "";
            ContractorAddress = "";
            //ContractorPhone = "";
            //ContractorFax = "";
            //ContractorEmail = "";
            MKTPName = "";
            MKTGroup = "";
            Currency = "";
            LDNo = "";
            LCTermsName = "";
            ErrorMessage = "";
            PaymentType = EnumPIPaymentType.None;
            OrderType = 0;
            SCNoFull = "";
            FabricID = 0;
            ProductID = 0;
            MUnitID = 0;
            UnitPrice = 0;
            Amount = 0;
            Description = "";
            ProductCode = "";
            ProductName = "";
            ProductCount = "";
            PINo = "";
            MUName = "";
            Currency = "";
            ColorInfo = "";
            StyleNo = "";
            FabricNo = "";
            FabricWidth = "";
            Construction = "";
            ConstructionPI = "";
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
            ExeNo = "";
            DeliveryDate_PP = DateTime.MinValue;
            HandLoomNo = string.Empty;
            OptionNo = string.Empty;
            NoOfFrame = 0;
            SubmissionDate = DateTime.Now;
            WeftColor = string.Empty;
            IsPrint = false;
            this.LabStatus = EnumFabricLabStatus.Initialized;
            DeliveryDate_Full = DateTime.MinValue;
            SLNo = 0;
            BUID = 0;
            ReviseNo = 0;
            BuyerCPersonName = "";
            EndUse = "";
            LightSourceName = "";
            LightSourceNameTwo = "";
            GarmentWash = "";
            QualityParameters = "";
            Emirzing = "";
            BuyerAddress = "";
            FabricReceiveByName = "";
            Code = "";
            FNLabdipDetailID = 0;
            Qty = 0;
            QtyDispo = 0;
            QtyWarp = 0;
            QtySizing = 0;
            QtyWeaving = 0;
            GreyRecd = 0;
            StoreRcvQty = 0;
            DCQty = 0;
            RCQty = 0;
            StockInHand = 0;
        }

        #region Properties
        public int FabricSalesContractDetailID { get; set; }
        public int FabricSalesContractID { get; set; }
        public int FEOSID { get; set; }
        public EnumPIPaymentType PaymentType { get; set; }
        public int OrderType { get; set; }
        public string OrderName { get; set; }
        public string SCNoFull { get; set; }
        public EnumFabricPOStatus CurrentStatus { get; set; }
        public DateTime SCDate { get; set; }
        public DateTime DeliveryDate_PP { get; set; }
        public DateTime ApprovedDate { get; set; }
        public int ContractorID { get; set; }
        public int BuyerID { get; set; }
        public int ContactPersonnelID { get; set; }
        public string BuyerCPName { get; set; }
        public int ContactPersonnelID_Buyer { get; set; }
        public int MktAccountID { get; set; }
        public int CurrencyID { get; set; }
        public int LightSourceID { get; set; }
        public int LightSourceIDTwo { get; set; }
        public bool IsInHouse { get; set; }
        public int LCTermID { get; set; }
        public string EndUse { get; set; }
        public string LightSourceName { get; set; }
        public string LightSourceNameTwo { get; set; }
        public string GarmentWash { get; set; }
        public string QualityParameters { get; set; }
        public string Note { get; set; }
        public string Emirzing { get; set; }
        public string ContractorName { get; set; }
        public string LDNo { get; set; }
        public string BuyerName { get; set; }
        public string ContractorAddress { get; set; }
        public string BuyerAddress { get; set; }
        public string Code { get; set; }
        public string MKTPName { get; set; }
        public string MKTGroup { get; set; }

        public int FNLabdipDetailID { get; set; }
        public int PreapeByID { get; set; }
        public string PreapeByName { get; set; }
        public string ApproveByName { get; set; }
        public string LCTermsName { get; set; }

        public EnumPOState Status { get; set; }
        public int LabDipID { get; set; }
        //public EnumPIStatus PIStatus { get; set; }
        public EnumFabricLabStatus LabStatus { get; set; }
        //public EnumExportLCStatus LCStatus { get; set; }
        public int SLNo { get; set; }
        public string ExeNo { get; set; }
        /// <summary>
        ///  Detail
        ///  
        /// </summary>
        public int BUID { get; set; }
        public int ReviseNo { get; set; }
        public string BuyerCPersonName { get; set; }
        public int FabricID { get; set; }
        public int ProductID { get; set; }
        public int MUnitID { get; set; }
        public double UnitPrice { get; set; }
        public double Amount { get; set; }
        public string Description { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string ProductCount { get; set; }
        public string PINo { get; set; }
        //public DateTime PIDate { get; set; }
        public string LCNo { get; set; }
        public string MUName { get; set; }
        public string Currency { get; set; }
        public string ProductNameCode
        {
            get
            {
                return this.ProductName + "[" + this.ProductCode + "]";
            }
        }
        public string StyleNo { get; set; }
        public string Size { get; set; }
        public string ColorInfo { get; set; }
        public string FabricNo { get; set; }
        public string Construction { get; set; }
        public string ConstructionPI { get; set; }
        public string FabricWidth { get; set; }
        public int ProcessType { get; set; }
        public int FabricWeave { get; set; }
        public int FinishType { get; set; }
        public string ProcessTypeName { get; set; }
        public string FabricWeaveName { get; set; }
        public string FinishTypeName { get; set; }
        public string Weight { get; set; }
        public string Shrinkage { get; set; }
        public string BuyerReference { get; set; }
        public string HLReference { get; set; }
        public string DesignPattern { get; set; }
        public int FabricDesignID { get; set; }
        public string FabricDesignName { get; set; }
        public DateTime FabricReceiveDate { get; set; }
        public int FabricReceiveBy { get; set; }
        public bool IsPrint { get; set; }
        public DateTime DeliveryDate_Full { get; set; }
        public string HandLoomNo { get; set; }
        public string OptionNo { get; set; }
        public int NoOfFrame { get; set; }
        public string WeftColor { get; set; }
        public string LabDipNo { get; set; }
        public string FabricReceiveByName { get; set; }
        public DateTime SubmissionDate { get; set; }

        public double Qty { get; set; }
        public double QtyDispo { get; set; }
        public double QtyWarp { get; set; }
        public double QtySizing { get; set; }
        public double QtyWeaving { get; set; }
        public double GreyRecd { get; set; }
        public double StoreRcvQty { get; set; }
        public double DCQty { get; set; }
        public double RCQty { get; set; }
        public double StockInHand { get; set; }
        #endregion

        #region Derive Property
        public string Params { get; set; }
        public string ErrorMessage { get; set; }
        public List<FNOrderFabricReceive> FNOrderFabricReceives { get; set; }
        public string ContractorNameCode
        {
            get
            {
                return this.ContractorName + "[" + this.ContractorID.ToString() + "]";
            }
        }
        public string SCDateSt
        {
            get
            {
                return this.SCDate.ToString("dd MMM yyyy");
            }
        }

        public string ApprovedDateSt
        {
            get
            {

                if (this.ApprovedDate == DateTime.MinValue)
                {
                    return "-";
                }
                else
                {
                    return this.ApprovedDate.ToString("dd MMM yyyy");
                }
            }
        }

        public string SubmissionDateSt
        {
            get
            {

                if (this.SubmissionDate == DateTime.MinValue)
                {
                    return "-";
                }
                else
                {
                    return this.SubmissionDate.ToString("dd MMM yyyy");
                }
            }
        }

        public string DeliveryDate_PPSt
        {
            get
            {

                if (this.DeliveryDate_PP == DateTime.MinValue)
                {
                    return "-";
                }
                else
                {
                    return this.DeliveryDate_PP.ToString("dd MMM yyyy");
                }
            }
        }

        public string DeliveryDate_FullSt
        {
            get
            {

                if (this.DeliveryDate_Full == DateTime.MinValue)
                {
                    return "-";
                }
                else
                {
                    return this.DeliveryDate_Full.ToString("dd MMM yyyy");
                }
            }
        }



        public string OrderTypeSt
        {
            get
            {
                return EnumObject.jGet((EnumFabricRequestType)this.OrderType); ;
            }
        }

        private string sCurrentStatusSt = "";
        public string CurrentStatusSt
        {
            get
            {
                if (this.FabricReceiveBy != 0)
                {
                    return "Received";
                }
                else
                {
                    sCurrentStatusSt = EnumObject.jGet(this.CurrentStatus);
                    return sCurrentStatusSt;
                }

            }
        }
        private string sLabStatusST = "";
        public string LabStatusST
        {
            get
            {
                sLabStatusST = EnumObject.jGet(this.LabStatus);
                return sLabStatusST;
            }
        }
        #region AmountSt
        public string AmountSt
        {
            get
            {
                return this.Currency + " " + Global.MillionFormat(this.Amount);
                //return this.Currency + "" + this.Amount.ToString("#,##0.0000");
            }
        }
        #endregion
        public string PaymentTypeSt
        {
            get
            {
                return EnumObject.jGet(this.PaymentType);
            }
        }

        public string FabricReceiveDateStr
        {
            get
            {
                return (this.FabricReceiveDate == DateTime.MinValue) ? "" : this.FabricReceiveDate.ToString("dd MMM yyyy");
            }
        }
        #endregion

    #endregion

        #region Functions
        public static List<DispoProduction> Gets(string sSQL, Int64 nUserID)
        {
            return DispoProduction.Service.Gets(sSQL, nUserID);
        }
        public static List<DispoProduction> UpdateMail(DispoProduction oDispoProduction, Int64 nUserID)
        {
            return DispoProduction.Service.UpdateMail(oDispoProduction, nUserID);
        }
        public static List<DispoProduction> Received(List<DispoProduction> oDispoProductions, long nUserID)
        {
            return DispoProduction.Service.Received(oDispoProductions, nUserID);
        }
        public DispoProduction SaveExcNo(long nUserID)
        {
            return DispoProduction.Service.SaveExcNo(this, nUserID);
        }
        public DispoProduction OperationLab(EnumDBOperation eBDO, long nUserID)
        {
            return DispoProduction.Service.OperationLab(this, eBDO, nUserID);
        }
        public DispoProduction CreateLab(EnumDBOperation eBDO, long nUserID)
        {
            return DispoProduction.Service.CreateLab(this, eBDO, nUserID);
        }
        public DispoProduction Get(int id, long nUserID)
        {
            return DispoProduction.Service.Get(id, nUserID);
        }
        public DispoProduction SaveExtra(FabricSalesContractDetail oFabricSalesContractDetail, EnumDBOperation oDBOperation, int nUserID)
        {
            return DispoProduction.Service.SaveExtra(oFabricSalesContractDetail, oDBOperation, nUserID);
        }
        #endregion

        #region ServiceFactory

        internal static IDispoProductionService Service
        {
            get { return (IDispoProductionService)Services.Factory.CreateService(typeof(IDispoProductionService)); }
        }
        #endregion

        public string StatusSt
        {
            get
            {
                return EnumObject.jGet(this.Status);
            }
        }

    }

    #region IPIReport interface
    public interface IDispoProductionService
    {
        List<DispoProduction> Gets(string sSQL, Int64 nUserID);
        List<DispoProduction> UpdateMail(DispoProduction oDispoProduction, Int64 nUserID);
        List<DispoProduction> Received(List<DispoProduction> oDispoProduction, long nUserID);
        DispoProduction SaveExcNo(DispoProduction oDispoProduction, long nUserID);
        DispoProduction OperationLab(DispoProduction oDispoProduction, EnumDBOperation eBDO, long nUserID);
        DispoProduction CreateLab(DispoProduction oDispoProduction, EnumDBOperation eBDO, long nUserID);
        DispoProduction Get(int id, Int64 nUserID);
        DispoProduction SaveExtra(FabricSalesContractDetail oFabricSalesContractDetail, EnumDBOperation oDBOperation, Int64 nUserID);
    }
    #endregion
}
