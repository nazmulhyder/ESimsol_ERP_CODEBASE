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

    #region SampleRequirement
    
    public class SampleRequirement : BusinessObject
    {
        public SampleRequirement()
        {
            SampleRequirementID = 0;
            OrderRecapID = 0;
            SampleTypeID = 0;
            Code = "";
            SampleName = "";
            Remark = "";
            ErrorMessage = "";

        }

        #region Properties
         
        public int SampleRequirementID { get; set; }
         
        public string Remark { get; set; }
         
        public int OrderRecapID { get; set; }
         
        public int SampleTypeID { get; set; }
         
        public string Code { get; set; }
         
        public string SampleName { get; set; }
         
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property
        public List<SampleRequirement> SampleRequirements { get; set; }
        public Company Company { get; set; }
        #endregion

        #region Functions

        public static List<SampleRequirement> Gets(long nUserID)
        {
            return SampleRequirement.Service.Gets( nUserID);
        }

        public static List<SampleRequirement> Gets(int nSaleOrderID, long nUserID)
        {
            return SampleRequirement.Service.Gets(nSaleOrderID, nUserID);
        }
        public static List<SampleRequirement> Gets(string sSQL, long nUserID)
        {
            return SampleRequirement.Service.Gets(sSQL, nUserID);
        }

        public SampleRequirement Get(int id, long nUserID)
        {

            return SampleRequirement.Service.Get(id, nUserID);
        }

        public SampleRequirement Save(long nUserID)
        {
            return SampleRequirement.Service.Save(this, nUserID);
        }

        public string Delete(int id, long nUserID)
        {
            return SampleRequirement.Service.Delete(id, nUserID);
        }


        #endregion

        #region ServiceFactory
 
        internal static ISampleRequirementService Service
        {
            get { return (ISampleRequirementService)Services.Factory.CreateService(typeof(ISampleRequirementService)); }
        }
        #endregion
    }
    #endregion

    #region Report Study
    public class SampleRequirementList : List<SampleRequirement>
    {
        public string ImageUrl { get; set; }
    }
    #endregion

    #region ISampleRequirement interface
     
    public interface ISampleRequirementService
    {
         
        SampleRequirement Get(int id, Int64 nUserID);
         
        List<SampleRequirement> Gets(Int64 nUserID);
         
        List<SampleRequirement> Gets(int nSaleOrderID, Int64 nUserID);
         
        List<SampleRequirement> Gets(string sSQL, Int64 nUserID);
         
        string Delete(int id, Int64 nUserID);
         
        SampleRequirement Save(SampleRequirement oSampleRequirement, Int64 nUserID);

    }
    #endregion
    
  
}
