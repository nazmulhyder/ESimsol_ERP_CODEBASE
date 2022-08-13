using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;


namespace ESimSol.Services.DataAccess
{
    public class TextileSubUnitMachineDA
    {
        public TextileSubUnitMachineDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, TextileSubUnitMachine oTextileSubUnitMachine, int eEnumDBOperation, Int64 nUserId)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_TextileSubUnitMachine]"
                                    + "%n, %n, %n,  %s, %n, %n",
                                    oTextileSubUnitMachine.TSUMachineID
                                    ,oTextileSubUnitMachine.TSUID
                                    ,oTextileSubUnitMachine.FMID
                                    ,oTextileSubUnitMachine.Note
                                    ,(int)eEnumDBOperation
                                    ,nUserId);
        }

        public static void Delete(TransactionContext tc, TextileSubUnitMachine oTextileSubUnitMachine, int eEnumDBOperation, Int64 nUserId)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_TextileSubUnitMachine]"
                                    + "%n, %n, %n,  %s, %n, %n",
                                     oTextileSubUnitMachine.TSUMachineID
                                    , oTextileSubUnitMachine.TSUID
                                    , oTextileSubUnitMachine.FMID
                                    , oTextileSubUnitMachine.Note
                                    , (int)eEnumDBOperation
                                    , nUserId);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_TextileSubUnitMachine WHERE TSUMachineID=%n", nID);
        }
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_TextileSubUnitMachine");
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion
    }
}
