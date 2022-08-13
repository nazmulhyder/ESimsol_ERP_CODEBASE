using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class LotDA
    {
        public LotDA() { }

        #region Insert Update Delete
        public static IDataReader InsertUpdateAdjLot(TransactionContext tc, Lot oLot, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_LotADj]"
                                    + "%n, %s, %s, %n, %n, %n,%n, %n,%n, %s, %n, %n, %n ",
                                    oLot.LotID, oLot.LotNo, oLot.LogNo, oLot.MUnitID, oLot.ProductID, oLot.WorkingUnitID, oLot.Balance, oLot.UnitPrice, oLot.CurrencyID, oLot.ProductName, oLot.BUID, nUserID, (int)eEnumDBOperation);
        }
        public static IDataReader InsertUpdate(TransactionContext tc, Lot oLot, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_Lot]"
                                    + "%n, %s, %s, %n, %n, %n, %n, %n, %n, %n, %n, %n, %n, %n, %n, %n, %n, %n, %n, %n, %n, %n",
                                    oLot.LotID, oLot.LotNo, oLot.LogNo, oLot.MUnitID, oLot.ProductID, oLot.WorkingUnitID, oLot.Balance, oLot.UnitPrice, oLot.CurrencyID, oLot.BUID, oLot.ModelReferenceID, oLot.StyleID, oLot.ColorID, oLot.SizeID, oLot.ContractorID, oLot.WeightPerCartoon, oLot.ConePerCartoon, oLot.FCUnitPrice, oLot.FCCurrencyID, oLot.RackID, nUserID, (int)eEnumDBOperation);
        }
        
        //UpdateLotPrice
        public static IDataReader UpdateLotPrice(TransactionContext tc, Lot oLot,  Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_UpdateLotPrice]"
                                    + "%n, %n",oLot.LotID, oLot.UnitPrice);
        }
        public static IDataReader UpdateRack(TransactionContext tc, Lot oLot, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("UPDATE Lot SET RackID = " + oLot.RackID + " WHERE LotID = " + oLot.LotID);
        }
        #endregion

        #region Insert Update Delete Function



        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, int eParentType, int nParentID, int nWorkingUnitID, int nProductID, Int64 nUserID)
        {
            return tc.ExecuteReader("select * from View_Lot where ParentID=%n and ParentType=%n  and Lot.WorkingUnitID=%n", nParentID, (int)eParentType, nWorkingUnitID);
        }
        public static IDataReader Get(TransactionContext tc, int nLotID, Int64 nUserID)
        {

            return tc.ExecuteReader("select * from View_Lot where LotID=%n ", nLotID);

        }
        public static IDataReader GetByProductID(TransactionContext tc, int nProductID, bool bIsZeroBalance, Int64 nUserID)
        {
            if (bIsZeroBalance)
            {
                return tc.ExecuteReader("select * from View_Lot where ProductID=%n ", nProductID);
            }
            else
            {
                return tc.ExecuteReader("select * from View_Lot where Balance>0.3 and ProductID=%n ", nProductID);
            }
        }
        public static IDataReader GetsByInvoiceDetail(TransactionContext tc, int eParentType, int nParentID, int nProductID, Int64 nUserID)
        {

            return tc.ExecuteReader("select * from View_Lot where ParentID in (%q) and Lot.ParentType=%n ", nParentID, eParentType);

        }
        public static IDataReader Gets(TransactionContext tc, string sProductID, string sWorkingUnitID)
        {

            return tc.ExecuteReader("select * from View_Lot  where ProductID in (%q) and WorkingUnitID in (%q) and Balance>0.3", sProductID, sWorkingUnitID);
        }
        public static IDataReader GetsZeroBalance(TransactionContext tc, string sProductID, string sWorkingUnitID)
        {

            return tc.ExecuteReader("select * from View_Lot AS HH WHERE HH.ProductID in (%q) and HH.WorkingUnitID in (%q)  ", sProductID, sWorkingUnitID);
        }
        public static IDataReader GetsDataSet(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        public static void CommitIsRunning(TransactionContext tc, Lot oLot, Int64 nUserId)
        {
            tc.ExecuteNonQuery("Update Lot SET LotStatus=0  WHERE LotStatus=1 and ProductID=%n and WorkingUnitID=%n", oLot.ProductID, oLot.WorkingUnitID);
            tc.ExecuteNonQuery("Update Lot SET LotStatus=1  WHERE LotID =%n", oLot.LotID);
        }

        public static void UpdateStatus(TransactionContext tc, Lot oLot, Int64 nUserId)
        {
            tc.ExecuteNonQuery("Update Lot SET LotStatus=%n WHERE LotID IN (SELECT * FROM dbo.SplitInToDataSet(%s,','))", oLot.LotStatus, oLot.ErrorMessage);
        }
    
        #endregion
    }
}
