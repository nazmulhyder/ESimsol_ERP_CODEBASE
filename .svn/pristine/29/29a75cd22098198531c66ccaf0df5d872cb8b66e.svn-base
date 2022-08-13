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
    public class FabricProcess
    {
        public FabricProcess()
        {
            FabricProcessID = 0;
            ProcessType = EnumFabricProcess.None;
            Name = "";
            IsYarnDyed = false;
            ErrorMessage = "";
            Params = "";
        }

        #region Properties
        public int FabricProcessID { get; set; }
        public EnumFabricProcess ProcessType { get; set; }
        public string Name { get; set; }
        public bool IsYarnDyed { get; set; }
        public string ErrorMessage { get; set; }
        public string Params { get; set; }
        #endregion

        #region Derive Properties
        public string IsYarnDyedSt
        {
            get
            {
                return (this.IsYarnDyed == true ? "Yard Dyed" : "-");
            }
        }
        public string ProcessTypeSt
        {
            get
            {
                return this.ProcessType.ToString();
            }
        }
        //public string ProcessTypeNameWithType
        //{
        //    get
        //    {
        //        return this.Name + " [" + this.ProcessType.ToString() + "]";
        //    }
        //}
        #endregion

        #region Functions
        public static List<FabricProcess> GetsByFabricNameType(string sName, string nFabricType, int nUserID)
        {
            return FabricProcess.Service.GetsByFabricNameType(sName, nFabricType, nUserID);
        }
        public static List<FabricProcess> Gets(long nUserID)
        {
            return FabricProcess.Service.Gets(nUserID);
        }
        public static List<FabricProcess> Gets(string sSQL, long nUserID)
        {
            return FabricProcess.Service.Gets(sSQL, nUserID);
        }
        public FabricProcess Save(long nUserID)
        {
            return FabricProcess.Service.Save(this, nUserID);
        }
        public FabricProcess Get(int nEPID, long nUserID)
        {
            return FabricProcess.Service.Get(nEPID, nUserID);
        }
        public string Delete(int nId, long nUserID)
        {
            return FabricProcess.Service.Delete(nId, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IFabricProcessService Service
        {
            get { return (IFabricProcessService)Services.Factory.CreateService(typeof(IFabricProcessService)); }
        }
        #endregion
    }

    #region IFabricProcess interface
    public interface IFabricProcessService
    {
        List<FabricProcess> GetsByFabricNameType(string sName, string eFabricType, int nUserID);
        List<FabricProcess> Gets(long nUserID);
        List<FabricProcess> Gets(string sSQL, long nUserID);
        FabricProcess Save(FabricProcess oFabricProcess, long nUserID);
        FabricProcess Get(int nEPID, long nUserID);
        string Delete(int id, long nUserID);
    }
    #endregion
}
