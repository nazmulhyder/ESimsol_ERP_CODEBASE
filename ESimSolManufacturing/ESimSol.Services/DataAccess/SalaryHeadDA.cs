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
    public class SalaryHeadDA
    {
        public SalaryHeadDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, SalaryHead oSalaryHead, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_SalaryHead]"
                                    + "%n, %s,%u, %s, %n, %b, %b, %n, %n,%n", oSalaryHead.SalaryHeadID, oSalaryHead.Name, oSalaryHead.NameInBangla, oSalaryHead.Description, (int)oSalaryHead.SalaryHeadType, oSalaryHead.IsActive,oSalaryHead.IsProcessDependent, nUserID, (int)eEnumDBOperation, oSalaryHead.Sequence);
        }

        public static void Delete(TransactionContext tc, SalaryHead oSalaryHead, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_SalaryHead]"
                                    + "%n, %s,%u, %s, %n, %b, %b, %n, %n,%n", oSalaryHead.SalaryHeadID, oSalaryHead.Name, oSalaryHead.NameInBangla, oSalaryHead.Description, (int)oSalaryHead.SalaryHeadType, oSalaryHead.IsActive, oSalaryHead.IsProcessDependent, nUserID, (int)eEnumDBOperation, oSalaryHead.Sequence);
        }
        #endregion

        public static IDataReader UpDown(TransactionContext tc, SalaryHead oSalaryHead, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_UpDown_SalaryHead]" + "%n, %b"
                , oSalaryHead.SalaryHeadID
                , oSalaryHead.IsUp
            );
        }
        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM SalaryHead WHERE SalaryHeadID=%n", nID);
        }

        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM SalaryHead ORDER BY SalaryHeadType, Sequence");
        }

        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }

        public static void ChangeActiveStatus(TransactionContext tc, SalaryHead oSalaryHead)
        {
            tc.ExecuteNonQuery("Update SalaryHead SET IsActive=%b WHERE SalaryHeadID=%n", oSalaryHead.IsActive, oSalaryHead.SalaryHeadID);
        }
        #endregion
    }
}
