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
    public class PayrollProcessManagementObjectDA
    {
        public PayrollProcessManagementObjectDA() { }

        #region Insert Update Delete Function

        public static IDataReader IUD(TransactionContext tc, PayrollProcessManagementObject oPayrollProcessManagementObject, Int64 nUserID, int nDBOperation)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_PayrollProcessManagementObject] %n,%n,%n,%n,%n,%n",
                   oPayrollProcessManagementObject.PPMOID, oPayrollProcessManagementObject.PPMID,
                   (int)oPayrollProcessManagementObject.PPMObject, oPayrollProcessManagementObject.ObjectID,
                   nUserID, nDBOperation);

        }

        #endregion

        #region Get & Exist Function

        public static IDataReader Get(int nPayrollProcessManagementObjectID, TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM PayrollProcessManagementObject WHERE PayrollProcessManagementObjectID=%n", nPayrollProcessManagementObjectID);
        }
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM PayrollProcessManagementObject");
        }
        public static IDataReader Gets(string sSQL, TransactionContext tc)
        {
            return tc.ExecuteReader(sSQL);
        }



        #endregion


        #region Compliance Salary

        #region Insert Update Delete Function

        public static IDataReader IUDComp(TransactionContext tc, PayrollProcessManagementObject oPayrollProcessManagementObject, Int64 nUserID, int nDBOperation)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_CompliancePayrollProcessManagementObject] %n,%n,%n,%n,%n,%n",
                   oPayrollProcessManagementObject.PPMOID, oPayrollProcessManagementObject.PPMID,
                   (int)oPayrollProcessManagementObject.PPMObject, oPayrollProcessManagementObject.ObjectID,
                   nUserID, nDBOperation);

        }

        #endregion

        #region Get & Exist Function

        public static IDataReader GetComp(int nPayrollProcessManagementObjectID, TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM CompliancePayrollProcessManagementObject WHERE PayrollProcessManagementObjectID=%n", nPayrollProcessManagementObjectID);
        }
        public static IDataReader GetsComp(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM CompliancePayrollProcessManagementObject");
        }
        public static IDataReader GetsComp(string sSQL, TransactionContext tc)
        {
            return tc.ExecuteReader(sSQL);
        }



        #endregion
        #endregion
    }
}
