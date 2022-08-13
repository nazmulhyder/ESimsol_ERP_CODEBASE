using ESimSol.BusinessObjects;
using ICS.Core.DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace ESimSol.Services.DataAccess
{
    class ELEncashComplianceDA
    {
        public static IDataReader InsertUpdate(TransactionContext tc, ELEncashCompliance oELEncashCompliance, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_ELEncashCompliance] %n, %d, %d, %d, %n, %n, %s, %n, %n,%n",
                   oELEncashCompliance.ELEncashCompID,
                   oELEncashCompliance.DeclarationDate,
                   oELEncashCompliance.StartDate,
                   oELEncashCompliance.EndDate,
                   oELEncashCompliance.BUID,
                   oELEncashCompliance.LocationID,
                   oELEncashCompliance.Note,
                   oELEncashCompliance.ConsiderELCount,
                   nUserID,
                   (int)eEnumDBOperation
                );
        }

        public static void Delete(TransactionContext tc, ELEncashCompliance oELEncashCompliance, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC []");
        }


        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }

        public static int ELEncashComplianceSave(TransactionContext tc, int nIndex, int ELEncashCompID, Int64 nUserID)
        {
            int nNewIndex = 0;
            object oIndex = tc.ExecuteScalar("EXEC [SP_Process_ELEncashCompliance] %n  ,%n  ,%n",
            nIndex, ELEncashCompID, nUserID);
            if (oIndex != null)
            {
                nNewIndex = Convert.ToInt32(oIndex);
            }
            return nNewIndex;
        }

        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_ELEncashCompliance");
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
