using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Framework;
using ICS.Core.Utility;
using ICS.Core.DataAccess;

namespace ESimSol.Services.DataAccess
{
    public class DevelopmentRecapDA
    {
        public DevelopmentRecapDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, DevelopmentRecap oDevelopmentRecap, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_DevelopmentRecap]"
                                    + "%n,%n, %n, %s, %n, %n, %s, %n, %n, %s, %s, %s, %s,%n, %n, %n,%n,%n,%s, %s, %n, %n, %n, %n, %n",
                                    oDevelopmentRecap.DevelopmentRecapID, oDevelopmentRecap.BUID, oDevelopmentRecap.BusinessSessionID, oDevelopmentRecap.DevelopmentRecapNo, oDevelopmentRecap.TechnicalSheetID, (int)oDevelopmentRecap.DevelopmentStatus, oDevelopmentRecap.GG, oDevelopmentRecap.SampleQty, oDevelopmentRecap.SampleSizeID, oDevelopmentRecap.AwbNo, oDevelopmentRecap.Remarks, oDevelopmentRecap.CollectionName, oDevelopmentRecap.SpecialFinish, oDevelopmentRecap.MerchandiserID, (int)oDevelopmentRecap.TransportType, oDevelopmentRecap.YarnCategoryID, oDevelopmentRecap.BuyerContactPersonID, oDevelopmentRecap.UnitID, oDevelopmentRecap.Weight, oDevelopmentRecap.Count, (int)oDevelopmentRecap.DevelopmentType, oDevelopmentRecap.UnitPrice, oDevelopmentRecap.CurrencyID, nUserID, (int)eEnumDBOperation);
        }

        public static void Delete(TransactionContext tc, DevelopmentRecap oDevelopmentRecap, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_DevelopmentRecap]"
                                    + "%n,%n, %n, %s, %n, %n, %s, %n, %n, %s, %s, %s, %s,%n, %n, %n,%n,%n,%s, %s, %n, %n, %n, %n, %n",
                                    oDevelopmentRecap.DevelopmentRecapID, oDevelopmentRecap.BUID, oDevelopmentRecap.BusinessSessionID, oDevelopmentRecap.DevelopmentRecapNo, oDevelopmentRecap.TechnicalSheetID, (int)oDevelopmentRecap.DevelopmentStatus, oDevelopmentRecap.GG, oDevelopmentRecap.SampleQty, oDevelopmentRecap.SampleSizeID, oDevelopmentRecap.AwbNo, oDevelopmentRecap.Remarks, oDevelopmentRecap.CollectionName, oDevelopmentRecap.SpecialFinish, oDevelopmentRecap.MerchandiserID, (int)oDevelopmentRecap.TransportType, oDevelopmentRecap.YarnCategoryID, oDevelopmentRecap.BuyerContactPersonID, oDevelopmentRecap.UnitID, oDevelopmentRecap.Weight, oDevelopmentRecap.Count, (int)oDevelopmentRecap.DevelopmentType, oDevelopmentRecap.UnitPrice, oDevelopmentRecap.CurrencyID, nUserID, (int)eEnumDBOperation);
        }

        public static void ActiveInActive(TransactionContext tc, int id, bool bIsActiveInActive, Int64 nUserID)
        {
            tc.ExecuteNonQuery("UPDATE DevelopmentRecap SET IsActive=%b WHERE DevelopmentRecapID=%n", bIsActiveInActive, id);
        }

        public static void UpdateInqRcvDate(TransactionContext tc, int id, DateTime dInqRcvDate, Int64 nUserID)
        {
            tc.ExecuteNonQuery("UPDATE DevelopmentRecap SET InquiryReceivedDate=%d WHERE DevelopmentRecapID=%n", dInqRcvDate, id);
        }
        public static void UpdateSmplRcvDate(TransactionContext tc, int id, DateTime dSmplRcvDate, Int64 nUserID)
        {
            tc.ExecuteNonQuery("UPDATE DevelopmentRecap SET SampleReceivedDate=%d WHERE DevelopmentRecapID=%n", dSmplRcvDate, id);
        }
        public static void UpdateSmplSendingDate(TransactionContext tc, int id, DateTime dSmplSendingDate, Int64 nUserID)
        {
            tc.ExecuteNonQuery("UPDATE DevelopmentRecap SET SampleSendingDate=%d WHERE DevelopmentRecapID=%n", dSmplSendingDate, id);
        }
        public static void UpdateSendingDeadLine(TransactionContext tc, int id, DateTime dSendingDeadLine, Int64 nUserID)
        {
            tc.ExecuteNonQuery("UPDATE DevelopmentRecap SET SendingDeadLine=%d WHERE DevelopmentRecapID=%n", dSendingDeadLine, id);
        }

        public static IDataReader ChangeStatus(TransactionContext tc, DevelopmentRecap oDevelopmentRecap, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_DevelopmetOperation]"
                                    + "%n,%n,%n,%n,%s,%n, %n,%d,%n,%n",
                                    oDevelopmentRecap.DevelopmentRecapHistoryID, oDevelopmentRecap.DevelopmentRecapID, (int)oDevelopmentRecap.CurrentStatus, (int)oDevelopmentRecap.PreviousStatus, oDevelopmentRecap.Note, (int)oDevelopmentRecap.ActionType,oDevelopmentRecap.OperationBy, oDevelopmentRecap.OperationDate, nUserID, (int)eEnumDBOperation);
        }
        #endregion
        public static IDataReader Gets_Report(TransactionContext tc, int id)
        {
            return tc.ExecuteReader("SELECT * FROM View_DevelopmentRecapSingle WHERE DevelopmentRecapID=%n", id);
        }
        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_DevelopmentRecap WHERE DevelopmentRecapID=%n", nID);
        }


        public static IDataReader GetsByTechnicalSheet(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT  * FROM View_DevelopmentRecap WHERE TechnicalSheetID=%n", nID);
        }

        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_DevelopmentRecap");
        }
        public static IDataReader GetsRecapWithRowNumber(TransactionContext tc, int nStartRow, int nEndRow)
        {
            return tc.ExecuteReader("EXEC [SP_GetDevelopmentRecap]" + "%n, %n", nStartRow, nEndRow);
        }

        public static IDataReader GetsRecapWithDevelopmentRecaps(TransactionContext tc, int nStartRow, int nEndRow, string sDevelopmentRecapIDs, bool bIsPrint)
        {
            return tc.ExecuteReader("EXEC [SP_GetDevelopmentRecapWithIDs]" + "%n, %n, %s, %b", nStartRow, nEndRow, sDevelopmentRecapIDs, bIsPrint);
        }

        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }

        #endregion
    }
}
