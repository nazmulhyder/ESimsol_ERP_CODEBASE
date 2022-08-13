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
	#region StyleBudgetRecap  
	public class StyleBudgetRecap : BusinessObject
	{	
		public StyleBudgetRecap()
		{
			StyleBudgetRecapID = 0; 
			StyleBudgetID = 0; 
			RefID = 0; 
			Note = ""; 
			RefNo = ""; 
			UnitPrice = 0; 
			Quantity = 0; 
			Amount = 0;
            UnitSymbol = "";
            SessionName = "";
            RefType = EnumStyleBudgetRecapType.None; 
            OrderConfimationDate = DateTime.Now;
			ErrorMessage = "";
		}

		#region Property
		public int StyleBudgetRecapID { get; set; }
		public int StyleBudgetID { get; set; }
		public int RefID { get; set; }
        public EnumStyleBudgetRecapType RefType{ get; set; }
		public string Note { get; set; }
		public string RefNo { get; set; }
		public double UnitPrice { get; set; }
		public double Quantity { get; set; }
		public double Amount { get; set; }
        public string UnitSymbol { get; set; }
        public string SessionName { get; set; }

        public DateTime OrderConfimationDate { get; set; }
		public string ErrorMessage { get; set; }
		#endregion 

		#region Derived Property
        public string RefTypeSt
        {
            get
            {
                return EnumObject.jGet(this.RefType);
            }
        }
        public string UnitPriceSt
        {
            get
            {
                return Global.MillionFormat(this.UnitPrice);
            }
        }
        public string OrderConfimationDateSt
        {
            get
            {
                return this.OrderConfimationDate.ToString("dd MMM yyyy");
                
            }
        }
		#endregion 

		#region Functions 
		public static List<StyleBudgetRecap> Gets(int nCSID, long nUserID)
		{
            return StyleBudgetRecap.Service.Gets(nCSID,nUserID);
		}
		public static List<StyleBudgetRecap> Gets(string sSQL, long nUserID)
		{
			return StyleBudgetRecap.Service.Gets(sSQL,nUserID);
		}
		public StyleBudgetRecap Get(int id, long nUserID)
		{
			return StyleBudgetRecap.Service.Get(id,nUserID);
		}
	
		#endregion

		#region ServiceFactory
		internal static IStyleBudgetRecapService Service
		{
			get { return (IStyleBudgetRecapService)Services.Factory.CreateService(typeof(IStyleBudgetRecapService)); }
		}
		#endregion
	}
	#endregion

	#region IStyleBudgetRecap interface
	public interface IStyleBudgetRecapService 
	{
		StyleBudgetRecap Get(int id, Int64 nUserID); 
		List<StyleBudgetRecap> Gets(int nCSID, Int64 nUserID);
		List<StyleBudgetRecap> Gets( string sSQL, Int64 nUserID);
		
	}
	#endregion
}
