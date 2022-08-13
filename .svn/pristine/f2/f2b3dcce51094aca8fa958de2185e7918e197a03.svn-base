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
    #region DUDyeingStep
    
    public class DUDyeingStep : BusinessObject
    {
        public DUDyeingStep()
        {
            DUDyeingStepID = 0;
            ErrorMessage = "";
            Note = "";
            
        }

        #region Properties
        public int DUDyeingStepID { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
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
        public static List<DUDyeingStep> Gets(long nUserID)
        {
            return DUDyeingStep.Service.Gets(nUserID);
        }
        public static List<DUDyeingStep> GetsByOrderSetup(string sDUOrderSetupID,long nUserID)
        {
            return DUDyeingStep.Service.GetsByOrderSetup(sDUOrderSetupID,nUserID);
        }
        public DUDyeingStep Get(int id, long nUserID)
        {
            return DUDyeingStep.Service.Get(id, nUserID);
        }
     
        public DUDyeingStep GetBy(int nDyeingStepType, long nUserID)
        {
            return DUDyeingStep.Service.GetBy(nDyeingStepType, nUserID);
        }
        public DUDyeingStep Save(long nUserID)
        {
            return DUDyeingStep.Service.Save(this, nUserID);
        }
        public DUDyeingStep Activate(Int64 nUserID)
        {
            return DUDyeingStep.Service.Activate(this, nUserID);
        }
        public string Delete(long nUserID)
        {
            return DUDyeingStep.Service.Delete(this, nUserID);
        }

        #endregion

        #region ServiceFactory
        internal static IDUDyeingStepService Service
        {
            get { return (IDUDyeingStepService)Services.Factory.CreateService(typeof(IDUDyeingStepService)); }
        }
        #endregion
    }
    #endregion


    #region IDUDyeingStep interface
    
    public interface IDUDyeingStepService
    {
        
        DUDyeingStep Get(int id, Int64 nUserID);
        DUDyeingStep GetBy(int nDyeingStepType, Int64 nUserID);
        List<DUDyeingStep> Gets(Int64 nUserID);
        List<DUDyeingStep> GetsByOrderSetup(string sDUOrderSetupID,Int64 nUserID);
        string Delete(DUDyeingStep oDUDyeingStep, Int64 nUserID);
        DUDyeingStep Save(DUDyeingStep oDUDyeingStep, Int64 nUserID);
        DUDyeingStep Activate(DUDyeingStep oDUDyeingStep, Int64 nUserID);
    }
    #endregion
}