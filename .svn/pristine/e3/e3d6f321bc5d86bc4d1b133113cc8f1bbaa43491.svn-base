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
    public class ExportUDDA
    {
        #region Insert Update Delete Function

        public static IDataReader InsertUpdate(TransactionContext tc, ExportUD oExportUD, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_ExportUD]"
                                   + "%n,%n,%n,%n,%s,%D,%n,%s,%s,%s,%s,%d,%n,%n",
                                   oExportUD.ExportUDID, oExportUD.ExportLCID, oExportUD.ANo, oExportUD.Amount, oExportUD.UDNo, oExportUD.UDReceiveDate, oExportUD.ReceiveBYID, oExportUD.ReceiveFrom, oExportUD.ContractNo, oExportUD.Note, oExportUD.AUDNo, oExportUD.ADate, nUserID, (int)eEnumDBOperation);
        }

        public static void Delete(TransactionContext tc, ExportUD oExportUD, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_ExportUD]"
                                    + "%n,%n,%n,%n,%s,%D,%n,%s,%s,%s,%s,%d,%n,%n",
                                    oExportUD.ExportUDID, oExportUD.ExportLCID, oExportUD.ANo, oExportUD.Amount, oExportUD.UDNo, oExportUD.UDReceiveDate, oExportUD.ReceiveBYID, oExportUD.ReceiveFrom, oExportUD.ContractNo, oExportUD.Note, oExportUD.AUDNo, oExportUD.ADate, nUserID, (int)eEnumDBOperation);
        }

        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_ExportUD WHERE ExportUDID=%n", nID);
        }
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM ExportUD");
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }

        #endregion
    }

}
