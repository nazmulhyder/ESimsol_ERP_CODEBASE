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
    public class EmployeeTypeDA
    {
        public EmployeeTypeDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, EmployeeType oEmployeeType, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_EmployeeType]"
                                    + "%n, %n, %s, %u, %s, %b, %n, %n, %n", 
                                    oEmployeeType.EmployeeTypeID, oEmployeeType.Code, oEmployeeType.Name, oEmployeeType.NameInBangla, oEmployeeType.Description, oEmployeeType.IsActive, (int)oEmployeeType.EmployeeGrouping, nUserID, (int)eEnumDBOperation);
        }

        public static void Delete(TransactionContext tc, EmployeeType oEmployeeType, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_EmployeeType]"
                                    + "%n, %n, %s, %u, %s, %b, %n, %n, %n", 
                                    oEmployeeType.EmployeeTypeID, oEmployeeType.Code, oEmployeeType.Name, oEmployeeType.NameInBangla, oEmployeeType.Description, oEmployeeType.IsActive, (int)oEmployeeType.EmployeeGrouping, nUserID, (int)eEnumDBOperation);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM EmployeeType WHERE EmployeeTypeID=%n", nID);
        }

        public static IDataReader Gets(string sSQL, TransactionContext tc)
        {
            return tc.ExecuteReader(sSQL);
        }
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM EmployeeType");
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }

        public static void ChangeActiveStatus(TransactionContext tc, EmployeeType oEmployeeType)
        {
            tc.ExecuteNonQuery("Update EmployeeType SET IsActive=%b WHERE EmployeeTypeID=%n", oEmployeeType.IsActive, oEmployeeType.EmployeeTypeID);
        }
        #endregion
    }
}
