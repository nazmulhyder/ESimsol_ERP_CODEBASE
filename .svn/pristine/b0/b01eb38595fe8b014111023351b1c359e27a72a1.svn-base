using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;


namespace ESimSol.Services.DataAccess
{
    public class InventoryTransactionDA
    {
        public InventoryTransactionDA() { }

        #region Insert Function
   

       #endregion

        #region Update Function
      

     
        #endregion

        
   

        #region Get & Exist Function
        public static IDataReader GetsByLotID(TransactionContext tc, int nLotID)
        {
            return tc.ExecuteReader("Select * from View_ITransaction where LotID=%n and TriggerParentType>99 order by [DateTime]", nLotID);
        }
        public static IDataReader Gets(TransactionContext tc, int nTriggerParentID, EnumTriggerParentsType eTriggerParentType)
        {
            return tc.ExecuteReader("SELECT * FROM View_ITransaction WHERE TriggerParentID=%n AND TriggerParentType=%n", nTriggerParentID, (int)eTriggerParentType);
        }
        public static IDataReader Gets(TransactionContext tc, int nEnumTriggerParentsType, int eInOutType, DateTime dTranDate, DateTime dTranDateTo, string sWorkingUnitIDs)
        {
            return tc.ExecuteReader("SELECT * FROM View_ITransaction  Where TriggerParentType=%n and InOutType=%n and [DateTime]>=%D and  [DateTime]<%D and WorkingUnitID in (%q) order by [DateTime] ", nEnumTriggerParentsType, eInOutType, dTranDate, dTranDateTo, sWorkingUnitIDs);
        }
        public static IDataReader Gets(TransactionContext tc, int nTriggerParentID, int eEnumTriggerParentsType, int eInOutType, int nProductID, string sStoreIDs)
        {
            return tc.ExecuteReader("SELECT * FROM View_ITransaction  Where TriggerParentID=%n and TriggerParentType=%n and InOutType =%n  and WorkingUnitID in(%q) order by IT.[DateTime]  ", nTriggerParentID, eEnumTriggerParentsType, eInOutType, sStoreIDs);

        }
     
        public static IDataReader Gets(TransactionContext tc, int nTriggerParentID, int eTriggerParentType, int eInOutType, int nProductCatagoryID)
        {

            return tc.ExecuteReader("SELECT * FROM View_ITransaction Where InOutType =%n and  TriggerParentID=%n and TriggerParentType=%n order by [DateTime] ", eInOutType, nTriggerParentID, eTriggerParentType);
     
        }
        public static IDataReader Gets(TransactionContext tc, int nTriggerParentID, EnumTriggerParentsType eTriggerParentType, EnumInOutType eInOutType, string sWorkingUnitID)
        {
            return tc.ExecuteReader("SELECT * FROM View_ITransaction Where TriggerParentID=%n AND TriggerParentType=%n AND InOutType=%n AND WorkingUnitID in (%q)", nTriggerParentID, (int)eTriggerParentType, (int)eInOutType, sWorkingUnitID);
        }
        public static IDataReader GetsByLotID(TransactionContext tc, int nProductID, string sLotIDs)
        {

            return tc.ExecuteReader("SELECT * FROM View_ITransaction Where LotID in (%q) order by IT.[DateTime] ", sLotIDs);

        }
        public static IDataReader GetsByInvoice(TransactionContext tc, int nTriggerParentID, int eTriggerParentType, int nInoutType,int nProductID)
        {
            return tc.ExecuteReader("SELECT * FROM View_ITransaction as IT where TriggerParentID=%n and TriggerParentType=%n and  IT.InOutType =%n and ProductID=%n  order by IT.[DateTime] ", nTriggerParentID, eTriggerParentType, nInoutType, nProductID);
        }
     
      
        public static double GetQty(TransactionContext tc, int nLotID, EnumInOutType eInOutType, EnumTriggerParentsType eTType)
        {
            object obj = tc.ExecuteScalar("SELECT Isnull(sum([Weight]),0) FROM ITransaction where LotID=%n and InOutType=%n and TriggerParentType= %n", nLotID, (int)eInOutType, (int)eTType);
            if (obj == null) return -1;
            return Convert.ToDouble(obj);
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }

        public static IDataReader GetsStockByLotID(TransactionContext tc, int nLotID)
        {
            return tc.ExecuteReader("EXEC [SP_LotStockReportDetail] %n",nLotID);
        }
        #endregion
    }
}