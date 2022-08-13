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
    #region MachineType
    
    public class MachineType : BusinessObject
    {
        public MachineType()
        {
            MachineTypeID = 0;
            Name = "";
            BUID = 0;
            ErrorMessage = "";
            Note = "";
        }

        #region Properties
        public int MachineTypeID { get; set; }
        public string Name { get; set; }
        public int BUID { get; set; }
        public string Note { get; set; }
        public string ModuleIDs { get; set; }
        public List<Module> Modules
        {
            get
            {
                List<Module> ModuleResult = new List<Module>();
                if (!string.IsNullOrEmpty(ModuleIDs))
                {
                    foreach (string sID in ModuleIDs.Split(','))
                        ModuleResult.Add(new Module() { id = Convert.ToInt32(sID) });
                }
                return ModuleResult;
            } 
        }
        public string ErrorMessage { get; set; }
       
        #endregion

        #region Functions
        public static List<MachineType> Gets(long nUserID)
        {
            return MachineType.Service.Gets(nUserID);
        }
        public static List<MachineType> Gets(string sql, long nUserID)
        {
            return MachineType.Service.Gets(sql, nUserID);
        }
        public static List<MachineType> GetsByModuleIDs(string ids, long nUserID)
        {
            return MachineType.Service.GetsByModuleIDs(ids, nUserID);
        }
        public static List<MachineType> GetsByModuleID(int id, long nUserID)
        {
            return MachineType.Service.GetsByModuleIDs(id.ToString(), nUserID);
        }
        public MachineType Get(int id, long nUserID)
        {
            return MachineType.Service.Get(id, nUserID);
        }
        public MachineType GetBy(int nDyeingStepType, long nUserID)
        {
            return MachineType.Service.GetBy(nDyeingStepType, nUserID);
        }
        public MachineType Save(long nUserID)
        {
            return MachineType.Service.Save(this, nUserID);
        }
        public MachineType Activate(Int64 nUserID)
        {
            return MachineType.Service.Activate(this, nUserID);
        }
        public string Delete(long nUserID)
        {
            return MachineType.Service.Delete(this, nUserID);
        }

        #endregion

        #region ServiceFactory
        internal static IMachineTypeService Service
        {
            get { return (IMachineTypeService)Services.Factory.CreateService(typeof(IMachineTypeService)); }
        }
        #endregion
    }
    #endregion

    #region IMachineType interface
    
    public interface IMachineTypeService
    {
        MachineType Get(int id, Int64 nUserID);
        MachineType GetBy(int nDyeingStepType, Int64 nUserID);
        List<MachineType> Gets(Int64 nUserID);
        List<MachineType> Gets(string sql, Int64 nUserID);
        List<MachineType> GetsByModuleIDs(string ids, Int64 nUserID);
        string Delete(MachineType oMachineType, Int64 nUserID);
        MachineType Save(MachineType oMachineType, Int64 nUserID);
        MachineType Activate(MachineType oMachineType, Int64 nUserID);
    }
    #endregion

    #region Module Class
    public class Module
    {
        public Module()
        {
            id = 0;
        }
        public int id { get; set; }
        public string Value { get { return EnumObject.jGet((EnumModuleName)id); } }
    }

    #endregion
}