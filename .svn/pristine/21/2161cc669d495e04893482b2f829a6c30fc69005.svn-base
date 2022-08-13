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
	#region MerchanidisingReport  
	public class MerchanidisingReport : BusinessObject
	{	
		public MerchanidisingReport()
		{
			BuyerID = 0; 
			BuyerName = ""; 
			NumberOfStyle = 0; 
			NumberOfOrderRecap = 0;  
			NumberOfCostSheet = 0; 
			NumberOfTAP = 0; 
            sParam = "";
			ErrorMessage = "";
		}

		#region Property
		public int BuyerID { get; set; }
		public string BuyerName { get; set; }
		public int NumberOfStyle { get; set; }
		public int NumberOfOrderRecap { get; set; }
		public int NumberOfCostSheet { get; set; }
		public int NumberOfTAP { get; set; }
        public string sParam{ get; set; }             
		public string ErrorMessage { get; set; }
		#endregion 

		#region Derived Property
        public string NumberOfStyleInString
        {
            get
            {
                return this.BuyerID + "~" + this.NumberOfStyle.ToString();
            }
        }
        public string NumberOfOrderRecapInString
        {
            get
            {
                return this.BuyerID + "~" + this.NumberOfOrderRecap.ToString();
            }
        }
        public string NumberOfCostSheetInString
        {
            get
            {
                return this.BuyerID + "~" + this.NumberOfCostSheet.ToString();
            }
        }
        public string NumberOfTAPInString
        {
            get
            {
                return this.BuyerID + "~" + this.NumberOfTAP.ToString();
            }
        }

		#endregion 

		#region Functions 
	
		public static List<MerchanidisingReport> Gets(string sMainSQL, string sTSSQL, string sORSQL, string sCSSQL, string sTAPSQL, long nUserID)
		{
			return MerchanidisingReport.Service.Gets(sMainSQL,sTSSQL, sORSQL, sCSSQL, sTAPSQL, nUserID);
		}
	
		#endregion

		#region ServiceFactory
		internal static IMerchanidisingReportService Service
		{
			get { return (IMerchanidisingReportService)Services.Factory.CreateService(typeof(IMerchanidisingReportService)); }
		}
		#endregion
	}
	#endregion

	#region IMerchanidisingReport interface
	public interface IMerchanidisingReportService 
	{

        List<MerchanidisingReport> Gets(string sMainSQL, string sTSSQL, string sORSQL, string sCSSQL, string sTAPSQL, long nUserID);
	
	}
	#endregion
}
