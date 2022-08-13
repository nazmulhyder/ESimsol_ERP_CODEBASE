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
	#region S2SLotTransfer  
	public class S2SLotTransfer : BusinessObject
	{	
		public S2SLotTransfer()
		{
			S2SLotTransferID = 0; 
			StoreID = 0; 
			TransferDate = DateTime.Now; 
			IssueStyleID = 0; 
			ReceiveStyleID = 0; 
			IssueLotID = 0; 
			ReceiveLotID = 0; 
			TransferQty = 0; 
			AuthorizedByID = 0; 
			IssueStyleNo = ""; 
			ReceiveStyleNo = ""; 
			StoreName = ""; 
			IssueLotNo = "";
            ReceiveLotNo = DateTime.Now.ToString("yyyyMMddhhmmss"); 
			ProductCode = ""; 
			ProductName = "";
            AuthorizedByName = "";
            IssueLotBalance = 0;
            BUID = 0;
            ProductID = 0;
            Remarks = "";
			ErrorMessage = "";
		}

		#region Property
		public int S2SLotTransferID { get; set; }
		public int StoreID { get; set; }
		public DateTime TransferDate { get; set; }
		public int IssueStyleID { get; set; }
		public int ReceiveStyleID { get; set; }
		public int IssueLotID { get; set; }
		public int ReceiveLotID { get; set; }
		public double TransferQty { get; set; }
		public int AuthorizedByID { get; set; }
		public string IssueStyleNo { get; set; }
		public string ReceiveStyleNo { get; set; }
		public string StoreName { get; set; }
		public string IssueLotNo { get; set; }
		public string ReceiveLotNo { get; set; }
		public string ProductCode { get; set; }
		public string ProductName { get; set; }
        public string AuthorizedByName { get; set; }
        public double IssueLotBalance { get; set; }
        public int BUID { get; set; }
        public string Remarks { get; set; }
        public int ProductID { get; set; }
		public string ErrorMessage { get; set; }
		#endregion 

		#region Derived Property
        public string IssueLotBalanceSt
        {
            get
            {
                return Global.MillionFormat(this.IssueLotBalance);
            }
        }
		public string TransferDateInString 
		{
			get
			{
				return TransferDate.ToString("dd MMM yyyy") ; 
			}
		}
		#endregion 

		#region Functions 
		public static List<S2SLotTransfer> Gets(long nUserID)
		{
			return S2SLotTransfer.Service.Gets(nUserID);
		}
		public static List<S2SLotTransfer> Gets(string sSQL, long nUserID)
		{
			return S2SLotTransfer.Service.Gets(sSQL,nUserID);
		}
		public S2SLotTransfer Get(int id, long nUserID)
		{
			return S2SLotTransfer.Service.Get(id,nUserID);
		}
		public S2SLotTransfer Save(long nUserID)
		{
			return S2SLotTransfer.Service.Save(this,nUserID);
		}
        public S2SLotTransfer Commit(long nUserID)
		{
            return S2SLotTransfer.Service.Commit(this, nUserID);
		}
        
		public  string  Delete(int id, long nUserID)
		{
			return S2SLotTransfer.Service.Delete(id,nUserID);
		}
		#endregion

		#region ServiceFactory
		internal static IS2SLotTransferService Service
		{
			get { return (IS2SLotTransferService)Services.Factory.CreateService(typeof(IS2SLotTransferService)); }
		}
		#endregion
	}
	#endregion

	#region IS2SLotTransfer interface
	public interface IS2SLotTransferService 
	{
		S2SLotTransfer Get(int id, Int64 nUserID); 
		List<S2SLotTransfer> Gets(Int64 nUserID);
		List<S2SLotTransfer> Gets( string sSQL, Int64 nUserID);
		string Delete(int id, Int64 nUserID);
 		S2SLotTransfer Save(S2SLotTransfer oS2SLotTransfer, Int64 nUserID);
        S2SLotTransfer Commit(S2SLotTransfer oS2SLotTransfer, Int64 nUserID);
        
	}
	#endregion
}
