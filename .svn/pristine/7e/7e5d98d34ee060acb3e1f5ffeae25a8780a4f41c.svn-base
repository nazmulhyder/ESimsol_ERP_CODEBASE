using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class ExportIncentiveDA
    {
        public ExportIncentiveDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, ExportIncentive oExportIncentive, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_ExportIncentive]"
                                    + "%n,%n,%n,%d, %n,%d,%n,%n, %d,  %d,%n,%d,%n, %n,%n, %n,%b, %s, %n, %n,%n",
                                    oExportIncentive.ExportIncentiveID, oExportIncentive.ExportLCID, oExportIncentive.ExportBillID,   NullHandler.GetNullValue(oExportIncentive.PRCDate),
                                    oExportIncentive.PRCCollectBy, NullHandler.GetNullValue(oExportIncentive.ApplicationDate), oExportIncentive.ApplicationBy, oExportIncentive.BTMAIssueBy,  NullHandler.GetNullValue(oExportIncentive.BTMAIssueDate),
                                     NullHandler.GetNullValue(oExportIncentive.AuditCertDate), oExportIncentive.AuditCertBy,  NullHandler.GetNullValue(oExportIncentive.RealizedDate), oExportIncentive.RealizedBy,
                                    oExportIncentive.Amount_Realized, oExportIncentive.CurrencyID_Real, oExportIncentive.SLNo, oExportIncentive.IsCopyTo, oExportIncentive.Remarks_Application,
                                    //%s,%s,%s,%s,%s,oExportIncentive.Remarks_PRC, oExportIncentive.Remarks_Application, oExportIncentive.Remarks_BTMA, oExportIncentive.Remarks_Audit, oExportIncentive.Remarks_Realized,
                                    oExportIncentive.Percentage_Incentive, (int)eEnumDBOperation, nUserID);
        }

        public static void Delete(TransactionContext tc, ExportIncentive oExportIncentive, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteReader("EXEC [SP_IUD_ExportIncentive]"
                                    + "%n,%n,%n,%d, %n,%d,%n,%n, %d,  %d,%n,%d,%n, %n,%n, %n,%b, %s, %n, %n,%n",
                                    oExportIncentive.ExportIncentiveID, oExportIncentive.ExportLCID, oExportIncentive.ExportBillID, NullHandler.GetNullValue(oExportIncentive.PRCDate),
                                    oExportIncentive.PRCCollectBy, NullHandler.GetNullValue(oExportIncentive.ApplicationDate), oExportIncentive.ApplicationBy, oExportIncentive.BTMAIssueBy, NullHandler.GetNullValue(oExportIncentive.BTMAIssueDate),
                                     NullHandler.GetNullValue(oExportIncentive.AuditCertDate), oExportIncentive.AuditCertBy, NullHandler.GetNullValue(oExportIncentive.RealizedDate), oExportIncentive.RealizedBy,
                                    oExportIncentive.Amount_Realized, oExportIncentive.CurrencyID_Real, oExportIncentive.SLNo, oExportIncentive.IsCopyTo, oExportIncentive.Remarks_Application,
                                    //%s,%s,%s,%s,%s,oExportIncentive.Remarks_PRC, oExportIncentive.Remarks_Application, oExportIncentive.Remarks_BTMA, oExportIncentive.Remarks_Audit, oExportIncentive.Remarks_Realized,
                                    oExportIncentive.Percentage_Incentive, (int)eEnumDBOperation, nUserID);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_ExportLC_Incentive WHERE ExportIncentiveID=%n", nID);
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);//use View_ExportIncentive
        }
        public static IDataReader Update_PRCDate(TransactionContext tc, ExportIncentive oExportIncentive)
        {
            string sSQL1 = SQLParser.MakeSQL("Update View_ExportLC_Incentive Set PRCDate= %d , Remarks_PRC=%s, PRCCollectBy= %n WHERE ExportIncentiveID=%n", NullHandler.GetNullValue(oExportIncentive.PRCDate), oExportIncentive.Remarks_PRC, oExportIncentive.PRCCollectBy, oExportIncentive.ExportIncentiveID);
            tc.ExecuteNonQuery(sSQL1);
            return tc.ExecuteReader("SELECT * FROM View_ExportLC_Incentive WHERE ExportIncentiveID=%n", oExportIncentive.ExportIncentiveID);
        }
        public static IDataReader Update_ApplicationDate(TransactionContext tc, ExportIncentive oExportIncentive)
        {
            string sSQL1 = SQLParser.MakeSQL("Update ExportIncentive Set "
                                            +"     ApplicationDate='" + oExportIncentive.ApplicationDateST 
                                            + "',  Remarks_Application='" + oExportIncentive.Remarks_Application
                                            + "',  Percentage_Incentive=" + oExportIncentive.Percentage_Incentive 
                                            + ",   ApplicationBy=" + oExportIncentive.ApplicationBy 
                                            + "    WHERE ExportIncentiveID=%n", oExportIncentive.ExportIncentiveID);
            tc.ExecuteNonQuery(sSQL1);
            return tc.ExecuteReader("SELECT * FROM View_ExportLC_Incentive WHERE ExportIncentiveID=%n", oExportIncentive.ExportIncentiveID);
        }
        public static IDataReader Update_BTMAIssueDate(TransactionContext tc, ExportIncentive oExportIncentive)
        {
            string sSQL1 = SQLParser.MakeSQL("Update ExportIncentive Set BTMAIssueDate= %d , Remarks_BTMA='" + oExportIncentive.Remarks_BTMA + "', BTMAIssueBy=" + oExportIncentive.BTMAIssueBy + "  WHERE ExportIncentiveID=%n",NullHandler.GetNullValue(oExportIncentive.BTMAIssueDate), oExportIncentive.ExportIncentiveID);
            tc.ExecuteNonQuery(sSQL1);
            return tc.ExecuteReader("SELECT * FROM View_ExportLC_Incentive WHERE ExportIncentiveID=%n", oExportIncentive.ExportIncentiveID);

        }
        public static IDataReader Update_AuditCertDate(TransactionContext tc, ExportIncentive oExportIncentive)
        {
            string sSQL1 = SQLParser.MakeSQL("Update ExportIncentive Set AuditCertDate= %d , Remarks_Audit='" + oExportIncentive.Remarks_Audit + "', AuditCertBy=" + oExportIncentive.AuditCertBy + "  WHERE ExportIncentiveID=%n",NullHandler.GetNullValue(oExportIncentive.AuditCertDate), oExportIncentive.ExportIncentiveID);
            tc.ExecuteNonQuery(sSQL1);
            return tc.ExecuteReader("SELECT * FROM View_ExportLC_Incentive WHERE ExportIncentiveID=%n", oExportIncentive.ExportIncentiveID);

        }
        public static IDataReader Update_RealizedDate(TransactionContext tc, ExportIncentive oExportIncentive)
        {
            string sSQL1 = SQLParser.MakeSQL("Update ExportIncentive Set RealizedDate= %d,  Remarks_Realized='" + oExportIncentive.Remarks_Realized
                                            + "',   CurrencyID_Real='" + oExportIncentive.CurrencyID_Real
                                            + "',   Amount_Realized=" + oExportIncentive.Amount_Realized
                                            + ",   RealizedBy=" + oExportIncentive.RealizedBy + "  WHERE ExportIncentiveID=%n", NullHandler.GetNullValue(oExportIncentive.RealizedDate), oExportIncentive.ExportIncentiveID);
            tc.ExecuteNonQuery(sSQL1);
            return tc.ExecuteReader("SELECT * FROM View_ExportLC_Incentive WHERE ExportIncentiveID=%n", oExportIncentive.ExportIncentiveID);

        }
        #endregion
        public static IDataReader Update_BankSubDate(TransactionContext tc, ExportIncentive oExportIncentive)
        {
            string sSQL1 = SQLParser.MakeSQL("Update ExportIncentive Set BankSubDate=%d "
                                            + ",  Remarks_BankSub='" + oExportIncentive.Remarks_BankSub
                                            + "',   BankSubBy=" + oExportIncentive.BankSubBy + "  WHERE ExportIncentiveID=%n", NullHandler.GetNullValue(oExportIncentive.BankSubDate), oExportIncentive.ExportIncentiveID);
            tc.ExecuteNonQuery(sSQL1);
            return tc.ExecuteReader("SELECT * FROM View_ExportLC_Incentive WHERE ExportIncentiveID=%n", oExportIncentive.ExportIncentiveID);

        }
    }
}
