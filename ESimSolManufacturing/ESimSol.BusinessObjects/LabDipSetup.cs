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
    #region LabDipSetup
    
    public class LabDipSetup : BusinessObject
    {
        public LabDipSetup()
        {
            LabDipSetupID = 0;
            ErrorMessage = "";
            PrintNo = 0;
            Activity = true;
            LDNoCreateBy = 0;
            OrderName = "";
            LDName = "";
            ColorNoName = "";
            IsApplyCode = false;
            IsApplyPO = false;
        }

        #region Properties
        public int LabDipSetupID { get; set; }
        public string OrderCode { get; set; }
        public string ColorNoName { get; set; }
        public string LDName { get; set; }
        public string OrderName { get; set; }
        public int PrintNo { get; set; }
        public string Note { get; set; }
        public bool Activity { get; set; }
        public bool IsApplyCode { get; set; }
        public bool IsApplyPO { get; set; }
        public int LDNoCreateBy { get; set; }
        public string ErrorMessage { get; set; }
        
        #region Derived Property
        public string ActivitySt
        {
            get
            {
                if (this.Activity == true) return "Active";
                else if (this.Activity == false) return "Inactive";
                else return "-";
            }
        }
        #endregion

        #endregion

        #region Functions
        public static List<LabDipSetup> Gets(long nUserID)
        {
            return LabDipSetup.Service.Gets(nUserID);
        }
       
        public LabDipSetup Get(int id, long nUserID)
        {
            return LabDipSetup.Service.Get(id, nUserID);
        }
      
        public LabDipSetup Save(long nUserID)
        {
            return LabDipSetup.Service.Save(this, nUserID);
        }
        public LabDipSetup Activate(Int64 nUserID)
        {
            return LabDipSetup.Service.Activate(this, nUserID);
        }
        public string Delete(long nUserID)
        {
            return LabDipSetup.Service.Delete(this, nUserID);
        }

        #endregion

        #region ServiceFactory
        internal static ILabDipSetupService Service
        {
            get { return (ILabDipSetupService)Services.Factory.CreateService(typeof(ILabDipSetupService)); }
        }
        #endregion
    }
    #endregion


    #region ILabDipSetup interface
    
    public interface ILabDipSetupService
    {
        
        LabDipSetup Get(int id, Int64 nUserID);
        List<LabDipSetup> Gets(Int64 nUserID);
        string Delete(LabDipSetup oLabDipSetup, Int64 nUserID);
        LabDipSetup Save(LabDipSetup oLabDipSetup, Int64 nUserID);
        LabDipSetup Activate(LabDipSetup oLabDipSetup, Int64 nUserID);
    }
    #endregion
}