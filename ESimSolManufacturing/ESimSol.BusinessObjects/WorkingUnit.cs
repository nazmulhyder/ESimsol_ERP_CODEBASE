using System;
using System.IO;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using ICS.Core.Framework;
using ICS.Core.Utility;

namespace ESimSol.BusinessObjects
{
    #region WorkingUnit
    public class WorkingUnit : BusinessObject
    {
        public WorkingUnit()
        {
            WorkingUnitID = 0;
            WorkingUnitCode = "";
            LocationID = 0;
            OperationUnitID = 0;           
            ErrorMessage = "";
            IsActive = true;
            BUID = 0;
            BUName = "";
            LOUNameCode = "";
            BUIDs = "";
            StoreType = (int)EnumStoreType.None;
            ModuleName = (int)EnumModuleName.None;
            UnitType = EnumWoringUnitType.General;
        }

        #region Properties
        public int WorkingUnitID { get; set; }
        public string WorkingUnitCode { get; set; }
        public int LocationID { get; set; }
        public int OperationUnitID { get; set; }
        public int ContainingProduct { get; set; }       
        public bool IsActive { get; set; }
        public string OperationUnitName { get; set; }
        public string LocationName { get; set; }
        public string LOUNameCode { get; set; }
        public int BUID { get; set; }
        public string BUName { get; set; }
        public string BUIDs { get; set; }
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property
        public bool IsStore { get; set; }
        public EnumWoringUnitType UnitType { get; set; }
        public int UnitTypeInt { get; set; }
        public int StoreType { get; set; }
        public string LocationShortName { get; set; }
        public int ModuleName { get; set; }
        public bool IsActive_Location { get; set; }
        public List<EnumObject> UnitTypeObjs { get; set; }
        public List<Location> LocationList { get; set; }
        public List<OperationUnit> OperationUnitList { get; set; }
        public string WorkingUnitName
        {
            get
            {
                if (this.OperationUnitName == "")
                {
                    return this.LocationName;
                }
                else
                {
                    return this.LocationName + "[" + this.OperationUnitName + "]";
                }
            }
        }
        public string WorkingUnitNameWithBU
        {
            get
            {
                if (this.OperationUnitName == "")
                {
                    return this.LocationName;
                }
                else
                {
                    return this.LocationName + " " + this.OperationUnitName + "[" + this.BUName + "]";
                }
            }
        }
        public string Store
        {
            get
            {
                if (this.IsStore)
                {
                    return "Store";
                }
                else
                {
                    return "Not Store";
                }
            }
        }
        public string IsActiveInString
        {
            get
            {
                if (this.IsActive)
                {
                    return "Active";
                }
                else
                {
                    return "Inactive";
                }
            }
        }
        public string IsActive_LocationST
        {
            get
            {
                if (this.IsActive_Location)
                {
                    return "Active";
                }
                else
                {
                    return "Inactive";
                }
            }
        }
        #endregion

        #region Functions
        public static List<WorkingUnit> Gets(int nUserID)
        {
            return WorkingUnit.Service.Gets(nUserID);
        }
        public static List<WorkingUnit> Gets(int id, int nUserID)
        {
            return WorkingUnit.Service.Gets(id, nUserID);
        }
        public static List<WorkingUnit> Gets(string sSQL, int nUserID)
        {
            return WorkingUnit.Service.Gets(sSQL, nUserID);
        }
        public WorkingUnit Get(int id, int nUserID)
        {
            return WorkingUnit.Service.Get(id, nUserID);
        }
        public WorkingUnit Save(int nUserID)
        {
            return WorkingUnit.Service.Save(this, nUserID);
        }
        public string Delete(int id, int nUserID)
        {
            return WorkingUnit.Service.Delete(id, nUserID);
        }        
        public static WorkingUnit UpdateForAcitivity(int nWorkingUnitID, int nUserID)
        {
            return WorkingUnit.Service.UpdateForAcitivity(nWorkingUnitID, nUserID);
        }
        public static List<WorkingUnit> GetsbyName(string sWorkingUnit, int nUserID)
        {
            return WorkingUnit.Service.GetsbyName(sWorkingUnit , nUserID);
        }
        public static List<WorkingUnit> BUWiseGets(int nBUID, int nUserID)
        {
            return WorkingUnit.Service.BUWiseGets(nBUID, nUserID);
        }
        public static List<WorkingUnit> GetsPermittedStore(int nBUID, EnumModuleName eModuleName, EnumStoreType eStoreType, int nUserID)
        {
            return WorkingUnit.Service.GetsPermittedStore(nBUID, eModuleName, eStoreType, nUserID);
        }
        public static List<WorkingUnit> GetsPermittedStoreByStoreName(int nBUID, EnumModuleName eModuleName, EnumStoreType eStoreType, string sStoreName, int nUserID)
        {
            return WorkingUnit.Service.GetsPermittedStoreByStoreName(nBUID, eModuleName, eStoreType, sStoreName, nUserID);
        }         
        public string WorkingUnit_AutoConfiguration(int nLocation_Assign, int nLocation_Source, int nUserID_Assign, int nUserID_Source, int nConType, int nUserID)
        {
            return WorkingUnit.Service.WorkingUnit_AutoConfiguration(nLocation_Assign, nLocation_Source, nUserID_Assign, nUserID_Source, nConType, nUserID);
        }
        public static List<WorkingUnit> Gets(string sDBObject, int nTRType, int nOEValue, int nInOutType, bool bDirection, int nPid, int nWUId, long nUserID)
        {
            return WorkingUnit.Service.Gets(sDBObject, nTRType, nOEValue, nInOutType, bDirection, nPid, nWUId, nUserID);
        }
        #endregion
                
        #region ServiceFactory
        internal static IWorkingUnitService Service
        {
            get { return (IWorkingUnitService)Services.Factory.CreateService(typeof(IWorkingUnitService)); }
        }
        #endregion
    
        #region Non DB Property
        public static string IDInString(List<WorkingUnit> oWorkingUnits)
        {
            string sReturn = "";
            foreach (WorkingUnit oItem in oWorkingUnits)
            {
                sReturn = sReturn + oItem.WorkingUnitID.ToString() + ",";
            }
            if (sReturn == "") return "";
            sReturn = sReturn.Remove(sReturn.Length - 1, 1);
            return sReturn;
        }
        #endregion
    }
    #endregion

    #region IWorkingUnit interface
    public interface IWorkingUnitService
    {
        List<WorkingUnit> GetsbyName(string sWorkingUnit, int nUserID);
        WorkingUnit Get(int id, int nUserID);
        List<WorkingUnit> Gets(int nUserID);
        List<WorkingUnit> Gets(int _nLocationID, int nUserID);        
        List<WorkingUnit> Gets(string sSQL, int nUserID);
        string Delete(int id, int nUserID);
        WorkingUnit Save(WorkingUnit oWorkingUnit, int nUserID);            
        WorkingUnit UpdateForAcitivity(int nWorkingUnitID, int nUserID);
        List<WorkingUnit> BUWiseGets(int nBUID, int nUserID);
        List<WorkingUnit> GetsPermittedStore(int nBUID, EnumModuleName eModuleName, EnumStoreType eStoreType, int nUserID);
        List<WorkingUnit> GetsPermittedStoreByStoreName(int nBUID, EnumModuleName eModuleName, EnumStoreType eStoreType, string sStoreName, int nUserID);
        string WorkingUnit_AutoConfiguration(int nLocation_Assign, int nLocation_Source, int nUserID_Assign, int nUserID_Source, int nConType, int nUserID);
        List<WorkingUnit> Gets(string sDBObject, int nTRType, int nOEValue, int nInOutType, bool bDirection, int nPid, int nWUId, long nUserID);
    }
    #endregion
}