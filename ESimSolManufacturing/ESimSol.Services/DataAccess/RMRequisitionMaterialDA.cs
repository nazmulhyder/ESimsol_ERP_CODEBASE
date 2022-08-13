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
    public class RMRequisitionMaterialDA
    {
        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, RMRequisitionMaterial oRMRequisitionMaterial, EnumDBOperation eEnumDBOperation, Int64 nUserID, string sDetailIDs)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_RMRequisitionMaterial]"
                                   + "%n, %n, %n, %n, %n, %n, %n, %n, %s",
                                   oRMRequisitionMaterial.RMRequisitionMaterialID, oRMRequisitionMaterial.RMRequisitionID, oRMRequisitionMaterial.ProductionSheetID, oRMRequisitionMaterial.ProductionRecipeID, oRMRequisitionMaterial.ProductID, oRMRequisitionMaterial.Qty, (int)eEnumDBOperation, nUserID, sDetailIDs);
        }

        public static void Delete(TransactionContext tc, RMRequisitionMaterial oRMRequisitionMaterial, EnumDBOperation eEnumDBOperation, Int64 nUserID, string sDetailIDs)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_RMRequisitionMaterial]"
                                   + "%n, %n, %n, %n, %n, %n, %n, %n, %s",
                                   oRMRequisitionMaterial.RMRequisitionMaterialID, oRMRequisitionMaterial.RMRequisitionID, oRMRequisitionMaterial.ProductionSheetID, oRMRequisitionMaterial.ProductionRecipeID, oRMRequisitionMaterial.ProductID, oRMRequisitionMaterial.Qty, (int)eEnumDBOperation, nUserID, sDetailIDs);
        }
        public static IDataReader CommitRawMaterialOut(TransactionContext tc, RMRequisitionMaterial oRMRequisitionMaterial, EnumTriggerParentsType eTriggerParentType, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_CommitProductionRecipe]"
                                   + "%n, %n, %n, %n, %n, %n, %n, %n", oRMRequisitionMaterial.RMRequisitionMaterialID, oRMRequisitionMaterial.ProductionRecipeID, oRMRequisitionMaterial.ProductionSheetID, oRMRequisitionMaterial.RMRequisitionID, oRMRequisitionMaterial.CurrentOutQty, oRMRequisitionMaterial.WUID, (int)eTriggerParentType, nUserID);
        }
        
        public static void ReceiveReturnQty(TransactionContext tc, ITransaction oITransaction, int nParetType, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_AcceptReturnQty]"
                                   + "%n,%n,%n,%n,%n",
                                   oITransaction.ITransactionID, oITransaction.TriggerParentID, oITransaction.ReturnQty, nParetType, nUserID);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_RMRequisitionMaterial WHERE RMRequisitionMaterialID=%n", nID);
        }
        public static IDataReader Gets(int nRMRequisitionID, TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_RMRequisitionMaterial WHERE RMRequisitionID = %n ORDER BY ProductionSheetID, ProductionRecipeID, ProductID", nRMRequisitionID);
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }

        #endregion
    }
  
}
