using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class DyeingCapacityDA
    {
        public DyeingCapacityDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, DyeingCapacity oDyeingCapacity, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_DyeingCapacity]"
                                    + "%n, %n, %n, %n, %n, %n, %s,%n, %n,%n",
                                    oDyeingCapacity.DyeingCapacityID, (Int16)oDyeingCapacity.DyeingType, oDyeingCapacity.ProductionHour, oDyeingCapacity.ProductionCapacity, (int)oDyeingCapacity.MUnitId, oDyeingCapacity.CapacityPerHour, oDyeingCapacity.Remarks, oDyeingCapacity.BaseProductID, (int)eEnumDBOperation, nUserID);
        }

        public static void Delete(TransactionContext tc, DyeingCapacity oDyeingCapacity, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_DyeingCapacity]"
                                    + "%n, %n, %n, %n, %n, %n, %s,%n, %n,%n",
                                    oDyeingCapacity.DyeingCapacityID, (Int16)oDyeingCapacity.DyeingType, oDyeingCapacity.ProductionHour, oDyeingCapacity.ProductionCapacity, (int)oDyeingCapacity.MUnitId, oDyeingCapacity.CapacityPerHour, oDyeingCapacity.Remarks, oDyeingCapacity.BaseProductID, (int)eEnumDBOperation, nUserID);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_DyeingCapacity WHERE DyeingCapacityID=%n", nID);
        }

        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_DyeingCapacity");
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);//use View_DyeingCapacity
        }
       
        #endregion
    }
}
