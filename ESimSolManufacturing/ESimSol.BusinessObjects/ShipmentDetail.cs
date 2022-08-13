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
    #region ShipmentDetail
    public class ShipmentDetail : BusinessObject
    {
        public ShipmentDetail()
        {
            ShipmentDetailID = 0;
            ShipmentID = 0;
            OrderRecapID = 0;
            LotID = 0;
            CountryID = 0;
            ShipmentQty = 0;
            CTNQty = 0;
            Remarks = "";
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
        public string Remarks { get; set; }

        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property
        public string OrderRecapNo { get; set; }
        public int CartonQty { get; set; }
        public string StyleNo { get; set; }
        public int TotalQuantity { get; set; }
        public string CountryName { get; set; }
        public string CountryShortName { get; set; }
        public int AlreadyShipmentQty { get; set; }
        public int YetToShipmentQty { get; set; }
        public int Balance { get; set; }
        public string LotNo { get; set; }
        #endregion

        #region Functions
        public static List<ShipmentDetail> Gets(int nShipmentID, long nUserID)
        {
            return ShipmentDetail.Service.Gets(nShipmentID, nUserID);
        }
        public static List<ShipmentDetail> Gets(string sSQL, long nUserID)
        {
            return ShipmentDetail.Service.Gets(sSQL, nUserID);
        }
        public ShipmentDetail Get(int id, long nUserID)
        {
            return ShipmentDetail.Service.Get(id, nUserID);
        }

        #endregion

        #region ServiceFactory
        internal static IShipmentDetailService Service
        {
            get { return (IShipmentDetailService)Services.Factory.CreateService(typeof(IShipmentDetailService)); }
        }
        #endregion

        public List<ShipmentDetail> ShipmentDetails { get; set; }
    }
    #endregion

    #region IShipmentDetail interface
    public interface IShipmentDetailService
    {
        ShipmentDetail Get(int id, Int64 nUserID);
        List<ShipmentDetail> Gets(int nShipmentID, Int64 nUserID);
        List<ShipmentDetail> Gets(string sSQL, Int64 nUserID);

    }
    #endregion
}
