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
    #region FNQCResultSetup
    
    public class FNQCResultSetup : BusinessObject
    {
        public FNQCResultSetup()
        {
            FNQCResultSetupID = 0;
            FNQCParameterID = 0;
            FNTPID = 0;
            SubName = "";
            Value = "";
            TestMethod = "";
            Name = "";
            Code = 0;
            Note = "";
            DBUserID = 0;
            DBUserName = "";
            LastUpdateBy = 0;
            LastUpdateByName = "";
            SLNo = 0;
            FnQCTestGroupID = 0;
            FnQCTestGroupName = "";
            ErrorMessage = "";
        }

        #region Properties

        public int FNQCResultSetupID { get; set; }
        public int FNQCParameterID { get; set; }
        public int FNTPID { get; set; }
        public string SubName { get; set; }
        public string TestMethod { get; set; }
        public string Value { get; set; }
        public string Note { get; set; }
        public string Name { get; set; }
        public int Code { get; set; }
        public int SLNo { get; set; }
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

        public static List<FNQCResultSetup> Gets(long nUserID)
        {
            return FNQCResultSetup.Service.Gets(nUserID);
        }
        public static List<FNQCResultSetup> Gets(string sSQL, Int64 nUserID)
        {
            return FNQCResultSetup.Service.Gets(sSQL, nUserID);
        }
        public FNQCResultSetup Get(int nId, long nUserID)
        {
            return FNQCResultSetup.Service.Get(nId,nUserID);
        }
        public FNQCResultSetup Save(FNQCResultSetup oFNQCResultSetup, long nUserID)
        {
            return FNQCResultSetup.Service.Save(oFNQCResultSetup, nUserID);
        }
        public List<FNQCResultSetup> SaveAll(List<FNQCResultSetup> oFNQCResultSetups, long nUserID)
        {
            return FNQCResultSetup.Service.SaveAll(oFNQCResultSetups, nUserID);
        }
        public string Delete(int nId, long nUserID)
        {
            return FNQCResultSetup.Service.Delete(nId, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IFNQCResultSetupService Service
        {
            get { return (IFNQCResultSetupService)Services.Factory.CreateService(typeof(IFNQCResultSetupService)); }
        }
        #endregion
    }
    #endregion

    #region IFNQCResultSetup interface
    
    public interface IFNQCResultSetupService
    {
        FNQCResultSetup Get(int id, long nUserID);
        List<FNQCResultSetup> Gets(long nUserID);
        List<FNQCResultSetup> Gets(string sSQL, Int64 nUserID);
        string Delete(int id, long nUserID);
        FNQCResultSetup Save(FNQCResultSetup oFNQCResultSetup, long nUserID);
        List<FNQCResultSetup> SaveAll(List<FNQCResultSetup> oFNQCResultSetups, long nUserID);

    }
    #endregion
}

