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
    #region RSShift
    
    public class RSShift : BusinessObject
    {
        public RSShift()
        {
            RSShiftID = 0;
            Name = "";
            Note = "";
            Activity = true;
            ModuleType = EnumModuleName.None;
            StartDateTime = DateTime.MinValue;
            EndDateTime = DateTime.MinValue;
            ErrorMessage = "";
        }

        #region Properties
        public int RSShiftID { get; set; }
        public string Name { get; set; }
        public string Note { get; set; }
        public bool Activity { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public EnumModuleName ModuleType { get; set; }
        public int ModuleTypeInt { get; set; }
        public string ErrorMessage { get; set; }

        #region Derived Property
        public string StartDateTimeSt { get { return StartDateTime.ToString("dd MMM yyyy HH:mm"); } }
        public string StartTimeSt { get { return StartDateTime.ToString("h:mm tt"); } }//h:mm tt
        public string EndDateTimeSt { get { return EndDateTime.ToString("dd MMM yyyy HH:mm"); } }
        public string EndTimeSt { get { return EndDateTime.ToString("h:mm tt"); } }
        public string ActivitySt 
        {
            get
            {
                if (Activity) return "Active"; else return "Inactive";
            }
        }
        public string ModuleTypeSt
        {
            get
            {
                return EnumObject.jGet(this.ModuleType);
            }
        }
        #endregion

        #endregion

        #region Functions
        public static List<RSShift> Gets(long nUserID)
        {
            return RSShift.Service.Gets(nUserID);
        }
        public static List<RSShift> Gets(string sql,long nUserID)
        {
            return RSShift.Service.Gets(sql,nUserID);
        }

        public RSShift Get(int id, long nUserID)
        {
            return RSShift.Service.Get(id, nUserID);
        }
        public static List<RSShift> GetsActive(long nUserID)
        {
            return RSShift.Service.GetsActive(nUserID);
        }
        public RSShift ToggleActivity(long nUserID)
        {
            return RSShift.Service.ToggleActivity(this, nUserID);
        }
        public RSShift Save(long nUserID)
        {
            return RSShift.Service.Save(this, nUserID);
        }

        public string Delete(int id, long nUserID)
        {
            return RSShift.Service.Delete(id, nUserID);
        }

        #endregion

        #region ServiceFactory
        internal static IRSShiftService Service
        {
            get { return (IRSShiftService)Services.Factory.CreateService(typeof(IRSShiftService)); }
        }
        #endregion

        public static List<RSShift> GetsByModule(int nBUID, string sModuleIDs, long nUserID)
        {
            return RSShift.Service.GetsByModule(nBUID, sModuleIDs, nUserID);
        }
    }
    #endregion

    #region IRSShift interface
    
    public interface IRSShiftService
    {
        RSShift Get(int id, Int64 nUserID);
        List<RSShift> GetsActive(Int64 nUserID);
        List<RSShift> Gets(Int64 nUserID);
        List<RSShift> Gets(string sql, Int64 nUserID);
        string Delete(int id, Int64 nUserID);
        RSShift Save(RSShift oRSShift, Int64 nUserID);
        RSShift ToggleActivity(RSShift oRSShift, Int64 nUserID);
        List<RSShift> GetsByModule(int nBUID, string sModuleIDs, Int64 nUserID);
    }
    #endregion
}