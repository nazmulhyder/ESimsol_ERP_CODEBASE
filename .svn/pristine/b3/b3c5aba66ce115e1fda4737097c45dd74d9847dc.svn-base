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
    #region GUQC
    public class GUQC : BusinessObject
    {
        public GUQC()
        {
            GUQCID = 0;
            QCNo = "";
            QCDate = DateTime.Now;
            QCBy = 0;
            Remarks = "";
            BuyerID = 0;
            StoreID = 0;
            ApproveBy = 0;
            ApproveDate = DateTime.Now;
            ErrorMessage = "";
            //QCType = EnumProductNature.Hanger;
            GUQCDetails = new List<GUQCDetail>();
        }

        #region Property
        public int GUQCID { get; set; }
        public DateTime QCDate { get; set; }
        public int QCBy { get; set; }
        public int BUID { get; set; }
        public string Remarks { get; set; }
        public string QCNo { get; set; }
        public int BuyerID { get; set; }
        public int StoreID { get; set; }
        public int ApproveBy { get; set; }
        public DateTime ApproveDate { get; set; }
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property

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

        public List<GUQCDetail> GUQCDetails { get; set; }
        #endregion

        #region Functions
        public static List<GUQC> Gets(long nUserID)
        {
            return GUQC.Service.Gets(nUserID);
        }
        public static List<GUQC> Gets(string sSQL, long nUserID)
        {
            return GUQC.Service.Gets(sSQL, nUserID);
        }
        public GUQC Get(int id, long nUserID)
        {
            return GUQC.Service.Get(id, nUserID);
        }
        public GUQC Save(long nUserID)
        {
            return GUQC.Service.Save(this, nUserID);
        }
        public string Delete(int id, long nUserID)
        {
            return GUQC.Service.Delete(id, nUserID);
        }
        public GUQC Approve(long nUserID)
        {
            return GUQC.Service.Approve(this, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IGUQCService Service
        {
            get { return (IGUQCService)Services.Factory.CreateService(typeof(IGUQCService)); }
        }
        #endregion
    }
    #endregion

    #region IGUQC interface
    public interface IGUQCService
    {
        GUQC Get(int id, Int64 nUserID);
        List<GUQC> Gets(Int64 nUserID);
        List<GUQC> Gets(string sSQL, Int64 nUserID);
        string Delete(int id, Int64 nUserID);
        GUQC Save(GUQC oGUQC, Int64 nUserID);
        GUQC Approve(GUQC oGUQC, Int64 nUserID);

    }
    #endregion
}
