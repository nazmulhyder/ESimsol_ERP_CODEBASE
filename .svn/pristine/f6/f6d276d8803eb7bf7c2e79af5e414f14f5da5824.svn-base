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
    #region FNQCParameter
    
    public class FNQCParameter : BusinessObject
    {
        public FNQCParameter()
        {
            FNQCParameterID = 0;
            Name = "";
            Code = 0;
            DBUserID = 0;
            DBUserName = "";
            LastUpdateBy = 0;
            LastUpdateByName = "";
            FnQCTestGroupID = 0;
            FnQCTestGroupName = "";
            ErrorMessage = "";
        }

        #region Properties

        public int FNQCParameterID { get; set; }
        public string Name { get; set; }
        public int Code { get; set; }
        public int DBUserID { get; set; }
        public string DBUserName { get; set; }
        public int LastUpdateBy { get; set; }
        public string LastUpdateByName { get; set; }
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property
        public int FnQCTestGroupID { get; set; }
        public string FnQCTestGroupName { get; set; }
        #endregion

        #region Functions

        public static List<FNQCParameter> Gets(long nUserID)
        {
            return FNQCParameter.Service.Gets(nUserID);
        }
        public static List<FNQCParameter> Gets(string sSQL, Int64 nUserID)
        {
            return FNQCParameter.Service.Gets(sSQL, nUserID);
        }
        public FNQCParameter Get(int nId, long nUserID)
        {
            return FNQCParameter.Service.Get(nId,nUserID);
        }
        public FNQCParameter Save(long nUserID)
        {
            return FNQCParameter.Service.Save(this, nUserID);
        }
        public string Delete(int nId, long nUserID)
        {
            return FNQCParameter.Service.Delete(nId, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IFNQCParameterService Service
        {
            get { return (IFNQCParameterService)Services.Factory.CreateService(typeof(IFNQCParameterService)); }
        }
        #endregion
    }
    #endregion

    #region IFNQCParameter interface
    
    public interface IFNQCParameterService
    {
        FNQCParameter Get(int id, long nUserID);
        List<FNQCParameter> Gets(long nUserID);
        List<FNQCParameter> Gets(string sSQL, Int64 nUserID);
        string Delete(int id, long nUserID);
        FNQCParameter Save(FNQCParameter oFNQCParameter, long nUserID);
    }
    #endregion
}
