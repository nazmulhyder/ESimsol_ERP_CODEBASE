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
    public class FabricQCGradeDA
    {
        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, FabricQCGrade oFabricQCGrade, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_FabricQCGrade]"
                                   + "%n,%s,%n,%n,%n,%n,%n,%n,%n",
                                   oFabricQCGrade.FabricQCGradeID,
                                   oFabricQCGrade.Name,
                                   (int)oFabricQCGrade.QCGradeType,
                                   oFabricQCGrade.SLNo,
                                   oFabricQCGrade.MinValue,
                                   oFabricQCGrade.MaxValue,
                                   oFabricQCGrade.GradeSL,
                                   nUserID,
                                   (int)eEnumDBOperation);
        }

        public static void Delete(TransactionContext tc, FabricQCGrade oFabricQCGrade, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_FabricQCGrade]"
                                   + "%n,%s,%n,%n,%n,%n,%n,%n,%n",
                                   oFabricQCGrade.FabricQCGradeID,
                                   oFabricQCGrade.Name,
                                   (int)oFabricQCGrade.QCGradeType,
                                   oFabricQCGrade.SLNo,
                                   oFabricQCGrade.MinValue,
                                   oFabricQCGrade.MaxValue,
                                   oFabricQCGrade.GradeSL,
                                   nUserID,
                                   (int)eEnumDBOperation);
        }

        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_FabricQCGrade WHERE FabricQCGradeID=%n", nID);
        }
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_FabricQCGrade");
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }

        #endregion
    }

}
