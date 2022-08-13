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
    #region ExportPIRegister
    public class ExportPIRegister : BusinessObject
    {
        public ExportPIRegister()
        {
            ExportPIID =0;
			ExportPINo ="";
			ExportPIDate =DateTime.Now;
			MotherBuyerID =0;
			BUID =0;
			ProductNature = EnumProductNature.Dyeing;
            ProductNatureInt = (int)EnumProductNature.Dyeing;
			MotherBuyerName ="";
			BuyerID =0;
			BuyerName ="";
			CurrencyID =0;
			CurrencySymbol ="";
			PIValue =0;
			DeliveryValue =0;
			PaidByLC =0;
            PaidByCash = 0;
            DueAmount = 0;
            ErrorMessage = "";
            SearchingData = "";
            IsWithDue = false;
            DueOptions = 0;
            ReportLayout = EnumReportLayout.None;
        }

        #region Properties
        
        public int ExportPIID { get; set; }
        public int MotherBuyerID { get; set; }
        public EnumProductNature ProductNature { get; set; }
        public double UnitPrice { get; set; }
        public double Qty { get; set; }
        public double Amount { get; set; }
        public string ExportPINo { get; set; }
        public int BUID { get; set; }
        public DateTime ExportPIDate { get; set; }
        public int BuyerID { get; set; }
        public int CurrencyID { get; set; }
        public double PIValue { get; set; }
        public double DeliveryValue { get; set; }
        public double PaidByLC { get; set; }
        public string MotherBuyerName { get; set; }
        public string BuyerName { get; set; }
        public double PaidByCash { get; set; }
        public string CurrencyName { get; set; }
        public string CurrencySymbol { get; set; }
        public int PIType { get; set; }
        public double DueAmount { get; set; }
        public string ErrorMessage { get; set; }
        public string SearchingData { get; set; }
        public bool IsWithDue { get; set; }
        public int ProductNatureInt { get; set; }
        public int DueOptions { get; set; }//0: None;1:Due with Challan;2:Only Due
        public EnumReportLayout ReportLayout { get; set; }
        #endregion

        #region Derived Property

        //public double DueAmount
        //{
        //    get
        //    {
        //        return this.PIValue - (this.PaidByCash + this.PaidByLC);
        //    }
        //}
        public string ExportPIDateSt
        {
            get 
            {
                if (this.ExportPIDate == DateTime.MinValue)
                {
                    return "--";
                }
                else
                {
                    return this.ExportPIDate.ToString("dd MMM yyyy");
                }
            }
        }
    
       
        #endregion

        #region Functions
        public static List<ExportPIRegister> Gets(string sSQL, int nReportLayout, int nDueOptions, long nUserID)
        {
            return ExportPIRegister.Service.Gets(sSQL,nReportLayout,nDueOptions, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IExportPIRegisterService Service
        {
            get { return (IExportPIRegisterService)Services.Factory.CreateService(typeof(IExportPIRegisterService)); }
        }
        #endregion
    }
    #endregion

    #region IExportPIRegister interface

    public interface IExportPIRegisterService
    {
        List<ExportPIRegister> Gets(string sSQL, int nReportLayout, int nDueOptions, Int64 nUserID);
    }
    #endregion
}
