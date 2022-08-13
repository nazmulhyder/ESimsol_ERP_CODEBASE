using System;
using System.IO;
using ICS.Core;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using ICS.Core.Framework;
using ICS.Core.Utility;
using ESimSol.BusinessObjects.ReportingObject;

namespace ESimSol.BusinessObjects
{
    #region FabricMachine
    public class FabricMachine:BusinessObject
    {
        public FabricMachine()
        {
            FMID = 0;             
            Name = "";
            WeavingProcess = EnumWeavingProcess.Warping;
            CCID = 0;
            Code = "";
            Capacity = "";
            RPM = 0;
            StandardEfficiency = 0;
            IsActive = true;
            InActiveDate = DateTime.Now;
            MachineStatus = EnumMachineStatus.Free;
            IsBeam = false;
            WeavingProcessInInt = 0;
            ErrorMessage = "";
            FBID = 0;
            IsFromWeaving = false;
            BatchNo = "";
            OrderNo = "";
            Duration = "";
            TSUID = 0;
            TextileSubUnitName = "";
            Params = string.Empty;
            ChildMachineTypeID = 0;
            ParentMachineTypeID = 0;
            ChildMachineTypeName = "";
            ParentMachineTypeName = "";
            FabricMachineGroupID = 0;
            FabricMachineGroupName = "";
            IsDirect = false;
        }

        #region Properties
        public int FMID { get; set; }      
        public string Name { get; set; }
        public string Capacity { get; set; }
        public EnumWeavingProcess WeavingProcess { get; set; }
        public int CCID { get; set; }
        public int WeavingProcessInInt { get; set; }
        public string Code { get; set; }
        public int RPM { get; set; }
        public double StandardEfficiency { get; set; }
        public bool IsActive { get; set; }
        public bool IsBeam { get; set; }
        public bool IsDirect { get; set; }
        public int ParentMachineTypeID { get; set; }
        public int ChildMachineTypeID { get; set; }
        public int FabricMachineGroupID { get; set; }
        public DateTime InActiveDate { get; set; }
        public EnumMachineStatus MachineStatus { get; set; }
        public string ErrorMessage { get; set; }
        public int FBID { get; set; }
        public bool IsFromWeaving { get; set; }
        public string BatchNo { get; set; }
        public string OrderNo { get; set; }
        public string Duration { get; set; }
        public string Params { get; set; }
      

        #endregion

        #region Derived Property
        public string ParentMachineTypeName { get; set; }
        public string ChildMachineTypeName { get; set; }
        public string FabricMachineGroupName { get; set; }
        public List<FabricMachine> FabricMachines { get; set; }
        public Company Company { get; set; }
        public int TSUID { get; set; }
        public string TextileSubUnitName { get; set; }
        public string ChildWithParent
        {
            get
            {
                return this.ChildMachineTypeName + " - " + this.ParentMachineTypeName;
            }
        }
        public string MachineCodeWithName
        {
            get
            {
                if (!string.IsNullOrEmpty(this.Code))
                {
                    return this.Code + " [" + this.Name + "] ";
                }
                else 
                {
                    return this.Name;
                }
            }
        }
        public string IsBeamSt
        {
            get
            {
                if (this.IsBeam)
                {
                    return "Beam";
                }
                return "-";
            }
        }

        public string WeavingProcessInString
        {
            get
            {
                return this.WeavingProcess.ToString();
            }
        }

        public string ActiveInActiveInString
        {
            get
            {
                if(this.IsActive)
                {
                    return "Active";
                }
                else
                {
                    return "In Active";
                }
            }
        }

        public string MachineStatusInString
        {
            get
            {
                return this.MachineStatus.ToString();
            }
        }
        #endregion

        #region Functions

        public static List<FabricMachine> Gets(long nUserID)
        {
            return FabricMachine.Service.Gets( nUserID);
        }
        public static List<FabricMachine> Gets(EnumWeavingProcess EWeavingProcess, EnumMachineStatus EnMachineStatus, long nUserID)
        {
            return FabricMachine.Service.Gets((int)EWeavingProcess, (int)EnMachineStatus, nUserID);
        }
        public static List<FabricMachine> Gets(bool bIsBeam, EnumWeavingProcess EWeavingProcess, EnumMachineStatus EnMachineStatus, long nUserID)
        {
            return FabricMachine.Service.Gets(bIsBeam, (int)EWeavingProcess, (int)EnMachineStatus, nUserID);
        }

        public static List<FabricMachine> Gets(string sSQL, long nUserID)
        {
            return FabricMachine.Service.Gets(sSQL, nUserID);
        }

        public FabricMachine Get(int id, long nUserID)
        {
            return FabricMachine.Service.Get(id, nUserID);
        }

        public FabricMachine Save(long nUserID)
        {
            return FabricMachine.Service.Save(this, nUserID);
        }

        public string Delete(int id, long nUserID)
        {
            return FabricMachine.Service.Delete(id, nUserID);
        }
        public FabricMachine ActiveInActive(int id, bool bIsActive)
        {
            return FabricMachine.Service.ActiveInActive(id, bIsActive);
        }
        public FabricMachine MakeFree(long nUserID)
        {
            return FabricMachine.Service.MakeFree(this, nUserID);
        }
        public FabricMachine LoomMachineRestore(int id)
        {
            return FabricMachine.Service.LoomMachineRestore(id);
        }
        public FabricMachine HoldBeamFinishForLoomProcess(int id)
        {
            return FabricMachine.Service.HoldBeamFinishForLoomProcess(id);
        }
        #endregion

        #region ServiceFactory
        internal static IFabricMachineService Service
        {
            get { return (IFabricMachineService)Services.Factory.CreateService(typeof(IFabricMachineService)); }
        }


        #endregion
    }
    #endregion

    #region Report Study
    public class FabricMachineList : List<FabricMachine>
    {
        public string ImageUrl { get; set; }
    }
    #endregion

    #region IFabricMachine interface
     
    public interface IFabricMachineService
    {
        FabricMachine Get(int id, Int64 nUserID);
        List<FabricMachine> Gets(Int64 nUserID);
        List<FabricMachine> Gets(int nWeavingProcess,int nMachineStatus, Int64 nUserID);
        List<FabricMachine> Gets(bool bIsBeam, int nWeavingProcess, int nMachineStatus, Int64 nUserID);
        List<FabricMachine> Gets(string sSQL, Int64 nUserID);
        string Delete(int id, Int64 nUserID);
        FabricMachine Save(FabricMachine oFabricMachine, Int64 nUserID);
        FabricMachine ActiveInActive(int id, bool bIsActive);
        FabricMachine MakeFree(FabricMachine oFabricMachine, Int64 nUserID);
        FabricMachine LoomMachineRestore(int id);
        FabricMachine HoldBeamFinishForLoomProcess(int id);

        
    }
    #endregion
    
  
    #region Fabric Machine Dashboard
    public class FabricMachineDashboard : BusinessObject
    {
        public FabricMachineDashboard()
        {
            WarpingTotal = 0;
            WarpingPlanning = 0;
            WarpingRunning = 0;
            WarpingBreak = 0;
            WarpingFree = 0;

            SizingTotal = 0;
            SizingPlanning = 0;
            SizingRunning = 0;
            SizingBreak = 0;
            SizingFree = 0;

            LoomTotal = 0;
            LoomPlanning = 0;
            LoomRunning = 0;
            LoomBreak = 0;
            LoomFree = 0;
        }

        #region Properties
        public int WarpingTotal { get; set; }
        public int WarpingPlanning { get; set; }
        public int WarpingRunning { get; set; }
        public int WarpingBreak { get; set; }
        public int WarpingFree { get; set; }

        public int SizingTotal { get; set; }
        public int SizingPlanning { get; set; }
        public int SizingRunning { get; set; }
        public int SizingBreak { get; set; }
        public int SizingFree { get; set; }

        public int LoomTotal { get; set; }
        public int LoomPlanning { get; set; }
        public int LoomRunning { get; set; }
        public int LoomBreak { get; set; }
        public int LoomFree { get; set; }     
        #endregion
    }
    #endregion
}
