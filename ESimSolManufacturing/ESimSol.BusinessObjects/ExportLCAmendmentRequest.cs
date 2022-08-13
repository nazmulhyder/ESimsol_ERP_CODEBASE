using System;
using System.IO;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using System.Data;
using ICS.Core.Framework;
using ICS.Core.Utility;

namespace ESimSol.BusinessObjects
{
    #region ExportLCAmendmentRequest
    public class ExportLCAmendmentRequest : BusinessObject
    {
        #region  Constructor
        public ExportLCAmendmentRequest()
        {
            ExportLCAmendmentRequestID = 0;
            ExportLCID = 0;
          
            DateOfRequest = DateTime.Today;
            Sequence = 0;
            RequestByID = 0;
            DateOfReceive = DateTime.Today;
            ExportLCAmendmentClauses = new List<ExportLCAmendmentClause>();
        }
        #endregion

        #region Properties
        public int ExportLCAmendmentRequestID { get; set; }
        public int ExportLCID { get; set; }
        public DateTime DateOfRequest { get; set; }
        public int Sequence { get; set; }
        public int RequestByID { get; set; }
        public int ReceivedByID { get; set; }
        public DateTime DateOfReceive { get; set; }
        public List<ExportLCAmendmentClause> ExportLCAmendmentClauses { get; set; }
        public ExportLC ExportLC { get; set; }
     
        public string ErrorMessage { get; set; }
         public string DateOfRequestSt
         {
             get
             {
                 return DateOfRequest.ToString("dd MMM yyyy");
             }
         }
         public string Sequencest
         {
             get
             {
                 return ((EnumNumericOrder)Sequence).ToString();
             }
         }
         private string _sDateOfReceiveSt = "";
         public string DateOfReceiveSt
         {
             get
             {
                 if (DateOfReceive==DateTime.MinValue)
                 {
                     _sDateOfReceiveSt = "Yet not received Amendment Copy";
                 }
                 else
                 {
                     _sDateOfReceiveSt = this.DateOfReceive.ToString("dd MMM yyyy");
                 }
                 return _sDateOfReceiveSt;
             }
         }
        #endregion

        #region Functions
         public ExportLCAmendmentRequest Get(int id, Int64 nUserID)
         {
             return ExportLCAmendmentRequest.Service.Get(id, nUserID);
         }
         public ExportLCAmendmentRequest Save(Int64 nUserID)
         {
             return ExportLCAmendmentRequest.Service.Save(this, nUserID);
         }
         public string Delete(Int64 nUserID)
         {
             return ExportLCAmendmentRequest.Service.Delete(this, nUserID);
         }
      
        public static List<ExportLCAmendmentRequest> Gets(int nExportLCID, Int64 nUserID)
        {
            return ExportLCAmendmentRequest.Service.Gets(nExportLCID, nUserID);
        }
      
        #endregion
        #region ServiceFactory

      
        internal static IExportLCAmendmentRequestService Service
        {
            get { return (IExportLCAmendmentRequestService)Services.Factory.CreateService(typeof(IExportLCAmendmentRequestService)); }
        }
        #endregion
    }
    #endregion

    
    #region IExportLCAmendmentRequest interface
    [ServiceContract]
    public interface IExportLCAmendmentRequestService
    {
        ExportLCAmendmentRequest Get(int nID, Int64 nUserID);
        List<ExportLCAmendmentRequest> Gets(int nExportLCID, Int64 nUserID);
        ExportLCAmendmentRequest Save(ExportLCAmendmentRequest oExportLCAmendmentRequest, Int64 nUserID);
        string Delete(ExportLCAmendmentRequest oExportLCAmendmentRequest, Int64 nUserID);
    }
    #endregion

}
