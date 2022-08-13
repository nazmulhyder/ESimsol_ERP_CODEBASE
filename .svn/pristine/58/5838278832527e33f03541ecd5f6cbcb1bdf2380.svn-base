using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESimSol.BusinessObjects;
using ICS.Core.Utility;
using ICS.Core.DataAccess;
using System.Data;
using System.Data.SqlClient;


namespace ESimSol.Services.DataAccess
{
    public class EmployeeDatabaseDA
    {
        public EmployeeDatabaseDA() { }

        #region Get & Exist Function
        public static IDataReader Gets(TransactionContext tc, string sParam, Int64 nUserId)
        {
            int nCount = sParam.Split('~').Length;
            List<Employee> oEmployees = new List<Employee>();
            string sName = Convert.ToString(sParam.Split('~')[0]).TrimStart(' ');
            string sCode = Convert.ToString(sParam.Split('~')[1]).TrimStart(' ');
            string sASID = Convert.ToString(sParam.Split('~')[2]);
            string sLocationID = Convert.ToString(sParam.Split('~')[3]);
            string sdepartmentID = Convert.ToString(sParam.Split('~')[4]);
            string sdesignationID = Convert.ToString(sParam.Split('~')[5]);
            string sGender = Convert.ToString(sParam.Split('~')[6]);
            int nEmployeeType = Convert.ToInt32(sParam.Split('~')[7]);
            int nShift = Convert.ToInt32(sParam.Split('~')[8]);
            int nIsActive = Convert.ToInt32(sParam.Split('~')[9]);
            int nIsUnOfficial = Convert.ToInt32(sParam.Split('~')[10]);
            int nIsInActive = Convert.ToInt32(sParam.Split('~')[11]);
            string sLastEmployeeIDs = sParam.Split('~')[12];
            int nbIsUser = Convert.ToInt32(sParam.Split('~')[13]);
            int nbIsOfficial = Convert.ToInt32(sParam.Split('~')[14]);
            int nCardStatus = Convert.ToInt32(sParam.Split('~')[15]);
            int nCardNotAsigned = Convert.ToInt32(sParam.Split('~')[16]);
            int nWorkingStatus = Convert.ToInt32(sParam.Split('~')[17]);
            int nSSNotAsigned = Convert.ToInt32(sParam.Split('~')[18]);// SS=Salary Structure
            string sStartDate = sParam.Split('~')[19];
            string sEndDate = sParam.Split('~')[20];
            int nDateType = Convert.ToInt32(sParam.Split('~')[21]);
            bool bIsJoiningDate = Convert.ToBoolean(sParam.Split('~')[22]);
            DateTime dtDateFrom = Convert.ToDateTime(sParam.Split('~')[23]);
            DateTime dtDateTo = Convert.ToDateTime(sParam.Split('~')[24]);
            string sEnrollNo = "";
            if (nCount >= 27) { sEnrollNo = Convert.ToString(sParam.Split('~')[25]).TrimStart(' '); }
            bool bIsnotEnroll = false;
            if (nCount >= 28) { bIsnotEnroll = Convert.ToBoolean(sParam.Split('~')[26]); }
            short nCategory = 0;
            if (nCount >= 29) { nCategory = Convert.ToInt16(sParam.Split('~')[27]); }
            string sBUIDs = "";
            if (nCount >= 30) { sBUIDs = sParam.Split('~')[28]; }
            string sPresentAddress = "";
            if (nCount >= 31) { sPresentAddress = sParam.Split('~')[29]; }
            string sPermanentAddress = "";
            if (nCount >= 32) { sPermanentAddress = sParam.Split('~')[30]; }
            string sEmployeeBlock = "";
            if (nCount >= 33) { sEmployeeBlock = sParam.Split('~')[31]; }
            string sEmployeeGroup = "";
            if (nCount >= 34) { sEmployeeGroup = sParam.Split('~')[32]; }


            string sMaritalStatus = "";
            if (nCount >= 35) { sMaritalStatus = sParam.Split('~')[33]; }
            double nStartSalaryRange = 0;
            if (nCount >= 36) { nStartSalaryRange = Convert.ToDouble(sParam.Split('~')[34]); }
            double nEndSalaryRange = 0;
            if (nCount >= 37) { nEndSalaryRange = Convert.ToDouble(sParam.Split('~')[35]); }
            string sSalarySchemeIDs = "";
            if (nCount >= 38) { sSalarySchemeIDs = sParam.Split('~')[36]; }
            bool bIsComp = false;
            if (nCount >= 39) { bIsComp = Convert.ToBoolean(sParam.Split('~')[37]); }

            return tc.ExecuteReader("EXEC [SP_Rpt_EmployeeDatabase]"
                        + "%s, %s, %s, %s, %s , %s, %s, %n, %n, %b , %b, %b, %s, %b, %b , %n, %n, %n, %n, %s, %s, %n, %b, %d, %d, %s, %b, %n, %s, %s, %s, %s, %s, %s, %n, %n, %s, %b, %n",
                        sName,
                        sCode,
                        sASID,
                        sLocationID,
                        sdepartmentID,

                        sdesignationID,
                        sGender,
                        nEmployeeType ,
                        nShift ,
                        Convert.ToBoolean(nIsActive) ,

                        Convert.ToBoolean(nIsUnOfficial),
                        Convert.ToBoolean(nIsInActive) ,
                        sLastEmployeeIDs ,
                        Convert.ToBoolean(nbIsUser) ,
                        Convert.ToBoolean(nbIsOfficial), 

                        nCardStatus,
                        nCardNotAsigned,
                        nWorkingStatus ,
                        nSSNotAsigned ,
                        sStartDate, 

                        sEndDate ,
                        nDateType ,
                        bIsJoiningDate,
                        dtDateFrom, 
                        dtDateTo ,

                        sEnrollNo,
                        bIsnotEnroll,
                        nCategory,
                        sBUIDs,
                        sPresentAddress ,

                        sPermanentAddress,
                        sEmployeeBlock ,
                        sEmployeeGroup,
                        sMaritalStatus ,
                        nStartSalaryRange, 

                        nEndSalaryRange,
                        sSalarySchemeIDs,
                        bIsComp,
                        nUserId
                      );
        }
        #endregion
    }
}
