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
	#region RSDetailAdditonal  
	public class RSDetailAdditonal : BusinessObject
	{	
		public RSDetailAdditonal()
		{
			RSDetailAdditonalID = 0; 
			RouteSheetDetailID = 0; 
			SequenceNo = 0; 
			InOutType = 0; 
			Qty = 0; 
			LotID = 0;
            Note = "";
            IssuedByID = 0;
            IssuedByName = "";
            IssueDate = DateTime.Now;
            ApprovedByID = 0;
            ApprovedByName = "";
            ApproveDate = DateTime.Now;
            WUName = "";
			ErrorMessage = "";
		}

		#region Property
		public int RSDetailAdditonalID { get; set; }
		public int RouteSheetDetailID { get; set; }
        public int RouteSheetID { get; set; }
		public int SequenceNo { get; set; }
		public int InOutType { get; set; }
		public double Qty { get; set; }
		public int LotID { get; set; }
        public string Note { get; set; }
        public string LotNo { get; set; }
		public int IssuedByID { get; set; }
		public DateTime IssueDate { get; set; }
        public EnumProductNature ProductType { get; set; }

        public string IssuedByName { get; set; }
        public int ApprovedByID { get; set; }
        public string ApprovedByName { get; set; }
        public DateTime ApproveDate { get; set; }
        public string WUName { get; set; }
        
		public string ErrorMessage { get; set; }
		#endregion 

		#region Derived Property
        public string ProductTypeSt
        {
            get
            {
                return ((EnumProductNature)this.ProductType).ToString();
            }
        }
		public string IssueDateInString 
		{
			get
			{
				return IssueDate.ToString("dd MMM yyyy") ; 
			}
		}
        public string QtySt
        {
            get
            {
                return SIUnitGenerator.Mass(this.Qty);
            }
        }
        public string InOutTypeSt
        {
            get
            {
                //  Receive = 101 [In],     Disburse = 102 [out]
                if (this.SequenceNo>0 && this.InOutType == (int)EnumInOutType.Disburse) { return "Addition"; }
                else if (this.SequenceNo > 0 && this.InOutType == (int)EnumInOutType.Receive) { return "Return"; }
                else if (this.SequenceNo <= 0 && this.InOutType == (int)EnumInOutType.Disburse) { return "fresh"; }
                else return "-";
            }
        }
		#endregion 

		#region Functions 
		public static List<RSDetailAdditonal> Gets(long nUserID)
		{
			return RSDetailAdditonal.Service.Gets(nUserID);
		}
		public static List<RSDetailAdditonal> Gets(string sSQL, long nUserID)
		{
			return RSDetailAdditonal.Service.Gets(sSQL,nUserID);
		}
		public RSDetailAdditonal Get(int id, long nUserID)
		{
			return RSDetailAdditonal.Service.Get(id,nUserID);
		}
		public RSDetailAdditonal Save(long nUserID)
		{
			return RSDetailAdditonal.Service.Save(this,nUserID);
		}

		public  string  Delete(int id, long nUserID)
		{
			return RSDetailAdditonal.Service.Delete(id,nUserID);
		}
		#endregion

		#region ServiceFactory
		internal static IRSDetailAdditonalService Service
		{
			get { return (IRSDetailAdditonalService)Services.Factory.CreateService(typeof(IRSDetailAdditonalService)); }
		}
		#endregion

    }
	#endregion

	#region IRSDetailAdditonal interface
	public interface IRSDetailAdditonalService 
	{
		RSDetailAdditonal Get(int id, Int64 nUserID); 
		List<RSDetailAdditonal> Gets(Int64 nUserID);
		List<RSDetailAdditonal> Gets( string sSQL, Int64 nUserID);
		string Delete(int id, Int64 nUserID);
 		RSDetailAdditonal Save(RSDetailAdditonal oRSDetailAdditonal, Int64 nUserID);
	}
	#endregion
}
