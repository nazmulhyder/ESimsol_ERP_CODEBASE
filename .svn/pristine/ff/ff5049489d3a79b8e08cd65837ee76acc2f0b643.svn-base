using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using ESimSol.BusinessObjects;
using ESimSol.Services.DataAccess;
using ICS.Core.DataAccess;
using ICS.Core.Utility;
using ICS.Core.Framework;

namespace ESimSol.Services.DataAccess
{
    public class ExportBillParticularDA
    {
        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, ExportBillParticular oExportBillParticular, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_ExportBillParticular]"
                                    + "%n, %s, %n, %b, %n, %n",
                                    oExportBillParticular.ExportBillParticularID, oExportBillParticular.Name, oExportBillParticular.InOutType, oExportBillParticular.Activity,(int)eEnumDBOperation, nUserID);
        }

        public static void Delete(TransactionContext tc, ExportBillParticular oExportBillParticular, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_ExportBillParticular]"
                                    + "%n, %s, %n, %b, %n, %n",
                                    oExportBillParticular.ExportBillParticularID, oExportBillParticular.Name, oExportBillParticular.InOutType, oExportBillParticular.Activity, (int)eEnumDBOperation, nUserID);
        }
        #endregion
        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM ExportBillParticular WHERE ExportBillParticularID=%n", nID);
        }
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM ExportBillParticular");
        }
        public static IDataReader Gets(TransactionContext tc,bool bActivity)
        {
            return tc.ExecuteReader("SELECT * FROM ExportBillParticular where Activity=%b ", bActivity);
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion
    }
}
