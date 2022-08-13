using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class DyeingPeriodConfigDA
    {
        public DyeingPeriodConfigDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, DyeingPeriodConfig oDyeingPeriodConfig, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_DyeingPeriodConfig]"
                                    + "%n, %n, %n, %n, %s, %n, %n",
                                    oDyeingPeriodConfig.DyeingPeriodConfigID, oDyeingPeriodConfig.ProductID, oDyeingPeriodConfig.DyeingCapacityID, oDyeingPeriodConfig.ReqDyeingPeriod, oDyeingPeriodConfig.Remarks, (int)eEnumDBOperation, nUserID);
        }

        public static void Delete(TransactionContext tc, DyeingPeriodConfig oDyeingPeriodConfig, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_DyeingPeriodConfig]"
                                    + "%n, %n, %n, %n, %s, %n, %n",
                                    oDyeingPeriodConfig.DyeingPeriodConfigID, oDyeingPeriodConfig.ProductID, oDyeingPeriodConfig.DyeingCapacityID, oDyeingPeriodConfig.ReqDyeingPeriod, oDyeingPeriodConfig.Remarks, (int)eEnumDBOperation, nUserID);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_DyeingPeriodConfig WHERE DyeingPeriodConfigID=%n", nID);
        }

        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_DyeingPeriodConfig ORDER BY ProductName ASC");
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        public static IDataReader ProductionForeCast(TransactionContext tc)
        {
            return tc.ExecuteReader("EXEC [SP_Dyeing_Production_Forecast]");
        }
        #endregion
    }
}
