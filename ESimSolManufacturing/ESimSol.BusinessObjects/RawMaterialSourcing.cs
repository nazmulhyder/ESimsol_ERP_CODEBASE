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
	#region RawMaterialSourcing  
	public class RawMaterialSourcing : BusinessObject
	{	
		public RawMaterialSourcing()
		{
			ProductID = 0; 
			ProductCode = ""; 
			ProductName = ""; 
			OrderRecapID = 0; 
			ColorName = ""; 
			SizeName = ""; 
			ReqQty = 0; 
			UnitName = ""; 
			Balance = 0; 
			ContractorID = 0; 
			SupplierName = "";
            ConsumptionQty = 0;
			ErrorMessage = "";
		}

		#region Property
		public int ProductID { get; set; }
		public string ProductCode { get; set; }
		public string ProductName { get; set; }
		public int OrderRecapID { get; set; }
		public string ColorName { get; set; }
		public string SizeName { get; set; }
		public double ReqQty { get; set; }
		public string UnitName { get; set; }
		public double Balance { get; set; }
		public int ContractorID { get; set; }
		public string SupplierName { get; set; }
        public double ConsumptionQty { get; set; }
		public string ErrorMessage { get; set; }
		#endregion 

		#region Derived Property
        public string ReqQtySt
        {
            get
            {
                if (this.ReqQty > 0)
                {
                    return Global.MillionFormatActualDigit(this.ReqQty)+" "+this.UnitName;
                }
                else
                {
                    return "";
                }
            }
        }
        public string BalanceSt
        {
            get
            {
                if (this.Balance > 0)
                {
                    return Global.MillionFormatActualDigit(this.Balance) + " " + this.UnitName;
                }
                else
                {
                    return "";
                }
            }
        }
		#endregion 

		#region Functions 
		public static List<RawMaterialSourcing> Gets(int OrderRecapID, long nUserID)
		{
            return RawMaterialSourcing.Service.Gets(OrderRecapID, nUserID);
		}
		public static List<RawMaterialSourcing> Gets(string sSQL, long nUserID)
		{
			return RawMaterialSourcing.Service.Gets(sSQL,nUserID);
		}
	
		#endregion

		#region ServiceFactory
		internal static IRawMaterialSourcingService Service
		{
			get { return (IRawMaterialSourcingService)Services.Factory.CreateService(typeof(IRawMaterialSourcingService)); }
		}
		#endregion
	}
	#endregion

	#region IRawMaterialSourcing interface
	public interface IRawMaterialSourcingService 
	{
		List<RawMaterialSourcing> Gets(int OrderRecapID, Int64 nUserID);
		List<RawMaterialSourcing> Gets( string sSQL, Int64 nUserID);	
	}
	#endregion
}
