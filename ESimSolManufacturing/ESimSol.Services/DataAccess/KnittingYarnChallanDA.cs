using System;
using ICS.Core.Framework;
using ICS.Core.Utility;
using ICS.Core;
using ESimSol.BusinessObjects;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.OleDb;
using ICS.Core.DataAccess;

namespace ESimSol.Services.DataAccess
{
    public class KnittingYarnChallanDA
    {
        #region Insert Update Delete Function

        public static IDataReader InsertUpdate(TransactionContext tc, KnittingYarnChallan oKnittingYarnChallan, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_KnittingYarnChallan]"
                                   + "%n,%n,%s,%d,%s,%s,%s,%s,%n,%n,%n",
                                   oKnittingYarnChallan.KnittingYarnChallanID, oKnittingYarnChallan.KnittingOrderID, oKnittingYarnChallan.ChallanNo, oKnittingYarnChallan.ChallanDate, oKnittingYarnChallan.DriverName, oKnittingYarnChallan.CarNumber, oKnittingYarnChallan.DeliveryPoint, oKnittingYarnChallan.Remarks, oKnittingYarnChallan.ApprovedBy, nUserID, (int)eEnumDBOperation);
        }
        public static IDataReader Approve(TransactionContext tc, KnittingYarnChallan oKnittingYarnChallan, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_KnittingYarnChallan]"
                                   + "%n,%n,%s,%d,%s,%s,%s,%s,%n,%n,%n",
                                   oKnittingYarnChallan.KnittingYarnChallanID, oKnittingYarnChallan.KnittingOrderID, oKnittingYarnChallan.ChallanNo, oKnittingYarnChallan.ChallanDate, oKnittingYarnChallan.DriverName, oKnittingYarnChallan.CarNumber, oKnittingYarnChallan.DeliveryPoint, oKnittingYarnChallan.Remarks, oKnittingYarnChallan.ApprovedBy, nUserID, (int)eEnumDBOperation);
        }

        public static void Delete(TransactionContext tc, KnittingYarnChallan oKnittingYarnChallan, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_KnittingYarnChallan]"
                                   + "%n,%n,%s,%d,%s,%s,%s,%s,%n,%n,%n",
                                   oKnittingYarnChallan.KnittingYarnChallanID, oKnittingYarnChallan.KnittingOrderID, oKnittingYarnChallan.ChallanNo, oKnittingYarnChallan.ChallanDate, oKnittingYarnChallan.DriverName, oKnittingYarnChallan.CarNumber, oKnittingYarnChallan.DeliveryPoint, oKnittingYarnChallan.Remarks, oKnittingYarnChallan.ApprovedBy, nUserID, (int)eEnumDBOperation);
        }

        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_KnittingYarnChallan WHERE KnittingYarnChallanID=%n", nID);
        }
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM KnittingYarnChallan");
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }

        #endregion
    }

}
