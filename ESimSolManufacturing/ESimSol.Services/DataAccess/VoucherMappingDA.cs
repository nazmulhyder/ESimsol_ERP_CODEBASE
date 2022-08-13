using System;
using System.Data;
using System.Data.SqlClient;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.DataAccess;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class VoucherMappingDA
    {
        public VoucherMappingDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, VoucherMapping oVoucherMapping, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC[SP_IUD_VoucherMapping]" + "%n, %s, %s, %n, %n, %n, %n, %n",
                                    oVoucherMapping.VoucherMappingID, oVoucherMapping.TableName, oVoucherMapping.PKColumnName, oVoucherMapping.PKValue, oVoucherMapping.VoucherSetupInt, oVoucherMapping.VoucherID, nUserID, (int)eEnumDBOperation);
        }

        public static void Delete(TransactionContext tc, VoucherMapping oVoucherMapping, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC[SP_IUD_VoucherMapping]" + "%n, %s, %s, %n, %n, %n, %n, %n",
                                    oVoucherMapping.VoucherMappingID, oVoucherMapping.TableName, oVoucherMapping.PKColumnName, oVoucherMapping.PKValue, oVoucherMapping.VoucherSetupInt, oVoucherMapping.VoucherID, nUserID, (int)eEnumDBOperation);
        }
        #endregion

        #region Get & Exist Function

        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM VoucherMapping WHERE VoucherMappingID=%n", nID);
        }

        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM VoucherMapping");
        }

        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion
    }
}
