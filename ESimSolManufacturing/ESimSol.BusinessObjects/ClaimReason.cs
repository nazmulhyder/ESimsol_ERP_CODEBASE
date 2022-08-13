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
    #region ClaimReason
    
    public class ClaimReason : BusinessObject
    {
        public ClaimReason()
        {
            ClaimReasonID = 0;
            BUID = 0;
            Name="";
            OperationType=EnumClaimOperationType.None;
            Note="";
            Activity=false;
            ErrorMessage = "";
        }

        #region Properties
        public int ClaimReasonID { get; set; }
        public int BUID { get; set; }
         public string Name { get; set; }
         public EnumClaimOperationType OperationType { get; set; }
        public string Note { get; set; }
        public bool Activity { get; set; }
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
        public int OperationTypeInt
        {
            get
            {
                return (int)this.OperationType;
            }
        }
        public string OperationTypeST
        {
            get
            {
                return EnumObject.jGet(this.OperationType);
            }
        }
        
        #endregion

        #endregion

        #region Functions
        public static List<ClaimReason> Gets(long nUserID)
        {
            return ClaimReason.Service.Gets(nUserID);
        }
        public static List<ClaimReason> Gets(int nBUID,long nUserID)
        {
            return ClaimReason.Service.Gets(nBUID, nUserID);
        }
        public static List<ClaimReason> Gets(string sSQL, long nUserID)
        {
            return ClaimReason.Service.Gets(sSQL, nUserID);
        }
        public ClaimReason Get(int id, long nUserID)
        {
            return ClaimReason.Service.Get(id, nUserID);
        }
        public static ClaimReason GetByType(int nOperationType, long nUserID)
        {
            return ClaimReason.Service.GetByType(nOperationType, nUserID);
        }

        public ClaimReason Save(long nUserID)
        {
            return ClaimReason.Service.Save(this, nUserID);
        }
        public ClaimReason Activate(Int64 nUserID)
        {
            return ClaimReason.Service.Activate(this, nUserID);
        }
        public string Delete(long nUserID)
        {
            return ClaimReason.Service.Delete(this, nUserID);
        }

        #endregion

        #region ServiceFactory
        internal static IClaimReasonService Service
        {
            get { return (IClaimReasonService)Services.Factory.CreateService(typeof(IClaimReasonService)); }
        }
        #endregion
    }
    #endregion


    #region IClaimReason interface
    
    public interface IClaimReasonService
    {
        ClaimReason Get(int id, Int64 nUserID);
        ClaimReason GetByType(int nOperationType, Int64 nUserID);
        List<ClaimReason> Gets(string sSQL, long nUserID);
        List<ClaimReason> Gets(Int64 nUserID);
        List<ClaimReason> Gets(int nBUID,Int64 nUserID);
        string Delete(ClaimReason oClaimReason, Int64 nUserID);
        ClaimReason Save(ClaimReason oClaimReason, Int64 nUserID);
        ClaimReason Activate(ClaimReason oClaimReason, Int64 nUserID);
    }
    #endregion
}
