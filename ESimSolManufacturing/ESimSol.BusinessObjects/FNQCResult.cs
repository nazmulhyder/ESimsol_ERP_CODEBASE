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
    #region FNQCResult
    
    public class FNQCResult : BusinessObject
    {
        public FNQCResult()
        {
            FNQCResultID = 0;
            FNQCParameterID = 0;
            FNTPID = 0;
            FNPBatchID = 0;
            SubName = "";
            Value = "";
            ValueResult = "";
            Name = "";
            Code = 0;
            Note = "";
            DBUserID = 0;
            TestMethod = "";
            DBUserName = "";
            LastUpdateBy = 0;
            LastUpdateByName = "";
            SLNo = 0;
            FnQCTestGroupID = 0;
            FnQCTestGroupName = "";
            ErrorMessage = "";
        }

        #region Properties

        public int FNQCResultID { get; set; }
        public int FNQCParameterID { get; set; }
        public int FNTPID { get; set; }
        public int FNPBatchID { get; set; }
        public string SubName { get; set; }
        public string Value { get; set; }
        public string ValueResult { get; set; }
        public string TestMethod { get; set; }
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

        public static List<FNQCResult> Gets(long nUserID)
        {
            return FNQCResult.Service.Gets(nUserID);
        }
        public static List<FNQCResult> Gets(string sSQL, Int64 nUserID)
        {
            return FNQCResult.Service.Gets(sSQL, nUserID);
        }
        public FNQCResult Get(int nId, long nUserID)
        {
            return FNQCResult.Service.Get(nId,nUserID);
        }
        public FNQCResult Save(FNQCResult oFNQCResult, long nUserID)
        {
            return FNQCResult.Service.Save(oFNQCResult, nUserID);
        }
        public List<FNQCResult> SaveAll(List<FNQCResult> oFNQCResults, long nUserID)
        {
            return FNQCResult.Service.SaveAll(oFNQCResults, nUserID);
        }
        public string Delete(int nId, long nUserID)
        {
            return FNQCResult.Service.Delete(nId, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IFNQCResultService Service
        {
            get { return (IFNQCResultService)Services.Factory.CreateService(typeof(IFNQCResultService)); }
        }
        #endregion
    }
    #endregion

    #region IFNQCResult interface
    
    public interface IFNQCResultService
    {
        FNQCResult Get(int id, long nUserID);
        List<FNQCResult> Gets(long nUserID);
        List<FNQCResult> Gets(string sSQL, Int64 nUserID);
        string Delete(int id, long nUserID);
        FNQCResult Save(FNQCResult oFNQCResult, long nUserID);
        List<FNQCResult> SaveAll(List<FNQCResult> oFNQCResults, long nUserID);

    }
    #endregion
}


