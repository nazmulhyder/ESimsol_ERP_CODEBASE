using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESimSol.BusinessObjects;
using ICS.Core.Utility;
using ICS.Core.DataAccess;
using System.Data;

namespace ESimSol.Services.DataAccess
{
    public class AttendanceSummery_ListDA
    {
        public AttendanceSummery_ListDA() { }

        #region Get

        public static IDataReader Gets(string sParams, TransactionContext tc)
        {
            int nEmpID = Convert.ToInt32(sParams.Split('~')[0]);
            string sDepasrtmentIds = sParams.Split('~')[1];
            DateTime DateFrom = Convert.ToDateTime(sParams.Split('~')[2]);
            DateTime DateTo = Convert.ToDateTime(sParams.Split('~')[3]);
            bool IsDateOfJoin = Convert.ToBoolean(sParams.Split('~')[4]);
            DateTime DateOfJoinFrom = Convert.ToDateTime(sParams.Split('~')[5]);
            DateTime DateOfJoinTo = Convert.ToDateTime(sParams.Split('~')[6]);
            bool IsIsDateOfBirth = Convert.ToBoolean(sParams.Split('~')[7]);
            DateTime DateOfBirthFrom = Convert.ToDateTime(sParams.Split('~')[8]);
            DateTime DateOfBirthTo = Convert.ToDateTime(sParams.Split('~')[9]);
            int nLoadRecords = Convert.ToInt32(sParams.Split('~')[10]);
            int nRowLength = Convert.ToInt32(sParams.Split('~')[11]);

            return tc.ExecuteReader("EXEC [SP_Process_AttendanceSummery_List]%n,%s,%d,%d,%b,%d,%d,%b,%d,%d,%n,%n", nEmpID, sDepasrtmentIds, DateFrom, DateTo
                , IsDateOfJoin, DateOfJoinFrom, DateOfJoinTo, IsIsDateOfBirth, DateOfBirthFrom, DateOfBirthTo, nLoadRecords, nRowLength);
        }

        #endregion


    }
}
