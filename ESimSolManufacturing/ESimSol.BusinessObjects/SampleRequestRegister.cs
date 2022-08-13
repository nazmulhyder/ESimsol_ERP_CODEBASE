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
    public class SampleRequestRegister : BusinessObject
    {
        public SampleRequestRegister()
        {
            SampleRequestID = 0;
            RequestDate = DateTime.Now;
            RequestBy = 0;
            BUID = 0;
            Remarks = "";
            RequestNo = "";
            RequestTo = 0;
            ContractorID = 0;
            ContactPersonID = 0;
            ErrorMessage = "";
            SampleRequestDetailID = 0;
            ProductID = 0;
            ColorCategoryID = 0;
            MUnitID = 0;
            Quantity=0;
            SampleRequestDetailRemarks="";
            ColorName = "";
            ProductName = "";
            UnitName="";
            ContractorName = "";
            ContactPersonName = "";
        }
        #region Properties
        public int SampleRequestID { get; set; }
        public DateTime RequestDate { get; set; }
        public int RequestBy { get; set; }
        public int BUID { get; set; }
        public string Remarks { get; set; }
        public string RequestNo { get; set; }
        public int RequestTo { get; set; }
        public EnumProductNature RequestType { get; set; }
        public int ContractorID { get; set; }
        public string ContractorName { get; set; }
        public int ContactPersonID { get; set; }
        public string ContactPersonName { get; set; }
        public string ErrorMessage { get; set; }
        public int SampleRequestDetailID { get; set; }
       
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public int ColorCategoryID { get; set; }
        public string ColorName { get; set; }
        public int MUnitID { get; set; }
        public string UnitName { get; set; }
        public double Quantity { get; set; }
        public string SampleRequestDetailRemarks { get; set; }
        public  EnumReportLayout ReportLayout { get; set; }
        #endregion
        #region Derived Properties
        public string RequestDateInString
        {
            get
            {
                return RequestDate.ToString("dd MMM yyyy");
            }
        }
        #endregion
        #region Fuinction
        public static List<SampleRequestRegister> Gets(string sSql, long nUserID)
        {
            return SampleRequestRegister.Service.Gets(sSql, nUserID);
        }
        #endregion
        #region ServiceFactory
        internal static ISampleRequestRegisterService Service
        {
            get { return (ISampleRequestRegisterService)Services.Factory.CreateService(typeof(ISampleRequestRegisterService)); }
        }
        #endregion
    }
    public interface ISampleRequestRegisterService
    {
       List<SampleRequestRegister> Gets(string sSql, long nUserID);
    }
}
