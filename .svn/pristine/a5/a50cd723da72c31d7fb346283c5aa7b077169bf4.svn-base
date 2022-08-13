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
    #region GRNDetail
    public class GRNDetail : BusinessObject
    {
        public GRNDetail()
        {
            GRNDetailID = 0;
            GRNID = 0;
            ProductID = 0;
            TechnicalSpecification = "";
            MUnitID = 0;
            RefQty = 0;
            ReceivedQty = 0;
            UnitPrice = 0;
            ItemWiseLandingCost = 0;
            InvoiceLandingCost = 0;
            LCLandingCost = 0;
            Amount = 0;           
            LotID = 0;
            LotNo = "";
            QtyPerPack = 0;
            NumberOfPack = 0;
            RefType = EnumGRNType.None;
            RefTypeInt = 0;
            RefObjectID = 0;
            StyleID = 0;
            ColorID = 0;
            SizeID = 0;
            GRNNo = "";
            MUName = "";
            MUSymbol = "";
            ProductName = "";
            ProductCategoryName = "";
            ProductCode = "";
            StyleNo = "";
            BuyerName = "";
            ColorName = "";
            SizeName = "";
            Origin = "";
            Brand = "";
            ProjectName = "";
            YetToReceiveQty = 0;
            DateYear = 0;
            DateMonth = 0;
            CurrencySymbol = "";
            RejectQty = 0;
            Remarks = "";
            WeightPerCartoon = 0;
            ConePerCartoon = 0;
            FinishDia = "";
            MCDia = "";
            GSM = "";
            Shade = "";
            Stretch_Length = "";
            RackID = 0;
            ErrorMessage = "";
        }
        #region Properties
        public int GRNDetailID { get; set; }
        public int GRNID { get; set; }
        public int ProductID { get; set; }
        public int VehicleModelID { get; set; }
        public string ModelShortName { get; set; }
		public string TechnicalSpecification { get; set; }
		public int MUnitID { get; set; }
		public double RefQty { get; set; }
		public double ReceivedQty { get; set; }
        public double RejectQty { get; set; }
        public string Remarks { get; set; }
        public int RackID { get; set; }
        public double WeightPerCartoon { get; set; }
        public double ConePerCartoon { get; set; }
		public double UnitPrice { get; set; }
        public double InvoiceLandingCost { get; set; }
        public double LCLandingCost { get; set; }
		public double Amount { get; set; }        
        public int LotID { get; set; }
        public string LotNo { get; set; }
        public double QtyPerPack { get; set; }
        public double NumberOfPack { get; set; }
        public EnumGRNType RefType { get; set; }
        public int RefTypeInt { get; set; }
        public int RefObjectID { get; set; }
        public string Shade { get; set; }
        public string Stretch_Length { get; set; }
        public int StyleID { get; set; }
        public int ColorID { get; set; }
        public int SizeID { get; set; }
		public string GRNNo { get; set; }
        public string MUName { get; set; }
        public string MUSymbol { get; set; }       
        public string ProductName { get; set; }
        public string ProductCode { get; set; }
        public string StyleNo { get; set; }
        public string BuyerName { get; set; }
        public string ColorName { get; set; }
        public string SizeName { get; set; }
        public string Origin { get; set; }
        public string Brand { get; set; }
        public string ProjectName { get; set; }
        public string ProductCategoryName { get; set; }
        public double YetToReceiveQty { get; set; }
        public int DateYear { get; set; }
        public int DateMonth { get; set; }
        public double ItemWiseLandingCost { get; set; }
        public string ErrorMessage { get; set; }
        public DateTime GRNDate { get; set; }
        public string CustomerName { get; set; }
        public string ImportInvoiceNo { get; set; }
        public string ImportLCNo { get; set; }
        public int ImportLCID { get; set; }
        public string CurrencySymbol { get; set; }
        public string FinishDia { get; set; }
        public string MCDia { get; set; }
        public string GSM { get; set; }
        public List<GRNLandingCost> GRNLandingCosts { get; set; }
        public Company Company { get; set; }
        #endregion
        #region Derived Property
        public string ShelfWithRackNo { get; set; }
        public double ActualQty { get; set; }
        public double SupplierPrice
        {
            get
            {
                return this.UnitPrice - (this.LCLandingCost + this.InvoiceLandingCost + this.ItemWiseLandingCost);
            }
        }
        public string SupplierPriceST
        {
            get
            {
                return this.CurrencySymbol+" "+ this.SupplierPrice.ToString("##,##0.00000");  //this.UnitPrice - (this.LCLandingCost + this.InvoiceLandingCost + this.ItemWiseLandingCost);
            }
        }
        #endregion

        #region Functions

        public GRNDetail Get(int id, int nUserID)
        {
            return GRNDetail.Service.Get(id, nUserID);
        }
        public GRNDetail Save(int nUserID)
        {
            return GRNDetail.Service.Save(this, nUserID);
        }
        public string Delete(int id, int nUserID)
        {
            return GRNDetail.Service.Delete(id, nUserID);
        }
        public static List<GRNDetail> Gets(int nUserID)
        {
            return GRNDetail.Service.Gets(nUserID);
        }
        public static List<GRNDetail> Gets(int nGRNID, int nUserID)
        {
            return GRNDetail.Service.Gets(nGRNID, nUserID);
        }
        public static List<GRNDetail> Gets(string sSQL, int nUserID)
        {
            return GRNDetail.Service.Gets(sSQL, nUserID);
        }

        public static List<GRNDetail> GetsRpt_Product(int BUID, int nWorkingUnitID, int DateYear, int nReportLayout, int nUserID)
        {
            return GRNDetail.Service.GetsRpt_Product(BUID, nWorkingUnitID, DateYear, nReportLayout, nUserID);
        }   
        #endregion

        #region ServiceFactory
        internal static IGRNDetailService Service
        {
            get { return (IGRNDetailService)Services.Factory.CreateService(typeof(IGRNDetailService)); }
        }
        #endregion

        
    }
    #endregion
       

    #region IGRNDetail interface
    public interface IGRNDetailService
    {
        GRNDetail Get(int id, int nUserID);
        List<GRNDetail> Gets(int nUserID);
        List<GRNDetail> Gets(int nGRNID, int nUserID);
        string Delete(int id, int nUserID);
        GRNDetail Save(GRNDetail oGRNDetail, int nUserID);        
        List<GRNDetail> Gets(string sSQL, int nUserID);
        List<GRNDetail> GetsRpt_Product(int BUID, int nWorkingUnitID, int DateYear, int ReportLayout, int nUserID);
    }
    #endregion
}