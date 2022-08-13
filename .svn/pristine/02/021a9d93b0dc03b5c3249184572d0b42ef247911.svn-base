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
    #region GRNRegister
    public class GRNRegister : BusinessObject
    {
        public GRNRegister()
        {
            GRNDetailID = 0;
            GRNID = 0;
            ProductID = 0;
            MUnitID = 0;
            UnitPrice = 0;
            ReceivedQty = 0;
            Amount = 0;
            GRNNo = "";
            BUID = 0;
            ChallanNo = "";
            ApproveDate = DateTime.MinValue;
            GRNStatus = EnumGRNStatus.Initialize;
            ContractorID = 0;
            ContactPersonID = 0;
            CurrencyID = 0;
            ApproveBy = 0;
            GRNDate = DateTime.MinValue;
            VersionNumber = 0;
            GLDate = DateTime.MinValue;
            GRNType = EnumGRNType.None;
            Remarks = "";
            ApprovedByName = "";
            ProductCode = "";
            ProductName = "";
            MUName = "";
            MUSymbol = "";
            SupplierName = "";
            ReceivedByName = "";
            StoreName = "";
            CurrencyName = "";
            CurrencySymbol = "";
            ErrorMessage = "";
            SearchingData = "";
            ConversionRate = 1;
            RefQty = 0;
            ExtraQty = 0;
            StyleNo= "";
            ReportLayout = EnumReportLayout.None;
            LotNo = "";
            LCNo = "";
            ColorName = "";
            PINo = "";
            PIQty = 0;
            ProductTypeSt = "";
            GroupName = "";
            MRIRNo = "";
            GatePassNo = "";
            CRate = 0;
        }

        #region Properties
        public int GRNDetailID { get; set; }
        public int GRNID { get; set; }
        public int ProductID { get; set; }
        public int MUnitID { get; set; }
        public double UnitPrice { get; set; }
        public double ReceivedQty { get; set; }
        public double Amount { get; set; }
        public string GRNNo { get; set; }
        public int BUID { get; set; }
        public string ChallanNo { get; set; }
        public DateTime ApproveDate { get; set; }
        public EnumGRNStatus GRNStatus { get; set; }
        public int ContractorID { get; set; }
        public int ContactPersonID { get; set; }
        public int CurrencyID { get; set; }
        public int ApproveBy { get; set; }
        public DateTime GRNDate { get; set; }
        public int VersionNumber { get; set; }
        public DateTime GLDate { get; set; }
        public EnumGRNType GRNType { get; set; }
        public string Remarks { get; set; }
        public string ApprovedByName { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string MUName { get; set; }
        public string MUSymbol { get; set; }
        public string SupplierName { get; set; }
        public string ReceivedByName { get; set; }
        public string StoreName { get; set; }
        public string CurrencyName { get; set; }
        public string CurrencySymbol { get; set; }
        public string ErrorMessage { get; set; }
        public string SearchingData { get; set; }
        public double ConversionRate { get; set; }
        public double CRate { get; set; }
        public double  RefQty { get; set; }
        public double ExtraQty { get; set; }
        public double PIQty { get; set; }//// import PI Qty
        public string StyleNo { get; set; }
        public string LotNo { get; set; }
        public string LCNo { get; set; }
        public string ColorName { get; set; }
        public string GroupName { get; set; }
        public EnumProductNature ProductType { get; set; }
        public EnumReportLayout ReportLayout { get; set; }
        public int BuyerID { get; set; }
        public string BuyerName { get; set; }
        public string RefObjectNo { get; set; }
        public string PINo { get; set; }
        public string BrandName { get; set; }
        public string ProductTypeSt { get; set; }
        public string GatePassNo { get; set; }
        public string MRIRNo { get; set; }

        #endregion 

        #region Derived Property

      

        public string UnitPriceSt
        {
            get
            {
                if (this.ConversionRate <= 1)
                {
                    return Global.MillionFormat(this.UnitPrice);
                }
                else
                {
                    return Global.MillionFormat(this.UnitPrice) + "/" + this.ConversionRate.ToString();
                }
            }
        }
        public string ApproveDateSt
        {
            get 
            {
                if (this.ApproveDate == DateTime.MinValue)
                {
                    return "--";
                }
                else
                {
                    return this.ApproveDate.ToString("dd MMM yyyy");
                }
            }
        }
        public string GRNDateSt
        {
            get
            {
                if (this.GRNDate == DateTime.MinValue)
                {
                    return "--";
                }
                else
                {
                    return this.GRNDate.ToString("dd MMM yyyy");
                }
            }
        }
        public string GLDateSt
        {
            get
            {
                if (this.GLDate == DateTime.MinValue)
                {
                    return "--";
                }
                else
                {
                    return this.GLDate.ToString("dd MMM yyyy");
                }
            }
        }
        public string GRNStatusSt
        {
            get
            {
                return EnumObject.jGet(this.GRNStatus);
            }
        }

        #endregion

        #region Functions
        public static List<GRNRegister> Gets(string sSQL, long nUserID)
        {
            return GRNRegister.Service.Gets(sSQL, nUserID);
        }
        public static List<GRNRegister> GetsTwo(string sSQL, long nUserID)
        {
            return GRNRegister.Service.GetsTwo(sSQL, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IGRNRegisterService Service
        {
            get { return (IGRNRegisterService)Services.Factory.CreateService(typeof(IGRNRegisterService)); }
        }
        #endregion
    }
    #endregion

    #region IGRNRegister interface

    public interface IGRNRegisterService
    {
        List<GRNRegister> Gets(string sSQL, Int64 nUserID);
        List<GRNRegister> GetsTwo(string sSQL, Int64 nUserID);
    }
    #endregion
}
