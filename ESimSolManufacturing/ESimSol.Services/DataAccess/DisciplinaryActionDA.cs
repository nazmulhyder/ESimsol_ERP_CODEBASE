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
    public class DisciplinaryActionDA
    {
        public DisciplinaryActionDA() { }

        #region Insert Update Delete Function

        public static IDataReader IUD(TransactionContext tc, DisciplinaryAction oDisciplinaryAction, Int64 nUserID, int nDBOperation)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_DisciplinaryAction] %n,%n,%n,%n,%n,%s,%n,%n,%d,%n,%d,%n,%n,%n,%n, %n",
                  oDisciplinaryAction.DisciplinaryActionID, oDisciplinaryAction.ActionType, oDisciplinaryAction.EmployeeID, oDisciplinaryAction.SalaryHeadID, oDisciplinaryAction.PaymentCycle, oDisciplinaryAction.Description, oDisciplinaryAction.Amount, oDisciplinaryAction.ProductionAmount, oDisciplinaryAction.ExecutedOn, oDisciplinaryAction.ApproveBy, oDisciplinaryAction.ApproveByDate, oDisciplinaryAction.ProcessID, oDisciplinaryAction.IsLock, nUserID, nDBOperation, oDisciplinaryAction.CompAmount);
        }

        public static IDataReader UploadXL(TransactionContext tc, DisciplinaryAction oDA, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_Upload_DisciplinaryAction] %s,%d,%s,%n,%n,%n",
                   oDA.EmployeeCode,
                   oDA.ExecutedOn,
                   oDA.SalaryHeadName,
                   oDA.Amount,
                   oDA.CompAmount,
                   nUserID);
        }

        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_DisciplinaryAction WHERE DisciplinaryActionID=%n", nID);
        }

        public static IDataReader Gets( string sSQL,TransactionContext tc)
        {
            return tc.ExecuteReader(sSQL);
        }

        public static IDataReader GetsForDAProcess(string sParams, TransactionContext tc)
        {

            string sEmployeeIds = sParams.Split('~')[0];
            string sDepartmentIds = sParams.Split('~')[1];
            string sDesignationIds = sParams.Split('~')[2];
            bool IsProd = Convert.ToBoolean(sParams.Split('~')[3]);
            DateTime StartDate_Prod = Convert.ToDateTime(sParams.Split('~')[4]);
            DateTime EndDate_Prod = Convert.ToDateTime(sParams.Split('~')[5]);
            bool IsExe = Convert.ToBoolean(sParams.Split('~')[6]);
            DateTime StartDate_Exe = Convert.ToDateTime(sParams.Split('~')[7]);
            DateTime EndDate_Exe = Convert.ToDateTime(sParams.Split('~')[8]);
            int nLoadRecords = Convert.ToInt32(sParams.Split('~')[9]);
            int nRowLength = Convert.ToInt32(sParams.Split('~')[10]);


            return tc.ExecuteReader("EXEC [SP_Process_DisciplinaryAction]%s,%s,%s,%b,%d,%d,%b,%d,%d,%n,%n",
                   sEmployeeIds, sDepartmentIds,sDesignationIds, IsProd, StartDate_Prod, EndDate_Prod, IsExe, StartDate_Exe, EndDate_Exe, nLoadRecords, nRowLength);
        }

        #endregion
    }
}
