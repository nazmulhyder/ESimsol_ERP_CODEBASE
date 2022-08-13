using System;
using System.IO;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using System.Data;
using ICS.Core;
using ICS.Core.Framework;
using ICS.Core.Utility;


namespace ESimSol.BusinessObjects.ReportingObject
{
    #region RptProductionCostAnalysis
    
    public class RptProductionCostAnalysis : BusinessObject
    {
        #region  Constructor
        public RptProductionCostAnalysis()
        {

            RouteSheetID=0;
            MachineName="";
            UsesWeight=0;
            OrderNo = "";
            BuyerName = "";
            Shade = "";
            ProductName = "";
            BatchNo = "";
            ProductionQty = 0;
            RedyingForRSNo="";
            ShadePercentage = 0;
            AdditionalPercentage = 0;
            DyesCost = 0;
            ChemicalCost = 0;
            Remark = "";
            StartTime = DateTime.Today;
            EndTime = DateTime.Today;
            ShadePercentage = 0;
            IsInHouse = false;
            ErrorMessage = "";
            #region DISTINCT RPT_DUProductionRFT
             OrderType = 0;
             ContractorID =0;
             DyeingOrderID =0;
             ProductID =0;
             DyeingOrderDetailID = 0; 
             MachineID  = 0;
             UsesWeight = 0;
             ColorName = "";
             PSBatchNo  = "";
             RouteSheetNo = "";
             Qty =0;
             RSState =0;
             IsInHouse =false;
             AddCount=0;
             ShadeName="";
             Liquor =0;
             LabdipDetailID =0;
             LabdipNo = "";
             IsReDyeing = false;
             OrderTypeSt = "";
             RouteSheetCombineID = 0;
            #endregion
        }
        #endregion

        #region Properties
        
        public int RouteSheetID { get; set; }       
        public string MachineName { get; set; }
        public double UsesWeight { get; set; }
        public string OrderNo { get; set; }
        public string BuyerName { get; set; }
        public string Shade { get; set; }
        public string ProductName { get; set; }
        public string BatchNo { get; set; }
        public double ProductionQty { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string RedyingForRSNo { get; set; }
        public double DyesQty { get; set; }
        public double AdditionalDyesQty { get; set; }
        public double ShadePercentage { get; set; }
        public double AdditionalPercentage { get; set; }
        public double DyesCost { get; set; }
        public double ChemicalCost { get; set; }
        public string Remark { get; set; }
        public string CombineRSNo { get; set; }
        public Boolean IsInHouse { get; set; }
        public string ErrorMessage { get; set; }

        #endregion

        #region DISTINCT RPT_DUProductionRFT
        public int OrderType { get; set; }
        public int ContractorID { get; set; }
        public int DyeingOrderID { get; set; }
        public int ProductID { get; set; }
        public int DyeingOrderDetailID { get; set; }
        public int MachineID { get; set; }
        public string ColorName { get; set; }
        public string PSBatchNo { get; set; }
        public string RouteSheetNo { get; set; }
        public double Qty { get; set; }
        public int RSState { get; set; }
        public int AddCount { get; set; }
        public string ShadeName { get; set; }
        public double Liquor { get; set; }
        public int LabdipDetailID { get; set; }
        public string LabdipNo { get; set; }
        public string OrderTypeSt { get; set; }
        public Boolean IsReDyeing{ get; set; }
        public int RouteSheetCombineID { get; set; }
        

        #endregion


        #region Derived Property
        public string IsInHouseST
        {          
            get
            {
                if (this.IsInHouse == true)
                {
                    this.ErrorMessage = "In House";
                    return this.ErrorMessage;
                }
                else
                {
                    this.ErrorMessage = "Outside";
                    return this.ErrorMessage;
                }
            }
        }
        public string IsReDyeingST
        {
            get
            {
                if (this.IsReDyeing == true)
                {
                    this.ErrorMessage = "Is ReDyeing";
                    return this.ErrorMessage;
                }
                else
                {
                    this.ErrorMessage = "Fresh";
                    return this.ErrorMessage;
                }
            }
        }
        public string StartDateInString
        {
            get
            {
                return StartTime.ToString("dd MMM yyyy hh:mm");
            }
        }
        public string EndDateInString
        {
            get
            {
                return EndTime.ToString("dd MMM yyyy hh:mm");
            }
        }
        public double Loading
        {
            get
            {
                if (this.UsesWeight > 0)
                {
                    if ((this.ProductionQty * 100) / this.UsesWeight > 100)
                    {
                        return 100;
                    }
                    else
                    {
                        return Math.Round((this.ProductionQty * 100) / this.UsesWeight,2);

                    }
                }
                else
                {
                    return 0;
                }

            }
        }

        public double TotalDyesQty
        {
            get
            {
                return Math.Round(this.DyesQty + this.AdditionalDyesQty, 2);
            }
        }

        public double TotalShadePercentage
        {
            get
            {
                return Math.Round(this.ShadePercentage +this.AdditionalPercentage,2);
            }
        }

        public double TotalCost
        {
            get
            {
                return Math.Round( Math.Round(this.DyesCost) + Math.Round(this.ChemicalCost),2);
            }
        }

        public string DurationInString
        {
            get
            {
                return this.EndTime.Subtract(this.StartTime).ToString("dd'd 'hh'h 'mm'm'");
            }
        }

        public string DurationHHMMInString
        {
            get
            {
                return    (int.Parse(this.DurationInString.Split(' ')[0].TrimEnd(new char[] { 'd' }))*24 + int.Parse(this.DurationInString.Split(' ')[1].TrimEnd(new char[] { 'h' }))).ToString()+":"+int.Parse(this.DurationInString.Split(' ')[2].TrimEnd(new char[] { 'm' })).ToString(); 
            }
        }

        public string PRRemarks
        {
            get
            {
                if(this.RedyingForRSNo!="")
                {
                    return this.Remark + ", R-" + this.RedyingForRSNo;
                }
                else
                {
                    return this.Remark;
                }
            }
        }


        #endregion



        #region Functions

        public static  List<RptProductionCostAnalysis> MailContent(int PSSID, DateTime StartTime, DateTime EndTime, long nUserID)
        {
            return RptProductionCostAnalysis.Service.MailContent(PSSID, StartTime, EndTime, nUserID);
        }

        public static List<RptProductionCostAnalysis> MailContentDUProductionRFT(int PSSID, DateTime StartTime, DateTime EndTime, int nViewType, long nUserID)
        {
            return RptProductionCostAnalysis.Service.MailContentDUProductionRFT(PSSID, StartTime, EndTime, nViewType, nUserID);
        }

        #endregion

        #region ServiceFactory
        internal static IRptProductionCostAnalysisService Service
        {
            get { return (IRptProductionCostAnalysisService)Services.Factory.CreateService(typeof(IRptProductionCostAnalysisService)); }
        }

        #endregion
    }
    #endregion



    #region IRptProductionCostAnalysis interface
    
    public interface IRptProductionCostAnalysisService
    {
        
        List<RptProductionCostAnalysis> MailContent(int PSSID, DateTime StartTime, DateTime EndTime, long nUserID);
        List<RptProductionCostAnalysis> MailContentDUProductionRFT(int PSSID, DateTime StartTime, DateTime EndTime, int nViewType,long nUserID);

    }
    #endregion
}