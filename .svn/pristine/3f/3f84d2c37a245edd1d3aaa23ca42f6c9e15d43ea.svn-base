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
    public class EmployeeSalaryDetailDA
    {
        public EmployeeSalaryDetailDA() { }

        #region Insert Update Delete Function

        public static IDataReader IUD(TransactionContext tc, EmployeeSalaryDetail oEmployeeSalaryDetail, Int64 nUserID, int nDBOperation)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_EmployeeSalaryDetail] ",
                   oEmployeeSalaryDetail.ESDID, oEmployeeSalaryDetail.EmployeeSalaryID,
                   oEmployeeSalaryDetail.SalaryHeadID, oEmployeeSalaryDetail.Amount,
                   nUserID, nDBOperation);

        }

        #endregion

        #region Get & Exist Function

        public static IDataReader Get(int nEmployeeSalaryDetailID, TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_EmployeeSalaryDetail WHERE EmployeeSalaryDetailID=%n", nEmployeeSalaryDetailID);
        }
        public static IDataReader Get(string sSQL, TransactionContext tc)
        {
            return tc.ExecuteReader(sSQL);
        }
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_EmployeeSalaryDetail");
        }
        public static IDataReader Gets(string sSQL, TransactionContext tc)
        {
            return tc.ExecuteReader(sSQL);
        }
        public static IDataReader GetsForPaySlip(string sEmployeeSalaryIDs, TransactionContext tc)
        {
            return tc.ExecuteReader("EXEC [SP_Process_PaySlip] %s", sEmployeeSalaryIDs);
        }

        #endregion
    }
}
