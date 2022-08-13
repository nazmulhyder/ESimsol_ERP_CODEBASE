using System;
using ICS.Core.Framework;
using ICS.Core.Utility;
using ICS.Core;
using ESimSol.BusinessObjects;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.ServiceModel;
using ESimSol.BusinessObjects.ReportingObject;

namespace ESimSol.BusinessObjects
{
    #region FNTreatmentSubProcess
    public class FNTreatmentSubProcess : BusinessObject
    {
        public FNTreatmentSubProcess()
        {
            FNTreatmentSubProcessID = 0;
            FNTPID = 0;
            SubProcessName = "";
            FNProcess = "";
            Code = "";
            ErrorMessage = "";
        }

        #region Property
        public int FNTreatmentSubProcessID { get; set; }
        public int FNTPID { get; set; }
        public string SubProcessName { get; set; }
        public string FNProcess { get; set; }
        public string Code { get; set; }
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property

        //public string RequestDateInString
        //{
        //    get
        //    {
        //        return RequestDate.ToString("dd MMM yyyy");
        //    }
        //}
        #endregion

        #region Functions
        public static List<FNTreatmentSubProcess> Gets(long nUserID)
        {
            return FNTreatmentSubProcess.Service.Gets(nUserID);
        }
        public static List<FNTreatmentSubProcess> Gets(string sSQL, long nUserID)
        {
            return FNTreatmentSubProcess.Service.Gets(sSQL, nUserID);
        }
        public FNTreatmentSubProcess Get(int id, long nUserID)
        {
            return FNTreatmentSubProcess.Service.Get(id, nUserID);
        }
        public FNTreatmentSubProcess Save(long nUserID)
        {
            return FNTreatmentSubProcess.Service.Save(this, nUserID);
        }
        public string Delete(int id, long nUserID)
        {
            return FNTreatmentSubProcess.Service.Delete(id, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IFNTreatmentSubProcessService Service
        {
            get { return (IFNTreatmentSubProcessService)Services.Factory.CreateService(typeof(IFNTreatmentSubProcessService)); }
        }
        #endregion
    }
    #endregion

    #region IFNTreatmentSubProcess interface
    public interface IFNTreatmentSubProcessService
    {
        FNTreatmentSubProcess Get(int id, Int64 nUserID);
        List<FNTreatmentSubProcess> Gets(Int64 nUserID);
        List<FNTreatmentSubProcess> Gets(string sSQL, Int64 nUserID);
        string Delete(int id, Int64 nUserID);
        FNTreatmentSubProcess Save(FNTreatmentSubProcess oFNTreatmentSubProcess, Int64 nUserID);


    }
    #endregion
}
