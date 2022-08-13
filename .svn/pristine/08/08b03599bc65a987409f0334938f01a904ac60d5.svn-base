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
    #region SampleRequestDetail
    public class SampleRequestDetail : BusinessObject
    {
        public SampleRequestDetail()
        {
            SampleRequestDetailID = 0;
            SampleRequestID = 0;
            ProductID = 0;
            ColorCategoryID = 0;
            MUnitID = 0;
            LotID = 0;
            LotNo = "";
            LotBalance = 0;
            Quantity = 0;
            Remarks = "";
     
            ErrorMessage = "";
        }

        #region Property
        public int SampleRequestDetailID { get; set; }
        public int SampleRequestID { get; set; }
        public int ProductID { get; set; }
        public int ColorCategoryID { get; set; }

        public int MUnitID { get; set; }
        public int LotID { get; set; }
        public String LotNo { get; set; }
        public double LotBalance { get; set; }
        public double Quantity { get; set; }
        public string Remarks { get; set; }

        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property
        public string ProductName { get; set; }

        public string ProductCode { get; set; }
        public string ColorName { get; set; }
        public string MUnitName { get; set; }
        #endregion

        #region Functions
        public static List<SampleRequestDetail> Gets(int nSampleRequestID, long nUserID)
        {
            return SampleRequestDetail.Service.Gets(nSampleRequestID, nUserID);
        }
        public static List<SampleRequestDetail> Gets(string sSQL, long nUserID)
        {
            return SampleRequestDetail.Service.Gets(sSQL, nUserID);
        }
        public SampleRequestDetail Get(int id, long nUserID)
        {
            return SampleRequestDetail.Service.Get(id, nUserID);
        }

        #endregion

        #region ServiceFactory
        internal static ISampleRequestDetailService Service
        {
            get { return (ISampleRequestDetailService)Services.Factory.CreateService(typeof(ISampleRequestDetailService)); }
        }
        #endregion

        public List<SampleRequestDetail> SampleRequestDetails { get; set; }
    }
    #endregion

    #region ISampleRequestDetail interface
    public interface ISampleRequestDetailService
    {
        SampleRequestDetail Get(int id, Int64 nUserID);
        List<SampleRequestDetail> Gets(int nSampleRequestID, Int64 nUserID);
        List<SampleRequestDetail> Gets(string sSQL, Int64 nUserID);

    }
    #endregion
}
