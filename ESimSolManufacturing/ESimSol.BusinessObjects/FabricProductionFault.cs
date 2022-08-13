using System;
using System.IO;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using ICS.Core;
using ICS.Core.Framework;
using ICS.Core.Utility;

namespace ESimSol.BusinessObjects
{
    public class FabricProductionFault
    {
        public FabricProductionFault()
        {
            FPFID = 0;
            FabricFaultType = EnumFabricFaultType.None;
            Name = "";
            IsActive = true;
            ErrorMessage = "";
            BUType = EnumBusinessUnitType.None;
            LastUpdateBy = 0;
            LastUpdateDateTime = DateTime.Now;
            Sequence = 0;
            LastUpdateByName = "";

        }

        #region Properties
        public int FPFID { get; set; }
        public EnumFabricFaultType FabricFaultType { get; set; }
        public EnumBusinessUnitType BUType { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public string ErrorMessage { get; set; }
        public int LastUpdateBy { get; set; }
        public DateTime LastUpdateDateTime { get; set; }
        public string LastUpdateByName { get; set; }
        public int Sequence { get; set; }
        #endregion

        #region Derive Properties
        public string FabricFaultTypeSt
        {
            get
            {
                return FabricFaultTypeObj.GetFabricFaultTypeObjs(this.FabricFaultType);
            }
        }
        public string BUTypeSt
        {
            get
            {
                if (this.BUType == EnumBusinessUnitType.None) return "";
                return EnumObject.jGet(this.BUType);
            }
        }
        public string IsAvtiveSt
        {
            get
            {
                return (this.IsActive == true ? "Active" : "Inactive");
            }
        }
        public string LastUpdateDateTimeInString
        {
            get
            {
                return LastUpdateDateTime.ToString("dd MMM yyyy");
            }
        }
        #endregion

        #region Functions
        public static List<FabricProductionFault> Gets(long nUserID)
        {
            return FabricProductionFault.Service.Gets(nUserID);
        }
        public static List<FabricProductionFault> Gets(string sSQL, long nUserID)
        {
            return FabricProductionFault.Service.Gets(sSQL, nUserID);
        }
        public FabricProductionFault Save(long nUserID)
        {
            return FabricProductionFault.Service.Save(this, nUserID);
        }
        public FabricProductionFault Get(int nEPIDID, long nUserID)
        {
            return FabricProductionFault.Service.Get(nEPIDID, nUserID);
        }
        public string Delete(int nId, long nUserID)
        {
            return FabricProductionFault.Service.Delete(nId, nUserID);
        }
        public FabricProductionFault ActiveOrInactive(int nFPFID, bool bIsActive, long nUserID)
        {
            return FabricProductionFault.Service.ActiveOrInactive(nFPFID, bIsActive, nUserID);
        }
        public List<FabricProductionFault> SaveList(List<FabricProductionFault> oFabricProductionFaults, long nUserID)
        {
            return FabricProductionFault.Service.SaveList(oFabricProductionFaults, nUserID);
        }
        public List<FabricProductionFault> RefreshSequence(List<FabricProductionFault> oFabricProductionFaults, long nUserID)
        {
            return FabricProductionFault.Service.RefreshSequence(oFabricProductionFaults, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IFabricProductionFaultService Service
        {
            get { return (IFabricProductionFaultService)Services.Factory.CreateService(typeof(IFabricProductionFaultService)); }
        }
        #endregion
    }
    #region IFabricProductionFault interface
    public interface IFabricProductionFaultService
    {
        List<FabricProductionFault> Gets(long nUserID);
        List<FabricProductionFault> Gets(string sSQL, long nUserID);
        FabricProductionFault Save(FabricProductionFault oFabricProductionFault, long nUserID);
        FabricProductionFault ActiveOrInactive(int nFPFID, bool bIsActive, long nUserID);
        FabricProductionFault Get(int nEPIDID, long nUserID);
        string Delete(int id, long nUserID);
        List<FabricProductionFault> SaveList(List<FabricProductionFault> oFabricProductionFaults, Int64 nUserID);
        List<FabricProductionFault> RefreshSequence(List<FabricProductionFault> oFabricProductionFaults, long nUserID);
    }
    #endregion

}

#region FabricFaultTypeObj
public class FabricFaultTypeObj
{
    public FabricFaultTypeObj()
    {
        id = 0;
        Value = "";
    }
    public int id { get; set; }
    public string Value { get; set; }
    public static List<FabricFaultTypeObj> Gets()
    {
        List<FabricFaultTypeObj> oFabricFaultTypeObjs = new List<FabricFaultTypeObj>();
        FabricFaultTypeObj oFabricFaultTypeObj = new FabricFaultTypeObj();
        foreach (int oItem in Enum.GetValues(typeof(EnumFabricFaultType)))
        {
            oFabricFaultTypeObj = new FabricFaultTypeObj();
            oFabricFaultTypeObj.id = oItem;
            oFabricFaultTypeObj.Value = GetFabricFaultTypeObjs((EnumFabricFaultType)oItem);
            oFabricFaultTypeObjs.Add(oFabricFaultTypeObj);
        }
        return oFabricFaultTypeObjs;
    }

    public static string GetFabricFaultTypeObjs(EnumFabricFaultType eEnumFabricFaultType)
    {
        string sEnumFabricFaultType = "";
        switch (eEnumFabricFaultType)
        {
            case EnumFabricFaultType.DyeingFault:
                sEnumFabricFaultType = "Dyeing Fault";
                break;
            case EnumFabricFaultType.YarnFault:
                sEnumFabricFaultType = "Yarn Fault";
                break;
            case EnumFabricFaultType.WeaveFault:
                sEnumFabricFaultType = "Weave Fault";
                break;
            case EnumFabricFaultType.Mechanical:
                sEnumFabricFaultType = "Mechanical";
                break;
            case EnumFabricFaultType.Electrical:
                sEnumFabricFaultType = "Electrical";
                break;
            case EnumFabricFaultType.Production:
                sEnumFabricFaultType = "Production";
                break;
            case EnumFabricFaultType.Quality:
                sEnumFabricFaultType = "Quality";
                break;
            case EnumFabricFaultType.Preparatory:
                sEnumFabricFaultType = "Preparatory";
                break;
            default:
                sEnumFabricFaultType = eEnumFabricFaultType.ToString();
                break;
        }
        return sEnumFabricFaultType;
    }
}
#endregion