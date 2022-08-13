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
    #region ExportDocTnC
    public class ExportDocTnC : BusinessObject
    {
        public ExportDocTnC()
        {
            ExportDocTnCID = 0;
            ExportTRID = 0;
            ReferenceID = 0;
            IsPrintGrossNetWeight = false;
            IsPrintOriginal = false;
            DeliveryBy = "";
            IsDeliveryBy = false;
            Term = "";
            IsTerm = false;
            ErrorMessage = "";
            Certification = "";
            GRPNonDate = "";
            TruckReceiptNo = "";
            TruckReceiptDate = DateTime.Now;
            Carrier = "";
            TruckNo = "";
            DriverName = "";
            MeasurementCarton = 0;
            PerCartonWeight = 0;
            NotifyBy = EnumNotifyBy.Party_Bank;
            NotifyByInt = (int)EnumNotifyBy.Party_Bank;
            RefType = EnumMasterLCType.ExportLC;
            ExportPartyInfoBills = new List<ExportPartyInfoBill>();
            ExportDocForwardings = new List<ExportDocForwarding>();
        }

        #region Properties
        public EnumMasterLCType RefType { get; set; }
        public int ExportDocTnCID { get; set; }
        public int ExportTRID { get; set; }
        public int ReferenceID { get; set; }
        public bool IsPrintGrossNetWeight { get; set; }
        public bool IsPrintOriginal { get; set; }
        public string DeliveryBy { get; set; }
        public bool IsDeliveryBy { get; set; }
        public EnumNotifyBy NotifyBy { get; set; }
        public int NotifyByInt { get; set; }
        public string Term { get; set; }
        public bool IsTerm { get; set; }
        public string ErrorMessage { get; set; }
        public string CTPApplicant { get; set; }
        public string Certification { get; set; }
        public string GRPNonDate { get; set; }
        public string TruckReceiptNo { get; set; }
        public DateTime TruckReceiptDate { get; set; }
        public string Carrier { get; set; }
        public double MeasurementCarton { get; set; }        
        public double PerCartonWeight { get; set; }
        public string TruckNo { get; set; }
        public string DriverName { get; set; }
        public List<ExportTR> ExportTRs { get; set; }
        public ExportLC ExportLC { get; set; }
        #endregion

        #region Derived Property
        public int RefTypeInInt { get; set; }

        public List<ExportPartyInfoBill> ExportPartyInfoBills { get; set; }
        public List<ExportDocForwarding> ExportDocForwardings { get; set; }
        public string TruckReceiptDateInString
        {
            get
            {
                return this.TruckReceiptDate.ToString("dd MMM yyyy");
            }
        }
        #endregion

        #region Functions
        public ExportDocTnC Get(int nId, Int64 nUserID)
        {
            return ExportDocTnC.Service.Get(nId, nUserID);
        }
        public static List<ExportDocTnC> Gets( Int64 nUserID)
        {
            return ExportDocTnC.Service.Gets( nUserID);
        }

        public static List<ExportDocTnC> Gets(string sSQL,Int64 nUserID)
        {
            return ExportDocTnC.Service.Gets(sSQL,nUserID);
        }
     
        public ExportDocTnC GetByLCID(int nReferenceID, int nRefType, Int64 nUserID)
        {
            return ExportDocTnC.Service.GetByLCID(nReferenceID, nRefType, nUserID);
        }
      
        public ExportDocTnC Save(Int64 nUserID)
        {
            return ExportDocTnC.Service.Save(this, nUserID);
        }
        public string Delete(Int64 nUserID)
        {
            return ExportDocTnC.Service.Delete(this, nUserID);
        }

        #endregion

        #region ServiceFactory

        internal static IExportDocTnCService Service
        {
            get { return (IExportDocTnCService)Services.Factory.CreateService(typeof(IExportDocTnCService)); }
        }
        #endregion
    }
    #endregion

    #region IExportDocTnC interface
 
    public interface IExportDocTnCService
    {
     
        ExportDocTnC Get(int id, Int64 nUserID);
        ExportDocTnC GetByLCID(int nReferenceID, int nnRefType, Int64 nUserID);
        List<ExportDocTnC> Gets(Int64 nUserID);
        List<ExportDocTnC> Gets(string sSQL, Int64 nUserID);
        string Delete(ExportDocTnC oExportDocTnC, Int64 nUserID);
        ExportDocTnC Save(ExportDocTnC oExportDocTnC, Int64 nUserID);
    }
    #endregion
}
