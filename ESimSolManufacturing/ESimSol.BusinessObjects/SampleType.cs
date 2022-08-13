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

    #region SampleType
    
    public class SampleType : BusinessObject
    {
        public SampleType()
        {
           
            SampleTypeID = 0;
            Code = "";
            SampleName = "";
            Note = "";
            ErrorMessage = "";

        }

        #region Properties
         
        public int SampleTypeID { get; set; }
         
        public string Code { get; set; }
         
        public string SampleName { get; set; }
         
        public string Note { get; set; }
         
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property
        public List<SampleType> SampleTypes { get; set; }
        public Company Company { get; set; }
  

        #endregion

        #region Functions

        public static List<SampleType> Gets(long nUserID)
        {
            return SampleType.Service.Gets( nUserID);
        }

        
        public static List<SampleType> Gets(string sSQL, long nUserID)
        {
            return SampleType.Service.Gets(sSQL, nUserID);
        }

        public SampleType Get(int id, long nUserID)
        {
            return SampleType.Service.Get(id, nUserID);
        }

        public SampleType Save(long nUserID)
        {
            return SampleType.Service.Save(this, nUserID);
        }

        public string Delete(int id, long nUserID)
        {
            return SampleType.Service.Delete(id, nUserID);
        }


        #endregion

        #region ServiceFactory
        internal static ISampleTypeService Service
        {
            get { return (ISampleTypeService)Services.Factory.CreateService(typeof(ISampleTypeService)); }
        }

        #endregion
    }
    #endregion

    #region ISampleType interface
     
    public interface ISampleTypeService
    {
         
        SampleType Get(int id, Int64 nUserID);
         
        List<SampleType> Gets(Int64 nUserID);
         
        List<SampleType> Gets(string sSQL, Int64 nUserID);
         
        string Delete(int id, Int64 nUserID);
         
        SampleType Save(SampleType oSampleType, Int64 nUserID);

    }
    #endregion
    
}
