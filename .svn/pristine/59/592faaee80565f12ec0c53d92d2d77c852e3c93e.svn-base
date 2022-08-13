using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class ServiceScheduleDA
    {
        public ServiceScheduleDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, ServiceSchedule oServiceSchedule, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_ServiceSchedule]"
                                    + "%n, %n,%n, %D, %n,%s, %n, %n",
                                    oServiceSchedule.ServiceScheduleID, oServiceSchedule.PreInvoiceID, oServiceSchedule.Status, oServiceSchedule.ServiceDate, oServiceSchedule.ChargeType,
                                    oServiceSchedule.Remarks, (int)eEnumDBOperation, nUserID);
        }
        public static void Delete(TransactionContext tc, ServiceSchedule oServiceSchedule, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_ServiceSchedule]"
                                    + "%n, %n,%n, %D, %n,%s, %n, %n",
                                    oServiceSchedule.ServiceScheduleID, oServiceSchedule.PreInvoiceID, oServiceSchedule.Status, oServiceSchedule.ServiceDate, oServiceSchedule.ChargeType,
                                    oServiceSchedule.Remarks, (int)eEnumDBOperation, nUserID);
        }
       
        public static IDataReader ReSchedule(TransactionContext tc, ServiceSchedule oServiceSchedule, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_ServiceReScheduleing]"
                                    + "%n, %n,%d, %n, %n, %n",
                                    oServiceSchedule.ServiceScheduleID, oServiceSchedule.PreInvoiceID, oServiceSchedule.DoneDate, oServiceSchedule.ServiceInterval, oServiceSchedule.ServiceDurationInMonth,nUserID);
        }
        public static void DeleteList(TransactionContext tc, string sServiceScheduleIDs, int nPreInvoiceID, Int64 nUserID)
        {
            string sSQL = "DELETE FROM ServiceSchedule WHERE PreInvoiceID = " + nPreInvoiceID;
            if (sServiceScheduleIDs.Length > 0)
            {
                sSQL = sSQL + " AND ServiceScheduleID NOT IN (" + sServiceScheduleIDs + ")";
            }
            tc.ExecuteNonQuery(sSQL);
        }
        public static void Done(TransactionContext tc, ServiceSchedule oServiceSchedule)
        {
            tc.ExecuteNonQuery("Update ServiceSchedule SET IsDone = %b, DoneDate =%d, Status = %n WHERE ServiceScheduleID=%n", true, oServiceSchedule.DoneDate, (int)EnumServiceScheduleStatus.Done,  oServiceSchedule.ServiceScheduleID);
           
        }
        public static void SMSSend(TransactionContext tc, ServiceSchedule oServiceSchedule)
        {
            tc.ExecuteNonQuery("Update ServiceSchedule SET IsSMSSend = %b  WHERE ServiceScheduleID=%n", true, oServiceSchedule.ServiceScheduleID);
        }

        public static void PhoneCall(TransactionContext tc, ServiceSchedule oServiceSchedule)
        {
            tc.ExecuteNonQuery("Update ServiceSchedule SET IsPhoneCall = %b  WHERE ServiceScheduleID=%n", true, oServiceSchedule.ServiceScheduleID);
        }
        public static void EmailSend(TransactionContext tc, ServiceSchedule oServiceSchedule)
        {
            tc.ExecuteNonQuery("Update ServiceSchedule SET IsEmailSend = %b, EmailBody=%s  WHERE ServiceScheduleID=%n", true, oServiceSchedule.EmailBody,  oServiceSchedule.ServiceScheduleID);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_ServiceSchedule WHERE ServiceScheduleID=%n", nID);
        }
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_ServiceSchedule ");
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);//use View_ServiceSchedule
        }
        #endregion
    }
}
