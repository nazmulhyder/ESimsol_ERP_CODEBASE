using System;
using System.IO;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using ICS.Core.Framework;

namespace ESimSol.BusinessObjects
{
    #region ExportPartyInfoBill
    public class ExportPartyInfoBill : BusinessObject
    {
        public ExportPartyInfoBill()
        {
            ExportPartyInfoBillID = 0;
            ReferenceID = 0;
            ExportPartyInfoID = 0;
            PartyInfo = "";
            RefNo = "";
            RefDate = "";
            Selected = false;
            ExportPartyInfoDetailID = 0;
            RefType = EnumMasterLCType.ExportLC;
            RefTypeInInt = 0;
            ErrorMessage = "";
            ExportPartyInfoBills = new List<ExportPartyInfoBill>();
        }

        #region Properties
        public int ExportPartyInfoBillID { get; set; }
        public int ReferenceID { get; set; }
        public EnumMasterLCType RefType { get; set; }
        public int RefTypeInInt { get; set; }
        public int ExportPartyInfoID { get; set; }
        public int ExportPartyInfoDetailID  { get; set; }
        public string PartyInfo { get; set; }
        public string RefNo { get; set; }
        public string RefDate { get; set; }
        public bool Selected { get; set; }
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Properties
        public List<ExportPartyInfoBill> ExportPartyInfoBills { get; set; }
        #endregion

        #region Functions
        public ExportPartyInfoBill Get(int id, int nUserID)
        {
            return ExportPartyInfoBill.Service.Get(id, nUserID);
        }
        public ExportPartyInfoBill Save(int nUserID)
        {
            return ExportPartyInfoBill.Service.Save(this, nUserID);
        }
        public string Delete(int id, int nUserID)
        {
            return ExportPartyInfoBill.Service.Delete(id, nUserID);
        }
        public static List<ExportPartyInfoBill> Gets(int nUserID)
        {
            return ExportPartyInfoBill.Service.Gets(nUserID);
        }
        public static List<ExportPartyInfoBill> Gets(string sSQL, int nUserID)
        {
            return ExportPartyInfoBill.Service.Gets(sSQL, nUserID);
        }
        public static List<ExportPartyInfoBill> GetsByExportLC(int nReferenceID, int nRefType, int nUserID)
        {
            return ExportPartyInfoBill.Service.GetsByExportLC(nReferenceID, nRefType, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IExportPartyInfoBillService Service
        {
            get { return (IExportPartyInfoBillService)Services.Factory.CreateService(typeof(IExportPartyInfoBillService)); }
        }
        #endregion
    }
    #endregion

    #region IExportPartyInfoBill interface
    public interface IExportPartyInfoBillService
    {
        ExportPartyInfoBill Get(int id, int nUserID);
        List<ExportPartyInfoBill> Gets(int nUserID);
        string Delete(int id, int nUserID);
        ExportPartyInfoBill Save(ExportPartyInfoBill oExportPartyInfoBill, int nUserID);
        List<ExportPartyInfoBill> Gets(string sSQL, int nUserID);
        List<ExportPartyInfoBill> GetsByExportLC(int nReferenceID,int nRefType, int nUserID);
    }
    #endregion
}