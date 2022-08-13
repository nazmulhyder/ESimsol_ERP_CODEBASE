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
	#region ProductionOrderDetail  
	public class ProductionOrderDetail : BusinessObject
	{	
		public ProductionOrderDetail()
		{
			ProductionOrderDetailID = 0; 
			ProductionOrderID = 0;
            ProductionOrderLogID = 0;
            ProductionOrderDetailLogID = 0;
			ProductID = 0; 
			ColorID = 0;
            ProductDescription = ""; 
			StyleDescription = ""; 
			Measurement = ""; 
			Qty = 0; 
			BuyerRef = ""; 
			Note = ""; 
			ModelReferenceID = 0;
            ProductCode = "";
			ProductName = ""; 
			ModelReferenceName = ""; 
            FinishGoodsWeight = 0;
            NaliWeight = 0;
            WeigthFor = 0;
            FinishGoodsUnit = 0;
            FGUSymbol = "";
			ColorName = ""; 
			SizeName = "";
            SizeInCM = "";
            UnitName = "";
            UnitID = 0;
            YetToProductionOrderQty = 0;
            ExportSCDetailID = 0;
            UnitSymbol = "";
            YetToProductionSheeteQty = 0;
            ColorQty = 0;
            IsApplySizer = false;//for product
            ExportPINo = "";
            PONo = "";
            ContractorID = 0;
            ContractorName = "";
            BUID = 0;
            OrderDate = DateTime.Today;
            ProductNature = EnumProductNature.Dyeing;
            ProductNatureInInt = 0;
            PolyMUnitID = 0;
            Model = "";
            PantonNo = "";
			ErrorMessage = "";
		}

		#region Property
		public int ProductionOrderDetailID { get; set; }
        public int ProductionOrderLogID { get; set; }
        public int ProductionOrderDetailLogID { get; set; }
		public int ProductionOrderID { get; set; }
		public int ProductID { get; set; }
		public int ColorID { get; set; }
        public int PolyMUnitID { get; set; }
        public string ProductDescription { get; set; }
		public string StyleDescription { get; set; }
		public string Measurement { get; set; }
		public double Qty { get; set; }
		public string BuyerRef { get; set; }
        public double ColorQty { get; set; }
		public string Note { get; set; }
		public int ModelReferenceID { get; set; }
        public string ProductCode { get; set; }
		public string ProductName { get; set; }
		public string ModelReferenceName { get; set; }
		public string ColorName { get; set; }
		public string SizeName { get; set; }
        public string SizeInCM { get; set; }
        public string UnitName { get; set; }
        public int UnitID { get; set; }
        public string UnitSymbol { get; set; }
        public double YetToProductionOrderQty { get; set; }
        public int ExportSCDetailID { get; set; }
        public double YetToProductionSheeteQty { get; set; }
        public bool IsApplySizer { get; set; }
        public string ExportPINo { get; set; }
        public string PONo { get; set; }
        public int ContractorID { get; set; }
        public string ContractorName { get; set; }
        public int BUID { get; set; }
        public double FinishGoodsWeight { get; set; }
        public double NaliWeight { get; set; }
        public double WeigthFor { get; set; }
        public int FinishGoodsUnit { get; set; }
        public string FGUSymbol { get; set; }
        public EnumProductNature ProductNature { get; set; }
        public int ProductNatureInInt { get; set; }
        public DateTime OrderDate { get; set; }
        public string Model { get; set; }
        public string PantonNo { get; set; }
		public string ErrorMessage { get; set; }
		#endregion 

		#region Derived Property
        public string OrderDateInString
        {
            get
            {
                return this.OrderDate.ToString("dd MMM yyyy");
            }
        }
        public string QtySt
        {
            get
            {
                return Global.MillionFormatActualDigit(this.Qty);
            }
        }
        public string QtyWithUnitSymbol
        {
            get
            {
                return Global.MillionFormatActualDigit(this.Qty)+" "+this.UnitSymbol;
            }
        }
        public string YetToProductionSheeteQtySt
        {
            get
            {
                return Global.MillionFormatActualDigit(this.YetToProductionSheeteQty);
            }
        }
		#endregion 

		#region Functions 
		public static List<ProductionOrderDetail> Gets(int nPOID, long nUserID)
		{
            return ProductionOrderDetail.Service.Gets(nPOID,nUserID);
		}

        public static List<ProductionOrderDetail> GetsByLog(int nPOLogID, long nUserID)
        {
            return ProductionOrderDetail.Service.GetsByLog(nPOLogID, nUserID);
        }
		public static List<ProductionOrderDetail> Gets(string sSQL, long nUserID)
		{
			return ProductionOrderDetail.Service.Gets(sSQL,nUserID);
		}
		public ProductionOrderDetail Get(int id, long nUserID)
		{
			return ProductionOrderDetail.Service.Get(id,nUserID);
		}
		
	
		#endregion

		#region ServiceFactory
		internal static IProductionOrderDetailService Service
		{
			get { return (IProductionOrderDetailService)Services.Factory.CreateService(typeof(IProductionOrderDetailService)); }
		}
		#endregion
	}
	#endregion

	#region IProductionOrderDetail interface
	public interface IProductionOrderDetailService 
	{
		ProductionOrderDetail Get(int id, Int64 nUserID); 
		List<ProductionOrderDetail> Gets(int nPOID, Int64 nUserID);
        List<ProductionOrderDetail> GetsByLog(int nPOLogID, Int64 nUserID);
		List<ProductionOrderDetail> Gets( string sSQL, Int64 nUserID);
	
	}
	#endregion
}
