using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class EmployeeLeaveInfoDA
    {
        public EmployeeLeaveInfoDA() { }


        #region Gets
        public static IDataReader Gets(TransactionContext tc, DateTime dtFrom,DateTime dtTo,int ACSID,int LeaveHeadId,string EmpIds,string DeptIds,string DesignationIds, bool @Param_bReportingPerson, long nID)
        {
            return tc.ExecuteReader("EXEC [SP_Process_EmployeeLeaveInfo]" + " %d, %d, %n, %n, %s, %s, %s,%b", dtFrom, dtTo, ACSID, LeaveHeadId, EmpIds, DeptIds, DesignationIds, @Param_bReportingPerson);
        }
        #endregion
    }
}
