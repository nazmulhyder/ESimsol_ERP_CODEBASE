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
	#region KnittingYarnReturn  
	public class KnittingYarnReturn : BusinessObject
	{	
		public KnittingYarnReturn()
		{
			KnittingYarnReturnID = 0; 
			KnittingOrderID = 0; 
			ReturnNo = ""; 
			ReturnDate = DateTime.Now; 
			PartyChallanNo = ""; 
			Remarks = ""; 
			ApprovedBy = 0; 
			ErrorMessage = "";
            FactoryName = "";
            KnittingYarnReturnDetails = new List<KnittingYarnReturnDetail>();
		}

		#region Property
		public int KnittingYarnReturnID { get; set; }
		public int KnittingOrderID { get; set; }
		public string ReturnNo { get; set; }
		public DateTime ReturnDate { get; set; }
		public string PartyChallanNo { get; set; }
		public string Remarks { get; set; }
		public int ApprovedBy { get; set; }
		public string ErrorMessage { get; set; }
		#endregion 

		#region Derived Property
        public int BUID { get; set; }
        public string ApprovedByName { get; set; }
        public string FactoryName { get; set; }
        public List<KnittingYarnReturnDetail> KnittingYarnReturnDetails { get; set; }
		public string ReturnDateInString 
		{
			get
			{
				return ReturnDate.ToString("dd MMM yyyy") ; 
			}
		}
        public string KnittingOrderNo { get; set; }
        public DateTime KnittingOrderDate { get; set; }
        public string BuyerName { get; set; }
        public string StyleNo { get; set; }
        public double KnittingOrderQty { get; set; }

        public string KnittingOrderDateInString
        {
            get
            {
                return KnittingOrderDate.ToString("dd MMM yyyy");
            }
        }
		#endregion 

		#region Functions 
		public static List<KnittingYarnReturn> Gets(long nUserID)
		{
			return KnittingYarnReturn.Service.Gets(nUserID);
		}
		public static List<KnittingYarnReturn> Gets(string sSQL, long nUserID)
		{
			return KnittingYarnReturn.Service.Gets(sSQL,nUserID);
		}
		public KnittingYarnReturn Get(int id, long nUserID)
		{
			return KnittingYarnReturn.Service.Get(id,nUserID);
		}
		public KnittingYarnReturn Save(long nUserID)
		{
			return KnittingYarnReturn.Service.Save(this,nUserID);
		}
		public  string  Delete(int id, long nUserID)
		{
			return KnittingYarnReturn.Service.Delete(id,nUserID);
		}
        public KnittingYarnReturn Approve(long nUserID)
        {
            return KnittingYarnReturn.Service.Approve(this, nUserID);
        }
		#endregion

		#region ServiceFactory
		internal static IKnittingYarnReturnService Service
		{
			get { return (IKnittingYarnReturnService)Services.Factory.CreateService(typeof(IKnittingYarnReturnService)); }
		}
		#endregion
	}
	#endregion

	#region IKnittingYarnReturn interface
	public interface IKnittingYarnReturnService 
	{
		KnittingYarnReturn Get(int id, Int64 nUserID); 
		List<KnittingYarnReturn> Gets(Int64 nUserID);
		List<KnittingYarnReturn> Gets( string sSQL, Int64 nUserID);
		string Delete(int id, Int64 nUserID);
 		KnittingYarnReturn Save(KnittingYarnReturn oKnittingYarnReturn, Int64 nUserID);
        KnittingYarnReturn Approve(KnittingYarnReturn oKnittingYarnReturn, Int64 nUserID);
	}
	#endregion
}
