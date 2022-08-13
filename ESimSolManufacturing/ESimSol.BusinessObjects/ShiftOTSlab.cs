using System;
using System.IO;
using ICS.Core;
using ICS.Core.Framework;
using ICS.Core.Utility;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;


namespace ESimSol.BusinessObjects
{
    #region ShiftOTSlab

    public class ShiftOTSlab : BusinessObject
    {
        public ShiftOTSlab()
        {
            ShiftOTSlabID = 0;
            ShiftID = 0;
            MinOTInMin=0;
            MaxOTInMin = 0;
            AchieveOTInMin = 0;
            IsActive = true;
            ErrorMessage = "";
            CompMinOTInMin = 0;
            CompMaxOTInMin = 0;
            CompAchieveOTInMin = 0;
            IsCompActive = true;
           
        }

        #region Properties
        public int ShiftOTSlabID { get; set; }
        public int ShiftID { get; set; }
        public int MinOTInMin { get; set; }
        public int MaxOTInMin { get; set; }
        public int AchieveOTInMin { get; set; }
        public bool IsActive { get; set; }
        public int CompMinOTInMin { get; set; }
        public int CompMaxOTInMin { get; set; }
        public int CompAchieveOTInMin { get; set; }
        public bool IsCompActive { get; set; }
        public string ErrorMessage { get; set; }

        #endregion

        #region Derived Property
        public string Activity { get { if (IsActive)return "Active"; else return "Inactive"; } }
        public string ActivityComp { get { if (IsCompActive)return "Active"; else return "Inactive"; } }

        public string OTSlab
        {
            get
            {
                return Global.MinInHourMin(MinOTInMin) + "-" + Global.MinInHourMin(MaxOTInMin);
            }
        }

        public string OTSlabComp
        {
            get
            {
                return Global.MinInHourMin(CompMinOTInMin) + "-" + Global.MinInHourMin(CompMaxOTInMin);
            }
        }
        public string AchieveOTInMinST
        {
            get
            {
                return Global.MinInHourMin(AchieveOTInMin);
            }
        }

        public string AchieveOTInMinSTComp
        {
            get
            {
                return Global.MinInHourMin(CompAchieveOTInMin);
            }
        }
        #endregion

        #region Functions
        public static ShiftOTSlab Get(int Id, long nUserID)
        {
            return ShiftOTSlab.Service.Get(Id, nUserID);
        }
        public static ShiftOTSlab Get(string sSQL, long nUserID)
        {
            return ShiftOTSlab.Service.Get(sSQL, nUserID);
        }
        public static List<ShiftOTSlab> Gets(long nUserID)
        {
            return ShiftOTSlab.Service.Gets(nUserID);
        }
        public static List<ShiftOTSlab> Gets(string sSQL, long nUserID)
        {
            return ShiftOTSlab.Service.Gets(sSQL, nUserID);
        }

        public ShiftOTSlab IUD(int nDBOperation, long nUserID)
        {
            return ShiftOTSlab.Service.IUD(this, nDBOperation, nUserID);
        }

        public static ShiftOTSlab Activite(int nId, long nUserID)
        {
            return ShiftOTSlab.Service.Activite(nId, nUserID);
        }
        public static ShiftOTSlab ActiviteComp(int nId, long nUserID)
        {
            return ShiftOTSlab.Service.ActiviteComp(nId, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IShiftOTSlabService Service
        {
            get { return (IShiftOTSlabService)Services.Factory.CreateService(typeof(IShiftOTSlabService)); }
        }

        #endregion
    }
    #endregion

    #region IShiftOTSlab interface

    public interface IShiftOTSlabService
    {
        ShiftOTSlab Get(int id, Int64 nUserID);
        ShiftOTSlab Get(string sSQL, Int64 nUserID);
        List<ShiftOTSlab> Gets(Int64 nUserID);
        List<ShiftOTSlab> Gets(string sSQL, Int64 nUserID);
        ShiftOTSlab IUD(ShiftOTSlab oShiftOTSlab, int nDBOperation, Int64 nUserID);
        ShiftOTSlab Activite(int nId, Int64 nUserID);
        ShiftOTSlab ActiviteComp(int nId, Int64 nUserID);
    }
    #endregion
}
