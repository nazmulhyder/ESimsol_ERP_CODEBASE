using System;
using System.IO;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using System.Drawing;
using ICS.Core;
using ICS.Core.Framework;
using ICS.Core.Utility;


namespace ESimSol.BusinessObjects
{
    #region FUProcess

    public class FNBatchQCFault : BusinessObject
    {
        public FNBatchQCFault()
        {
            FNBQCFaultID = 0;
            FNBatchQCDetailID = 0;
            FPFID = 0;
            FaultPoint =1;
            NoOfFault = 0;
            ErrorMessage = "";
        }

        #region Properties

        public int FNBQCFaultID { get; set; }
        public int FNBatchQCDetailID { get; set; }
        public int FPFID { get; set; }
        public int FaultPoint { get; set; }
        public int NoOfFault { get; set; }
        public string ErrorMessage { get; set; }
        public string Params { get; set; }

        #endregion

        #region Derived Property
        public string FaultName { get; set; }
        public string FaultPointStr { get { return this.FaultPoint.ToString(); } }
        public int FaultTotal { get { return this.FaultPoint * this.NoOfFault; } }

        

        #endregion

        #region Functions

        public static FNBatchQCFault Get(int nFNBQCFaultID, long nUserID)
        {
            return FNBatchQCFault.Service.Get(nFNBQCFaultID, nUserID);
        }
        public static List<FNBatchQCFault> Gets(string sSQL, long nUserID)
        {
            return FNBatchQCFault.Service.Gets(sSQL, nUserID);
        }
        public FNBatchQCFault IUD(int nDBOperation, long nUserID)
        {
            return FNBatchQCFault.Service.IUD(this, nDBOperation, nUserID);
        }
        public static List<FNBatchQCFault> SaveMultipleFNBatchQCFault(List<FNBatchQCFault> oFNBatchQCFaults, long nUserID)
        {
            return FNBatchQCFault.Service.SaveMultipleFNBatchQCFault(oFNBatchQCFaults, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IFNBatchQCFaultService Service
        {
            get { return (IFNBatchQCFaultService)Services.Factory.CreateService(typeof(IFNBatchQCFaultService)); }
        }

        #endregion
    }
    #endregion

    #region IFUProcess interface

    public interface IFNBatchQCFaultService
    {
        FNBatchQCFault Get(int nFNBQCFaultID, Int64 nUserID);
        List<FNBatchQCFault> Gets(string sSQL, Int64 nUserID);
        List<FNBatchQCFault> SaveMultipleFNBatchQCFault(List<FNBatchQCFault> oFNBatchQCFaults, Int64 nUserID);
        FNBatchQCFault IUD(FNBatchQCFault oFNBatchQCDetail, int nDBOperation, Int64 nUserID);
    }
    #endregion
}
