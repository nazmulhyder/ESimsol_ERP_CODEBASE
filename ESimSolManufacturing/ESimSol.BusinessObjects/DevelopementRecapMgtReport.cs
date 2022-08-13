using System;
using System.IO;
using ICS.Core;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using ICS.Core.Framework;
using ICS.Core.Utility;
namespace ESimSol.BusinessObjects
{

    #region DevelopementRecapMgtReport
    
    public class DevelopementRecapMgtReport : BusinessObject
    {
        public DevelopementRecapMgtReport()
        {
            DevelopmentRecapID = 0;
            DevelopmentRecapDetailID = 0;
            DevelopmentRecapNo = "";
            TechnicalSheetID = 0;
            MerchandiserID = 0;
            DevelopmentType = 0;
            DevelopmentTypeName = "";
            InquiryReceivedDate = DateTime.Now;
            SendingDeadLine = DateTime.Now;
            BuyerID = 0;
            FactoryID = 0;
            ProductCategoryID = 0;
            StyleNo = "";
            BuyerName = "";
            FactoryName = "";
            MerchandiserName = "";
            ProductCategoryName = "";
            SampleQty = 0;
            OrderQty = 0;
            ErrorMessage = "";

        }

        #region Properties
         
        public int DevelopmentRecapID { get; set; }
         
        public int DevelopmentRecapDetailID { get; set; }
         
        public string DevelopmentRecapNo { get; set; }
         
        public int TechnicalSheetID { get; set; }
         
        public int MerchandiserID { get; set; }
         
        public int DevelopmentType { get; set; }
         
        public DateTime InquiryReceivedDate { get; set; }
         
        public DateTime SendingDeadLine { get; set; }
         
        public int ProductCategoryID { get; set; }
         
        public string StyleNo { get; set; }
         
        public string BuyerName { get; set; }
         
        public string FactoryName { get; set; }
         
        public string MerchandiserName { get; set; }
         
        public string ProductCategoryName { get; set; }
         
        public double SampleQty { get; set; }
         
        public double OrderQty { get; set; }
         
        public int BuyerID { get; set; }
        public string DevelopmentTypeName { get; set; }
        public int FactoryID { get; set; }
         
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property

         
        public List<DevelopementRecapMgtReport> DevelopementRecapMgtReports { get; set; }
       
        public string InquiryReceivedDateInString
        {
            get
            {
                if (this.InquiryReceivedDate == DateTime.MinValue)
                {
                    return " ";
                }
                else
                {
                    return this.InquiryReceivedDate.ToString("dd MMM yyyy");
                }
            }
        }
        public string SendingDeadLineInString
        {
            get
            {
                if (this.SendingDeadLine == DateTime.MinValue)
                {
                    return " ";
                }else
                {
                    return this.SendingDeadLine.ToString("dd MMM yyyy");
                }
                
            }
        }




        public Company Company { get; set; }
        #endregion


        #region Functions
        public static List<DevelopementRecapMgtReport>Gets(string sSql, int ReportFormat, long nUserID)
        {
            return DevelopementRecapMgtReport.Service.Gets(sSql, ReportFormat, nUserID);
        }
        #endregion

        #region ServiceFactory

        internal static IDevelopementRecapMgtReportService Service
        {
            get { return (IDevelopementRecapMgtReportService)Services.Factory.CreateService(typeof(IDevelopementRecapMgtReportService)); }
        }

        #endregion
    }

    #endregion

    #region IDevelopementRecapMgtReport interface
         
        public interface IDevelopementRecapMgtReportService
        {

             
            List<DevelopementRecapMgtReport> Gets(string sSql, int ReportFormat, Int64 nUserID);

        }
        #endregion

}
