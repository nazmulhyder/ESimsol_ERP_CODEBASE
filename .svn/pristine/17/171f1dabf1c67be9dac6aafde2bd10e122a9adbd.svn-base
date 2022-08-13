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
    #region FGQC
    public class FGQC : BusinessObject
    {
        public FGQC()
        {
            FGQCID = 0;
            FGQCNo = "";
            FGQCDate = DateTime.Today;
            BUID = 0;
            ApprovedBy = 0;
            Remarks = "";
            ApprovedByName = "";
            BUName = "";
            BUShortName = "";
            FGQCAmount = 0;
            FGQCDetails = new List<FGQCDetail>();
            ErrorMessage = "";
        }

        #region Property
        public int FGQCID { get; set; }
        public string FGQCNo { get; set; }
        public DateTime FGQCDate { get; set; }
        public int BUID { get; set; }
        public int ApprovedBy { get; set; }
        public string Remarks { get; set; }
        public string ApprovedByName { get; set; }
        public string BUName { get; set; }
        public string BUShortName { get; set; }
        public double FGQCAmount { get; set; }
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property
        public List<FGQCDetail> FGQCDetails { get; set; }
        public string FGQCDateST
        {
            get
            {
                return FGQCDate.ToString("dd MMM yyyy");
            }
        }
        #endregion

        #region Functions
        public static List<FGQC> Gets(long nUserID)
        {
            return FGQC.Service.Gets(nUserID);
        }
        public static List<FGQC> Gets(string sSQL, long nUserID)
        {
            return FGQC.Service.Gets(sSQL, nUserID);
        }
        public FGQC Get(int id, long nUserID)
        {
            return FGQC.Service.Get(id, nUserID);
        }
        public FGQC Save(long nUserID)
        {
            return FGQC.Service.Save(this, nUserID);
        }
        public FGQC GetSuggestFGQCDate(string sSQl, long nUserID)
        {
            return FGQC.Service.GetSuggestFGQCDate(sSQl, nUserID);
        }
        public FGQC Approved(long nUserID)
        {
            return FGQC.Service.Approved(this, nUserID);
        }
        public string Delete(int id, long nUserID)
        {
            return FGQC.Service.Delete(id, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IFGQCService Service
        {
            get { return (IFGQCService)Services.Factory.CreateService(typeof(IFGQCService)); }
        }
        #endregion
    }
    #endregion

    #region IFGQC interface
    public interface IFGQCService
    {
        FGQC Get(int id, Int64 nUserID);
        List<FGQC> Gets(Int64 nUserID);
        List<FGQC> Gets(string sSQL, Int64 nUserID);
        string Delete(int id, Int64 nUserID);
        FGQC Save(FGQC oFGQC, Int64 nUserID);
        FGQC GetSuggestFGQCDate(string sSQl, Int64 nUserID);
        FGQC Approved(FGQC oFGQC, Int64 nUserID);
    }
    #endregion
}