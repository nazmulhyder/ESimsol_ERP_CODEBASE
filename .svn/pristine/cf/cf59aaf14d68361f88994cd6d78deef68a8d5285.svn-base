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
    public class YarnRequisitionDA
    {
        #region Insert Update Delete Function

        public static IDataReader InsertUpdate(TransactionContext tc, YarnRequisition oYarnRequisition, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_YarnRequisition]" + "%n, %n, %s, %d, %n, %n, %n, %n, %s, %n, %n",
                                   oYarnRequisition.YarnRequisitionID, oYarnRequisition.BUID, oYarnRequisition.RequisitionNo, oYarnRequisition.RequisitionDate, oYarnRequisition.MerchandiserID, oYarnRequisition.BusinessSessionID, oYarnRequisition.BuyerID, oYarnRequisition.ApprovedBy, oYarnRequisition.Remarks, nUserID, (int)eEnumDBOperation);
        }

        public static void Delete(TransactionContext tc, YarnRequisition oYarnRequisition, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_YarnRequisition]" + "%n, %n, %s, %d, %n, %n, %n, %n, %s, %n, %n ",
                                   oYarnRequisition.YarnRequisitionID, oYarnRequisition.BUID, oYarnRequisition.RequisitionNo, oYarnRequisition.RequisitionDate, oYarnRequisition.MerchandiserID, oYarnRequisition.BusinessSessionID, oYarnRequisition.BuyerID, oYarnRequisition.ApprovedBy, oYarnRequisition.Remarks, nUserID, (int)eEnumDBOperation);
        }

        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_YarnRequisition WHERE YarnRequisitionID=%n", nID);
        }
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_YarnRequisition ");
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }

        #endregion
    }

}
