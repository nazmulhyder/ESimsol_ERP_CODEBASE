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
    #region Shipment
    public class Shipment : BusinessObject
    {
        public Shipment()
        {
            ShipmentID = 0;
            ChallanNo = "";
            BUID = 0;
            ShipmentDate = DateTime.Now;
            ShipmentMode = EnumShipmentMode.None;
            BuyerID = 0;
            StoreID = 0;
            TruckNo = "";
            DriverName = "";
            DriverMobileNo = "";
            Depo = "";
            Escord = "";
            ApprovedBy = 0;
            FactoryName = "";
            SecurityLock = "";
            EmptyCTNQty = 0;
            GumTapeQty = 0;
            Remarks = "";
            ErrorMessage = "";
            ShipmentDetails = new List<ShipmentDetail>();
        }

        #region Property
        public int ShipmentID { get; set; }
        public string ChallanNo { get; set; }
        public int BUID { get; set; }
        public int BuyerID { get; set; }
        public int StoreID { get; set; }
        public DateTime ShipmentDate { get; set; }
        public EnumShipmentMode ShipmentMode { get; set; }
        public string TruckNo { get; set; }
        public string DriverName { get; set; }
        public string DriverMobileNo { get; set; }
        public string Depo { get; set; }
        public string Escord { get; set; }
        public int ApprovedBy { get; set; }
        public string FactoryName { get; set; }
        public string SecurityLock { get; set; }
        public int EmptyCTNQty { get; set; }
        public int GumTapeQty { get; set; }
        public string Remarks { get; set; }
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property

        public string BuyerName { get; set; }
        public string ApproveByName { get; set; }
        public string StoreName { get; set; }

        public string ShipmentDateInString
        {
            get
            {
                return ShipmentDate.ToString("dd MMM yyyy");
            }
        }
        public string ShipmentModeInString
        {
            get
            {
                return EnumObject.jGet(this.ShipmentMode);
            }
        }
        public int ShipmentModeInt
        {
            get
            {
                return (int)ShipmentMode;
            }
        }

        public List<ShipmentDetail> ShipmentDetails { get; set; }
        #endregion

        #region Functions
        public static List<Shipment> Gets(long nUserID)
        {
            return Shipment.Service.Gets(nUserID);
        }
        public static List<Shipment> Gets(string sSQL, long nUserID)
        {
            return Shipment.Service.Gets(sSQL, nUserID);
        }
        public Shipment Get(int id, long nUserID)
        {
            return Shipment.Service.Get(id, nUserID);
        }
        public Shipment Save(long nUserID)
        {
            return Shipment.Service.Save(this, nUserID);
        }
        public string Delete(int id, long nUserID)
        {
            return Shipment.Service.Delete(id, nUserID);
        }
        public Shipment Approve(long nUserID)
        {
            return Shipment.Service.Approve(this, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IShipmentService Service
        {
            get { return (IShipmentService)Services.Factory.CreateService(typeof(IShipmentService)); }
        }
        #endregion
    }
    #endregion

    #region IShipment interface
    public interface IShipmentService
    {
        Shipment Get(int id, Int64 nUserID);
        List<Shipment> Gets(Int64 nUserID);
        List<Shipment> Gets(string sSQL, Int64 nUserID);
        string Delete(int id, Int64 nUserID);
        Shipment Save(Shipment oShipment, Int64 nUserID);
        Shipment Approve(Shipment oGUQC, Int64 nUserID);

    }
    #endregion
}
