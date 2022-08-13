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
    #region MeasurementSpecDetail
    
    public class MeasurementSpecDetail : BusinessObject
    {
        public MeasurementSpecDetail()
        {
            MeasurementSpecDetailID = 0;	
            MeasurementSpecID=0;
            POM="";
            DescriptionNote="";
            Addition=0;
            Deduction=0;	
            SizeID=0;
            SizeValue=0;
            SizeCategoryName = "";
            Sequence = 0;
            ErrorMessage = "";
        }

        #region Properties
         
        public int MeasurementSpecDetailID { get; set; }
         
        public int MeasurementSpecID { get; set; }
         
        public string POM { get; set; }
         
        public string DescriptionNote { get; set; }
         
        public double Addition { get; set; }
         
        public double Deduction { get; set; }
         
        public int SizeID { get; set; }
         
        public double SizeValue { get; set; }
         
        public string SizeCategoryName { get; set; }
         
        public int Sequence { get; set; }
         
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property
        
        #endregion

        #region Functions
        public static List<MeasurementSpecDetail> Gets_Report(int id, long nUserID)
        {
            return MeasurementSpecDetail.Service.Gets_Report(id, nUserID);
        }
        public static List<MeasurementSpecDetail> Gets(long nUserID)
        {            
            return MeasurementSpecDetail.Service.Gets( nUserID);
        }

        public static List<MeasurementSpecDetail> Gets(int nMSID, long nUserID)
        {
            return MeasurementSpecDetail.Service.Gets(nMSID, nUserID);
        }

        public static List<MeasurementSpecDetail> GetsByTechnicalSheet(int nTSID, long nUserID)
        {
            return MeasurementSpecDetail.Service.GetsByTechnicalSheet(nTSID, nUserID);
        }

        public MeasurementSpecDetail Get(int id, long nUserID)
        {
            return MeasurementSpecDetail.Service.Get(id, nUserID);
        }

        public MeasurementSpecDetail Save(long nUserID)
        {           
            return MeasurementSpecDetail.Service.Save(this, nUserID);
        }

        public string Delete(int id, long nUserID)
        {
            return MeasurementSpecDetail.Service.Delete(id, nUserID);
        }

        #endregion

        #region ServiceFactory

        internal static IMeasurementSpecDetailService Service
        {
            get { return (IMeasurementSpecDetailService)Services.Factory.CreateService(typeof(IMeasurementSpecDetailService)); }
        }


        #endregion
    }
    #endregion

    #region IMeasurementSpecDetail interface
     
    public interface IMeasurementSpecDetailService
    {
         
        MeasurementSpecDetail Get(int id, Int64 nUserID);
         
        List<MeasurementSpecDetail> Gets_Report(int id, Int64 nUserID);
		
         
        List<MeasurementSpecDetail> Gets(Int64 nUserID);
         
        List<MeasurementSpecDetail> Gets(int nMSID, Int64 nUserID);
         
        List<MeasurementSpecDetail> GetsByTechnicalSheet(int nTSID, Int64 nUserID);
         
        string Delete(int id, Int64 nUserID);
         
        MeasurementSpecDetail Save(MeasurementSpecDetail oMeasurementSpecDetail, Int64 nUserID);
    }
    #endregion
}
