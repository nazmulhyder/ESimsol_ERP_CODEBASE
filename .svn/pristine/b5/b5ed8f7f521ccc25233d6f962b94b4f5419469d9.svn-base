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
	#region ExportClaimSettle  
	public class ExportClaimSettle : BusinessObject
	{
		public ExportClaimSettle()
		{
			ExportClaimSettleID = 0; 
			ExportBillID = 0;
            InoutTypeInt = 0;
            InOutType = EnumInOutType.None;
			SettleName  = ""; 
			Amount = 0; 
			CurrencyID = 0;
			ErrorMessage = "";
		}

		#region Property
		public int ExportClaimSettleID { get; set; }
		public int ExportBillID { get; set; }
		public string SettleName  { get; set; }
		public double Amount { get; set; }
        public int CurrencyID { get; set; }
        public EnumInOutType InOutType { get; set; }
        public int InoutTypeInt { get; set; }
		public string ErrorMessage { get; set; }
		#endregion 

        #region Derived Property
        public string ExportBillNo { get; set; }
        public string SettleByName { get; set; }
        public string InoutTypeSt 
        {
            get 
            {
                if (this.InOutType == EnumInOutType.Disburse)
                    return "Deduct";
                else if (this.InOutType == EnumInOutType.Receive)
                    return "Add";
                else return "-";
            } 
        }
		#endregion 

		#region Functions 
		public static List<ExportClaimSettle> Gets(long nUserID)
		{
			return ExportClaimSettle.Service.Gets(nUserID);
		}
		public static List<ExportClaimSettle> Gets(string sSQL, long nUserID)
		{
			return ExportClaimSettle.Service.Gets(sSQL,nUserID);
		}
        public static List<ExportClaimSettle> GetsByBillID(int nExportBillID, long nUserID)
        {
            return ExportClaimSettle.Service.GetsByBillID(nExportBillID, nUserID);
        }
		public ExportClaimSettle Get(int id, long nUserID)
		{
			return ExportClaimSettle.Service.Get(id,nUserID);
		}
		public ExportClaimSettle Save(long nUserID)
		{
			return ExportClaimSettle.Service.Save(this,nUserID);
		}
		public  string  Delete(int id, long nUserID)
		{
			return ExportClaimSettle.Service.Delete(id,nUserID);
		}
		#endregion

		#region ServiceFactory
		internal static IExportClaimSettleService Service
		{
			get { return (IExportClaimSettleService)Services.Factory.CreateService(typeof(IExportClaimSettleService)); }
		}
		#endregion


    }
	#endregion

	#region IExportClaimSettle interface
	public interface IExportClaimSettleService 
	{
		ExportClaimSettle Get(int id, Int64 nUserID); 
		List<ExportClaimSettle> Gets(Int64 nUserID);
        List<ExportClaimSettle> Gets(string sSQL, Int64 nUserID);
        List<ExportClaimSettle> GetsByBillID(int nExportBillID, Int64 nUserID);
		string Delete(int id, Int64 nUserID);
 		ExportClaimSettle Save(ExportClaimSettle oExportClaimSettle, Int64 nUserID);
	}
	#endregion
}
