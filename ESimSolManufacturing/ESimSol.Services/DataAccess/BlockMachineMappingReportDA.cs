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
    public class BlockMachineMappingReportDA
    {
        public BlockMachineMappingReportDA() { }

        #region Get Function

        public static IDataReader Gets(TransactionContext tc, string sParams)
        {
            DateTime dtStartDate = Convert.ToDateTime(sParams.Split('~')[0]);
            DateTime dtEndDate = Convert.ToDateTime(sParams.Split('~')[1]);
            string sBMMIDs = Convert.ToString(sParams.Split('~')[2]);
            int nDepartmentID = Convert.ToInt32(sParams.Split('~')[3]);
            int nEmployeeID = Convert.ToInt32(sParams.Split('~')[4]);
            bool IsDate = Convert.ToBoolean(sParams.Split('~')[5]);
            string sStyleNos = Convert.ToString(sParams.Split('~')[6]);

            return tc.ExecuteReader("EXEC [SP_Rpt_BlockMachineProduction] %d,%d,%s,%n,%n,%b,%s",

                  dtStartDate, dtEndDate, sBMMIDs, nDepartmentID, nEmployeeID, IsDate, sStyleNos);
        }

        #endregion


    }
}
