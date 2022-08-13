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
    public class ExportPIShipmentDA
    {
        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, ExportPIShipment oExportPIShipment, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_ExportPIShipment]"
                                   + "%n,%n,%n,%s,%s,%n,%n",
                                   oExportPIShipment.ExportPIShipmentID,
                                   oExportPIShipment.ExportPIID,
                                   oExportPIShipment.ExportBillID,
                                   oExportPIShipment.ShipmentBy,
                                   oExportPIShipment.DestinationPort,
                                   nUserID,
                                   (int)eEnumDBOperation);
        }

        public static void Delete(TransactionContext tc, ExportPIShipment oExportPIShipment, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_ExportPIShipment]"
                                   + "%n,%n,%n,%s,%s,%n,%n",
                                   oExportPIShipment.ExportPIShipmentID,
                                   oExportPIShipment.ExportPIID,
                                   oExportPIShipment.ExportBillID,
                                   oExportPIShipment.ShipmentBy,
                                   oExportPIShipment.DestinationPort,
                                   nUserID,
                                   (int)eEnumDBOperation);
        }

        #endregion
        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_ExportPIShipment WHERE ExportPIShipmentID=%n", nID);
        }
        public static IDataReader GetByExportPIID(TransactionContext tc, long ExportPIID)
        {
            return tc.ExecuteReader("SELECT * FROM View_ExportPIShipment WHERE ExportPIID=%n", ExportPIID);
        }
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_ExportPIShipment");
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion
    }
}
