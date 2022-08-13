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
    #region VehicleReturnChallan
    
    public class VehicleReturnChallan : BusinessObject
    {
        public VehicleReturnChallan()
        {
            VehicleReturnChallanID = 0;
            ContractorID = 0;
            ApproveBy = 0;
            ReturnChallanDate = DateTime.Now;
            Note = "";
            ApproveDate = DateTime.Now;
            Note = "";
            ReturnChallanNo = "";
            Refund_Amount = 0;
            ProductID = 0;
            ProductName = "";
            ReceivedByID = 0;
            ReceivedByName = "";
            LCNo = "";
            PreaperByName = "";
            MKTPName = "";
            WorkingUnitID = 0;
            SQNo = "";
        }
       
        #region Properties
        public int VehicleReturnChallanID { get; set; }
        public string ReturnChallanNo { get; set; }
        public DateTime ReturnChallanDate { get; set; }
        public int ContractorID { get; set; }
        public int ContactPersonnelID { get; set; }
        public string Note { get; set; }
        public double Refund_Amount { get; set; }
        public string SQNo { get; set; }
        public string InvoiceNo { get; set; }
        public string ModelNo { get; set; }
        public string EngineNo { get; set; }
        public string ChassisNo { get; set; }
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public string VehicleNo { get; set; }
        public int ReceivedByID { get; set; }
        public string ReceivedByName { get; set; }
        public int WorkingUnitID { get; set; }
        public int ApproveBy { get; set; }
        public DateTime ApproveDate { get; set; }
        public DateTime InvoiceDate { get; set; }
        public string CustomerName { get; set; }
        public int LotID { get; set; }
        public string LotNo { get; set; }
        public int SaleInvoiceID { get; set; }
        public int IsReceived { get; set; }
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
        public string ReturnChallanNoFull { get; set; }
        public string ContractorName { get; set; }
        public string ContactPersonnelName { get; set; }
        public string ApproveByName { get; set; }
        public string PreaperByName { get; set; }
        public string LCNo { get; set; }
        public string MKTPName { get; set; }
        public string ReturnChallanDateSt
        {
            get
            {
                return this.ReturnChallanDate.ToString("dd MMM yyyy");
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
        public static VehicleReturnChallan Get(int nId, long nUserID)
        {
            return VehicleReturnChallan.Service.Get(nId, nUserID);
        }
        public static List<VehicleReturnChallan> Gets(string sSQL, long nUserID)
        {
            return VehicleReturnChallan.Service.Gets(sSQL, nUserID);
        }
        public VehicleReturnChallan Save( long nUserID)
        {
            return VehicleReturnChallan.Service.Save(this,  nUserID);
        }
        public VehicleReturnChallan Approve(long nUserID)
        {
            return VehicleReturnChallan.Service.Approve(this, nUserID);
        }
        public string Delete(long nUserID)
        {
            return VehicleReturnChallan.Service.Delete(this,nUserID);
        }

        #endregion

        #region ServiceFactory
        internal static IVehicleReturnChallanService Service
        {
            get { return (IVehicleReturnChallanService)Services.Factory.CreateService(typeof(IVehicleReturnChallanService)); }
        }
        #endregion



        public string Params { get; set; }

        public string OperationUnitName { get; set; }
    }

    #region IVehicleReturnChallan interface
    
    public interface IVehicleReturnChallanService
    {
        VehicleReturnChallan Get(int id, long nUserID);
        List<VehicleReturnChallan> Gets(string sSQL, long nUserID);
        VehicleReturnChallan Save(VehicleReturnChallan oVehicleReturnChallan,  long nUserID);
        VehicleReturnChallan Approve(VehicleReturnChallan oVehicleReturnChallan, long nUserID);
        string Delete(VehicleReturnChallan oVehicleReturnChallan, long nUserID);
    }
    #endregion

    #endregion
}
