using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class ExportDocTnCDA
    {
        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, ExportDocTnC oExportDocTnC, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_ExportDocTnC]"
                                    + "%n, %n, %n, %b, %b, %s, %b, %s, %b, %n, %n,%n,%n,%s,%s,%s, %n, %n",
                                    oExportDocTnC.ExportDocTnCID, oExportDocTnC.ExportTRID, oExportDocTnC.ReferenceID, oExportDocTnC.IsPrintGrossNetWeight, oExportDocTnC.IsPrintOriginal, oExportDocTnC.DeliveryBy, oExportDocTnC.IsDeliveryBy, oExportDocTnC.Term,oExportDocTnC.IsTerm, oExportDocTnC.MeasurementCarton, oExportDocTnC.PerCartonWeight, (int)oExportDocTnC.RefType,oExportDocTnC.NotifyByInt,oExportDocTnC.CTPApplicant,oExportDocTnC.Certification,oExportDocTnC.GRPNonDate, (int)eEnumDBOperation, nUserID);
        }
        public static void Delete(TransactionContext tc, ExportDocTnC oExportDocTnC, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_ExportDocTnC]"
                                    + "%n, %n, %n, %b, %b, %s, %b, %s, %b, %n, %n,%n,%n,%s,%s,%s, %n, %n",
                                    oExportDocTnC.ExportDocTnCID, oExportDocTnC.ExportTRID, oExportDocTnC.ReferenceID, oExportDocTnC.IsPrintGrossNetWeight, oExportDocTnC.IsPrintOriginal, oExportDocTnC.DeliveryBy, oExportDocTnC.IsDeliveryBy, oExportDocTnC.Term, oExportDocTnC.IsTerm, oExportDocTnC.MeasurementCarton, oExportDocTnC.PerCartonWeight, (int)oExportDocTnC.RefType, oExportDocTnC.NotifyByInt, oExportDocTnC.CTPApplicant, oExportDocTnC.Certification, oExportDocTnC.GRPNonDate, (int)eEnumDBOperation, nUserID);
        }

        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_ExportDocTnC WHERE ExportDocTnCID=%n", nID);
        }
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_ExportDocTnC");
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        public static IDataReader GetByLCID(TransactionContext tc, long nReferenceID, int nRefType)
        {
            return tc.ExecuteReader("SELECT * FROM View_ExportDocTnC WHERE ReferenceID=%n AND RefType=%n", nReferenceID,nRefType);
        }
        #endregion
    }
}
