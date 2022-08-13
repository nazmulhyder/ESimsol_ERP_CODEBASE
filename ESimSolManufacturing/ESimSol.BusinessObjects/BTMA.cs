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
	#region BTMA  
	public class BTMA : BusinessObject
	{	
		public BTMA()
		{
			BTMAID = 0; 
			ExportLCID = 0; 
			ExportBillID = 0; 
			BankName = ""; 
			BranchName = ""; 
			Amount = 0; 
			SupplierID = 0; 
			MasterLCNos = ""; 
			MasterLCDates = ""; 
			InvoiceDate = DateTime.Now; 
			ImportLCID = 0; 
			ImportLCNo = ""; 
			ImportLCDate = DateTime.Now; 
			GarmentsQty = ""; 
			DeliveryDate = DateTime.Now; 
			DeliveryChallanNo = ""; 
			MushakNo = ""; 
			MushakDate = DateTime.Now; 
			GatePassNo = ""; 
			GatePassDate = DateTime.Now; 
			PrintBy = 0; 
			PrintDate = DateTime.Now;
            ExportLCNo = "";
            Amount_LC = 0.0;
            CertificateNo = 0.0;
            LCDate = DateTime.MinValue;
            LCExpireDate = DateTime.MinValue;
            ExportBillNo = "";
            SupplierName = "";
            SupplierAddress = "";
            PrintByName = "";
            Amount_Bill = 0.0;
            BUID = 0;
            BTMADetails =new List<BTMADetail>();
			ErrorMessage = "";
            PartyName = "";
            PartyAddress = "";
            BUAddress = "";
            BUShortName = "";
            sSearchParam1 = "";
            sSearchParam2 = "";
            IsWaitForBTMA = false;
            BUtypes =  EnumBusinessUnitType.None;

		}

		#region Property
		public int BTMAID { get; set; }
		public int ExportLCID { get; set; }
		public int ExportBillID { get; set; }
		public string BankName { get; set; }
		public string BranchName { get; set; }
		public double Amount { get; set; }
		public int SupplierID { get; set; }
		public string MasterLCNos { get; set; }
		public string MasterLCDates { get; set; }
		public DateTime InvoiceDate { get; set; }
		public int ImportLCID { get; set; }
		public string ImportLCNo { get; set; }
		public DateTime ImportLCDate { get; set; }
		public string GarmentsQty { get; set; }
		public DateTime DeliveryDate { get; set; }
		public string DeliveryChallanNo { get; set; }
		public string MushakNo { get; set; }
		public DateTime MushakDate { get; set; }
		public string GatePassNo { get; set; }
		public DateTime GatePassDate { get; set; }
		public int PrintBy { get; set; }
		public DateTime PrintDate { get; set; }
        public double Amount_ImportLC { get; set; }
        public double CertificateNo { get; set; }
		public string ErrorMessage { get; set; }
        //EXTRA
        public string sSearchParam1 { get; set; }
        public string sSearchParam2 { get; set; }
        public bool IsWaitForBTMA { get; set; }
		#endregion 

        #region Derived From View
        public double Amount_LC { get; set; }
        public DateTime LCDate { get; set; }
        public string ExportBillNo { get; set; }
        public DateTime LCExpireDate { get; set; }
        public string SupplierName { get; set; }
        public string SupplierAddress { get; set; }
        public string PrintByName { get; set; }
        public string ExportLCNo { get; set; }
        public double Amount_Bill { get; set; }
        public int BUID { get; set; }
        public int BUType { get; set; }
        public EnumBusinessUnitType BUtypes { get; set; }
        public int CurrencyID { get; set; }
        public string CurrencyName { get; set; }
        public string Currency { get; set; }
        #endregion 

		#region Derived Property
        public List<BTMADetail> BTMADetails { get; set; }
        public string PartyName { get; set; }
        public string PartyAddress { get; set; }
         public string BUName { get; set; }
        public string BUAddress { get; set; }
        public string BUShortName { get; set; }

        public string BUTypeST
        {
            get
            {
                return this.BUtypes.ToString();
            }
        }	
        public string LCDateST
        {
            get
            {
                if (LCDate == DateTime.MinValue)
                    return LCDate.ToString("dd MMM yyyy");
                else
                    return LCDate.ToString("dd MMM yyyy");
            }
        }
        public string LCExpireDateST
        {
            get
            {
                if (LCExpireDate == DateTime.MinValue)
                    return LCExpireDate.ToString("dd MMM yyyy");
                else
                    return LCExpireDate.ToString("dd MMM yyyy");
            }
        }
        public string InvoiceDateST 
		{
			get
			{
				return InvoiceDate.ToString("dd MMM yyyy") ; 
			}
		}
		public string ImportLCDateST 
		{
			get
			{
                return ImportLCDate.ToString("dd MMM yyyy"); 
			}
		}
		public string DeliveryDateST 
		{
			get
			{
				return DeliveryDate.ToString("dd MMM yyyy") ; 
			}
		}
		public string MushakDateST 
		{
			get
			{
				return MushakDate.ToString("dd MMM yyyy") ; 
			}
		}
		public string GatePassDateST 
		{
			get
			{
				return GatePassDate.ToString("dd MMM yyyy") ; 
			}
		}
		public string PrintDateST 
		{
			get
			{
				return PrintDate.ToString("dd MMM yyyy") ; 
			}
		}
		#endregion 

		#region Functions 
		public static List<BTMA> Gets(long nUserID)
		{
			return BTMA.Service.Gets(nUserID);
		}
		public static List<BTMA> Gets(string sSQL, long nUserID)
		{
			return BTMA.Service.Gets(sSQL,nUserID);
		}
		public BTMA Get(int id, long nUserID)
		{
			return BTMA.Service.Get(id,nUserID);
		}
		public BTMA Save(long nUserID)
		{
			return BTMA.Service.Save(this,nUserID);
		}
		public  string  Delete(int id, long nUserID)
		{
			return BTMA.Service.Delete(id,nUserID);
		}
        public BTMA GetMaxCNo(long nUserID)
        {
            return BTMA.Service.GetMaxCNo(nUserID);
        }
		#endregion

		#region ServiceFactory
		internal static IBTMAService Service
		{
			get { return (IBTMAService)Services.Factory.CreateService(typeof(IBTMAService)); }
		}
		#endregion

        public BTMA Update_PrintBy(long nUserID)
        {
            return BTMA.Service.Update_PrintBy(this, nUserID);
        }
        public BTMA SaveBTMA(long nUserID)
        {
            return BTMA.Service.SaveBTMA(this, nUserID);
        }

    }
	#endregion

	#region IBTMA interface
	public interface IBTMAService 
	{
        BTMA Get(int id, Int64 nUserID);
        BTMA GetMaxCNo(Int64 nUserID); 
		List<BTMA> Gets(Int64 nUserID);
		List<BTMA> Gets( string sSQL, Int64 nUserID);
		string Delete(int id, Int64 nUserID);
        BTMA Save(BTMA oBTMA, Int64 nUserID);
        BTMA Update_PrintBy(BTMA oBTMA, Int64 nUserID);
        BTMA SaveBTMA(BTMA oBTMA, Int64 nUserID);
	}
	#endregion
}
