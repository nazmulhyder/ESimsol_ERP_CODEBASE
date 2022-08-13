using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Framework;
using ICS.Core.Utility;
using ICS.Core.DataAccess;
using System.Net.Mail;

namespace ESimSol.Services.DataAccess
{
    public class HIASetupDA
    {
        public HIASetupDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, HIASetup oHIASetup, EnumDBOperation eEnumDBOperation, Int64 nUserId)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_HIASetup]" + "%n,%n,%n, %s, %s, %s, %s, %s, %s, %s, %s, %b, %s, %s, %n, %n, %n,%s,%s, %n, %n",
                                    oHIASetup.HIASetupID, oHIASetup.BUID, (int)oHIASetup.HIASetupType, oHIASetup.SetupName, oHIASetup.DBTable, oHIASetup.KeyColumn, oHIASetup.FileNumberColumn, oHIASetup.SenderColumn, oHIASetup.ReceiverColumn,  oHIASetup.WhereClause, oHIASetup.MessageBodyText, oHIASetup.Activity, oHIASetup.LinkReference, oHIASetup.Parameter,  oHIASetup.OrderStepID,(int)oHIASetup.TimeEventType,oHIASetup.TimeCounter, oHIASetup.OperationName, oHIASetup.OperationValue, nUserId, (int)eEnumDBOperation);
        }

        public static void Delete(TransactionContext tc, HIASetup oHIASetup, EnumDBOperation eEnumDBOperation, Int64 nUserId)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_HIASetup]" + "%n,%n,%n, %s, %s, %s, %s, %s, %s, %s, %s, %b, %s, %s, %n, %n, %n,%s,%s, %n, %n",
                                    oHIASetup.HIASetupID, oHIASetup.BUID, (int)oHIASetup.HIASetupType, oHIASetup.SetupName, oHIASetup.DBTable, oHIASetup.KeyColumn, oHIASetup.FileNumberColumn, oHIASetup.SenderColumn, oHIASetup.ReceiverColumn, oHIASetup.WhereClause, oHIASetup.MessageBodyText, oHIASetup.Activity, oHIASetup.LinkReference, oHIASetup.Parameter, oHIASetup.OrderStepID, (int)oHIASetup.TimeEventType, oHIASetup.TimeCounter, oHIASetup.OperationName, oHIASetup.OperationValue, nUserId, (int)eEnumDBOperation);
        }
     
        #endregion

        #region Get & Exist Function

        public static int GetHIA_Noty(TransactionContext tc)
        {
            object obj = tc.ExecuteScalar("SELECT COUNT(*) FROM HIASetup where [Status]=1");
            if (obj == null) return 0;
            return Convert.ToInt32(obj);
        }
        public static IDataReader Get(TransactionContext tc, long nid)
        {
            return tc.ExecuteReader("SELECT * FROM HIASetup WHERE HIASetupID=%n", nid);
        }

        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM HIASetup");
        }

        public static IDataReader GetsBy(TransactionContext tc, Int64 nUserID, int buid)
        {
            return tc.ExecuteReader("SELECT * FROM HIASetup  WHERE ISNULL(BUID,0)=%n", buid);
        }
        public static IDataReader GetsByOrderStep(TransactionContext tc, int id,  Int64 nUserID)
        {
            return tc.ExecuteReader("SELECT * FROM HIASetup WHERE OrderStepID = %n", id);//Order Step ID
        }
        public static IDataReader GetsByOrderStepBUWise(TransactionContext tc, int id,   int nBUID)
        {
            return tc.ExecuteReader("SELECT * FROM HIASetup WHERE OrderStepID = %n AND BUID = %n",id, nBUID);//Order Step ID
        }

        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        //internal static void SendEmail(MailAddress fromAddress, MailAddress toAddress, string subject, string body)
        //{
        //    var message = new MailMessage(fromAddress, toAddress)
        //    {
        //        Subject = subject,
        //        Body = body
        //    };
        //    System.Net.NetworkCredential smtpusercredential = new System.Net.NetworkCredential("mamun.khan.g@gmail.com", "***");
        //    var client = new SmtpClient();
        //    client.UseDefaultCredentials = false;
        //    client.Credentials = smtpusercredential;
        //    client.EnableSsl = true;
        //    client.Host = "smtp.gmail.com";
        //    client.Port = 587;
        //    client.Send(message);
          
        //}
        #endregion
    }
}
