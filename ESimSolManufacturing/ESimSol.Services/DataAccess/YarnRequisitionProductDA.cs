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
    public class YarnRequisitionProductDA
    {
        #region Insert Update Delete Function

        public static IDataReader InsertUpdate(TransactionContext tc, YarnRequisitionProduct oYarnRequisitionProduct, EnumDBOperation eEnumDBOperation, string sYarnRequisitionProductIDs, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_YarnRequisitionProduct]" + "%n, %n, %n, %s, %n, %n, %n, %n, %s",
                                   oYarnRequisitionProduct.YarnRequisitionProductID, oYarnRequisitionProduct.YarnRequisitionID, oYarnRequisitionProduct.YarnID, oYarnRequisitionProduct.YarnCount, oYarnRequisitionProduct.RequisitionQty, oYarnRequisitionProduct.MUnitID, nUserID, (int)eEnumDBOperation, sYarnRequisitionProductIDs);
        }

        public static void Delete(TransactionContext tc, YarnRequisitionProduct oYarnRequisitionProduct, EnumDBOperation eEnumDBOperation, string sYarnRequisitionProductIDs, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_YarnRequisitionProduct]" + "%n, %n, %n, %s, %n, %n, %n, %n, %s",
                                   oYarnRequisitionProduct.YarnRequisitionProductID, oYarnRequisitionProduct.YarnRequisitionID, oYarnRequisitionProduct.YarnID, oYarnRequisitionProduct.YarnCount, oYarnRequisitionProduct.RequisitionQty, oYarnRequisitionProduct.MUnitID, nUserID, (int)eEnumDBOperation, sYarnRequisitionProductIDs);
        }

        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_YarnRequisitionProduct WHERE YarnRequisitionProductID=%n", nID);
        }
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_YarnRequisitionProduct ");
        }
        public static IDataReader Gets(TransactionContext tc, int nYarnRequisitionID)
        {
            return tc.ExecuteReader("SELECT * FROM View_YarnRequisitionProduct WHERE YarnRequisitionID=%n ", nYarnRequisitionID);
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }

        #endregion
    }

}
