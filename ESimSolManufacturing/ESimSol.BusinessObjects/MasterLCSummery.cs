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

    #region MasterLCSummery
    
    public class MasterLCSummery : BusinessObject
    {
        public MasterLCSummery()
        {
            MasterLCID = 0;
            MasterLCDate = DateTime.Now;
            MasterLCNo = "";
            BuyerName = "";
            ApplicantBankName = "";
            AdviceBankName = "";
            MasterLCValue = 0;
            ExportPIQty = 0;
            ExportPIValue = 0;
            NumberOfStyle = 0;
            NumberOfLCTransfer = 0;
            NumberOfSaleContact = 0;
            ProductionQty = 0;
            ExportQty = 0;
            ComercialInvoiceQty = 0;
            YarnValue = 0;
            AccessoriesValue = 0;
            ImportPIValue = 0;
            B2BLCValue = 0;
            CMValue = 0;
            EndosmentCommission = 0;
            B2BCommission = 0;
            TotalCommission = 0;
            CommissionRealise = 0;
            ErrorMessage = "";

        }

        #region Properties
         
        public int MasterLCID { get; set; }
         
		public DateTime MasterLCDate  { get; set; }
         
		public string MasterLCNo { get; set; }
         
		public string BuyerName { get; set; }
         
		public string ApplicantBankName { get; set; }
         
		public string AdviceBankName { get; set; }
         
		public double MasterLCValue { get; set; }
         
		public double ExportPIQty { get; set; }
         
		public double ExportPIValue { get; set; }
         
		public int NumberOfStyle { get; set; }
         
		public int NumberOfLCTransfer { get; set; }
         
        public int NumberOfSaleContact { get; set; }
         
		public double ProductionQty { get; set; }
         
		public double ExportQty { get; set; }
         
		public double ComercialInvoiceQty { get; set; }
         
		public double YarnValue { get; set; }
         
		public double AccessoriesValue { get; set; }
         
		public double ImportPIValue { get; set; }
         
		public double B2BLCValue { get; set; }
         
		public double CMValue { get; set; }
         
		public double EndosmentCommission { get; set; }
         
		public double B2BCommission { get; set; }
         
		public double TotalCommission { get; set; }
         
        public double CommissionRealise { get; set; }
         
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property
  
        public List<MasterLCSummery> MasterLCSummerys { get; set; }
        public Company Company { get; set; }

        public string MasterLCDateInString
        {
            get
            {
                return this.MasterLCDate.ToString("dd MMM yyyy");
            }
        }

        public string NumberOfStyleInString
        {
            get
            {
                return this.MasterLCID+"~"+this.NumberOfStyle;
            }
        }
        public string NumberOfLCTransferInString
        {
            get
            {
                return this.MasterLCID + "~" + this.NumberOfLCTransfer;
            }
        }

        public string NumberOfSaleContactInString
        {
            get
            {
                return this.MasterLCID + "~" + this.NumberOfSaleContact;
            }
        }
        
        #endregion

        #region Functions
        public static List<MasterLCSummery> Gets(string sIDs, long nUserID)
        {
            return MasterLCSummery.Service.Gets(sIDs, nUserID);
        }

     

        #endregion

        #region ServiceFactory
        internal static IMasterLCSummeryService Service
        {
            get { return (IMasterLCSummeryService)Services.Factory.CreateService(typeof(IMasterLCSummeryService)); }
        }

        #endregion
    }
    #endregion

    #region Report Study
    public class MasterLCSummeryList : List<MasterLCSummery>
    {
        public string ImageUrl { get; set; }
    }
    #endregion

    #region IMasterLCSummery interface
     
    public interface IMasterLCSummeryService
    {
         
        List<MasterLCSummery> Gets(string sIDs, Int64 nUserID);

    }
    #endregion
    
  
}
