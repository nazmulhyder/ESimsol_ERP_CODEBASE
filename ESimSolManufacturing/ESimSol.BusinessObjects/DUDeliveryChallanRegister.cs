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
    #region DUDeliveryChallanRegister
    public class DUDeliveryChallanRegister : BusinessObject
    {
        public DUDeliveryChallanRegister()
        {
            DUDeliveryChallanDetailID = 0;
            DUDeliveryChallanID = 0;
            ProductID = 0;
            //MUnitID = 0;
            Qty = 0;
            ChallanNo = "";
            BUID = 0;
            ChallanDate = DateTime.MinValue;
            //ChallanStatus = EnumChallanStatus.Initialized;
            ContractorID = 0;
            ContactPersonnelID = 0;
            DeliveryOrderID = 0;
            WorkingUnitID = 0;
            StoreInchargeID = 0;
            OrderType = EnumOrderType.None;
            ApproveBy = 0;
            ApproveDate = DateTime.MinValue;
          
            DeliveryToAddress = "";
            ReceivedByName = "";
            Remarks = "";
            ApproveByName = "";
            ProductCode = "";
            ProductName = "";
            LotNo = "";
            MUnit = "";
            ContractorName = "";
            SCPName = "";
            CPName = "";
            VehicleNo = "";
            VehicleName = "";
            GatePassNo = "";
            DONo = "";
            PINo = "";
            ExportLCNo = "";
            ErrorMessage = "";
            SearchingData = "";
            OrderNo = "";
            ColorName = "";
            UnitPrice = 0;
            ReportLayout = EnumReportLayout.None;
            ClaimOrderType = EnumClaimOrderType.None;
            DyeingOrderID = 0;
            DyeingOrderDetailID = 0;
            LotID = 0;
            NoCode = "";
        }

        #region Properties
        public int DUDeliveryChallanDetailID { get; set; }
        public int DyeingOrderDetailID { get; set; }
        public int DUDeliveryChallanID { get; set; }
        public int ProductID { get; set; }
        public double Qty { get; set; }
        public double UnitPrice { get; set; }
        public string ChallanNo { get; set; }
        public int BUID { get; set; }
        public DateTime ChallanDate { get; set; }
        public int LotID { get; set; }
        public int ContractorID { get; set; }
        public int ContactPersonnelID { get; set; }
        public int DeliveryOrderID { get; set; }
        public int DyeingOrderID { get; set; }
        public int WorkingUnitID { get; set; }
        public int StoreInchargeID { get; set; }
        public EnumOrderType OrderType { get; set; }
        public EnumClaimOrderType ClaimOrderType { get; set; }
        public int ApproveBy { get; set; }
        public DateTime ApproveDate { get; set; }    
        public string DeliveryToAddress { get; set; }
        public string ReceivedByName { get; set; }
        public string PINo { get; set; }
        public string LotNo { get; set; }
        public string LogNo { get; set; }
        public int Shade { get; set; }
        public string OrderNo { get; set; }
        public string NoCode { get; set; }
        public string ExportLCNo { get; set; }
        public string Remarks { get; set; }
        public string ApproveByName { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string MUnit { get; set; }
        public string ContractorName { get; set; }
        public string SCPName { get; set; }
        public string CPName { get; set; }
        public string VehicleNo { get; set; }
        public string VehicleName { get; set; }
        public string GatePassNo { get; set; }
        public string DONo { get; set; }
        public string ColorName { get; set; }
        public string ErrorMessage { get; set; }
        public string SearchingData { get; set; }
     
        public EnumReportLayout ReportLayout { get; set; }
        #endregion

        #region Derived Property
        public Double Qty_Order { get; set; }
        public Double Qty_Dyeing { get; set; }
        public string ChallanDateSt
        {
            get 
            {
                if (this.ChallanDate == DateTime.MinValue)
                {
                    return "--";
                }
                else
                {
                    return this.ChallanDate.ToString("dd MMM yyyy");
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
        #region
        private string _sOrderNoFull = "";
        public string OrderNoFull
        {
            get
            {
                _sOrderNoFull =this.NoCode+""+ this.OrderNo;
                //if (this.OrderType == EnumOrderType.SampleOrder)
                //{
                //    _sOrderNoFull = "BSY-" + this.OrderNo;
                //}
                //else if (this.OrderType == EnumOrderType.BulkOrder)
                //{
                //    _sOrderNoFull = "BPO-" + this.OrderNo;
                //}
                //else if (this.OrderType == EnumOrderType.DyeingOnly)
                //{
                //    _sOrderNoFull = "BRD-" + this.OrderNo;
                //}
                //else if (this.OrderType == EnumOrderType.Sampling)
                //{
                //    _sOrderNoFull = "SP-" + this.OrderNo;
                //}
                return _sOrderNoFull;
            }
        }
        #endregion
        
        #endregion

        #region Functions
        public static List<DUDeliveryChallanRegister> Gets(string sSQL, long nUserID)
        {
            return DUDeliveryChallanRegister.Service.Gets(sSQL, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IDUDeliveryChallanRegisterService Service
        {
            get { return (IDUDeliveryChallanRegisterService)Services.Factory.CreateService(typeof(IDUDeliveryChallanRegisterService)); }
        }
        #endregion

    }
    #endregion

    #region IDUDeliveryChallanRegister interface

    public interface IDUDeliveryChallanRegisterService
    {
        List<DUDeliveryChallanRegister> Gets(string sSQL, Int64 nUserID);
    }
    #endregion
}
