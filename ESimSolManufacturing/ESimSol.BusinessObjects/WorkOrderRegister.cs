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
    #region WorkOrderRegister
    public class WorkOrderRegister : BusinessObject
    {
        public WorkOrderRegister()
        {
            WorkOrderDetailID = 0;
            WorkOrderID = 0;
            ProductID = 0;
            MUnitID = 0;
            UnitPrice = 0;
            Qty = 0;
            Amount = 0;
            WorkOrderNo = "";
            BUID = 0;
            FiLeNo = "";
            ApproveDate = DateTime.MinValue;
            WorkOrderStatus = EnumWorkOrderStatus.Intialize;
            SupplierID = 0;
            ContactPersonID = 0;
            CurrencyID = 0;
            ApproveBy = 0;
            WorkOrderDate = DateTime.MinValue;
            VersionNumber = 0;
            ExpectedDeliveryDate = DateTime.MinValue;
            ShipmentBy = EnumShipmentBy.None;
            Remarks = "";
            ApprovedByName = "";
            ProductCode = "";
            ProductName = "";
            MUName = "";
            MUSymbol = "";
            SupplierName = "";
            SCPName = "";
            CPName = "";
            CurrencyName = "";
            CurrencySymbol = "";
            ErrorMessage = "";
            SearchingData = "";
            RateUnit = 1;
            ReportLayout = EnumReportLayout.None;
        }

        #region Properties
        public int WorkOrderDetailID { get; set; }
        public int WorkOrderID { get; set; }
        public int ProductID { get; set; }
        public int MUnitID { get; set; }
        public double UnitPrice { get; set; }
        public double Qty { get; set; }
        public double Amount { get; set; }
        public string WorkOrderNo { get; set; }
        public int BUID { get; set; }
        public string FiLeNo { get; set; }
        public DateTime ApproveDate { get; set; }
        public EnumWorkOrderStatus WorkOrderStatus { get; set; }
        public int SupplierID { get; set; }
        public int ContactPersonID { get; set; }
        public int CurrencyID { get; set; }
        public int ApproveBy { get; set; }
        public DateTime WorkOrderDate { get; set; }
        public int VersionNumber { get; set; }
        public DateTime ExpectedDeliveryDate { get; set; }
        public EnumShipmentBy ShipmentBy { get; set; }
        public string Remarks { get; set; }
        public string ApprovedByName { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string MUName { get; set; }
        public string MUSymbol { get; set; }
        public string SupplierName { get; set; }
        public string SCPName { get; set; }
        public string CPName { get; set; }
        public string CurrencyName { get; set; }
        public string CurrencySymbol { get; set; }
        public string ErrorMessage { get; set; }
        public string SearchingData { get; set; }
        public int RateUnit { get; set; }
        public EnumReportLayout ReportLayout { get; set; }
        #endregion

        #region Derived Property
        public string UnitPriceSt
        {
            get
            {
                if (this.RateUnit <= 1)
                {
                    return Global.MillionFormat(this.UnitPrice);
                }
                else
                {
                    return Global.MillionFormat(this.UnitPrice) + "/" + this.RateUnit.ToString();
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
        public string WorkOrderDateSt
        {
            get
            {
                if (this.WorkOrderDate == DateTime.MinValue)
                {
                    return "--";
                }
                else
                {
                    return this.WorkOrderDate.ToString("dd MMM yyyy");
                }
            }
        }
        public string ExpectedDeliveryDateSt
        {
            get
            {
                if (this.ExpectedDeliveryDate == DateTime.MinValue)
                {
                    return "--";
                }
                else
                {
                    return this.ExpectedDeliveryDate.ToString("dd MMM yyyy");
                }
            }
        }
        public string WorkOrderStatusSt
        {
            get
            {
                return EnumObject.jGet(this.WorkOrderStatus);
            }
        }

        #endregion

        #region Functions
        public static List<WorkOrderRegister> Gets(string sSQL, long nUserID)
        {
            return WorkOrderRegister.Service.Gets(sSQL, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IWorkOrderRegisterService Service
        {
            get { return (IWorkOrderRegisterService)Services.Factory.CreateService(typeof(IWorkOrderRegisterService)); }
        }
        #endregion
    }
    #endregion

    #region IWorkOrderRegister interface

    public interface IWorkOrderRegisterService
    {
        List<WorkOrderRegister> Gets(string sSQL, Int64 nUserID);
    }
    #endregion
}
