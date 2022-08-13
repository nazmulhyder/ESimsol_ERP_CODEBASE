using System;
using System.Data;
using System.Data.SqlClient;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.DataAccess;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class WorkingUnitDA
    {
        public WorkingUnitDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, WorkingUnit oWorkingUnit, EnumDBOperation eEnumDBOperation, Int64 nUserId)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_WorkingUnit]"
                                    + "%n, %s, %n, %n, %b, %n,%n,%n, %n ",
                                    oWorkingUnit.WorkingUnitID, oWorkingUnit.WorkingUnitCode, oWorkingUnit.LocationID, oWorkingUnit.OperationUnitID, oWorkingUnit.IsActive, oWorkingUnit.BUID, oWorkingUnit.UnitTypeInt, nUserId, (int)eEnumDBOperation);
        }

        public static void Delete(TransactionContext tc, WorkingUnit oWorkingUnit, EnumDBOperation eEnumDBOperation, Int64 nUserId)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_WorkingUnit]"
                                    + "%n, %s, %n, %n, %b, %n,%n,%n, %n ",
                                    oWorkingUnit.WorkingUnitID, oWorkingUnit.WorkingUnitCode, oWorkingUnit.LocationID, oWorkingUnit.OperationUnitID, oWorkingUnit.IsActive, oWorkingUnit.BUID,oWorkingUnit.UnitTypeInt, nUserId, (int)eEnumDBOperation);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader GetsbyName(TransactionContext tc, string sWorkingUnit)
        {
            if (sWorkingUnit == "")
            {
                return tc.ExecuteReader("SELECT * FROM View_WorkingUnit WHERE IsActive = 1 AND IsStore = 1 ");
            }
            else
            {
                return tc.ExecuteReader("SELECT * FROM View_WorkingUnit WHERE LOUNameCode Like ('%" + sWorkingUnit + "%') AND  IsActive = 1 AND IsStore = 1 ");
            }
        }
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_WorkingUnit WHERE WorkingUnitID=%n", nID);
        }
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_WorkingUnit");
        }
        public static IDataReader Gets(TransactionContext tc, int nLocationID)
        {
            return tc.ExecuteReader("SELECT * FROM View_WorkingUnit WHERE IsActive = 1 AND IsStore = 1 AND LocationID =%n", nLocationID);
        }        
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        public static IDataReader Gets(TransactionContext tc, string sDBObject, int nTRType, int nOEValue, int nInOutType, bool bDirection, int nPTMId, int nPid, int nWUId, Int64 nUserID, int nCompanyID)
        {
            return tc.ExecuteReader("SELECT * FROM View_WorkingUnit WHERE CompanyID=%n AND WorkingUnitID IN (SELECT WorkingUnitID FROM FN_GetStore(%s,%n,%n,%n,%b,%n,%n,%n,%n))",nCompanyID, sDBObject, nTRType, nOEValue, nInOutType, bDirection, nPTMId, nPid, nWUId, nUserID);
        }              
        public static IDataReader UpdateForAcitivity(TransactionContext tc, int nWorkingUnitID)
        {
            string sSQL1 = SQLParser.MakeSQL("Update WorkingUnit Set IsActive=~IsActive Where WorkingUnitID=%n", nWorkingUnitID);
            tc.ExecuteNonQuery(sSQL1);
            return tc.ExecuteReader("SELECT * FROM View_WorkingUnit WHERE WorkingUnitID=" + nWorkingUnitID);
        }
        public static IDataReader BUWiseGets(TransactionContext tc, int nBUID)
        {
            return tc.ExecuteReader("SELECT * FROM View_WorkingUnit WHERE IsActive = 1 AND IsStore = 1 AND BUID =%n", nBUID);
        }

        public static IDataReader GetsPermittedStore(TransactionContext tc, int nBUID, EnumModuleName eModuleName, EnumStoreType eStoreType, int nUserID)
        {
            if(nBUID!=0)
            {
                return tc.ExecuteReader("SELECT * FROM View_WorkingUnit AS WU WHERE WU.IsActive = 1 AND WU.IsStore = 1 AND WU.BUID= %n AND WU.WorkingUnitID IN (SELECT SP.WorkingUnitID FROM StorePermission AS SP WHERE SP.ModuleName = %n AND SP.StoreType = %n AND SP.UserID = %n)", nBUID, (int)eModuleName, (int)eStoreType, nUserID);
            }
            else
            {
                return tc.ExecuteReader("SELECT * FROM View_WorkingUnit AS WU WHERE WU.IsActive = 1 AND WU.IsStore = 1  AND WU.WorkingUnitID IN (SELECT SP.WorkingUnitID FROM StorePermission AS SP WHERE SP.ModuleName = %n AND SP.StoreType = %n AND SP.UserID = %n)", (int)eModuleName, (int)eStoreType, nUserID);
            }
            
        }
        public static IDataReader GetsPermittedStoreByStoreName(TransactionContext tc, int nBUID, EnumModuleName eModuleName, EnumStoreType eStoreType, string sStoerName, int nUserID)
        {
            return tc.ExecuteReader("SELECT * FROM View_WorkingUnit AS WU WHERE WU.StoreName LIKE '%" + sStoerName + "%' AND WU.IsActive = 1 AND WU.IsStore = 1 AND WU.BUID= " + nBUID.ToString() + " AND WU.WorkingUnitID IN (SELECT SP.WorkingUnitID FROM StorePermission AS SP WHERE SP.ModuleName = " + ((int)eModuleName).ToString() + " AND SP.StoreType = " + ((int)eStoreType).ToString() + " AND SP.UserID = " + nUserID.ToString() + ")");
        }
        public static IDataReader WorkingUnit_AutoConfiguration(TransactionContext tc, int nLocation_Assign, int nLocation_Source, int nUserID_Assign, int nUserID_Source, int nConType, Int64 nUserID)
        {
            return tc.ExecuteReader(CommandType.StoredProcedure, "[SP_IUD_WorkingUnit_AutoConfiguration]", nLocation_Assign, nLocation_Source, nUserID_Assign, nUserID_Source, nConType, nUserID);
        }
        public static IDataReader Gets(TransactionContext tc, string sDBObject, int nTRType, int nOEValue, int nInOutType, bool bDirection, int nPid, int nWUId, Int64 nUserID)
        {
            return tc.ExecuteReader("SELECT * FROM View_WorkingUnit WHERE WorkingUnitID IN (SELECT WorkingUnitID FROM FN_GetStore(%s,%n,%n,%n,%b,%n,%n,%n))", sDBObject, nTRType, nOEValue, nInOutType, bDirection, nPid, nWUId, nUserID);
        }
        #endregion
    }
}
