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
	#region TermsAndCondition  
	public class TermsAndCondition : BusinessObject
	{	
		public TermsAndCondition()
		{
			TermsAndConditionID = 0; 
			ModuleID = 0; 
			TermsAndConditionText = ""; 
			ErrorMessage = "";
		}

		#region Property
		public int TermsAndConditionID { get; set; }
        public int ModuleID { get; set; }
        public string TermsAndConditionText { get; set; }
		public string ErrorMessage { get; set; }
		#endregion

		#region Derived Property
		public string ModuleName
        {
            get
            {
                return EnumObject.jGet((EnumModuleName)this.ModuleID);
            }
        }
		#endregion 

		#region Functions 
		public static List<TermsAndCondition> Gets(long nUserID)
		{
			return TermsAndCondition.Service.Gets(nUserID);
		}
        public static List<TermsAndCondition> GetsByModule(int nModuleID, long nUserID)
		{
            return TermsAndCondition.Service.GetsByModule(nModuleID, nUserID);
		}
        
		public static List<TermsAndCondition> Gets(string sSQL, long nUserID)
		{
			return TermsAndCondition.Service.Gets(sSQL,nUserID);
		}
		public TermsAndCondition Get(int id, long nUserID)
		{
			return TermsAndCondition.Service.Get(id,nUserID);
		}
		public TermsAndCondition Save(long nUserID)
		{
			return TermsAndCondition.Service.Save(this,nUserID);
		}
		public  string  Delete(int id, long nUserID)
		{
			return TermsAndCondition.Service.Delete(id,nUserID);
		}
		#endregion

		#region ServiceFactory
		internal static ITermsAndConditionService Service
		{
			get { return (ITermsAndConditionService)Services.Factory.CreateService(typeof(ITermsAndConditionService)); }
		}
		#endregion
	}
	#endregion

	#region ITermsAndCondition interface
	public interface ITermsAndConditionService 
	{
		TermsAndCondition Get(int id, Int64 nUserID); 
		List<TermsAndCondition> Gets(Int64 nUserID);
        List<TermsAndCondition> GetsByModule(int ModuleID, Int64 nUserID);        
		List<TermsAndCondition> Gets( string sSQL, Int64 nUserID);
		string Delete(int id, Int64 nUserID);
 		TermsAndCondition Save(TermsAndCondition oTermsAndCondition, Int64 nUserID);
	}
	#endregion
}
