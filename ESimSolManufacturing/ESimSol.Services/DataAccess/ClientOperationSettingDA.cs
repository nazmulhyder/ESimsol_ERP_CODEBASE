using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Framework;
using ICS.Core.Utility;
using ICS.Core.DataAccess;


namespace ESimSol.Services.DataAccess
{
    public class ClientOperationSettingDA
    {
        
        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, ClientOperationSetting oClientOperationSetting, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_ClientOperationSetting]" + "%n, %n, %n, %s, %n, %n",
                                    oClientOperationSetting.ClientOperationSettingID, (int)oClientOperationSetting.OperationType, (int)oClientOperationSetting.DataType, oClientOperationSetting.Value, nUserID, (int)eEnumDBOperation);

        }

        public static void Delete(TransactionContext tc, ClientOperationSetting oClientOperationSetting, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_ClientOperationSetting]" + "%n, %n, %n, %s, %n, %n",
                                    oClientOperationSetting.ClientOperationSettingID, (int)oClientOperationSetting.OperationType, (int)oClientOperationSetting.DataType, oClientOperationSetting.Value, nUserID, (int)eEnumDBOperation);
        }

        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, int nClientOperationSettingID)
        {
            return tc.ExecuteReader("SELECT * FROM ClientOperationSetting WHERE ClientOperationSettingID=%n", nClientOperationSettingID);
        }

        public static IDataReader GetByOperationType(TransactionContext tc, int nOperationTypeID)
        {
            return tc.ExecuteReader("SELECT top 1 * FROM ClientOperationSetting WHERE OperationType=%n", nOperationTypeID);
        }
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM ClientOperationSetting");
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion
    }
    
  
}
