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
    #region ShipmentRegister
    public class ShipmentRegister : BusinessObject
    {
        public ShipmentRegister()
        {
            ShipmentDetailID = 0;
            ShipmentID = 0;
            OrderRecapID = 0;
            LotID = 0;
            CountryID = 0;
            ShipmentQty = 0;
            CTNQty = 0;
            DetailRemarks = "";

            ChallanNo = "";
            ShipmentDate = DateTime.Now;
            BuyerID = 0;
            StoreID = 0;
            ShipmentMode = EnumShipmentMode.None;
            DriverName = "";
            TruckNo = "";
            DriverMobileNo = "";
            Depo = "";
            Escord = "";
            ApprovedBy = 0;
            FactoryName = "";
            SecurityLock = "";
            EmptyCTNQty = 0;
            GumTapeQty = 0;
            Remarks = "";

            ReportLayout = EnumReportLayout.None;

            ErrorMessage = "";
        }

        #region Property
        public int ShipmentDetailID { get; set; }
        public int ShipmentID { get; set; }
        public int OrderRecapID { get; set; }
        public int LotID { get; set; }
        public int CountryID { get; set; }
        public int ShipmentQty { get; set; }
        public int CTNQty { get; set; }
        public string DetailRemarks { get; set; }
        public string OrderRecapNo { get; set; }
        public int CartonQty { get; set; }
        public string StyleNo { get; set; }
        public int TotalQuantity { get; set; }
        public string CountryName { get; set; }
        public string CountryShortName { get; set; }
        public int Balance { get; set; }
        public string LotNo { get; set; }

        public string ChallanNo { get; set; }
        public DateTime ShipmentDate { get; set; }
        public int BUID { get; set; }
        public int BuyerID { get; set; }
        public int StoreID { get; set; }
        public EnumShipmentMode ShipmentMode { get; set; }
        public string DriverName { get; set; }
        public string TruckNo { get; set; }
        public string DriverMobileNo { get; set; }
        public string Depo { get; set; }
        public string Escord { get; set; }
        public int ApprovedBy { get; set; }
        public string FactoryName { get; set; }
        public string SecurityLock { get; set; }
        public int EmptyCTNQty { get; set; }
        public int GumTapeQty { get; set; }
        public string Remarks { get; set; }
        public string BuyerName { get; set; }
        public string StoreName { get; set; }
        public string ApproveByName { get; set; }

        public string ErrorMessage { get; set; }
        public EnumReportLayout ReportLayout { get; set; }
        #endregion

        #region Derived Property
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
        #endregion

        #region Functions
        public static List<ShipmentRegister> Gets(int nShipmentID, long nUserID)
        {
            return ShipmentRegister.Service.Gets(nShipmentID, nUserID);
        }
        public static List<ShipmentRegister> Gets(string sSQL, long nUserID)
        {
            return ShipmentRegister.Service.Gets(sSQL, nUserID);
        }
        public ShipmentRegister Get(int id, long nUserID)
        {
            return ShipmentRegister.Service.Get(id, nUserID);
        }

        #endregion

        #region ServiceFactory
        internal static IShipmentRegisterService Service
        {
            get { return (IShipmentRegisterService)Services.Factory.CreateService(typeof(IShipmentRegisterService)); }
        }
        #endregion

        public List<ShipmentRegister> ShipmentRegisters { get; set; }
    }
    #endregion

    #region IShipmentRegister interface
    public interface IShipmentRegisterService
    {
        ShipmentRegister Get(int id, Int64 nUserID);
        List<ShipmentRegister> Gets(int nShipmentID, Int64 nUserID);
        List<ShipmentRegister> Gets(string sSQL, Int64 nUserID);
    }
    #endregion
}
