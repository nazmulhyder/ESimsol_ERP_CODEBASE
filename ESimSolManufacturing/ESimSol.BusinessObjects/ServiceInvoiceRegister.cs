using System;
using ICS.Core.Framework;
using ICS.Core.Utility;
using ICS.Core;
using ESimSol.BusinessObjects;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.ServiceModel;
using ESimSol.BusinessObjects.ReportingObject;

namespace ESimSol.BusinessObjects
{
    #region ServiceInvoiceRegister
    public class ServiceInvoiceRegister : BusinessObject
    {
        public ServiceInvoiceRegister()
        {
            ServiceInvoiceDetailID = 0;
            ServiceInvoiceID = 0;
            PartsNo = "";
            PartsName = "";
            PartsCode = "";
            CustomerID = 0;

            CustomerName = "";
            ServiceInvoiceDate = DateTime.Now;
            VehicleRegNo = "";
            ChassisNo = "";
            EngineNo = "";
            VehicleModelNo = "";
            VehicleTypeName = "";

            MUnitID = 0;
            MUName = "";
            Qty = 0;
            UnitPrice = 0;
            TotalPrice = 0;
            Remarks = "";

            ServiceOrderType = EnumServiceOrderType.None;
            ReportLayout = EnumReportLayout.None;
            ErrorMessage = "";
        }

        #region Property
        public int ServiceInvoiceDetailID { get; set; }
        public int ServiceInvoiceID { get; set; }
        public string PartsNo { get; set; }
        public string PartsName { get; set; }
        public string PartsCode { get; set; }
        public int CustomerID { get; set; }
        public string CustomerName { get; set; }
        public DateTime ServiceInvoiceDate { get; set; }
        public string VehicleRegNo { get; set; }
        public int BUID { get; set; }
        public string ChassisNo { get; set; }
        public string EngineNo { get; set; }
        public string VehicleModelNo { get; set; }
        public string VehicleTypeName { get; set; }
        public int MUnitID { get; set; }
        public string MUName { get; set; }
        public int VehiclePartsID { get; set; }
        public int ChargeType { get; set; }
        public EnumServiceOrderType ServiceOrderType { get; set; }
        public double Qty { get; set; }
        public double UnitPrice { get; set; }
        public double TotalPrice { get; set; }
        public string Remarks { get; set; }
        public string ErrorMessage { get; set; }
        public EnumReportLayout ReportLayout { get; set; }
        #endregion

        #region Derived Property

        public string ServiceInvoiceDateInString
        {
            get
            {
                if (ServiceInvoiceDate == DateTime.MinValue) return "";
                return ServiceInvoiceDate.ToString("dd MMM yyyy");
            }
        }
        public string ServiceOrderTypeSt
        {
            get
            {
                return EnumObject.jGet(this.ServiceOrderType);
            }
        }
        
        #endregion

        #region Functions
        public static List<ServiceInvoiceRegister> Gets(int nServiceInvoiceID, long nUserID)
        {
            return ServiceInvoiceRegister.Service.Gets(nServiceInvoiceID, nUserID);
        }
        public static List<ServiceInvoiceRegister> Gets(string sSQL, long nUserID)
        {
            return ServiceInvoiceRegister.Service.Gets(sSQL, nUserID);
        }
        public ServiceInvoiceRegister Get(int id, long nUserID)
        {
            return ServiceInvoiceRegister.Service.Get(id, nUserID);
        }

        #endregion

        #region ServiceFactory
        internal static IServiceInvoiceRegisterService Service
        {
            get { return (IServiceInvoiceRegisterService)Services.Factory.CreateService(typeof(IServiceInvoiceRegisterService)); }
        }
        #endregion

        public List<ServiceInvoiceRegister> ServiceInvoiceRegisters { get; set; }
    }
    #endregion

    #region IServiceInvoiceRegister interface
    public interface IServiceInvoiceRegisterService
    {
        ServiceInvoiceRegister Get(int id, Int64 nUserID);
        List<ServiceInvoiceRegister> Gets(int nServiceInvoiceID, Int64 nUserID);
        List<ServiceInvoiceRegister> Gets(string sSQL, Int64 nUserID);
    }
    #endregion
}
