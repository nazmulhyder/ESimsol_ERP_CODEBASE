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
    #region FabricDispo
    public class FabricDispo : BusinessObject
    {
        public FabricDispo()
        {
            FabricDispoID = 0;
            Code = "";
            FabricOrderType = EnumFabricRequestType.None;
            BusinessUnitType = EnumBusinessUnitType.None;
            CodeLength = 0;
            ErrorMessage = "";
        }

        #region Properties
        public int FabricDispoID { get; set; }
        public string Code { get; set; }
        public EnumFabricRequestType FabricOrderType { get; set; }
        public EnumBusinessUnitType BusinessUnitType { get; set; }
        public bool IsReProduction { get; set; }
        public bool IsYD { get; set; }
        public int CodeLength { get; set; }
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Properties
        public int FabricOrderTypeInt { get; set; }
        public string FabricOrderTypeST
        {
            get
            {
                return FabricOrderType.ToString();
            }
        }
        public int BusinessUnitTypeInt { get; set; }

        public string BusinessUnitTypeST
        {
            get
            {
                return BusinessUnitType.ToString();
            }
        }
        #endregion

        #region Functions
        public static List<FabricDispo> Gets(long nUserID)
        {
            return FabricDispo.Service.Gets(nUserID);
        }
        public static List<FabricDispo> Gets(string sSQL, Int64 nUserID)
        {
            return FabricDispo.Service.Gets(sSQL, nUserID);
        }
        public FabricDispo Get(int nId, long nUserID)
        {
            return FabricDispo.Service.Get(nId,nUserID);
        }
        public FabricDispo Save(FabricDispo oFabricDispo, long nUserID)
        {
            return FabricDispo.Service.Save(this, nUserID);
        }
        public string Delete(int nId, long nUserID)
        {
            return FabricDispo.Service.Delete(nId, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IFabricDispoService Service
        {
            get { return (IFabricDispoService)Services.Factory.CreateService(typeof(IFabricDispoService)); }
        }
        #endregion
    }
    #endregion

    #region IFabricDispo interface
    public interface IFabricDispoService
    {
        FabricDispo Get(int id, long nUserID);
        List<FabricDispo> Gets(long nUserID);
        List<FabricDispo> Gets(string sSQL, Int64 nUserID);
        string Delete(int id, long nUserID);
        FabricDispo Save(FabricDispo oFabricDispo, long nUserID);
    }
    #endregion
}
