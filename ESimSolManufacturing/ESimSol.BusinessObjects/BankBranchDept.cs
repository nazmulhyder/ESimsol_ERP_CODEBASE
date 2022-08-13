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

    #region BankBranchDept
    public class BankBranchDept : BusinessObject
    {
        public BankBranchDept()
        {
            BankBranchDeptID = 0;
            OperationalDept = EnumOperationalDept.Marketing;
            BankBranchID = 0;
            BranchName = "";
            BranchCode = "";
            ErrorMessage = "";
        }

        #region Property
        public int BankBranchDeptID { get; set; }
        public EnumOperationalDept OperationalDept { get; set; }
        public int OperationalDeptInInt { get; set; }
        public string OperationalDeptName { get; set; }
        public int BankBranchID { get; set; }
        public string BranchName { get; set; }
        public string BranchCode { get; set; }
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property
        #endregion

        #region Functions
        public static List<BankBranchDept> Gets(int nID, long nUserID) //nID is contractor ID
        {
            return BankBranchDept.Service.Gets(nID, nUserID);
        }
        public static List<BankBranchDept> Gets(string sSQL, long nUserID)
        {
            return BankBranchDept.Service.Gets(sSQL, nUserID);
        }
        public BankBranchDept Get(int id, long nUserID)
        {
            return BankBranchDept.Service.Get(id, nUserID);
        }
        public BankBranchDept Save(long nUserID)
        {
            return BankBranchDept.Service.Save(this, nUserID);
        }
        public string Delete(int id, long nUserID)
        {
            return BankBranchDept.Service.Delete(id, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IBankBranchDeptService Service
        {
            get { return (IBankBranchDeptService)Services.Factory.CreateService(typeof(IBankBranchDeptService)); }
        }
        #endregion
    }
    #endregion

    #region IBankBranchDept interface
    public interface IBankBranchDeptService
    {
        BankBranchDept Get(int id, Int64 nUserID);
        List<BankBranchDept> Gets(int nID, Int64 nUserID);
        List<BankBranchDept> Gets(string sSQL, Int64 nUserID);
        string Delete(int id, Int64 nUserID);
        BankBranchDept Save(BankBranchDept oBankBranchDept, Int64 nUserID);
    }
    #endregion
  
    
}
