using System;
using System.Data;
using System.Data.SqlClient;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.DataAccess;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class ProductCategoryDA
    {
        public ProductCategoryDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, ProductCategory oPC, int nEnumDBOperation, Int64 nUserId)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_ProductCategory]"
                                    + "%n, %s, %n, %s, %b, %n, %n",
                                    oPC.ProductCategoryID, oPC.ProductCategoryName, oPC.ParentCategoryID, oPC.Note, oPC.IsLastLayer,  nUserId, nEnumDBOperation);
        }

        public static void Delete(TransactionContext tc, ProductCategory oPC, int nEnumDBOperation, Int64 nUserId)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_ProductCategory]"
                                    + "%n, %s, %n, %s, %b, %n, %n",
                                       oPC.ProductCategoryID, oPC.ProductCategoryName, oPC.ParentCategoryID, oPC.Note, oPC.IsLastLayer, nUserId, nEnumDBOperation);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM VIEW_ProductCategory WHERE ProductCategoryID=%n", nID);
        }
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM VIEW_ProductCategory  ");
        }
        public static IDataReader BUWiseGets(int nBUID, TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM VIEW_ProductCategory AS TT WHERE TT.ProductCategoryID IN  (SELECT HH.ProductCategoryID FROM  BUWiseProductCategory AS HH WHERE HH.BUID = %n) OR TT.ProductCategoryID = 1 Order BY TT.ProductCategoryID", nBUID);
        }

        public static IDataReader GetsBUWiseLastLayer(TransactionContext tc, int nBUID)
        {
            return tc.ExecuteReader("SELECT * FROM VIEW_ProductCategory AS TT WHERE TT.IsLastLayer=1 AND TT.ProductCategoryID IN  (SELECT HH.ProductCategoryID FROM  BUWiseProductCategory AS HH WHERE HH.BUID = %n) Order BY TT.ParentCategoryName, TT.ProductCategoryName ASC", nBUID);
        }

        public static IDataReader GetsLastLayer(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM VIEW_ProductCategory where IsLastLayer=1");
        }
        public static IDataReader GetsByParentID(TransactionContext tc, int nParentID)
        {
            return tc.ExecuteReader("SELECT * FROM GetProductCategoryByRoot(%n)", nParentID);
        }
        public static void Update_AccountHead(TransactionContext tc, ProductCategory oProductCategory)
        {
            tc.ExecuteNonQuery("UPDATE ProductCategory SET DrAccountHeadID=%n , CrAccountHeadID =%n " + " WHERE ProductCategoryID=%n", oProductCategory.DrAccountHeadID, oProductCategory.CrAccountHeadID, oProductCategory.ProductCategoryID);
        }  
   
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        public static IDataReader GetsByUserAutMTR(TransactionContext tc, string sDBObjectName, int nOperationalEvent, int eTriggerParentsType, int eInOutType, bool bDirection, int nStoreID, int nMapStoreID, Int64 nUserID)
        {
            return tc.ExecuteReader("select * from VIEW_ProductCategory as P where P.ProductCategoryID in (  select ProductCategoryID from [dbo].[fn_GetProductCategoryByMTR] (%s,%n,%n,%n,%b,%n,%n,%n)) order by ProductCategoryID", sDBObjectName, nOperationalEvent, eTriggerParentsType, eInOutType, bDirection, nStoreID, nMapStoreID, nUserID);
        }

        public static IDataReader GetsForTree(TransactionContext tc, string sTableName, string sPK, string sPCID, bool IsParent, bool IsChild, string IDs, bool IsAll)
        {
            return tc.ExecuteReader("EXEC [SP_GetHierarchyList]" + " %s, %s, %s, %b , %b, %s, %b", sTableName, sPK, sPCID, IsParent, IsChild, IDs, IsAll);
        }
        #endregion
    }
}
