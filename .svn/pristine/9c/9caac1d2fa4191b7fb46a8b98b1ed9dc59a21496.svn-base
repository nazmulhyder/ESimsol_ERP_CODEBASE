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


    #region InspectionCertificate
    
    public class InspectionCertificate : BusinessObject
    {
        public InspectionCertificate()
        {
           
            InspectionCertificateID = 0;
            RefNo = "";
            ICDate = DateTime.Now;
            CommercialInvoiceID = 0;
            ShipperID = 0;
            ManufacturerID = 0;
            InvoiceNo  = "";
            InvoiceDate = DateTime.Now;
            InvoiceValue = 0;
            MasterLCNo  = "";
            MasterLCDate = DateTime.Now;
            BillOfLadingNo  = "";
            BillOfLadingDate = DateTime.Now;
            Vessel  = "";
            PortOfLoading  = "";
            FinalDestination  = "";
            Remarks  = "";
            AuthorizeCompany = 0;
            ShipperName  = "";
            MenufacturerName  = "";
            CompanyName = "";
            ShipmentMode = EnumTransportType.None;
            ErrorMessage = "";
            InspectionCertificates = new List<InspectionCertificate>();
            
        }

        #region Properties
         
        public int InspectionCertificateID { get; set; }
         
        public string RefNo { get; set; }
         
        public DateTime ICDate { get; set; }
         
        public int CommercialInvoiceID { get; set; }
         
        public int ShipperID { get; set; }
         
        public int  ManufacturerID { get; set; }
         
        public string InvoiceNo { get; set; }
         
        public DateTime InvoiceDate { get; set; }
         
        public double InvoiceValue { get; set; }
         
        public DateTime MasterLCDate { get; set; }
         
        public string BillOfLadingNo { get; set; }
         
        public DateTime BillOfLadingDate { get; set; }
         
        public string Vessel { get; set; }
         
        public string PortOfLoading { get; set; }
         
        public EnumTransportType ShipmentMode { get; set; }

         
        public string FinalDestination { get; set; }
         
        public string Remarks { get; set; }
         
        public int AuthorizeCompany { get; set; }
         
        public string ShipperName { get; set; }
         
        public string MenufacturerName { get; set; }
         
        public string CompanyName { get; set; }
        
         
        public string MasterLCNo { get; set; }
         
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property
         
        public int InvoiceStatusInInt { get; set; }

         
        public string ActionTypeInString { get; set; }
         
        public List<InspectionCertificateDetail> InspectionCertificateDetails { get; set; }
        public List<InspectionCertificate> InspectionCertificates { get; set; }
        public List<Company> Companies { get; set; }

        public CommercialInvoice CommercialInvoice { get; set; }
        public string ShipmentModeInString
        {
            get
            {
                return this.ShipmentMode.ToString();
            }
        }
        public string InvoiceDateInString
        {
            get
            {
                return this.InvoiceDate.ToString("dd MMM yyyy");
            }
        }



        public string ICDateInString
        {
            get
            {
                return this.ICDate.ToString("dd MMM yyyy");
            }
        }

        public string MasterLCDateInString
        {
            get
            {
                

                    return this.MasterLCDate.ToString("dd MMM yyyy");
                
            }
        }
        public string BillOfLadingDateInString
        {
            get
            {

                    return this.BillOfLadingDate.ToString("dd MMM yyyy");

            }
        }
       

        #endregion

        #region Functions

        public static List<InspectionCertificate> Gets(long nUserID)
        {
            return InspectionCertificate.Service.Gets(nUserID);
        }

        public static List<InspectionCertificate> Gets(int id, long nUserID)
        {
            return InspectionCertificate.Service.Gets(id, nUserID);
        }

        public static List<InspectionCertificate> Gets(string sSQL, long nUserID)
        {
            return InspectionCertificate.Service.Gets(sSQL, nUserID);
        }

        public InspectionCertificate Get(int id, long nUserID) //id is commercial Invoice ID
        {
            return InspectionCertificate.Service.Get(id, nUserID);
        }

        public InspectionCertificate GetIC(int id, long nUserID) //id is IC  ID
        {
            return InspectionCertificate.Service.GetIC(id, nUserID);
        }

        public InspectionCertificate Save(long nUserID)
        {
            return InspectionCertificate.Service.Save(this, nUserID);
        }

        public string Delete(int nInspectionCertificateID, long nUserID)
        {
            return InspectionCertificate.Service.Delete(nInspectionCertificateID, nUserID);
        }

        #endregion

        #region ServiceFactory

        internal static IInspectionCertificateService Service
        {
            get { return (IInspectionCertificateService)Services.Factory.CreateService(typeof(IInspectionCertificateService)); }
        }

        #endregion
    }
    #endregion

    #region IInspectionCertificate interface
     
    public interface IInspectionCertificateService
    {
         
        InspectionCertificate Get(int id, Int64 nUserID);

         
        InspectionCertificate GetIC(int id, Int64 nUserID);
        
         
        List<InspectionCertificate> Gets(Int64 nUserID);
         
        List<InspectionCertificate> Gets(int id, Int64 nUserID);
         
        List<InspectionCertificate> Gets(string sSQL, Int64 nUserID);
         
        InspectionCertificate Save(InspectionCertificate oInspectionCertificate, Int64 nUserID);
         
        string Delete(int nInspectionCertificateID, Int64 nUserID);
    }
    #endregion
    
    
 
}
