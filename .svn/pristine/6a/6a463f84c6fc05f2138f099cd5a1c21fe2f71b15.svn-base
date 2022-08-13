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
	#region FAScheduleRule  
	public class FAScheduleRule : BusinessObject
	{	
		public FAScheduleRule()
		{
			FAScheduleRuleID = 0; 
			FANo = "";
            ProductID = 0;
            ProductCode = "";
            ProductName = "";
			SavageValue = 0; 
			UseFullLife = 0;
            Percentage = 0;
            FAMethod = EnumFAMethod.None;
            FAMethodInt = -1;
            DEPCalculateOn = EnumDateDisplayPart.Month;
            DEPCalculateOnInt = -1;
			IsFirst = false;
            IsValued = false; 
			CurrencyID = 0;
            CurrencyName = "";
            CurrencySymbol = "";
			MUnitID = 0;
            MUName = "";
            MUSymbol = "";
			ErrorMessage = "";
		}

		#region Property
		public int FAScheduleRuleID { get; set; }
		public string FANo { get; set; }
        public EnumFAMethod FAMethod { get; set; }
        public int FAMethodInt { get; set; }
        public EnumDateDisplayPart DEPCalculateOn { get; set; }
		public int DEPCalculateOnInt { get; set; }
		public double SavageValue { get; set; }
		public int UseFullLife { get; set; }
		public double Percentage { get; set; }
		public bool IsFirst { get; set; }
		public bool IsValued  { get; set; }
		public int CurrencyID { get; set; }
		public int MUnitID { get; set; }
        public int ProductID { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string CurrencyName { get; set; }
        public string CurrencySymbol { get; set; }
        public string MUName { get; set; }
        public string MUSymbol { get; set; }
		public string ErrorMessage { get; set; }
		#endregion 

		#region Derived Property
        public string FAMethodSt { get { return EnumObject.jGet(this.FAMethod); } }
        public string DEPCalculateOnSt { get { return EnumObject.jGet(this.DEPCalculateOn); } }
		#endregion 

		#region Functions 
		public static List<FAScheduleRule> Gets(long nUserID)
		{
			return FAScheduleRule.Service.Gets(nUserID);
		}
		public static List<FAScheduleRule> Gets(string sSQL, long nUserID)
		{
			return FAScheduleRule.Service.Gets(sSQL,nUserID);
		}
		public FAScheduleRule Get(int id, long nUserID)
		{
			return FAScheduleRule.Service.Get(id,nUserID);
		}
        public FAScheduleRule GetByProduct(int pid, long nUserID)
        {
            return FAScheduleRule.Service.GetByProduct(pid, nUserID);
        }
		public FAScheduleRule Save(long nUserID)
		{
			return FAScheduleRule.Service.Save(this,nUserID);
		}
		public  string  Delete(int id, long nUserID)
		{
			return FAScheduleRule.Service.Delete(id,nUserID);
		}
		#endregion

		#region ServiceFactory
		internal static IFAScheduleRuleService Service
		{
			get { return (IFAScheduleRuleService)Services.Factory.CreateService(typeof(IFAScheduleRuleService)); }
		}
		#endregion



       
    }
	#endregion

	#region IFAScheduleRule interface
	public interface IFAScheduleRuleService 
	{
        FAScheduleRule Get(int id, Int64 nUserID);
        FAScheduleRule GetByProduct(int pid, Int64 nUserID); 
		List<FAScheduleRule> Gets(Int64 nUserID);
		List<FAScheduleRule> Gets( string sSQL, Int64 nUserID);
		string Delete(int id, Int64 nUserID);
 		FAScheduleRule Save(FAScheduleRule oFAScheduleRule, Int64 nUserID);
	}
	#endregion
}
