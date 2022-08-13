using System;
using System.IO;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using ICS.Core;
using ICS.Core.Framework;
using ICS.Core.Utility;

namespace ESimSol.BusinessObjects
{
    #region RPT_LotTrackings

    public class RPT_LotTrackings : BusinessObject
    {
        public RPT_LotTrackings()
        {
            LotID = 0;
            LotNo = "";
            ProductCategoryName = "";
            ProductCode = "";
            ProductName = "";
            ContractorName = "";
            LCNo = "";
            MUnit = "";
            ProductID = 0;
            MUnitID = 0;
            QtyGRN = 0;
            QtyAdjIn = 0;
            QtyAdjOut = 0;
            QtyProIn = 0;// Raw
            QtyProOut = 0;//Raw
            QtyRecycle = 0;
            QtyWestage = 0;
            QtyDelivery = 0;
            QtyRSCancel = 0;
            QtyReturn = 0;
            CurrentBalance = 0;
            ParentType = 0;
            ParentLotID = 0;
            QtyProInDye = 0;
            QtyProProcess = 0;
            QtyShort = 0;
            QtySoft = 0;
            InvoiceNo = "";
            QtyProIn_Dye = 0;
            QtyRecycle_Dye = 0;
            QtyWestage_Dye = 0;
            QtyRSCancel_Dye = 0;
            QtyDelivery_Dye = 0;
            QtyReturn_Dye = 0;
            QtyProProcess_Dye = 0;
            QtyShort_Dye = 0;
            QtyFinish_Dye = 0;
            QtyFinish = 0;
            Params = "";
            Recycle_Recd = 0;
            LotDate = DateTime.Today;
        }

        #region Properties
        public int LotID { get; set; }
        public string LotNo { get; set; }
        public string ProductCategoryName { get; set; }
		public string ProductCode { get; set; }
		public string ProductName { get; set; }
		public string ContractorName { get; set; }
		public string LCNo { get; set; }
        public string InvoiceNo { get; set; }
		public string MUnit { get; set; }
		public int ProductID { get; set; }
		public int MUnitID { get; set; }
		public double QtyGRN { get; set; }
        public double QtyAdjIn { get; set; }
        public double QtyAdjOut { get; set; }
		public double QtyProIn { get; set; }
		public double QtyProOut { get; set; }
        public double QtyProInDye { get; set; }
		public double QtyRecycle { get; set; }
		public double QtyWestage { get; set; }
		public double QtyDelivery { get; set; }
        public double QtyReturn { get; set; }
        public double QtyRSCancel { get; set; }
        public double QtyProProcess { get; set; }
        public double QtyShort { get; set; }
        public double QtySoft { get; set; }
        public double QtyFinish { get; set; }
        public double QtySoft_Dye { get; set; }
        public double QtyProIn_Dye { get; set; }
        public double QtyRecycle_Dye { get; set; }
        public double QtyWestage_Dye { get; set; }
        public double QtyRSCancel_Dye { get; set; }
        public double QtyDelivery_Dye { get; set; }
        public double QtyReturn_Dye { get; set; }
        public double QtyProProcess_Dye { get; set; }
        public double QtyShort_Dye { get; set; }
        public double QtyFinish_Dye { get; set; }
        public double QtyTR { get; set; }/// Transfer Delivery To Recycle
        public double Recycle_Recd { get; set; }
        public double CurrentBalance { get; set; }
        public double CurrentBalanceDye { get; set; }
		public int ParentType { get; set; }
		public int ParentLotID { get; set; }
        public string Params { get; set; }
        public DateTime LotDate { get; set; }
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property
        public string LotDateSt
        {
            get
            {
                return this.LotDate.ToString("dd MMM yyy");
            }
        }
        public double QtyActualDelivery
        {
            get
            {
                return this.QtyDelivery - this.QtyReturn;
            }
        }
        public double QtyActualDeliveryDye
        {
            get
            {
                return this.QtyDelivery_Dye - this.QtyReturn_Dye;
            }
        }
        public double RawReced
        {
            get
            {
                return this.QtyGRN + this.QtyAdjIn-this.QtyAdjOut;
            }
        }
        public double QtyRecycleTotal
        {
            get
            {
                return this.QtyRecycle + this.QtyRSCancel;
            }
        }
        public double Total
        {
            get
            {
                return this.QtyFinish + this.QtyRecycle  + this.QtyWestage+this.QtyShort;
            }
        }
       
        public double QtyWIP
        {
            get
            {
                return this.QtyProIn - (this.QtyFinish + this.QtyRecycle + this.QtyWestage + this.QtyShort);
            }
        }
        public double TotalDye
        {
            get
            {
                return this.QtyFinish_Dye + this.QtyRecycle_Dye + this.QtyRSCancel_Dye + this.QtyWestage_Dye + this.QtyShort_Dye;
            }
        }
        public double QtyWIPDye
        {
            get
            {
                return this.QtyProIn_Dye - (this.QtyFinish_Dye + this.QtyRecycle_Dye + this.QtyRSCancel_Dye + this.QtyWestage_Dye + this.QtyShort_Dye);
            }
        }
        public double DyedYarnStock
        {
            get
            {
                return this.QtyFinish + this.QtyReturn - this.QtyDelivery - this.QtyTR;
            }
        }
        public double DyedYarnStockDye
        {
            get
            {
                return this.QtyFinish_Dye - this.QtyDelivery_Dye + this.QtyReturn_Dye;
            }
        }
        public double RawYarnStock
        {
            get
            {
                return (this.QtyGRN + this.QtyAdjIn - this.QtyAdjOut) - this.QtyProIn;
            }
        }
        #endregion

        #region Functions
        public static List<RPT_LotTrackings> Gets(string sSQL, int nReportType, int nBUID, Int64 nUserID)
        {
            return RPT_LotTrackings.Service.Gets(sSQL, nReportType, nBUID, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IRPT_LotTrackingsService Service
        {
            get { return (IRPT_LotTrackingsService)Services.Factory.CreateService(typeof(IRPT_LotTrackingsService)); }
        }
        #endregion
    }
    #endregion

    #region IRPT_LotTrackings interface

    public interface IRPT_LotTrackingsService
    {
        List<RPT_LotTrackings> Gets(string sSQL, int nReportType, int nBUID, Int64 nUserID);
    }
    #endregion
}

