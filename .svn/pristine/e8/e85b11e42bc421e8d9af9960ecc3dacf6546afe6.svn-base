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
    #region HIAUserAssign
    
    public class HIAUserAssign : BusinessObject
    {
        public HIAUserAssign()
        {
            HIAUserAssignID = 0;
            HIASetupID = 0;
            UserID = 0;
            ErrorMessage = "";
        }

        #region Properties
         
        public int HIAUserAssignID { get; set; }
         
        public int HIASetupID { get; set; }
         
        public int UserID { get; set; }
         
        public string LogInID { get; set; }
         
        public string UserName { get; set; }
         
        public string LocationName { get; set; }
         
        public string EmployeeNameCode { get; set; }

         
        public string ErrorMessage { get; set; }

        #endregion

        #region Derived Property
      
        #endregion

        #region Functions
        public static List<HIAUserAssign> Gets(int nHIASetupID, long nUserID)
        {
            return HIAUserAssign.Service.Gets(nHIASetupID, nUserID);
        }
        public static List<HIAUserAssign> Gets(string sSQL, long nUserID)
        {
            return HIAUserAssign.Service.Gets(sSQL, nUserID);
        }
        public HIAUserAssign Get(int nHIAUserAssignID, long nUserID)
        {
            
            return HIAUserAssign.Service.Get(nHIAUserAssignID, nUserID);
        }
        public HIAUserAssign Save(long nUserID)
        {
            return HIAUserAssign.Service.Save(this, nUserID);
        }
        #endregion

        #region ServiceFactory

        internal static IHIAUserAssignService Service
        {
            get { return (IHIAUserAssignService)Services.Factory.CreateService(typeof(IHIAUserAssignService)); }
        }

        #endregion
    }
    #endregion

    #region IHIAUserAssign interface
     
    public interface IHIAUserAssignService
    {
         
        HIAUserAssign Get(int nHIAUserAssignID, Int64 nUserID);
         
        List<HIAUserAssign> Gets(int nHIASetupID, Int64 nUserID);
         
        List<HIAUserAssign> Gets(string sSQL, Int64 nUserID);
         
        HIAUserAssign Save(HIAUserAssign oHIAUserAssign, Int64 nUserID);
    }
    #endregion
}