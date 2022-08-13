using ESimSol.BusinessObjects;
using ICS.Core.DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace ESimSol.Services.DataAccess
{
    public class SalaryFieldSetupDetailDA
    {
        public SalaryFieldSetupDetailDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, SalaryFieldSetupDetail oSalaryFieldSetupDetail, EnumDBOperation eEnumDBSalaryFieldSetupDetail, Int64 nUserID, string sIDs)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_SalaryFieldSetupDetail]" + "%n, %n, %n, %s, %n, %n, %s",
                                    oSalaryFieldSetupDetail.SalaryFieldSetupDetailID, oSalaryFieldSetupDetail.SalaryFieldSetupID, (int)oSalaryFieldSetupDetail.SalaryField, oSalaryFieldSetupDetail.Remarks, (int)eEnumDBSalaryFieldSetupDetail, nUserID, sIDs);
        }

        public static void Delete(TransactionContext tc, SalaryFieldSetupDetail oSalaryFieldSetupDetail, EnumDBOperation eEnumDBSalaryFieldSetupDetail, Int64 nUserID, string sIDs)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_SalaryFieldSetupDetail]" + "%n, %n, %n, %s, %n, %n, %s",
                                    oSalaryFieldSetupDetail.SalaryFieldSetupDetailID, oSalaryFieldSetupDetail.SalaryFieldSetupID, (int)oSalaryFieldSetupDetail.SalaryField, oSalaryFieldSetupDetail.Remarks, (int)eEnumDBSalaryFieldSetupDetail, nUserID, sIDs);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Gets(TransactionContext tc, int id)
        {
            return tc.ExecuteReader("SELECT * FROM View_SalaryFieldSetupDetail WHERE SalaryFieldSetupID = %n ORDER BY SalaryFieldSetupDetailID", id);
        }
        #endregion

        
    }
}
