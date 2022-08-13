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
    public class EmployeeSalaryStructureDetailDA
    {
        public EmployeeSalaryStructureDetailDA() { }

        #region Insert Update Delete Function

        public static IDataReader IUD(TransactionContext tc, EmployeeSalaryStructureDetail oEmployeeSalaryStructureDetail, Int64 nUserID, int nDBOperation, string sESSDIDs)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_EmployeeSalaryStructureDetail] %n,%n,%n,%n,%n,%n,%n,%s",
                   oEmployeeSalaryStructureDetail.ESSSID, oEmployeeSalaryStructureDetail.ESSID,
                   oEmployeeSalaryStructureDetail.SalaryHeadID, oEmployeeSalaryStructureDetail.Amount,
                   oEmployeeSalaryStructureDetail.CompAmount,nUserID, nDBOperation, sESSDIDs);

        }

        public static IDataReader DeleteSingleSalaryStructureDetail(TransactionContext tc, EmployeeSalaryStructureDetail oEmployeeSalaryStructureDetail, Int64 nUserID, EnumDBOperation eDBOperation, string sESSDIDs)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_EmployeeSalaryStructureDetail] %n,%n,%n,%n,%n,%n,%n,%s",
                   oEmployeeSalaryStructureDetail.ESSSID, oEmployeeSalaryStructureDetail.ESSID,
                   oEmployeeSalaryStructureDetail.SalaryHeadID, oEmployeeSalaryStructureDetail.Amount,
                   oEmployeeSalaryStructureDetail.CompAmount,nUserID, (int)eDBOperation, sESSDIDs);

        }
        #endregion

        #region Get & Exist Function

        public static IDataReader Get(int nESSSID, TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_EmployeeSalaryStructureDetail WHERE ESSSID=%n", nESSSID);
        }
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_EmployeeSalaryStructureDetail");
        }
        public static IDataReader Gets(string sSQL, TransactionContext tc)
        {
            return tc.ExecuteReader(sSQL);
        }


        #endregion
    }
}
