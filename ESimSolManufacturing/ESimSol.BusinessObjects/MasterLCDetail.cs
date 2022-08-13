using System;
using System.IO;
using ICS.Core;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using ICS.Core.Framework;
using ICS.Core.Utility;
using ESimSol.BusinessObjects.ReportingObject;

namespace ESimSol.BusinessObjects
{

    #region MasterLCDetail
    
    public class MasterLCDetail : BusinessObject
    {
        public MasterLCDetail()
        {
            MasterLCDetailID = 0;
            MasterLCDetailLogID = 0;
            MasterLCID = 0;
            MasterLCLogID = 0;
            ProformaInvoiceID = 0;
            PINo = "";
            PIStatus =EnumPIStatus.Initialized;
            BuyerID =0;
            PIIssueDate =DateTime.Now;
            PIQty = 0;
            BuyerName = "";
            Amount = 0;
            MasterLCNo = "";
            LCValue = 0;
            CurrencySymbol = "";
            ErrorMessage = "";
        }

        #region Properties

         public string MasterLCNo { get; set; }
          public double  LCValue { get; set; }
              
        public int MasterLCDetailID { get; set; }
         
        public int ProformaInvoiceID { get; set; }
         
        public int MasterLCID { get; set; }
         
        public int MasterLCLogID { get; set; }
         
        public string PINo { get; set; }
         
        public EnumPIStatus PIStatus { get; set; }
         
        public int BuyerID { get; set; }

        public DateTime PIIssueDate { get; set; }
         
        public double PIQty { get; set; }
         
        public double Amount { get; set; }
         
        public string BuyerName { get; set; }
         
        public int DEPT { get; set; }
         
        public int MasterLCDetailLogID { get; set; }
         
        public int ProformaInvoiceLogID { get; set; }

        public string CurrencySymbol { get; set; }
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property
        public int PIStatusInInt { get; set; }
        public string LCValueString
        {
            get
            {
                return this.CurrencySymbol+Global.MillionFormat(this.LCValue);
            }
        }
        public string PIStatusInString
        {
            get
            {
                return PIStatus.ToString();
            }
        }
        public string PIIssueDateInString
        {
            get
            {
                return PIIssueDate.ToString("dd MMM yyyy");
            }
        }
        public ProformaInvoice ProformaInvoice { get; set; }

        #endregion

        #region Functions

        public static List<MasterLCDetail> Gets(int ProformaInvoiceID, long nUserID)
        {
            return MasterLCDetail.Service.Gets(ProformaInvoiceID, nUserID);
        }

        public static List<MasterLCDetail> GetsMasterLCLog(int ProformaInvoiceLogID, long nUserID)
        {
            return MasterLCDetail.Service.GetsMasterLCLog(ProformaInvoiceLogID, nUserID);
        }

        public static List<MasterLCDetail> Gets(string sSQL, long nUserID)
        {
            return MasterLCDetail.Service.Gets(sSQL, nUserID);
        }
        public MasterLCDetail Get(int MasterLCDetailID, long nUserID)
        {           
            return MasterLCDetail.Service.Get(MasterLCDetailID, nUserID);
        }
        public MasterLCDetail GetByOrderRecap(int ORID, long nUserID)
        {
            return MasterLCDetail.Service.GetByOrderRecap(ORID, nUserID);
        }
        public MasterLCDetail Save(long nUserID)
        {
            return MasterLCDetail.Service.Save(this, nUserID);
        }
        public MasterLCDetail SaveMLCDetailByOrderTrack(long nUserID)
        {
            return MasterLCDetail.Service.SaveMLCDetailByOrderTrack(this, nUserID);
        }
        public MasterLCDetail AcceptPIReviseWithMLCDetail(long nUserID)
        {
            return MasterLCDetail.Service.AcceptPIReviseWithMLCDetail(this, nUserID);
        }
        public string DeleteMLCDeatil(int nMasterLCDetailID, int nProformaInvoiceID, long nUserID)
        {
            return MasterLCDetail.Service.DeleteMLCDeatil(nMasterLCDetailID, nProformaInvoiceID, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IMasterLCDetailService Service
        {
            get { return (IMasterLCDetailService)Services.Factory.CreateService(typeof(IMasterLCDetailService)); }
        }

        #endregion
    }
    #endregion

    #region IMasterLCDetail interface
    public interface IMasterLCDetailService
    {
        MasterLCDetail Get(int MasterLCDetailID, Int64 nUserID);
        MasterLCDetail GetByOrderRecap(int MasterLCDetailID, Int64 nUserID);
        List<MasterLCDetail> Gets(int ProformaInvoiceID, Int64 nUserID);
        List<MasterLCDetail> GetsMasterLCLog(int ProformaInvoiceLogID, Int64 nUserID);
        List<MasterLCDetail> Gets(string sSQL, Int64 nUserID);
        MasterLCDetail Save(MasterLCDetail oMasterLCDetail, Int64 nUserID);
        MasterLCDetail SaveMLCDetailByOrderTrack(MasterLCDetail oMasterLCDetail, Int64 nUserID);
        MasterLCDetail AcceptPIReviseWithMLCDetail(MasterLCDetail oMasterLCDetail, Int64 nUserID);
        string DeleteMLCDeatil(int nMasterLCDetailID, int nProformaInvoiceID, Int64 nUserID);
    }
    #endregion 
}
