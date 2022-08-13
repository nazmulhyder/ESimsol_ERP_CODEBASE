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
    #region MeasurementSpec
    
    public class MeasurementSpec : BusinessObject
    {
        public MeasurementSpec()
        {
            MeasurementSpecID = 0;
            TechnicalSheetID = 0;
            SampleSizeID = 0;
            SizeClassID = 0;
            GarmentsTypeID = 0;
            MeasurementUnitID = 0;
            ShownAs = "";
            Note = "";
            GarmentsTypeName = "";
            SampleSizeName = "";
            UnitName = "";
            SizeClassName = "";
            ErrorMessage = "";
        }

        #region Properties
         
        public int MeasurementSpecID { get; set; }
         
        public int TechnicalSheetID { get; set; }
         
        public int SampleSizeID { get; set; }
         
        public int SizeClassID { get; set; }
         
        public int GarmentsTypeID { get; set; }
         
        public int MeasurementUnitID { get; set; }
         
        public string ShownAs { get; set; }
         
        public string Note { get; set; }
         
        public string GarmentsTypeName { get; set; }
         
        public string SampleSizeName { get; set; }
         
        public string UnitName { get; set; }
         
        public string SizeClassName { get; set; }       
         
        public string ErrorMessage { get; set; }
         
        public string MeasurementSpecDetailInString { get; set; }
        #endregion

        #region Derived Property
         
        public List<TechnicalSheetSize> TechnicalSheetSizes { get; set; }
        
        
         
        public List<MeasurementSpecDetail> MeasurementSpecDetails { get; set; }
         
        public List<TempMeasurementSpecDetail> TempMeasurementSpecDetails { get; set; }
        #endregion

        #region Functions
        public static List<MeasurementSpec> Gets_Report(int id, long nUserID)
        {
            return MeasurementSpec.Service.Gets_Report(id, nUserID);
        }


        public static List<MeasurementSpec> Gets(long nUserID)
        {
            return MeasurementSpec.Service.Gets( nUserID);
        }

        public MeasurementSpec Get(int id, long nUserID)
        {
            return MeasurementSpec.Service.Get(id, nUserID);
        }
        public MeasurementSpec Save(long nUserID)
        {           
            return MeasurementSpec.Service.Save(this, nUserID);
        }

        public string Delete(int id, long nUserID)
        {
            return MeasurementSpec.Service.Delete(id, nUserID);
        }

        #endregion

        #region ServiceFactory
        internal static IMeasurementSpecService Service
        {
            get { return (IMeasurementSpecService)Services.Factory.CreateService(typeof(IMeasurementSpecService)); }
        }

        #endregion
    }
    #endregion

    #region IMeasurementSpec interface
     
    public interface IMeasurementSpecService
    {
         
        MeasurementSpec Get(int id, Int64 nUserID);
         
        List<MeasurementSpec> Gets(Int64 nUserID);
         
        List<MeasurementSpec> Gets_Report(int id, Int64 nUserID);
         
        string Delete(int id, Int64 nUserID);
         
        MeasurementSpec Save(MeasurementSpec oMeasurementSpec, Int64 nUserID);
    }
    #endregion
}
