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
	#region StyleBudgetCM  
	public class StyleBudgetCM : BusinessObject
	{	
		public StyleBudgetCM()
		{
			StyleBudgetCMID = 0; 
			StyleBudgetID = 0; 
			NumberOfMachine = 0; 
			MachineCost = 0; 
			ProductionPerDay = 0; 
			BufferDays = 2; //detault
			TotalRequiredDays = 0; 
			CMAdditionalPerent = 15; //default
            CSQty = 0;
            CMPart = "";
			ErrorMessage = "";
		}

		#region Property
		public int StyleBudgetCMID { get; set; }
		public int StyleBudgetID { get; set; }
		public int NumberOfMachine { get; set; }
		public double MachineCost { get; set; }
		public double ProductionPerDay { get; set; }
		public int BufferDays { get; set; }
		public int TotalRequiredDays { get; set; }
		public double CMAdditionalPerent { get; set; }
        public string CMPart { get; set; }
		public string ErrorMessage { get; set; }
		#endregion 

		#region Derived Property
        public double CSQty { get; set; }
        public double CMPerPc
        {
            get
            {
                return this.NumberOfMachine > 0 && this.MachineCost > 0 && this.TotalRequiredDays > 0 ? Math.Round(((this.NumberOfMachine * this.MachineCost * this.TotalRequiredDays) / this.CSQty), 2) : 0;
            }
        }
        public double CMWithAddition
        {
            get
            {
                return Math.Round((this.CMPerPc + (this.CMPerPc * (this.CMAdditionalPerent / 100))), 2);//consider%
            }
        }
        public double CMPerDozen
        {
            get
            {
                return this.CMWithAddition * 12;
            }
        }
        public string CSQtySt
        {
            get
            {
                return Global.MillionFormat(this.CSQty,0);
            }
        }
		#endregion 

		#region Functions 
		public static List<StyleBudgetCM> Gets(int CSID, long nUserID)
		{
			return StyleBudgetCM.Service.Gets(CSID, nUserID);
		}

        public static List<StyleBudgetCM> GetsByLog(int CSLogID, long nUserID)
		{
            return StyleBudgetCM.Service.GetsByLog(CSLogID, nUserID);
		}
		public static List<StyleBudgetCM> Gets(string sSQL, long nUserID)
		{
			return StyleBudgetCM.Service.Gets(sSQL,nUserID);
		}
		public StyleBudgetCM Get(int id, long nUserID)
		{
			return StyleBudgetCM.Service.Get(id,nUserID);
		}
        
		
		#endregion

		#region ServiceFactory
		internal static IStyleBudgetCMService Service
		{
			get { return (IStyleBudgetCMService)Services.Factory.CreateService(typeof(IStyleBudgetCMService)); }
		}
		#endregion
	}
	#endregion

	#region IStyleBudgetCM interface
	public interface IStyleBudgetCMService 
	{
		StyleBudgetCM Get(int id, Int64 nUserID); 
		List<StyleBudgetCM> Gets(int CSID, Int64 nUserID);
        List<StyleBudgetCM> GetsByLog(int CSLogID, Int64 nUserID);
        
		List<StyleBudgetCM> Gets( string sSQL, Int64 nUserID);
	
	}
	#endregion
}
