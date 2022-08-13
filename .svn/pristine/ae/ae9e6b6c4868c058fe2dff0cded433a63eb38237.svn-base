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
	#region FNRequisition  
	public class FNRequisition : BusinessObject
	{	
		public FNRequisition()
		{
			FNRID = 0;
            TreatmentID = EnumFNTreatment.None;
            ShiftID = EnumShift.None; 
			FNExODetailID = 0;
            Shade = EnumShade.NA; 
			WorkingUnitID = 0;
            WorkingUnitReceiveID = 0; 
			Remarks = "";
            ApproveBy = 0;
			ApproveDate = DateTime.Now; 
			DisburseBy = 0; 
			DisburseDate = DateTime.Now; 
			ReceiveBy = 0; 
			ReceiveDate = DateTime.Now;
            RequestDate = DateTime.Now;
			WorkingUnitName = ""; 
			ApproveByName = ""; 
			DisburseByName = "";
            MachineCode = "";
			ReceiveByName = "";
            MachineID = 0;
            MachineName = "";
			IssueByName = ""; 
			FNExONo = ""; 
			BuyerName = "";
            sParam = "";
			Qty = 0;
            ColorName = "";
            LDNo = "";
            FNRNo = "";
            IsRequisitionOpen = false;
            FNRequisitionDetails = new List<FNRequisitionDetail>();
			ErrorMessage = "";
		}

		#region Property
		public int FNRID { get; set; }
		public EnumFNTreatment TreatmentID { get; set; }
		public EnumShift ShiftID { get; set; }
		public int FNExODetailID { get; set; }
		public EnumShade Shade { get; set; }
		public int WorkingUnitID { get; set; }
        public int WorkingUnitReceiveID { get; set; }
		public string Remarks { get; set; }
		public int ApproveBy { get; set; }
        public int MachineID { get; set; }
        public string MachineName { get; set; }
		public DateTime ApproveDate { get; set; }
		public int DisburseBy { get; set; }
		public DateTime DisburseDate { get; set; }
		public int ReceiveBy { get; set; }
        public string MachineCode { get; set; }
		public DateTime ReceiveDate { get; set; }
		public string WorkingUnitName { get; set; }
		public string ApproveByName { get; set; }
		public string DisburseByName { get; set; }
		public string ReceiveByName { get; set; }
		public string IssueByName { get; set; }
		public string FNExONo { get; set; }
        public string ExeNo { get; set; }
		public string BuyerName { get; set; }
		public double Qty { get; set; }
        public string Construction { get; set; }
        public string ScNoFull { get { return this.FNExONo; } }
        public DateTime RequestDate { get; set; }
        public int RequestBy { get; set; }
        public string ColorName { get; set; }
        public string LDNo { get; set; }
        public string FNRNo { get; set; }
        public bool IsRequisitionOpen { get; set; }
		public string ErrorMessage { get; set; }
		#endregion 

		#region Derived Property
        public string sParam { get; set; }
        public List<FNRequisitionDetail> FNRequisitionDetails { get; set; }
        public double QtyInMeter
        {
            get
            {
                if (this.Qty > 0)
                {
                    return Global.GetMeter(this.Qty,2);
                }
                else
                {
                    return 0;
                }
            }
        }
        public string TreatmentInSt
        {
            get
            {
                return EnumObject.jGet(this.TreatmentID);
            }
        }
        public string ShiftInSt
        {
            get
            {
                return EnumObject.jGet(this.ShiftID);
            }
        }
        public string ShadeInString
        {
            get
            {
                return EnumObject.jGet(this.Shade);
            }
        }
        public string IsRequisitionOpenSt
        {
            get
            {
                if (this.IsRequisitionOpen)
                {
                    return "Open";
                }
                else
                {
                    return "Order Wise";
                }
            }
        }
        public string QtySt
        {
            get
            {
                if(this.Qty>0)
                {
                    return Global.MillionFormat(this.Qty);
                }
                else
                {
                    return "";
                }
            }
        }
		public string ApproveDateInString 
		{
			get
			{
                if (this.ApproveDate == DateTime.MinValue)
                {
                    return "-";
                }
                else
                {
                    return ApproveDate.ToString("dd MMM yyyy");
                }
			}
		}
		public string DisburseDateInString 
		{
			get
			{
                if (this.DisburseDate == DateTime.MinValue)
                {
                    return "-";
                }
                else
                {
                    return DisburseDate.ToString("dd MMM yyyy");
                }
			}
		}
		public string ReceiveDateInString 
		{
			get
			{
                if (this.ReceiveDate == DateTime.MinValue)
                {
                    return "-";
                }
                else
                {
                    return ReceiveDate.ToString("dd MMM yyyy");
                }
			}
		}
        public string RequestDateInString 
		{
			get
			{
                return RequestDate.ToString("dd MMM yyyy"); 
			}
		}
		#endregion 

		#region Functions 
		public static List<FNRequisition> Gets(long nUserID)
		{
			return FNRequisition.Service.Gets(nUserID);
		}
		public static List<FNRequisition> Gets(string sSQL, long nUserID)
		{
			return FNRequisition.Service.Gets(sSQL,nUserID);
		}
		public FNRequisition Get(int id, long nUserID)
		{
			return FNRequisition.Service.Get(id,nUserID);
		}
		public FNRequisition Save(long nUserID)
		{
			return FNRequisition.Service.Save(this,nUserID);
		}
        public FNRequisition Approval(bool bIsApprove, long nUserID)
        {
            return FNRequisition.Service.Approval(this, bIsApprove, nUserID);
        }

        public FNRequisition Received(long nUserID)
        {
            return FNRequisition.Service.Received(this, nUserID);
        }
        public FNRequisition Disburse(long nUserID)
        {
            return FNRequisition.Service.Disburse(this, nUserID);
        }
		public  string  Delete(int id, long nUserID)
		{
			return FNRequisition.Service.Delete(id,nUserID);
		}
		#endregion

		#region ServiceFactory
		internal static IFNRequisitionService Service
		{
			get { return (IFNRequisitionService)Services.Factory.CreateService(typeof(IFNRequisitionService)); }
		}
		#endregion


    }
	#endregion

	#region IFNRequisition interface
	public interface IFNRequisitionService 
	{
		FNRequisition Get(int id, Int64 nUserID); 
		List<FNRequisition> Gets(Int64 nUserID);
		List<FNRequisition> Gets( string sSQL, Int64 nUserID);
		string Delete(int id, Int64 nUserID);
 		FNRequisition Save(FNRequisition oFNRequisition, Int64 nUserID);
        FNRequisition Approval(FNRequisition oFNRequisition, bool bIsApprove, Int64 nUserID);
        FNRequisition Received(FNRequisition oFNRequisition, Int64 nUserID);
        FNRequisition Disburse(FNRequisition oFNRequisition, Int64 nUserID);
        
	}
	#endregion
}
