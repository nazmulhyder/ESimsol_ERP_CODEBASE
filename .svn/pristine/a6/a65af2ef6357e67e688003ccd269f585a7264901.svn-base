using System;
using System.IO;
using System.ComponentModel.DataAnnotations;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using ICS.Core.Framework;
using ICS.Core.Utility;

namespace ESimSol.BusinessObjects
{
    #region DUStepWiseSetup
    
    public class DUStepWiseSetup : BusinessObject
    {
        public DUStepWiseSetup()
        {
            DUStepWiseSetupID = 0;
            DUOrderSetupID = 0;
            ErrorMessage = "";
            Note = "";
        }

        #region Properties
        public int DUStepWiseSetupID { get; set; }
        public int DUOrderSetupID { get; set; }
        public int DyeingStepType { get; set; }
        public string Note { get; set; }
        public string ErrorMessage { get; set; }
        
        #region Derived Property
        public string DyeingStepTypeSt
        {
            get
            {
                return EnumObject.jGet((EnumDyeingStepType)this.DyeingStepType);
            }
        }
        #endregion

        #endregion

        #region Functions
        public static List<DUStepWiseSetup> Gets(long nUserID)
        {
            return DUStepWiseSetup.Service.Gets(nUserID);
        }
        public static List<DUStepWiseSetup> Gets(string sSQL, long nUserID)
        {
            return DUStepWiseSetup.Service.Gets(sSQL, nUserID);
        }
        public DUStepWiseSetup Get(int id, long nUserID)
        {
            return DUStepWiseSetup.Service.Get(id, nUserID);
        }
        public DUStepWiseSetup GetBy(int nDyeingStepType, long nUserID)
        {
            return DUStepWiseSetup.Service.GetBy(nDyeingStepType, nUserID);
        }
        public DUStepWiseSetup Save(long nUserID)
        {
            return DUStepWiseSetup.Service.Save(this, nUserID);
        }
        public string Delete(long nUserID)
        {
            return DUStepWiseSetup.Service.Delete(this, nUserID);
        }

        #endregion

        #region ServiceFactory
        internal static IDUStepWiseSetupService Service
        {
            get { return (IDUStepWiseSetupService)Services.Factory.CreateService(typeof(IDUStepWiseSetupService)); }
        }
        #endregion
    }
    #endregion


    #region IDUStepWiseSetup interface
    
    public interface IDUStepWiseSetupService
    {
        DUStepWiseSetup Get(int id, Int64 nUserID);
        DUStepWiseSetup GetBy(int nDyeingStepType, Int64 nUserID);
        List<DUStepWiseSetup> Gets(Int64 nUserID);
        List<DUStepWiseSetup> Gets(string sSQL, Int64 nUserID);
        string Delete(DUStepWiseSetup oDUStepWiseSetup, Int64 nUserID);
        DUStepWiseSetup Save(DUStepWiseSetup oDUStepWiseSetup, Int64 nUserID);
    }
    #endregion
}