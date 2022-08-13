using System;
using System.IO;
using ICS.Core;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using ICS.Core.Framework;
using ICS.Core.Utility;

namespace ESimSol.BusinessObjects
{

    #region FactoryWiseLCTransferReport
    
    public class FactoryWiseLCTransferReport : BusinessObject
    {
        public FactoryWiseLCTransferReport()
        {
            LCTransferID =0;
		    ProductionFactoryID =0;
			FactoryName = "";
			LCTransferNo = "";
            TransferDate = DateTime.Now;
			MasterLCID =0;
			MasterLCNo = "";
			BuyerID =0;
			BuyerName = "";
			StyleNo = "";
			RecapNo = "";
			Qty =0;
			Amount =0;
			Commission =0;
			CommissionPerPcs =0;
			CommissionAmount = 0;
            ErrorMessage = "";

        }

        #region Properties
         
        public int LCTransferID  { get; set; }
         
        public int ProductionFactoryID  { get; set; }
         
        public string FactoryName = "";
         
        public string LCTransferNo = "";
         
        public DateTime TransferDate {get;set;}
         
        public int MasterLCID {get;set;}
         
        public string MasterLCNo = "";
         
        public int BuyerID  { get; set; }
         
        public string BuyerName = "";
         
        public string StyleNo = "";
         
        public string RecapNo = "";
         
        public double Qty  { get; set; }
         
        public double Amount  { get; set; }
         
        public double Commission  { get; set; }
         
        public double CommissionPerPcs  { get; set; }
         
        public double CommissionAmount = 0;
         
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property
        public string TransferDateInstring
        {
            get
            {
                return this.TransferDate.ToString("dd MMM yyyy");
            }
        }
        public List<FactoryWiseLCTransferReport> FactoryWiseLCTransferReports { get; set; }
        public Company Company { get; set; }
        #endregion

        #region Functions

        public static List<FactoryWiseLCTransferReport> Gets(string sIDs, long nUserID)
        {
            return FactoryWiseLCTransferReport.Service.Gets(sIDs, nUserID);
        }

      

        #endregion

        #region ServiceFactory
        internal static IFactoryWiseLCTransferReportService Service
        {
            get { return (IFactoryWiseLCTransferReportService)Services.Factory.CreateService(typeof(IFactoryWiseLCTransferReportService)); }
        }

        #endregion
    }
    #endregion

    #region Report Study
    public class FactoryWiseLCTransferReportList : List<FactoryWiseLCTransferReport>
    {
        public string ImageUrl { get; set; }
    }
    #endregion

    #region IFactoryWiseLCTransferReport interface
     
    public interface IFactoryWiseLCTransferReportService
    {
         
        List<FactoryWiseLCTransferReport> Gets(string sIDs, Int64 nUserID);
    }
    #endregion
    
  
}
