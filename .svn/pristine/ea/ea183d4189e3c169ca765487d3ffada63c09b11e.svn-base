using System;
using System.IO;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using ICS.Core.Framework;
using ICS.Core.Utility;

namespace ESimSol.BusinessObjects
{
    #region ProductionRegister
    public class ProductionRegister : BusinessObject
    {
        public ProductionRegister()
        {
            PETransactionID=0;
		    ProductionSheetID=0;
		    ShiftID=0;
		    MachineID=0;
		    ShiftName="";
		    MachineName="";
            TransactionDate = DateTime.MinValue;
		    SheetNo="";
            UnitSymbol = "";
		    ProductionExecutionID=0;
		    ProductID=0;
		    ProductCode="";
		    ProductName="";
		    CustomerName="";
		    BuyerName="";
		    ExportPINo="";
		    
			YetToProduction=0;
			SheetQty=0;
			MoldingProduction=0;
			ActualMoldingProduction=0;
			ActualRejectGoods=0;
			ActualFinishGoods=0;
            Results = new List<ProductionRegister>();
        }
        #region Properties
        public int ProductionSheetID { get; set; }
        public int PETransactionID { get; set; }
        public int MachineID { get; set; }
        public int ShiftID { get; set; }
        public string ShiftName { get; set; }
        public string MachineName { get; set; }
        public DateTime TransactionDate { get; set; }
        public string SheetNo { get; set; }
        public string UnitSymbol { get; set; }
        public int ProductionExecutionID { get; set; }
        public int ProductID { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string CustomerName { get; set; }
        public string BuyerName { get; set; }
        public string ExportPINo { get; set; }

        public double SheetQty { get; set; }
        public double MoldingProduction { get; set; }
        public double ActualMoldingProduction { get; set; }
        public double ActualRejectGoods { get; set; }
        public double ActualFinishGoods { get; set; }
        public double YetToProduction { get; set; }
        public string RowHeader { get; set; }
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Properties
        public string TransactionDateSt 
        { 
            get 
            {
                if (this.TransactionDate != DateTime.MinValue)
                    return this.TransactionDate.ToString("dd MMM yyyy");
                else return "";
            }
        }
        public double YetToModling
        {
            get
            {
                if (this.ActualMoldingProduction>=this.SheetQty)
                    return 0;
                else return (this.SheetQty - this.ActualMoldingProduction);
            }
        }
        public int BUID { get; set; }
        public int DateCriteria { get; set; }
        public DateTime TransactionStartDate { get; set; }
        public DateTime TransactionEndDate { get; set; }
        public EnumReportLayout ReportLayout { get; set; }
        public List<ProductionRegister> Results { get; set; }
        #endregion

        #region Functions
        public static List<ProductionRegister> Gets(string sSQL,EnumReportLayout eEnumReportLayout, int nUserID)
        {
            return ProductionRegister.Service.Gets(sSQL,eEnumReportLayout, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IProductionRegisterService Service
        {
            get { return (IProductionRegisterService)Services.Factory.CreateService(typeof(IProductionRegisterService)); }
        }
        #endregion
        //For Production Execution Report
        public int ProductionStepID { get; set; }
    }
    #endregion

    #region IProductionRegister interface
    public interface IProductionRegisterService
    {
        List<ProductionRegister> Gets(string sSQL, EnumReportLayout eEnumReportLayout, int nUserID);
    }
    #endregion
}
