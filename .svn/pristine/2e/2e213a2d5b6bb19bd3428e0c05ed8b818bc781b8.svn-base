using System;
using System.IO;
using ICS.Core;
using ICS.Core.Framework;
using ICS.Core.Utility;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;


namespace ESimSol.BusinessObjects
{
    #region EmployeeSalaryDetail

    public class RPTSalarySheetDetail : BusinessObject
    {
        public RPTSalarySheetDetail()
        {
            ESDID = 0;
            EmployeeSalaryID = 0;
            SalaryHeadID = 0;
            Amount = 0;
            SalaryHeadName = "";
            Equation = "";
            SalaryHeadType = 0;
            ErrorMessage = "";
            CompAmount = 0;
            SalaryHeadSequence = 0;
        }

        #region Properties
        public int ESDID { get; set; }
        public int EmployeeSalaryID { get; set; }
        public int SalaryHeadID { get; set; }
        public double Amount { get; set; }
        public double CompAmount { get; set; }
        public string ErrorMessage { get; set; }
        public string SalaryHeadName { get; set; }
        public int SalaryHeadSequence { get; set; }

        #endregion

        #region Derived Property
        public string Equation { get; set; }
        public int SalaryHeadType { get; set; }

        public string SalaryHeadNameWithEquation
        {
            get { if (Equation != "") return (SalaryHeadName + "(" + Equation + ")"); else return SalaryHeadName; }
        }

        #endregion

        #region Functions
        public static RPTSalarySheetDetail Get(int id, long nUserID)
        {
            return RPTSalarySheetDetail.Service.Get(id, nUserID);
        }

        public static RPTSalarySheetDetail Get(string sSQL, long nUserID)
        {
            return RPTSalarySheetDetail.Service.Get(sSQL, nUserID);
        }

        public static List<RPTSalarySheetDetail> Gets(long nUserID)
        {
            return RPTSalarySheetDetail.Service.Gets(nUserID);
        }

        public static List<RPTSalarySheetDetail> Gets(string sSQL, long nUserID)
        {
            return RPTSalarySheetDetail.Service.Gets(sSQL, nUserID);
        }

        public static List<RPTSalarySheetDetail> GetEmployeesSalaryDetail(string sBU, string sLocationID, string sDepartmentIDs, string sDesignationIDs, string sSalarySchemeIDs, string sBlockIDs, string sGroupIDs, string sEmpIDs, int nMonthID, int nYear, bool bNewJoin, int IsOutSheet, double nStartSalaryRange, double nEndSalaryRange, bool IsCompliance)
        {
            return RPTSalarySheetDetail.Service.GetEmployeesSalaryDetail(sBU, sLocationID, sDepartmentIDs, sDesignationIDs, sSalarySchemeIDs, sBlockIDs, sGroupIDs, sEmpIDs, nMonthID, nYear, bNewJoin, IsOutSheet, nStartSalaryRange, nEndSalaryRange, IsCompliance);
        }

        #endregion

        #region ServiceFactory
        internal static IRPTSalarySheetDetailService Service
        {
            get { return (IRPTSalarySheetDetailService)Services.Factory.CreateService(typeof(IRPTSalarySheetDetailService)); }
        }

        #endregion
    }
    #endregion

    #region IRPTSalarySheetDetail interface

    public interface IRPTSalarySheetDetailService
    {
        RPTSalarySheetDetail Get(int id, Int64 nUserID);
        RPTSalarySheetDetail Get(string sSQL, Int64 nUserID);
        List<RPTSalarySheetDetail> Gets(Int64 nUserID);
        List<RPTSalarySheetDetail> Gets(string sSQL, Int64 nUserID);
        List<RPTSalarySheetDetail> GetEmployeesSalaryDetail(string sBU, string sLocationID, string sDepartmentIDs, string sDesignationIDs, string sSalarySchemeIDs, string sBlockIDs, string sGroupIDs, string sEmpIDs, int nMonthID, int nYear, bool bNewJoin, int IsOutSheet, double nStartSalaryRange, double nEndSalaryRange, bool IsCompliance);


    }
    #endregion
}
