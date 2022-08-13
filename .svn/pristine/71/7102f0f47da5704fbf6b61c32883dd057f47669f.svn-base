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
    #region DUDeliveryChallan
    
    public class DUDeliveryChallan : BusinessObject
    {
        public DUDeliveryChallan()
        {
            DUDeliveryChallanID = 0;
            
            ContractorID = 0;
            ContactPersonnelID = 0;
            ApproveBy = 0;
            ChallanDate = DateTime.Now;
            Note = "";
            ApproveDate = DateTime.Now;
            Qty = 0.0;
            DUDeliveryChallanDetails = new List<DUDeliveryChallanDetail>();
            Note = "";
            ChallanNo = "";
            GatePassNo = "";
            VehicleName = "";
            ReceivedByName = "";
            ReceiveByID = 0;
            StoreInchargeID = 0;
            OrderType = 0;
            FactoryAddress = "";
            LCNo = "";
            PreaperByName = "";
            MKTPName = "";
            WorkingUnitID = 0;
            OrderTypeSt = "";
            OrderCode = "";
            DeliveryZone = "";
            MBuyer = "";
            DeliveryBy = "";
            DUDeliveryChallanPacks = new List<DUDeliveryChallanPack>();
            //PackCountBy = EnumPackCountBy.Bag;
        }
       
        #region Properties

        public int DUDeliveryChallanID { get; set; }
        public string ChallanNo { get; set; }
        public DateTime ChallanDate { get; set; }
        public int ContractorID { get; set; }
        public int ContactPersonnelID { get; set; }
        public string Note { get; set; }
        public string GatePassNo { get; set; }
        public string OrderTypeSt { get; set; }
        public string OrderCode { get; set; }
        public string VehicleName { get; set; }
        public string VehicleNo { get; set; }
        public string ReceivedByName { get; set; }
        public string DeliveryBy { get; set; }
        public int ReceiveByID { get; set; }
        public int StoreInchargeID { get; set; }
        public int OrderType { get; set; }
        public int WorkingUnitID { get; set; }
        public int ApproveBy { get; set; }
        public DateTime ApproveDate { get; set; }
        public string FactoryAddress { get; set; }
        public EnumPackCountBy PackCountBy { get; set; }
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property
        public List<DUDeliveryChallanDetail> DUDeliveryChallanDetails { get; set; }
        public List<DUDeliveryChallanPack> DUDeliveryChallanPacks { get; set; }
        public DeliverySetup DeliverySetup { get; set; }
        public string ContractorName { get; set; }
        public string ContactPersonnelName { get; set; }
        public double Qty { get; set; }
        public string ApproveByName { get; set; }
        public string PreaperByName { get; set; }
        public string OrderNos { get; set; }
        public string DONos { get; set; }
        public string LCNo { get; set; }
        public string MKTPName { get; set; }
        
        public string ChallanDateSt
        {
            get
            {
                return this.ChallanDate.ToString("dd MMM yyyy");
            }
        }
        public string PackCountByInSt
        {
            get
            {
                return EnumObject.jGet(this.PackCountBy);
            }
        }
        //public string OrderTypeSt
        //{
        //    get
        //    {
        //        return EnumObject.jGet((EnumOrderType)this.OrderType);
        //    }
        //}
        public double DOQty { get; set; }
        public string DeliveryZone { get; set; }
        public string MBuyer { get; set; }
        
        #endregion

        #region Functions
        public static DUDeliveryChallan Get(int nId, long nUserID)
        {
            return DUDeliveryChallan.Service.Get(nId, nUserID);
        }
        public static DUDeliveryChallan GetbyDO(int nId, long nUserID)
        {
            return DUDeliveryChallan.Service.GetbyDO(nId, nUserID);
        }
        public static List<DUDeliveryChallan> Gets(string sSQL, long nUserID)
        {
            return DUDeliveryChallan.Service.Gets(sSQL, nUserID);
        }
     
        public static List<DUDeliveryChallan> GetsBy(string sContractorID, long nUserID)
        {
            return DUDeliveryChallan.Service.GetsBy(sContractorID, nUserID);
        }

        public static List<DUDeliveryChallan> GetsByPI(int nExportPIID, long nUserID)
        {
            return DUDeliveryChallan.Service.GetsByPI(nExportPIID, nUserID);
        }
        public DUDeliveryChallan Save( long nUserID)
        {
            return DUDeliveryChallan.Service.Save(this,  nUserID);
        }
       
        public DUDeliveryChallan Approve(long nUserID)
        {
            return DUDeliveryChallan.Service.Approve(this, nUserID);
        }
        public string Delete(long nUserID)
        {
            return DUDeliveryChallan.Service.Delete(this,nUserID);
        }
        public DUDeliveryChallan UpdateFields(long nUserID)
        {
            return DUDeliveryChallan.Service.UpdateFields(this, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IDUDeliveryChallanService Service
        {
            get { return (IDUDeliveryChallanService)Services.Factory.CreateService(typeof(IDUDeliveryChallanService)); }
        }
        #endregion
    }


    #region IDUDeliveryChallan interface
    
    public interface IDUDeliveryChallanService
    {
        DUDeliveryChallan Get(int id, long nUserID);
        DUDeliveryChallan GetbyDO(int DOID, long nUserID);
        List<DUDeliveryChallan> Gets(string sSQL, long nUserID);
        List<DUDeliveryChallan> GetsBy(string sContractorIDs, long nUserID);
        List<DUDeliveryChallan> GetsByPI(int nExportPIID, long nUserID);
        DUDeliveryChallan Save(DUDeliveryChallan oDUDeliveryChallan,  long nUserID);
        DUDeliveryChallan Approve(DUDeliveryChallan oDUDeliveryChallan, long nUserID);
        string Delete(DUDeliveryChallan oDUDeliveryChallan, long nUserID);
        DUDeliveryChallan UpdateFields(DUDeliveryChallan oDUDeliveryChallan, Int64 nUserID);
       
    }
    #endregion

    #endregion
}
