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
	#region PAMDetail  
	public class PAMDetail : BusinessObject
	{	
		public PAMDetail()
		{
			PAMDetailID = 0; 
			PAMID = 0; 
			ColorID = 0; 
			MinQuantity = 0; 
			Quantity = 0; 
			MaxQuantity = 0;
            ConfirmWeek = "";
            DesignationWeek = "";
            ForwardWeek = "";
            WearHouseWeek = "";
            Remarks = "";
            StyleNo = "";
            StyleID = 0;
			ColorName = "";
            ColorWiseYetToRecapQty = 0;
			ErrorMessage = "";
		}

		#region Property
		public int PAMDetailID { get; set; }
		public int PAMID { get; set; }
		public int ColorID { get; set; }
		public double MinQuantity { get; set; }
		public double Quantity { get; set; }
		public double MaxQuantity { get; set; }
		public string ColorName { get; set; }
        public string ConfirmWeek { get; set; }
        public string DesignationWeek { get; set; }
        public string ForwardWeek { get; set; }
        public string WearHouseWeek { get; set; }
        public string Remarks { get; set; }
        public string StyleNo { get; set; }
        public int StyleID { get; set; }
        public double ColorWiseYetToRecapQty { get; set; }
		public string ErrorMessage { get; set; }
		#endregion 

		#region Derived Property
        public string LeadTime
        {
            get
            { 
                int nStartWeek = 0;
                int nEndWeek = 0;
                int nNeedWeek =0;
                string sLeadTime ="";
                string sConfirmYear = "";
                string sForwardYear = "";
                if (this.ConfirmWeek != null && this.ConfirmWeek.Length > 4)
                {
                    sConfirmYear = this.ConfirmWeek.Substring(0, 4);
                }
                if (this.ForwardWeek != null && this.ForwardWeek.Length > 4)
                {
                    sForwardYear = this.ForwardWeek.Substring(0, 4);
                }

                if (sConfirmYear == sForwardYear)
                {
                    if (this.ConfirmWeek != null && this.ConfirmWeek.Length == 6)
                    {
                        nStartWeek = Convert.ToInt32(this.ConfirmWeek.Substring(4, 2));
                    }
                    if (this.ConfirmWeek != null && this.ConfirmWeek.Length == 6)
                    {
                        nEndWeek = Convert.ToInt32(this.ForwardWeek.Substring(4, 2));
                    }
                    nNeedWeek = (nEndWeek - nStartWeek);
                    if (nNeedWeek <= 0)
                    {
                        nNeedWeek = 1;
                    }
                    sLeadTime = (nNeedWeek *7).ToString() + " Days";
                }
                else
                {
                    if (this.ConfirmWeek != null && this.ConfirmWeek.Length == 6)
                    {
                        nStartWeek = Convert.ToInt32(this.ConfirmWeek.Substring(4, 2));
                    }
                    nEndWeek = 53;
                    nNeedWeek = (nEndWeek - nStartWeek);

                    nStartWeek = 1;
                    if (this.ConfirmWeek != null && this.ConfirmWeek.Length == 6)
                    {
                        nEndWeek = Convert.ToInt32(this.ForwardWeek.Substring(4, 2));
                    }
                    nNeedWeek = nNeedWeek + (nEndWeek - nStartWeek + 1);
                    if (nNeedWeek <= 0)
                    {
                        nNeedWeek = 1;
                    }
                    sLeadTime = (nNeedWeek * 7).ToString() + " Days";
                }
                return sLeadTime;
            }
        }
		#endregion 

		#region Functions 
		public static List<PAMDetail> Gets(int nPAMID, long nUserID)
		{
            return PAMDetail.Service.Gets(nPAMID,nUserID);
		}
		public static List<PAMDetail> Gets(string sSQL, long nUserID)
		{
			return PAMDetail.Service.Gets(sSQL,nUserID);
		}
		public PAMDetail Get(int id, long nUserID)
		{
			return PAMDetail.Service.Get(id,nUserID);
		}
		
		#endregion

		#region ServiceFactory
		internal static IPAMDetailService Service
		{
			get { return (IPAMDetailService)Services.Factory.CreateService(typeof(IPAMDetailService)); }
		}
		#endregion
	}
	#endregion

	#region IPAMDetail interface
	public interface IPAMDetailService 
	{
		PAMDetail Get(int id, Int64 nUserID); 
		List<PAMDetail> Gets(int nPAMID, Int64 nUserID);
		List<PAMDetail> Gets( string sSQL, Int64 nUserID);
	
	}
	#endregion
}
