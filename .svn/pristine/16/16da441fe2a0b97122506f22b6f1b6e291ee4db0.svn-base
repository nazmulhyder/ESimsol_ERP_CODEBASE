using System;
using System.Data;
using System.Data.SqlClient;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.DataAccess;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class ExportPILCMappingDA
    {
        public ExportPILCMappingDA() { }

        #region Insert Function
        public static IDataReader InsertUpdate(TransactionContext tc, ExportPILCMapping oExportPILCMapping, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_ExportPILCMapping]"
                                    + "%n, %n, %n, %b, %D,%D, %n, %s, %n, %n",
                                    oExportPILCMapping.ExportPILCMappingID, oExportPILCMapping.ExportPIID, oExportPILCMapping.ExportLCID, oExportPILCMapping.Activity, oExportPILCMapping.Date, oExportPILCMapping.LCReceiveDate, oExportPILCMapping.Amount, oExportPILCMapping.ReviseNo, (int)eEnumDBOperation, nUserID);
        }
        public static IDataReader InsertUpdateLog(TransactionContext tc, ExportPILCMapping oExportPILCMapping, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_ExportPILCMappingLog]"
                                    + "%n, %n, %n, %b, %d, %n, %s, %n, %n",
                                    oExportPILCMapping.ExportPILCMappingLogID, oExportPILCMapping.ExportPIID, oExportPILCMapping.ExportLCID, oExportPILCMapping.Activity, oExportPILCMapping.Date, oExportPILCMapping.Amount, oExportPILCMapping.ReviseNo, (int)eEnumDBOperation, nUserID);
        }
        #endregion

        public static void Delete(TransactionContext tc, ExportPILCMapping oExportPILCMapping, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_ExportPILCMapping]"
                                    + "%n, %n, %n, %b, %d, %d, %n, %s, %n, %n",
                                    oExportPILCMapping.ExportPILCMappingID, oExportPILCMapping.ExportPIID, oExportPILCMapping.ExportLCID, oExportPILCMapping.Activity, oExportPILCMapping.Date, oExportPILCMapping.LCReceiveDate, oExportPILCMapping.Amount, oExportPILCMapping.ReviseNo, (int)eEnumDBOperation, nUserID);
        }
        public static void DeleteLog(TransactionContext tc, ExportPILCMapping oExportPILCMapping, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_ExportPILCMappingLog]"
                                    + "%n, %n, %n, %b, %d, %n, %s, %n, %n",
                                    oExportPILCMapping.ExportPILCMappingLogID, oExportPILCMapping.ExportPIID, oExportPILCMapping.ExportLCID, oExportPILCMapping.Activity, oExportPILCMapping.Date, oExportPILCMapping.Amount, oExportPILCMapping.ReviseNo, (int)eEnumDBOperation, nUserID);
        }

        public static void Update(TransactionContext tc, ExportPILCMapping oExportPILCMapping, Int64 nUserID)
        {
            tc.ExecuteNonQuery("UPDATE PILCMapping SET ShipmentDate=%D   WHERE ExportPILCMappingID=%n", oExportPILCMapping.IssueDate, oExportPILCMapping.ExportPILCMappingID);
        }

        public static void UpdateUDInfo(TransactionContext tc, ExportPILCMapping oEPILCM, Int64 nUserID)
        {
            tc.ExecuteNonQuery("UPDATE ExportPILCMapping SET UDRecDate=%d, UDRcvType=%n, VersionNo=%n  WHERE ExportPILCMappingID=%n", NullHandler.GetNullValue(oEPILCM.UDRecDate), oEPILCM.UDRcvType, oEPILCM.VersionNo, oEPILCM.ExportPILCMappingID);
        }

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, int nID)
        {
            return tc.ExecuteReader("Select * From View_ExportPILCMapping Where Activity=1 and ExportPILCMappingID=%n", nID);
        }

        public static IDataReader Gets(TransactionContext tc, int nExportLCID, int nVersionNo)
        {
            return tc.ExecuteReader("Select * From View_ExportPILCMapping Where  ExportLCID=%n and VersionNo=%n",  nExportLCID,  nVersionNo);
        }
        public static IDataReader GetsByLCID(TransactionContext tc, int nExportLCID)
        {
            return tc.ExecuteReader("Select * From View_ExportPILCMapping Where Activity=1 AND ExportLCID=%n", nExportLCID);
        }
        public static IDataReader GetsLogByLCID(TransactionContext tc, int nExportLCLogID)
        {
            return tc.ExecuteReader("Select * From View_ExportPILCMappingLog Where Activity=1 AND ExportLCLogID=%n", nExportLCLogID);
        }
        public static IDataReader GetsByEBillID(TransactionContext tc, int nExportBillID)
        {
            return tc.ExecuteReader("Select * From View_ExportPILCMapping Where Activity=1 AND   ExportPIID in (Select ExportPIID from ExportPIDetail where ExportPIDetailID IN (Select ExportBillDetail.ExportPIDetailID From ExportBillDetail Where ExportBillID=%n))", nExportBillID);
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
     
        public static IDataReader UpdateExportPILCMapping(TransactionContext tc, ExportPILCMapping oExportPILCMapping, Int64 nUserID)
        {
            tc.ExecuteNonQuery("Update ExportPILCMapping SET LCReceiveDate = %d,ReportDate=%d,Date = %d,ShipmentDate = %d,ExpiryDate = %d,LastUpdateBy=%n,LastUpdateDateTime=getdate() WHERE ExportPILCMappingID = %n", oExportPILCMapping.LCReceiveDate, oExportPILCMapping.ReportDate, oExportPILCMapping.Date, oExportPILCMapping.ShipmentDate, oExportPILCMapping.ExpiryDate, nUserID, oExportPILCMapping.ExportPILCMappingID);
            return tc.ExecuteReader("SELECT * FROM View_ExportPILCMapping WHERE ExportPILCMappingID=%n", oExportPILCMapping.ExportPILCMappingID);
        }
        #endregion
    }
}