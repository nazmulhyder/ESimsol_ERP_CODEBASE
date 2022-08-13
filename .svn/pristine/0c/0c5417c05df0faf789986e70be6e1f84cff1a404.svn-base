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
	#region ProductionOrder  
	public class ProductionOrder : BusinessObject
	{	
		public ProductionOrder()
		{
			ProductionOrderID = 0;
            ProductionOrderLogID = 0;
			PONo = "";
            ReviseNo = 0;
            IsNewVersion = false;
            FullPONo = "";
			OrderDate = DateTime.Now;
            ProductionOrderStatus = EnumProductionOrderStatus.Intialize;
			BUID = 0;
            ContractorID = 0; 
			ContactPersonnelID = 0; 
			Note = ""; 
			ExportSCID = 0; 
			ApproveDate = DateTime.Now; 
			ApproveBy = 0; 
			DeliveryTo = 0; 
			DeliveryContactPerson = 0; 
			ContractorName = ""; 
			ContractorAddress = ""; 
			ContactPersonnelName = ""; 
			DeliveryContactPersonName = ""; 
			ExportPINo = ""; 
			ApproveByName = ""; 
			DeliveryToName = ""; 
			Qty = 0; 
            BuyerID = 0;
            BuyerName = "";
            ExpectedDeliveryDate = DateTime.Now;
            MKTEmpID = 0;
            MKTPName = "";
            MKTPNickName = "";
            LCNo = "";
            YetToProductionScheduleQty = 0;
            ProductNature = EnumProductNature.Dyeing;
            ProductionOrderDetails = new List<ProductionOrderDetail>();
            SizeCategories = new List<SizeCategory>();
            ColorCategories = new List<ColorCategory>();
            ColorSizeRatios = new List<ColorSizeRatio>();
            POSizerBreakDowns = new List<POSizerBreakDown>();
            PrepareBy = 0;
            DirApprovedBy = 0;
            PrepareByName = "";
            DirApprovedByName = "";
            DBServerDateTime = DateTime.Now;
            LastReviseDate = DateTime.Now;
			ErrorMessage = "";
		}

		#region Property
		public int ProductionOrderID { get; set; }
        public int ProductionOrderLogID { get; set; }
		public string PONo { get; set; }
        public int ReviseNo { get; set; }
        public bool IsNewVersion { get; set; }
        public string FullPONo { get; set; }
		public DateTime OrderDate { get; set; }
		public int BUID { get; set; }
        public EnumProductionOrderStatus ProductionOrderStatus { get; set; }
        public int ContractorID { get; set; }
        public int ContactPersonnelID { get; set; }
		public string Note { get; set; }
		public int ExportSCID { get; set; }
		public DateTime ApproveDate { get; set; }
		public int ApproveBy { get; set; }
		public int DeliveryTo { get; set; }
		public int DeliveryContactPerson { get; set; }
		public string ContractorName { get; set; }
		public string ContractorAddress { get; set; }
		public string ContactPersonnelName { get; set; }
		public string DeliveryContactPersonName { get; set; }
		public string ExportPINo { get; set; }
		public string ApproveByName { get; set; }
		public string DeliveryToName { get; set; }
		public double Qty { get; set; }
        public int ProductionOrderStatusInInt { get; set; }
        public int OperationBy { get; set;}
        public int BuyerID { get; set;}
        public string BuyerName { get; set;}
        public DateTime ExpectedDeliveryDate { get; set;}
        public EnumProductionOrderActionType ProductionOrderActionType { get; set; }
        public int MKTEmpID { get; set; }
        public string  MKTPName { get; set; }
        public string MKTPNickName { get; set; }
        public string LCNo { get; set; }
        public EnumProductNature ProductNature { get; set; }
        public int ProductNatureInt { get; set; }
        public double YetToProductionScheduleQty { get; set; }
        public int PrepareBy { get; set; }
        public int DirApprovedBy { get; set; }
        public string   PrepareByName { get; set; }
        public string DirApprovedByName { get; set; }
        public DateTime DBServerDateTime { get; set; }
        public DateTime LastReviseDate { get; set; }
		public string ErrorMessage { get; set; }
		#endregion 

		#region Derived Property
        public List<ProductionOrderDetail> ProductionOrderDetails { get; set; }
        public List<ProductionOrder> ProductionOrderList { get; set; }
        public ApprovalRequest ApprovalRequest { get; set; }
        public ReviseRequest ReviseRequest { get; set; }
        public Company Company { get; set; }
        public Contractor Buyer { get; set; }
        public Contractor DeliveryToPerson { get; set; }
        public BusinessUnit BusinessUnit { get; set; }
        public List<SizeCategory> SizeCategories { get; set; }
        public List<ColorCategory> ColorCategories { get; set; }
        public List<ColorSizeRatio> ColorSizeRatios { get; set; }
        public List<POSizerBreakDown> POSizerBreakDowns { get; set; }
        public string ActionTypeExtra { get; set; }
        public string ExpectedDeliveryDateInString
        {
            get
            {
                return ExpectedDeliveryDate.ToString("dd MMM yyyy");
            }
        }
		public string OrderDateInString 
		{
			get
			{
				return OrderDate.ToString("dd MMM yyyy") ; 
			}
		}
		public string ApproveDateInString 
		{
			get
			{
				if(this.ApproveDate==DateTime.MinValue)
                {
                    return "-"; 
                }
                else
                {
                    return ApproveDate.ToString("dd MMM yyyy"); 
                }
                
			}
		}
        public string DBServerDateTimeInString 
		{
			get
			{
                return DBServerDateTime.ToString("dd MMM yyyy hh:mm tt"); 
			}
		}
        
	    public string ProductionOrderStatusInString
        {
            get
            {
                return this.ProductionOrderStatus.ToString();
            }
        }
        public string LastReviseDateSt
        {
            get 
            {
                if (this.LastReviseDate == DateTime.MinValue)
                {
                    return "";
                }
                else
                {
                    return this.LastReviseDate.ToString("dd MMM yyyy");
                }
            }
        }
		#endregion 

		#region Functions 
		public static List<ProductionOrder> Gets(long nUserID)
		{
			return ProductionOrder.Service.Gets(nUserID);
		}
        public static List<ProductionOrder> BUWithProductNatureWiseGets(int nBUID, int ProductNature, long nUserID)
        {
            return ProductionOrder.Service.BUWithProductNatureWiseGets(nBUID, ProductNature, nUserID);
        }
		public static List<ProductionOrder> Gets(string sSQL, long nUserID)
		{
			return ProductionOrder.Service.Gets(sSQL,nUserID);
		}
		public ProductionOrder Get(int id, long nUserID)
		{
			return ProductionOrder.Service.Get(id,nUserID);
		}
        public ProductionOrder GetByLog(int nLogid, long nUserID)
        {
            return ProductionOrder.Service.GetByLog(nLogid, nUserID);
        }
		public ProductionOrder Save(long nUserID)
		{
			return ProductionOrder.Service.Save(this,nUserID);
		}

        public ProductionOrder AcceptRevise(long nUserID)
		{
            return ProductionOrder.Service.AcceptRevise(this, nUserID);
		}
       
        public ProductionOrder ChangeStatus(long nUserID)
        {
            return ProductionOrder.Service.ChangeStatus(this, nUserID);
        }
		public  string  Delete(int id, long nUserID)
		{
			return ProductionOrder.Service.Delete(id,nUserID);
		}
		#endregion

		#region ServiceFactory
		internal static IProductionOrderService Service
		{
			get { return (IProductionOrderService)Services.Factory.CreateService(typeof(IProductionOrderService)); }
		}
		#endregion
	}
	#endregion

	#region IProductionOrder interface
	public interface IProductionOrderService 
	{
		ProductionOrder Get(int id, Int64 nUserID);
        ProductionOrder GetByLog(int id, Int64 nUserID); 
		List<ProductionOrder> Gets(Int64 nUserID);
        List<ProductionOrder> BUWithProductNatureWiseGets(int nBUID, int nProductNature, Int64 nUserID);
        
		List<ProductionOrder> Gets( string sSQL, Int64 nUserID);
		string Delete(int id, Int64 nUserID);
 		ProductionOrder Save(ProductionOrder oProductionOrder, Int64 nUserID);
        ProductionOrder AcceptRevise(ProductionOrder oProductionOrder, Int64 nUserID);
        
        ProductionOrder ChangeStatus(ProductionOrder oProductionOrder, Int64 nUserID);
        
	}
	#endregion
}
