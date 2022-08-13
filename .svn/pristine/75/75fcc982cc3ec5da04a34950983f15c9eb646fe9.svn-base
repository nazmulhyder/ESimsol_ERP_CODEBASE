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
    #region GUQCRegister
    public class GUQCRegister : BusinessObject
    {
        public GUQCRegister()
        {
            GUQCDetailID = 0;
            GUQCID = 0;
            OrderRecapID = 0;
            QCPassQty = 0;
            RejectQty = 0;
            Remarks = "";

            QCNo = "";
            QCDate = DateTime.Now;
            QCBy = 0;
            BuyerID = 0;
            StoreID = 0;
            ApproveBy = 0;
            ApproveDate = DateTime.Now;
            ReportLayout = EnumReportLayout.None;
            ShipmentDate = DateTime.MinValue;
            ErrorMessage = "";
        }

        #region Property
        public int GUQCDetailID { get; set; }
        public int GUQCID { get; set; }
        public int OrderRecapID { get; set; }
        public int QCPassQty { get; set; }
        public int RejectQty { get; set; }
        public string Remarks { get; set; }

        public DateTime QCDate { get; set; }
        public int QCBy { get; set; }
        public int BUID { get; set; }
        public string QCNo { get; set; }
        public int BuyerID { get; set; }
        public int StoreID { get; set; }
        public int ApproveBy { get; set; }
        public DateTime ApproveDate { get; set; }
        public string POWiseRemarks { get; set; }
        public DateTime ShipmentDate { get; set; }
        public string ErrorMessage { get; set; }
        public EnumReportLayout ReportLayout { get; set; }
        #endregion

        #region Derived Property
        public string StyleNo { get; set; }
        public string OrderRecapNo { get; set; }
        public int TechnicalSheetID { get; set; }
        public int TotalQuantity { get; set; }
        public int AlredyQCQty { get; set; }
        public int YetToQCQty { get; set; }


        public string QCByName { get; set; }
        public string ApproveByName { get; set; }
        public string BuyerName { get; set; }
        public string StoreName { get; set; }
        public string QCDateInString
        {
            get
            {
                if (QCDate == DateTime.MinValue) return "";
                return QCDate.ToString("dd MMM yyyy");
            }
        }
        public string ApproveDateInString
        {
            get
            {
                if (ApproveDate == DateTime.MinValue) return "";
                return ApproveDate.ToString("dd MMM yyyy");
            }
        }
        public string ShipmentDateInString
        {
            get
            {
                if (ShipmentDate == DateTime.MinValue) return "";
                return ShipmentDate.ToString("dd MMM yyyy");
            }
        }
        #endregion

        #region Functions
        public static List<GUQCRegister> Gets(int nGUQCID, long nUserID)
        {
            return GUQCRegister.Service.Gets(nGUQCID, nUserID);
        }
        public static List<GUQCRegister> Gets(string sSQL, long nUserID)
        {
            return GUQCRegister.Service.Gets(sSQL, nUserID);
        }
        public GUQCRegister Get(int id, long nUserID)
        {
            return GUQCRegister.Service.Get(id, nUserID);
        }

        #endregion

        #region ServiceFactory
        internal static IGUQCRegisterService Service
        {
            get { return (IGUQCRegisterService)Services.Factory.CreateService(typeof(IGUQCRegisterService)); }
        }
        #endregion

        public List<GUQCRegister> GUQCRegisters { get; set; }
    }
    #endregion

    #region IGUQCRegister interface
    public interface IGUQCRegisterService
    {
        GUQCRegister Get(int id, Int64 nUserID);
        List<GUQCRegister> Gets(int nGUQCID, Int64 nUserID);
        List<GUQCRegister> Gets(string sSQL, Int64 nUserID);
    }
    #endregion
}
