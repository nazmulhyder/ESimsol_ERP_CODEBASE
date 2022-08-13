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
    #region VehicleChallan
    
    public class VehicleChallan : BusinessObject
    {
        public VehicleChallan()
        {
            VehicleChallanID = 0;
            ContractorID = 0;
            ApproveBy = 0;
            ChallanDate = DateTime.Now;
            Note = "";
            ApproveDate = DateTime.Now;
            Note = "";
            ChallanNo = "";
            GatePassNo = "";
            ProductID = 0;
            ProductName = "";
            DeliveredByID = 0;
            DeliveredByName = "";
            LCNo = "";
            PreaperByName = "";
            MKTPName = "";
            WorkingUnitID = 0;
            SQNo = "";
        }
       
        #region Properties
        public int VehicleChallanID { get; set; }
        public string ChallanNo { get; set; }
        public DateTime ChallanDate { get; set; }
        public int ContractorID { get; set; }
        public int ContactPersonnelID { get; set; }
        public string Note { get; set; }
        public string GatePassNo { get; set; }
        public string SQNo { get; set; }
        public string InvoiceNo { get; set; }
        public string ModelNo { get; set; }
        public string EngineNo { get; set; }
        public string ChassisNo { get; set; }
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public string VehicleNo { get; set; }
        public int DeliveredByID { get; set; }
        public string DeliveredByName { get; set; }
        public int WorkingUnitID { get; set; }
        public int ApproveBy { get; set; }
        public DateTime ApproveDate { get; set; }
        public DateTime InvoiceDate { get; set; }
        public string CustomerName { get; set; }
        public int LotID { get; set; }
        public string LotNo { get; set; }
        public int SaleInvoiceID { get; set; }
        public int IsDelivered { get; set; }
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property
        public string BankName { get; set; }
        public string CurrencyName { get; set; }
        public double CRate { get; set; }
        public double OTRAmount { get; set; }
        public double NetAmount { get; set; }
        public double DiscountAmount { get; set; }
        public double AdvanceAmount { get; set; }
        public string ChallanNoFull { get; set; }
        public string ContractorName { get; set; }
        public string ContactPersonnelName { get; set; }
        public string ApproveByName { get; set; }
        public string PreaperByName { get; set; }
        public string LCNo { get; set; }
        public string MKTPName { get; set; }
        public string ChallanDateSt
        {
            get
            {
                return this.ChallanDate.ToString("dd MMM yyyy");
            }
        }
        public string InvoiceDateSt
        {
            get
            {
                return this.InvoiceDate.ToString("dd MMM yyyy");
            }
        }
        #endregion

        #region Functions
        public static VehicleChallan Get(int nId, long nUserID)
        {
            return VehicleChallan.Service.Get(nId, nUserID);
        }
        public static List<VehicleChallan> Gets(string sSQL, long nUserID)
        {
            return VehicleChallan.Service.Gets(sSQL, nUserID);
        }
        public VehicleChallan Save( long nUserID)
        {
            return VehicleChallan.Service.Save(this,  nUserID);
        }
        public VehicleChallan Approve(long nUserID)
        {
            return VehicleChallan.Service.Approve(this, nUserID);
        }
        public string Delete(long nUserID)
        {
            return VehicleChallan.Service.Delete(this,nUserID);
        }

        #endregion

        #region ServiceFactory
        internal static IVehicleChallanService Service
        {
            get { return (IVehicleChallanService)Services.Factory.CreateService(typeof(IVehicleChallanService)); }
        }
        #endregion



        public string Params { get; set; }

        public string OperationUnitName { get; set; }
    }

    #region IVehicleChallan interface
    
    public interface IVehicleChallanService
    {
        VehicleChallan Get(int id, long nUserID);
        List<VehicleChallan> Gets(string sSQL, long nUserID);
        VehicleChallan Save(VehicleChallan oVehicleChallan,  long nUserID);
        VehicleChallan Approve(VehicleChallan oVehicleChallan, long nUserID);
        string Delete(VehicleChallan oVehicleChallan, long nUserID);
    }
    #endregion

    #endregion
}
