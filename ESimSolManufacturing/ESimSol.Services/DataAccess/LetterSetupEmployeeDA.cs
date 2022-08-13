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
    public class LetterSetupEmployeeDA
    {
        public LetterSetupEmployeeDA() { }

        #region Insert Update Delete Function

        public static IDataReader IUD(TransactionContext tc, LetterSetupEmployee oLetterSetupEmployee, Int64 nUserID, int nDBOperation)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_LetterSetupEmployee]" + "%n,%n,%n,%u,%u,%u,%n,%n"
                , oLetterSetupEmployee.LSEID
                , oLetterSetupEmployee.LSID
                , oLetterSetupEmployee.EmployeeID
                , oLetterSetupEmployee.Code
                , oLetterSetupEmployee.Body
                , oLetterSetupEmployee.Remark
                , nUserID
                , nDBOperation
            );
        }
        public static IDataReader Delete(TransactionContext tc, LetterSetupEmployee oLetterSetupEmployee, Int64 nUserID, int nDBOperation)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_LetterSetupEmployee]" + "%n,%n,%n,%u,%u,%u,%n,%n"
                , oLetterSetupEmployee.LSEID
                , oLetterSetupEmployee.LSID
                , oLetterSetupEmployee.EmployeeID
                , oLetterSetupEmployee.Code
                , oLetterSetupEmployee.Body
                , oLetterSetupEmployee.Remark
                , nUserID
                , nDBOperation
            );
        }
        public static IDataReader GetEmpLetter(TransactionContext tc, LetterSetupEmployee oLetterSetupEmployee, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_LetterSetupDataFromDB]" + "%n,%n"
                , oLetterSetupEmployee.EmployeeID
                , oLetterSetupEmployee.LSID
            );
        }

        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        public static IDataReader Gets(string sSQL, TransactionContext tc)
        {
            return tc.ExecuteReader(sSQL);
        }

        public static IDataReader Get(TransactionContext tc, int nLSEID)
        {
            return tc.ExecuteReader("SELECT * FROM View_LetterSetupEmployee WHERE LSEID=%n", nLSEID);
        }

        #endregion
    }
}


