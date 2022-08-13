using ESimSol.BusinessObjects;
using ICS.Core.DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace ESimSol.Services.DataAccess
{
    class ELSetupDA
    {
        public static IDataReader InsertUpdate(TransactionContext tc, ELSetup oELSetup, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_ELSetup]" + "%n, %b, %b, %b, %b, %n, %d, %n, %d, %n, %n",
                oELSetup.ELSetupID, oELSetup.IsConsiderLeave, oELSetup.@IsConsiderDayOff, oELSetup.IsConsiderHoliday
                , oELSetup.IsConsiderAbsent, oELSetup.InactiveBy, oELSetup.InactiveDate, oELSetup.ApproveBy, oELSetup.ApproveDate
                , nUserID, eEnumDBOperation);
        }

        public static void Delete(TransactionContext tc, ELSetup oELSetup, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_ELSetup]" + "%n, %b, %b, %b, %b, %n, %d, %n, %d, %n, %n",
                oELSetup.ELSetupID, oELSetup.IsConsiderLeave, oELSetup.@IsConsiderDayOff, oELSetup.IsConsiderHoliday
                , oELSetup.IsConsiderAbsent, oELSetup.InactiveBy, oELSetup.InactiveDate, oELSetup.ApproveBy, oELSetup.ApproveDate
                , nUserID, eEnumDBOperation);
        }


        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }


        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_ELSetup");
        }

        public static IDataReader Get(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        //public static IDataReader Get(TransactionContext tc, string sSQL)
        //{
        //    return tc.ExecuteReader(sSQL);
        //}
    }
}
