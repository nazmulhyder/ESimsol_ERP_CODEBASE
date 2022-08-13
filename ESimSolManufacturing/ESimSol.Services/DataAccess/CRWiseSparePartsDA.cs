using System;
using System.Data;
using System.Data.SqlClient;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.DataAccess;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class CRWiseSparePartsDA
    {
        public CRWiseSparePartsDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, CRWiseSpareParts oCRWiseSpareParts, EnumDBOperation eEnumDBOperation, Int64 nUserId)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_CRWiseSpareParts]"
                                    + "%n, %n, %n, %n, %n, %s, %n, %n, %n",
                                    oCRWiseSpareParts.CRWiseSparePartsID, oCRWiseSpareParts.CRID, oCRWiseSpareParts.SparePartsID, oCRWiseSpareParts.BUID,
                                    oCRWiseSpareParts.ReqPartsQty, oCRWiseSpareParts.Remarks, false, nUserId, (int)eEnumDBOperation);
        }
        public static void Delete(TransactionContext tc, CRWiseSpareParts oCRWiseSpareParts, EnumDBOperation eEnumDBOperation, Int64 nUserId)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_CRWiseSpareParts]"
                                    + "%n, %n, %n, %n, %n, %s, %n, %n, %n",
                                    oCRWiseSpareParts.CRWiseSparePartsID, oCRWiseSpareParts.CRID, oCRWiseSpareParts.SparePartsID, oCRWiseSpareParts.BUID,
                                    oCRWiseSpareParts.ReqPartsQty, oCRWiseSpareParts.Remarks, false, nUserId, (int)eEnumDBOperation);
        }
        public static IDataReader SaveFromCopy(TransactionContext tc, CRWiseSpareParts oCRWiseSpareParts, EnumDBOperation eEnumDBOperation, Int64 nUserId)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_CRWiseSpareParts]"
                                    + "%n, %n, %n, %n, %n, %s, %n, %n, %n",
                                    oCRWiseSpareParts.CRWiseSparePartsID, oCRWiseSpareParts.CRID, oCRWiseSpareParts.SparePartsID, oCRWiseSpareParts.BUID,
                                    oCRWiseSpareParts.ReqPartsQty, oCRWiseSpareParts.Remarks, true, nUserId, (int)eEnumDBOperation);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader GetsbyName(TransactionContext tc, string sCRWiseSpareParts)
        {
            if (sCRWiseSpareParts == "")
            {
                return tc.ExecuteReader("SELECT * FROM View_CRWiseSpareParts WHERE IsActive = 1 AND IsStore = 1 ");
            }
            else
            {
                return tc.ExecuteReader("SELECT * FROM View_CRWiseSpareParts WHERE LOUNameCode Like ('%" + sCRWiseSpareParts + "%') AND  IsActive = 1 AND IsStore = 1 ");
            }
        }
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_CRWiseSpareParts WHERE CRWiseSparePartsID=%n", nID);
        }
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_CRWiseSpareParts");
        }
        public static IDataReader Gets(TransactionContext tc, int nCRID, int nBUID)
        {
            return tc.ExecuteReader("SELECT * FROM View_CRWiseSpareParts WHERE CRID = %n AND BUID=%n", nCRID, nBUID);
        }
        public static IDataReader GetsByNameCRAndBUID(TransactionContext tc, string sName, int nCRID, int nBUID)
        {
            return tc.ExecuteReader("SELECT * FROM View_CRWiseSpareParts WHERE ProductName LIKE '%" + sName + "%' AND CRID = " + nCRID + " AND BUID=" + nBUID);
        }
        public static IDataReader GetsByNameCRAndBUIDWithLot(TransactionContext tc, string sName, int nCRID, int nBUID)
        {
            return tc.ExecuteReader("SELECT CRWSP.*, (SELECT SUM(ISNULL((Balance),0)) FROM Lot WHERE ProductID = CRWSP.SparePartsID AND BUID = " + nBUID + ") as TotalLotBalance FROM View_CRWiseSpareParts AS CRWSP WHERE ProductName LIKE '%" + sName + "%' AND CRID = " + nCRID + " AND BUID=" + nBUID);
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        public static IDataReader BUWiseGets(TransactionContext tc, int nBUID)
        {
            return tc.ExecuteReader("SELECT * FROM View_CRWiseSpareParts WHERE IsActive = 1 AND IsStore = 1 AND BUID =%n", nBUID);
        }

        #endregion
    }
}
