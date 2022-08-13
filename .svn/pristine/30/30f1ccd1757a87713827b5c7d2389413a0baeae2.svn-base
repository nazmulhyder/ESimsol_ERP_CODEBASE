using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class MaterialTransactionRuleDA
    {
        public MaterialTransactionRuleDA() { }

        #region Insert Function
        public static IDataReader Insert(TransactionContext tc, MaterialTransactionRule oMaterialTransactionRule, EnumDBOperation eEnumDBOperation, Int64 nUserId)
        {
            return tc.ExecuteReader("EXEC [sp_MTR_InsertUpdateDelete]"
                                    + "%n,%n,%n,%n,%n,%n,%b,%n,%n,%b,%s,%n",
                                    (int)eEnumDBOperation,oMaterialTransactionRule.MaterialTransactionRuleID, oMaterialTransactionRule.LocationID, oMaterialTransactionRule.WorkingUnitID, (int)oMaterialTransactionRule.TriggerParentType, (int)oMaterialTransactionRule.InOutType, oMaterialTransactionRule.Direction, (int)oMaterialTransactionRule.ProductType, oMaterialTransactionRule.ProductCategoryID, oMaterialTransactionRule.IsActive, oMaterialTransactionRule.Note, nUserId);
        }


        #endregion
        #region Update Function
        public static IDataReader Update(TransactionContext tc, MaterialTransactionRule oMaterialTransactionRule, EnumDBOperation eEnumDBOperation, Int64 nUserId)
        {
            return tc.ExecuteReader("EXEC [sp_MTR_InsertUpdateDelete]"
                                     + "%n,%n,%n,%n,%n,%n,%b,%n,%n,%b,%s,%n",
                                     (int)eEnumDBOperation, oMaterialTransactionRule.MaterialTransactionRuleID, oMaterialTransactionRule.LocationID, oMaterialTransactionRule.WorkingUnitID, (int)oMaterialTransactionRule.TriggerParentType, (int)oMaterialTransactionRule.InOutType, oMaterialTransactionRule.Direction, (int)oMaterialTransactionRule.ProductType, oMaterialTransactionRule.ProductCategoryID, oMaterialTransactionRule.IsActive, oMaterialTransactionRule.Note, nUserId);
        }


        #endregion
        #region Delete Function
        public static void Delete(TransactionContext tc, MaterialTransactionRule oMaterialTransactionRule, EnumDBOperation eEnumDBOperation, Int64 nUserId)
        {
            tc.ExecuteNonQuery("EXEC [sp_MTR_InsertUpdateDelete]"
                                     + "%n,%n,%n,%n,%n,%n,%b,%n,%n,%b,%s,%n",
                                     (int)eEnumDBOperation, oMaterialTransactionRule.MaterialTransactionRuleID, oMaterialTransactionRule.LocationID, oMaterialTransactionRule.WorkingUnitID, (int)oMaterialTransactionRule.TriggerParentType, (int)oMaterialTransactionRule.InOutType, oMaterialTransactionRule.Direction, (int)oMaterialTransactionRule.ProductType, oMaterialTransactionRule.ProductCategoryID, oMaterialTransactionRule.IsActive, oMaterialTransactionRule.Note, nUserId);
        }
        #endregion
        #region Insert MAP
        public static void InsertMAP(TransactionContext tc, MaterialTransactionRule oMaterialTransactionRule, EnumDBOperation eEnumDBOperation, Int64 nUserId)
        {
            tc.ExecuteNonQuery("EXEC [sp_MTR_DirectionMapping]"
                                    + "%n,%s",
                                    oMaterialTransactionRule.MaterialTransactionRuleID, oMaterialTransactionRule.IDs);
        }


        #endregion
        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_DocumentLeaf WHERE DLID=%n", nID);
        }
       
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_DocumentLeaf WHERE DocumentStatus > 2 Order by UpdatedDBServerDate DESC");
        }
        public static IDataReader GetsForBiDirectionalRule(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }

        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }

        public static IDataReader Get(TransactionContext tc, int eTPT, int eInOutType, bool bDirection, int nProductCaegoryId, int nWorkingUnitID, Int64 nUserID,int nOEvalue, string sDbObjectName)
        {
            return tc.ExecuteReader("select * from MaterialTransactionRule where  TriggerParentType=%n AND InOutType=%n AND Direction=%b AND ProductCategoryID=%n AND workingunitid=%n"
                                    +"AND MaterialTransactionRuleID IN(select MappingRuleID from View_AuthorizationUserOEDOMTR where UserID=%n AND AUOEDOID IN (SELECT AUOEDOID FROM View_AuthorizationUserOEDO WHERE OEValue=%n AND DBObjectName=%s))", (int)eTPT, (int)eInOutType, bDirection, nProductCaegoryId, nWorkingUnitID, nUserID,nOEvalue,sDbObjectName);
        }
        #endregion
    }
}