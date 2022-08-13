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
	#region OrderSheet  
	public class OrderSheet : BusinessObject
	{	
		public OrderSheet()
		{
			OrderSheetID = 0; 
			PONo = "";
            ReviseNo = 0;
            IsNewVersion = false;
            FullPONo = "";
			OrderDate = DateTime.Now;
            OrderSheetStatus = EnumOrderSheetStatus.Intialize;
			BUID = 0;
            OrderSheetType = EnumOrderSheetType.Sample;
            ContractorID = 0; 
			ContactPersonnelID = 0; 
			Priority = EnumPriorityLevel.None; 
			Note = ""; 
			MKTEmpID = 0; 
			ApproveDate = DateTime.Now; 
			ApproveBy = 0; 
			PartyPONo = ""; 
			PaymentType = EnumPaymentType.None; 
			DeliveryTo = 0; 
			DeliveryContactPerson = 0; 
			ContractorName = ""; 
			ContractorAddress = ""; 
			ContactPersonnelName = ""; 
			DeliveryContactPersonName = ""; 
			MKTPName = ""; 
			MKTPNickName = ""; 
			ApproveByName = ""; 
			DeliveryToName = ""; 
			Qty = 0; 
			Amount = 0;
            CurrencyName = "";
            CurrencySymbol = "";
            CurrencyID = 0;
            BuyerID = 0;
            BuyerName = "";
            RateUnit = 1;//default
            OrderSheetLogID = 0;
            ExpectedDeliveryDate = DateTime.Now;
            ProductNature = EnumProductNature.Dyeing;
            OrderSheetDetails = new List<OrderSheetDetail>();
            ReviseRequest = new BusinessObjects.ReviseRequest();
			ErrorMessage = "";
		}

		#region Property
		public int OrderSheetID { get; set; }
        public int OrderSheetLogID { get; set; }
		public string PONo { get; set; }
        public int ReviseNo { get; set; }
        public bool IsNewVersion { get; set; }
        public string FullPONo { get; set; }
		public DateTime OrderDate { get; set; }
		public int BUID { get; set; }
        public EnumOrderSheetStatus OrderSheetStatus { get; set; }
        public EnumOrderSheetType OrderSheetType { get; set; }
        public int ContractorID { get; set; }
		public int ContactPersonnelID { get; set; }
        public EnumPriorityLevel Priority { get; set; }
		public string Note { get; set; }
		public int MKTEmpID { get; set; }
		public DateTime ApproveDate { get; set; }
		public int ApproveBy { get; set; }
		public string PartyPONo { get; set; }
        public EnumPaymentType PaymentType { get; set; }
		public int DeliveryTo { get; set; }
		public int DeliveryContactPerson { get; set; }
		public string ContractorName { get; set; }
		public string ContractorAddress { get; set; }
		public string ContactPersonnelName { get; set; }
		public string DeliveryContactPersonName { get; set; }
		public string MKTPName { get; set; }
		public string MKTPNickName { get; set; }
		public string ApproveByName { get; set; }
		public string DeliveryToName { get; set; }
		public double Qty { get; set; }
		public double Amount { get; set; }
        public int OrderSheetStatusInInt { get; set; }
        public int OperationBy { get; set;}
        public string CurrencyName { get; set;}
        public string CurrencySymbol { get; set; }
        public int CurrencyID { get; set;}
        public int BuyerID { get; set;}
         public string BuyerName { get; set;}
         public DateTime ExpectedDeliveryDate { get; set;}
         public int RateUnit { get; set; }
         public EnumProductNature ProductNature { get; set; }
         public int ProductNatureInt { get; set; }
		public string ErrorMessage { get; set; }
		#endregion 

		#region Derived Property
        public List<OrderSheetDetail> OrderSheetDetails { get; set; }
        public List<OrderSheet> OrderSheetList { get; set; }
        public EnumOrderSheetActionType OrderSheetActionType { get; set; }
        public ApprovalRequest ApprovalRequest { get; set; }
        public ReviseRequest ReviseRequest { get; set; }
        public Company Company { get; set; }
        public Contractor Buyer { get; set; }
        public Contractor DeliveryToPerson { get; set; }
        public BusinessUnit BusinessUnit { get; set; }
        public string ActionTypeExtra { get; set; }
        public string AmountSt
        {
            get
            {
                return Global.MillionFormat(this.Amount);
            }
        }
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
				return ApproveDate.ToString("dd MMM yyyy") ; 
			}
		}
	    public string OrderSheetStatusInString
        {
            get
            {
                return this.OrderSheetStatus.ToString();
            }
        }
     

		#endregion 

		#region Functions 
		public static List<OrderSheet> Gets(long nUserID)
		{
			return OrderSheet.Service.Gets(nUserID);
		}
        public static List<OrderSheet> BUWiseWithProductNatureGets(int nBUID, int nProductNature, long nUserID)
        {
            return OrderSheet.Service.BUWiseWithProductNatureGets(nBUID, nProductNature, nUserID);
        }
		public static List<OrderSheet> Gets(string sSQL, long nUserID)
		{
			return OrderSheet.Service.Gets(sSQL,nUserID);
		}
		public OrderSheet Get(int id, long nUserID)
		{
			return OrderSheet.Service.Get(id,nUserID);
		}

        public OrderSheet GetByLog(int Logid, long nUserID)
        {
            return OrderSheet.Service.GetByLog(Logid, nUserID);
        }
		public OrderSheet Save(long nUserID)
		{
			return OrderSheet.Service.Save(this,nUserID);
		}

        public OrderSheet AcceptRevise(long nUserID)
		{
            return OrderSheet.Service.AcceptRevise(this, nUserID);
		}
       
        
        public OrderSheet ChangeStatus(long nUserID)
        {
            return OrderSheet.Service.ChangeStatus(this, nUserID);
        }
		public  string  Delete(int id, long nUserID)
		{
			return OrderSheet.Service.Delete(id,nUserID);
		}
		#endregion

		#region ServiceFactory
		internal static IOrderSheetService Service
		{
			get { return (IOrderSheetService)Services.Factory.CreateService(typeof(IOrderSheetService)); }
		}
		#endregion
	}
	#endregion

	#region IOrderSheet interface
	public interface IOrderSheetService 
	{
		OrderSheet Get(int id, Int64 nUserID);
        OrderSheet GetByLog(int Logid, Int64 nUserID); 
        
		List<OrderSheet> Gets(Int64 nUserID);
        List<OrderSheet> BUWiseWithProductNatureGets(int nBUID,int nProductNature, Int64 nUserID);
        
		List<OrderSheet> Gets( string sSQL, Int64 nUserID);
		string Delete(int id, Int64 nUserID);
 		OrderSheet Save(OrderSheet oOrderSheet, Int64 nUserID);
        OrderSheet AcceptRevise(OrderSheet oOrderSheet, Int64 nUserID);
        
        OrderSheet ChangeStatus(OrderSheet oOrderSheet, Int64 nUserID);
        
	}
	#endregion
}
