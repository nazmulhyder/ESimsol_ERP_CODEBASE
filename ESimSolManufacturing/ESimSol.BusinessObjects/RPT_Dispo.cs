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
    #region RPT_Dispo

    public class RPT_Dispo : BusinessObject
    {
        public RPT_Dispo()
        {
            ExeNo = "";
            ExeDate = DateTime.Today;
            DODate = DateTime.Today;
            ReviseDate = DateTime.Today;
            SCNoFull = "";
            Qty_Dispo = 0;
            Qty_Order = 0;
            BuyerName = "";
            ContractorName = "";
            FinishTypeName = "";
            FinishDesign = "";
            ProcessTypeName = "";
            ProcessType = 0;
            FabricWeave = 0;
            FabricWeaveName = "";
            Construction = "";
            YarnType = "";
            Req_GreyYarn = 0;
            GreyYarnReqWarp = 0;
            DyedYarnReqWarp = 0;
            YarnPriceWarp = 0;
            GreyYarnReqWeft = 0;
            DyedYarnReqWeft = 0;
            YarnPriceWeft = 0;
            ReqDyedYarn = 0;
            ReqDyedYarnPro = 0;
            ReqGreyFabrics = 0;
            GreyProductionActual = 0;
            ReqFinishedFabrics = 0;
            ActualFinishfabrics = 0;
            PIRate = 0;
            YDChemicalValue = 0;
            YDDyesValue = 0;
            PrintingDCValue = 0;
            SizingChemicalVal = 0;
            FinishingChemicalVal = 0;
            DOQty = 0;
            ShortExcessPro = 0;
            SampleQty = 0;
            Qty = 0;
            DCNo = "";
            DONo = "";
            DispoRef = "";
            ContractorID = 0;
            BCPID = 0;
            MKTPersonID = 0;
            FabricID = 0;
            MUID = 0;
            UnitPrice = 0;
            ProductName = "";
            FabricNo = "";
            ColorInfo = "";
            FabricWidth = "";
            BuyerReference = "";
            StyleNo = "";
            PINo = "";
            LCNo = "";
            ProductID = 0;
            FinishType = 0;
            BuyerID = 0;
            FSCID = 0;
            FEOSID = 0;
            DyeingOrderID = 0;
            MKTPersonName = "";
            Note = "";
            MUnit = "";
            FabricTypeName = "";
            Params = "";
            ErrorMessage = "";
            IsPrint = false;
            WarpLengthRecd = 0;

            /*** FOR DISPO WISE REPORT  ***/
            Qty_Dye = 0.00;
            Qty_Greige= 0.00;
		    Qty_SRS= 0.00;
		    Qty_SRM = 0.00;
            FSCDetailID = 0;
            Qty_FWP = 0;
            ExesQty =0;
            ExesCount = 0;
            SWQty = 0;
            WYReqWarp = 0;
            SWQty = 0;
            RequiredWarpLengthLB = 0;
            Qty_FWP_LB = 0;
            YDYarnValue = 0;
        }

        /*
		Qty_Dye decimal(30,17) --Qty-Yds ****
        Qty_Greige****
        Qty_Greige decimal(30,17) 
		Qty_SRS decimal(30,17) 
		Qty_SRM decimal(30,17) 
        FSCDetailID int
         */


        #region Properties
        public string ExeNo { get; set; }
        public DateTime ExeDate { get; set; }
        public DateTime DODate { get; set; }
        public DateTime ReviseDate { get; set; }
        public string SCNoFull { get; set; }
        public double Qty_Dispo { get; set; }
        public double Qty_Order { get; set; }
        public string BuyerName { get; set; }
        public string ContractorName { get; set; }
        public string FinishTypeName { get; set; }
        public string FinishDesign { get; set; }
        public string ProcessTypeName { get; set; }
        public int ProcessType { get; set; }
        public int FabricWeave { get; set; }
        public string FabricWeaveName { get; set; }
        public string Construction { get; set; }
        public string YarnType { get; set; }
        public double Req_GreyYarn { get; set; }
        public double GreyYarnReqWarp { get; set; }
        public double GreyYarnReqWeft { get; set; }
        public double DyedYarnReqWarp { get; set; }
        public double DyedYarnReqWeft { get; set; }
        public double ValueGreyYarn { get; set; }
        public double YarnPriceWarp { get; set; }
     
     
        public double YarnPriceWeft { get; set; }
        public double ReqDyedYarn { get; set; }
        public double ReqDyedYarnPro { get; set; }
        public double ReqGreyFabrics { get; set; }
        public double GreyProductionActual { get; set; }
        public double ReqFinishedFabrics { get; set; }
        public double ActualFinishfabrics { get; set; }
        public double PIRate { get; set; }
        public double YDChemicalValue { get; set; }
        public double YDDyesValue { get; set; }
        public double YDYarnValue { get; set; }
        public double PrintingDCValue { get; set; }
        public double SizingChemicalVal { get; set; }
        public double FinishingChemicalVal { get; set; }
        public EnumDispoProType ProdtionType { get; set; }
        public double DOQty { get; set; }
        public double ShortExcessPro { get; set; }
        public bool IsPrint { get; set; }
        public double SampleQty { get; set; }
        public double Qty { get; set; }
        public double DCQty { get; set; }
        public string DCNo { get; set; }
        public string DONo { get; set; }
        public string DispoRef { get; set; }
        public int ContractorID { get; set; }
        public int BCPID { get; set; }
        public int MKTPersonID { get; set; }
        public string MKTPersonName { get; set; }
        public int FabricID { get; set; }
        public int MUID { get; set; }
        public double UnitPrice { get; set; }
        public string ProductName { get; set; }
        public string WarpCount { get; set; }
        public string WeftCount { get; set; }
        public double TotalEnds { get; set; }
        public string FabricNo { get; set; }
        public string ColorInfo { get; set; }
        public string FabricWidth { get; set; }
        public string BuyerReference { get; set; }
        public string StyleNo { get; set; }
        public string PINo { get; set; }
        public string LCNo { get; set; }
        public int ProductID { get; set; }
        public int FinishType { get; set; }
        public int BuyerID { get; set; }
        public int FSCDID { get; set; }
        public int FSCID { get; set; }
        public int FEOSID { get; set; }
        public int DyeingOrderID { get; set; }
        public string MUnit { get; set; }
        public string Params { get; set; }
        public string FabricTypeName { get; set; }
        public string Note { get; set; }
        public int ColorWarp { get; set; }
        public int ColorWeft { get; set; }
        public double ReedNo { get; set; }
        public double Dent { get; set; }
        public double REEDWidth { get; set; }
        public double WarpLength { get; set; }
        public double RequiredWarpLengthLB { get; set; }
        public double Qty_FWP_LB { get; set; }
        public double WarpLengthRecd { get; set; }

        public double Qty_Dye { get; set; }
        public double Qty_Greige { get; set; }
        public double Qty_SRS { get; set; }
        public double Qty_SRM { get; set; }
        public double Qty_FWP { get; set; }
        public double ExesQty { get; set; }
        public double SWQty { get; set; }
        public double WYReqWarp { get; set; }
        public double WYReqWeft { get; set; }
        public int ExesCount { get; set; }
        public int FSCDetailID { get; set; }
        public int ReportType { get; set; }
        private string sProdtionTypeSt = "";
        public int Status { get; set; }
        public string ProdtionTypeSt
        {
            get
            {
                sProdtionTypeSt = ((EnumDispoProType)this.ProdtionType).ToString();
                //if (this.ReviseNo > 0) { sProdtionTypeSt = sProdtionTypeSt + " R-" + this.ReviseNo; };
                return sProdtionTypeSt;
            }
        }
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property
        public DateTime CurrentDate { get { return DateTime.Now; } }
        public string CurrentDateSt
        {
            get
            {
                return this.CurrentDate.ToString("dd MMM yyyy");
            }
        }
        public int Dateline
        {
            get
            {
                TimeSpan diff = this.CurrentDate - this.ExeDate;
                return diff.Days;
            }
        }
        public double Qty_SW
        {
            get
            {
                return this.Qty_SRS - this.Qty_SRM;
            }
        }
        public double Qty_Balance
        {
            get
            {
                return this.Qty_Greige - this.Qty_SRS + this.Qty_SRM;
            }
        }
        public string ReedNoSt
        {
            get
            {
                return this.ReedNo.ToString() + "/" + this.Dent.ToString();
            }
        }
        public string ExeDateSt
        {
            get
            {
                if (this.ExeDate == DateTime.MinValue) return "";
                return this.ExeDate.ToString("dd MMM yyyy");
            }
        }
        public string ReviseDateSt
        {
            get
            {
                if (this.ReviseDate == DateTime.MinValue) return "";
                return this.ReviseDate.ToString("dd MMM yyyy");
            }
        }
        public string IsPrintSt
        {
            get
            {
                if (this.IsPrint == true) return "Yes";
                return "No";
            }
        }
        public string DODateSt
        {
            get
            {
                if (this.DODate == DateTime.MinValue) return "";
                return this.DODate.ToString("dd MMM yyyy");
            }
        }
        public double TotalDispoGreyYarnReq
        {
            get
            {
                return this.GreyYarnReqWarp + this.GreyYarnReqWeft;
            }
            set
            { }
        }
        public double TotalGreyYarnIssue
        {
            get
            {
                return this.DyedYarnReqWarp + this.DyedYarnReqWeft;
            }
            set { }
        }
        public double ActualGreyYarnValue
        {
            get
            {
                return (this.DyedYarnReqWarp * this.YarnPriceWarp) + (this.DyedYarnReqWeft * this.YarnPriceWeft);
            }
            set { }
        }
        public double ReqGreyFabricsY
        {
            get
            {
                return this.ReqGreyFabrics * 1.09361;
            }
            set { }
        }
        public double ReqFinishedFabricsY
        {
            get
            {
                return this.ReqFinishedFabrics * 1.09361;
            }
            set { }
        }
        public double LengthReaming
        {
            get
            {
                return this.WarpLength - this.WarpLengthRecd;
            }
            
        }
        #endregion

        #region Functions
        public static List<RPT_Dispo> Gets(string sSQL, int nReportType, Int64 nUserID)
        {
            return RPT_Dispo.Service.Gets(sSQL, nReportType, nUserID);
        }
        public static List<RPT_Dispo> Gets_FYStockDispoWise(string sSQL, int nReportType, Int64 nUserID)
        {
            return RPT_Dispo.Service.Gets_FYStockDispoWise(sSQL, nReportType, nUserID);
        }
        
        public static List<RPT_Dispo> Gets_Weaving(string sSQL, int nReportType, Int64 nUserID)
        {
            return RPT_Dispo.Service.Gets_Weaving(sSQL, nReportType, nUserID);
        }
        public static List<RPT_Dispo> Gets(string sSQL,  Int64 nUserID)
        {
            return RPT_Dispo.Service.Gets(sSQL, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IRPT_DispoService Service
        {
            get { return (IRPT_DispoService)Services.Factory.CreateService(typeof(IRPT_DispoService)); }
        }
        #endregion

    }
    #endregion

    #region IRPT_Dispo interface

    public interface IRPT_DispoService
    {
        List<RPT_Dispo> Gets(string sSQL,Int64 nUserID);
        List<RPT_Dispo> Gets(string sSQL, int nReportType, Int64 nUserID);
        List<RPT_Dispo> Gets_FYStockDispoWise(string sSQL, int nReportType, Int64 nUserID);
        List<RPT_Dispo> Gets_Weaving(string sSQL, int nReportType, Int64 nUserID);
    }
    #endregion
}
