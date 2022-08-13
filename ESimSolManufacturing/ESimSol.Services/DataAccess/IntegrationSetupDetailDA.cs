using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class IntegrationSetupDetailDA
    {
        public IntegrationSetupDetailDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, IntegrationSetupDetail oIntegrationSetupDetail, EnumDBOperation eEnumDBOperation, Int64 nUserID, String sIntegrationSetupDetailIDs)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_IntegrationSetupDetail]" + "%n, %n, %n, %s, %s, %s, %s, %s, %s, %s, %n, %n, %s",
                                    oIntegrationSetupDetail.IntegrationSetupDetailID, oIntegrationSetupDetail.IntegrationSetupID, oIntegrationSetupDetail.VoucherTypeID, oIntegrationSetupDetail.BusinessUnitSetup, oIntegrationSetupDetail.VoucherDateSetup, oIntegrationSetupDetail.NarrationSetup, oIntegrationSetupDetail.ReferenceNoteSetup, oIntegrationSetupDetail.UpdateTable, oIntegrationSetupDetail.KeyColumn, oIntegrationSetupDetail.Note, nUserID, (int)eEnumDBOperation, sIntegrationSetupDetailIDs);
        }

        public static void Delete(TransactionContext tc, IntegrationSetupDetail oIntegrationSetupDetail, EnumDBOperation eEnumDBOperation, Int64 nUserID, String sIntegrationSetupDetailIDs)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_IntegrationSetupDetail]" + "%n, %n, %n, %s, %s, %s, %s, %s, %s, %s, %n, %n, %s",
                                    oIntegrationSetupDetail.IntegrationSetupDetailID, oIntegrationSetupDetail.IntegrationSetupID, oIntegrationSetupDetail.VoucherTypeID, oIntegrationSetupDetail.BusinessUnitSetup, oIntegrationSetupDetail.VoucherDateSetup, oIntegrationSetupDetail.NarrationSetup, oIntegrationSetupDetail.ReferenceNoteSetup, oIntegrationSetupDetail.UpdateTable, oIntegrationSetupDetail.KeyColumn, oIntegrationSetupDetail.Note, nUserID, (int)eEnumDBOperation, sIntegrationSetupDetailIDs);
        }


        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_IntegrationSetupDetail WHERE IntegrationSetupDetailID=%n", nID);
        }

        public static IDataReader Gets(TransactionContext tc, int nIntegrationSetupID)
        {
            return tc.ExecuteReader("SELECT * FROM View_IntegrationSetupDetail where IntegrationSetupID =%n ORDER BY IntegrationSetupDetailID ASC", nIntegrationSetupID);
        }

        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion
    }
}
