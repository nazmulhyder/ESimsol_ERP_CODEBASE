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
   
        #region InspectionCertificateDetail
    
    public class InspectionCertificateDetail : BusinessObject
    {
        public InspectionCertificateDetail()
        {
            
            InspectionCertificateDetailID = 0;
            InspectionCertificateID  = 0;
            CommercialInvoiceDetailID  = 0;
            OrderQty  = 0;
            ShipedQty = 0;
            CartonQty = 0;
            StyleNo = "";
            OrderRecapNo = "";
            OrderNo = ""; 
            ErrorMessage = "";
        }

        #region Properties
         
        public int InspectionCertificateDetailID { get; set; }
         
        public int InspectionCertificateID { get; set; }
         
        public int CommercialInvoiceDetailID { get; set; }
         
        public double OrderQty { get; set; }
         
        public double ShipedQty { get; set; }
         
        public double CartonQty { get; set; }
         
        public string StyleNo { get; set; }
         
        public string OrderRecapNo { get; set; }
         
        public string OrderNo { get; set; }
         
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property

        public string StyleNoOrderNo
        {
            get
            {
                return this.StyleNo + " /" + this.OrderNo;
            }
        }

        #endregion

        #region Functions

        public static List<InspectionCertificateDetail> Gets(int ProformaInvoiceID, long nUserID)
        {
            return InspectionCertificateDetail.Service.Gets(ProformaInvoiceID, nUserID);
        }
        public static List<InspectionCertificateDetail> Gets(string sSQL, long nUserID)
        {
            return InspectionCertificateDetail.Service.Gets(sSQL, nUserID);
        }
        public InspectionCertificateDetail Get(int InspectionCertificateDetailID, long nUserID)
        {

            return InspectionCertificateDetail.Service.Get(InspectionCertificateDetailID, nUserID);
        }
        public InspectionCertificateDetail Save(long nUserID)
        {
            return InspectionCertificateDetail.Service.Save(this, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IInspectionCertificateDetailService Service
        {
            get { return (IInspectionCertificateDetailService)Services.Factory.CreateService(typeof(IInspectionCertificateDetailService)); }
        }

        #endregion
    }
    #endregion

    #region IInspectionCertificateDetail interface
     
    public interface IInspectionCertificateDetailService
    {
         
        InspectionCertificateDetail Get(int InspectionCertificateDetailID, Int64 nUserID);
         
        List<InspectionCertificateDetail> Gets(int ProformaInvoiceID, Int64 nUserID);
         
        List<InspectionCertificateDetail> Gets(string sSQL, Int64 nUserID);
         
        InspectionCertificateDetail Save(InspectionCertificateDetail oInspectionCertificateDetail, Int64 nUserID);
    }
    #endregion
    

}
