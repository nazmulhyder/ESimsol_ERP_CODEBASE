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
	#region FabricBatchQCTransfer  
	public class FabricBatchQCTransfer : BusinessObject
	{	
		public FabricBatchQCTransfer()
		{
			FBQCTransferID = 0; 
			TransferNo = ""; 
			IssueDate = DateTime.Now; 
			IssueBy = 0; 
			ErrorMessage = "";
            TSIssueName = string.Empty;
            RoleCount = string.Empty;
            BatchNo = "";
		}

		#region Property
		public int FBQCTransferID { get; set; }
		public string TransferNo { get; set; }
		public DateTime IssueDate { get; set; }
		public int IssueBy { get; set; }
		public string ErrorMessage { get; set; }
        public string RoleCount { get; set; }
		#endregion 

		#region Derived Property
		public string IssueDateInString 
		{
			get
			{
				return IssueDate.ToString("dd MMM yyyy") ; 
			}
		}
        public string TransferNoWithRoleCount { get { return this.TransferNo + " - " + this.RoleCount; } }
        public string TSIssueName { get; set; }
        public string BatchNo { get; set; }
		#endregion 

		#region Functions 
		public static List<FabricBatchQCTransfer> Gets(long nUserID)
		{
			return FabricBatchQCTransfer.Service.Gets(nUserID);
		}
		public static List<FabricBatchQCTransfer> Gets(string sSQL, long nUserID)
		{
			return FabricBatchQCTransfer.Service.Gets(sSQL,nUserID);
		}
		public static FabricBatchQCTransfer Get(int id, long nUserID)
		{
			return FabricBatchQCTransfer.Service.Get(id,nUserID);
		}
		public FabricBatchQCTransfer Save(long nUserID)
		{
			return FabricBatchQCTransfer.Service.Save(this,nUserID);
		}
		public  string  Delete(int id, long nUserID)
		{
			return FabricBatchQCTransfer.Service.Delete(id,nUserID);
		}
		#endregion

		#region ServiceFactory
		internal static IFabricBatchQCTransferService Service
		{
			get { return (IFabricBatchQCTransferService)Services.Factory.CreateService(typeof(IFabricBatchQCTransferService)); }
		}
		#endregion
	}
	#endregion

	#region IFabricBatchQCTransfer interface
	public interface IFabricBatchQCTransferService 
	{
		FabricBatchQCTransfer Get(int id, Int64 nUserID); 
		List<FabricBatchQCTransfer> Gets(Int64 nUserID);
		List<FabricBatchQCTransfer> Gets( string sSQL, Int64 nUserID);
		string Delete(int id, Int64 nUserID);
 		FabricBatchQCTransfer Save(FabricBatchQCTransfer oFabricBatchQCTransfer, Int64 nUserID);
	}
	#endregion
}
