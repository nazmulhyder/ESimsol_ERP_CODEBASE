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
    #region SparePartsConsumptionRegister

    public class SparePartsConsumptionRegister : BusinessObject
    {
        public SparePartsConsumptionRegister()
        {
            SparePartsChallanDetailID = 0;
            SparePartsChallanID = 0;
            SparePartsRequisitionDetailID = 0;
            ProductID = 0;
            ProductName = "";
            ProductCode = "";
            LotID = 0;
            LotNo = "";
            MUnitID = 0;
            MUnitName = "";
            ConsumptionQty = 0;
            UnitPrice = 0;
            Remarks = "";
            ChallanBy = 0;
            ChallanByName = "";
            ChallanNo = "";
            StoreID = 0;
            SparePartsRequisitionID = 0;
            CRID = 0;
            RequisitionBy = 0;
            RequisitionByName = "";
            RequisitionNo = "";
            ApprovedBy = 0;
            ApprovedByName = "";
            StoreName = "";
            CRCode = "";
            CRName = "";
            CRModel = "";
            CRBrand = "";
            ResourceType = 0;
            BusinessUnitName = "";
            BUID = 0;
            ResourceTypeName = "";
            LocationName = "";
            Currency = "";
            Amount = 0;
            BUShortName = "";
            ChallanDate = DateTime.Now;
            IssueDate = DateTime.Now;
            ReportLayout = EnumReportLayout.None;
            LineNumber = 0;
            SparePartsConsumptionRegisters = new List<SparePartsConsumptionRegister>();
        }

        #region Properties
        public int SparePartsChallanDetailID { get; set; }
        public int SparePartsChallanID { get; set; }
        public int SparePartsRequisitionDetailID { get; set; }
        public int ProductID { get; set; }
        public int LotID { get; set; }
        public int MUnitID { get; set; }
        public string Remarks { get; set; }
        public double ConsumptionQty { get; set; }
        public int ChallanBy { get; set; }
        public int BUID { get; set; }
        public string ChallanNo { get; set; }
        public DateTime ChallanDate { get; set; }
        public int StoreID { get; set; }
        public int SparePartsRequisitionID { get; set; }
        public int CRID { get; set; }
        public string RequisitionNo { get; set; }
        public int RequisitionBy { get; set; }
        public DateTime IssueDate { get; set; }
        public int ApprovedBy { get; set; }
        public string CRCode { get; set; }
        public string CRName { get; set; }
        public string CRModel { get; set; }
        public string CRBrand { get; set; }
        public int ResourceType { get; set; }
        public string ResourceTypeName { get; set; }
        public string ProductName { get; set; }
        public string ProductCode { get; set; }
        public string LotNo { get; set; }
        public double UnitPrice { get; set; }
        public double Amount { get; set; }
        public string MUnitName { get; set; }
        public string BusinessUnitName { get; set; }
        public string BUShortName { get; set; }
        public string RequisitionByName { get; set; }
        public string ApprovedByName { get; set; }
        public string ChallanByName { get; set; }
        public string StoreName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public List<SparePartsConsumptionRegister> SparePartsConsumptionRegisters { get; set; }
        public int LineNumber { get; set; }
        public string LocationName { get; set; }
        public string Currency { get; set; }
        public EnumReportLayout ReportLayout { get; set; }
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property

        public string IssueDateSt
        {
            get
            {
                return this.IssueDate.ToString("dd MMM yyyy");
            }
        }
        public string ChallanDateSt
        {
            get
            {
                return this.ChallanDate.ToString("dd MMM yyyy");
            }
        }
        public string StartDateSt
        {
            get
            {
                return this.StartDate.ToString("dd MMM yyyy");
            }
        }
        public string EndDateSt
        {
            get
            {
                return this.EndDate.ToString("dd MMM yyyy");
            }
        }
        public string AmountSt
        {
            get
            {
                return Global.MillionFormat(this.Amount);
            }
        }
        public string SearchingCiteria
        {
            get
            {
                this.ErrorMessage = "";
                if (this.BUID > 0) this.ErrorMessage += this.BUShortName + " || ";
                if (this.ResourceType > 0) this.ErrorMessage += this.ResourceTypeName + " || ";
                if (!string.IsNullOrEmpty(this.CRName)) this.ErrorMessage += this.CRName + " || ";
                if (!string.IsNullOrEmpty(this.CRCode)) this.ErrorMessage += this.CRCode + " || ";
                if (!string.IsNullOrEmpty(this.ProductName)) this.ErrorMessage += this.ProductName + " || ";
                if (!string.IsNullOrEmpty(this.ProductCode)) this.ErrorMessage += this.ProductCode + " || ";
                if (!string.IsNullOrEmpty(this.ChallanNo)) this.ErrorMessage += "Challan No: " + this.ChallanNo + " || ";
                if (!string.IsNullOrEmpty(this.RequisitionNo)) this.ErrorMessage += "RequisitionNo: " + this.RequisitionNo + " || ";
                this.ErrorMessage += this.StartDateSt + " to " + this.EndDateSt;
                return ErrorMessage;
            }
        }
        #endregion

        #region Functions
        public static List<SparePartsConsumptionRegister> Gets(string sSQL, Int64 nUserID)
        {
            return SparePartsConsumptionRegister.Service.Gets(sSQL, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static ISparePartsConsumptionRegisterService Service
        {
            get { return (ISparePartsConsumptionRegisterService)Services.Factory.CreateService(typeof(ISparePartsConsumptionRegisterService)); }
        }
        #endregion
    }
    #endregion

    #region ISparePartsConsumptionRegister interface

    public interface ISparePartsConsumptionRegisterService
    {
        List<SparePartsConsumptionRegister> Gets(string sSQL, Int64 nUserID);
    }
    #endregion
}