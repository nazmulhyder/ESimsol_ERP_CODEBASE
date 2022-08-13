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
	#region PurchaseReturn  
	public class PurchaseReturn : BusinessObject
	{	
		public PurchaseReturn()
		{
			PurchaseReturnID = 0; 
			ReturnNo = ""; 
			ReturnDate = DateTime.Now;
            RefType = EnumPurchaseReturnType.None;
			SupplierID = 0; 
			RefObjectID = 0; 
			WorkingUnitID = 0; 
			Remarks = ""; 
			ApprovedBy = 0; 
			ApprovedDate = DateTime.Now; 
			SupplierName = ""; 
			WorkingUnitName = ""; 
			RefNo = ""; 
			ApprovedByName = "";
            BUID = 0;
            RefDate = DateTime.Now;
            PurchaseReturnDetails = new List<PurchaseReturnDetail>();
			ErrorMessage = "";
		}

		#region Property
		public int PurchaseReturnID { get; set; }
		public string ReturnNo { get; set; }
		public DateTime ReturnDate { get; set; }
        public EnumPurchaseReturnType RefType { get; set; }
		public int SupplierID { get; set; }
		public int RefObjectID { get; set; }
		public int WorkingUnitID { get; set; }
		public string Remarks { get; set; }
		public int ApprovedBy { get; set; }
		public DateTime ApprovedDate { get; set; }
		public string SupplierName { get; set; }
		public string WorkingUnitName { get; set; }
		public string RefNo { get; set; }
		public string ApprovedByName { get; set; }
        public int RefTypeInt { get; set; }
        public int DisbursedBy { get; set; }
        public string DisbursedByName { get; set; }
        public int BUID { get; set; }
        public DateTime RefDate { get; set; }
		public string ErrorMessage { get; set; }
		#endregion 
        #region Derived Property
        public List<PurchaseReturnDetail> PurchaseReturnDetails { get; set; }
        public BusinessUnit BusinessUnit { get; set; }

        public string RefDateInString
        {
            get
            {
                return this.RefDate.ToString("dd MMM yyyy");
            }
        }

		public string ReturnDateInString 
		{
			get
			{
				return ReturnDate.ToString("dd MMM yyyy") ; 
			}
		}
		public string ApprovedDateInString 
		{
			get
			{
				return ApprovedDate.ToString("dd MMM yyyy") ; 
			}
		}

        public string RefTypeInString
        {
            get
            {
                return EnumObject.jGet(this.RefType); 
            }
        }
	
		#endregion 

		#region Functions 
		public static List<PurchaseReturn> Gets(long nUserID)
		{
			return PurchaseReturn.Service.Gets(nUserID);
		}
		public static List<PurchaseReturn> Gets(string sSQL, long nUserID)
		{
			return PurchaseReturn.Service.Gets(sSQL,nUserID);
		}
		public PurchaseReturn Get(int id, long nUserID)
		{
			return PurchaseReturn.Service.Get(id,nUserID);
		}
		public PurchaseReturn Save(long nUserID)
		{
			return PurchaseReturn.Service.Save(this,nUserID);
		}
        public PurchaseReturn Approve(long nUserID)
        {
            return PurchaseReturn.Service.Approve(this, nUserID);
        }
        public PurchaseReturn Disburse(long nUserID)
        {
            return PurchaseReturn.Service.Disburse(this, nUserID);
        }
        //
		public  string  Delete(int id, long nUserID)
		{
			return PurchaseReturn.Service.Delete(id,nUserID);
		}
		#endregion

		#region ServiceFactory
		internal static IPurchaseReturnService Service
		{
			get { return (IPurchaseReturnService)Services.Factory.CreateService(typeof(IPurchaseReturnService)); }
		}
		#endregion
	}
	#endregion

	#region IPurchaseReturn interface
	public interface IPurchaseReturnService 
	{
		PurchaseReturn Get(int id, Int64 nUserID); 
		List<PurchaseReturn> Gets(Int64 nUserID);
		List<PurchaseReturn> Gets( string sSQL, Int64 nUserID);
		string Delete(int id, Int64 nUserID);
 		PurchaseReturn Save(PurchaseReturn oPurchaseReturn, Int64 nUserID);
        PurchaseReturn Approve(PurchaseReturn oPurchaseReturn, Int64 nUserID);
        PurchaseReturn Disburse(PurchaseReturn oPurchaseReturn, Int64 nUserID);
	}
	#endregion
}
