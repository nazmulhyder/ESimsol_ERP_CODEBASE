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
    #region Machine
    
    public class Machine : BusinessObject
    {
        public Machine()
        {
            MachineID = 0;
            MachineTypeID = 0;
            SequenceNo = 0;
            MachineTypeName = "";
            LocationType = EnumLocationType.None;
            BUID = 0;
            Note = "";
            ErrorMessage = "";
            Activity = true;
            IsBold = false;
        }

        #region Properties
        public int MachineID { get; set; }
        public int LocationID { get; set; }
        public int MachineTypeID { get; set; }
        public int SequenceNo { get; set; }
        public int BUID { get; set; }
        public string MachineTypeName { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Note { get; set; }
        public bool Activity { get; set; }
        public double Capacity { get; set; }
        public string Capacity2 { get; set; }
        public string LocCode { get; set; }
        public EnumLocationType LocationType { get; set; }
        public string LocationName {get; set;}
        public string BUnit { get; set; }
        public string ErrorMessage { get; set; }
        public string MachineNoWithCapacityAndTotalSchedule { get; set; }
        
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
        public int LocationTypeInt { get { return (int)LocationType; } }
        public bool IsBold { get; set; }
        #endregion

        #endregion

        #region Functions
        public static List<Machine> Gets(long nUserID)
        {
            return Machine.Service.Gets(nUserID);
        }
        public static List<Machine> GetsActive(long nUserID)
        {
            return Machine.Service.GetsActive(nUserID);
        }
        public static List<Machine> GetsBy(int nBUID,long nUserID)
        {
            return Machine.Service.GetsBy(nBUID, nUserID);
        }
        public static List<Machine> GetsByModule(int nBUID, string sModuleIDs, long nUserID)
        {
            return Machine.Service.GetsByModule(  nBUID, sModuleIDs, nUserID);
        }
        public static List<Machine> Gets(string sSQL, long nUserID)
        {
            return Machine.Service.Gets(sSQL, nUserID);
        }
        public Machine Get(int id, long nUserID)
        {
            return Machine.Service.Get(id, nUserID);
        }
        public Machine GetByType(int nMachineTypeID, long nUserID)
        {
            return Machine.Service.GetByType(nMachineTypeID, nUserID);
        }

        public Machine Save(long nUserID)
        {
            return Machine.Service.Save(this, nUserID);
        }
        public Machine Activate(Int64 nUserID)
        {
            return Machine.Service.Activate(this, nUserID);
        }
        public string Delete(long nUserID)
        {
            return Machine.Service.Delete(this, nUserID);
        }
        public List<Machine> Update(List<Machine> oMachines,long nUserID)
        {
            return Machine.Service.Update(oMachines, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IMachineService Service
        {
            get { return (IMachineService)Services.Factory.CreateService(typeof(IMachineService)); }
        }
        #endregion
    }
    #endregion

    #region IMachine interface
    
    public interface IMachineService
    {
        
        Machine Get(int id, Int64 nUserID);
        Machine GetByType(int nMachineTypeID, Int64 nUserID);
        List<Machine> Gets(string sSQL, long nUserID);
        List<Machine> Gets(Int64 nUserID);
        List<Machine> GetsActive( Int64 nUserID);
        List<Machine> GetsBy(int nBUID,Int64 nUserID);
        List<Machine> GetsByModule(int nBUID, string sModuleIDs, Int64 nUserID);
        string Delete(Machine oMachine, Int64 nUserID);
        Machine Save(Machine oMachine, Int64 nUserID);
        List<Machine> Update(List<Machine> oMachines, Int64 nUserID);
        Machine Activate(Machine oMachine, Int64 nUserID);
    }
    #endregion
}