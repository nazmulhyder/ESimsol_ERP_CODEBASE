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
    public class YarnRequisitionDetailDA
    {
        #region Insert Update Delete Function

        public static IDataReader InsertUpdate(TransactionContext tc, YarnRequisitionDetail oYarnRequisitionDetail, EnumDBOperation eEnumDBOperation, string sYarnRequisitionDetailIDs, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_YarnRequisitionDetail]" + "%n, %n, %n, %n, %s, %n, %n, %s, %n, %s, %n, %n, %n, %n, %n, %s, %n, %n, %s",
                                   oYarnRequisitionDetail.YarnRequisitionDetailID, oYarnRequisitionDetail.YarnRequisitionID, oYarnRequisitionDetail.TechnicalSheetID, oYarnRequisitionDetail.ColorID, oYarnRequisitionDetail.PentonNo, oYarnRequisitionDetail.GARQty, oYarnRequisitionDetail.FabricID, oYarnRequisitionDetail.GSM, oYarnRequisitionDetail.YarnID, oYarnRequisitionDetail.YarnCount, oYarnRequisitionDetail.YarnPercent, oYarnRequisitionDetail.ActualConsumption, oYarnRequisitionDetail.CostingConsumption, oYarnRequisitionDetail.RequisitionQty, oYarnRequisitionDetail.MUnitID, oYarnRequisitionDetail.Remarks, nUserID, (int)eEnumDBOperation, sYarnRequisitionDetailIDs);
        }

        public static void Delete(TransactionContext tc, YarnRequisitionDetail oYarnRequisitionDetail, EnumDBOperation eEnumDBOperation, string sYarnRequisitionDetailIDs, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_YarnRequisitionDetail]" + "%n, %n, %n, %n, %s, %n, %n, %s, %n, %s, %n, %n, %n, %n, %n, %s, %n, %n, %s",
                                   oYarnRequisitionDetail.YarnRequisitionDetailID, oYarnRequisitionDetail.YarnRequisitionID, oYarnRequisitionDetail.TechnicalSheetID, oYarnRequisitionDetail.ColorID, oYarnRequisitionDetail.PentonNo, oYarnRequisitionDetail.GARQty, oYarnRequisitionDetail.FabricID, oYarnRequisitionDetail.GSM, oYarnRequisitionDetail.YarnID, oYarnRequisitionDetail.YarnCount, oYarnRequisitionDetail.YarnPercent, oYarnRequisitionDetail.ActualConsumption, oYarnRequisitionDetail.CostingConsumption, oYarnRequisitionDetail.RequisitionQty, oYarnRequisitionDetail.MUnitID, oYarnRequisitionDetail.Remarks, nUserID, (int)eEnumDBOperation, sYarnRequisitionDetailIDs);
        }

        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_YarnRequisitionDetail WHERE YarnRequisitionDetailID=%n", nID);
        }
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_YarnRequisitionDetail ");
        }
        public static IDataReader Gets(TransactionContext tc, int nYarnRequisitionID)
        {
            return tc.ExecuteReader("SELECT * FROM View_YarnRequisitionDetail WHERE YarnRequisitionID = %n ORDER BY TechnicalSheetID,	ColorID, FabricID ASC", nYarnRequisitionID);
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }

        #endregion
    }
}